
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure_qianghua_gui : MonoBehaviour,IMessage {

	dhc.treasure_t m_treasure;
	int enhance_num =0;
	public GameObject m_effect;
	private string m_out_message;
	public GameObject m_powder;
	public GameObject m_enhance;
	public GameObject m_exp_num;
	public GameObject m_bar1;
	public float m_bar_value = 0.0f;
	public GameObject m_main_enhance;
	public GameObject m_main1_enhance;
	public GameObject m_yj_enhance;
	public GameObject m_main;
	public GameObject m_main1;
	public GameObject m_num_gui;

	public List<GameObject> m_treasures = new List<GameObject>();
	public List<ulong> m_treasure_guids = new List<ulong >();
	public List<ulong> m_guids = new List<ulong>();
	public List<dhc.treasure_t> m_baowus = new List<dhc.treasure_t>();
	public List <ulong> m_qianghua_guids = new List<ulong>();
	public List<int> m_gongzhens = new List<int>();
	private Vector3 m_mouse_pos;
	private int count =0;
	public GameObject m_enhance_new;
	public GameObject m_enhance_old;
	public GameObject m_attr;
	public GameObject m_attr1;
	public GameObject m_attr_new;
	public GameObject m_attr_old;
	public GameObject m_attr1_new;
	public GameObject m_attr1_old;
	public GameObject m_right;
	public GameObject m_left;
	private bool sflag = false;
	//private int treasure_exp = 0;
	private int m_select_treasure = 0;
	private int enhance_up = 0;
	private string error = "";
	private GameObject m_enhance_button;

	public UILabel m_enhance_xg;
	public UILabel m_gold_cons;
	public UILabel m_tishi1;
	public UILabel m_tishi2;
	public UILabel m_zd_Label;
	public UILabel m_des;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}


	public void OnEnable()
	{
		m_qianghua_guids.Clear ();
		count = 0;
		for(int i =0;i<m_guids.Count;++i)
		{
			if(m_treasure.guid == m_guids[i])
			{
				break;
			}
			count ++;
		}
		m_right.SetActive(true);
		m_left.SetActive (true);
		if(count >= m_guids.Count -1)
		{
			m_right.SetActive(false);
		}
		if(count <= 0)
		{
			m_left.SetActive(false);
		}
		update_icon ();

		if (m_treasure.role_guid != 0 ) 
		{
			ccard m_card = sys._instance.m_self.get_card_guid (m_treasure.role_guid);
			sys._instance.m_card = m_card;
			sys._instance.m_gongzhengs.Clear();
			sys._instance.m_gongzhengs = gongzheng_mubiao_gui.gongzhen_cur (m_card);
		}
	}

	public void press()
	{
		m_mouse_pos.x = sys._instance.get_mouse_position ().x;
		m_mouse_pos.y = sys._instance.get_mouse_position ().y;
	}
	
	public void release()
	{
		if(sys._instance.m_pause == true )
		{
			return;
		}
		Vector3 _mouse_pos = sys._instance.get_mouse_position ();
		int _add = 0;
		float _x = m_mouse_pos.x - _mouse_pos.x;
		float _y = m_mouse_pos.y - _mouse_pos.y;
		
		if(Mathf.Abs(_x) > Mathf.Abs(_y) && Mathf.Abs(_x) > 50)
		{
			if(m_mouse_pos.x >  _mouse_pos.x && m_right.activeSelf)
			{
				sflag = true;
				_add = 1;
			}
			else if(m_mouse_pos.x <  _mouse_pos.x && m_left.activeSelf)
			{
				sflag = true;
				_add = -1;
			}
			
			if(count != add_cur(_add))
			{
				sflag = true;
				m_treasure_guids.Clear();
				for(int i = 0; i < m_treasures.Count;++i)
				{
					sys._instance.remove_child(m_treasures[i]);
				}
				m_treasure = sys._instance.m_self.get_treasure_guid(m_guids[count]);
				reset (m_treasure,m_out_message);
			}
		}
	}

	void add_scale_anim(GameObject obj)
	{
		TweenScale _scale = TweenScale.Begin (obj,0.25f,new Vector3(1.0f,1.0f,1.0f));

		_scale.method = UITweener.Method.EaseInOut;
		_scale.from = new Vector3 (1.5f, 1.5f, 1.5f);
		_scale.to = new Vector3(1.0f,1.0f,1.0f);
		_scale.delay = 0;
	}

	public void reset(dhc.treasure_t t_treasure, string out_message,bool sx_flag = true) 
	{
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_yj_treasure_qh)
		{
			m_main.SetActive(true);
			m_main1.SetActive(false);
			m_enhance_button = m_main_enhance;
		}
		else
		{
			m_main.SetActive(false);
			m_main1.SetActive(true);
			m_enhance_button = m_main1_enhance;
		}
		m_treasure = t_treasure;
		if (m_treasure.role_guid != 0 ) 
		{
			ccard m_card = sys._instance.m_self.get_card_guid (m_treasure.role_guid);
			m_gongzhens.Clear();
			m_gongzhens = gongzheng_mubiao_gui.gongzhen_cur (m_card);
		}
		error = "";
		enhance_up = sys._instance.m_self.m_t_player.level;
		if(enhance_up > game_data._instance.m_dbc_treasure_enhance.get_y() -1)
		{
			enhance_up = game_data._instance.m_dbc_treasure_enhance.get_y() -1;
		}

		m_out_message = out_message;
		int level_up_value = 0;
		sys._instance.remove_child (transform.Find("center").Find("old_treasure").gameObject);
		GameObject _icon = icon_manager._instance.create_treasure_icon(m_treasure);
		_icon.transform.parent = this.transform.Find("center").Find("old_treasure");
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
		if(sflag)
		{
			sys._instance.add_pos_anim(_icon,0.3f, new Vector3(0, 60, 0), 0.05f);
			sys._instance.add_alpha_anim(_icon,0.3f, 0, 1.0f, 0.05f);
		}
		_icon.transform.GetComponent<BoxCollider>().enabled = false;
		
		s_t_baowu _treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		transform.Find("center").Find("name").GetComponent<UILabel>().text = _treasure.name ;
		if (m_treasure.enhance < game_data._instance.m_dbc_treasure_enhance.get_y() - 1)
		{
			int num = level_num();
			int gold = toltle_gold();
			string s = sys._instance.get_res_color(1);
			if (gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
			{
				error = game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
				s = "[ff0000]";
			}

			m_powder.GetComponent<UILabel>().text = s + gold.ToString();
			if(sx_flag)
			{
				add_scale_anim(m_powder);
			}

			s = "";
			if (m_treasure.enhance  >= sys._instance.m_self.m_t_player.level)
			{
				error = game_data._instance.get_t_language ("chong_neng_gui.cs_258_12");//玩家等级不足
			}
			int _num = sys._instance.m_self.m_t_player.level;
			if(_num > game_data._instance.m_dbc_treasure_enhance.get_y() - 1)
			{
				_num = game_data._instance.m_dbc_treasure_enhance.get_y() - 1;
			}
			level_up_value = game_data._instance.get_treasure_enhance (m_treasure.enhance + 1+num, _treasure.font_color);
			if(m_treasure.enhance + 1+num > game_data._instance.m_dbc_treasure_enhance.get_y() -1)
			{
				level_up_value = game_data._instance.get_treasure_enhance (game_data._instance.m_dbc_treasure_enhance.get_y() -1, _treasure.font_color);
			}
			int level_toltal_value = game_data._instance.get_total_treasure_enhance (_num, _treasure.font_color);
			int level_max = game_data._instance.get_total_treasure_enhance(m_treasure.enhance +num ,_treasure.font_color);
			m_exp_num.GetComponent<UILabel>().text = get_enhance_exp().ToString() +  "/" +  level_up_value;
			m_bar_value = (float)get_enhance_exp()/ level_up_value;
	
			m_enhance.GetComponent<UILabel>().text = "[+" + m_treasure.enhance.ToString() + "]";
			m_enhance_old.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("czjh_sub.cs_21_55"),m_treasure.enhance.ToString ());//{0}级
			m_enhance_new.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("czjh_sub.cs_21_55"),(m_treasure.enhance+num).ToString () );//{0}级
			if(sx_flag)
			{
				add_scale_anim(m_enhance_new);
			}
			string text = game_data._instance.get_value_string(_treasure.attr1,(int)_treasure.value1);
			m_attr.GetComponent<UILabel>().text = get_atrr_name(text);
			if(_treasure.attr2 > 0)
			{
				m_attr1.SetActive(true);
				text = game_data._instance.get_value_string(_treasure.attr2,(int)_treasure.value2,0);
				m_attr1.GetComponent<UILabel>().text = get_atrr_name(text);
			}
			else
			{
				m_attr1.SetActive(false);
			}
			m_attr_old.GetComponent<UILabel>().text = get_treasure_string(m_treasure.template_id,m_treasure.enhance,0);
			m_attr_new.GetComponent<UILabel>().text = get_treasure_string(m_treasure.template_id,m_treasure.enhance+num,0);
			if(sx_flag)
			{
				add_scale_anim(m_attr_new);
			}
			m_attr1_old.GetComponent<UILabel>().text = get_treasure_string(m_treasure.template_id,m_treasure.enhance,1);
			m_attr1_new.GetComponent<UILabel>().text = get_treasure_string(m_treasure.template_id,m_treasure.enhance+num,1);
			if(sx_flag)
			{
				add_scale_anim(m_attr1_new);
			}
		}
		else
		{
			m_enhance.GetComponent<UILabel>().text = "[+" + m_treasure.enhance.ToString() + "]";
			m_powder.GetComponent<UILabel>().text = "----";;
			m_enhance_new.GetComponent<UILabel>().text = "----";
			m_bar1.GetComponent<UIProgressBar>().value = 1;
			m_bar_value = 1;
			m_exp_num.GetComponent<UILabel>().text = "--/--";
			m_enhance_old.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("czjh_sub.cs_21_55"),m_treasure.enhance.ToString () );//{0}级
			m_attr_old.GetComponent<UILabel>().text = get_treasure_string(m_treasure.template_id,m_treasure.enhance,0);
			m_attr1_old.GetComponent<UILabel>().text = get_treasure_string(m_treasure.template_id,m_treasure.enhance,1);
			m_attr_new.GetComponent<UILabel>().text = "---";
			m_attr1_new.GetComponent<UILabel>().text = "---";
		}
	
		m_enhance_button.GetComponent<UIButton>().isEnabled = true;
		m_yj_enhance.GetComponent<UIButton>().isEnabled = true;
		m_effect.SetActive(false);
	}

	public string get_atrr_name(string text)
	{
		string text1 = "";
		for(int i = 0;i < text.Length;++i)
		{
			if(text[i] == '+')
			{
				text1 = text.Substring(0,i).Replace(" ","");
				text1 += ":";
				return text1;
			}
		}
		return "";
	}

	string get_treasure_string(int id, int enhance_level,int i)
	{
		string attr = game_data._instance.get_t_language ("equip_qianghua_panel.cs_191_16");//强化已满
		s_t_baowu _treasure = game_data._instance.get_t_baowu (id);
		if (enhance_level <= game_data._instance.m_dbc_treasure_enhance.get_y() -1)
		{
			attr = treasure.get_treasure_attr_text(id,enhance_level,i);
		}
		return attr;
	}
	
	void IMessage.net_message(s_net_message message)
	{
	}
	
	void IMessage.message(s_message message)
	{
		if(message.m_type == "treasure_qianghua_effect_end" )
		{
			m_treasure_guids.Clear();
			for(int i = 0; i < m_treasures.Count;++i)
			{
				m_treasures[i].transform.GetComponent<BoxCollider>().enabled = true;
				sys._instance.remove_child(m_treasures[i]);
			}
			m_effect.SetActive(false);
			reset(m_treasure, m_out_message);
			
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
		}
		if(message.m_type == "common_devour1_treasure")
		{
			bool flag = false;
			sys._instance.remove_child(m_treasures[m_select_treasure]);
			for(int i = 0; i< sys._instance.m_select_treasures.Count && i < m_treasures.Count;++i)
			{
				flag = false;
				for(int j = 0; j < m_treasure_guids.Count;++j)
				{
					if(m_treasure_guids[j] == 0 && sys._instance.m_select_treasures[i] != 0)
					{
						flag = true;
						m_treasure_guids[j] = sys._instance.m_select_treasures[i];
						break;
					}
				}
				if(!flag)
				{
					m_treasure_guids.Add(sys._instance.m_select_treasures[i]);
				}
			}
			update_icon();
		}
		if(message.m_type == "treasure_yj_enhance")
		{
			for(int i = 0; i < message.m_long.Count;++i)
			{
				m_treasure_guids.Add((ulong)message.m_long[i]);
			}
			update_icon();
			sflag = false;
			int treasure_exp = get_exp();
			int num = level_num();
			int toltal_gold  = toltle_gold();
			if(toltal_gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
				m_treasure_guids.Clear();
				return;
			}
			update_button();
			s_t_baowu _treasure1 = game_data._instance.get_t_baowu (m_treasure.template_id);
			int value = m_treasure.enhance + 1;
			int exp = game_data._instance.get_treasure_enhance(value, _treasure1.font_color);
			if(num == 0)
			{
				m_treasure.enhance_exp += treasure_exp;
			}
			else
			{
				if(m_treasure.enhance +num >= sys._instance.m_self.m_t_player.level)
				{
					enhance_num = sys._instance.m_self.m_t_player.level - m_treasure.enhance;
					m_treasure.enhance = sys._instance.m_self.m_t_player.level;
				}
				else
				{
					enhance_num = num;
					m_treasure.enhance += num;
				}
				while((treasure_exp >= exp || m_treasure.enhance_exp + treasure_exp >= exp))
				{
					treasure_exp -=  exp;
					value += 1;
					exp = game_data._instance.get_treasure_enhance(value, _treasure1.font_color);
					if (m_treasure.enhance >= enhance_up)
					{
						m_treasure.enhance_exp = 0;
						treasure_exp = 0;
						break;
					}
					if (toltal_gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
					{
						break;
					}
				}
				m_treasure.enhance_exp +=  treasure_exp;
			}
			sys._instance.m_self.sub_att(e_player_attr.player_gold, toltal_gold,game_data._instance.get_t_language ("treasure_qianghua_gui.cs_415_71"));//宝物强化消耗
			m_treasure.enhance_counts += toltal_gold;
			for(int i = 0;i< m_treasure_guids.Count;++i)
			{
				if(m_treasure_guids[i] != 0)
				{
					sys._instance.m_self.remove_treasure(m_treasure_guids[i]);
				}
			}
			check();
			m_effect.SetActive(true);
			m_enhance_button.GetComponent<UIButton>().isEnabled = false;
			m_yj_enhance.GetComponent<UIButton>().isEnabled = false;
			sys._instance.play_sound ("sound/qianghua");
		}
	}

	int level_num()
	{
		int treasure_exp = 0;
		int num = 0;
		int value = m_treasure.enhance+1;
		s_t_baowu _treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		treasure_exp = get_exp();
		int exp = game_data._instance.get_treasure_enhance(value,_treasure.font_color) - m_treasure.enhance_exp;
		int max = treasure_exp ;
		while( max >= exp && exp != 0)
		{
			value = value + 1;
			num ++;
			if(value > enhance_up)
			{
				break;
			}
			max -=  exp;
			exp = game_data._instance.get_treasure_enhance(value,_treasure.font_color);
		}
		return num;
	}

	int toltle_gold()
	{
		int toltal_gold = 0;
		int num = level_num ();
		s_t_baowu _treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		int treasure_exp = get_exp ();
		int _num = sys._instance.m_self.m_t_player.level;
		if(_num > (game_data._instance.m_dbc_treasure_enhance.get_y() - 1))
		{
			_num = game_data._instance.m_dbc_treasure_enhance.get_y() - 1;
		}
		if(m_treasure.enhance + num >= _num)
		{
			toltal_gold = game_data._instance.get_total_treasure_enhance(_num , _treasure.font_color)
				- game_data._instance.get_total_treasure_enhance(m_treasure.enhance,_treasure.font_color) - m_treasure.enhance_exp;
		}
		else
		{
			toltal_gold = treasure_exp;
		}
		return toltal_gold;
	}

	void update_icon()
	{
		for(int i =0;i < m_treasure_guids.Count && i < m_treasures.Count;++i)
		{
			if(m_treasure_guids[i] == 0)
			{
				continue;
			}
			sys._instance.remove_child(m_treasures[i]);
			GameObject _icon = icon_manager._instance.create_treasure_icon(m_treasure_guids[i]);
			_icon.transform.GetComponent<BoxCollider>().enabled = false;
			_icon.transform.parent = m_treasures[i].transform;
			_icon.transform.localPosition = new Vector3(0,0,0);
			_icon.transform.localScale = new Vector3(1,1,1);
		}
		reset(m_treasure, m_out_message);
	}
	
	// Update is called once per frame
	void Update () {
		float _value = m_bar1.GetComponent<UIProgressBar>().value;
		if(_value != m_bar_value)
		{
			float _add = Time.deltaTime;
			
			if(Mathf.Abs(m_bar_value - _value) > _add)
			{
				if(_value < m_bar_value)
				{
					_value += _add;
				}
				else
				{
					_value -= _add;
				}
			}
			else
			{
				_value = m_bar_value;
			}
			m_bar1.GetComponent<UIProgressBar>().value = _value;
		}
	
	}
	
	void check()
	{

		protocol.game.cmsg_treasure_enhance _msg = new protocol.game.cmsg_treasure_enhance ();
		for(int i =0 ; i <m_treasure_guids.Count;++i)
		{
			if(m_treasure_guids[i] != 0)
			{
				_msg.treasure_guids.Add(m_treasure_guids[i]);
			}
		}
		_msg.enhance_guid = m_guids[count];
		m_qianghua_guids.Add(m_guids[count]);
		net_http._instance.send_msg<protocol.game.cmsg_treasure_enhance> (opclient_t.CMSG_TREASURE_ENHANCE, _msg);
		sys._instance.m_self.m_t_player.bweh_task_num += enhance_num;
		sys._instance.m_self.add_active(1700, 1);
		sys._instance.m_self.check_target_done();
		sflag = false;
		enhance_num = 0;
		if (m_treasure.role_guid != 0) 
		{
			ccard m_card = sys._instance.m_self.get_card_guid (m_treasure.role_guid);
			string s = sys._instance.m_self.gongzhengs_ex(m_card,m_gongzhens);
			if(s != "")
			{
				root_gui._instance.show_prompt_dialog_box(s);
			}
		}
	}

	void update_button()
	{
		int i = 0;
		for(i = 0; i< m_guids.Count;++i)
		{
			if(m_guids[i] == m_treasure.guid)
			{
				break;
			}
		}
		count = i;
		if(i >= m_guids.Count - 1 )
		{
			m_right.SetActive(false);
		}
		else 
		{
			m_right.SetActive(true);
		}
		
		if(i <= 0)
		{
			m_left.SetActive(false);
		}
		else
		{
			m_left.SetActive(true);
		}
	}
	
	int add_cur(int cur)
	{
		
		count += cur;
		
		if(count >= m_guids.Count - 1 )
		{
			m_right.SetActive(false);
		}
		else 
		{
			m_right.SetActive(true);
		}
		
		if(count <= 0)
		{
			count = 0;
			m_left.SetActive(false);
		}
		else
		{
			m_left.SetActive(true);
		}
		return count;
	}
	
	public void select_treasure(GameObject obj)
	{
		m_select_treasure = int.Parse(obj.transform.name);
		sflag = false;
		s_t_baowu _treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		if(m_treasure.enhance >= sys._instance.m_self.m_t_player.level)
		{
			string s = game_data._instance.get_t_language ("chong_neng_gui.cs_258_12");//玩家等级不足
			root_gui._instance.show_prompt_dialog_box("[ffc882]" + s);
			return;
		}
		if(m_treasure.enhance >= game_data._instance.m_dbc_treasure_enhance.get_y() - 1)
		{
			string s = game_data._instance.get_t_language ("treasure_qianghua_gui.cs_622_14");//[ffc882]饰品强化已满
			root_gui._instance.show_prompt_dialog_box(s);
			return;
		}
		if(obj.transform.childCount != 1)
		{
			List<ulong > m_hide_guids = new List<ulong >();
			int count = 0;
			for(int i = 0; i< m_treasure_guids.Count;++i)
			{
				m_hide_guids.Add(m_treasure_guids[i]);
			}
			for(int i = 0; i< m_treasures.Count;++i)
			{
				if(m_treasures[i].transform.childCount == 0)
				{
					count ++;
				}
			}
			string text = game_data._instance.get_t_language ("treasure_qianghua_gui.cs_641_17");//选择饰品进行强化
			m_hide_guids.Add(m_treasure.guid);
			root_gui._instance.show_common_treasure_panel (text, false, false, 0, m_hide_guids, "common_devour_treasure", true,2,this.gameObject,count);
			this.GetComponent<ui_show_anim>().hide_ui();
		}
		else
		{
			m_treasure_guids[m_select_treasure] = 0;
			sys._instance.remove_child(m_treasures[m_select_treasure]);
			reset (sys._instance.m_self.get_treasure_guid(m_guids[count]),m_out_message);
		}
	}

	public static int comp(dhc.treasure_t x, dhc.treasure_t y)
	{
		s_t_baowu t_treasure_x = game_data._instance.get_t_baowu(x.template_id);
		s_t_baowu t_treasure_y = game_data._instance.get_t_baowu(y.template_id);
		if (t_treasure_x.type == 5 && t_treasure_y.type != 5)
		{
			return -1;
		}
		else if (t_treasure_x.type != 5 && t_treasure_y.type == 5)
		{
			return 1;
		}
		else if (t_treasure_x.exp < t_treasure_y.exp)
		{
			return -1;
		}
		else if (t_treasure_x.exp > t_treasure_y.exp)
		{
			return 1;
		}
		else if (x.jilian < y.jilian)
		{
			return -1;
		}
		else if (x.jilian > y.jilian)
		{
			return 1;
		}
		else if (x.enhance < y.enhance)
		{
			return -1;
		}
		else if (x.enhance > y.enhance)
		{
			return 1;
		}
		else if (t_treasure_x.id < t_treasure_y.id)
		{
			return -1;
		}
		else if (t_treasure_x.id > t_treasure_y.id)
		{
			return 1;
		}
		
		return 0; 
	}

	int get_exp()
	{
		int treasure_exp = 0;
		for(int i = 0; i < m_treasure_guids.Count;++i)
		{
			if(m_treasure_guids[i] == 0)
			{
				continue;
			}
			dhc.treasure_t t_treasure = sys._instance.m_self.get_treasure_guid(m_treasure_guids[i]);
			s_t_baowu _treasure = game_data._instance.get_t_baowu (t_treasure.template_id);
			treasure_exp += _treasure.exp;
			treasure_exp +=  t_treasure.enhance_exp;
			treasure_exp += game_data._instance.get_total_treasure_enhance(t_treasure.enhance, _treasure.font_color);
			if(t_treasure.jilian > 0)
			{
				treasure_exp +=  _treasure.exp * t_treasure.jilian;
			}
		}
		return treasure_exp;
	}

	int get_enhance_exp()
	{
		int enhance_exp = m_treasure.enhance_exp;
		s_t_baowu _treasure1 = game_data._instance.get_t_baowu (m_treasure.template_id);
		int value = m_treasure.enhance + 1;
		int max_enhace = Mathf.Min (sys._instance.m_self.m_t_player.level,(game_data._instance.m_dbc_treasure_enhance.get_y() -1));
		if(m_treasure.enhance +1 >= max_enhace)
		{
			value = max_enhace;
		}
		int exp = game_data._instance.get_treasure_enhance(value, _treasure1.font_color);
		int treasure_exp = get_exp();
		int num = level_num();
		if(num == 0)
		{
			if(m_treasure.enhance +num >= sys._instance.m_self.m_t_player.level)
			{
				enhance_exp = 0;
			}
			else if(m_treasure.enhance +num >= game_data._instance.m_dbc_treasure_enhance.get_y() -1)
			{
				enhance_exp = exp;
			}
			else
			{
				enhance_exp += treasure_exp;
			}
		}
		else
		{
			while((treasure_exp >= exp || m_treasure.enhance_exp + treasure_exp >= exp))
			{
				if (value >= max_enhace)
				{
					break;
				}
				treasure_exp -=  exp;
				value += 1;
				exp = game_data._instance.get_treasure_enhance(value, _treasure1.font_color);
			}
			enhance_exp +=  treasure_exp;
		}
		return enhance_exp;
	}

	public void click(GameObject obj)
	{
		if (obj.name == "close")
		{
			m_qianghua_guids.Add(m_treasure.guid);
			s_message _msg = new s_message();
			_msg.m_type = m_out_message;
            _msg.m_ints.Add(5);
			cmessage_center._instance.add_message(_msg);
			m_treasure_guids.Clear();
			for(int i = 0; i < m_treasures.Count;++i)
			{
				sys._instance.remove_child(m_treasures[i]);
			}
			Object.Destroy(this.gameObject);
		}
		if (obj.name == "enhance")
		{
			int a = 0;
			if(error != "")
			{
				string s = "[ffc882]" + error;
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			for(int i = 0; i < m_treasures.Count;i++)
			{
				if(m_treasures[i].transform.childCount == 0)
				{
					a++;
				}
			}
			if(a == m_treasures.Count)
			{
				string s = "[ffc882]" + game_data._instance.get_t_language ("treasure_qianghua_gui.cs_641_17");//选择饰品进行强化
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			sflag = false;
			int treasure_exp = get_exp();
			int num = level_num();
			int toltal_gold  = toltle_gold();
			if(toltal_gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
				m_treasure_guids.Clear();
				return;
			}
			update_button();
			for(int i = 0; i < m_treasures.Count;++i)
			{
				m_treasures[i].transform.GetComponent<BoxCollider>().enabled = false;
			}
			s_t_baowu _treasure1 = game_data._instance.get_t_baowu (m_treasure.template_id);
			int value = m_treasure.enhance + 1;
			int exp = game_data._instance.get_treasure_enhance(value, _treasure1.font_color);
			if(num == 0)
			{
				m_treasure.enhance_exp += treasure_exp;
			}
			else
			{
				if(m_treasure.enhance +num >= sys._instance.m_self.m_t_player.level)
				{
					enhance_num = sys._instance.m_self.m_t_player.level - m_treasure.enhance;
					m_treasure.enhance = sys._instance.m_self.m_t_player.level;
				}
				else
				{
					enhance_num = num;
					m_treasure.enhance += num;
				}
				while((treasure_exp >= exp || m_treasure.enhance_exp + treasure_exp >= exp))
				{
					treasure_exp -=  exp;
					value += 1;
					exp = game_data._instance.get_treasure_enhance(value, _treasure1.font_color);
					if (m_treasure.enhance >= enhance_up)
					{
						m_treasure.enhance_exp = 0;
						treasure_exp = 0;
						break;
					}
					if (toltal_gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
					{
						break;
					}
				}
				m_treasure.enhance_exp +=  treasure_exp;
			}
			sys._instance.m_self.sub_att(e_player_attr.player_gold, toltal_gold,game_data._instance.get_t_language ("treasure_qianghua_gui.cs_415_71"));//宝物强化消耗
			m_treasure.enhance_counts += toltal_gold;
			for(int i = 0;i< m_treasure_guids.Count;++i)
			{
				if(m_treasure_guids[i] != 0)
				{
					sys._instance.m_self.remove_treasure(m_treasure_guids[i]);
				}
			}
			check();
			m_effect.SetActive(true);
			m_enhance_button.GetComponent<UIButton>().isEnabled = false;
			sys._instance.play_sound ("sound/qianghua");
		}
		if (obj.name == "enhance_ex")
		{
			List<dhc.treasure_t> m_fit_treasures = new List<dhc.treasure_t>();
			for(int i = 0; i < sys._instance.m_self.m_t_player.treasures.Count;++i)
			{
				dhc.treasure_t t_treasure = sys._instance.m_self.get_treasure_guid(sys._instance.m_self.m_t_player.treasures[i]);
				s_t_baowu _treasure = game_data._instance.get_t_baowu(t_treasure.template_id);
				if(t_treasure.role_guid > 0 || t_treasure.guid == m_treasure.guid || t_treasure.locked ==1
				   || t_treasure.jilian >0 || t_treasure.star > 0 || t_treasure.star_exp > 0)
				{
					continue;
				}
				m_fit_treasures.Add(t_treasure);
			}
			if(m_fit_treasures.Count <= 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_qianghua_gui.cs_889_46"));//[ffc882]没有符合条件的饰品用于强化
				return;
			}
			m_treasure_guids.Clear();
			for(int i = 0; i < m_treasures.Count;++i)
			{
				sys._instance.remove_child(m_treasures[i]);
			}
			sflag = false;
			reset(m_treasure, m_out_message,false);
			m_num_gui.GetComponent<treasure_fast_gui>().m_treasure = m_treasure;
			m_num_gui.GetComponent<treasure_fast_gui>().m_select = 0;
			m_num_gui.GetComponent<treasure_fast_gui>().m_input_num = 0;
			m_num_gui.GetComponent<treasure_fast_gui>().reset();
			m_num_gui.SetActive(true);
		}
		if(obj.name == "right")
		{
			add_cur(1);
			sflag = true;
			m_treasure_guids.Clear();
			for(int i = 0; i < m_treasures.Count;++i)
			{
				sys._instance.remove_child(m_treasures[i]);
			}
			m_treasure = sys._instance.m_self.get_treasure_guid(m_guids[count]);
			reset (m_treasure,m_out_message);
		}
		if(obj.name == "left")
		{
			add_cur(-1);
			sflag = true;
			m_treasure_guids.Clear();
			for(int i = 0; i < m_treasures.Count;++i)
			{
				sys._instance.remove_child(m_treasures[i]);
			}
			m_treasure = sys._instance.m_self.get_treasure_guid(m_guids[count]);
			reset (m_treasure,m_out_message);
		}
		if(obj.name == "bq")
		{
			int a = 0;
			int treasure_exp = 0;
			sflag = false;
			s_t_baowu _t_baowu = game_data._instance.get_t_baowu (m_treasure.template_id);
			if(m_treasure.enhance >= sys._instance.m_self.m_t_player.level)
			{
				string s = game_data._instance.get_t_language ("chong_neng_gui.cs_258_12");//玩家等级不足
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + s);
				return;
			}
			if(m_treasure.enhance >= game_data._instance.m_dbc_treasure_enhance.get_y() -1)
			{
				string s = game_data._instance.get_t_language ("treasure_qianghua_gui.cs_622_14");//[ffc882]饰品强化已满
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			m_baowus.Clear();
			for(int i = 0; i < sys._instance.m_self.m_t_player.treasures.Count;++i)
			{
				dhc.treasure_t t_treasure = sys._instance.m_self.get_treasure_guid(sys._instance.m_self.m_t_player.treasures[i]);
				s_t_baowu _treasure = game_data._instance.get_t_baowu(t_treasure.template_id);
				if((_treasure.font_color >= 3 && _treasure.type != 5) || t_treasure.role_guid>0 || t_treasure.guid == m_treasure.guid || t_treasure.locked ==1
				   || t_treasure.jilian != 0 || t_treasure.star > 0 || t_treasure.star_exp > 0)
				{
					continue;
				}
				m_baowus.Add(t_treasure);
			}
			m_baowus.Sort(comp);
			if(m_baowus.Count == 0)
			{
				string s = game_data._instance.get_t_language ("treasure_qianghua_gui.cs_962_15");//[ffc882]无饰品可以选择
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if(m_treasure.enhance >= sys._instance.m_self.m_t_player.level)
			{
				return;
			}
			for(int i = 0; i < m_baowus.Count;++i)
			{
				int id = m_baowus[i].template_id;
				int num = level_num();
				s_t_baowu t_treasure = game_data._instance.get_t_baowu(id);
				if( a >= m_treasures.Count || m_treasure.enhance+num >= enhance_up)
				{
					break;
				}
				if(t_treasure == null)
				{
					continue;
				}
				if(!m_treasure_guids.Contains(m_baowus[i].guid))
				{
					for(a= 0;a < m_treasures.Count;++a)
					{
						if(m_treasures[a].transform.childCount == 0)
						{
							s_t_baowu _treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
							int has_exp = game_data._instance.get_total_treasure_enhance(m_treasure.enhance,_treasure.font_color); 
							int exp = game_data._instance.get_total_treasure_enhance(enhance_up,_treasure.font_color) - has_exp;
							if(m_treasure_guids.Count-1 < a)
							{
								m_treasure_guids.Add(m_baowus[i].guid);
							}
							else
							{
								m_treasure_guids[a] = m_baowus[i].guid;
							}
							treasure_exp = get_exp();
							sys._instance.remove_child(m_treasures[a]);
							GameObject _icon = icon_manager._instance.create_treasure_icon(m_baowus[i].guid);
							_icon.transform.GetComponent<BoxCollider>().enabled = false;
							_icon.transform.parent = m_treasures[a].transform;
							_icon.transform.localPosition = new Vector3(0,0,0);
							_icon.transform.localScale = new Vector3(1,1,1);
							if(treasure_exp + m_treasure.enhance_exp > exp)
							{
								reset (sys._instance.m_self.get_treasure_guid(m_guids[count]),m_out_message);
								return;
							}
							break;
						}
					}
				}
			}
			reset (sys._instance.m_self.get_treasure_guid(m_guids[count]),m_out_message);
		}
	}
}
