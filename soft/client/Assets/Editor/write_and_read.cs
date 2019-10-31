using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text.RegularExpressions;
//using LitJson;

public class write_and_read : EditorWindow {




	static write_and_read m_window;
	[MenuItem("language/wuyongcode")]
	
	static void Execute()
	{
		if (m_window == null)
			m_window = (write_and_read)GetWindow(typeof (write_and_read));
		m_window.Show ();
	}

	void OnGUI()
	{
		{
			GUILayout.Label ("--- 测试读取所有的脚本 ---");
			GUILayout.BeginHorizontal ();
			bool _ok = GUILayout.Button ("读写脚本", GUILayout.Width (120f));
			GUILayout.EndHorizontal ();
			
			if (_ok)
			{
				xgtj ();
			}

			GUILayout.Label ("--- 在xml去掉中文配音添加到assets下 ---");
			GUILayout.BeginHorizontal ();
			bool _ok1 = GUILayout.Button ("去掉", GUILayout.Width (120f));
			GUILayout.EndHorizontal ();
			
			if (_ok1)
			{
				qudiao ();
			}
		}
	}


	//文本中每行的内容

//	//皮肤资源，这里用于显示中文
//	public GUISkin skin;
//	void Start ()	
//	{	
//		//删除文件
//		DeleteFile(Application.persistentDataPath,"FileName.txt");
//
//		//创建文件，共写入3次数据
//		
//	
//		//得到文本中每一行的内容
//		infoall = LoadFile(Application.persistentDataPath,"FileName.txt");
//
//	}
//	
	void CreateFile(string path,string info)
	{
		//文件流信息
		StreamWriter sw;		
		Debug.Log (path);
		FileInfo t = new FileInfo(path);		
		if(!t.Exists)			
		{			
			//如果此文件不存在则创建			
			sw = t.CreateText();			
		}
		else	
		{
			//如果此文件存在则打开
			sw = t.AppendText();	
		}
		Debug.Log (info);
		//以行的形式写入信息
		sw.WriteLine(info);
		//关闭流
		sw.Close();
		//销毁流
		sw.Dispose();
	}
//		
	List<string> LoadFile(string path)
	{
		//使用流的形式读取
		StreamReader sr =null;
		try{
			sr = File.OpenText(path);
			
		}catch(Exception)	
		{
			//路径与名称未找到文件则直接返回空
			return null;
		}
		
		string line;
		List<string> arrlist = new List<string>();
		while ((line = sr.ReadLine()) != null)
		{
			//一行一行的读取
			//将每一行的内容存入数组链表容器中
			arrlist.Add(line);
		}
		//关闭流
		sr.Close();
		//销毁流
		sr.Dispose();
		//将数组链表容器返回
		return arrlist;
	}
//	
//	
//	
//	/**
//
// * path：删除文件的路径
//
// * name：删除文件的名称
//
// */
//	
//	
//	
//	void DeleteFile(string path,string name)
//	{
//		File.Delete(path+"//"+ name);

//	}





	void xgtj()
	{
		{
			HashSet<string> files = new HashSet<string>();
			DirectoryInfo direction = new DirectoryInfo("Assets/script");
			FileInfo[] fileinfos = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
			int num_random = 0; 
			for (int i = 0; i < fileinfos.Length; ++i)
			{
				if (!fileinfos[i].Name.EndsWith(".cs"))
				{  
					continue;  
				}
				string name = fileinfos[i].FullName;

				int index = name.IndexOf("script");
				string s1 = name.Substring(index);


				List<string> infoall = LoadFile(name);
				int g = 0;
				string[] zimubiao = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","b","q","r","s","t","e","z","_c","_d","_g","_b","_p","_db","_fa","_fb","_dda","_fdfb","_sa","_yb"};
				num_random ++;
				for (int j = 1; j < infoall.Count; j++) 
				{
					string bianliang ="";
					if(infoall[j].Contains("{") && g == 0 && !infoall[j-1].Contains("enum"))
					{
//						infoall[j] =infoall[j]+ "\t\nstatic void yymoonteshalwoadakzmsld_weadsfo()\n{\nint afdfgwe = 1;\nafdfgwe +=111;\nfloat wearesorryforyou = 3.0f;\nstring sdasdasd = afdfgwe.ToString();\n}";

						int cdd =UnityEngine.Random.Range(5,21);
						for (int cd = 0; cd < cdd; cd++) {
							bianliang = string.Format("{0}{1}",bianliang,zimubiao[UnityEngine.Random.Range(1,zimubiao.Length - 2)]);
						}
						bianliang = bianliang+num_random.ToString();
						infoall[j] = infoall[j]+string.Format("\n{0} {1} = 20;","int",bianliang);
						g++;
					}
					else if(infoall[j].Contains("{") && infoall[j-1].Contains("class") && !infoall[j-1].Contains("("))
					{
//						infoall[j] =infoall[j]+ "\t\nstatic void yymoonteshalwoadakzmsld_weadsfo()\n{\nint afdfgwe = 1;\nafdfgwe +=111;\nfloat wearesorryforyou = 3.0f;\nstring sdasdasd = afdfgwe.ToString();\n}";
						int cdd =UnityEngine.Random.Range(5,21);
						for (int cd = 0; cd < cdd; cd++) {
							bianliang = string.Format("{0}{1}",bianliang,zimubiao[UnityEngine.Random.Range(1,zimubiao.Length - 2)]);
						}
						bianliang = bianliang+num_random.ToString();
						infoall[j] = infoall[j]+string.Format("{0} {1} = 20;","int",bianliang);
					}
					else if(infoall[j].Contains("{") && (infoall[j-1].Contains("enum") || !infoall[j-1].Contains("(")))
					{
	
						continue;
					}
					else if(infoall[j].Contains("{") && !infoall[j-1].Contains("switch") && !infoall[j].Contains("[") && !infoall[j].Contains("\""))
					{
//						infoall[j] +="\t\nyymoonteshalwoadakzmsld_weadsfo();";
						int cdd =UnityEngine.Random.Range(5,21);
						for (int cd = 0; cd < cdd; cd++) {
							bianliang =string.Format("{0}{1}",bianliang,zimubiao[UnityEngine.Random.Range(1,zimubiao.Length - 2)]);
						}
						bianliang = bianliang+num_random.ToString();
						infoall[j] = infoall[j]+string.Format("{0} {1} = 20;","int",bianliang);
					}


//					if(j == infoall.Count-1)
//					{
//						infoall[infoall.Count-1] = "\t\nvoid adfakjfhkajhfks()\n{\nint adasd = 1;\nadasd +=1;\nfloat adsdgggg = 2.0f;\n}"
//								+infoall[infoall.Count-1];
//
//					}else
//					{
//						continue;
//					}
				}

				string infoma ="";
				for (int k = 0; k < infoall.Count; k++) 
				{
					infoma = string.Format("{0}\n{1}",infoma,infoall[k]);
				}

				name =name.Replace("script","script_add");



				CreateFile(name,infoma);

			}
		}
	}

	void qudiao()
	{
		HashSet<string> files = new HashSet<string>();
		DirectoryInfo direction = new DirectoryInfo("Assets/Resources/unit_config");
		FileInfo[] fileinfos = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
		int num_random = 0; 
		for (int i = 0; i < fileinfos.Length; ++i)
		{
			if (!fileinfos[i].Name.EndsWith(".xml"))
			{  
				continue;  
			}
			string name = fileinfos[i].FullName;
				
			int index = name.IndexOf("unit_config");
			string s1 = name.Substring(index);
				
				
			List<string> infoall = LoadFile(name);
			string[] zimubiao = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","b","q","r","s","t","e","z","_c","_d","_g","_b","_p","_db","_fa","_fb","_dda","_fdfb","_sa","_yb"};
			num_random ++;
			for (int j = 1; j < infoall.Count; j++) 
			{

				if(infoall[j].Contains("tspy_jn"))
				{

					infoall[j] = "<!--"+infoall[j]+"-->";
				}
			}
				
			string infoma ="";
			for (int k = 0; k < infoall.Count; k++) 
			{
				infoma = string.Format("{0}\n{1}",infoma,infoall[k]);
			}
				
			name =name.Replace("unit_config","unit_config_copy");
				
				
				
			CreateFile(name,infoma);
				
		}
	}
	void xg_t_lang()
	{

		{
			HashSet<string> files = new HashSet<string>();
			DirectoryInfo direction = new DirectoryInfo("Assets/script");
			FileInfo[] fileinfos = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < fileinfos.Length; ++i)
			{
				if (!fileinfos[i].Name.EndsWith(".cs"))
				{  
					continue;  
				}
				string name = fileinfos[i].FullName;
				
				int index = name.IndexOf("script");
				string s1 = name.Substring(index);
				
				
				List<string> infoall = LoadFile(name);
				for (int j = 0; j < infoall.Count; j++) 
				{
					Regex r = new Regex("get_t_language (\""); //邮箱格式正则 
					if (r.IsMatch (infoall[j])) 
					{
						string[] strtemp = infoall[j].Split('"');

						if(strtemp.Length > 0)
						{
							for (int strindex = 0; strindex < strtemp.Length; strindex++) {

							}
						}
					}
				}
				
				string infoma ="";
				for (int k = 0; k < infoall.Count; k++) 
				{
					infoma = string.Format("{0}\n{1}",infoma,infoall[k]);
				}
				
				name =name.Replace("script","script_add");                

                CreateFile(name,infoma);
				
			}
		}
		 

	}

}