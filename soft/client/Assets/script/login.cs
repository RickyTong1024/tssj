using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;
using LitJson;

public class login : MonoBehaviour,IMessage {	

	public GameObject m_player_name;
	public GameObject m_player_name_main;
	public GameObject m_server_name;
	public GameObject m_login;
	public GameObject m_start_game;
	public GameObject m_create_account;
	public GameObject m_create_player_name;
	public GameObject m_create_pass_name;
	
	public GameObject m_change_password;
	public GameObject m_change_player_name;
	public GameObject m_change_old_pass;
	public GameObject m_change_new_pass1;
	public GameObject m_change_new_pass2;

	public GameObject m_recently_server;
	public GameObject m_all_server;
	public GameObject m_ver;
	public GameObject m_login2;

	public GameObject m_notice;
    public GameObject m_isbn_obj;

    public GameObject[] m_heads;
    public GameObject m_game_zonggao;
	///新登陆界面
	/// 开始
	/// 按钮


	public GameObject register_account_button;
	public GameObject recover_account_button;
	public GameObject login_button;
	public GameObject register_panel_registerbutton;
	public GameObject register_panel_loginbutton;
	/// <summary>
	///text 部分 
	/// </summary>

	public GameObject m_email_text;
	public GameObject m_account_text;
	public GameObject m_password_text;
	public GameObject m_password_confirm_text;

	public GameObject m_old_account_text;
	public GameObject m_old_password_text;

	public GameObject m_recover_email_text;

	public GameObject timer_show;


	/// 新登陆界面
	/// 开始
	/// 界面

	public GameObject recover_account_panel;
	public GameObject google_login_panel;
	public GameObject select_server_list_main;
	public GameObject register_account_panel;
	public GameObject confirm_login_panel;

	public GameObject register_panel_loginpanel;
	public GameObject register_panel_registerpanel;

	public UILabel m_start;
	public UILabel m_login_Label;
	public UILabel m_ok_Label;
	public UILabel m_usename;
	public UILabel m_password;
	public UILabel m_zhuce_tishi;
	public UILabel m_pass_tishi;
	ulong ctimer = 0;


	void Start () {
		cmessage_center._instance.add_handle (this);
		sys._instance.m_buttle_cam.SetActive (false);
		m_start_game.SetActive (true);
        if (game_data._instance.m_language == e_language.Simplified)
        {
            m_game_zonggao.SetActive(true);
        }
        else
        {
            m_game_zonggao.SetActive(false);
        }

        if (platform_config_common.m_isbn == 0)
        {
            m_isbn_obj.SetActive(false);
        }
        else
        {
            m_isbn_obj.SetActive(true);
            if (platform_config_common.m_login == 1)
            {
                UILabel label = m_isbn_obj.GetComponentInChildren<UILabel>();
                if (label != null)
                {
                    label.text = platform._instance.get_platform_isbn();
                }
            }     
        }
	}
	void OnDestroy()
	{	 
		cmessage_center._instance.remove_handle (this);
	}
    

	public void show_create_account()
	{	
		save_data _data = game_data._instance.m_player_data; 

		if (_data.m_user == null || _data.m_user.Length == 0 || _data.m_user == "null")
		{
			_data.m_user = "zh" + Random.Range(1000000,9999999);
			_data.m_pass = "mm" + Random.Range(1000000,9999999);
		}
		m_create_player_name.GetComponent<UIInput>().value = _data.m_user;
		m_create_pass_name.GetComponent<UIInput>().value = _data.m_pass;
		m_create_account.SetActive(true);
	}

	public void Androin2UnityMessage(string message)
	{
		if (message == "success")
        { // 注册成功
			game_data._instance.save();
            do_serverlist();
        }
        else if (message == "login")
        { // 登录成功
			game_data._instance.save();
            do_serverlist();
        }
		else if(message == "quxiao")   //执行取消
		{
			save_data _data = game_data._instance.m_player_data; 	

			m_create_player_name.GetComponent<UIInput>().value = _data.m_user;
			m_create_pass_name.GetComponent<UIInput>().value = _data.m_pass;

			m_create_account.SetActive(true);
		}
		else 
		{
			game_data._instance.m_player_data.m_user = message.Split('*')[0];
			game_data._instance.m_player_data.m_pass = message.Split('*')[1];

			m_create_player_name.GetComponent<UIInput>().value = game_data._instance.m_player_data.m_user;
			m_create_pass_name.GetComponent<UIInput>().value = game_data._instance.m_player_data.m_pass;
			game_data._instance.save();
            do_serverlist();
        }
	}

	public void show_create_account_ex()
	{	
		
		save_data _data = game_data._instance.m_player_data; 
		
		if (_data.m_user == null || _data.m_user.Length == 0 || _data.m_user == "null")
		{
			_data.m_user = "zh" + Random.Range(1000000,9999999);
			_data.m_pass = "mm" + Random.Range(1000000,9999999);
		}
		m_create_player_name.GetComponent<UIInput>().value = _data.m_user;
		m_create_pass_name.GetComponent<UIInput>().value = _data.m_pass;

		m_create_account.SetActive(true);

	}

	void reset_login()
	{	
		save_data _data = game_data._instance.m_player_data; 
		if(_data.m_id == -1)
		{	
			_data.m_id =  int.Parse(game_data._instance.m_server_list[game_data._instance.m_server_list.Count - 1].m_id);
			game_data._instance.save();
		}

		if(game_data._instance.m_server_list.Count == 0)
		{	

			string s = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			string s1 = game_data._instance.get_t_language ("login.cs_100_15");//服务器错误
			root_gui._instance.show_single_dialog_box(s,s1, new s_message());
			return;
		}
		bool flag = false;
		for (int i = 0; i < game_data._instance.m_server_list.Count; i++) {
			if(_data.m_id.ToString() == game_data._instance.m_server_list[i].m_id)
			{	
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			_data.m_id =  int.Parse(game_data._instance.m_server_list[game_data._instance.m_server_list.Count - 1].m_id);
			game_data._instance.save();
		}

		for (int i = 0; i < game_data._instance.m_server_list.Count; i++) 
		{
			if(game_data._instance.m_server_list[i].m_id == game_data._instance.m_player_data.m_id.ToString())
			{
				server_list _list = game_data._instance.m_server_list[i];
				if (game_data._instance.m_player_data.m_is_register == 2)
				{
					recover_account_button.SetActive(true);
					register_account_button.SetActive(false);
				}
				
				net_http._instance.m_ip = _list.m_http;
				net_tcp_chat._instance.m_ip = _list.m_tcp;
				net_tcp_chat._instance.m_port = _list.m_port;
                net_tcp_chat._instance.m_ip = "127.0.0.1";
                net_tcp_chat._instance.m_port = 9111;

                m_server_name.GetComponent<UILabel>().text = _list.m_name + game_data._instance.get_t_language ("login.cs_118_63");// [ffff44]<点击选区>
				m_player_name.GetComponent<UILabel>().text = _data.m_user;
			}
		}
	}

	GameObject create_item(int id)
	{	
		GameObject _item = game_data._instance.ins_object_res("ui/server_list_item");

		string _name = game_data._instance.m_server_list [id].m_name;
		string s = "";
		if(game_data._instance.m_server_list [id].m_state == 1)
		{	 
			s = game_data._instance.get_t_language ("login.cs_130_7");//流畅
			_name += "[00ff00]【" + s + "】";
		}
		else if(game_data._instance.m_server_list [id].m_state == 2)
		{	 
			s = game_data._instance.get_t_language ("login.cs_135_7");//普通
			_name += "[ffff00]【" + s + "】";
		}
		else if(game_data._instance.m_server_list [id].m_state == 3)
		{	 
			s = game_data._instance.get_t_language ("login.cs_140_7");//爆满
			_name += "[ff0000]【" + s + "】";
		}

		_item.transform.GetChild (0).GetComponent<UILabel>().text = _name ;
		_item.GetComponent<UIButtonMessage>().target = this.gameObject;
		_item.GetComponent<UIButtonMessage>().functionName = "select_server";

		return _item;
	}
	public void select_server(GameObject obj)
	{	
		game_data._instance.m_player_data.m_id = int.Parse (obj.transform.name);
		reset_login ();
		game_data._instance.save ();
	}

	void do_serverlist()
	{		
		s_message _msg = new s_message();
		_msg.m_type = "serverlist";
		cmessage_center._instance.add_message(_msg);
	}
	
	IEnumerator serverlist()
	{	 
		////服务器列表
        print(game_data._instance.get_t_language ("login.cs_174_14"));//读取服务器列表中...
		WWW _server_www = new WWW (platform_config.m_common_url + platform_config_common.m_serverlist_file + platform_config_common.get_url_end());
		while(!_server_www.isDone)
		{	 
			yield  return new WaitForSeconds(0.1f);
		}
		if(_server_www.error != null)
		{	 
			Debug.Log("http error  :" + _server_www.error);  
			do_serverlist();
		}
		else
		{
            XDocument _doc = new XDocument();
            StringReader reader = new StringReader(_server_www.text);
            _doc = XDocument.Load(reader);

            IEnumerable<XElement > servers = _doc.Elements("servers"); ;
            IEnumerable<XElement> nodeList = servers.Elements("server");
            IEnumerable<XElement> nodeList_pvp = servers.Elements("pvp");
            foreach (XElement xe in nodeList_pvp)
            {	
                net_http._instance.m_pvp_ip = xe.Attribute("http").Value;
            }
            IEnumerable<XElement> nodeList_team = servers.Elements("team");
            foreach (XElement xe in nodeList_team)
            {	
 
                net_tcp_bingyua.ip = xe.Attribute("tcp").Value;
                net_tcp_bingyua.port = int.Parse(xe.Attribute("port").Value);
            }
            game_data._instance.m_server_list.Clear();
            foreach (XElement xe in nodeList)
            {
                server_list _list = new server_list();
                _list.m_id = xe.Attribute("id").Value;
                _list.m_http = xe.Attribute("http").Value;
                _list.m_tcp = xe.Attribute("tcp").Value;
                _list.m_port = int.Parse(xe.Attribute("port").Value);
                _list.m_name = xe.Attribute("name").Value;
                _list.m_state = int.Parse(xe.Attribute("state").Value);
                game_data._instance.m_server_list.Add(_list);
            }

            IEnumerable<XElement> postList = servers.Elements("post");
            game_data._instance.m_postzd_list.Clear();
            foreach (XElement xe in postList)
            {
                post_zd post = new post_zd();
                post.title = xe.Attribute("title").Value;
                post.text = xe.Attribute("text").Value;
                game_data._instance.m_postzd_list.Add(post);
                sys._instance.m_self.is_postzd = true;
            }
			reset_login();
		}
	}


    void do_storage_data()
    {
        s_message _msg = new s_message();
        _msg.m_type = "get_storage_data";
        cmessage_center._instance.add_message(_msg);
    }

    IEnumerator storage_data()
    {
        root_gui._instance.wait(true);
        WWWForm _form = new WWWForm();
        _form.AddField("token", game_data._instance.m_player_data.m_token);
        WWW _www = new WWW(platform_config_common.get_storage_url() + "get", _form);
        yield return _www;
        root_gui._instance.wait(false);
        if (_www.error != null)
        {
            string s = game_data._instance.get_t_language("arena.cs_104_45");//提示
            string s1 = game_data._instance.get_t_language("login.cs_100_15");//服务器错误
            s_message msg = new s_message();
            msg.m_type = "get_storage_data";
            root_gui._instance.show_single_dialog_box(s, s1, msg);
        }
        else
        {
            game_data._instance.m_storage_sid.Clear();
            game_data._instance.m_storage_level.Clear();

            JsonData jd = JsonMapper.ToObject(_www.text);
            for (int i = 0; i < jd.Count; ++i)
            {
                int sid = int.Parse(jd[i]["serverid"].ToString());
                int level = int.Parse(jd[i]["level"].ToString());
                game_data._instance.m_storage_sid.Add(sid);
                game_data._instance.m_storage_level.Add(level);
            }
            show_server_gui();
        }
    }

    void do_account_first_login(string name, string pass)
    {
        s_message _msg = new s_message();
        _msg.m_type = "account_first_login";
        _msg.m_string.Add(name);
        _msg.m_string.Add(pass);
        cmessage_center._instance.add_message(_msg);
    }

    IEnumerator account_first_login(string name, string pass)
    {
        root_gui._instance.wait(true);
        WWWForm _form = new WWWForm();
        _form.AddField("username", name);
        _form.AddField("password", pass);
        WWW _www = new WWW(platform_config_common.get_account_url() + "login", _form);
        yield return _www;
        root_gui._instance.wait(false);
        if (_www.error != null)
        {
            string s = game_data._instance.get_t_language("arena.cs_104_45");//提示
            string s1 = game_data._instance.get_t_language("login.cs_100_15");//服务器错误
            s_message msg = new s_message();
            msg.m_type = "account_first_login";
            msg.m_string.Add(name);
            msg.m_string.Add(pass);
            root_gui._instance.show_single_dialog_box(s, s1, msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "account_first_login_end";
            _msg.m_string.Add(_www.text);
            _msg.m_string.Add(name);
            _msg.m_string.Add(pass);
            cmessage_center._instance.add_message(_msg);
        }
    }

    void do_account_reg(string name, string pass)
    {
        s_message _msg = new s_message();
        _msg.m_type = "account_reg";
        _msg.m_string.Add(name);
        _msg.m_string.Add(pass);
        cmessage_center._instance.add_message(_msg);
    }

    IEnumerator account_reg(string name, string pass)
    {
        root_gui._instance.wait(true);
        WWWForm _form = new WWWForm();
        _form.AddField("username", name);
        _form.AddField("password", pass);
        WWW _www = new WWW(platform_config_common.get_account_url() + "reg", _form);
        yield return _www;
        root_gui._instance.wait(false);
        if (_www.error != null)
        {
            string s = game_data._instance.get_t_language("arena.cs_104_45");//提示
            string s1 = game_data._instance.get_t_language("login.cs_100_15");//服务器错误
            s_message msg = new s_message();
            msg.m_type = "account_reg";
            msg.m_string.Add(name);
            msg.m_string.Add(pass);
            root_gui._instance.show_single_dialog_box(s, s1, msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "account_reg_end";
            _msg.m_string.Add(_www.text);
            _msg.m_string.Add(name);
            _msg.m_string.Add(pass);
            cmessage_center._instance.add_message(_msg);
        }
    }

    void do_account_login(string name, string pass)
    {
        s_message _msg = new s_message();
        _msg.m_type = "account_login";
        _msg.m_string.Add(name);
        _msg.m_string.Add(pass);
        cmessage_center._instance.add_message(_msg);
    }

    IEnumerator account_login(string name, string pass)
	{
		root_gui._instance.wait(true);
		WWWForm _form = new WWWForm();
		_form.AddField("username",name);
		_form.AddField("password",pass);
		WWW _www = new WWW (platform_config_common.get_account_url() + "login", _form);
		yield  return _www;
		root_gui._instance.wait(false);
		if(_www.error != null)
		{	
			string s = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			string s1 = game_data._instance.get_t_language ("login.cs_100_15");//服务器错误
            s_message msg = new s_message();
            msg.m_type = "account_login";
            msg.m_string.Add(name);
            msg.m_string.Add(pass);
            root_gui._instance.show_single_dialog_box(s, s1, msg);
        }
		else
		{
			s_message _msg = new s_message ();
			_msg.m_type = "account_login_end";
			_msg.m_string.Add (_www.text);
			_msg.m_string.Add (name);
			_msg.m_string.Add (pass);
			cmessage_center._instance.add_message (_msg);
		}
	}

    void do_account_bind(string name, string pass, string mail, string old_name, string old_password)
    {
        s_message _msg = new s_message();
        _msg.m_type = "account_bind";
        _msg.m_string.Add(name);
        _msg.m_string.Add(pass);
        _msg.m_string.Add(mail);
        _msg.m_string.Add(old_name);
        _msg.m_string.Add(old_password);
        cmessage_center._instance.add_message(_msg);
    }

    IEnumerator account_bind(string name, string pass, string mail, string old_name, string old_password)
    {
        root_gui._instance.wait(true);
        WWWForm _form = new WWWForm();
        _form.AddField("mail", mail);
        _form.AddField("username", name);
        _form.AddField("password", pass);
        _form.AddField("old_username", old_name);
        _form.AddField("old_password", old_password);
        WWW _www = new WWW(platform_config_common.get_account_url() + "bind", _form);
        yield return _www;
        root_gui._instance.wait(false);
        if (_www.error != null)
        {
            string s = game_data._instance.get_t_language("arena.cs_104_45");//提示
            string s1 = game_data._instance.get_t_language("login.cs_100_15");//服务器错误
            s_message msg = new s_message();
            msg.m_type = "account_bind";
            msg.m_string.Add(name);
            msg.m_string.Add(pass);
            msg.m_string.Add(mail);
            msg.m_string.Add(old_name);
            msg.m_string.Add(old_password);
            root_gui._instance.show_single_dialog_box(s, s1, msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "account_bind_end";
            _msg.m_string.Add(_www.text);
            _msg.m_string.Add(name);
            _msg.m_string.Add(pass);
            cmessage_center._instance.add_message(_msg);
        }
    }

    void do_account_chpwd(string name, string old_pass, string new_pwd)
    {
        s_message _msg = new s_message();
        _msg.m_type = "account_chpwd";
        _msg.m_string.Add(name);
        _msg.m_string.Add(old_pass);
        _msg.m_string.Add(new_pwd);
        cmessage_center._instance.add_message(_msg);
    }

    IEnumerator account_chpwd(string name, string old_pass, string new_pwd)
	{	
		root_gui._instance.wait(true);
		WWWForm _form = new WWWForm();
		_form.AddField("username",name);
		_form.AddField("old_password",old_pass);
		_form.AddField("password",new_pwd);
		WWW _www = new WWW (platform_config_common.get_account_url() + "chpwd", _form);
		yield  return _www;

		root_gui._instance.wait(false);
		if(_www.error != null)
		{	
			string s = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			string s1 = game_data._instance.get_t_language ("login.cs_100_15");//服务器错误
            s_message msg = new s_message();
            msg.m_type = "account_chpwd";
            msg.m_string.Add(name);
            msg.m_string.Add(old_pass);
            msg.m_string.Add(new_pwd);
            root_gui._instance.show_single_dialog_box(s, s1, msg);
		}
		else
		{
			s_message _msg = new s_message ();
			_msg.m_type = "account_chpwd_end";
			_msg.m_string.Add (_www.text);
			_msg.m_string.Add (new_pwd);
			cmessage_center._instance.add_message (_msg);
		}
	}

    void do_account_recovery(string mail)
    {
        s_message _msg = new s_message();
        _msg.m_type = "account_recovery";
        _msg.m_string.Add(mail);
        cmessage_center._instance.add_message(_msg);
    }

    IEnumerator account_recovery(string mail)
    {
        root_gui._instance.wait(true);
        WWWForm _form = new WWWForm();
        _form.AddField("mail", mail);
        string lang = "1";
        if (game_data._instance.m_language == e_language.Simplified)
        {
            lang = "0";
        }
        _form.AddField("lang", lang);
        WWW _www = new WWW(platform_config_common.get_account_url() + "recovery", _form);
        yield return _www;
        root_gui._instance.wait(false);
        if (_www.error != null)
        {
            string s = game_data._instance.get_t_language("arena.cs_104_45");//提示
            string s1 = game_data._instance.get_t_language("login.cs_100_15");//服务器错误
            s_message msg = new s_message();
            msg.m_type = "account_recovery";
            msg.m_string.Add(mail);
            root_gui._instance.show_single_dialog_box(s, s1, msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "account_recovery_end";
            _msg.m_string.Add(_www.text);
            cmessage_center._instance.add_message(_msg);
        }
    }

    void show_server_gui()
    {
        GameObject _server_list_gui = game_data._instance.ins_object_res("ui/server_list_gui");
        _server_list_gui.transform.parent = this.transform;
        _server_list_gui.transform.localPosition = new Vector3(0, 0, 0);
        _server_list_gui.transform.localScale = new Vector3(1, 1, 1);
        m_login2.SetActive(false);
        _server_list_gui.SetActive(true);
    }

    public void click(GameObject obj)
	{	
		save_data _data = game_data._instance.m_player_data; 
		if(obj.transform.name == "select_server")
		{
			if(game_data._instance.m_server_list.Count == 0)
			{	
				string s = game_data._instance.get_t_language ("login.cs_320_15");//[ffd000]请稍后…
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}

            do_storage_data();
        }

		if(obj.transform.name == "player_name")
		{	
			show_create_account_ex();
		}

		if(obj.transform.name == "ok")
		{	
			string _name = m_create_player_name.GetComponent<UILabel>().text;
			string _pass = m_create_pass_name.GetComponent<UILabel>().text;
			string s = "";
			if(_name.Length == 0 || _pass.Length == 0)
			{
				s = game_data._instance.get_t_language ("login.cs_353_8");//[ffd000]必须输入账号名和密码
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
            do_account_login(_name, _pass);
		}

		if(obj.transform.name == "chpwd")
		{	 
			m_create_account.SetActive(false);
			m_change_password.SetActive(true);
			if (_data.m_user == null || _data.m_user.Length == 0 || _data.m_user == "null")
			{	 
				m_change_player_name.GetComponent<UIInput>().value = "";
				m_change_old_pass.GetComponent<UIInput>().value = "";
			}
			else
			{
				m_change_player_name.GetComponent<UIInput>().value = _data.m_user;
				m_change_old_pass.GetComponent<UIInput>().value = _data.m_pass;
			}
			m_change_new_pass1.GetComponent<UIInput>().value = "";
			m_change_new_pass2.GetComponent<UIInput>().value = "";
		}
		if(obj.transform.name == "change_pwd")
		{	 
			string _name = m_change_player_name.GetComponent<UILabel>().text;
			string _old_pass = m_change_old_pass.GetComponent<UILabel>().text;
			string _new_pass1 = m_change_new_pass1.GetComponent<UILabel>().text;
			string _new_pass2 = m_change_new_pass2.GetComponent<UILabel>().text;
			string s = "";
			if(_name.Length == 0 || _old_pass.Length == 0 || _new_pass1.Length == 0 || _new_pass2.Length == 0)
			{	 
				s = game_data._instance.get_t_language ("login.cs_353_8");//[ffd000]必须输入账号名和密码
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if(_new_pass1 != _new_pass2)
			{	 
				s = game_data._instance.get_t_language ("login.cs_393_8");//[ffd000]两次密码输入不一致
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}

            do_account_chpwd(_name, _old_pass, _new_pass1);
		}
		if(obj.transform.name == "cancel_change")
		{	 
			m_change_password.SetActive(false);
			if (_data.m_user == null || _data.m_user.Length == 0 || _data.m_user == "null")
			{	
				show_create_account();
			};
		}
		if (obj.transform.name == "guest_login") {

			confirm_login_panel.SetActive(true);
			google_login_panel.SetActive(false);
		}

		if (obj.transform.name == "Account_Login") 
		{
			if(!register_panel_loginpanel.activeSelf)
			{
				register_panel_loginpanel.SetActive(true);
				register_panel_registerpanel.SetActive(false);

				m_email_text.GetComponent<UIInput>().value = "";
				m_account_text.GetComponent<UIInput>().value= "";
				m_password_text.GetComponent<UIInput>().value= "";
				m_password_confirm_text.GetComponent<UIInput>().value= "";
			}
		}
		if (obj.transform.name == "register_account_1") 
		{
			if(!register_panel_registerpanel.activeSelf)
			{
				register_panel_loginpanel.SetActive(false);
				register_panel_registerpanel.SetActive(true);
				m_old_account_text.GetComponent<UIInput>().value = "";
				m_old_password_text.GetComponent<UIInput>().value = "";
			}
		}
		if (obj.transform.name == "confirm_login") 
		{
			confirm_login();
		}
		if (obj.transform.name == "recover_button") 
		{
			recover_account_panel.SetActive(true);
		}
		if (obj.transform.name == "register_ok") 
		{
			register_account();
		}
		if(obj.transform.name == "login_ok")
		{
			string _name = m_old_account_text.GetComponent<UILabel>().text;
			string _pass = m_old_password_text.GetComponent<UILabel>().text;
			string s = "";
			if(_name.Length == 0 || _pass.Length == 0)
			{
				s = game_data._instance.get_t_language ("login.cs_353_8");//[ffd000]必须输入账号名和密码
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}

            do_account_login(_name, _pass);
		}
		if (obj.transform.name == "mail_close") 
		{
			recover_account_panel.SetActive(false);
			m_recover_email_text.GetComponent<UIInput>().value = "";

		}
		if (obj.transform.name == "register_close") 
		{
			register_account_panel.SetActive(false);
			m_email_text.GetComponent<UIInput>().value = "";
			m_account_text.GetComponent<UIInput>().value= "";
			m_password_text.GetComponent<UIInput>().value= "";
			m_password_confirm_text.GetComponent<UIInput>().value= "";
			m_old_account_text.GetComponent<UIInput>().value = "";
			m_old_password_text.GetComponent<UIInput>().value = "";
		}
		if (obj.transform.name == "recover_ok") 
		{
			if(ctimer < timer.now())
			{
				recover_account();
			}
			else
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("login.cs_728_15"));//[ffd000]稍后发送
			}
		}
	}
	void recover_account()
	{
		Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$"); //邮箱格式正则 
		string m_email = m_recover_email_text.GetComponent<UIInput>().value ;
		string s = "";
		if (r.IsMatch (m_email)) {
			ctimer = timer.now() + 30 * 1000;
			InvokeRepeating ("time", 0.0f, 1.0f);
            do_account_recovery(m_email);
		}
		else
		{
			s = game_data._instance.get_t_language ("login.cs_185_15");//[ffd000]邮箱格式错误
			root_gui._instance.show_prompt_dialog_box(s);
		}

	}
	void register_account()
	{
		Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$"); //邮箱格式正则 
		string m_email = m_email_text.GetComponent<UIInput>().value ;
		string tempname = m_account_text.GetComponent<UIInput>().value;
		string temppass = m_password_text.GetComponent<UIInput>().value;
		string tempconfirm = m_password_confirm_text.GetComponent<UIInput>().value;
		string s = "";
		if (temppass != tempconfirm) {
			s = game_data._instance.get_t_language ("login.cs_393_8");//[ffd000]两次密码输入不一致
			root_gui._instance.show_prompt_dialog_box(s);
			return;
		}
		if (r.IsMatch(m_email))
		{
			//邮箱正确的操作			
			if (game_data._instance.m_player_data.m_is_register == 0) 
			{
				//新玩家注册入口
                if (game_data._instance.m_player_data.m_user != "null")
                {
                    do_account_bind(tempname, temppass, m_email, game_data._instance.m_player_data.m_user, game_data._instance.m_player_data.m_pass);
                }
                else
                {
                    do_account_bind(tempname, temppass, m_email, "", "");
                }
			} 
			else if (game_data._instance.m_player_data.m_is_register == 1) 
			{
                //老玩家注册入口
                do_account_bind(tempname,temppass,m_email,game_data._instance.m_player_data.m_user, game_data._instance.m_player_data.m_pass);
			}
		}
		else
		{
			//邮箱不正确额操作
			s = game_data._instance.get_t_language ("login.cs_185_15");//[ffd000]邮箱格式错误
			root_gui._instance.show_prompt_dialog_box(s);
			return;
		}
	}

	void confirm_login()
	{
		if (game_data._instance.m_player_data.m_is_register == 0)
        {
			game_data._instance.m_player_data.m_user = "zh" + Random.Range (1000000, 9999999);
			game_data._instance.m_player_data.m_pass = "mm" + Random.Range (1000000, 9999999);
			m_create_player_name.GetComponent<UIInput>().value = game_data._instance.m_player_data.m_user;
			m_create_pass_name.GetComponent<UIInput>().value = game_data._instance.m_player_data.m_pass;
			s_message _msg = new s_message();
			_msg.m_type = "automatic_registered";
			_msg.m_string.Add(game_data._instance.m_player_data.m_user);
			_msg.m_string.Add(game_data._instance.m_player_data.m_pass);
			cmessage_center._instance.add_message(_msg);
		} 
		else if (game_data._instance.m_player_data.m_is_register == 1) 
		{
			m_create_player_name.GetComponent<UIInput>().value = game_data._instance.m_player_data.m_user;
			m_create_pass_name.GetComponent<UIInput>().value = game_data._instance.m_player_data.m_pass;
		}
		confirm_login_panel.SetActive (false);
	}
    
	public void start_game()
	{
        if (platform_config_common.m_login == 0)
		{
            save_data _data = game_data._instance.m_player_data;
            if (_data.m_user == null || _data.m_user.Length == 0 || _data.m_user == "null")
            {
                show_login();
                show_register_account();
            }
            else
            {
                do_account_first_login(_data.m_user, _data.m_pass);
            }
		}
		else
		{
            platform._instance.game_login();
        }
	}

    public void show_login()
    {
        m_start_game.SetActive(false);
        string v = game_data._instance.get_t_language("game_data.cs_2385_15") + ":" + platform_config_common.m_high_ver.ToString() + ".0";//版本
        m_ver.GetComponent<UILabel>().text = v;
        m_login.SetActive(true);
    }

    public void start_game_platform()
    {
        show_login();
        m_player_name_main.SetActive(false);
        register_account_button.SetActive(false);
        recover_account_button.SetActive(false);
        do_serverlist();
    }

	public void show_register_account()
	{
		google_login_panel.SetActive (true);
		register_account_button.SetActive (false);
		recover_account_button.SetActive (false);
		login_button.SetActive (false);
		m_player_name_main.SetActive (false);
		select_server_list_main.SetActive (false);
	}
	public void select_login_way(string result)
	{
		show_create_account();
		game_data._instance.m_player_data.m_user = "";
		game_data._instance.m_player_data.m_pass = "";
		game_data._instance.save ();
	}
	
	public void login_game(GameObject obj)
	{
        root_gui._instance.wait(true);
        download_fixinfo();
	}

    void update_res()
    {
        this.StopAllCoroutines();
        StartCoroutine(game_weihu());
    }
    public void download_fixinfo()
    {
        StartCoroutine(game_weihu());
    }
    IEnumerator game_weihu()
    {
        string m_text = game_data._instance.get_t_language("GameLoad.cs_41_11");//读取维护信息中
        WWW _www = new WWW(platform_config.m_common_url + "weihu" + platform_config_common.m_high_ver + ".xml" + platform_config_common.get_url_end());
        WWW m_gres_www = _www;
        while (!_www.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (_www.error != null)
        {
            Debug.Log("http error  :" + _www.error);
            update_res();
        }
        else
        {
            platform_config_common.read_weihu(_www.text);
            login_game_weihu();
        }
    }

    public void login_game_weihu()
    {
        if (game_data._instance.m_server_list.Count == 0)
        {
            string s = game_data._instance.get_t_language("login.cs_320_15");//[ffd000]请稍后…
            root_gui._instance.show_prompt_dialog_box("[ffd000]" + game_data._instance.get_t_language("login.cs_478_58"));//请稍后...
            return;
        }

        if (platform_config_common.m_weihu == 1)
        {
            bool _bai = false;
            for (int i = 0; i < game_data._instance.m_users.Count; i++)
            {
                if (game_data._instance.m_users[i] == game_data._instance.m_player_data.m_user)
                {
                    _bai = true;
                }
            }
            if (_bai == false)
            {
                root_gui._instance.show_prompt_dialog_box(platform_config_common.m_weihu_text);
                return;
            }
        }
        common_login();
    }

    void common_login()
    {
        string _name = m_player_name.GetComponent<UILabel>().text;
        sys._instance.m_sid = game_data._instance.m_player_data.m_id.ToString();

        for (int i = 0; i < game_data._instance.m_server_list.Count; i++)
        {
            if (game_data._instance.m_server_list[i].m_id == game_data._instance.m_player_data.m_id.ToString())
            {
                sys._instance.m_sname = game_data._instance.m_server_list[i].m_name;
            }
        }

        protocol.game.cmsg_client_login _msg = new protocol.game.cmsg_client_login();
        _msg.username = game_data._instance.m_player_data.m_token;
        _msg.password = game_data._instance.m_player_data.m_pass;
        _msg.serverid = sys._instance.m_sid;
        _msg.extra = platform._instance.get_platform_id();
        _msg.platform = platform_config_common.m_platform;
        _msg.lang_ver = (int)game_data._instance.m_language;
        _msg.device = platform_config_common.m_kc_code;
        _msg.version = platform_config_common.m_high_ver;

        net_http._instance.send_msg<protocol.game.cmsg_client_login>(opclient_t.CMSG_CLIENT_LOGIN, _msg, false);
    }

	void IMessage.net_message(s_net_message message)
	{	
		s_net_message _message = message;
		if (_message.m_opcode == opclient_t.CMSG_CLIENT_LOGIN) 
		{	
			protocol.game.smsg_client_login _msg = net_http._instance.parse_packet<protocol.game.smsg_client_login> (_message.m_byte);
			sys._instance.m_self.m_sig = _msg.sig;
			load_hall();
			sys._instance.m_self.m_t_player = _msg.player;
			sys._instance.m_self.m_guid = _msg.guid;
			for(int i = 0;i < _msg.roles.Count;i ++)
			{	
				sys._instance.m_self.add_card_login(_msg.roles[i]);
			}
			for(int i = 0;i < _msg.pets.Count;i ++)
			{	 
				sys._instance.m_self.add_pet_login(_msg.pets[i]);
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{	 
				sys._instance.m_self.add_equip_login(_msg.equips[i]);
			}
			for (int i = 0; i < _msg.treasures.Count; ++i)
			{	 
				sys._instance.m_self.add_treasure_login(_msg.treasures[i]);
			}
			sys._instance.m_self.check_huiyi_xuyao();
			sys._instance.m_self.update_attr();
			sys._instance.m_self.guild_shop_xs_time = _msg.guild_time;
			if (sys._instance.m_self.m_t_player.name == "")
			{	 
				string s = game_data._instance.get_t_language ("login.cs_553_15");//我
				sys._instance.m_self.m_t_player.name = s;
			}
			int bf = (int)sys._instance.m_self.get_fighting();
			sys._instance.m_self.m_t_player.bf = bf;
			timer.set_server_time(_msg.server_time);
			timer.start_time_ = _msg.start_time;
			sys._instance.m_self.m_time = timer.now();
			sys._instance.m_chat_msgs = _msg.chats;
			sys._instance.m_chat_msgs1 = _msg.guild_chats;
            platform_config_common.m_kc_time1 = _msg.device_time;
            platform._instance.on_game_login(_msg.is_new);
            net_tcp_chat._instance.connect();
		}
	}

    public string setid_price()
    {
        string id_prices = "";
        for (int i = 0; i < game_data._instance.m_dbc_recharge.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_recharge.get(0, i));
            s_t_recharge t_recharge = game_data._instance.get_t_recharge(id);
            if (!recharge_panel.recharge_vis(t_recharge))
            {
                continue;
            }

            if (id_prices == "")
            {
#if UNITY_ANDROID
                id_prices = t_recharge.ios_id.ToString();
#elif UNITY_IPHONE
		        id_prices = "Eng" + t_recharge.ios_id.ToString();
#endif

            }
            else
            {
#if UNITY_ANDROID
                id_prices = string.Format("{0}_{1}", id_prices, t_recharge.ios_id.ToString());
#elif UNITY_IPHONE
		        id_prices = string.Format("{0}_{1}", id_prices, "Eng" + t_recharge.ios_id.ToString());
#endif

            }
        }
        Debug.Log(id_prices);
        return id_prices;
    }

	void load_hall()
	{	 
		s_message _out_message = new s_message();
		_out_message.time = 1.0f;
		_out_message.m_type = "login_anim";
		cmessage_center._instance.add_message(_out_message);
		this.gameObject.SetActive (false);
	}

	void IMessage.message(s_message message)
	{
        if (message.m_type == "select_server")
        {
            game_data._instance.m_player_data.m_id = (int)message.m_ints[0];
            reset_login();
            m_login2.SetActive(true);
            game_data._instance.save();
        }
        else if (message.m_type == "serverlist")
        {
            StartCoroutine(serverlist());
        }
        else if (message.m_type == "reg_photo_end")
        {
            m_create_account.SetActive(false);
            do_serverlist();
        }
        else if (message.m_type == "get_storage_data")
        {
            StartCoroutine(storage_data());
        }
        else if (message.m_type == "account_first_login")
        {
            string _user = (string)message.m_string[0];
            string _pass = (string)message.m_string[1];
            StartCoroutine(account_first_login(_user, _pass));
        }
        else if (message.m_type == "account_first_login_end")
        {
            string _text = (string)message.m_string[0];
            string _user = (string)message.m_string[1];
            string _pass = (string)message.m_string[2];
            JsonData jd = JsonMapper.ToObject(_text);
            string _type = jd["res"].ToString();

            string s = "";
            show_login();
            if (_type == "0")
            {
                game_data._instance.m_player_data.m_token = jd["token"].ToString();
                game_data._instance.m_player_data.m_user = _user;
                game_data._instance.m_player_data.m_pass = _pass;
                if (jd["is_reg"].ToString() == "0")
                {
                    game_data._instance.m_player_data.m_is_register = 1;
                }
                else
                {
                    game_data._instance.m_player_data.m_is_register = 2;
                }
                game_data._instance.save();
                do_serverlist();
            }
            else
            {
                game_data._instance.m_player_data.m_user = "";
                game_data._instance.m_player_data.m_pass = "";
                game_data._instance.m_player_data.m_is_register = 0;
                show_register_account();
            }
        }
        else if (message.m_type == "account_reg")
        {
            string _user = (string)message.m_string[0];
            string _pass = (string)message.m_string[1];
            StartCoroutine(account_reg(_user, _pass));
        }
        else if (message.m_type == "account_reg_end")
        {
            string _text = (string)message.m_string[0];
            string _user = (string)message.m_string[1];
            string _pass = (string)message.m_string[2];
            JsonData jd = JsonMapper.ToObject(_text);
            string _type = jd["res"].ToString();
            string s = "";
            if (_type == "1")
            {
                game_data._instance.m_player_data.m_token = jd["token"].ToString();
                game_data._instance.m_player_data.m_user = _user;
                game_data._instance.m_player_data.m_pass = _pass;
                game_data._instance.m_player_data.m_is_register = 1;
                game_data._instance.save();
                s = game_data._instance.get_t_language("login.cs_877_14");//[00ff00]注册成功
                root_gui._instance.show_prompt_dialog_box(s);
                m_create_account.SetActive(false);
                m_player_name_main.SetActive(true);
                select_server_list_main.SetActive(true);
                login_button.SetActive(true);
                register_account_button.SetActive(true);
                do_serverlist();
            }
            else if (_type == "-1")
            {
                save_data guest_data = game_data._instance.m_player_data;
                guest_data.m_user = "zh" + Random.Range(1000000, 9999999);
                guest_data.m_pass = "mm" + Random.Range(1000000, 9999999);
                m_create_player_name.GetComponent<UIInput>().value = guest_data.m_user;
                m_create_pass_name.GetComponent<UIInput>().value = guest_data.m_pass;
                game_data._instance.m_player_data.m_user = guest_data.m_user;
                game_data._instance.m_player_data.m_pass = guest_data.m_pass;
                s_message _msg = new s_message();
                _msg.m_type = "automatic_registered";
                _msg.m_string.Add(guest_data.m_user);
                _msg.m_string.Add(guest_data.m_pass);
                cmessage_center._instance.add_message(_msg);
            }
            else if (_type == "-2")
            {
                s = game_data._instance.get_t_language("login.cs_652_8");//[ffd000]未知错误
                root_gui._instance.show_prompt_dialog_box(s);
                m_create_account.SetActive(true);
            }
            else if (_type == "-3")
            {
                s = game_data._instance.get_t_language("login.cs_658_8");//[ffd000]密码或用户名长度错误
                root_gui._instance.show_prompt_dialog_box(s);
                m_create_account.SetActive(true);
            }
        }
        else if (message.m_type == "account_login")
        {
            string _user = (string)message.m_string[0];
            string _pass = (string)message.m_string[1];
            StartCoroutine(account_login(_user, _pass));
        }
        else if (message.m_type == "account_login_end")
        {
            string _text = (string)message.m_string[0];
            string _user = (string)message.m_string[1];
            string _pass = (string)message.m_string[2];
            JsonData jd = JsonMapper.ToObject(_text);
            string _type = jd["res"].ToString();
            string s = "";
            if (_type == "0")
            {
                //登录成功   游客
                s = game_data._instance.get_t_language("login.cs_626_8");//[00ff00]登陆平台成功
                root_gui._instance.show_prompt_dialog_box(s);
                game_data._instance.m_player_data.m_token = jd["token"].ToString();
                game_data._instance.m_player_data.m_user = _user;
                game_data._instance.m_player_data.m_pass = _pass;
                if (jd["is_reg"].ToString() == "0")
                {
                    game_data._instance.m_player_data.m_is_register = 1;
                    recover_account_button.SetActive(false);
                    register_account_button.SetActive(true);
                }
                else
                {
                    game_data._instance.m_player_data.m_is_register = 2;
                    recover_account_button.SetActive(true);
                    register_account_button.SetActive(false);
                }
                game_data._instance.save();
                confirm_login_panel.SetActive(false);
                google_login_panel.SetActive(false);
                select_server_list_main.SetActive(true);
                m_player_name_main.SetActive(true);
                login_button.SetActive(true);
                register_account_panel.SetActive(false);
                m_create_account.SetActive(false);
                do_serverlist();
            }
            else if (_type == "-1")
            {
                s = game_data._instance.get_t_language("login.cs_689_8");//[ffd000]用户名不存在或密码错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-2")
            {
                s = game_data._instance.get_t_language("login.cs_652_8");//[ffd000]未知错误
                root_gui._instance.show_prompt_dialog_box(s);
                m_create_account.SetActive(true);
            }
            else if (_type == "-3")
            {
                s = game_data._instance.get_t_language("login.cs_658_8");//[ffd000]密码或用户名长度错误
                root_gui._instance.show_prompt_dialog_box(s);
                m_create_account.SetActive(true);
            }
            else if (_type == "-4")
            {
                s = game_data._instance.get_t_language("login.cs_689_8");//[ffd000]用户名不存在或密码错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else
            {
                s = game_data._instance.get_t_language("login.cs_652_8"); //[ffd000]未知错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
        }
        else if (message.m_type == "account_bind")
        {
            string _user = (string)message.m_string[0];
            string _pass = (string)message.m_string[1];
            string _mail = (string)message.m_string[2];
            string _ouser = (string)message.m_string[3];
            string _opass = (string)message.m_string[4];
            StartCoroutine(account_bind(_user, _pass, _mail, _ouser, _opass));
        }
        else if (message.m_type == "account_bind_end")
        {
            string _text = (string)message.m_string[0];
            string _user = (string)message.m_string[1];
            string _pass = (string)message.m_string[2];
            JsonData jd = JsonMapper.ToObject(_text);
            string _type = jd["res"].ToString();
            string s = "";
            //-1密码格式 -2未知错误 -3账号已注册 -4邮箱格式 -5邮箱已注册  1注册成功
            if (_type == "1" || _type == "2")
            {
                s = game_data._instance.get_t_language("login.cs_877_14");//[00ff00]注册成功
                root_gui._instance.show_prompt_dialog_box(s);
                if (_type == "1")
                {
                    game_data._instance.m_player_data.m_token = jd["token"].ToString();
                }
                game_data._instance.m_player_data.m_user = _user;
                game_data._instance.m_player_data.m_pass = _pass;
                game_data._instance.m_player_data.m_is_register = 2;
                game_data._instance.save();
                recover_account_button.SetActive(true);
                register_account_button.SetActive(false);
                confirm_login_panel.SetActive(false);
                google_login_panel.SetActive(false);
                select_server_list_main.SetActive(true);
                m_player_name_main.SetActive(true);
                login_button.SetActive(true);
                register_account_panel.SetActive(false);
                do_serverlist();
            }
            else if (_type == "-1")
            {
                s = game_data._instance.get_t_language("login.cs_689_8");//[ffd000]用户名不存在或密码错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-2")
            {
                s = game_data._instance.get_t_language("login.cs_652_8");//[ffd000]未知错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-3")
            {
                s = game_data._instance.get_t_language("login.cs_658_8");//[ffd000]密码或用户名长度错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-4")
            {
                s = game_data._instance.get_t_language("login.cs_185_15");//[ffd000]邮箱格式错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-5")
            {
                s = game_data._instance.get_t_language("login.cs_621_15");//[ffd000]邮箱已注册
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-6")
            {
                s = game_data._instance.get_t_language("login.cs_689_8");//[ffd000]用户名不存在或密码错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-7")
            {
                s = game_data._instance.get_t_language("login.cs_9652_15");//[ffd000]账号已注册
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else
            {
                s = game_data._instance.get_t_language("login.cs_652_8"); //[ffd000]未知错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
        }
        else if (message.m_type == "account_chpwd")
        {
            string _user = (string)message.m_string[0];
            string _opass = (string)message.m_string[1];
            string _npass = (string)message.m_string[2];
            StartCoroutine(account_chpwd(_user, _opass, _npass));
        }
        else if (message.m_type == "account_chpwd_end")
        {
            string _text = (string)message.m_string[0];
            string _pass = (string)message.m_string[1];
            JsonData jd = JsonMapper.ToObject(_text);
            string _type = jd["res"].ToString();
            string s = "";
            if (_type == "0")
            {
                s = game_data._instance.get_t_language("login.cs_676_8");//[00ff00]修改密码成功
                root_gui._instance.show_prompt_dialog_box(s);
                game_data._instance.m_player_data.m_pass = _pass;
                m_change_password.SetActive(false);
                game_data._instance.save();
                do_serverlist();
            }
            else if (_type == "-1")
            {
                s = game_data._instance.get_t_language("login.cs_689_8");//[ffd000]用户名不存在或密码错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-2")
            {
                s = game_data._instance.get_t_language("login.cs_652_8");//[ffd000]未知错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-3")
            {
                s = game_data._instance.get_t_language("login.cs_658_8");//[ffd000]密码或用户名长度错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-4")
            {
                s = game_data._instance.get_t_language("login.cs_689_8");//[ffd000]用户名不存在或密码错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else
            {
                s = game_data._instance.get_t_language("login.cs_652_8"); //[ffd000]未知错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
        }
        else if (message.m_type == "account_recovery")
        {
            string _mail = (string)message.m_string[0];
            StartCoroutine(account_recovery(_mail));
        }
        else if (message.m_type == "account_recovery_end")   //-1发送邮件失败 -2未知问题 -3邮箱未注册 -4邮箱格式错误  0发送成功
        {
            string _text = (string)message.m_string[0];
            JsonData jd = JsonMapper.ToObject(_text);
            string _type = jd["res"].ToString();
            string s = "";
            if (_type == "0")
            {
                s = game_data._instance.get_t_language("login.cs_129_15");//[00ff00]发送成功
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-1")
            {
                s = game_data._instance.get_t_language("login.cs_129_15");//[ffd000]发送邮件失败
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-2")
            {
                s = game_data._instance.get_t_language("login.cs_652_8");//[ffd000]未知错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-3")
            {
                s = game_data._instance.get_t_language("login.cs_123_15");//[ffd000]邮箱未注册
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else if (_type == "-4")
            {
                s = game_data._instance.get_t_language("login.cs_185_15"); //[ffd000]邮箱格式错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else
            {
                s = game_data._instance.get_t_language("login.cs_1574_15"); //[ffd000]未知错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
        }
        else if (message.m_type == "automatic_registered")
        {
            automatic_registered(message.m_string[0].ToString(), message.m_string[1].ToString());
        }
        else if (message.m_type == "game_user_success_login_game")
        {
            start_game_platform();
        }
	}

	void automatic_registered(string username , string password)
	{
		string _name = username;
		string _pass = password;
		string s = "";
		if(_name.Length == 0 || _pass.Length == 0)
		{
			s = game_data._instance.get_t_language ("login.cs_353_8");//[ffd000]必须输入账号名和密码
			root_gui._instance.show_prompt_dialog_box(s);
			return;
		}
        do_account_reg(_name, _pass);
	}

	public void register_login()
	{
		save_data _data = game_data._instance.m_player_data;
		if (_data.m_user == null || _data.m_user.Length == 0 || _data.m_user == "null") 
		{
			register_panel_registerbutton.GetComponent<UIToggle>().Set (true);
			register_panel_loginbutton.GetComponent<UIToggle>().Set (false);
			register_panel_loginpanel.SetActive (false);
			register_panel_registerpanel.SetActive (true);
			register_account_panel.SetActive (true);
		}
		else 
		{
			register_panel_loginbutton.SetActive(false);
			register_panel_registerbutton.GetComponent<UIToggle>().Set (true);
			register_panel_loginpanel.SetActive (false);
			register_panel_registerpanel.SetActive (true);
			register_account_panel.SetActive (true);
		}
	}

	void time()
	{
		if (timer.now () < ctimer)
        {
			long temp = (long)(ctimer-timer.now());
			timer_show.GetComponent<UILabel>().text = timer.show_sec(temp);
		} 
		else 
		{
			timer_show.GetComponent<UILabel>().text ="";
			CancelInvoke ("time");
		}
	}
}
