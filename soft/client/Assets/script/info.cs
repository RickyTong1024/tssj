using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LitJson;

public class info : MonoBehaviour, IMessage {

	public GameObject m_gold;
	public GameObject m_jewel;
	public GameObject m_mw;
    public GameObject m_hj;
    public GameObject m_shuijin;
	public GameObject m_jjc;
	public GameObject m_tili;
	public GameObject m_yuanli;
	public GameObject m_music;
	public GameObject m_sound;
    public GameObject m_guanhuan;
	public GameObject m_exp;
	public GameObject m_level;
	public GameObject m_vip;
	public GameObject m_jjd;
	public GameObject m_jl;
	public GameObject m_icon;
	public GameObject m_name;
	public GameObject m_lbinput;
	public GameObject m_id;
	private int m_t_class_id;
	public GameObject m_gamecenter;
	public GameObject m_question_input;
	public GameObject m_question_panel;
	public GameObject m_libao_panel;
	public GameObject m_touxiang;
	public GameObject m_scro;
	public GameObject m_num;
	public GameObject m_change_name_gui;
	public GameObject m_text;
	public GameObject m_name_price;
	public GameObject m_chenghao_icon;
	public GameObject m_google_button;
	private Dictionary<int, GameObject> m_icons = new Dictionary<int, GameObject>();
	private string m_code;
	private string text1 = "";
	private bool flag = false;
    public UILabel m_time1;
    public UILabel m_time2;
    public UILabel m_nltime1;
    public UILabel m_nltime2;
    public UILabel m_attacktime1;
    public UILabel m_attacktime2;
	public UILabel m_yinyue;
	public UILabel m_yinxiao;
	public UILabel m_zhnaghaoid;
	public UILabel m_renwudengji;
	public UILabel m_renwujingyan;
	public UILabel m_qiehuanzhanghao;
	public UILabel m_libaoma;
	public UILabel m_shurulibao;
	public UILabel m_queding;
	public UILabel m_weunti_tijiao;
	public UILabel m_wenti_queding;
	public UILabel m_wenti_name_Label;
	public UILabel m_touxiang_name_Labe;


	public GameObject m_mail;
	public GameObject m_account;
	public GameObject m_password;
	public GameObject m_confirm;

	// Use this for initialization
	void Start ()
    {
        cmessage_center._instance.add_handle(this);

        save_data _data_register = game_data._instance.m_player_data;
		if (_data_register.m_is_register == 2) 
		{
			transform.Find("back/register_button").gameObject.SetActive(false);
		}
        if (platform_config_common.m_login > 0)
        {
            transform.Find("back/register_button").gameObject.SetActive(false);
        }

        if (sys._instance.m_self.m_t_player.google_stat == 1) 
		{
			m_google_button.SetActive(false);
		}

        if (platform_config_common.m_nationality == 0)
        {
            this.transform.Find("back").Find("change_nationality").gameObject.SetActive(false);
        }
        else
        {
            this.transform.Find("back").Find("change_nationality").gameObject.SetActive(true);
        }


        if (platform_config_common.m_libao > 0)
		{
			this.transform.Find("back").Find("li_bao").gameObject.SetActive(true);
		}
		else
		{
			this.transform.Find("back").Find("li_bao").gameObject.SetActive(false);
		}
		m_gamecenter.SetActive (false);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		flag = false;
		update_ui ();
	}

	public void click(GameObject obj)
	{
		if(obj.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
		}
		if (obj.name == "music")
		{
            game_data._instance.m_player_data.m_music = 1 - game_data._instance.m_player_data.m_music;
			game_data._instance.save ();
			sys._instance.restart_cfg();
            do_sound();
        }
		if (obj.name == "sound")
		{
            game_data._instance.m_player_data.m_sound = 1 - game_data._instance.m_player_data.m_sound;
			game_data._instance.save ();
			sys._instance.restart_cfg();
            do_sound();
        }
        if (obj.name == "guanhuan")
        {
            game_data._instance.m_player_data.m_guanghuan = 1 - game_data._instance.m_player_data.m_guanghuan;
            game_data._instance.save();
            do_sound();
        }
        if (obj.name == "question")
		{
			flag = true;
			m_question_input.GetComponent<UIInput>().value = "";
			m_question_panel.SetActive(true);
		}
		if (obj.name == "lbok")
		{
			m_code = m_lbinput.GetComponent<UILabel>().text;
			if (m_code == "")
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("info.cs_190_71"));
				return;
			}
			protocol.game.cmsg_libao _msg = new protocol.game.cmsg_libao ();
			_msg.code = m_code;
			net_http._instance.send_msg<protocol.game.cmsg_libao> (opclient_t.CMSG_LIBAO, _msg);
		}
        if(obj.name == "change_nationality")
        {
            s_message _mes_nationality = new s_message();
            _mes_nationality.m_type = "show_nationality";
            _mes_nationality.m_ints.Add(0);
            cmessage_center._instance.add_message(_mes_nationality);
        }
		if (obj.name == "li_bao")
		{
			m_libao_panel.SetActive(true);
		}//lbclose
		if (obj.name == "lbclose")
		{
			m_libao_panel.transform.Find("frame_big").GetComponent<frame>().hide();
		}//lbclose
		if (obj.name == "question_close")
		{
			flag = false;
			m_question_panel.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if (obj.name == "sure")
		{
			m_code = m_question_input.GetComponent<UIInput>().value;
			if(m_code == "")
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("info.cs_215_71"));//不能为空
				return;
			}
			else
			{
				m_question_input.GetComponent<UIInput>().value = "";
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("info.cs_221_71"));//问题回馈成功，感谢您的帮助
				m_question_panel.transform.Find("frame_big").GetComponent<frame>().hide();
				return;
			}
		}
		if(obj.name == "gh")
		{
			m_touxiang.SetActive(true);
			create_panel ();
			check_icon ();
		}
		if(obj.name == "ok")
		{
			m_touxiang.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.name == "zx")
		{
			if(Application.isEditor)
			{
				sys._instance.game_logout();
			}
			else
			{
                if (platform_config_common.m_login==0)
                {
                    sys._instance.game_logout();
                }
                else if (platform_config_common.m_login == 1)
                {
                    platform._instance.game_logout();
                }               
			}
		}
		if(obj.name == "change_name")
		{
			change_name();
			m_change_name_gui.SetActive(true);
		}
		if(obj.name == "close_name")
		{
			m_change_name_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.name == "queding")
		{
			string text = m_text.GetComponent<UILabel>().text;
			if(text == game_data._instance.get_t_language ("info.cs_270_14") || text.Trim() == "")//角色名不得超过7个字
			{
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("info.cs_272_58"));//[ffc882]请先输入更改的名字
				return;
			}
			if(game_data._instance.m_dfa.fei_fa(text) || text.IndexOf(" ") >= 0)
			{
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("info.cs_277_58"));//[ffc882]非法名字
				return;
			}
			int num = sys._instance.m_self.m_t_player.change_name_num +1;
			if( num >= game_data._instance.m_dbc_price.get_y())
			{
				num = game_data._instance.m_dbc_price.get_y();
			}
			s_t_price t_price = game_data._instance.get_t_price (num);
			if(sys._instance.m_self.m_t_player.jewel < t_price.change_name)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
				return;
			}

			string _des = string.Format(game_data._instance.get_t_language ("info.cs_292_31"),sys._instance.get_res_color(2) ,t_price.change_name//是否花费{0}{1}钻石[-]更改名字?
			);
			s_message _msg = new s_message();
			_msg.m_type = "change_name";
            root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"), _des, _msg);//提示
		}
		if (obj.name == "register_button") 
		{
			transform.Find("back/register_account").gameObject.SetActive(true);
		}
		if (obj.name == "register_close") 
		{
			m_mail.GetComponent<UIInput>().value = "";
			m_account.GetComponent<UIInput>().value = "";
			m_password.GetComponent<UIInput>().value = "";
			m_confirm.GetComponent<UIInput>().value = "";
			transform.Find("back/register_account").gameObject.SetActive(false);
		}
		if (obj.name == "register") 
		{
			Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$"); //邮箱格式正则 
			string m_email = m_mail.GetComponent<UIInput>().value ;
			string tempname = m_account.GetComponent<UIInput>().value;
			string temppass = m_password.GetComponent<UIInput>().value;
			string tempconfirm = m_confirm.GetComponent<UIInput>().value;
			
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
					do_account_bind(tempname, temppass, m_email, "", "");
				} 
				else if (game_data._instance.m_player_data.m_is_register == 1)
				{
					//老玩家注册入口
					do_account_bind(tempname, temppass, m_email, game_data._instance.m_player_data.m_user, game_data._instance.m_player_data.m_pass);
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

    public void change_name()
	{
		m_text.GetComponent<UILabel>().text = game_data._instance.get_t_language ("info.cs_270_14");//角色名不得超过7个字
		int num = sys._instance.m_self.m_t_player.change_name_num +1;
		if( num >= game_data._instance.m_dbc_price.get_y())
		{
			num = game_data._instance.m_dbc_price.get_y();
		}
		s_t_price t_price = game_data._instance.get_t_price (num);
		if(sys._instance.m_self.m_t_player.jewel < t_price.change_name)
		{
			m_name_price.GetComponent<UILabel>().text = "[ff0000]x" + t_price.change_name.ToString();
		}
		else
		{
			m_name_price.GetComponent<UILabel>().text = sys._instance.get_res_color(2) + "x" + t_price.change_name.ToString();
		}
	}

	void IMessage.message (s_message message)
	{
		if(message.m_type == "chang_touxiang")
		{
			m_touxiang.transform.Find("frame_big").GetComponent<frame>().hide();
			m_t_class_id = (int)message.m_ints[0];
			protocol.game.cmsg_player_template _msg = new protocol.game.cmsg_player_template ();
			_msg.template_id = m_t_class_id;
			net_http._instance.send_msg<protocol.game.cmsg_player_template> (opclient_t.CMSG_PLAYER_TEMPLATE, _msg);
		}
		if(message.m_type == "change_name")
		{
			protocol.game.cmsg_player_name _msg = new protocol.game.cmsg_player_name ();				
			_msg.name = m_text.GetComponent<UILabel>().text;				
			net_http._instance.send_msg<protocol.game.cmsg_player_name> (opclient_t.CMSG_CHANGE_NAME, _msg);
		}
        if (message.m_type == "nationality_change")
        {
            update_ui();
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
        if (message.m_type == "account_bind_end")   //-1密码格式 -2邮箱已注册 -3账号已注册 -4邮箱格式 -10未知错误  1注册成功
		{
            string _text = (string)message.m_string[0];
            string _user = (string)message.m_string[1];
            string _pass = (string)message.m_string[2];
            JsonData jd = JsonMapper.ToObject(_text);
            string _type = jd["res"].ToString();
            string s = "";
            // -1密码格式 -2未知错误 -3账号已注册 -4邮箱格式 -5邮箱已注册  1注册成功
            if (_type == "1")
			{
				s = game_data._instance.get_t_language ("login.cs_877_14");//[00ff00]注册成功
				root_gui._instance.show_prompt_dialog_box (s);
                game_data._instance.m_player_data.m_token = jd["token"].ToString();
                game_data._instance.m_player_data.m_user = _user;
				game_data._instance.m_player_data.m_pass = _pass;
				game_data._instance.m_player_data.m_is_register = 2;
				game_data._instance.save();

				m_mail.GetComponent<UIInput>().value = "";
				m_account.GetComponent<UIInput>().value = "";
				m_password.GetComponent<UIInput>().value = "";
				m_confirm.GetComponent<UIInput>().value = "";
				transform.Find("back/register_account").gameObject.SetActive(false);
				transform.Find("back/register_button").gameObject.SetActive(false);

				this.transform.Find("frame_big").GetComponent<frame>().hide();
				s_message _message = new s_message();
				_message.m_type = "show_main_gui";
				cmessage_center._instance.add_message(_message);
			}
			else if(_type == "-1")
			{
				s = game_data._instance.get_t_language ("login.cs_965_15");//[ffd000]密码格式错误
				root_gui._instance.show_prompt_dialog_box (s);
			}
			else if(_type == "-3")
			{
				s = game_data._instance.get_t_language ("login.cs_9652_15");//[ffd000]账号已注册
				root_gui._instance.show_prompt_dialog_box (s);
			}
			else if(_type == "-4")
			{
				s = game_data._instance.get_t_language ("login.cs_185_15");//[ffd000]邮箱格式错误
				root_gui._instance.show_prompt_dialog_box (s);
            }
            else if (_type == "-5")
            {
                s = game_data._instance.get_t_language("login.cs_621_15");//[ffd000]邮箱已注册
                root_gui._instance.show_prompt_dialog_box(s);
            }
            else
            {
                s = game_data._instance.get_t_language("login.cs_652_8"); //[ffd000]未知错误
                root_gui._instance.show_prompt_dialog_box(s);
            }
        } 

	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_LIBAO)
		{
			protocol.game.smsg_libao _msg = net_http._instance.parse_packet<protocol.game.smsg_libao> (message.m_byte);
			for(int i =0 ;i< _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
			}
			for (int i = 0; i < _msg.types.Count; ++i)
			{
				sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("info.cs_352_101"));//礼包获得
			}
            if (_msg.chongzhi > 0)
            {
                sys._instance.player_check();
            }
		}
		if (message.m_opcode == opclient_t.CMSG_PLAYER_TEMPLATE)
		{
			sys._instance.m_self.m_t_player.template_id = (uint)m_t_class_id;
			sys._instance.remove_child (m_icon);
			GameObject _obj1 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id,sys._instance.m_self.m_t_player.dress_achieves.Count
			                                                             ,sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
			
			_obj1.transform.parent = m_icon.transform;
			_obj1.transform.localScale = new Vector3(1,1,1);
			_obj1.transform.localPosition = new Vector3(0,0,0);
		}
		if (message.m_opcode == opclient_t.CMSG_CHANGE_NAME)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if(_msg.success)
			{
				int num = sys._instance.m_self.m_t_player.change_name_num +1;
				if( num >= game_data._instance.m_dbc_price.get_y())
				{
					num = game_data._instance.m_dbc_price.get_y();
				}
				s_t_price t_price = game_data._instance.get_t_price (num);
				sys._instance.m_self.sub_att(e_player_attr.player_jewel,t_price.change_name,game_data._instance.get_t_language ("info.cs_384_80"));//改名字消耗
				sys._instance.m_self.m_t_player.name = m_text.GetComponent<UILabel>().text;
				sys._instance.m_self.m_t_player.change_name_num ++;
				m_change_name_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("info.cs_389_58"));//[fcc882]修改名字成功
				update_ui();
			}
		}
	}

	void update_ui()
	{
		m_level.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.level.ToString();
		s_t_exp t_exp = game_data._instance.get_t_exp (sys._instance.m_self.m_t_player.level + 1);
		if (t_exp == null)
		{
			m_exp.transform.GetComponent<UILabel>().text = "--/--";
		}
		else
		{
			m_exp.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.exp.ToString () + "/" + t_exp.exp.ToString ();
		}
		string s;
		if(sys._instance.m_self.m_t_player.vip >= 10)
		{
			s = sys._instance.m_self.m_t_player.vip.ToString();
		}
		else
		{
			s = "0" + sys._instance.m_self.m_t_player.vip;
		}
		m_vip.transform.GetComponent<UISprite>().spriteName = "grxx_vip0" + s;
		int _tili =  sys._instance.m_self.get_max_tili ();
		m_tili.transform.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)sys._instance.m_self.m_t_player.tili) + "/" + sys._instance.value_to_wan((long)_tili);
		m_yuanli.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.yuanli.ToString();
		
		{
			m_gold.transform.GetComponent<UILabel>().text = sys._instance.value_to_wan(sys._instance.m_self.m_t_player.gold);
		}
		
		m_jewel.transform.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)sys._instance.m_self.m_t_player.jewel);
		m_mw.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.mw_point.ToString();
		m_jjc.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.jjc_point.ToString();
		sys._instance.remove_child (m_icon);
		GameObject _obj1 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id,sys._instance.m_self.m_t_player.dress_achieves.Count
		                                                             ,sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
		m_name.transform.GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) + sys._instance.m_self.m_t_player.name;
		m_id.transform.GetComponent<UILabel>().text = game_data._instance.m_player_data.m_token;
		m_jjd.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.powder.ToString();
		m_jl.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.energy.ToString() + "/" +  sys._instance.m_self.get_max_nl();
        m_hj.GetComponent<UILabel>().text = sys._instance.m_self.get_att(e_player_attr.player_medal_point) + "";
        m_shuijin.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.boss_num + "/10";
		sys._instance.get_chenghao (sys._instance.m_self.m_t_player.chenghao_on,m_chenghao_icon);
		do_sound ();
        CancelInvoke();
        InvokeRepeating("time",0,1f);
	}

	void do_sound()
	{
		if (game_data._instance.m_player_data.m_music == 1)
		{
			m_music.transform.Find("open").gameObject.SetActive(true);
			m_music.transform.Find("close").gameObject.SetActive(false);
		}
		else
		{
			m_music.transform.Find("open").gameObject.SetActive(false);
			m_music.transform.Find("close").gameObject.SetActive(true);
		}
        if (game_data._instance.m_player_data.m_sound == 1)
        {
			m_sound.transform.Find("open").gameObject.SetActive(true);
			m_sound.transform.Find("close").gameObject.SetActive(false);
		}
		else
		{
			m_sound.transform.Find("open").gameObject.SetActive(false);
			m_sound.transform.Find("close").gameObject.SetActive(true);
		}
        if (game_data._instance.m_player_data.m_guanghuan == 1)
        {
           m_guanhuan.transform.Find("open").gameObject.SetActive(true);
           m_guanhuan.transform.Find("close").gameObject.SetActive(false);
        }
        else
        {
            m_guanhuan.transform.Find("open").gameObject.SetActive(false);
            m_guanhuan.transform.Find("close").gameObject.SetActive(true);
        }
	}
   
	void create_panel()
	{
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_scro);
		int num = 0;
		for (int l = 5; l >= 3; --l)
		{
			for (int i = 0; i < game_data._instance.m_dbc_class.get_y(); ++i)
			{
				int id = int.Parse(game_data._instance.m_dbc_class.get(0, i));
				s_t_class t_class = game_data._instance.get_t_class(id);
                if (id > 1000)
                {
                    continue;
                }
				if (t_class.color == l && t_class.ycang < 2)
				{
                    GameObject citem = game_data._instance.ins_object_res("ui/touxiang_item");
					citem.transform.parent = m_scro.transform;
					citem.transform.localPosition = new Vector3(-160 + (num % 4) * 105, 123 - (num / 4) * 162,0);
					citem.transform.localScale = new Vector3(1,1,1);
					citem.transform.GetComponent<touxiang_item>().t_class_id = id;
					citem.transform.GetComponent<touxiang_item>().reset();
					GameObject icon = icon_manager._instance.create_card_icon(t_class.id, 0, 0, 0);
					icon.transform.parent = citem.transform;
					icon.transform.localPosition = new Vector3(0, 0, 0);
					icon.transform.localScale = new Vector3(1, 1, 1);
					icon.transform.GetComponent<BoxCollider>().enabled = false;
					
					m_icons[id] = icon;
					
					num++;
				}
			}
		}
	}

	void check_icon()
	{
		foreach (int id in m_icons.Keys)
		{
			bool flag = false;
			for (int i = 0; i < sys._instance.m_self.m_t_player.role_template_ids.Count; ++i)
			{
				if (id == sys._instance.m_self.m_t_player.role_template_ids[i])
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				m_icons[id].transform.GetComponent<UISprite>().alpha = 0.5f;
			}
			else
			{
				m_icons[id].transform.GetComponent<UISprite>().alpha = 1.0f;
			}
		}
	}
    void time()
    {
        long value = 0;
        long _time = 360000 - (int)(timer.now() - sys._instance.m_self.m_t_player.last_tili_time);//6min
        if (_time > 0)
        {
            value  += _time + (sys._instance.m_self.get_max_tili() - sys._instance.m_self.m_t_player.tili - 1) * 6 * 60 * 1000;
            m_time1.GetComponent<UILabel>().text = timer.get_time_show(_time);
        }
        else
        {
            m_time1.GetComponent<UILabel>().text = "00:00:00";
 
        }
        if (sys._instance.m_self.m_t_player.tili < sys._instance.m_self.get_max_tili())
        {
            m_time2.text = get_time(timer.dtnow().AddMilliseconds((double)value)) + "";
        }
        else
        {
            m_time2.text = game_data._instance.get_t_language ("info.cs_562_27");//已满
            m_time1.GetComponent<UILabel>().text = "00:00:00";
        }
        
        _time = 30 * 60 * 1000 - (int)(timer.now() - sys._instance.m_self.m_t_player.last_energy_time);//30min
        value = 0;
        if (_time > 0)
        {
            value += _time + (sys._instance.m_self.get_max_nl() - sys._instance.m_self.m_t_player.energy - 1) * 30 * 60 * 1000;
            m_nltime1.text = timer.get_time_show(_time);
           
        }
        else
        {
            m_nltime1.text = "00:00:00";
 
        }
        if (sys._instance.m_self.m_t_player.energy < sys._instance.m_self.get_max_nl())
        {
            m_nltime2.text = get_time (timer.dtnow().AddMilliseconds((double)value)) + "";
        }
        else
        {
            m_nltime2.text = game_data._instance.get_t_language ("info.cs_562_27");//已满
            m_nltime1.text = "00:00:00";
        }
        value = 0;
        _time = 60 * 60 * 1000 -(int)(timer.now() - sys._instance.m_self.m_t_player.boss_last_time);//120min
        if (_time > 0)
        {
            value += _time + (10 - sys._instance.m_self.m_t_player.boss_num - 1) * 60 * 60 * 1000;
            m_attacktime1.text = timer.get_time_show(_time);
        }
        else
        {
            m_attacktime1.text = "00:00:00";
 
        }
        if (sys._instance.m_self.m_t_player.boss_num < 10)
        {
            m_attacktime2.text = get_time(timer.dtnow().AddMilliseconds((double)value));
        }
        else
        {
            m_attacktime2.text = game_data._instance.get_t_language ("info.cs_562_27");//已满
            m_attacktime1.text = "00:00:00";
        }
       
    }
    string  get_time(System.DateTime time)
    {
        string min = "";
        if (time.Minute > 9)
        {
            min = time.Minute + "";
        }
        else
        {
            min = "0" + time.Minute;
        }
        if (timer.dtnow().Day != time.Day)
        {
            
            return game_data._instance.get_t_language ("info.cs_625_19") + time.Hour + ":" + min;//明日
        }
        else
        {
            return game_data._instance.get_t_language ("info.cs_629_19") + time.Hour + ":" + min;//今日
        }
 
    }
	public static int GetHanNumFromString(string str)
	{
		float count = 0;
		Regex regex = new Regex(@"^[\u4E00-\u9FA5]{0,}$");
		Regex regex1 = new Regex (@"^[A-Za-z0-9]+$");

		for (int i = 0; i < str.Length; i++)
		{
			if (regex.IsMatch(str[i].ToString()))
			{
				count++;
			}
			else if (regex1.IsMatch(str[i].ToString()))
			{
				count += 0.5f;
			}
			else
			{
				count += 0.5f;
			}
		}
		return (int)count;
	}

	// Update is called once per frame
	void Update () {
		if (flag) 
		{
			int count = 0;
			string text = "";
			text = m_question_input.GetComponent<UIInput>().value;
			count = GetHanNumFromString (text);
			if (count <= 200) {
				m_question_input.GetComponent<UIInput>().enabled = true;
				text1 = m_question_input.GetComponent<UIInput>().value;
			}
			if (count > 200) {
				m_question_input.GetComponent<UIInput>().value = text1;
			
				m_question_input.GetComponent<UIInput>().enabled = false;
			}
			m_num.GetComponent<UILabel>().text = count.ToString () + "/200";
		}
		m_tili.transform.GetComponent<UILabel>().text = sys._instance.value_to_wan ((long)sys._instance.m_self.m_t_player.tili) + "/" + sys._instance.value_to_wan((long)sys._instance.m_self.get_max_tili ());
        m_jl.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.energy.ToString() + "/" + sys._instance.m_self.get_max_nl();
	}
}
