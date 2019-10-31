
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip_gaizao_panel : MonoBehaviour, IMessage {
	
	dhc.equip_t m_equip;

	public GameObject m_effect;
	public GameObject m_itemcount;
	public GameObject m_center;
	public bool m_play = false;
	private string m_out_message;
	public GameObject[] m_lock;
	public GameObject[] m_unlock;
	public GameObject[] m_sx;
	public int _itemcount;
	public List<GameObject>m_jp_atts = new List<GameObject>();
	public List<GameObject> m_atts = new List<GameObject>();
	public UILabel m_gaizhaoshicur;
	public GameObject m_num_buy;
	public UILabel[] m_level;
	bool m_ten = false; 
	int m_ten_num;

	
	public UILabel m_jipinyulan;
	public UILabel m_gaizaoshuxing;
	public UILabel m_suodingxiaohao;
	public UILabel m_dangqianyongyou;
	public UILabel m_desc;
	public UILabel m_sx1;
	public UILabel m_gaizaolv;
	public UILabel m_tishi;
	private int m_max_level = 120;

	// Use this for initialization
	void Start () 
	{
	
			cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void reset(dhc.equip_t _equip, string out_message, bool is_clear = false)
	{
		m_out_message = out_message;
		GameObject _main = m_center.transform.Find("main").gameObject;
		m_equip = _equip;
		GameObject icon0 = _main.transform.Find("icon0").gameObject;
		sys._instance.remove_child (icon0);
		GameObject icon1 = _main.transform.Find("icon1").gameObject;
		sys._instance.remove_child (icon1);
		GameObject icon2 = _main.transform.Find("icon2").gameObject;
		sys._instance.remove_child (icon2);
		if (m_equip.role_guid != 0) 
		{
			sys._instance.m_sxs.Clear ();
			ccard m_card = sys._instance.m_self.get_card_guid (m_equip.role_guid);
			sys._instance.m_sxs = sys._instance.m_self.get_gongzhen(m_card);
			sys._instance.m_gongzhengs.Clear();
			sys._instance.m_gongzhengs = gongzheng_mubiao_gui.gongzhen_cur (m_card);
			sys._instance.m_card = m_card;
		}
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		s_t_gaizao _gaizao = game_data._instance.get_gaizao(t_equip.font_color);
		_itemcount = _gaizao.m_0;
		int num1 = 0 ;
		for(int i = 0;i < m_atts.Count;i ++)
		{
			if(is_lock(i))
			{
				num1++;
			}
		}
		switch(num1)
		{
		case 1:
			_itemcount += _gaizao.m_1;
			break;
		case 2:
			_itemcount += _gaizao.m_2;
			break;
		case 3:
			_itemcount += _gaizao.m_3;
			break;
		case 4:
			_itemcount += _gaizao.m_4;
			break;
		}
		if (is_clear)
		{
			_itemcount = _gaizao.m_0;
		}
		int inum = sys._instance.m_self.get_item_num((uint)_gaizao.item_id);
		m_itemcount.GetComponent<UILabel>().text = "x" + (_itemcount - _gaizao.m_0);
		string color = "";
		if(inum >= _itemcount)
		{
			color = "[42ffff]";
		}
		else
		{
			color = "[ff0000]";
		}
		m_gaizhaoshicur.text = color + "x" + sys._instance.m_self.get_item_num((uint)_gaizao.item_id);
        GameObject iicon0 = icon_manager._instance.create_item_icon_ex(_gaizao.item_id, inum, _gaizao.m_0);
		iicon0.transform.parent = icon1.transform;
		iicon0.transform.localPosition = new Vector3(0,0,0);
		iicon0.transform.localScale = new Vector3(1,1,1);
		UIButtonMessage[] message = iicon0.GetComponents<UIButtonMessage>();
		message[0].target = this.gameObject;
		message[0].functionName = "click_item_icon";
		message[1].target = null;
		message[1].functionName = "";
		message[2].target = null;
		message[2].functionName = "";

		GameObject iicon3 = icon_manager._instance.create_equip_icon(m_equip);
		iicon3.transform.parent = _main.transform.Find("icon0");
		iicon3.transform.localPosition = new Vector3(0,0,0);
		iicon3.transform.localScale = new Vector3(1,1,1);
		iicon3.transform.GetComponent<BoxCollider>().enabled = false;

		GameObject iicon4 = icon_manager._instance.create_equip_icon(m_equip.template_id,m_max_level);
		iicon4.transform.parent = _main.transform.Find("icon2");
		iicon4.transform.localPosition = new Vector3(0,0,0);
		iicon4.transform.localScale = new Vector3(1,1,1);
		iicon4.transform.GetComponent<BoxCollider>().enabled = false;
		s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx (t_equip.font_color, m_equip.star);
		for(int i = 0; i < m_atts.Count;++i)
		{
			if (is_clear)
			{
				m_atts[i].transform.Find("lock").gameObject.SetActive(false);
			}
			if(i < m_equip.rand_ids.Count)
			{
				m_atts[i].GetComponent<BoxCollider>().enabled = true;
				m_atts[i].transform.Find("sx").GetComponent<UILabel>().text = equip.get_equip_random_value2(m_equip.rand_values[i], m_equip.rand_ids[i],t_equip,m_equip);
				m_atts[i].transform.Find("max").GetComponent<UILabel>().text = equip.get_equip_random_max( m_equip.rand_ids[i],t_equip,m_equip);
				m_atts[i].transform.Find("level").GetComponent<UILabel>().text =equip.get_equip_random_color(m_equip.rand_values[i], m_equip.rand_ids[i],t_equip,m_equip) 
					+  "Lv " + m_equip.rand_values[i] * 100/ equip.get_equip_random_max1( m_equip.rand_ids[i],t_equip,m_equip,m_max_level);
				m_atts[i].transform.Find("levelmax").GetComponent<UILabel>().text = "Lv " + 
					equip.get_equip_random_levelmax ( m_equip.rand_ids[i],t_equip,m_equip) * 100/equip.get_equip_random_max1( m_equip.rand_ids[i],t_equip,m_equip,m_max_level);
			}
			else
			{
				m_atts[i].GetComponent<BoxCollider>().enabled = false;
				m_atts[i].transform.Find("sx").GetComponent<UILabel>().text = "[2299ff]????????";
				m_atts[i].transform.Find("max").GetComponent<UILabel>().text = "";
				m_atts[i].transform.Find("level").GetComponent<UILabel>().text = "";
				m_atts[i].transform.Find("levelmax").GetComponent<UILabel>().text = "";
			}
		}


		for(int i = 0; i < m_jp_atts.Count;++i)
		{
			int num =0;
			if(t_equip_sx != null)
			{
				num = (int)(t_equip.eeattr[i].value2 + t_equip.eeattr[i].value2 * t_equip_sx.enhance_rate * m_max_level);
			}
			else
			{
				num = (int)(t_equip.eeattr[i].value2);
			}
			string attr =  "[ffff00]" + game_data._instance.get_value_string( t_equip.eeattr[i].attr, num);
			m_jp_atts[i].transform.Find("sx").GetComponent<UILabel>().text = attr;
		}
        
		s_t_equip _t_equip = game_data._instance.get_t_equip (m_equip.template_id);

		_main.transform.Find("name").GetComponent<UILabel>().text = _t_equip.name;

		m_ten = false;
		m_ten_num = 0;
		m_play = false;
		m_effect.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (m_play && isplay() )
		{
			m_play = false;
			s_message _mes = new s_message();
			_mes.m_type = "refresh_gaizhaoshi";
			cmessage_center._instance.add_message(_mes);
			if(m_ten)
			{
				root_gui._instance.show_single_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),string.Format(game_data._instance.get_t_language ("equip_gaizao_panel.cs_198_65") , m_ten_num ),_mes);//提示//一键改造完成，共进行了{0}次改造
			}
			reset(m_equip, m_out_message);

			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);

		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_EQUIP_GAIZAO )
		{
			protocol.game.smsg_equip_gaizao _msg = net_http._instance.parse_packet<protocol.game.smsg_equip_gaizao> (message.m_byte);
			m_equip.rand_ids.Clear();
			m_equip.rand_values.Clear();
			for (int i = 0; i < _msg.equip.rand_ids.Count; ++i)
			{
				m_equip.rand_ids.Add(_msg.equip.rand_ids[i]);
				m_equip.rand_values.Add(_msg.equip.rand_values[i]);
			}

			s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
			s_t_gaizao _gaizao = game_data._instance.get_gaizao(t_equip.font_color);
			int _itemcount = _gaizao.m_0;
			int num = 0 ;
			for(int i = 0;i < m_atts.Count;i ++)
			{
				if(is_lock(i))
				{
					num++;
				}
			}
			switch(num)
			{
			case 1:
				_itemcount += _gaizao.m_1;
				break;
			case 2:
				_itemcount += _gaizao.m_2;
				break;
			case 3:
				_itemcount += _gaizao.m_3;
				break;
			case 4:
				_itemcount += _gaizao.m_4;
				break;
			}
			sys._instance.m_self.remove_item((uint)_gaizao.item_id,_itemcount,game_data._instance.get_t_language ("equip_gaizao_panel.cs_248_69") );//装备改造消耗
            m_equip.gaizao_counts += _itemcount;
			m_effect.GetComponent<Animator>().speed = 4.0f;
			m_effect.gameObject.SetActive (true);

			sys._instance.play_sound ("sound/gaizao");
			m_play = true;

			GameObject _main = m_center.transform.Find("main").gameObject;

			_main.transform.Find("gaizao").GetComponent<UIButton>().isEnabled = false;
			_main.transform.Find("yijiangaizao").GetComponent<UIButton>().isEnabled = false;

		}
		else if (message.m_opcode == opclient_t.CMSG_EQUIP_GAIZAO_TEN)
		{
			protocol.game.smsg_equip_gaizao _msg = net_http._instance.parse_packet<protocol.game.smsg_equip_gaizao> (message.m_byte);
			m_equip.rand_ids.Clear();
			m_equip.rand_values.Clear();
			for (int i = 0; i < _msg.equip.rand_ids.Count; ++i)
			{
				m_equip.rand_ids.Add(_msg.equip.rand_ids[i]);
				m_equip.rand_values.Add(_msg.equip.rand_values[i]);
			}
			
			s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
			s_t_gaizao _gaizao = game_data._instance.get_gaizao(t_equip.font_color);
			int _itemcount = _gaizao.m_0;
			int num = 0 ;
			for(int i = 0;i < m_atts.Count;i ++)
			{
				if(is_lock(i))
				{
					num++;
				}
			}
			switch(num)
			{
			case 1:
				_itemcount += _gaizao.m_1;
				break;
			case 2:
				_itemcount += _gaizao.m_2;
				break;
			case 3:
				_itemcount += _gaizao.m_3;
				break;
			case 4:
				_itemcount += _gaizao.m_4;
				break;
			}
			sys._instance.m_self.remove_item((uint)_gaizao.item_id,_itemcount * _msg.num ,game_data._instance.get_t_language ("equip_gaizao_panel.cs_248_69"));//装备改造消耗
            m_equip.gaizao_counts += _itemcount * _msg.num;
			m_effect.GetComponent<Animator>().speed = 4.0f;
			m_effect.gameObject.SetActive (true);
			
			sys._instance.play_sound ("sound/gaizao");
			m_play = true;
			
			GameObject _main = m_center.transform.Find("main").gameObject;
			
			_main.transform.Find("gaizao").GetComponent<UIButton>().isEnabled = false;
			_main.transform.Find("yijiangaizao").GetComponent<UIButton>().isEnabled = false;
			m_ten = true;
			m_ten_num = _msg.num;

		}
	}



	void IMessage.message(s_message message)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		s_t_gaizao _gaizao = game_data._instance.get_gaizao(t_equip.font_color);
		_itemcount = 0;
		int num1 = 0 ;
		for(int i = 0;i < m_atts.Count;i ++)
		{
			if(is_lock(i))
			{
				num1++;
			}
		}
		switch(num1)
		{
		case 1:
			_itemcount += _gaizao.m_1;
			break;
		case 2:
			_itemcount += _gaizao.m_2;
			break;
		case 3:
			_itemcount += _gaizao.m_3;
			break;
		case 4:
			_itemcount += _gaizao.m_4;
			break;
		}
		if(message.m_type == "gaizao")
		{
			protocol.game.cmsg_equip_gaizao _msg = new protocol.game.cmsg_equip_gaizao ();
			_msg.equip_guid = m_equip.guid;
			for(int i = 0;i < m_atts.Count;i ++)
			{
				if(is_lock(i))
				{
					_msg.suos.Add(i);
				}
			}
			net_http._instance.send_msg<protocol.game.cmsg_equip_gaizao> (opclient_t.CMSG_EQUIP_GAIZAO, _msg);
		}
		else if(message.m_type == "yijiangaizao")
		{
			protocol.game.cmsg_equip_gaizao _msg = new protocol.game.cmsg_equip_gaizao ();
			_msg.equip_guid = m_equip.guid;
			for(int i = 0;i < m_atts.Count;i ++)
			{
				if(is_lock(i))
				{
					_msg.suos.Add(i);
				}
			}
			net_http._instance.send_msg<protocol.game.cmsg_equip_gaizao> (opclient_t.CMSG_EQUIP_GAIZAO_TEN, _msg);
		}
		else if(message.m_type == "refresh_gaizhaoshi" )
		{
			int inum = sys._instance.m_self.get_item_num((uint)_gaizao.item_id);
			m_itemcount.GetComponent<UILabel>().text = "x" + _itemcount;
			string color = "";
			if(inum >= _itemcount + _gaizao.m_0)
			{
				color = "[42ffff]";
			}
			else
			{
				color = "[ff0000]";
			}
			m_gaizhaoshicur.text = color + "x" + sys._instance.m_self.get_item_num((uint)_gaizao.item_id);
			GameObject _main = m_center.transform.Find("main").gameObject;
			
			{
				_main.transform.Find("gaizao").GetComponent<UIButton>().isEnabled = true;
			}
			{
				_main.transform.Find("yijiangaizao").GetComponent<UIButton>().isEnabled = true;
			}
            GameObject icon1 = _main.transform.Find("icon1").gameObject;
            sys._instance.remove_child(icon1);
            GameObject iicon0 = icon_manager._instance.create_item_icon_ex(_gaizao.item_id, inum, _itemcount + _gaizao.m_0 );
            iicon0.transform.parent = icon1.transform;
            iicon0.transform.localPosition = new Vector3(0, 0, 0);
            iicon0.transform.localScale = new Vector3(1, 1, 1);
		}
        else if (message.m_type == "refresh_equip_gaizao_gui")
        {
 
        }
	}

	bool isplay()
	{
		Animator  animator = m_effect.GetComponent<Animator>();
		AnimatorStateInfo animatorInfo; 
		animatorInfo =animator.GetCurrentAnimatorStateInfo(0); 
		if (animatorInfo.normalizedTime > 1.0f ) 
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

	int get_now_max()
	{
		int _num = 0;

		for(int i = 0; i < m_atts.Count;++i)
		{
			if(m_atts[i].transform.Find("lock").gameObject.activeSelf)
			{
				_num ++;
			}
		}

		return _num;
	}

	bool is_lock(int id)
	{
		if(m_atts[id].activeSelf && m_atts[id].transform.Find("lock").gameObject.activeSelf)
		{
			return true;
		}

		return false;
	}

	bool is_lock(string name)
	{
		for(int i = 0; i < m_atts.Count;++i)
		{
			if(m_atts [i].transform.name == name)
			{
				GameObject _lock = m_atts [i].transform.Find("lock").gameObject;
				
				if(_lock.gameObject.activeSelf)
				{
					return true;
				}
			}
		}
		
		return false;
	}

	void lock_att(string name)
	{
		for(int i = 0; i < m_atts.Count;++i)
		{
			if(m_atts [i].transform.name == name)
			{
				GameObject _lock = m_atts [i].transform.Find("lock").gameObject;
				if(_lock.gameObject.activeSelf)
				{
					_lock.SetActive(false);
				}
				else
				{
					_lock.SetActive(true);
				}
			}
		}
	}

	public void click_item_icon()
	{
		s_message _message = new s_message();
		_message.m_type = "buy_num_gui";
		_message.m_ints.Add(101000);
		_message.m_ints.Add(2);
		cmessage_center._instance.add_message(_message);
	}

	public void click(GameObject obj)
	{
        s_t_equip t_equip = game_data._instance.get_t_equip(m_equip.template_id);
        s_t_gaizao _gaizao = game_data._instance.get_gaizao(t_equip.font_color);
        int _itemcount = _gaizao.m_0;
        int num = 0;
        for (int i = 0; i < m_atts.Count; i++)
        {
            if (is_lock(i))
            {
                num++;
            }
        }
        switch (num)
        {
            case 1:
                _itemcount += _gaizao.m_1;
                break;
            case 2:
                _itemcount += _gaizao.m_2;
                break;
            case 3:
                _itemcount += _gaizao.m_3;
                break;
            case 4:
                _itemcount += _gaizao.m_4;
                break;
        }
		if (obj.name == "gaizao")
		{
			if(_itemcount > sys._instance.m_self.get_item_num((uint)_gaizao.item_id) )
			{
				s_message _message1 = new s_message();
				_message1.m_type = "buy_num_gui";
				_message1.m_ints.Add(101000);
				_message1.m_ints.Add(2);
				cmessage_center._instance.add_message(_message1);
				return;
			}
			s_message _message = new s_message();
			_message.m_type = "gaizao";
			cmessage_center._instance.add_message(_message);


		}
		else if(obj.name == "yijiangaizao")
		{
			
			if(_itemcount * 10 > sys._instance.m_self.get_item_num((uint)_gaizao.item_id) )
			{
				s_message _message1 = new s_message();
				_message1.m_type = "buy_num_gui";
				_message1.m_ints.Add(101000);
				_message1.m_ints.Add(2);
				cmessage_center._instance.add_message(_message1);
				return;
			}
			s_message _message = new s_message();
			_message.m_type = "yijiangaizao";
			root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),game_data._instance.get_t_language ("equip_gaizao_panel.cs_553_50"),_message);//提示//使用一键改造会进行自动改造，当出现新的属性条或紫色以上属性时会停止，{nn}是否继续？
		}
		else if (obj.name == "close")
		{
			this.transform.GetComponent<ui_show_anim>().hide_ui();

			s_message _msg = new s_message();
			_msg.m_type = m_out_message;
            _msg.m_ints.Add(2);
			_msg.m_long.Add(m_equip.guid);
			cmessage_center._instance.add_message(_msg);
		}
		else if(obj.name == "buy_count")
		{
			s_message _message = new s_message();
			_message.m_type = "buy_num_gui";
			_message.m_ints.Add(101000);
			_message.m_ints.Add(2);
			cmessage_center._instance.add_message(_message);
		}
		if(obj.name.Substring(0,3) == "att")
		{
			int max_num = 4;
			if (max_num > m_equip.rand_ids.Count)
			{
				max_num = m_equip.rand_ids.Count;
			}
			if (get_now_max() >= max_num && is_lock(obj.name) == false)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_gaizao_panel.cs_582_59"));//锁定超过最大个数
				return;
			}
			else
			{
				lock_att(obj.name);

				int inum = sys._instance.m_self.get_item_num((uint)_gaizao.item_id);
				 num = 0 ;
				_itemcount = 0;
				for(int i = 0;i < m_atts.Count;i ++)
				{
					if(is_lock(i))
					{
						num++;
					}
				}
				switch(num)
				{
				case 1:
					_itemcount += _gaizao.m_1;
					break;
				case 2:
					_itemcount += _gaizao.m_2;
					break;
				case 3:
					_itemcount += _gaizao.m_3;
					break;
				case 4:
					_itemcount += _gaizao.m_4;
					break;
				}
				string color = "";
				if(inum >= _itemcount + _gaizao.m_0)
				{
					color = "[42ffff]";
				}
				else
				{
					color = "[ff0000]";
				}
				m_gaizhaoshicur.text = color + "x" + sys._instance.m_self.get_item_num((uint)_gaizao.item_id);
				m_itemcount.GetComponent<UILabel>().text = "x" + _itemcount;
				GameObject _main = m_center.transform.Find("main").gameObject;
				//if(_itemcount + _gaizao.m_0 > sys._instance.m_self.get_item_num((uint)_gaizao.item_id) )
				{
					//_main.transform.Find("gaizao").GetComponent<UIButton>().isEnabled = false;
				}
				//else
				{
					_main.transform.Find("gaizao").GetComponent<UIButton>().isEnabled = true;
				}
			//	if((_itemcount + _gaizao.m_0) * 10 > sys._instance.m_self.get_item_num((uint)_gaizao.item_id) )
				{
				//	_main.transform.Find("yijiangaizao").GetComponent<UIButton>().isEnabled = false;
				}
			//	else
				{
					_main.transform.Find("yijiangaizao").GetComponent<UIButton>().isEnabled = true;
				}
			}
		}
	}
}
