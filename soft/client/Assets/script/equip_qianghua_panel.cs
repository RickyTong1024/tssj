
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip_qianghua_panel : MonoBehaviour,IMessage {
	
	dhc.equip_t m_equip;
	bool need_update = false;
	int enhance_num = 0;
	public GameObject m_jt;
	float m_jt_x = 0;
	public GameObject m_effect;
	private string m_out_message;
	public List<ulong> m_guids= new List<ulong>();
	public List <ulong> m_qianghua_guids = new List<ulong>();
	public List<int> m_gongzhengs = new List<int>();
	private Vector3 m_mouse_pos;
	private int count =0;
	public GameObject m_right;
	public GameObject m_left;
	public GameObject m_gold;
	public GameObject m_level;
	private bool sflag = false;
	private string error = "";


	public UILabel m_xuyaozhujiao;
	public UILabel m_qianghuafeiyong;
	public UILabel m_dangqianyongyou;
	public UILabel m_desc;
	// Use this for initialization
	void Start () 
	{
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
			if(m_equip.guid == m_guids[i])
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
		if (m_equip.role_guid != 0) 
		{
			ccard m_card = sys._instance.m_self.get_card_guid (m_equip.role_guid);
			sys._instance.m_card = m_card;
			sys._instance.m_gongzhengs.Clear();
			sys._instance.m_gongzhengs = gongzheng_mubiao_gui.gongzhen_cur (m_card);
		}
	}
	public void OnDisable()
	{
		check (0);
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
				reset (sys._instance.m_self.get_equip_guid(m_guids[count]),m_out_message);
				check(-_add);
			}
		}
	}


	public void reset(dhc.equip_t eequip, string out_message) 
	{
		m_equip = eequip;
		if (m_equip.role_guid != 0) 
		{
			ccard m_card = sys._instance.m_self.get_card_guid (m_equip.role_guid);
			m_gongzhengs.Clear();
			m_gongzhengs = gongzheng_mubiao_gui.gongzhen_cur (m_card);
		}
		error = "";
		m_out_message = out_message;
		sys._instance.remove_child (transform.Find("center").Find("old_equip").gameObject);
		GameObject _icon = icon_manager._instance.create_equip_icon(m_equip);
		_icon.transform.parent = this.transform.Find("center").Find("old_equip");
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
		if(sflag)
		{
			sys._instance.add_pos_anim(_icon,0.3f, new Vector3(0, 60, 0), 0.05f);
			sys._instance.add_alpha_anim(_icon,0.3f, 0, 1.0f, 0.05f);
		}
		_icon.transform.GetComponent<BoxCollider>().enabled = false;
        int max_enchance1;
		max_enchance1 = game_data._instance.m_dbc_enhance.get_y() - 1;
		s_t_equip _equip = game_data._instance.get_t_equip (m_equip.template_id);
		transform.Find("center").Find("name").GetComponent<UILabel>().text = _equip.name ;
		transform.Find("center").Find("equip_enhance").GetComponent<UILabel>().text = "[+" + m_equip.enhance.ToString () + "]";
		transform.Find("center").Find("old/old_enhance").GetComponent<UILabel>().text = "+" + m_equip.enhance.ToString();
		transform.Find("center").Find("new/new_enhance").GetComponent<UILabel>().text = "+" + (m_equip.enhance + 1).ToString();
		transform.Find("center").Find("old/old_attr").GetComponent<UILabel>().text = get_equip_string(m_equip.template_id, m_equip.enhance, m_equip.star);
		transform.Find("center").Find("new/new_attr").GetComponent<UILabel>().text = get_equip_string(m_equip.template_id, m_equip.enhance + 1, m_equip.star);
		if (m_equip.enhance < max_enchance1)
		{
			int gold = game_data._instance.get_enhance (m_equip.enhance + 1, _equip.font_color);
			string s = sys._instance.get_res_color(1);
			if (gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
			{
				error = game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
				s = "[ff0000]";
			}
			m_gold.GetComponent<UILabel>().text = s + gold.ToString();
			s = "";
            int max_enchance;
            max_enchance = Mathf.Min(int.Parse(game_data._instance.m_dbc_enhance.get_index(0, game_data._instance.m_dbc_enhance.get_y() - 1)),sys._instance.m_self.m_t_player.level);
            if (m_equip.enhance >= max_enchance)
            {
				error = game_data._instance.get_t_language ("equip_qianghua_panel.cs_163_12");//已到达强化上限
                s = "[ff0000]";
            }
            int level = sys._instance.m_self.m_t_player.level;
			if(m_equip.enhance >= level)
            {
				error = game_data._instance.get_t_language ("bag_gui.cs_2039_75");//等级不足
            }

           m_level.GetComponent<UILabel>().text = m_equip.enhance + "/" + max_enchance;

		}
		else
		{
			m_gold.GetComponent<UILabel>().text = "----";
			m_level.GetComponent<UILabel>().text = "----";
			transform.Find("center").Find("new/new_attr").GetComponent<UILabel>().text = "----";
			error = game_data._instance.get_t_language ("equip_qianghua_panel.cs_163_12");//已到达强化上限
		}
		m_effect.SetActive(false);
		transform.Find("center").Find("enhance").GetComponent<UIButton>().isEnabled = true;
		transform.Find("center").Find("enhance_ex").GetComponent<UIButton>().isEnabled = true;
	}

	string get_equip_string(int id, int enhance_level, int star)
	{
		string attr = game_data._instance.get_t_language ("equip_qianghua_panel.cs_191_16");//强化已满
		s_t_equip _equip = game_data._instance.get_t_equip (id);

		//if (enhance_level <= _equip.enhance_up)
		{
			attr = equip.get_equip_value(id, enhance_level, star);
		}
		return attr;
	}

	void IMessage.net_message(s_net_message message)
	{
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "qianghua_effect_end")
		{
			m_effect.SetActive(false);
			reset(m_equip, m_out_message);

			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
		}
	}
	
	// Update is called once per frame
	void Update () {
		m_jt_x += Time.deltaTime * 100;
		if (m_jt_x > 60)
		{
			m_jt_x -= (int)m_jt_x / 60 * 60;
		}
		m_jt.transform.localPosition = new Vector3 (m_jt_x, 0, 0);
	}

	void check(int add)
	{
		if (need_update)
		{
			protocol.game.cmsg_equip_enhance _msg = new protocol.game.cmsg_equip_enhance ();
			int num = count + add; 
			if(num <= 0)
			{
				num = 0;
			}
			if(count + add >= m_guids.Count)
			{
				num = m_guids.Count;
			}
			_msg.equip_guid = m_guids[num];
			_msg.enhance_num = enhance_num;
			m_qianghua_guids.Add(m_guids[num]);
			net_http._instance.send_msg<protocol.game.cmsg_equip_enhance> (opclient_t.CMSG_EQUIP_ENHANCE, _msg);
			sys._instance.m_self.m_t_player.qh_task_num += enhance_num;
			sys._instance.m_self.add_active(600, enhance_num);
			sys._instance.m_self.check_target_done();
			need_update = false;
			sflag = false;
			enhance_num = 0;
		}
	}

	int add_cur(int cur)
	{

		count += cur;
			
		if(count >= m_guids.Count - 1)
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

	public void click(GameObject obj)
	{
		if (obj.name == "close")
		{
			this.transform.GetComponent<ui_show_anim>().hide_ui();
			m_qianghua_guids.Add(m_equip.guid);
			s_message _msg = new s_message();
			_msg.m_type = m_out_message;
            _msg.m_ints.Add(2);
			for(int i = 0; i < m_qianghua_guids.Count;++i)
			{
				_msg.m_long.Add(m_qianghua_guids[i]);
			}
			cmessage_center._instance.add_message(_msg);
		}
		if (obj.name == "enhance")
		{
			if(error != "")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + error);
				return;
			}
			need_update = true;
			sflag = false;
			enhance_num++;

			s_t_equip _equip = game_data._instance.get_t_equip (m_equip.template_id);
			int gold = game_data._instance.get_enhance (m_equip.enhance + 1, _equip.font_color);
			sys._instance.m_self.sub_att(e_player_attr.player_gold, gold,game_data._instance.get_t_language ("bu_zheng_panel.cs_975_65"));//装备强化消耗
			m_equip.enhance++;
			if (m_equip.role_guid != 0) 
			{
				ccard m_card = sys._instance.m_self.get_card_guid (m_equip.role_guid);
				string s = sys._instance.m_self.gongzhengs_ex(m_card,m_gongzhengs);
				if(s != "")
				{
					root_gui._instance.show_prompt_dialog_box(s);
				}
			}
			m_effect.SetActive(true);
			transform.Find("center").Find("enhance").GetComponent<UIButton>().isEnabled = false;
            transform.Find("center").Find("enhance_ex").GetComponent<UIButton>().isEnabled = false;
			sys._instance.play_sound ("sound/qianghua");
		}
		if (obj.name == "enhance_ex")
		{
			if(error != "")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + error);
				return;
			}
			need_update = true;
			sflag = false;
			s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
			int t = 10;
			while (t > 0)
			{
				t--;  
				enhance_num++;
				s_t_equip _equip = game_data._instance.get_t_equip (m_equip.template_id);
				int gold = game_data._instance.get_enhance (m_equip.enhance + 1, _equip.font_color);
				sys._instance.m_self.sub_att(e_player_attr.player_gold, gold,game_data._instance.get_t_language ("bu_zheng_panel.cs_975_65"));//装备强化消耗
				m_equip.enhance++;

                //if (m_equip.enhance >= t_equip.enhance_up)
                //{
                //    break;
                //}
                int max_enchance;
                max_enchance =Mathf.Min(int.Parse(game_data._instance.m_dbc_enhance.get_index(0, game_data._instance.m_dbc_enhance.get_y() - 1)),sys._instance.m_self.m_t_player.level);
				gold = game_data._instance.get_enhance (m_equip.enhance + 1, _equip.font_color);
				if (gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
				{
					break;
				}
                if (m_equip.enhance >= max_enchance)
                {
                    break;
                }
			}
			if (m_equip.role_guid != 0) 
			{
				ccard m_card = sys._instance.m_self.get_card_guid (m_equip.role_guid);
				string s = sys._instance.m_self.gongzhengs_ex(m_card,m_gongzhengs);
				if(s != "")
				{
					root_gui._instance.show_prompt_dialog_box( s);
				}
			}
			m_effect.GetComponent<Animator>().speed=2.0f;
			m_effect.SetActive(true);
            transform.Find("center").Find("enhance").GetComponent<UIButton>().isEnabled = false;
            transform.Find("center").Find("enhance_ex").GetComponent<UIButton>().isEnabled = false;
			sys._instance.play_sound ("sound/qianghua");
		}
		if(obj.name == "right")
		{
			sflag = true;
			add_cur(1);
			reset (sys._instance.m_self.get_equip_guid(m_guids[count]),m_out_message);
			check(-1);

		}
		if(obj.name == "left")
		{
			sflag = true;
			add_cur(-1);
			reset (sys._instance.m_self.get_equip_guid(m_guids[count]),m_out_message);
			check(1);

		}
	}
}
