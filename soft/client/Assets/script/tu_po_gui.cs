
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tu_po_gui : MonoBehaviour,IMessage {
	
	public string m_out_message = "";
	public ccard m_card;
	public GameObject m_tp;
	public GameObject m_tp_ok;
	public GameObject m_panel;
	public GameObject m_scro;
	public GameObject m_max_star;
	public static bool is_texiao = true;
    public UILabel m_tupodes;
	private int flat = 0;
	public List<Color> m_color = new List<Color>();
	public GameObject m_des;
	public List<GameObject> m_shizhuang;
    List<GameObject> m_topo_items = new List<GameObject>();

	public UILabel m_special_Label;
	public UILabel m_base_Label;
	public UILabel m_tp_xj_Label;
	public UILabel m_max_xj_Label;
	public UILabel m_hp_add_Label;
	public UILabel m_attack_add_Label;
	public UILabel m_wf_add_Label;
	public UILabel m_mf_add_Label;
	public UILabel m_speed_add_Label;
	public UILabel m_tp_Label;
	public UILabel m_role_Label;
	public UILabel m_need_gold;

	void Start () {
		cmessage_center._instance.add_handle (this);

	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	string get_jg_lable(int id,int color,int value)
	{
		string _name = "h_";

		if(color == 1)
		{
			_name = "g_";
		}
		else if(color == 2)
		{
			_name = "b_";
		}
		else if(color == 3)
		{
			_name = "p_";
		}
		else if(color == 4)
		{
			_name = "r_";
		}
		else if(color == 5)
		{
			_name = "y_";
		}

		if(id == 0)
		{
			_name += "sm";
		}
		else if(id == 1)
		{
			_name += "gj";
		}
		else if(id == 2)
		{
			_name += "wf";
		}
		else if(id == 3)
		{
			_name += "mf";
		}
		else if(id == 4)
		{
			_name += "sd";
		}

		_name += "0";
		_name += value;

		_name += "_tpan";

		return _name;
	}
	string get_jh_name(int id)
	{
		string _name = "xtp_jh_sm00";

		if(id == 1)
		{
			_name = "xtp_jh_gj00";
		}
		else if(id == 2)
		{
			_name = "xtp_jh_wf00";
		}
		else if(id == 3)
		{
			_name = "xtp_jh_mf00";
		}
		else if(id == 4)
		{
			_name = "xtp_jh_sd00";
		}

		return _name;
	}
	
	double get_att_cz(int att,int glevel)
	{
		double _f = m_card.m_t_class.cz[att] + m_card.m_t_class.czcz[att] * glevel;
		if (m_card.m_player != null)
		{
			return _f;
		}
		return 0.0;
	}

	void tp_icon_click(GameObject obj)
	{
		int id = obj.transform.GetComponent<item_icon>().m_item_id;
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add (id);
		cmessage_center._instance.add_message(message);
	}

	void tp_jy_click(GameObject obj)
	{
		int id = obj.transform.GetComponent<item_icon>().m_item_id;
		s_t_item t_item = game_data._instance.get_item (id);
		//id = t_item.def_4;
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add (id);
		cmessage_center._instance.add_message(message);
		sys._instance.m_message_type.Clear ();
		sys._instance.m_message_type.Add ("show_bu_zheng_gui");
		sys._instance.m_message_type.Add ("show_tu_po_gui");
		sys._instance.m_message_long.Clear ();
		sys._instance.m_message_long.Add (m_card.get_guid ());
	}
	

	private int get_color(int glevel)
	{
		int _color = 0;
		
		if(glevel >= 3 && glevel <= 4)
		{

			_color = 1;
		}
		
		if(glevel >= 5 && glevel <= 7)
		{
			_color = 2;
		}
		
		if(glevel >= 8 && glevel <= 11)
		{
			_color = 3;
		}
		
		if(glevel == 12)
		{
			_color = 4;
		}

		return _color;
	}

	public void star (int glevel,GameObject obj,string text)
	{
		for(int i = 0 ; i < 5;++i)
		{
			obj.transform.Find(text+i.ToString()).gameObject.SetActive(false);
		}
		for(int i = 0; i < 5;i ++)
		{
			if(i < glevel)
			{
				obj.transform.Find(text + i.ToString()).GetComponent<UISprite>().spriteName = "xstar_002d";
				obj.transform.Find(text + i.ToString()).gameObject.SetActive(true);
			}
			else
			{
				obj.transform.Find(text+ i.ToString()).gameObject.SetActive(false);
			}
			
			if(i + 5 < glevel)
			{
				obj.transform.Find(text + i.ToString()).GetComponent<UISprite>().spriteName = "xstar_003d";
			}
			
			if(i + 10 < glevel)
			{
				obj.transform.Find(text + i.ToString()).GetComponent<UISprite>().spriteName = "xstar_001d";
			}
		}
	}

	private void show_tp()
	{
		int _glevel = m_card.get_glevel ();
		int _glevel1 = _glevel + 1;
		GameObject _obj_0 = m_tp.transform.Find("0").gameObject;
		s_t_tupo _tupo = game_data._instance.get_tupo (_glevel1);
		int max = (int)e_skill_type.skill_end - (int)e_skill_type.skill_type_glevel_1;
		star(max,_obj_0,"max/max");
		List<double> attrs = ccard.get_role_org_attr (m_card.get_template_id(), m_card.get_level(), m_card.get_jlevel(), m_card.get_glevel(), m_card.get_pinzhi());
		_obj_0.transform.Find("attack").Find("cur").GetComponent<UILabel>().text = ((int)attrs[2]).ToString();
		_obj_0.transform.Find("hp").Find("cur").GetComponent<UILabel>().text = ((int)attrs[1]).ToString();
		_obj_0.transform.Find("pd").Find("cur").GetComponent<UILabel>().text = ((int)attrs[3]).ToString();
		_obj_0.transform.Find("md").Find("cur").GetComponent<UILabel>().text = ((int)attrs[4]).ToString();
		_obj_0.transform.Find("speed").Find("cur").GetComponent<UILabel>().text = ((int)attrs[5]).ToString();
		m_max_star.SetActive(false);
		int _id = 0;
		int _num = 0;
        m_topo_items.Clear();
		for(int i = (int)e_skill_type.skill_type_glevel_1;i < (int)e_skill_type.skill_type_glevel_1 + max;i ++)
		{
			role_skill _skill = m_card.m_skills[i];
			
			if(_skill != null)
			{
				GameObject temp = game_data._instance.ins_object_res ("ui/tu_po_item");
				temp.transform.parent = m_scro.transform;
				temp.transform.localPosition = new Vector3 (0, -138-102*_id ,0);
				temp.transform.localScale = new Vector3(1,1,1);
                temp.transform.Find("xiala").gameObject.SetActive(false);
                temp.transform.Find("xiala").GetComponent<UIButtonMessage>().target = this.gameObject;
				temp.transform.Find("des").GetComponent<UILabel>().text = _skill.get_des();
                m_topo_items.Add(temp);
				star(_id+1,temp,"xj");
				if(_id +1 > m_card.get_glevel())
				{
					temp.transform.Find("lock").gameObject.SetActive(true);
				}
				else
				{
					temp.transform.Find("lock").gameObject.SetActive(false);
				}
				string text = shizhuang(_id);
				if(text != "")
				{
                    temp.transform.Find("sz").GetComponent<UILabel>().text = game_data._instance.get_t_language ("tu_po_gui.cs_260_82");//时装
					temp.transform.Find("sz/sz").GetComponent<UILabel>().text = text;
				}
				else
				{
					temp.transform.Find("sz").gameObject.SetActive(false);
				}
                if (_num + 1 == 10)
                {
                    if (m_card.m_t_class.tupo10 != 0)
                    {
                        temp.name = "10";
                        temp.transform.Find("sz").GetComponent<UILabel>().text = game_data._instance.get_t_skill(m_card.m_t_class.tupo10).name;
                        temp.transform.Find("sz").gameObject.SetActive(true);
                        temp.transform.Find("xiala").gameObject.SetActive(true);
                        temp.transform.Find("sz/sz").GetComponent<UILabel>().text = "";

 
                    }
                }
                else if (_num + 1 == 12)
                {
                    if (m_card.m_t_class.tupo12 != 0)
                    {
                        temp.name = 12 + "";
                        temp.transform.Find("sz").gameObject.SetActive(true);
                        temp.transform.Find("sz").GetComponent<UILabel>().text = game_data._instance.get_t_skill(m_card.m_t_class.tupo12).name;
                        temp.transform.Find("sz/sz").GetComponent<UILabel>().text = "";
                        temp.transform.Find("xiala").gameObject.SetActive(true);

                    }
                }
                else if (_num + 1 == 15)
                {
                    if (m_card.m_t_class.tupo15 != 0)
                    {
                        temp.name = 15 + "";
                        temp.transform.Find("sz").gameObject.SetActive(true);
                        temp.transform.Find("sz").GetComponent<UILabel>().text = game_data._instance.get_t_skill(m_card.m_t_class.tupo15).name;
                        temp.transform.Find("xiala").gameObject.SetActive(true);
                        temp.transform.Find("sz/sz").GetComponent<UILabel>().text = "";


                    }
                }
				_num ++;
				_id ++;
			}
			
		}
		if(_tupo == null ||  _glevel >= max)
		{
			star (_glevel,_obj_0,"tp/tp");
			_obj_0.transform.Find("hp").Find("next").GetComponent<UILabel>().text = "----";
			_obj_0.transform.Find("attack").Find("next").GetComponent<UILabel>().text = "----";
			_obj_0.transform.Find("pd").Find("next").GetComponent<UILabel>().text = "----";
			_obj_0.transform.Find("md").Find("next").GetComponent<UILabel>().text = "----";
			_obj_0.transform.Find("speed").Find("next").GetComponent<UILabel>().text = "----";
			_obj_0.transform.Find("gold/gold").GetComponent<UILabel>().text = "----";
			_obj_0.transform.Find("level/level").GetComponent<UILabel>().text ="--/--";
			m_max_star.SetActive(true);
			GameObject _icon_parent = _obj_0.transform.Find("icon0").gameObject;
			sys._instance.remove_child(_icon_parent);
			int num = sys._instance.m_self.get_item_num ((uint)m_card.get_fragment_id());
			GameObject _icon = icon_manager._instance.create_item_icon_ex (m_card.get_fragment_id(), num, -1);
			_icon.transform.parent = _icon_parent.transform;
			_icon.transform.localPosition = new Vector3 (0, 0, 0);
			_icon.transform.localScale = new Vector3 (1, 1, 1);
			UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "tp_jy_click";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			return;
		}
		int sp_num = 0;
		sp_num = _tupo.sps [m_card.get_color()-2];
		_tupo = game_data._instance.get_tupo (_glevel1);
	
		star (_glevel1,_obj_0,"tp/tp");
		if(_glevel >= max)
		{
			star (max,_obj_0,"tp/tp");
		}
		///////////////////////////////////////////////
		double d_value = get_att_cz (0, _glevel1) - get_att_cz (0, _glevel);
		int _add = (int)(d_value * m_card.get_level ());

		string _des = get_att_cz (0, _glevel1).ToString ("f2");

		List<double> attr1s = ccard.get_role_org_attr (m_card.get_template_id(), m_card.get_level(), m_card.get_jlevel(), m_card.get_glevel()+1, m_card.get_pinzhi());
		_obj_0.transform.Find("hp").Find("next").GetComponent<UILabel>().text = ((int)attr1s[1]).ToString();
		///////////////////////////////////////


		///////////////////////////////////////

		d_value = get_att_cz (1, _glevel1) - get_att_cz (1, _glevel);
		_add = (int)(d_value * m_card.get_level ());

		_des = get_att_cz (1, _glevel1).ToString ("f2");


		_obj_0.transform.Find("attack").Find("next").GetComponent<UILabel>().text = ((int)attr1s[2]).ToString();

		////////////////////////////////////////


		/////////////////////////////////////////
		d_value = get_att_cz (2, _glevel1) - get_att_cz (2, _glevel);
		_add = (int)(d_value * m_card.get_level ());
		
		_des = get_att_cz (2, _glevel1).ToString ("f2");

		 
		_obj_0.transform.Find("pd").Find("next").GetComponent<UILabel>().text = ((int)attr1s[3]).ToString();
		////////////////////////////////////////////////


		///////////////////////////////////////////////
		d_value = get_att_cz (3, _glevel1) - get_att_cz (3, _glevel);
		_add = (int)(d_value * m_card.get_level ());

		_des = get_att_cz (3, _glevel1).ToString ("f2");


		_obj_0.transform.Find("md").Find("next").GetComponent<UILabel>().text = ((int)attr1s[4]).ToString();
		/////////////////////////////////////////////


		////////////////////////////////////////////////

		d_value = get_att_cz (4, _glevel1) - get_att_cz (4, _glevel);
		_add = (int)(d_value * m_card.get_level ());
		
		_des = get_att_cz (4, _glevel1).ToString ("f2");


		_obj_0.transform.Find("speed").Find("next").GetComponent<UILabel>().text = ((int)attr1s[5]).ToString();
		//////////////////////////////////////////////////////


		{
			GameObject _gold = _obj_0.transform.Find("gold/gold").gameObject;

			if(_tupo.cl_gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
			{
				_gold.GetComponent<UILabel>().text = "[ff0000]" + _tupo.cl_gold;
			}
			else
			{
				string text = sys._instance.get_res_color(1);
				_gold.GetComponent<UILabel>().text = text + _tupo.cl_gold;
			}
		}

		{
			GameObject _icon_parent = _obj_0.transform.Find("icon0").gameObject;
			sys._instance.remove_child(_icon_parent);

			if(sp_num > 0)
			{
				int num = sys._instance.m_self.get_item_num ((uint)m_card.get_fragment_id());
				GameObject _icon = icon_manager._instance.create_item_icon_ex (m_card.get_fragment_id(), num, sp_num);
				_icon.transform.parent = _icon_parent.transform;
				_icon.transform.localPosition = new Vector3 (0, 0, 0);
				_icon.transform.localScale = new Vector3 (1, 1, 1);
				UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
				meses[0].target = this.gameObject;
				meses[0].functionName = "tp_jy_click";
				meses[1].target = null;
				meses[1].functionName = "";
				meses[2].target = null;
				meses[2].functionName = "";
			}
		}

		string _level = _tupo.role_level.ToString();
		
		if(m_card.get_level() < _tupo.role_level)
		{
			_level = "[ff0000]" + m_card.get_level().ToString() + "/" + _level;
		}
		else
		{
			_level = "[6cff00]"+ m_card.get_level().ToString() + "/" + _level;
		}
		
		_obj_0.transform.Find("level/level").GetComponent<UILabel>().text = _level;

	}

    
	string shizhuang(int num)
	{
		string text = "";
		for(int i = 0; i < m_shizhuang.Count;++i)
		{
			m_shizhuang[i].SetActive(false);
		}
		dbc m_dbc_role_dress = game_data._instance.get_dbc_role_dress();
		List<s_t_role_dress> _role_dresss = new List<s_t_role_dress>();
		for(int i = 0;i < m_dbc_role_dress.get_y();i ++)
		{
			int id = int.Parse( game_data._instance.m_dbc_role_dress.get(0,i));
			s_t_role_dress _dress = game_data._instance.get_t_role_dress(id);
			if(_dress.role == m_card.get_template_id() && _dress.hq_condition == 1)
			{
				_role_dresss.Add(_dress);
			}
		}
		for(int i = 0;i < _role_dresss.Count;++i)
		{
			if(_role_dresss[i].hq_Level == num +1)
			{
				text = _role_dresss[i].name;
				break;
			}
			else
			{
				text = "";
			}
		}
		return text;
	}

	void OnEnable()
	{

	}

	void release()
	{
		m_des.GetComponent<UILabel>().alpha = 0.5f;
	}
	void press()
	{
		m_des.GetComponent<UILabel>().alpha = 1.0f;
	}

	void IMessage.net_message(s_net_message message)
	{

		if (message.m_opcode == opclient_t.CMSG_ROLE_TUPO)
		{
			int _glevel = m_card.get_glevel();
			s_t_tupo t_tupo = game_data._instance.get_tupo (_glevel + 1);

            sys._instance.m_self.remove_item((uint)m_card.get_fragment_id(), t_tupo.sps[m_card.get_color() - 2], game_data._instance.get_t_language ("tu_po_gui.cs_468_113"));//角色突破消耗
			sys._instance.m_self.sub_att(e_player_attr.player_gold,t_tupo.cl_gold,game_data._instance.get_t_language ("tu_po_gui.cs_468_113"));//角色突破消耗


			m_card.get_role().glevel ++;
			m_card.role_dress(m_card);
			sys._instance.m_self.m_t_player.sj_task_num++;
			sys._instance.m_self.check_target_done();

			s_message _message3 = new s_message();
			_message3.m_type = "check_bf";
			cmessage_center._instance.add_message(_message3);
			//show_tp();
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message();
			_message.m_type = "update_bu_zheng";
			_message.m_bools.Add(is_texiao);
			cmessage_center._instance.add_message(_message);

			s_message _message2 = new s_message();
			_message2.m_type = "update_houyuan";
			cmessage_center._instance.add_message(_message2);

			//root_gui._instance.do_mask(1.0f);
			s_message _message1 = new s_message();
			_message1.time = 0.5f;
			_message1.m_type = "show_tp_label";
			cmessage_center._instance.add_message(_message1);
			//guanghuan(_glevel + 1);
		}
	}

	public void resert(ulong guid)
	{
		m_card = sys._instance.m_self.get_card_guid(guid);
		if(sys._instance.m_self.is_houyuan(m_card.get_guid()))
		{
			is_texiao = false;
		}
		else
		{
			is_texiao = true;
		}
		show_tp();
	}

	void IMessage.message(s_message message)
	{
	
	}

	public void guanghuan(int glevel)
	{
		string s = "";
	
		if(glevel == 1)
		{
			s = "[00ff00]" + game_data._instance.get_t_language ("ccard.cs_956_21");//绿色光环开启
			root_gui._instance.show_prompt_dialog_box(s);
		}

		if(glevel == 3)
		{
			s = "[00abff]"+ game_data._instance.get_t_language ("ccard.cs_964_21");//蓝色光环开启
			root_gui._instance.show_prompt_dialog_box(s);
		}			

		if(glevel == 5)
		{
			s = "[ff00ed]"+ game_data._instance.get_t_language ("ccard.cs_972_21");//紫色光环开启
			root_gui._instance.show_prompt_dialog_box(s);
		}

		if(glevel == 8)
		{
			s = "[ff0000]"+ game_data._instance.get_t_language ("tu_po_gui.cs_543_19");//红色光环开启
			root_gui._instance.show_prompt_dialog_box(s);
		}
	
		if(glevel == 12)
		{
			s = "[e6ff00]"+ game_data._instance.get_t_language ("tu_po_gui.cs_549_19");//橙色光环开启
			root_gui._instance.show_prompt_dialog_box(s);
		}
	}
	
	bool skill(int id,int glevel,UILabel des)
	{
		int _glevel = m_card.get_role ().glevel;

		s_t_skill _sklll = game_data._instance.get_t_skill(id);

		if(_sklll == null)
		{
			return false;
		}

		if(_glevel + 1 == glevel)
		{
			des.text = _sklll.name + " " + _sklll.des;

			return true;
		}

		return false;
	}
	
	public void click(GameObject obj)
	{
		if(obj.transform.name == "tp_ok")
		{
			flat = 0;
			if(m_card.get_glevel() >= ((int)e_skill_type.skill_end - (int)e_skill_type.skill_type_glevel_1))
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("tu_po_gui.cs_582_59"));//已突破到最高星
				return;
			}
			bu_zu(m_card);
			if(flat !=0)
			{
				return;
			}
			int _glevel = m_card.get_glevel();
			int _id = m_card.get_fragment_id();
			int sp_num = sys._instance.m_self.get_item_num ((uint)_id);
			s_t_tupo t_tupo = game_data._instance.get_tupo (_glevel + 1);
			if(t_tupo.sps[m_card.get_color()- 2] > sp_num)
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add (_id);
				cmessage_center._instance.add_message(message);
				sys._instance.m_message_type.Clear ();
				sys._instance.m_message_type.Add ("show_bu_zheng_gui");
				sys._instance.m_message_type.Add ("show_tu_po_gui");
				sys._instance.m_message_long.Clear ();
				sys._instance.m_message_long.Add (m_card.get_guid ());
				return;
			}
			//m_tp.GetComponent<ui_show_anim>().hide_ui();

			protocol.game.cmsg_role_tupo _msg = new protocol.game.cmsg_role_tupo ();
			_msg.role_guid = m_card.get_guid();
			net_http._instance.send_msg<protocol.game.cmsg_role_tupo> (opclient_t.CMSG_ROLE_TUPO, _msg);
		}
		if (obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
				
		}
        else if (obj.name == "xiala")
        {
            obj.GetComponent<Collider>().enabled = false;
            obj.GetComponent<TweenRotation>().onFinished.Clear();
            obj.GetComponent<TweenRotation>().AddOnFinished(delegate {

                obj.GetComponent<Collider>().enabled = true;
            
            });
            UIToggle toggle = obj.GetComponent<UIToggle>();
            if (obj.transform.parent.name == "10")
            {
               
                if (!toggle.value)
                {
                    
                    insert_item(obj.transform.parent.name, m_card.m_t_class.tupo10);
                }
                else
                {

                    remove_item(obj.transform.parent.name, m_card.m_t_class.tupo10);
                }
 
            }
            else if (obj.transform.parent.name == "12")
            {

                if (!toggle.value)
                {
                    insert_item(obj.transform.parent.name, m_card.m_t_class.tupo12);
                }
                else
                {
                    remove_item(obj.transform.parent.name, m_card.m_t_class.tupo12);
                }

            }
            else if (obj.transform.parent.name == "15")
            {
                if (!toggle.value)
                {

                    insert_item(obj.transform.parent.name, m_card.m_t_class.tupo15);
                }
                else
                {
                  

                    remove_item(obj.transform.parent.name, m_card.m_t_class.tupo15);
                }

            }
 
        }
	}
    void remove_item(string name,int tupo)
    {
        int pos = -1;

        for (int i = 0; i < m_topo_items.Count; i++)
        {
            Vector3 v3 = m_topo_items[i].transform.localPosition;
            if (pos != -1)
            {
                TweenPosition.Begin(m_topo_items[i], 0.0f, new Vector3(v3.x, v3.y + 82, v3.z));
            }

            if ("tupo" + tupo == m_topo_items[i].name)
            {
                pos = i;
                Destroy(m_topo_items[i]);

            }

        }
        m_topo_items.RemoveAt(pos);
 
    }
    void insert_item(string name,int tupo)
    {
        int pos = -1;
        GameObject temp = Instantiate(m_tupodes.transform.parent.gameObject) as GameObject;
        temp.name = "tupo" + tupo;
        for(int i = 0;i < m_topo_items.Count;i ++)
        {
            Vector3 v3 = m_topo_items[i].transform.localPosition;
            if (pos != -1)
            {
                
               TweenPosition.Begin(m_topo_items[i], 0.0f, new Vector3(v3.x, v3.y - 82,v3.z));
            

            }

            if (name == m_topo_items[i].name)
            {
                pos = i;
                temp.gameObject.SetActive(true);
                temp.transform.parent = m_scro.transform;
                temp.transform.localScale = Vector3.one;
                temp.transform.localPosition = new Vector3(v3.x, v3.y - 82,v3.z);
                temp.transform.Find("cur").GetComponent<UILabel>().text = game_data._instance.get_t_skill(tupo).des;

            }
         

        }
        m_topo_items.Insert(pos + 1, temp);

    }
	void bu_zu(ccard m_card)
	{
		int _glevel = m_card.get_glevel();
		s_t_tupo t_tupo = game_data._instance.get_tupo (_glevel + 1);
		int _id = m_card.get_fragment_id();
		int sp_num = sys._instance.m_self.get_item_num ((uint)_id);

		if(t_tupo.cl_gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
		{
			string s = "[ffc882]"+game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
			root_gui._instance.show_prompt_dialog_box(s);
			flat +=1;
		}
		if(t_tupo.role_level > m_card.get_level())
		{
			string s = "[ffc882]"+game_data._instance.get_t_language ("jinjie_gui.cs_339_59");//伙伴等级不足
			root_gui._instance.show_prompt_dialog_box(s);
			flat +=1;
		}
	}
	
	public static bool is_tupo(ccard _card )
	{
		s_t_tupo _tupo = game_data._instance.get_tupo (_card.get_role().glevel + 1);
		int num = _card.get_color() -2;
		if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_tupo)
		{
			return false;
		}
		if(_card.get_glevel() >= ((int)e_skill_type.skill_end - (int)e_skill_type.skill_type_glevel_1))
		{
			return false;
		}
		if(_tupo.role_level > _card.get_level())
		{
			return false;
		}
		if(_tupo.cl_gold > sys._instance.m_self.m_t_player.gold)
		{
			return false;
		}
		int _id = _card.get_fragment_id();
		int sp_num = sys._instance.m_self.get_item_num ((uint)_id);
		if( sp_num >= _tupo.sps[num]) 
		{
			return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
