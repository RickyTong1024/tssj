using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

public class GameLoad : MonoBehaviour {	
	public XDocument m_version_www;
	private string m_file;
    private WWW m_gres_www;
	public GameObject m_string;
	private string m_text;
	public GameObject m_tip;
	public GameObject m_des;
	public GameObject m_name;

    void Start () 
	{	
		game_data._instance.load ();
        update_res();
	}
    
	void update_res()
	{
		
		this.StopAllCoroutines ();     
        StartCoroutine(game_weihu());
	}
	
	IEnumerator game_weihu()
	{	
		m_text = game_data._instance.get_t_language ("GameLoad.cs_41_11");// 读取维护信息中
		string _weihu = "weihu" + platform_config_common.m_high_ver + ".xml"  + platform_config_common.get_url_end();
		WWW _www = new WWW (platform_config.m_common_url + _weihu);
        m_gres_www = _www;
		while(!_www.isDone)
		{	
			yield  return new WaitForSeconds(0.1f);
		}
		
		if(_www.error != null)
		{	
			Debug.Log("http error  :" + _www.error);  
			update_res();
		}
		else
		{
            platform_config_common.read_weihu(_www.text);
            StartCoroutine(download_res());
		}
	}

	public void click(GameObject obj)
	{	
        if (obj.name == "ok")
        {
            platform._instance.game_update();
            Application.Quit();
        }
	}

	void tip(int high_ver)
	{
		m_tip.SetActive(true);
		m_name.GetComponent<UILabel>().text = game_data._instance.get_t_language("GameLoad.cs_282_8");
		m_des.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("GameLoad.cs_282_27"), high_ver) + ".00";//当前版本过旧，需要更新最新版本：{0};
    }

	void login_game()
	{	
		this.StopAllCoroutines ();
		UnityEngine.SceneManagement.SceneManager.LoadScene("game_main");
        GameObject.Destroy(this.gameObject);
    }

	IEnumerator download_res()
	{	
		m_text = game_data._instance.get_t_language ("GameLoad.cs_147_11");//读取版本信息中
		string _ver = "version" + platform_config_common.m_high_ver + ".xml" + platform_config_common.get_url_end();
		WWW _version_www = new WWW (platform_config.m_common_url + _ver);
		while(!_version_www.isDone)
		{	
			yield  return new WaitForSeconds(0.1f);
		}
		if(_version_www.error != null)
        {  
			Debug.Log("http error :" + _version_www.error);  
			update_res();
		}
		else
		{
			StringReader reader = new StringReader(_version_www.text);
			m_version_www = XDocument.Load(reader); 
			XElement ver = m_version_www.Element("ver");
            if (ver.Attribute ("shenhe") != null)
			{
                platform_config_common.m_shenhe = int.Parse(ver.Attribute("shenhe").Value);
            }
			else
			{
                platform_config_common.m_shenhe = 0;
			}
			if(ver.Attribute ("test") != null)
			{
                platform_config_common.m_test = int.Parse(ver.Attribute("test").Value);
			}
			else
			{
                platform_config_common.m_test = 0;
			}
            if (ver.Attribute("sys_ip") != null)
            {
                platform_config_common.m_sys_ip = ver.Attribute("sys_ip").Value;
            }
            if (ver.Attribute("open_url") != null)
            {
                platform_config_common.m_open_url = ver.Attribute("open_url").Value;
            }
            if (ver.Attribute("open_market") != null)
            {
                platform_config_common.m_open_market = ver.Attribute("open_market").Value;
            }
            if(ver.Attribute("extra") != null)
            {
                platform_config_common.m_extra = ver.Attribute("extra").Value;
            }
            IEnumerable<XElement> m_platform_notelist = ver.Elements("platform");
            foreach (XElement xe in m_platform_notelist)
            {
                string version_platform = xe.Attribute("channel").Value;
                if (version_platform == platform._instance.get_platform_id())
                {
                    if (xe.Attribute("shenhe") != null)
                    {
                        platform_config_common.m_shenhe = int.Parse(xe.Attribute("shenhe").Value);
                    }
                    if (xe.Attribute("test") != null)
                    {
                        platform_config_common.m_test = int.Parse(xe.Attribute("test").Value);
                    }
                    if (xe.Attribute("open_url") != null)
                    {
                        platform_config_common.m_open_url = xe.Attribute("open_url").Value;
                    }
                    if (xe.Attribute("open_market") != null)
                    {
                        platform_config_common.m_open_market = xe.Attribute("open_market").Value;
                    }
                }
            }
          
            int _high_ver = int.Parse(ver.Attribute("hight_ver").Value);
            if (_high_ver > platform_config_common.m_high_ver)
			{
				this.gameObject.AddComponent<sys>();
				tip(_high_ver);
			}
			else
			{
                StartCoroutine(gonggao(platform_config.m_common_url + platform_config_common.m_gonggao_file + platform_config_common.get_url_end()));
            }
		}
	}

    IEnumerator gonggao(string path)
    {	
		m_text = game_data._instance.get_t_language ("GameLoad.cs_360_17");//读取公告信息中
		WWW _www = new WWW(path);
        m_gres_www = _www;
        while (!_www.isDone)
        {	
			yield return new WaitForSeconds(0.1f);
        }
        if (_www.error != null)
        {	
			Debug.Log("http error  :" + _www.error);
            StartCoroutine(gonggao(path));
        }
        else
        {
            platform_config_common.m_gonggao = _www.text;
            print(_www.text);
            login_game();
        }
    }

	void Update () {
		string _show_text;
        if (m_gres_www != null)
        {	
            _show_text = m_text + m_gres_www.progress * 100 + "%";
            m_string.GetComponent<UILabel>().text = _show_text; 
        }
	}
}
