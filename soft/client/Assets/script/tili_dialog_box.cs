
using UnityEngine;
using System.Collections;

public class tili_dialog_box : MonoBehaviour ,IMessage{

	public GameObject m_name;
	public GameObject m_icon;
	public GameObject m_input;
	public GameObject m_num;
	private int type = 0;
	private uint item_id = 0;
	private int m_input_num = 1;

	public UILabel m_ok_Label;
	public UILabel m_close_Label;
	public UILabel m_tiltle;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void set_des(s_message msg)
	{
		int id = (int)msg.m_ints [0];
		item_id = (uint)id;
		type = (int)msg.m_ints [1];
		m_input_num = 1;
		updata_ui ();
	}

	public void hide()
	{
		this.transform.Find("frame_big").GetComponent<frame>().hide();
		
	}

	public void updata_ui()
	{
		GameObject _icon = icon_manager._instance.create_item_icon((int)item_id,0,0);
		sys._instance.remove_child (m_icon);
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (2, (int)item_id, 1, 0);
		m_input.GetComponent<UILabel>().text = (m_input_num).ToString ();
		m_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("card_dialog_box.cs_81_62")," "+sys._instance.m_self.get_item_num ((uint)item_id).ToString ());//当前拥有：{0}
		if(type == 0)
		{
			m_tiltle.text = game_data._instance.get_t_language ("tili_dialog_box.cs_54_19");//道具使用
		}
		else
		{
			m_tiltle.text = game_data._instance.get_t_language ("tili_dialog_box.cs_58_19");//基因改造
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "hide") 
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			return;
		}
		int toltle_num = sys._instance.m_self.get_item_num (item_id);
		if(toltle_num >= 100)
		{
			toltle_num = 100;
		}
		if (obj.name == "add") 
		{
			if(m_input_num + 1 <= toltle_num)
			{
				m_input_num += 1;
			}
		}
		else if(obj.name == "sub")
		{
			if (m_input_num > 1)
			{
				m_input_num--;
			}
		}
		else if(obj.name == "add10")
		{
			if(m_input_num + 10 <= toltle_num)
			{
				m_input_num += 10;
			}
			else
			{
				m_input_num = toltle_num;
			}
		}
		else if(obj.name == "sub10")
		{
			if (m_input_num - 10 >= 1) 
			{
				m_input_num -= 10;
			}
			else
			{
				m_input_num = 1;
			}
		}
        else if (obj.name == "queding")
        {
			if(type == 0)
			{
            	ok();
			}
 			else
			{
				s_message _msg = new s_message();
				_msg.m_type = "common_select_jiyin";
				_msg.m_ints.Add((int)item_id);
				_msg.m_ints.Add (m_input_num);
				cmessage_center._instance.add_message (_msg);
				this.transform.Find("frame_big").GetComponent<frame>().hide();
				return;
			}
        }
		updata_ui ();
	}

	public void ok()
	{
		
		protocol.game.cmsg_item_apply _msg = new protocol.game.cmsg_item_apply ();
		_msg.item_id = item_id;
		_msg.item_count = m_input_num;
		net_http._instance.send_msg<protocol.game.cmsg_item_apply> (opclient_t.CMSG_ITEM_APPLY, _msg);	
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_ITEM_APPLY && this.gameObject.activeSelf)
		{
           
			protocol.game.smsg_item_apply _msg = net_http._instance.parse_packet<protocol.game.smsg_item_apply> (message.m_byte);
			s_t_item t_item = game_data._instance.get_item((int)item_id);
			for(int c = 0;c < _msg.types.Count;c ++)
			{
				sys._instance.m_self.add_reward(_msg.types[c], _msg.value1s[c], _msg.value2s[c], _msg.value3s[c], true,game_data._instance.get_t_language ("bag_gui.cs_301_124"));//物品使用获得
			}
            for (int i = 0; i < _msg.pets.Count; i++)
            {
                sys._instance.m_self.add_pet(_msg.pets[i]);
            }
			sys._instance.m_self.remove_item(item_id, m_input_num,game_data._instance.get_t_language ("bag_gui.cs_183_75"));//物品使用消耗
			updata_ui();
			s_message _mes = new s_message();
			_mes.m_type = "update_cn_gui";
			cmessage_center._instance.add_message(_mes);

			s_message _message = new s_message();
			_message.m_type = "refresh_mowang_gui";
			cmessage_center._instance.add_message(_message);

            s_message _message1 = new s_message();
            _message1.m_type = "refresh_bw_gui";
            cmessage_center._instance.add_message(_message1);

			s_message _message2 = new s_message();
			_message2.m_type = "refresh_buttle_gui";
			cmessage_center._instance.add_message(_message2);

            s_message _message3 = new s_message();
            _message3.m_type = "refresh_bag_gui1";
            cmessage_center._instance.add_message(_message3);

			s_message _message4 = new s_message();
			_message4.m_type = "refresh_huo_dong_gui";
			cmessage_center._instance.add_message(_message4);

            s_message _message5 = new s_message();
            _message5.m_type = "refresh_select_num";
            cmessage_center._instance.add_message(_message5);
            this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	void IMessage.message(s_message message)
	{

	}
	// Update is called once per frame
	void Update () {
	
	}
}
