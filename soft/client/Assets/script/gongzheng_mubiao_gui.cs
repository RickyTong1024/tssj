
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gongzheng_mubiao_gui : MonoBehaviour {
	
	int m_select = 0;
	public GameObject m_zb;
	public GameObject m_bw;
	public GameObject m_zb_jl;
	public GameObject m_bw_jl;
	public GameObject m_jn;
	public GameObject m_un_zb_jl;
	public GameObject m_un_bw;
	public GameObject m_un_bw_jl;
	public GameObject m_un_jn;
	public GameObject m_tishi;
	public GameObject m_gongzhen_level;
	public GameObject m_gongzhen_cur;
	public GameObject m_gongzhen_next;
	public GameObject m_skill_icon;
	public GameObject m_tiltle;
	public GameObject m_zero;
	public GameObject m_all;
	public GameObject m_scro;
	private bool u_flag = false;
	public ccard m_card;
	public List<GameObject> m_items = new List<GameObject>();
	public GameObject m_old_sx;
	public GameObject m_new_sxs;
	public List<dhc.equip_t> m_equips = new List<dhc.equip_t>();
	public List<dhc.treasure_t> m_treasures = new List<dhc.treasure_t>();

	// Use this for initialization
	void Start () {

	}

	public void OnEnable()
	{
		sys._instance.m_self.m_item_shop_effect = 0;
		if(!u_flag)
		{
			m_select = 0;
		}
		update_ui ();
	}

	public void update_ui()
	{
		bool flag = false;
		for(int i  =0 ; i < m_card.m_treasure.Count;++i)
		{
			dhc.treasure_t _treasure = m_card.m_treasure[i];
			if(_treasure == null)
			{
				flag = true;
				break;
			}
		}
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasue_qh || flag)
		{
			m_bw.GetComponent<BoxCollider>().enabled = false;
			m_un_bw.GetComponent<UISprite>().set_enable(false);
		}
		else
		{
			m_bw.GetComponent<BoxCollider>().enabled = true;
			m_un_bw.GetComponent<UISprite>().set_enable(true);
		}
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure_jl || flag)
		{
			m_bw_jl.GetComponent<BoxCollider>().enabled = false;
			m_un_bw_jl.GetComponent<UISprite>().set_enable(false);
		}
		else
		{
			m_bw_jl.GetComponent<BoxCollider>().enabled = true;
			m_un_bw_jl.GetComponent<UISprite>().set_enable(true);
		}
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_equip_jl)
		{
			m_zb_jl.GetComponent<BoxCollider>().enabled = false;
			m_un_zb_jl.GetComponent<UISprite>().set_enable(false);
		}
		else
		{
			m_zb_jl.GetComponent<BoxCollider>().enabled = true;
			m_un_zb_jl.GetComponent<UISprite>().set_enable(true);
		}
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_skill)
		{
			m_jn.GetComponent<BoxCollider>().enabled = false;
			m_un_jn.GetComponent<UISprite>().set_enable(false);
		}
		else
		{
			m_jn.GetComponent<BoxCollider>().enabled = true;
			m_un_jn.GetComponent<UISprite>().set_enable(true);
		}
		if (m_select == 0)
		{
			m_zb.GetComponent<UIToggle>().value = true;
			m_tishi.SetActive(true);
			m_tishi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_105_42");//点击装备可直接前往强化
		}
		else if (m_select == 1)
		{
			m_bw.GetComponent<UIToggle>().value = true;
			m_tishi.SetActive(true);
			m_tishi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_111_42");//点击饰品可直接前往强化
		}
		else if (m_select == 2)
		{
			m_zb_jl.GetComponent<UIToggle>().value = true;
			m_tishi.SetActive(true);
			m_tishi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_117_42");//点击装备可直接前往精炼
		}
		else if (m_select == 3)
		{
			m_bw_jl.GetComponent<UIToggle>().value = true;
			m_tishi.SetActive(true);
			m_tishi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_123_42");//点击饰品可直接前往精炼
		}
		else if(m_select == 4)
		{
			m_jn.GetComponent<UIToggle>().value = true;
			m_tishi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_128_42");//点击技能可直接前往升级
			m_tishi.SetActive(true);
		}
		u_flag = false;
		reset (m_select);
	}

	public void skill()
	{
		m_select = 4;
		m_jn.GetComponent<UIToggle>().value = true;
		m_tishi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_128_42");//点击技能可直接前往升级
		m_tishi.SetActive(true);
		u_flag = true;
		reset (m_select);
	}
	public void reset(int select)
	{
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		int condition = 0;
		int min_num = 0;
		min_num = min_enhance(select,m_card);
		List<int> ids = new List<int>();
		for(int i = 0;i < game_data._instance.m_dbc_gongzhen.get_y();++i )
		{
			int id = int.Parse(game_data._instance.m_dbc_gongzhen.get (1,i));
			int type = int.Parse(game_data._instance.m_dbc_gongzhen.get (2,i));
			s_t_gongzhen _t_gongzheng = game_data._instance.get_t_gongzhen(type,id);
			if(_t_gongzheng.task_type != select+1)
			{
				continue;
			}
			ids.Add(_t_gongzheng.condition);
		}
		s_t_gongzhen _gongzhen = null;
		s_t_gongzhen _gongzhen_next = null;

		_gongzhen = game_data._instance.get_t_gongzhen(select+1,min_num);
		if(_gongzhen == null)
		{
			_gongzhen_next = game_data._instance.get_t_gongzhen(select+1,ids[0]);
			condition = _gongzhen_next.condition;
		}
		else
		{
			bool flag = false;
			for(int i = 0;i < ids.Count;++i)
			{
				s_t_gongzhen t_gongzhen = game_data._instance.get_t_gongzhen(select+1,ids[i]);
				if(min_num < t_gongzhen.condition)
				{
					flag = true;
					_gongzhen_next = game_data._instance.get_t_gongzhen(select+1,ids[i]);
					condition = _gongzhen_next.condition;
					break;
				}
			}
			if(!flag)
			{
				_gongzhen_next = null;
				condition = game_data._instance.get_t_gongzhen(select+1,ids[ids.Count -1]).condition;
			}
		}
		m_zero.SetActive (false);
		m_all.SetActive (true);
		if(select == 0 || select == 1 || select == 2 || select == 3)
		{
			for(int i = 0; i < m_items.Count;++i)
			{
				m_items[i].SetActive(true);
			}
			m_skill_icon.SetActive(false);
		}
		else
		{
			for(int i = 0; i < m_items.Count;++i)
			{
				m_items[i].SetActive(false);
			}
			m_skill_icon.SetActive(true);
		}
		if(select == 0)
		{
			equip_gongzheng(_gongzhen,_gongzhen_next,min_num,condition);
		}
		else if(select == 1)
		{
			treasure_gongzheng(_gongzhen,_gongzhen_next,min_num,condition);
		}
		else if(select == 2)
		{
			equip_jl_gongzheng(_gongzhen,_gongzhen_next,min_num,condition);
		}
		else if(select == 3)
		{
			treasure_jl_gongzheng(_gongzhen,_gongzhen_next,min_num,condition);
		}
		else if(select == 4)
		{
			skill_gongzheng(_gongzhen,_gongzhen_next,min_num,condition);
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

	void equip_gongzheng(s_t_gongzhen  _gongzhen, s_t_gongzhen _gongzhen_next,int min_num,int condition)
	{
		m_equips.Clear ();
		int id = 0;
		for(int i = 0; i < 4;++i)
		{
			dhc.equip_t _equip = m_card.m_equip[i];
			if( _equip == null)
			{
				continue;
			}
			m_items[id].SetActive(true);
			m_equips.Add(_equip);
			GameObject m_icon = m_items[id].transform.Find("icon").gameObject;
			sys._instance.remove_child(m_icon);
			GameObject iicon1 = icon_manager._instance.create_equip_icon_ex(_equip.template_id);
			iicon1.transform.name = id.ToString();
			iicon1.transform.parent =  m_icon.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			m_items[id].GetComponent<UIButtonMessage>().functionName = "click_equip_icon";
			m_items[id].GetComponent<UIButtonMessage>().target = this.gameObject;

			UIButtonMessage[] meses = iicon1.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click_equip_icon";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			m_items[id].transform.Find("name").GetComponent<UILabel>().text = equip.get_equip_name(_equip.template_id,0);
			m_items[id].transform.Find("num").GetComponent<UILabel>().text = _equip.enhance + "/" + condition;
			m_items[id].transform.Find("bar").GetComponent<UIProgressBar>().value = (float)_equip.enhance/condition;
			id ++;
		}
		for(int i = id; i < m_items.Count;++i)
		{
			m_items[i].SetActive(false);
		}
		m_tiltle.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_323_42");//装备强化进度
		if(_gongzhen_next == null)
		{
			m_gongzhen_next.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_326_50");//已达到最强共振
			m_all.SetActive(false);
		}
		else
		{
			m_gongzhen_next.GetComponent<UILabel>().text =  string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_331_65"),_gongzhen_next.gongzhen_level);//装备强化共振{0}级
			m_all.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_332_54"),_gongzhen_next.condition);//4件装备强化{0}级
		}
		if(_gongzhen == null)
		{
			m_gongzhen_level.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_336_51");//装备强化共振0级
			m_gongzhen_cur.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_336_51");//装备强化共振0级
			m_zero.SetActive(true);
		}
		else
		{
			m_gongzhen_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_331_65"),_gongzhen.gongzhen_level);//装备强化共振{0}级
			m_gongzhen_cur.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_331_65"),_gongzhen.gongzhen_level);//装备强化共振{0}级
			m_zero.SetActive(false);
		}
	}

	void treasure_gongzheng(s_t_gongzhen  _gongzhen, s_t_gongzhen _gongzhen_next,int min_num,int condition)
	{
		m_treasures.Clear ();
		int id = 0;
		for(int i = 0; i < 2;++i)
		{
			dhc.treasure_t _treasure = m_card.m_treasure[i];
			if(_treasure == null)
			{
				continue;
			}
			m_treasures.Add(_treasure);
			m_items[id].SetActive(true);
			GameObject m_icon = m_items[id].transform.Find("icon").gameObject;
			sys._instance.remove_child(m_icon);
			GameObject iicon1 = icon_manager._instance.create_treasure_icon_ex(_treasure.template_id);
			iicon1.transform.parent =  m_icon.transform;
			iicon1.transform.name = id.ToString();
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			m_items[id].GetComponent<UIButtonMessage>().functionName = "click_treasure_icon";
			m_items[id].GetComponent<UIButtonMessage>().target = this.gameObject;

			UIButtonMessage[] meses = iicon1.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click_treasure_icon";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			m_items[id].transform.Find("name").GetComponent<UILabel>().text = treasure.get_treasure_name(_treasure.template_id,0);
			m_items[id].transform.Find("num").GetComponent<UILabel>().text = _treasure.enhance + "/" + condition;
			m_items[id].transform.Find("bar").GetComponent<UIProgressBar>().value = (float)_treasure.enhance/condition;
			id ++;
		}
		for(int i = id; i < m_items.Count;++i)
		{
			m_items[i].SetActive(false);
		}
		m_tiltle.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_387_42");//饰品强化进度
		if(_gongzhen_next == null)
		{
			m_gongzhen_next.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_326_50");//已达到最强共振
			m_all.SetActive(false);
		}
		else
		{
			m_gongzhen_next.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_395_64"),_gongzhen_next.gongzhen_level);//饰品强化共振{0}级
			m_all.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_396_54"),_gongzhen_next.condition);//2件饰品强化{0}级
		}
		if(_gongzhen == null)
		{
			m_gongzhen_level.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_400_51");//饰品强化共振0级
			m_gongzhen_cur.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_400_51");//饰品强化共振0级
			m_zero.SetActive(true);
		}
		else
		{
			m_gongzhen_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_395_64"),_gongzhen.gongzhen_level);//饰品强化共振{0}级
			m_gongzhen_cur.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_395_64"),_gongzhen.gongzhen_level);//饰品强化共振{0}级
			m_zero.SetActive(false);
		}
	}

	void skill_gongzheng(s_t_gongzhen  _gongzhen, s_t_gongzhen _gongzhen_next,int min_num,int condition)
	{
		m_skill_icon.transform.Find("num").GetComponent<UILabel>().text = min_num + "/" + condition;
		m_skill_icon.transform.Find("bar").GetComponent<UIProgressBar>().value = (float)min_num/condition;
		m_skill_icon.transform.Find("name").GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_417_91"), min_num);//技能等级 {0}
		m_tiltle.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_418_42");//技能升级进度
		if(_gongzhen_next == null)
		{
			m_gongzhen_next.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_326_50");//已达到最强共振
			m_all.SetActive(false);
		}
		else
		{
			m_gongzhen_next.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_426_64"),_gongzhen_next.gongzhen_level);//技能升级共振{0}级
			m_all.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_427_54"),_gongzhen_next.condition);//技能等级总和达到{0}级
		}
		if(_gongzhen == null)
		{
			m_gongzhen_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_426_64"),"0");//技能升级共振{0}级
			m_gongzhen_cur.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_426_64"),"0");//技能升级共振{0}级
			m_zero.SetActive(true);
		}
		else
		{
			m_gongzhen_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_426_64"), _gongzhen.gongzhen_level);//技能升级共振{0}级
			m_gongzhen_cur.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_426_64"), _gongzhen.gongzhen_level);//技能升级共振{0}级
			m_zero.SetActive(false);
		}
	}

	void equip_jl_gongzheng(s_t_gongzhen  _gongzhen, s_t_gongzhen _gongzhen_next,int min_num,int condition)
	{
		int id = 0;
		m_equips.Clear ();
		for(int i = 0; i < 4;++i)
		{
			dhc.equip_t _equip = m_card.m_equip[i];
			if( _equip == null)
			{
				continue;
			}
			m_items[id].SetActive(true);
			m_equips.Add(_equip);
			GameObject m_icon = m_items[id].transform.Find("icon").gameObject;
			sys._instance.remove_child(m_icon);
			GameObject iicon1 = icon_manager._instance.create_equip_icon_ex(_equip.template_id);
			iicon1.transform.name = id.ToString();
			iicon1.transform.parent =  m_icon.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			m_items[id].GetComponent<UIButtonMessage>().functionName = "click_equip_jl_icon";
			m_items[id].GetComponent<UIButtonMessage>().target = this.gameObject;
			
			UIButtonMessage[] meses = iicon1.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click_equip_jl_icon";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			m_items[id].transform.Find("name").GetComponent<UILabel>().text = equip.get_equip_name(_equip.template_id,0);
			m_items[id].transform.Find("num").GetComponent<UILabel>().text = _equip.jilian + "/" + condition;
			m_items[id].transform.Find("bar").GetComponent<UIProgressBar>().value = (float)_equip.jilian/condition;
			id ++;
		}
		for(int i = id; i < m_items.Count;++i)
		{
			m_items[i].SetActive(false);
		}
		m_tiltle.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_482_42");//装备精炼进度
		if(_gongzhen_next == null)
		{
			m_gongzhen_next.GetComponent<UILabel>().text =  game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_326_50");//已达到最强共振
			m_all.SetActive(false);
		}
		else
		{
			m_gongzhen_next.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_490_64"),_gongzhen_next.gongzhen_level);//装备精炼共振{0}级
			m_all.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_491_54"),_gongzhen_next.condition);//4件装备精炼{0}级
		}
		if(_gongzhen == null)
		{
			m_gongzhen_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_490_64"),"0");//装备精炼共振{0}级
			m_gongzhen_cur.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_490_64"),"0");//装备精炼共振{0}级
			m_zero.SetActive(true);
		}
		else
		{
			m_gongzhen_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_490_64"),_gongzhen.gongzhen_level);//装备精炼共振{0}级
			m_gongzhen_cur.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_490_64"),_gongzhen.gongzhen_level);//装备精炼共振{0}级
			m_zero.SetActive(false);
		}
	}

	void treasure_jl_gongzheng(s_t_gongzhen  _gongzhen, s_t_gongzhen _gongzhen_next,int min_num,int condition)
	{
		int id = 0;
		m_treasures.Clear ();
		for(int i = 0; i < 2;++i)
		{
			dhc.treasure_t _treasure = m_card.m_treasure[i];
			if(_treasure == null)
			{
				continue;
			}
			m_items[id].SetActive(true);
			m_treasures.Add(_treasure);
			GameObject m_icon = m_items[id].transform.Find("icon").gameObject;
			sys._instance.remove_child(m_icon);
			GameObject iicon1 = icon_manager._instance.create_treasure_icon_ex(_treasure.template_id);
			iicon1.transform.parent =  m_icon.transform;
			iicon1.transform.name = id.ToString();
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			m_items[id].GetComponent<UIButtonMessage>().functionName = "click_treasure_jl_icon";
			m_items[id].GetComponent<UIButtonMessage>().target = this.gameObject;
			
			UIButtonMessage[] meses = iicon1.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click_treasure_jl_icon";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			m_items[id].transform.Find("name").GetComponent<UILabel>().text = treasure.get_treasure_name(_treasure.template_id,0);
			m_items[id].transform.Find("num").GetComponent<UILabel>().text = _treasure.jilian  + "/" + condition;
			m_items[id].transform.Find("bar").GetComponent<UIProgressBar>().value = (float)_treasure.jilian/condition;
			id ++;
		}
		for(int i = id; i < m_items.Count;++i)
		{
			m_items[i].SetActive(false);
		}
		m_tiltle.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_546_42");//饰品精炼进度
		if(_gongzhen_next == null)
		{
			m_gongzhen_next.GetComponent<UILabel>().text = game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_326_50");//已达到最强共振
			m_all.SetActive(false);
		}
		else
		{
			m_gongzhen_next.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_554_64"),_gongzhen_next.gongzhen_level);//饰品精炼共振{0}级
			m_all.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_555_54"),_gongzhen_next.condition);//2件饰品精炼{0}级
		}
		if(_gongzhen == null)
		{
			m_gongzhen_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_554_64"),"0");//饰品精炼共振{0}级
			m_gongzhen_cur.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_554_64"),"0");//饰品精炼共振{0}级
			m_zero.SetActive(true);
		}
		else
		{
			m_gongzhen_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_554_64"),_gongzhen.gongzhen_level);//饰品精炼共振{0}级
			m_gongzhen_cur.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("gongzheng_mubiao_gui.cs_554_64"),_gongzhen.gongzhen_level);//饰品精炼共振{0}级
			m_zero.SetActive(false);
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			m_zb.GetComponent<UIToggle>().value = true;
		}
		if(obj.transform.name == "zb")
		{
			m_select = 0;
			update_ui ();
		}
		if(obj.transform.name == "bw")
		{
			m_select = 1;
			update_ui ();
		}
		if(obj.transform.name == "zb_jl")
		{
			m_select = 2;
			update_ui ();
		}
		if(obj.transform.name == "bw_jl")
		{
			m_select = 3;
			update_ui ();
		}

		if(obj.transform.name == "jn")
		{
			m_select = 4;
			update_ui ();
		}
	}

	public void click_equip_icon(GameObject obj)
	{
		u_flag = true;
		int id = int.Parse (obj.transform.name);
		m_zb.GetComponent<UIToggle>().value = true;
		s_message msg = new s_message();
		msg.m_type = "show_qianghua_gui";
		msg.m_long.Add(m_equips[id].guid);
		for(int i =0 ; i < m_equips.Count;++i)
		{
			msg.m_long.Add(m_equips[i].guid);
		}
		msg.m_string.Add("show_buzheng2");
		cmessage_center._instance.add_message(msg);
		
		s_message msg1 = new s_message();
		msg1.m_type = "hide_buzheng";
		cmessage_center._instance.add_message(msg1);
	}

	public void click_treasure_icon(GameObject obj)
	{
		u_flag = true;
		int id = int.Parse (obj.transform.name);
		m_bw.GetComponent<UIToggle>().value = true;
		s_message msg = new s_message();
		msg.m_type = "show_treasure_qianghua_gui";
		msg.m_long.Add(m_treasures[id].guid);
		for(int i =0 ; i < m_treasures.Count;++i)
		{
			msg.m_long.Add(m_treasures[i].guid);
		}
		msg.m_string.Add("show_buzheng2");
		cmessage_center._instance.add_message(msg);
		
		s_message msg1 = new s_message();
		msg1.m_type = "hide_buzheng";
		cmessage_center._instance.add_message(msg1);
	}

	public void click_equip_jl_icon(GameObject obj)
	{
		u_flag = true;
		int id = int.Parse (obj.transform.name);
		m_zb_jl.GetComponent<UIToggle>().value = true;
		s_message msg = new s_message();
		msg.m_type = "show_equip_jinglian_gui";
		msg.m_long.Add(m_equips[id].guid);
		msg.m_string.Add("show_buzheng2");
		cmessage_center._instance.add_message(msg);
		
		s_message msg1 = new s_message();
		msg1.m_type = "hide_buzheng";
		cmessage_center._instance.add_message(msg1);
	}

	public void click_treasure_jl_icon(GameObject obj)
	{
		u_flag = true;
		int id = int.Parse (obj.transform.name);
		m_bw_jl.GetComponent<UIToggle>().value = true;
		s_message msg = new s_message();
		msg.m_type = "show_jianglian_gui";
		msg.m_long.Add(m_treasures[id].guid);
		msg.m_string.Add("show_buzheng2");
		cmessage_center._instance.add_message(msg);
		
		s_message msg1 = new s_message();
		msg1.m_type = "hide_buzheng";
		cmessage_center._instance.add_message(msg1);
	}


	public void click_skill_icon(GameObject obj)
	{
		u_flag = true;
		//int id = int.Parse (obj.transform.name);
		this.transform.Find("frame_big").GetComponent<frame>().hide();
		m_jn.GetComponent<UIToggle>().value = true;
		s_message msg = new s_message();
		msg.m_type = "show_jn_gui";
		msg.m_long.Add ((ulong)m_card.get_guid());
		cmessage_center._instance.add_message(msg);
		sys._instance.m_message_type.Clear ();
		sys._instance.m_message_type.Add ("show_gongzheng_skill_gui");
	}
	
	public  static int min_enhance(int select,ccard m_card)
	{
		int min_enhance = 0;
		if(select == 0)
		{
			int id = 0;
			for(int i = 0; i < 4;++i)
			{
				dhc.equip_t _equip = m_card.m_equip[i];
				if(_equip != null)
				{
					min_enhance = _equip.enhance;
					break;
				}
			}
			for(int i = 0; i < 4;++i)
			{
				dhc.equip_t _equip = m_card.m_equip[i];
				if(_equip == null)
				{
					continue;
				}
				id ++;
				if(_equip.enhance < min_enhance)
				{
					min_enhance = _equip.enhance;
				}
			}
			if(id < 4)
			{
				min_enhance = 0;
			}
		}
		else if(select == 1)
		{
			min_enhance = 0;
			int id = 0;
			for(int i = 0; i < 2;++i)
			{
				dhc.treasure_t _treasure = m_card.m_treasure[i];
				if(_treasure != null)
				{
					min_enhance = _treasure.enhance;
					break;
				}
			}
			for(int i = 0; i < 2;++i)
			{
				dhc.treasure_t _treasure = m_card.m_treasure[i];
				if(_treasure == null)
				{
					continue;
				}
				id ++;
				if(_treasure.enhance < min_enhance)
				{
					min_enhance = _treasure.enhance;
				}
			}
			if(id < 2)
			{
				min_enhance = 0;
			}
		}
		else if(select == 2)
		{
			int id = 0;
			min_enhance = 0;
			for(int i = 0; i < 4;++i)
			{
				dhc.equip_t _equip = m_card.m_equip[i];
				if(_equip != null)
				{
					min_enhance = _equip.jilian;
					break;
				}
			}
			for(int i = 0; i < 4;++i)
			{
				dhc.equip_t _equip = m_card.m_equip[i];
				if(_equip == null)
				{
					continue;
				}
				id ++;

				if(_equip.jilian < min_enhance)
				{
					min_enhance = _equip.jilian;
				}
			}
			if(id < 4)
			{
				min_enhance = 0;
			}
		}
		else if(select == 3)
		{
			min_enhance = 0;
			int id = 0;
			for(int i = 0; i < 2;++i)
			{
				dhc.treasure_t _treasure = m_card.m_treasure[i];
				if(_treasure != null)
				{
					min_enhance = _treasure.jilian;
					break;
				}
			}
			for(int i = 0; i < 2;++i)
			{
				dhc.treasure_t _treasure = m_card.m_treasure[i];
				if(_treasure == null)
				{
					continue;
				}
				id ++;
	
				if(_treasure.jilian  < min_enhance)
				{
					min_enhance = _treasure.jilian ;
				}
			}
			if(id < 2)
			{
				min_enhance = 0;
			}
		}
		else if(select == 4)
		{
			min_enhance += m_card.get_role().jskill_level[0];
			for (int i = (int)e_skill_type_ex.skill_type_jlevel_1; i < (int)e_skill_type_ex.skill_end; ++i)
			{
				int index = i;
				if (m_card.is_skill_ex((e_skill_type_ex)i))
				{
					min_enhance += m_card.get_role().jskill_level[index];
				}
			}
		}
		return min_enhance;
	}

	public static List<int> gongzhen_cur(ccard m_card)
	{
		List<int> conditions = new List<int>();
      
		for(int i = 0; i < 5;++i)
		{
			int min_enhace =  gongzheng_mubiao_gui.min_enhance (i,m_card);
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

	// Update is called once per frame
	void Update () {
	
	}
}
