using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HBObj
{
	public string text;
	public Texture2D tex;
	public AudioClip sound;
	public float time;
};

public class common_http : MonoBehaviour {

	public static common_http _instance;
	private float m_wait = 0;
	private WWW m_www;
	private Dictionary<string, HBObj> m_objs = new Dictionary<string, HBObj>();

	public delegate void HttpBack(HBObj obj);

	// Use this for initialization
	void Awake()
	{
		_instance = this;
	}

	void Start () {

	}
	
	public void get(string code, int type, HttpBack hb, string msg = "", float wait = 10)
	{
		m_wait = wait;
		root_gui._instance.wait(true);
		StartCoroutine(http(code, type, hb, msg));
	}

	IEnumerator http(string url, int type, HttpBack hb, string msg)
	{
		if (m_objs.ContainsKey(url))
		{
			hb(m_objs[url]);
		}
		else
		{
			if (msg == "")
			{
				m_www = new WWW (url);
			}
			else
			{
				byte[] byteArray = System.Text.Encoding.UTF8.GetBytes (msg);
				m_www = new WWW(url, byteArray);
			}
			while(!m_www.isDone)
			{
				yield return new WaitForSeconds(0.1f);
			}
			
			do_www (url, m_www, type, hb, true);
		}
	}

	public void get_ex(string code, int type, HttpBack hb, string msg = "")
	{
		StartCoroutine(http_ex(code, type, hb, msg));
	}
	
	IEnumerator http_ex(string url, int type, HttpBack hb, string msg)
	{
		if (m_objs.ContainsKey(url))
		{
			hb(m_objs[url]);
		}
		else
		{
			WWW www;
			if (msg == "")
			{
				www = new WWW (url);
			}
			else
			{
				byte[] byteArray = System.Text.Encoding.UTF8.GetBytes (msg);
				www = new WWW(url, byteArray);
			}
			while(!www.isDone)
			{
				yield return new WaitForSeconds(0.1f);
			}
			
			do_www (url, www, type, hb, false);
		}
	}

	private void do_www(string url, WWW _www, int type, HttpBack hb, bool flag)
	{
		if (!string.IsNullOrEmpty(_www.error))
		{  
			Debug.Log("http error  :" + _www.error + " url:" + url);
			root_gui._instance.wait(false);
			m_wait = 0;
		} 
		else
		{
			if (flag)
			{
				root_gui._instance.wait(false);
				m_wait = 0;
			}
			HBObj hbobj = new HBObj();
			if (type == 0)
			{
				hbobj.text = _www.text;
				hb(hbobj);
			}
			else if (type == 1)
			{
				hbobj.tex = _www.texture;
				/*hbobj.time = 1800;
				if (!m_objs.ContainsKey(url))
				{
					m_objs.Add(url, hbobj);
				}
				else
				{
					m_objs[url] = hbobj;
				}*/
				hb(hbobj);
			}
			else if (type == 2)
			{
				hbobj.sound = _www.GetAudioClip(false);
				/*hbobj.time = 1800;
				if (!m_objs.ContainsKey(url))
				{
					m_objs.Add(url, hbobj);
				}
				else
				{
					m_objs[url] = hbobj;
				}*/
				hb(hbobj);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(m_wait > 0)
		{
			m_wait -= Time.deltaTime;
			
			if(m_wait <= 0)
			{
				this.StopAllCoroutines ();
				root_gui._instance.wait(false);
			}
		}
		foreach (string key in m_objs.Keys)
		{
			m_objs[key].time -= Time.deltaTime;
			if (m_objs[key].time <= 0)
			{
				m_objs.Remove(key);
			}
		}
	}
}
