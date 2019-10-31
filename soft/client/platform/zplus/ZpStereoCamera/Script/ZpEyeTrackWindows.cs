#if !UNITY_IOS
using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Text.RegularExpressions;
using ZpGame;

public class ZpEyeTrackWindows : ZpMonoUnitySingleton<ZpEyeTrackWindows>
{
	#region EMUM_STATE

	enum ETServiceState
	{
		TOSTOP = 1 << 0,
		STOPED = 1 << 1,
		STOP = TOSTOP | STOPED,
		TORUN = 1 << 2,
		RUNED = 1 << 3,
		RUN = TORUN | RUNED,
		OTHERS = 0,
	}

	enum AppScreenState
	{
		FOCUS,
		NONFOCUS,
		FULLSCREEN,
		NONFULLSCREEN,
	}

	#endregion

	bool SupportETAndroid = false;
	bool SupportETWindows = false;
	bool AndroidServerStart = false;
	bool HasTcpLink = false;
	bool IsEyeTracking = false;
	//能获取到人眼跟踪数据
	TcpClient client;
	readonly object lockThis = new object ();
	int ReadbytesInCB = 0;
	float[] res = new float[20];
	ETServiceState ETState = ETServiceState.OTHERS;
	//Eye track State
	ETServiceState FTState = ETServiceState.OTHERS;
	//Screen Film State
	AppScreenState AppState;
	private bool isAndroidServiceRunning = false;
	private volatile Boolean CameraStateOk = true;
	//默认摄像机正常
	private volatile Boolean OuterDiaphragm = true;
	//是否外挂膜片   默认挂起
	private volatile int screenType = (int)ZpGameContant.AEffects.Original;
	AutoResetEvent wait = new AutoResetEvent (false);

	int RGB_Mode;

	public bool IsSupportEyeTrack {
		get {
			if (isStraight ()) {//获得手机屏幕类型为直排
				return false;
			}
			return SupportETAndroid || SupportETWindows; 
		}
	}

	private bool isStraight ()
	{
		//Debugger.Log ("isStraight:" + (screenType));
		return screenType == (int)ZpGameContant.AEffects.LRWeaved || screenType == (int)ZpGameContant.AEffects.RLWeaved;
	}

	public bool Is3DOn {
		get { return IsEyeTracking; }
	}

	public void Begin ()
	{
		Debugger.Log ("Eye Track Begin");
	}

	public ZpEyeTrackWindows()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        CheckAndroidSupportET();
		updateState();
#endif
#if UNITY_EDITOR
		UnityEditor.EditorApplication.playmodeStateChanged = HandleOnPlayModeChanged;
#endif
		startTaskForEt ();

	}

	private bool CheckAndroidSupportET ()
	{
		SupportETAndroid = ZpGameAndroidApi.checkeEeserverInstalled ();
		return SupportETAndroid;
	}

	private bool taskEnd = false;
	private bool onApplicationPause = false;
	private volatile static int heartCount = 0;
	//用来计数 发送回环消息
	private void startTaskForEt ()
	{
		ThreadPool.QueueUserWorkItem (new WaitCallback (MainTask));

	}

	private AutoResetEvent serviceStartEvent = new AutoResetEvent (false);
	//控制android service的启动
	private AutoResetEvent serviceStopEvent = new AutoResetEvent (false);
	//控制android service的关闭

	private AutoResetEvent applicationStateEvent = new AutoResetEvent (false);
	private static bool hasCheckedSupportETWindows = false;
	private bool isEditorApplicationQuit = false;
	private volatile bool validate_timer = false;
	NetworkStream networkStream = null;
	private AndroidServiceState serviceState = AndroidServiceState.STOPED;

	/**
     * 主要任务：1、检查服务以及tcp链接状态 2、开启读写任务
     * */
	private void MainTask (object stateInfo)
	{
		try {
#if UNITY_EDITOR
			while (!isEditorApplicationQuit)
#else
		while (true)
#endif
            {
				while (onApplicationPause) {//游戏暂停 停止人眼跟踪
					//Debugger.Log("MainTask wait");
					invokeTaskEnd ();
					closeConnect ();
					//Debugger.Log ("MainTask Wait onApplication change.................................");
					applicationStateEvent.WaitOne ();
				}
                //Debugger.Log("MainTask onApplicationPause" + onApplicationPause);  
#if UNITY_ANDROID && !UNITY_EDITOR
                if (!SupportETAndroid)
                {
//Debugger.Log("MainTask !SupportETAndroid");
                    return;
                }
                if (!CameraStateOk)//camera 有问题，停止所有线程，关闭服务，稍后重启
                {
                    CameraStateOk = true;
                    invokeTaskEnd();
                    closeConnect();
                    serviceState = AndroidServiceState.TOSTOP;
                    Loom.QueueOnMainThread(() =>
                    {
                         AndroidSdkEye.stopEeserver();
                    });
                   
                    while (isAndroidServiceRunning&&!onApplicationPause)
                    {
//Debugger.Log("MainTask Wait android service stop.................................");
                        serviceStopEvent.WaitOne();
                    }
                    if (onApplicationPause)
                    {
                        continue;
                    }
                }
                if (!isAndroidServiceRunning)//服务没有开启 到主线程开启服务      
                {
                   
                    invokeTaskEnd();
                    closeConnect();
                    serviceState = AndroidServiceState.TOOPEN;
                    Loom.QueueOnMainThread(() =>
                    {
                        StartAndroidService();
                    });
                    while (!isAndroidServiceRunning&& !onApplicationPause)
                    {
                        log("MainTask Wait android service start.................................");
                        serviceStartEvent.WaitOne();
                    }
                    if (onApplicationPause)
                    {
                        continue;
                    }
//Debugger.Log("MainTask android 服务启动");
                    //validate_timer = true;
                }
#else
                if (!hasCheckedSupportETWindows) {//没有检查过edit下安装人眼跟踪服务
					hasCheckedSupportETWindows = true;
					for (int Try = 0; Try < 3; Try++) {
						networkStream = checkedSupportETWindows ();
						SupportETWindows = (networkStream != null);
						if (SupportETWindows) {
							//validate_timer = true;
							break;
						}
					}
					closeConnect ();
				}
				if (!SupportETWindows) {
					return;
				}
#endif
				//Debugger.Log("MainTask running..................OuterDiaphragm："+ OuterDiaphragm);
				if (!OuterDiaphragm || isStraight ()) {//检查到没有膜片不支持斜排  或者本身就是直排手机 
					invokeTaskEnd ();
					closeConnect ();
#if UNITY_ANDROID && !UNITY_EDITOR
                     Loom.QueueOnMainThread(() =>
                    {
//Debugger.Log("MainTask 检查到没有膜片 ..................");
                         AndroidSdkEye.stopEeserver();
                    });
#endif
					if (isStraight ()) {
						Loom.QueueOnMainThread (() => {
                            log("MainTask 停止 加载直排 ..................");
							ZpGameSceneSet.Instance.reload ();
                            ZpGameSceneSet.Instance.ZpGamePostRenderReload();

                        });
					}
					Thread.CurrentThread.Abort ();
					return;
				}

				//Debugger.Log("MainTask !HasTcpLink" + (!HasTcpLink));
				if (!HasTcpLink) {//tcp断开状态，发起连接
					networkStream = GetTCPConnectStream ();
					if (networkStream != null) {//连接成功开启读写线程
						//连接上了 开始计时
						heartCount = 0;
						validate_timer = true;
						startWorkTask (networkStream);
					}
				}
				Thread.Sleep (100);
			}
		} catch (ThreadAbortException e) {
            log("Task--Main----stop");
		} catch (Exception e) {
			//Debugger.Log("MainTask------1"+ e.Message);
		} finally {
			closeConnect ();
		}

        log("Task--Main----stop");
	}

	private void invokeTaskEnd ()
	{
		taskEnd = true;
		writeEvent.Reset ();
	}

	private void startWorkTask (NetworkStream networkStream)
	{
		taskEnd = false;
		SendClientType ();
		OpenEyeTrace ();
		//SendPlatformRequest();
		for (int i = 0; i < 5; i++) {
			SentRGBModeRequest ();//
            //OpenEyeTrace();
            Thread.Sleep (100);
		}
		ThreadPool.QueueUserWorkItem (new WaitCallback (ReadMessageTask), networkStream);
		ThreadPool.QueueUserWorkItem (new WaitCallback (WriteMessageTask), networkStream);
		//ThreadPool.QueueUserWorkItem(new WaitCallback(HandleMessageTask));
	}

	private void closeConnect ()
	{
		try {
			if (networkStream != null) {
				writeCommond (networkStream, Command_ft_close);
				networkStream.Close ();
				networkStream = null;
			}
			if (client != null) {
				client.Close ();
			}
		} catch (Exception e) {
			//Debugger.Log("_____closeConnect:" + e.Message);
		} finally {
			HasTcpLink = false;
		}


	}
	//private volatile bool tcp
	int testcount = 0;

	IEnumerator TimerTask (int time)
	{
		while (true) {
			yield return new WaitForSeconds (time);
			try {
				if (validate_timer) {
					// Debugger.Log("计时--------------------------------------------------------count=" + heartCount);
					heartCount++;
					testcount++;
					//if (testcount == 15)
					//{
					SendPlatformRequest ();
					//}

					if (heartCount % 4 == 0) {
						SendLoopRequest ();
					}
					bool restart = false;
#if UNITY_ANDROID && !UNITY_EDITOR
            restart = (heartCount > 20 && !onApplicationPause);
#else
					restart = (heartCount > 15 && !isEditorApplicationQuit);
#endif
					if (restart) {//很久没收到消息了
						invokeTaskEnd ();
						closeConnect ();
						IsEyeTracking = false;
						applicationStateEvent.Set ();
						validate_timer = false;
						//Debug.Log("long time no msg--------------------------------------------------------");
					}
				}
			} catch (Exception e) {
				// Debugger.Log("_____TimerTask" + e.Message);
			}
		}
	}

	/**
    * 读取消息任务 读取到存入msgQueue 并通知处理线程
    * */
	private void ReadMessageTask (object networkStream)
	{
		if (networkStream == null) {
			return;
		}
		NetworkStream stream = (NetworkStream)networkStream;

		//Debugger.Log ("ReadMessageTask start read&&&");
		while (!taskEnd) {
			try {
				if (stream != null && stream.CanRead) {
					ReadbytesInCB = stream.Read (buffer, 0, 500);//！！！消息可能没都全
					string jsonmsg = System.Text.Encoding.ASCII.GetString (buffer, 0, ReadbytesInCB);
                    log("ReadMessageTask get::::::::::" + jsonmsg);
					analyzeMsg (jsonmsg);
				}
			} catch (Exception e) {
				//Debugger.Log ("_____ReadMessageTask:" + e.Message);
			}

		}
		//Debugger.Log ("Task-----ReadMessage--- stop");
	}

	static string str = "";
	//整条消息
	private void analyzeMsg (string jsonmsg)
	{
        if (!string.IsNullOrEmpty (jsonmsg)) {
			jsonmsg = jsonmsg.Replace (" ", "");
			jsonmsg = jsonmsg.Replace ("\n", "");
		}
        if (string.IsNullOrEmpty (jsonmsg)) {
			str = "";
            return;
		}


		if (!string.IsNullOrEmpty (str) && str.StartsWith ("{") && !LastIndexOfJudge (str, "}") && !jsonmsg.StartsWith ("{")) { //如果str不为空 则说明有msg待拼接
			str += jsonmsg;
			//Debugger.LogWarning("ReadMessageTask success 拼接得到" + str);
		} else {
			//Debugger.LogWarning("ReadMessageTask success 无需拼接" + jsonmsg);
			str = "";//一旦读取的msg以{开头，清除待拼接的str
			str += jsonmsg;
		}
		str = Regex.Replace (str, @"[\n\r\t]", "");
        if (string.IsNullOrEmpty (str)) {
            return;
		}

		if (str.Contains ("}{")) {//多条消息
            while (str.Contains ("}{")) {
				str = str.Replace ("}{", "}|{");
			}
			string[] msgs = str.Split ('|');
			int length = msgs.Length;
			for (int i = 0; i < length; i++) {
				string msg = msgs [i];
				if (msg.StartsWith ("{") && LastIndexOfJudge (str, "}")) {//消息完整
					//Debugger.Log("ReadMessageTask 拆分得到  单条有效 realMsg_____________" + msg);
					ParseJsonMsg (msg);
				}
				if (i == length - 1) {//分析拆分的最后一条
					if (msg.StartsWith ("{") && !LastIndexOfJudge (str, "}")) {//格式如{.... 可用于后续拼接//不完整消息
						str = msg;
						// Debugger.Log("ReadMessageTask 拆分得到  单条无效 realMsg_____________" + msg);
					}
				}

			}
		} else {//单条消息
            if (str.StartsWith ("{") && LastIndexOfJudge (str, "}")) {//消息完整
				//Debugger.Log("ReadMessageTask 单条有效 realMsg_____________" + str);
				ParseJsonMsg (str);
				str = "";
            }
           
		}
	}

	private bool LastIndexOfJudge (string str, string endStr)
	{
		if (string.IsNullOrEmpty (str)) {
			return false;
		}
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (str.LastIndexOf (endStr) == str.Length - 1 - endStr.Length) {//unity edit 平台下。。。//判断结尾字符
			return true;
		}
#else
        return str.EndsWith(endStr);

#endif
		return false;
	}

	/**
    * 写消息任务
    * */
	private Queue commandQueue = new Queue ();
	private AutoResetEvent writeEvent = new AutoResetEvent (false);
	//控制读操作
	private void WriteMessageTask (object networkStream)
	{
		if (networkStream == null) {
			return;
		}
		NetworkStream stream = (NetworkStream)networkStream;
		//Debugger.Log ("WriteMessageTask tcp start write &&&");
		while (!taskEnd) {
			try {
				while (commandQueue.Count <= 0 && !taskEnd) {
					writeEvent.WaitOne ();
				}
				if (taskEnd) {
					commandQueue.Clear ();
				}
				if (commandQueue.Count > 0) {
					string jsonText = (string)commandQueue.Dequeue ();//获取一条命令 写入tcp
					try {
						writeCommond (stream, jsonText);
					} catch (Exception e) {
						 //Debugger.Log("_____WriteMessageTask1:" + e.Message);
					}
				}
			} catch (Exception e) {
				//Debugger.Log("_____WriteMessageTask2:" + e.Message);
			}

		}
		//Debugger.Log ("Task-----WriteMessage---- stop");
	}

	private void writeCommond (NetworkStream stream, string jsonText)
	{
		Boolean ret = false;
		int count = 0;
		while (!ret) {
			byte[] responseBuffer = System.Text.Encoding.ASCII.GetBytes (jsonText);
			if (stream != null && stream.CanWrite) {
				stream.Write (responseBuffer, 0, responseBuffer.Length);
				stream.Flush ();
				ret = true;
				Debugger.LogWarning("WriteMessageTask success " + jsonText);
			}
			if (count++ > 2) {//写两次 不成功判定失败
				Debugger.LogWarning("WriteMessageTask fail " + jsonText);
				break;
			}
		}
	}

	private void addCommand (string command)
	{
		commandQueue.Enqueue (command);
		writeEvent.Set ();
	}

	private NetworkStream GetTCPConnectStream (TcpClient client)
	{
		//TcpClient client = new TcpClient();
		client.ReceiveTimeout = 200;
		client.SendTimeout = 200;

		for (int Cp = 0; Cp < 3; Cp++) {
			for (int i = 0; i < 3; i++) {
				try {
					client.Connect (System.Net.IPAddress.Parse ("127.0.0.1"), ConnectPort [Cp]);//192.168.2.68
				} catch {
					Debugger.Log ("connect error：" + ConnectPort [Cp]);
					HasTcpLink = false;
					continue;
				}
				HasTcpLink = true;
				Debugger.Log ("connect ok：" + ConnectPort [Cp]);
				break;
			}
			if (HasTcpLink)
				return client.GetStream ();
		}
		return null;
	}

	private void StartAndroidService (object o = null)
	{
		AndroidSdkEye.startOrbindEEServerService ();
	}

	NetworkStream checkedSupportETWindows ()
	{
		NetworkStream clientStream = GetTCPConnectStream ();
		if (clientStream != null) {
			Debugger.Log ("clientStream !=null");
			return clientStream;
		}
		return null;
	}

	private void OnApplicationPause (bool pause)
	{
		this.onApplicationPause = pause;
        if (SupportETAndroid || SupportETWindows)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            isAndroidServiceRunning = AndroidSdkEye.isEEServerServiceRunningAndBinding ();
#endif
        }
            
		//Debugger.Log ("OnApplicationPause " + pause);
		applicationStateEvent.Set ();
		serviceStartEvent.Set ();
		serviceStopEvent.Set ();
		if (pause) {//游戏暂停
			writeEvent.Set ();
			//handEvent.Set();
		}
		if (SupportETAndroid || SupportETWindows) {
			if (pause) {
				CloseEyeTrace ();
			}
		}
	}

	private bool onapplicationfocus = false;

	private void Onapplicationfocus (bool focus)
	{
		onapplicationfocus = focus;
		//isandroidservicerunning = androidsdkeye.iseeserverservicerunningandbinding();
		//Debugger.Log ("onapplicationfocus " + focus);
		//if (onapplicationfocus)
		//{
		//    applicationstateevent.set();
		//}


	}

	public void updateState ()
	{
		//Debugger.Log ("EyeTrack updateState");
		screenType = (int)ZpGameAndroidApi.getScreenType ();
	}
	private bool needCheckState=true;
	private void FixedUpdate ()
	{
		if (SupportETAndroid || SupportETWindows) {
#if UNITY_ANDROID && !UNITY_EDITOR
			isAndroidServiceRunning = AndroidSdkEye.isEEServerServiceRunningAndBinding();
            log("isAndroidServiceRunning:"+isAndroidServiceRunning);
            if (serviceState == AndroidServiceState.TOOPEN)
            {
                if (isAndroidServiceRunning)
                {
                    serviceState = AndroidServiceState.RUNNING;
                    serviceStartEvent.Set();
                }
            }
            if (serviceState == AndroidServiceState.TOSTOP)
            {
                
                if (!isAndroidServiceRunning)
                {
                    serviceState = AndroidServiceState.STOPED;
                    serviceStopEvent.Set();
                }
            }

#endif
        }
    }

	private void Awake ()
	{
		StartCoroutine (TimerTask (1));//开始计时 定时发送心跳消息
	}

	public override void OnDestroy ()
	{
		if (SupportETAndroid || SupportETWindows) {
			CloseEyeTrace ();
			StopCoroutine (TimerTask (1));
			invokeTaskEnd ();
		}

	}

	private void HandleOnPlayModeChanged ()
	{
#if UNITY_EDITOR
		if (UnityEditor.EditorApplication.isPaused) {
			FocusOut ();
		} else {
			FocusOn ();
		}
		if (!UnityEditor.EditorApplication.isPlaying) {
			//msgQueue.Clear();
			commandQueue.Clear ();
			invokeTaskEnd ();
			isEditorApplicationQuit = true;
			CloseEyeTrace ();
			//handEvent.Set();
		} else {
			isEditorApplicationQuit = false;
		}
#endif
	}

	public float[] getEyetrackData ()
	{
		float[] tmp = new float[20];
		lock (lockThis) {
			tmp = res;
		}
		return tmp;
	}

	public void FocusOn ()
	{
		AppState = AppScreenState.FOCUS;
		SendFTRequest (true);
	}

	public void FocusOut ()
	{
		AppState = AppScreenState.NONFOCUS;
		SendFTRequest (false);
	}

	public void EyeTracePause ()
	{
		if (AppState == AppScreenState.FOCUS) {
			SendFTRequest (false);
		}

	}

	public void EyeTraceResume ()
	{
		if (AppState == AppScreenState.FOCUS) {
			SendFTRequest (true);
		}
	}

	public void OpenEyeTrace ()
	{
		SendETRequest (true);
		SendFTRequest (true);
	}

	public void CloseEyeTrace ()
	{
		SendETRequest (false);
		SendFTRequest (false);
	}

	public class ResultData
	{
		public int CLIENT_TYPE;
		public int SERVER_MSG;
		//public int FACE_NUM;
		public string SLANT;
		public string PITCH;
		public string CTVIEW;
		public string CAMERAX;
		public string CAMERAY;
		public int FACE_NUM;

		public int ON_OFF_3D;
		public int ON_OFF_OPTION;
		public int ACTION_STATUS;
	}

	private void ParseJsonMsg (string jsonstr)
	{
		if (!string.IsNullOrEmpty (jsonstr)) {
			jsonstr = jsonstr.Replace (" ", "");
			jsonstr = jsonstr.Replace ("\n", "");
			jsonstr = jsonstr.Replace ("'", "");
		}

		if (string.IsNullOrEmpty (jsonstr)) {
			return;
		}
        log("ParseJsonMsg  " + jsonstr);
		try {
			ResultData data = new ResultData ();
#if UNITY_EDITOR
		string jsonstrsub = jsonstr.Substring (0, jsonstr.Length - 1) + " ";
#elif UNITY_ANDROID && !UNITY_EDITOR
        string jsonstrsub = jsonstr;
#elif UNITY_STANDALONE_WIN
        string jsonstrsub = jsonstr.Substring (0, jsonstr.Length - 1) + " ";
#endif
            JsonData json = null;
			try {
				json = JsonMapper.ToObject (jsonstrsub);
			} catch (Exception e) {
				//Debugger.Log("ParseJsonMsg  " + jsonstr);
				//Debugger.Log ("_____ParseJsonMsg1:" + e.Message);
			}
			if (json == null) {
				return;
			}

			if (((IDictionary)json).Contains ("CLIENT_TYPE")) {
				data.CLIENT_TYPE = (int)json ["CLIENT_TYPE"];
			}

			if (((IDictionary)json).Contains ("SERVER_MSG")) {
				data.SERVER_MSG = (int)json ["SERVER_MSG"];
			}

			switch (data.SERVER_MSG) {
			case 7:
				DisposeBroadcastEyetrack (json, data);
				break;
			case 4:
				DisposeClientType (json, data);
				break;
			case 28:
				DisposeETRequest (json);
				break;
			case 30:
				DisposeFTRequest (json);
				break;
			case 1:
				DisposeClientTypeRequest ();
				break;
			case 5:
				DisposeLoopRequest ();
				break;
			case 44:
				DisposeETStateRequest (json);
				break;
			case 62:
				DisposeRGBModeRequest (json, data);
				break;
			case 82:
				DisposePlatformResponse (json);
				break;
			//case 6: 
			}
		} catch (Exception e) {
			Debug.Log ("_____ParseJsonMsg:" + e.Message);
		}

	}

	byte[] buffer = new byte[1000];
	int[] ConnectPort = { 9647, 6143, 25938 };

	private NetworkStream GetTCPConnectStream ()
	{
		client = new TcpClient ();
		client.ReceiveTimeout = 200;
		client.SendTimeout = 200;

		for (int Cp = 0; Cp < 3; Cp++) {
			for (int i = 0; i < 3; i++) {
				try {
					client.Connect (System.Net.IPAddress.Parse ("127.0.0.1"), ConnectPort [Cp]);//192.168.2.68
					//同步方法，连接成功、抛出异常、服务器不存在等之前程序会被阻塞  
				} catch {
					Debug.Log ("connect error：" + ConnectPort [Cp]);
					HasTcpLink = false;
					continue;
				}
				HasTcpLink = true;
                Debug.Log ("connect ok：" + ConnectPort [Cp]);
				break;
			}

			if (HasTcpLink)
				return client.GetStream ();
		}

		return null;
	}


	//msg:1
	void DisposeClientTypeRequest ()
	{
		SendClientTypeRequest ();
	}

	//msg:2
	void SendClientTypeRequest ()
	{
		addCommand ("{\"CLIENT_TYPE\":3,\"SERVER_MSG\":2}");
	}

	//msg:3
	void SendClientType ()
	{
		addCommand ("{\"CLIENT_TYPE\":3,\"SERVER_MSG\":3}");
	}

	//msg:4
	void DisposeClientType (JsonData json, ResultData data)
	{
		if (((IDictionary)json).Contains ("ACTION_STATUS"))
			data.ACTION_STATUS = (int)json ["ACTION_STATUS"];
		ZpGameLog.Log ("DisposeClientType 4 " + data.ACTION_STATUS);
	}

	//msg:5
	void SendLoopRequest ()
	{
		addCommand ("{\"CLIENT_TYPE\":3,\"SERVER_MSG\":5}");
	}

	//msg:6
	void DisposeLoopRequest ()
	{
		//SendLoopRequest();
	}

	//msg: 7  client:255
	void DisposeBroadcastEyetrack (JsonData json, ResultData data)
	{
		if (data.CLIENT_TYPE == 255) {
			heartCount = 0;//及时清0
			if (((IDictionary)json).Contains ("SLANT"))
				data.SLANT = (string)json ["SLANT"];
			if (((IDictionary)json).Contains ("PITCH"))
				data.PITCH = (string)json ["PITCH"];
			if (((IDictionary)json).Contains ("CTVIEW"))
				data.CTVIEW = (string)json ["CTVIEW"];
			if (((IDictionary)json).Contains ("CAMERAX"))
				data.CAMERAX = (string)json ["CAMERAX"];
			if (((IDictionary)json).Contains ("CAMERAY"))
				data.CAMERAY = (string)json ["CAMERAY"];

			if (((IDictionary)json).Contains ("FACE_NUM"))
				data.FACE_NUM = (int)json ["FACE_NUM"];

			if (((IDictionary)json).Contains ("ON_OFF_3D"))
				data.ON_OFF_3D = (int)json ["ON_OFF_3D"];
			//Debugger.Log("EyeTrackResult_2222" + res.gSlant);
			lock (lockThis) {
                res[0] = 6;
                res[1] = float.Parse (data.PITCH);
                res[2] = float.Parse(data.SLANT);
                res[3] = float.Parse (data.CTVIEW);

				res[4] = float.Parse (data.CAMERAX);
				res[5] = float.Parse (data.CAMERAY);
				res[6] = (float)RGB_Mode;
				IsEyeTracking = true;
			}
		}
	}

	private static string Command_eye_close = "{\"CLIENT_TYPE\":3,\"SERVER_MSG\":27,\"ON_OFF_OPTION\":0}";
	private static string Command_eye_open = "{\"CLIENT_TYPE\":3,\"SERVER_MSG\":27,\"ON_OFF_OPTION\":1}";
	//msg:27
	void SendETRequest (bool Trig)
	{
		if (Trig) {
			addCommand (Command_eye_open);
		} else {
			addCommand (Command_eye_close);
		}
	}

	//msg:28
	void DisposeETRequest (JsonData json)
	{
		int Opt = 0;
		int Act = 2;

		if (((IDictionary)json).Contains ("ON_OFF_OPTION"))
			Opt = (int)json ["ON_OFF_OPTION"];
		if (((IDictionary)json).Contains ("ACTION_STATUS"))
			Act = (int)json ["ACTION_STATUS"];

		if (Opt == 0) {
			if (Act == 0 || Act == 1) {
				ETState = ETServiceState.STOPED;
			} else {
				SendETRequest (false);
			}
		} else if (Opt == 1) {
			if (Act == 0 || Act == 1) {
				ETState = ETServiceState.RUNED;
			} else
				SendETRequest (true);
		}

	}

	private static string Command_ft_close = "{\"CLIENT_TYPE\":3,\"SERVER_MSG\":29,\"ON_OFF_OPTION\":0}";
	private static string Command_ft_open = "{\"CLIENT_TYPE\":3,\"SERVER_MSG\":29,\"ON_OFF_OPTION\":1}";
	//msg:29
	void SendFTRequest (bool Trig)
	{
		if (Trig) {
			addCommand (Command_ft_open);
		} else {
			addCommand (Command_ft_close);
		}
	}

	//msg:30
	void DisposeFTRequest (JsonData json)
	{
		int Opt = 0;
		int Act = 2;

		if (((IDictionary)json).Contains ("ON_OFF_OPTION"))
			Opt = (int)json ["ON_OFF_OPTION"];
		if (((IDictionary)json).Contains ("ACTION_STATUS"))
			Act = (int)json ["ACTION_STATUS"];

		if (Opt == 0) {
			if (Act == 0 || Act == 1) {
				FTState = ETServiceState.STOPED;
			} else {
				SendFTRequest (false);
			}
		} else if (Opt == 1) {
			if (Act == 0 || Act == 1) {
				FTState = ETServiceState.RUNED;
			} else {
				SendFTRequest (true);
			}
		}
	}
    //msg:43
    void SentETStateRequest ()
	{
		addCommand ("{\"CLIENT_TYPE\":3,\"SERVER_MSG\":43}");
	}

	void DisposeETStateRequest (JsonData json)
	{

	}
    //msg:61
    void SentRGBModeRequest ()
	{
		addCommand ("{\"CLIENT_TYPE\":3,\"SERVER_MSG\":61}");
	}

    /**
     * 平台状态信息 1、是否外挂膜片 2、camera状态
     * */
    //msg:81
    void SendPlatformRequest ()
	{
		addCommand ("{\"CLIENT_TYPE\":3,\"SERVER_MSG\":81}");
	}

	void DisposePlatformResponse (JsonData json)
	{
		try {
			if (((IDictionary)json).Contains ("STATUS_0")) {//相机状态
				uint STATUS_0 = (uint)json ["STATUS_0"];
				uint cameraState = STATUS_0 >> 28;
				uint outerDiaphragm = (STATUS_0 << 4) >> 28;
				if (cameraState == 7 || cameraState == 8 || cameraState == 9 || cameraState == 10) {//待完成......
					CameraStateOk = false;
				}
				if (outerDiaphragm == 10) {//待完成......
					OuterDiaphragm = false;
				}
				log ("DisposePlatformResponse :" + "STATUS_0=" + STATUS_0 + "   cameraState=" + cameraState + "    outerDiaphragm=" + outerDiaphragm);
			}
			if (((IDictionary)json).Contains ("STATUS_1")) {//膜片状态
				long STATUS_1 = (long)json ["STATUS_1"];
			}
		} catch (Exception e) {
			//Debugger.Log("DisposePlatformResponse :" + e.Message);
		}
	}

	void DisposeRGBModeRequest (JsonData json, ResultData data)
	{
		if (((IDictionary)json).Contains ("RGB_MODE"))
			RGB_Mode = (int)json ["RGB_MODE"];
		Debug.Log ("DisposeRGBModeRequest " + RGB_Mode);
	}
    private void log(string msg)
    {
        if (!string.IsNullOrEmpty(msg)&&ZpGameStatic.Show_EyeTrace_Log)
        {
            Debug.Log(msg);
        }
    }

}

public class EyeTrackResult
{
	public float _newPitch;
	public float _leftTopViewNo;
	public float gSlant;
	public float gCameraX;
	public float gCameraY;
	public int RGB_Mode;

	public EyeTrackResult ()
	{
		_newPitch = 0;
		_leftTopViewNo = 0;
		gSlant = 0;
		gCameraX = 0;
		gCameraY = 0;
		RGB_Mode = 0;
	}

}

public enum AndroidServiceState
{
	TOOPEN,
	//正在启动
	RUNNING,
	//开启状态
	TOSTOP,
	//正在关闭
	STOPED,
	//停止状态
}

#endif