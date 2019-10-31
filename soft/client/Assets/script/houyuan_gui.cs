
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class houyuan_gui : MonoBehaviour ,IMessage{
	
	public GameObject m_text;
	public GameObject m_buzheng;
	public GameObject m_huoyuan_panel;
	public GameObject m_gongzhen_panel;
	public GameObject m_zuhe_num;
	public GameObject m_guwu_button;
	public GameObject m_un_guwu_button;
	public GameObject m_zhuwei_button;
	public GameObject m_un_zhuwei_button;
	public GameObject m_tishi;
	public GameObject m_gongzhen_level;
	public GameObject m_gongzhen_cur;
	public GameObject m_gongzhen_next;
	public GameObject m_tiltle;
	public GameObject m_zero;
	public GameObject m_all;
	public List<GameObject> m_items = new List<GameObject>();
	public GameObject m_old_sx;
	public GameObject m_new_sxs;
	public int m_select_card_id = 0;
	public GameObject m_scro;
	public GameObject m_view;
	public List<GameObject> m_cards = new List<GameObject>();
	public UIToggle m_houyuan;

	ccard _card = null;
	private ulong change_id = 0;
	private int num = 0;
	public ulong card_id = 0;
	public int m_select = 0;
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
		sys._instance.m_houyuans.Clear();
		sys._instance.m_houyuans = houyuan_gui.gongzhen_cur();
		sys._instance.m_houyuan_sxs.Clear();
		sys._instance.m_houyuan_sxs = sys._instance.m_self.get_houyuan_sxs();
		update_ui ();
		reset ();
	}

	public void reset(bool refresh = false)
	{
		if(m_select == 0)
		{
			m_huoyuan_panel.SetActive(true);
			m_gongzhen_panel.SetActive(false);
			if(m_scro.GetComponent<SpringPanel>() != null)
			{
				m_scro.GetComponent<SpringPanel>().enabled = false;
			}
			m_scro.transform.localPosition = new Vector3(0, 0, 0);
			m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			num = 0;
			string _zh = "";
			bool flag = false;
			bool first = false;
			for(int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count;++i)
			{
				List<int> jbs = new List<int>();
				if(sys._instance.m_self.m_t_player.zhenxing[i] == 0)
				{
					continue;
				}
				ulong guid = sys._instance.m_self.m_t_player.zhenxing [i];
				ccard m_card = sys._instance.m_self.get_card_guid (guid);
				_card = m_card;
				for(int j = 0; j < m_card.m_t_class.jbs.Count;++j)
				{
					if(m_card.m_t_class.jbs[j] > 0)
					{
						s_t_ji_ban _jbex = game_data._instance.get_t_ji_ban(m_card.m_t_class.jbs[j]);
						if(_jbex != null)
						{
							jbs.Add(m_card.m_t_class.jbs[j]);
						}
					}
				}
				if(jbs.Count > 0)
				{
					jbs.Sort (comp);
					if(first)
					{
						_zh += "\n";
					}
					for(int k = 0 ; k < 32;++k)
					{
						_zh += " ";
					}
					if(first)
					{
						_zh += m_card.get_color_name()+"[-]";
					}
					else
					{
						_zh += m_card.get_color_name()+"[-]" + "\n";
					}
					first = true;
				}
				for(int j = 0; j < jbs.Count;++j)
				{
					if(flag)
					{
						_zh += "\n";
					}
					if (jbs[j] > 0 )
					{
						flag = true;
						_zh += get_jb_text (jbs[j], _zh == "",m_card);
					}
				}
			}
			m_zuhe_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_128_46") + num.ToString ();//当前组合激活数量:
			m_text.GetComponent<UILabel>().text = _zh;
		}
		else
		{
			m_huoyuan_panel.SetActive(false);
			m_gongzhen_panel.SetActive(true);
			if(!refresh)
			{
				if(m_view.GetComponent<SpringPanel>() != null)
				{
					m_view.GetComponent<SpringPanel>().enabled = false;
				}
				m_view.transform.localPosition = new Vector3(0, 0, 0);
				m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			}
			int condition = 0;
			int min_num = 0;
			min_num = min_enhance(m_select+4);
			List<int> ids = new List<int>();
			for(int i = 0;i < game_data._instance.m_dbc_gongzhen.get_y();++i )
			{
				int id = int.Parse(game_data._instance.m_dbc_gongzhen.get (1,i));
				int type = int.Parse(game_data._instance.m_dbc_gongzhen.get (2,i));
				s_t_gongzhen _t_gongzheng = game_data._instance.get_t_gongzhen(type,id);
				if(_t_gongzheng.task_type != m_select+5)
				{
					continue;
				}
				ids.Add(_t_gongzheng.condition);
			}
			s_t_gongzhen _gongzhen = null;
			s_t_gongzhen _gongzhen_next = null;
			
			_gongzhen = game_data._instance.get_t_gongzhen(m_select+5,min_num);
			if(_gongzhen == null)
			{
				_gongzhen_next = game_data._instance.get_t_gongzhen(m_select+5,ids[0]);
				condition = _gongzhen_next.condition;
			}
			else
			{
				bool flag = false;
				for(int i = 0;i < ids.Count;++i)
				{
					s_t_gongzhen t_gongzhen = game_data._instance.get_t_gongzhen(m_select+5,ids[i]);
					if(min_num < t_gongzhen.condition)
					{
						flag = true;
						_gongzhen_next = game_data._instance.get_t_gongzhen(m_select+5,ids[i]);
						condition = _gongzhen_next.condition;
						break;
					}
				}
				if(!flag)
				{
					_gongzhen_next = null;
					condition = game_data._instance.get_t_gongzhen(m_select+1,ids[ids.Count -1]).condition;
				}
			}
			m_zero.SetActive (false);
			m_all.SetActive (true);
			for(int i = 0; i < m_items.Count;++i)
			{
				m_items[i].SetActive(true);
			}
			if(m_select == 1)
			{
				m_tishi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_197_43");//点击伙伴可直接前往充能
				guwu_gongzheng(_gongzhen,_gongzhen_next,min_num,condition);
			}
			else if(m_select == 2)
			{
				m_tishi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_202_43");//点击伙伴可直接前往突破
				zhuwei_gongzheng(_gongzhen,_gongzhen_next,min_num,condition);
			}
			if(_gongzhen != null)
			{
				m_old_sx.SetActive(true);
				string attr = "";
				bool first_attr = false;
				for(int j = 0; j < _gongzhen.attrs.Count;++j)
				{
					if(first_attr)
					{
						attr += "\n" +  game_data._instance.get_float_string(_gongzhen.attrs[j],_gongzhen.value1[j]);
					}
					else
					{
						attr += game_data._instance.get_float_string(_gongzhen.attrs[j],_gongzhen.value1[j]);
					}
					first_attr = true;
				}
				m_old_sx.GetComponent<UILabel>().text = attr;
			}
			else
			{
				m_old_sx.SetActive(false);
			}
			if(_gongzhen_next != null)
			{
				m_new_sxs.SetActive(true);
				string attr = "";
				bool first_attr = false;
				for(int j = 0; j < _gongzhen_next.attrs.Count;++j)
				{
					if(first_attr)
					{
						attr += "\n" +  game_data._instance.get_float_string(_gongzhen_next.attrs[j],_gongzhen_next.value1[j]);
					}
					else
					{
						attr += game_data._instance.get_float_string(_gongzhen_next.attrs[j],_gongzhen_next.value1[j]);
					}
					first_attr = true;
					m_new_sxs.GetComponent<UILabel>().text = attr;
				}
			}
			else
			{
				m_new_sxs.SetActive(false);
			}
		}
	}

	void guwu_gongzheng(s_t_gongzhen  _gongzhen, s_t_gongzhen _gongzhen_next,int min_num,int condition)
	{
		int id = 0;
		for(int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count;++i)
		{
			ulong guid = sys._instance.m_self.m_t_player.houyuan[i];
			ccard card = sys._instance.m_self.get_card_guid(guid);
			if( card == null)
			{
				continue;
			}
			m_items[id].SetActive(true);
			GameObject m_icon = m_items[id].transform.Find("icon").gameObject;
			sys._instance.remove_child(m_icon);
			GameObject iicon1 = icon_manager._instance.create_card_icon_ex(card);
			iicon1.transform.name = id.ToString();
			iicon1.transform.parent =  m_icon.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			m_items[id].GetComponent<UIButtonMessage>().functionName = "click_guwu_icon";
			m_items[id].GetComponent<UIButtonMessage>().target = this.gameObject;
			
			UIButtonMessage[] meses = iicon1.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click_guwu_icon";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			m_items[id].transform.Find("name").GetComponent<UILabel>().text = ccard.get_color_name(card.m_t_class.name,card.m_t_class.color);
			m_items[id].transform.Find("num").GetComponent<UILabel>().text = card.get_level() + "/" + condition;
			m_items[id].transform.Find("bar").GetComponent<UIProgressBar>().value = (float)card.get_level()/condition;
			id ++;
		}
		for(int i = id; i < m_items.Count;++i)
		{
			m_items[i].SetActive(false);
		}
		m_tiltle.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_292_42");//后援伙伴等级
		if(_gongzhen_next == null)
		{
			m_gongzhen_next.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_295_50");//后援鼓舞已达最大等级
			m_all.SetActive(false);
		}
		else
		{
			m_gongzhen_next.GetComponent<UILabel>().text =  string.Format(game_data._instance.get_t_language ("houyuan_gui.cs_300_65") , _gongzhen_next.gongzhen_level //后援鼓舞{0}级
				+ "");
			m_all.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("houyuan_gui.cs_302_54") , _gongzhen_next.condition + //6个后援伙伴等级达到{0}级
				"");
		}
		if(_gongzhen == null)
		{
			m_gongzhen_level.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_307_51");//后援鼓舞0级
			m_gongzhen_cur.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_307_51");//后援鼓舞0级
			m_zero.SetActive(true);
		}
		else
		{
			m_gongzhen_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("houyuan_gui.cs_300_65") , _gongzhen.gongzhen_level //后援鼓舞{0}级
				+ "");
			m_gongzhen_cur.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("houyuan_gui.cs_300_65"), _gongzhen.gongzhen_level //后援鼓舞{0}级
				+ "");
			m_zero.SetActive(false);
		}
	}

	void zhuwei_gongzheng(s_t_gongzhen  _gongzhen, s_t_gongzhen _gongzhen_next,int min_num,int condition)
	{
		int id = 0;
		for(int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count;++i)
		{
			ulong guid = sys._instance.m_self.m_t_player.houyuan[i];
			ccard card = sys._instance.m_self.get_card_guid(guid);
			if( card == null)
			{
				continue;
			}
			m_items[id].SetActive(true);
			GameObject m_icon = m_items[id].transform.Find("icon").gameObject;
			sys._instance.remove_child(m_icon);
			GameObject iicon1 = icon_manager._instance.create_card_icon_ex(card);
			iicon1.transform.name = id.ToString();
			iicon1.transform.parent =  m_icon.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			m_items[id].GetComponent<UIButtonMessage>().functionName = "click_zhuwei_icon";
			m_items[id].GetComponent<UIButtonMessage>().target = this.gameObject;
			
			UIButtonMessage[] meses = iicon1.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click_zhuwei_icon";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			m_items[id].transform.Find("name").GetComponent<UILabel>().text = ccard.get_color_name(card.m_t_class.name,card.m_t_class.color);
			m_items[id].transform.Find("num").GetComponent<UILabel>().text = card.get_glevel() + "/" + condition;
			m_items[id].transform.Find("bar").GetComponent<UIProgressBar>().value = (float)card.get_glevel()/condition;
			id ++;
		}
		for(int i = id; i < m_items.Count;++i)
		{
			m_items[i].SetActive(false);
		}
		m_tiltle.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_359_42");//后援伙伴突破
		if(_gongzhen_next == null)
		{
			m_gongzhen_next.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_362_50");//后援助威已达最大等级
			m_all.SetActive(false);
		}
		else
		{
			m_gongzhen_next.GetComponent<UILabel>().text =  string.Format(game_data._instance.get_t_language ("houyuan_gui.cs_367_65"), _gongzhen_next.gongzhen_level //后援助威{0}级
				+ "");
			m_all.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("houyuan_gui.cs_369_54"), _gongzhen_next.condition + //6个后援伙伴突破达到{0}级
				"");
		}
		if(_gongzhen == null)
		{
			m_gongzhen_level.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_374_51");//后援助威0级
			m_gongzhen_cur.GetComponent<UILabel>().text = game_data._instance.get_t_language ("houyuan_gui.cs_374_51");//后援助威0级
			m_zero.SetActive(true);
		}
		else
		{
			m_gongzhen_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("houyuan_gui.cs_367_65") , _gongzhen.gongzhen_level //后援助威{0}级
				);
			m_gongzhen_cur.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("houyuan_gui.cs_367_65") , _gongzhen.gongzhen_level //后援助威{0}级
				);
			m_zero.SetActive(false);
		}
	}

	public int comp(int jb1,int jb2)
	{
		s_t_ji_ban _jbex1 = game_data._instance.get_t_ji_ban(jb1);
		s_t_ji_ban _jbex2 = game_data._instance.get_t_ji_ban(jb2);
		bool jb1_flag = false;
		bool jb2_flag = false;
		for (int i = 0; i < _jbex1.tids.Count; ++i)
		{
			if(_jbex1.type == 1)
			{
				if(!sys._instance.m_self.is_zheng(_jbex1.tids[i]) && !sys._instance.m_self.is_houyuan(_jbex1.tids[i]))
				{
					jb1_flag = true;
					break;
				}
			}
			else if(_jbex1.type == 2)
			{
				jb1_flag = true;
				for(int k = 0; k < _card.m_equip.Count;k ++ )
				{
					if (_card.m_equip[k] == null)
					{
						continue;
					}
					
					if (_jbex1.tids[i] == _card.m_equip[k].template_id)
					{
						jb1_flag = false;
						break;
					}
				}
			}
			else if(_jbex1.type == 3)
			{
				jb1_flag = true;
				for(int k = 0; k < _card.m_treasure.Count;k ++ )
				{
					if (_card.m_treasure[k] == null)
					{
						continue;
					}
					
					if (_jbex1.tids[i] == _card.m_treasure[k].template_id)
					{
						jb1_flag = false;
						break;
					}
				}
			}
		}
		for (int i = 0; i < _jbex2.tids.Count; ++i)
		{
			if(_jbex2.type == 1)
			{
				if(!sys._instance.m_self.is_zheng(_jbex2.tids[i]) && !sys._instance.m_self.is_houyuan(_jbex2.tids[i]))
				{
					jb2_flag = true;
					break;
				}
			}
			else if(_jbex2.type == 2)
			{
				jb2_flag = true;
				for(int k = 0; k < _card.m_equip.Count;k ++ )
				{
					if (_card.m_equip[k] == null)
					{
						continue;
					}
					
					if (_jbex2.tids[i] == _card.m_equip[k].template_id)
					{
						jb2_flag = false;
						break;
					}
				}
			}
			else if(_jbex2.type == 3)
			{
				jb2_flag = true;
				for(int k = 0; k < _card.m_treasure.Count;k ++ )
				{
					if (_card.m_treasure[k] == null)
					{
						continue;
					}
					
					if (_jbex2.tids[i] == _card.m_treasure[k].template_id)
					{
						jb2_flag = false;
						break;
					}
				}
			}
		}
		if(!jb1_flag && jb2_flag)
		{
			return -1;
		}
		else if(jb1_flag && !jb2_flag)
		{
			return 1;
		}
		else if(_jbex1.type < _jbex2.type )
		{
			return -1;
		}
		else if(_jbex1.type > _jbex2.type )
		{
			return 1;
		}
		else
		{
			return jb1 - jb2;
		}
	}

	public string get_jb_text(int jbex_id, bool first,ccard m_card)
	{
		string _text = "";
		s_t_ji_ban _jbex = game_data._instance.get_t_ji_ban(jbex_id);
		if(_jbex.type == 1)
		{
			bool flag = true;
			for (int i = 0; i < _jbex.tids.Count; ++i)
			{
				if (i != 0)
				{
					_text += "、";
				}
				if(sys._instance.m_self.is_zheng(_jbex.tids[i]) || sys._instance.m_self.is_houyuan(_jbex.tids[i]))
				{
					_text +=  game_data._instance.get_t_class(_jbex.tids[i]).name;
				}
				else
				{
					flag = false;
					_text +=  game_data._instance.get_t_class(_jbex.tids[i]).name;
				}
			}
			if (flag)
			{
				num ++;
				_text = "[0aabff]" + _jbex.name + game_data._instance.get_t_language ("card_dialog_box.cs_272_54") + _text;// 与
			}
			else
			{
				_text = "[777777]" + _jbex.name + game_data._instance.get_t_language ("card_dialog_box.cs_272_54") + _text;// 与
			}
			_text += game_data._instance.get_t_language ("card_dialog_box.cs_278_12") + "，";//一起上阵
			if(_jbex.attr1 > 0)
			{
				if(_jbex.attr2 > 0)
				{
					_text += game_data._instance.get_value_string(_jbex.attr1, _jbex.value1) + "、";
				}
				else
				{
					_text += game_data._instance.get_value_string(_jbex.attr1, _jbex.value1);
				}
			}
			if(_jbex.attr2 > 0)
			{
				_text += game_data._instance.get_value_string(_jbex.attr2, _jbex.value2);
			}
		}
		else if (_jbex.type == 2)
		{
			bool flag = false;
			for(int j = 0 ;j < _jbex.tids.Count;++j)
			{
				flag = false;
				for(int i = 0; i < m_card.m_equip.Count;i ++ )
				{
					if (m_card.m_equip[i] == null)
					{
						continue;
					}
					
					if (_jbex.tids[j] == m_card.m_equip[i].template_id)
					{
						flag = true;
						break;
					}
				}
			}
			if(flag)
			{
				num ++;
				_text += "[0aabff]";
			}
			else
			{
				_text += "[777777]";
			}
			_text +=  _jbex.name  + " " + game_data._instance.get_t_language ("bu_zheng_panel.cs_2956_35");//装备
			for(int j = 0 ;j < _jbex.tids.Count;++j)
			{
				if(j == _jbex.tids.Count -1)
				{
					_text += game_data._instance.get_t_equip(_jbex.tids[j]).name + ",";
				}
				else
				{
					_text += game_data._instance.get_t_equip(_jbex.tids[j]).name + "、";
				}
			}
			if(_jbex.attr1 > 0)
			{
				if(_jbex.attr2 > 0)
				{
					_text += game_data._instance.get_value_string(_jbex.attr1, _jbex.value1) + "、";
				}
				else
				{
					_text += game_data._instance.get_value_string(_jbex.attr1, _jbex.value1);
				}
			}
			if(_jbex.attr2 > 0)
			{
				_text += game_data._instance.get_value_string(_jbex.attr2, _jbex.value2);
			}
		}
		else if (_jbex.type == 3)
		{
			bool flag = false;
			for(int j = 0 ;j < _jbex.tids.Count;++j)
			{
				flag = false;
				for(int i = 0; i < m_card.m_treasure.Count;i ++ )
				{
					if (m_card.m_treasure[i] == null)
					{
						continue;
					}
					
					if (_jbex.tids[j] == m_card.m_treasure[i].template_id)
					{
						flag = true;
						break;
					}
				}
			}
			if(flag)
			{
				num++;
				_text += "[0aabff]";
			}
			else
			{
				_text += "[777777]";
			}
			_text += _jbex.name + " "+ game_data._instance.get_t_language ("bu_zheng_panel.cs_2956_35");//装备
			for(int j = 0 ;j < _jbex.tids.Count;++j)
			{
				if(j == _jbex.tids.Count -1)
				{
					_text += game_data._instance.get_t_baowu(_jbex.tids[j]).name + ",";
				}
				else
				{
					_text += game_data._instance.get_t_baowu(_jbex.tids[j]).name + "、";
				}
			}
			if(_jbex.attr1 > 0)
			{
				if(_jbex.attr2 > 0)
				{
					_text += game_data._instance.get_value_string(_jbex.attr1, _jbex.value1) + "、";
				}
				else
				{
					_text += game_data._instance.get_value_string(_jbex.attr1, _jbex.value1);
				}
			}
			if(_jbex.attr2 > 0)
			{
				_text += game_data._instance.get_value_string(_jbex.attr2, _jbex.value2);
			}
		}
		return _text;
	}

	public void update_ui()
	{
		for(int i = 0;i < sys._instance.m_self.m_t_player.houyuan.Count;i ++)
		{
			set_card_guid(i,sys._instance.m_self.m_t_player.houyuan[i]);
		}
	}

	void set_card_guid(int id,ulong guid)
	{
		if(id >= 6)
		{
			return;
		}
		Transform _add_tag = m_cards [id].transform.Find("null");
		Transform name = m_cards [id].transform.Find("name");
		if (!is_open(id))
		{
			_add_tag.gameObject.SetActive(false);
			name.gameObject.SetActive(false);
			return;
		}
		else if(guid <= 0)
		{
			sys._instance.remove_child(m_cards[id],id.ToString());
			sys._instance.remove_child(m_cards[id],"label");
			name.gameObject.SetActive(false);
			_add_tag.gameObject.SetActive(true);
			return;
		}
		
		sys._instance.remove_child(m_cards[id],id.ToString());
		sys._instance.remove_child(m_cards[id],"label");
		name.gameObject.SetActive(true);
		ccard m_card = sys._instance.m_self.get_card_guid (guid);
		name.gameObject.GetComponent<UILabel>().text = m_card.get_color_name ();
		_add_tag.gameObject.SetActive(true);
		
		GameObject _icon = icon_manager._instance.create_card_icon_ex (guid);
		_icon.transform.name = id.ToString();
		_icon.transform.parent = m_cards[id].transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);

		UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
		meses[0].target = this.gameObject;
		meses[0].functionName = "click_card_icon";
		meses[1].target = null;
		meses[1].functionName = "";
		meses[2].target = null;
		meses[2].functionName = "";
		s_message _message2 = new s_message();
		_message2.m_type = "check_bf";
		cmessage_center._instance.add_message(_message2);

	}

	public  static int min_enhance(int select)
	{
		int min_enhance = 9999;
		if(select == 5)
		{
			for(int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count;++i)
			{
				ulong guid = sys._instance.m_self.m_t_player.houyuan[i];
				ccard card = sys._instance.m_self.get_card_guid(guid);
				if(card == null)
				{
					min_enhance = 0;
					break;
				}
				if(card.get_level() < min_enhance)
				{
					min_enhance = card.get_level();
				}
			}
		}
		else if(select == 6)
		{
			for(int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count;++i)
			{
				ulong guid = sys._instance.m_self.m_t_player.houyuan[i];
				ccard card = sys._instance.m_self.get_card_guid(guid);
				if(card == null)
				{
					min_enhance = 0;
					break;
				}
				if(card.get_glevel() < min_enhance)
				{
					min_enhance = card.get_glevel();
				}
			}
		}
		return min_enhance;
	}

	public static List<int> gongzhen_cur()
	{
		List<int> conditions = new List<int>();
		for(int i = 5; i < 7;++i)
		{
			int min_enhace =  houyuan_gui.min_enhance (i);
			List<int> ids = new List<int>();
			for(int j = 0;j < game_data._instance.m_dbc_gongzhen.get_y();++j )
			{
				int level = int.Parse(game_data._instance.m_dbc_gongzhen.get (1,j));
				int type = int.Parse(game_data._instance.m_dbc_gongzhen.get (2,j));
				s_t_gongzhen _t_gongzheng = game_data._instance.get_t_gongzhen(type,level);
				if(_t_gongzheng.task_type != i+1)
				{
					continue;
				}
				ids.Add(_t_gongzheng.condition);
			}
			if(ids.Count == 0)
			{
				continue;
			}
			s_t_gongzhen _gongzhen = game_data._instance.get_t_gongzhen(i+1,ids[0]);
			if(_gongzhen == null)
			{
				continue;
			}
			if(min_enhace < _gongzhen.condition)
			{
				conditions.Add(0);
			}
			else
			{
				bool flag = false;
				for(int k = 0;k < ids.Count;++k)
				{
					_gongzhen = game_data._instance.get_t_gongzhen(i+1,ids[k]);
					if(min_enhace < _gongzhen.condition)
					{
						flag = true;
						_gongzhen = game_data._instance.get_t_gongzhen(i+1,ids[k -1]);
						conditions.Add(_gongzhen.condition);
						break;
					}
				}
				if(!flag)
				{
					conditions.Add(999);
				}
			}
		}
		return conditions;
	}
	
	void click_guwu_icon(GameObject obj)
	{
		int id = int.Parse (obj.transform.name);
		ulong guid = sys._instance.m_self.m_t_player.houyuan [id];
		sys._instance.m_houyuans.Clear();
		sys._instance.m_houyuans = houyuan_gui.gongzhen_cur();
		sys._instance.m_houyuan_sxs.Clear();
		sys._instance.m_houyuan_sxs = sys._instance.m_self.get_houyuan_sxs();
		s_message _message = new s_message();
		_message.m_type = "show_cn_gui";
		_message.m_long.Add(guid);
		cmessage_center._instance.add_message(_message);
	}

	void click_zhuwei_icon(GameObject obj)
	{
		int id = int.Parse (obj.transform.name);
		ulong guid = sys._instance.m_self.m_t_player.houyuan [id];
		sys._instance.m_houyuans.Clear();
		sys._instance.m_houyuans = houyuan_gui.gongzhen_cur();
		sys._instance.m_houyuan_sxs.Clear();
		sys._instance.m_houyuan_sxs = sys._instance.m_self.get_houyuan_sxs();
		s_message _message = new s_message();
		_message.m_type = "show_tu_po_gui";
		_message.m_long.Add(guid);
		cmessage_center._instance.add_message(_message);
	}

	public void click_card_icon(GameObject obj)
	{
		sys._instance.m_houyuans.Clear();
		sys._instance.m_houyuans = houyuan_gui.gongzhen_cur();
		sys._instance.m_houyuan_sxs.Clear();
		sys._instance.m_houyuan_sxs = sys._instance.m_self.get_houyuan_sxs();
		m_select_card_id = int.Parse (obj.transform.name);
		List<ulong> self = new List<ulong>();
		for (int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count; ++i)
		{
			if (sys._instance.m_self.m_t_player.houyuan[i] != 0)
			{
				self.Add(sys._instance.m_self.m_t_player.houyuan[i]);
			}
		}
		for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
		{
			if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
			{
				self.Add(sys._instance.m_self.m_t_player.zhenxing[i]);
			}
		}
		root_gui._instance.show_common_card_panel (game_data._instance.get_t_language ("bu_zheng_panel.cs_300_46"), true, 1, self, "common_select_houyuan", false,m_buzheng);//请选择需要上阵的伙伴
		m_buzheng.GetComponent<ui_show_anim>().hide_ui();
		m_buzheng.GetComponent<gui_remove>().m_remove = false;
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			m_select = 0;
			s_message _message = new s_message();
			_message.m_type = "update_bu_zheng";
			_message.m_bools.Add(true);
			cmessage_center._instance.add_message(_message);
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			m_houyuan.value = true;
		}
		else if(obj.transform.name == "guwu")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_houyuan_ex)
			{
				m_guwu_button.GetComponent<UIToggle>().enabled = false;
			}
			else
			{
				m_guwu_button.GetComponent<UIToggle>().enabled = true;
			}
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_houyuan_ex)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("houyuan_gui.cs_905_73"),(int)e_open_level.el_houyuan_ex));//后援鼓舞{0}级开启，主人赶快提升等级吧
				return;
			}
			m_select = 1;
			reset();
		}
		else if(obj.transform.name == "zhuwei")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_houyuan_ex)
			{
				m_zhuwei_button.GetComponent<UIToggle>().enabled = false;
			}
			else
			{
				m_zhuwei_button.GetComponent<UIToggle>().enabled = true;
			}
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_houyuan_ex)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("houyuan_gui.cs_923_73"),(int)e_open_level.el_houyuan_ex));//后援助威{0}级开启，主人赶快提升等级吧
				return;
			}
			m_select = 2;
			reset();
		}
		else if(obj.transform.name == "houyuan")
		{
			m_select = 0;
			update_ui();
			reset();
		}
	}


	public void select_card(GameObject obj)
	{
		m_select_card_id = int.Parse (obj.transform.name);
		if(!is_open(m_select_card_id))
		{
			root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_2775_58"));//尚未开启
			return ;
		}
		sys._instance.m_houyuans.Clear();
		sys._instance.m_houyuans = houyuan_gui.gongzhen_cur();
		sys._instance.m_houyuan_sxs.Clear();
		sys._instance.m_houyuan_sxs = sys._instance.m_self.get_houyuan_sxs();
		List<ulong> self = new List<ulong>();
		for (int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count; ++i)
		{
			if (sys._instance.m_self.m_t_player.houyuan[i] != 0)
			{
				self.Add(sys._instance.m_self.m_t_player.houyuan[i]);
			}
		}
		for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
		{
			if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
			{
				self.Add(sys._instance.m_self.m_t_player.zhenxing[i]);
			}
		}
		root_gui._instance.show_common_card_panel (game_data._instance.get_t_language ("bu_zheng_panel.cs_300_46"), false, 1, self, "common_select_houyuan", false,m_buzheng);//请选择需要上阵的伙伴
		m_buzheng.GetComponent<ui_show_anim>().hide_ui();
		m_buzheng.GetComponent<gui_remove>().m_remove = false;
	}


	bool is_open(int id)
	{
		if(id == 0 && sys._instance.m_self.m_t_player.level < (int)e_open_level.el_first_houyuan)
		{
			return false;
		}
		if(id == 1 && sys._instance.m_self.m_t_player.level < (int)e_open_level.el_second_houyuan)
		{
			return false;
		}
		if(id == 2 && sys._instance.m_self.get_att(e_player_attr.player_level) < (int)e_open_level.el_third_houyuan)
		{
			return false;
		}
		
		if(id == 3 && sys._instance.m_self.get_att(e_player_attr.player_level) < (int)e_open_level.el_fourth_houyuan)
		{
			return false;
		}
		
		if(id == 4 && sys._instance.m_self.get_att(e_player_attr.player_level) < (int)e_open_level.el_fifth_houyuan)
		{
			return false;
		}
		
		if(id == 5 && sys._instance.m_self.get_att(e_player_attr.player_level) < (int)e_open_level.el_sixth_houyuan)
		{
			return false;
		}
		
		return true;
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_ROLE_HOUYUAN)
		{
			List<ulong> _houyuans = new List<ulong>();
			
			for(int i = 0;i < sys._instance.m_self.m_t_player.houyuan.Count;i ++)
			{
				_houyuans.Add(sys._instance.m_self.m_t_player.houyuan[i]);
			}
			_houyuans[m_select_card_id] = change_id;
			card_id = change_id;
			if (change_id == 0)
			{
				ccard m_card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.houyuan[m_select_card_id]);
				for(int i = 0; i < m_card.m_treasure.Count;++i)
				{
					if(m_card.m_treasure[i] != null)
					{
						m_card.m_treasure[i].role_guid = 0;
						m_card.get_role().treasures[i] = 0;
					}
				}
				for(int i = 0; i < m_card.m_equip.Count;++i)
				{
					if(m_card.m_equip[i] != null)
					{
						m_card.m_equip[i].role_guid = 0;
						m_card.get_role().zhuangbeis[i] = 0;
					}
				}
			}
			else
			{
				ccard m_card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.houyuan[m_select_card_id]);
				ccard _card = sys._instance.m_self.get_card_guid(change_id);
				if(m_card != null)
				{
					for(int i = 0; i < m_card.m_treasure.Count;++i)
					{
						if(m_card.m_treasure[i] != null)
						{
							m_card.m_treasure[i].role_guid = change_id;
							_card.get_role().treasures[i] = m_card.get_role().treasures[i];
							m_card.get_role().treasures[i] = 0;
						}
					}
					for(int i = 0; i < m_card.m_equip.Count;++i)
					{
						if(m_card.m_equip[i] != null)
						{
							m_card.m_equip[i].role_guid = change_id;
							_card.get_role().zhuangbeis[i] = m_card.get_role().zhuangbeis[i];
							m_card.get_role().zhuangbeis[i] = 0;
						}
					}
				}
			}
			for(int i = 0; i < _houyuans.Count; i ++)
			{
				sys._instance.m_self.m_t_player.houyuan[i] = _houyuans[i];
			}
			
			reset();
			update_ui();
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);

			s_message _message = new s_message();
			_message.m_type = "update_bu_zheng";
	
			_message.m_long.Add(card_id);
			cmessage_center._instance.add_message(_message);
		}
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "common_select_houyuan" &&  this.gameObject.activeSelf)
		{
			change_id = (ulong)message.m_long[0];
			
			protocol.game.cmsg_zhenxing _msg = new protocol.game.cmsg_zhenxing();
			
			if(change_id == 0)
			{
				_msg.index = -1;
				_msg.role_guid = sys._instance.m_self.m_t_player.houyuan[m_select_card_id];
			}
			else
			{
				_msg.index = m_select_card_id;
				_msg.role_guid = change_id;
			}
			
			net_http._instance.send_msg<protocol.game.cmsg_zhenxing> (opclient_t.CMSG_ROLE_HOUYUAN, _msg);
		}
		if(message.m_type == "update_houyuan")
		{
			reset(true);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
