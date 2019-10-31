
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vr_gui : MonoBehaviour,IMessage {

	public GameObject m_view;
	public GameObject m_sub;
	public GameObject m_vr;
	public GameObject m_ui;
	public GameObject m_shizhuang;
	public GameObject m_duihuan;
	public GameObject m_effect;
	public GameObject m_item_effect;
	public GameObject m_tuzhi;
	public GameObject m_num;
	public GameObject m_bar;
	public GameObject m_sx;
	public GameObject m_sz_effect;
	public s_t_dress_unlock t_dress_unlock;
	List<GameObject> m_roles = new List<GameObject>();
	public List<s_t_class> non_cards = new List<s_t_class>();
	public List<ccard> my_cards = new List<ccard>();
	int m_index;

	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}


	public void OnEnable()
	{
		non_cards.Clear ();
		m_roles.Clear ();
		my_cards.Clear ();
		m_sx.SetActive(false);
		sys._instance.remove_child(m_view);
		GameObject temp1 = Instantiate(m_sub) as GameObject;
		temp1.transform.parent = m_view.transform;
		temp1.transform.localScale = Vector3.one;
		temp1.transform.localPosition = new Vector3(0,157,0);
		temp1.SetActive(true);
		temp1.name = "sub0";
		temp1.transform.Find("back").gameObject.SetActive(false);
		temp1.transform.Find("icon").gameObject.SetActive(false);
		temp1.transform.Find("zhushou_icon").gameObject.SetActive(true);

		if (m_index == 0)
		{
			temp1.transform.Find("select").gameObject.SetActive(true);
		}
		else
		{
			temp1.transform.Find("select").gameObject.SetActive(false);
		}
		m_roles.Add(temp1);
		for (int i = 0; i < game_data._instance.m_dbc_class.get_y(); ++i)
		{
			int class_id = int.Parse(game_data._instance.m_dbc_class.get(0, i));
			if (class_id > 1000)
			{
				continue;
			}
			s_t_class t_class = game_data._instance.get_t_class(class_id);
			if(t_class.color >= 4)
			{
				ccard c_card =  sys._instance.m_self.get_card_id(class_id);
				if(sys._instance.m_self.has_card(class_id))
				{
					my_cards.Add(c_card);
				}
				else
				{
					non_cards.Add(t_class);
				}
			}
		}
		my_cards.Sort (bu_zheng_panel.compare);
		for(int i = 0; i < my_cards.Count;++i)
		{
			GameObject temp = Instantiate(m_sub) as GameObject;
			temp.transform.parent = m_view.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,157 - 93 * (i+1),0);
			temp.SetActive(true);
			temp.name = "sub" + (i+1).ToString();
			temp.transform.Find("back").gameObject.SetActive(false);
			temp.transform.Find("icon").gameObject.SetActive(true);
			temp.transform.Find("zhushou_icon").gameObject.SetActive(false);
			GameObject icon = icon_manager._instance.create_card_icon(my_cards[i]);
			icon.transform.parent = temp.transform.Find("icon");
			icon.transform.localPosition = new Vector3(0, 0, 0);
			icon.transform.localScale = new Vector3(1, 1, 1);
			icon.transform.GetComponent<BoxCollider>().enabled = false;
			if (m_index == (i+1))
			{
				temp.transform.Find("select").gameObject.SetActive(true);
			}
			else
			{
				temp.transform.Find("select").gameObject.SetActive(false);
			}
			m_roles.Add(temp);
		}

		for(int i = my_cards.Count; i < my_cards.Count + non_cards.Count;++i)
		{
			GameObject temp = Instantiate(m_sub) as GameObject;
			temp.transform.parent = m_view.transform;
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = new Vector3(0,157 - 93 * (i+1),0);
			temp.SetActive(true);
			temp.name = "sub" + (i+1).ToString();
			temp.transform.Find("back").gameObject.SetActive(true);
			temp.transform.Find("icon").gameObject.SetActive(true);
			temp.transform.Find("zhushou_icon").gameObject.SetActive(false);
			GameObject icon = icon_manager._instance.create_card_icon_ex(non_cards[i-my_cards.Count].id, 0, 0, 0);
			icon.transform.parent = temp.transform.Find("icon");
			icon.transform.localPosition = new Vector3(0, 0, 0);
			icon.transform.localScale = new Vector3(1, 1, 1);
			icon.transform.GetComponent<BoxCollider>().enabled = false;
			if (m_index == (i+1))
			{
				temp.transform.Find("select").gameObject.SetActive(true);
			}
			else
			{
				temp.transform.Find("select").gameObject.SetActive(false);
			}
			m_roles.Add(temp);
		}
		update_ui (m_index);
	}
	
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_DRESS_UNLOCK && (root_gui._instance.m_dress_gui == null || !root_gui._instance.m_dress_gui.activeSelf))
		{
			protocol.game.smsg_success _msg1 = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if (_msg1.success)
			{
				sys._instance.m_self.m_t_player.dress_ids.Add (t_dress_unlock.sz_id);

				sys._instance.m_self.m_t_player.dress_tuzhi -= t_dress_unlock.tz_num;
			
				string text = "";
				for(int i = 0; i < sys._instance.m_self.player_dress_check(t_dress_unlock.sz_id).Count;++i)
				{
					if(i ==0)
					{
						text += sys._instance.m_self.player_dress_check(t_dress_unlock.sz_id)[i];
					}
					else
					{
						text += "\n"+sys._instance.m_self.player_dress_check(t_dress_unlock.sz_id)[i];
					}
				}
				m_sx.SetActive(false);
				hide_Label _hide_Label = m_sx.GetComponent<hide_Label>();
				if(_hide_Label != null)
				{
					Destroy(_hide_Label);
				}
				m_sx.GetComponent<UILabel>().text = text;
				dacheng();
				update_ui (m_index);
			}
		}
	}

	public void dacheng()
	{
		if(m_sx.GetComponent<UILabel>().text != "")
		{
			m_sx.SetActive(true);
			TweenScale _scale = sys._instance.add_scale_anim(m_sx.gameObject,0.2f,0.5f,1.2f,0);
			EventDelegate.Add(_scale.onFinished, delegate() 
			                  {
				hide();
			},true);
		}
	}
	
	public void hide()
	{
		hide_Label _hide_Label = m_sx.GetComponent<hide_Label>();
		
		if(_hide_Label == null)
		{
			_hide_Label = m_sx.AddComponent<hide_Label>();
		}
		
		_hide_Label.m_time = 1.6f;
	}

	
	void IMessage.message(s_message message)
	{

	}
	
	void update_ui(int index)
	{
		m_tuzhi.GetComponent<BoxCollider>().enabled = false;
		m_item_effect.SetActive (false);
		m_duihuan.SetActive (true);
		if(index == 0)
		{
			s_message _new_msg = new s_message();
			
			_new_msg.m_type = "vr_show_unit";
			_new_msg.m_long.Add((ulong)99);
			
			cmessage_center._instance.add_message(_new_msg);
		}
		else
		{
			ccard card = my_cards [index-1];
			//s_t_class t_class = game_data._instance.get_t_class (card.get_template_id());
			
			s_message _new_msg = new s_message();
			
			_new_msg.m_type = "vr_show_unit";
			_new_msg.m_long.Add(card.get_guid());
			
			cmessage_center._instance.add_message(_new_msg);
		}
		if(index == 0)
		{
			m_shizhuang.SetActive(true);
			if(dress_gui.is_effect())
			{
				m_sz_effect.SetActive(true);
			}
			else
			{
				m_sz_effect.SetActive(false);
			}
		}
		else
		{
			m_sz_effect.SetActive(false);
			dbc m_dbc_role_dress = game_data._instance.get_dbc_role_dress();
			List<s_t_role_dress> m_dresss_role = new List<s_t_role_dress>();
			for(int i = 0;i < m_dbc_role_dress.get_y();++i )
			{
				int role_dress_id = int.Parse(m_dbc_role_dress.get (0,i));
				s_t_role_dress _t_role_dress = game_data._instance.get_t_role_dress(role_dress_id);
				if(_t_role_dress.role == my_cards [index-1].get_template_id() && _t_role_dress.hq_condition != 3)
				{
					m_dresss_role.Add(_t_role_dress);
				}
			}
			if(m_dresss_role.Count == 0)
			{
				m_shizhuang.SetActive(false);
			}
			else
			{
				m_shizhuang.SetActive(true);
			}
		}
		t_dress_unlock = null;
		foreach(int id in game_data._instance.m_dbc_dress_unlock.m_index.Keys)
		{
			s_t_dress_unlock _t_dress_unlock = game_data._instance.get_t_dress_unlock(id);
			s_t_dress _dress = game_data._instance.get_t_dress(_t_dress_unlock.sz_id);
			if(!sys._instance.m_self.m_t_player.dress_ids.Contains(_dress.id))
			{
				t_dress_unlock = game_data._instance.get_t_dress_unlock(id);
				break;
			}
		}
		if(t_dress_unlock == null)
		{
			m_duihuan.SetActive(false);
		}
		else
		{
			m_num.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.dress_tuzhi + "/" + t_dress_unlock.tz_num;
			if(sys._instance.m_self.m_t_player.dress_tuzhi >= t_dress_unlock.tz_num)
			{
				m_tuzhi.GetComponent<BoxCollider>().enabled = true;
				m_item_effect.SetActive(true);
			}
			m_bar.GetComponent<UIProgressBar>().value = (float)(sys._instance.m_self.m_t_player.dress_tuzhi) / (float) t_dress_unlock.tz_num;
		}
	}

	public static bool effect()
	{
		s_t_dress_unlock _dress_unlock = null;
		for(int i = 0;i < game_data._instance.m_dbc_dress_unlock.get_y();i ++)
		{
			bool flag  = false;
			int id = int.Parse( game_data._instance.m_dbc_dress_unlock.get(0,i));
			s_t_dress_unlock _t_dress_unlock = game_data._instance.get_t_dress_unlock(id);
			s_t_dress _dress = game_data._instance.get_t_dress(_t_dress_unlock.sz_id);
			for(int j =0 ;j < sys._instance.m_self.m_t_player.dress_ids.Count;++j)
			{
				if(_dress.id == sys._instance.m_self.m_t_player.dress_ids[j])
				{
					flag = true;
					break;
				}
			}
			if(!flag)
			{
				_dress_unlock = game_data._instance.get_t_dress_unlock(id);
				break;
			}
		}
		if(sys._instance.m_self.m_t_player.dress_ids.Count == game_data._instance.m_dbc_dress_unlock.get_y()+2)
		{
			_dress_unlock = null;
		}
		if(_dress_unlock == null)
		{
			return false;
		}
		else
		{
			if(sys._instance.m_self.m_t_player.dress_tuzhi >= _dress_unlock.tz_num)
			{
				return true;
			}
		}
		return false;
	}
	
	
	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);

            root_gui._instance.show_mask();

			sys._instance.m_game_state = "hall";
			sys._instance.load_scene(sys._instance.m_hall_name);
		}

		if (obj.name.Length >= 3 && obj.name.Substring(0, 3) == "sub")
		{
			int index = int.Parse(obj.name.Substring(3, obj.name.Length - 3));
			if(index > my_cards.Count)
			{
				return;
			}
			m_roles[m_index].transform.Find("select").gameObject.SetActive(false);
			m_roles[index].transform.Find("select").gameObject.SetActive(true);
			m_index = index;
			update_ui(m_index);
		}
		if(obj.transform.name == "vr")
		{
			m_vr.SetActive(false);
			m_ui.SetActive(true);
			
			s_message _out = new s_message();
			
			_out.m_type = "hide_vr";
			
			cmessage_center._instance.add_message(_out);
		}
		
		if(obj.transform.name == "show_vr")
		{
			m_vr.SetActive(true);
			m_ui.SetActive(false);
			
			s_message _out = new s_message();
			
			_out.m_type = "show_vr";
			
			cmessage_center._instance.add_message(_out);
		}
		if(obj.transform.name == "sz")
		{
			if(m_index == 0)
			{
				this.gameObject.SetActive(false);
				s_message _out = new s_message();
				
				_out.m_type = "show_dress_gui";
				_out.m_string.Add("hide_dress_gui");
				
				cmessage_center._instance.add_message(_out);
			}
			else
			{
				this.gameObject.SetActive(false);
				ulong _gui = my_cards [m_index-1].get_guid();

				s_message _message = new s_message();
				_message.m_type = "show_hb_dress_gui";
				_message.m_long.Add ((ulong)_gui);
				_message.m_string.Add("hide_hb_dress_gui");
				cmessage_center._instance.add_message(_message);
			}
		}
		if(obj.transform.name == "duihuan")
		{
			protocol.game.cmsg_dress_unlock _msg = new protocol.game.cmsg_dress_unlock ();
			_msg.dress_id = t_dress_unlock.sz_id;
			net_http._instance.send_msg<protocol.game.cmsg_dress_unlock> (opclient_t.CMSG_DRESS_UNLOCK, _msg);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
