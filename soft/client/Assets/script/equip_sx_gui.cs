
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip_sx_gui : MonoBehaviour,IMessage {

	dhc.equip_t m_equip;
	public GameObject m_effect;
	public GameObject m_center;
	public bool m_play = false;
	private string m_out_message;
	public List<GameObject> m_old_attr = new List<GameObject>();
	public List<GameObject> m_new_attr = new List<GameObject>();
	public GameObject m_tishi1;
	public GameObject m_jj;
	public GameObject m_old_icon;
	public GameObject m_new_icon;
	public GameObject m_zuigao;
	private string error = "";


	// Use this for initialization
	void Start ()
	{
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	public void reset(dhc.equip_t _equip, string out_message)
	{
		m_out_message = out_message;
		GameObject _main = m_center.transform.Find("main").gameObject;
		m_equip = _equip;
		GameObject icon1 = _main.transform.Find("icon1").gameObject;
		sys._instance.remove_child (icon1);

		sys._instance.remove_child (m_old_icon);
		GameObject iicon2 = icon_manager._instance.create_equip_icon(m_equip.template_id, m_equip.enhance, m_equip.star);
		iicon2.transform.parent =  m_old_icon.transform;
		iicon2.transform.localPosition = new Vector3(0,0,0);
		iicon2.transform.localScale = new Vector3(1,1,1);
		iicon2.transform.GetComponent<BoxCollider>().enabled = false;
		
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		s_t_equip_sx _equip_sx  = game_data._instance.get_t_equip_sx (t_equip.font_color,m_equip.star+1);
		if(_equip_sx != null)
		{
			m_tishi1.SetActive(false);
			m_new_icon.SetActive(true);
			sys._instance.remove_child (m_new_icon);
			int sp_num = sys._instance.m_self.get_item_num((uint)equip.get_equip_frame_id(m_equip));
			GameObject iicon1 = icon_manager._instance.create_item_icon_ex(equip.get_equip_frame_id(m_equip),sp_num,_equip_sx.sp_num);
			iicon1.transform.parent =  icon1.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			iicon1.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message = iicon1.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_item_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";

			GameObject iicon3 = icon_manager._instance.create_equip_icon(m_equip.template_id, m_equip.enhance, m_equip.star+1);
			iicon3.transform.parent =  m_new_icon.transform;
			iicon3.transform.localPosition = new Vector3(0,0,0);
			iicon3.transform.localScale = new Vector3(1,1,1);
			iicon3.transform.GetComponent<BoxCollider>().enabled = false;

			for(int i = 0; i < m_old_attr.Count;++i)
			{
				m_old_attr[i].SetActive(true);
				m_old_attr[i].GetComponent<UILabel>().text = equip.get_equip_sx_text(m_equip,m_equip.star,i);
				if(m_old_attr[i].GetComponent<UILabel>().text == "0")
				{
					m_old_attr[i].SetActive(false);
				}
			}
			for(int i = 0; i < m_new_attr.Count;++i)
			{
				m_new_attr[i].SetActive(true);
				m_new_attr[i].GetComponent<UILabel>().text = equip.get_equip_sx_text(m_equip,m_equip.star+1,i);
				if(m_new_attr[i].GetComponent<UILabel>().text == "0")
				{
					m_new_attr[i].SetActive(false);
				}
			}
			string cl = sys._instance.get_res_color(1);
            int gold_num = sys._instance.m_self.get_att(e_player_attr.player_gold);
			if (gold_num < _equip_sx.gold)
			{
				cl = "[ff0000]";
				error = game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
			}
			if(sp_num < _equip_sx.sp_num)
			{
				error = "null";
			}
			m_jj.GetComponent<UILabel>().text = cl + _equip_sx.gold.ToString();
		}
		else
		{
			m_tishi1.SetActive(true);
			m_new_icon.SetActive(false);
			int sp_num = sys._instance.m_self.get_item_num((uint)equip.get_equip_frame_id(m_equip));
			GameObject iicon1 = icon_manager._instance.create_item_icon_ex(equip.get_equip_frame_id(m_equip),sp_num,-1);
			iicon1.transform.parent =  icon1.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			iicon1.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message = iicon1.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_item_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";

			for(int i = 0; i < m_old_attr.Count;++i)
			{
				m_old_attr[i].GetComponent<UILabel>().text = equip.get_equip_sx_text(m_equip,m_equip.star, i);
			}
			for(int i = 0; i < m_new_attr.Count;++i)
			{
				m_new_attr[i].SetActive(false);
			}
			m_jj.GetComponent<UILabel>().text = "--";
			error = game_data._instance.get_t_language ("equip_sx_gui.cs_135_11");//当前已升至最高星级
		}

		_main.transform.Find("name").GetComponent<UILabel>().text = t_equip.name;
		m_zuigao.GetComponent<UILabel>().text = "(" + string.Format(game_data._instance.get_t_language ("equip_sx_gui.cs_139_62"),(t_equip.font_color + 1)) + ""+")";//此装备最高可升至{0}星
		

		_main.transform.Find("jinglian").GetComponent<UIButton>().isEnabled = true;
		
		m_play = false;
		m_effect.gameObject.SetActive (false);
	}

	public void click_item_icon(GameObject obj)
	{
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add ((int)equip.get_equip_frame_id(m_equip));
		cmessage_center._instance.add_message(message);
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

	public void click(GameObject obj)
	{
		if (obj.name == "close")
		{
			Object.Destroy(this.gameObject);
			
			s_message _msg = new s_message();
			_msg.m_ints.Add(2);
			_msg.m_type = m_out_message;
			_msg.m_long.Add(m_equip.guid);
			cmessage_center._instance.add_message(_msg);
		}
		if(obj.name == "jinglian")
		{
			if(error != "" && error != "null")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + error);
				return;
			}
			else if(error != "" && error == "null")
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)equip.get_equip_frame_id(m_equip));
				cmessage_center._instance.add_message(message);
				return;
			}
			protocol.game.cmsg_equip_star _msg = new protocol.game.cmsg_equip_star ();
			_msg.star_guid = m_equip.guid;
			net_http._instance.send_msg<protocol.game.cmsg_equip_star> (opclient_t.CMSG_EQUIP_STAR, _msg);
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_EQUIP_STAR)
		{
			s_t_equip _equip = game_data._instance.get_t_equip(m_equip.template_id);
			s_t_equip_sx _equip_sx= game_data._instance.get_t_equip_sx(_equip.font_color,m_equip.star+1);
			int sp_id = equip.get_equip_frame_id(m_equip);
            sys._instance.m_self.sub_att(e_player_attr.player_gold, _equip_sx.gold,game_data._instance.get_t_language ("equip_sx_gui.cs_212_83"));//装备升星消耗
			sys._instance.m_self.remove_item((uint)sp_id,_equip_sx.sp_num,game_data._instance.get_t_language ("equip_sx_gui.cs_212_83"));//装备升星消耗
			m_equip.star += 1;
			m_effect.GetComponent<Animator>().speed = 4.0f;
			m_effect.gameObject.SetActive (true);
			sys._instance.play_sound ("sound/gaizao");
			m_play = true;
			
			GameObject _main = m_center.transform.Find("main").gameObject;
			
			_main.transform.Find("jinglian").GetComponent<UIButton>().isEnabled = false;
		}
	}

	void IMessage.message(s_message message)
	{

	}

	// Update is called once per frame
	void Update () {
		if (m_play && isplay() )
		{
			m_play = false;
			reset(m_equip, m_out_message);
			
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
		}
	}
}
