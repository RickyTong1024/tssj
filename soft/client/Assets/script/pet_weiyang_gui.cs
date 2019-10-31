
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pet_weiyang_gui : MonoBehaviour,IMessage{

	public ulong m_guid;
	public GameObject m_cur_level;
	public GameObject m_next_level;
	public UILabel m_cur_hp;
	public UILabel m_cur_attack;
	public UILabel m_cur_wf;
	public UILabel m_cur_mf;
	public UILabel m_next_hp;
	public UILabel m_next_attack;
	public UILabel m_next_wf;
	public UILabel m_next_mf;
	public GameObject m_bar;
	public List<GameObject> m_icons = new List<GameObject>();
	public List<GameObject> m_nulls = new List<GameObject>();
	public List<GameObject> m_ups = new List<GameObject>();
	public GameObject m_level;
	public UILabel m_gold;
	public UILabel m_sj;
	public UILabel m_exp;
	public GameObject m_tishi;
	public GameObject m_num_gui;
	public GameObject m_show_Label;
	public string m_message = "";
	dhc.pet_t m_t_pet;
	private pet m_pet;
	private int m_select_id = 0;
	private List<int> m_item_ids = new List<int>();
	List<int> ciliaos = new List<int>();
	List<int> counts = new List<int>();
	int[] item_ids = new int[4]{110020001,110020002,110020003,110020004};
	// Use this for initialization
	void Start () {
		
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void update_ui()
	{
		m_select_id = 0;
		m_sj.text = game_data._instance.get_t_language ("pet_weiyang_gui.cs_51_14");//一键升级
		m_cur_level.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_weiyang_gui.cs_52_46") + m_pet.get_level ().ToString();//等级 
		m_cur_hp.text = m_pet.pet_weiyang_attr (1,m_pet.get_level()).ToString();
		m_cur_attack.text = m_pet.pet_weiyang_attr (2,m_pet.get_level()).ToString();
		m_cur_wf.text = m_pet.pet_weiyang_attr (3,m_pet.get_level()).ToString();
		m_cur_mf.text = m_pet.pet_weiyang_attr (4,m_pet.get_level()).ToString();
		m_level.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_weiyang_gui.cs_52_46") + m_pet.get_level ().ToString();//等级 
		s_t_exp t_exp = game_data._instance.get_t_exp (m_pet.get_level ()+1);
		if(t_exp == null)
		{
			m_bar.GetComponent<UIProgressBar>().value = 1.0f;
			m_exp.text = "--/--";
			m_gold.text = "----";
			m_next_hp.text = m_pet.pet_weiyang_attr (1,m_pet.get_level()).ToString();
			m_next_attack.text = m_pet.pet_weiyang_attr (2,m_pet.get_level()).ToString();
			m_next_wf.text = m_pet.pet_weiyang_attr (3,m_pet.get_level()).ToString();
			m_next_mf.text = m_pet.pet_weiyang_attr (4,m_pet.get_level()).ToString();
			for(int i = 0; i < m_ups.Count;++i)
			{
				m_ups[i].SetActive(false);
			}
			for(int i = 0 ; i < m_nulls.Count;++i)
			{
				m_nulls[i].SetActive(false);
				sys._instance.remove_child(m_icons[i]);
				m_icons[i].GetComponent<BoxCollider>().enabled = false;
			}
			m_next_level.GetComponent<UILabel>().text =  game_data._instance.get_t_language ("pet_weiyang_gui.cs_52_46") + m_pet.get_level().ToString();//等级 
			m_tishi.SetActive(true);
		}
		else
		{
			int max_num = Mathf.Min(m_pet.get_level ()+level_num()+1,game_data._instance.m_dbc_exp.get_y());
			s_t_exp _exp = game_data._instance.get_t_exp (max_num);
			int gold = toltle_gold();
			m_bar.GetComponent<UIProgressBar>().value = (float)(get_enhance_exp()) / (float)(m_pet.get_pet_exp(_exp));
			m_exp.text = game_data._instance.get_t_language ("pet_weiyang_gui.cs_87_16") + get_enhance_exp() + "/" + m_pet.get_pet_exp(_exp);//当前经验:
			m_next_hp.text = m_pet.pet_weiyang_attr (1,m_pet.get_level()+1).ToString("f0");
			m_next_attack.text = m_pet.pet_weiyang_attr (2,m_pet.get_level()+1).ToString("f0");
			m_next_wf.text = m_pet.pet_weiyang_attr (3,m_pet.get_level()+1).ToString("f0");
			m_next_mf.text = m_pet.pet_weiyang_attr (4,m_pet.get_level()+1).ToString("f0");
			for(int i = 0; i < m_ups.Count;++i)
			{
				m_ups[i].SetActive(true);
			}
			if(sys._instance.m_self.m_t_player.gold >= gold)
			{
				m_gold.text = sys._instance.get_res_color(1) +"x"+gold;
			}
			else
			{
				m_gold.text = "[ff0000]x" + gold;
			}
			for(int i = 0 ; i < m_nulls.Count;++i)
			{
				m_nulls[i].SetActive(true);
				sys._instance.remove_child(m_icons[i]);
				m_icons[i].GetComponent<BoxCollider>().enabled = true;
			}
			m_tishi.SetActive(false);
			m_next_level.GetComponent<UILabel>().text =  game_data._instance.get_t_language ("pet_weiyang_gui.cs_52_46") + (m_pet.get_level() + 1).ToString();//等级 
			m_level.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_weiyang_gui.cs_52_46") + (m_pet.get_level ()+level_num()).ToString();//等级 
			update_icon ();
		}
	}

	int get_enhance_exp()
	{
		int enhance_exp = 0;
		s_t_pet _pet = game_data._instance.get_t_pet (m_t_pet.template_id);
		int max_enhace = game_data._instance.m_dbc_exp.get_y();
		int num = level_num();
		int exp = get_exp ();
		if(m_t_pet.level + num >= max_enhace)
		{
			enhance_exp = get_exp(m_pet,max_enhace-1,1);
		}
		else if(m_t_pet.level + num == m_t_pet.level)
		{
			enhance_exp = exp;
		}
		else
		{
			enhance_exp = exp - get_exp(m_pet,m_t_pet.level,num);
		}
		return enhance_exp;
	}

	public void update_icon()
	{
		for(int i =0;i < m_icons.Count && i < m_item_ids.Count;++i)
		{
			if(m_item_ids[i] == 0)
			{
				sys._instance.remove_child(m_icons[i]);
				m_nulls[i].SetActive(true);
			}
			else
			{
				sys._instance.remove_child(m_icons[i]);
				m_nulls[i].SetActive(false);
				GameObject _icon = icon_manager._instance.create_item_icon(m_item_ids[i]);
				_icon.transform.GetComponent<BoxCollider>().enabled = false;
				_icon.transform.parent = m_icons[i].transform;
				_icon.transform.localPosition = new Vector3(0,0,0);
				_icon.transform.localScale = new Vector3(1,1,1);
			}
		}
		
	}

	public static int get_exp(pet _pet, int min, int level)
	{
		int _need_exp = 0;
		
		for(int i = min +1 ;i <= min +level;i ++)
		{
			s_t_exp _exp = game_data._instance.get_t_exp(i);
			if (_exp != null)
			{
				_need_exp += _pet.get_pet_exp(_exp);
			}
		}
		
		return _need_exp;
	}

	public void reset(ulong guid)
	{
		m_guid = guid;
		m_pet = sys._instance.m_self.get_pet_guid (m_guid);
		m_t_pet = m_pet.get_pet ();
		m_item_ids.Clear ();
		update_ui ();
	}

	public void select(GameObject obj)
	{
		m_select_id = int.Parse (obj.transform.name);
		if(obj.transform.childCount != 1)
		{
			List<int > m_hide_ids = new List<int >();
			for(int i = 0; i< m_item_ids.Count;++i)
			{
				m_hide_ids.Add(m_item_ids[i]);
			}
			List<int > m_ids = new List<int >();
			for(int i = 0; i < item_ids.Length;++i)
			{
				m_ids.Add(item_ids[i]);
			}
			int count = 0;
			for(int i = 0; i< m_icons.Count;++i)
			{
				if(m_icons[i].transform.childCount == 0)
				{
					count ++;
				}
			}
			string text = game_data._instance.get_t_language ("pet_weiyang_gui.cs_226_17");//选择粮食进行升级
			root_gui._instance.show_common_item_panel (text, false, m_ids, m_hide_ids, "common_select_item", true,this.gameObject,count);
			this.GetComponent<ui_show_anim>().hide_ui();
			this.GetComponent<gui_remove>().m_remove = false;
		}
		else
		{
			m_item_ids[m_select_id] = 0;
			update_ui();
		}
	}

	int get_exp()
	{
		int exp = 0;
		for(int i = 0; i < m_item_ids.Count;++i)
		{
			if(m_item_ids[i] == 0)
			{
				continue;
			}
			s_t_item t_item = game_data._instance.get_item (m_item_ids[i]);
			exp += t_item.def_1;
		}
		exp +=  m_pet.get_exp();
		return exp;
	}

	int level_num()
	{
		int exp = 0;
		int num = 0;
		s_t_pet _pet = game_data._instance.get_t_pet (m_pet.get_template_id());
		exp = get_exp ();
		int need_exp = get_exp(m_pet,m_pet.get_level(),num+1);
		while( exp >= need_exp)
		{
			num ++;
			if(m_pet.get_level() + num >= game_data._instance.m_dbc_exp.get_y())
			{
				break;
			}
			need_exp = get_exp(m_pet,m_pet.get_level(),num +1);
		}
		return num;
	}

	int toltle_gold()
	{
		int toltal_gold = 0;
		int num = level_num ();
		s_t_pet _pet = game_data._instance.get_t_pet (m_pet.get_template_id());
		int item_exp = get_exp ();
		int _num = game_data._instance.m_dbc_exp.get_y();
		if(m_pet.get_level() + num < _num)
		{
			toltal_gold = item_exp - m_t_pet.exp;
		}
		else
		{
			toltal_gold = get_exp(m_pet,m_t_pet.level,_num - m_t_pet.level) - m_t_pet.exp;
		}
		return toltal_gold;
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "sj")
		{
			bool flag = false;
			for(int i = 0; i < m_item_ids.Count;++i)
			{
				if(m_item_ids[i] != 0)
				{
					flag = true;
					break;
				}
			}
			ciliaos.Clear();
			counts.Clear();
			s_t_exp t_exp = game_data._instance.get_t_exp(m_t_pet.level+1);
			if(t_exp == null)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_weiyang_gui.cs_309_46"));//[ffc882]该宠物已升至满级
				return;
			}
			if(!flag)
			{
				if(pet_fast_level.get_max_num(m_pet) < 1)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_weiyang_gui.cs_316_47"));//[ffc882]拥有的饲料或金币无法提升等级
					return;
				}
				m_item_ids.Clear();
				for(int i = 0; i < m_icons.Count;++i)
				{
					sys._instance.remove_child(m_icons[i]);
				}
				m_num_gui.GetComponent<pet_fast_level>().m_pet = m_pet;
				m_num_gui.GetComponent<pet_fast_level>().m_input_num = 1;
				m_num_gui.GetComponent<pet_fast_level>().reset();
				m_num_gui.SetActive(true);
			}
			else
			{
				int toltal_gold  = toltle_gold();
				if(toltal_gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
					return;
				}
				for(int i = 0; i < item_ids.Length;++i)
				{
					if(m_item_ids.Contains(item_ids[i]))
					{
						ciliaos.Add(item_ids[i]);
					}
				}
				for(int i = 0; i < ciliaos.Count;++i)
				{
					int num = 0;
					for(int j = 0; j < m_item_ids.Count;++j)
					{
						if(ciliaos[i] == m_item_ids[j])
						{
							num ++;
						}
					}
					counts.Add(num);
				}
				protocol.game.cmsg_pet_level _msg = new protocol.game.cmsg_pet_level ();
				for(int i = 0; i < ciliaos.Count;++i)
				{
					_msg.ciliao.Add(ciliaos[i]);
					_msg.count.Add(counts[i]);
				}
				_msg.guid = m_pet.get_guid();
				net_http._instance.send_msg<protocol.game.cmsg_pet_level> (opclient_t.CMSG_PET_LEVEL, _msg);
			}
		}
		else if(obj.transform.name == "close")
		{
			s_message _message = new s_message ();
			_message.m_type = m_message;
			cmessage_center._instance.add_message (_message);
			this.transform.GetComponent<ui_show_anim>().hide_ui();
		}
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "common_select_item1")
		{
			bool flag = false;
			sys._instance.remove_child(m_icons[m_select_id]);
			for(int i = 0; i< sys._instance.m_select_items.Count && i < m_icons.Count;++i)
			{
				flag = false;
				for(int j = 0; j < m_item_ids.Count;++j)
				{
					if(m_item_ids[j] == 0 && sys._instance.m_select_items[i] != 0)
					{
						flag = true;
						m_item_ids[j] = sys._instance.m_select_items[i];
						break;
					}
				}
				if(!flag)
				{
					m_item_ids.Add(sys._instance.m_select_items[i]);
				}
			}
			update_ui();
		}
		if(message.m_type == "pet_yj_enhance")
		{
			for(int i = 0; i < message.m_ints.Count;++i)
			{
				m_item_ids.Add((int)message.m_ints[i]);
			}
			int toltal_gold  = toltle_gold();
			if(toltal_gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
				return;
			}
			for(int i = 0; i < item_ids.Length;++i)
			{
				if(m_item_ids.Contains(item_ids[i]))
				{
					ciliaos.Add(item_ids[i]);
				}
			}
			for(int i = 0; i < ciliaos.Count;++i)
			{
				int num = 0;
				for(int j = 0; j < m_item_ids.Count;++j)
				{
					if(ciliaos[i] == m_item_ids[j])
					{
						num ++;
					}
				}
				counts.Add(num);
			}
			protocol.game.cmsg_pet_level _msg = new protocol.game.cmsg_pet_level ();
			for(int i = 0; i < ciliaos.Count;++i)
			{
				_msg.ciliao.Add(ciliaos[i]);
				_msg.count.Add(counts[i]);
			}
			_msg.guid = m_pet.get_guid();
			net_http._instance.send_msg<protocol.game.cmsg_pet_level> (opclient_t.CMSG_PET_LEVEL, _msg);
		}
	}
	
	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_PET_LEVEL)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if (_msg.success)
			{
				sys._instance.clear_select_items(5);
				for(int i = 0; i < ciliaos.Count;++i)
				{
					sys._instance.m_self.remove_item((uint)ciliaos[i],counts[i],game_data._instance.get_t_language ("pet_weiyang_gui.cs_452_65"));//宠物升级消耗
				}
	
				ciliaos.Clear();
				counts.Clear();
				int item_exp = get_exp();
				int num = level_num();
				int toltal_gold  = toltle_gold();
				s_t_pet _pet = game_data._instance.get_t_pet (m_pet.get_template_id());
				int exp = get_exp(m_pet,m_pet.get_level(),num);
				if(m_t_pet.level +num >= game_data._instance.m_dbc_exp.get_y())
				{
					m_t_pet.level = game_data._instance.m_dbc_exp.get_y();
					m_t_pet.exp = 0;
				}
				else
				{
					m_t_pet.level += num;
					m_t_pet.exp = item_exp - exp;
				}
				sys._instance.m_self.sub_att(e_player_attr.player_gold, toltal_gold,game_data._instance.get_t_language ("pet_weiyang_gui.cs_452_65"));//宠物升级消耗
				m_item_ids.Clear();
				update_ui();
				s_message _message3 = new s_message();
				_message3.m_type = "check_bf";
				cmessage_center._instance.add_message(_message3);
				m_show_Label.SetActive(true);
				Label_time();
			}
		}
	}

	void Label_time()
	{
		hide_time _hide = m_show_Label.GetComponent<hide_time>();
		
		if(_hide == null)
		{
			_hide = m_show_Label.AddComponent<hide_time>();
		}
		
		_hide.m_time = 0.3f;
	}

	public static bool is_weiyang(pet m_pet)
	{
		if(m_pet == null)
		{
			return false;
		}
		s_t_exp t_exp = game_data._instance.get_t_exp (m_pet.get_level()+1);
		if(t_exp == null)
		{
			return false;
		}
		if(pet_fast_level.get_max_num(m_pet) > 0)
		{
			return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
