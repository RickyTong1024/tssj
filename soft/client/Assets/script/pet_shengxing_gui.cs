
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pet_shengxing_gui : MonoBehaviour,IMessage {

	public ulong m_guid;
	public GameObject m_star;
	public UILabel m_cur_hp;
	public UILabel m_cur_attack;
	public UILabel m_cur_wf;
	public UILabel m_cur_mf;
	public UILabel m_cur_jn;
	public UILabel m_cur_skill;
	public UILabel m_next_hp;
	public UILabel m_next_attack;
	public UILabel m_next_wf;
	public UILabel m_next_mf;
	public UILabel m_next_skill;
	public UILabel m_cur_hp_add;
	public UILabel m_cur_attack_add;
	public UILabel m_cur_wf_add;
	public UILabel m_cur_mf_add;
	public UILabel m_next_hp_add;
	public UILabel m_next_attack_add;
	public UILabel m_next_wf_add;
	public UILabel m_next_mf_add;
	public UILabel m_next_jn_add;
	public GameObject m_cl_icon;
	public GameObject m_sp_icon;
	public List<GameObject> m_ups = new List<GameObject>();
	public List<GameObject> m_add_ups = new List<GameObject>();
	public GameObject m_level;
	public UILabel m_gold;
	public GameObject m_tishi;
	public GameObject m_show_Label;
	public string m_message = "";
	dhc.pet_t m_t_pet;
	private pet m_pet;

	private int stone_id = 110010001;
	// Use this for initialization
	void Start () {
		
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void reset(ulong guid)
	{
		m_guid = guid;
		m_pet = sys._instance.m_self.get_pet_guid (m_guid);
		m_t_pet = m_pet.get_pet ();
		update_ui ();
	}

	public void update_ui()
	{
		m_cur_hp.text = m_pet.get_pet_shengxing_attr(1,m_t_pet.star).ToString("f0");
		m_cur_attack.text = m_pet.get_pet_shengxing_attr(2,m_t_pet.star).ToString("f0");
		m_cur_wf.text = m_pet.get_pet_shengxing_attr(3,m_t_pet.star).ToString("f0");
		m_cur_mf.text =m_pet.get_pet_shengxing_attr(4,m_t_pet.star).ToString("f0");
		m_cur_hp_add.text = m_pet.get_pet_shengxing_cz_attr(1,m_t_pet.star).ToString("f0");
		m_cur_attack_add.text = m_pet.get_pet_shengxing_cz_attr(2,m_t_pet.star).ToString("f0");
		m_cur_wf_add.text = m_pet.get_pet_shengxing_cz_attr(3,m_t_pet.star).ToString("f0");
		m_cur_mf_add.text = m_pet.get_pet_shengxing_cz_attr(4,m_t_pet.star).ToString("f0");
		m_cur_jn.text = m_pet.m_t_pet.shengxing_jncz*m_pet.get_star()*100 + "%";
		s_t_pet_skill t_skill = game_data._instance.get_t_pet_skill(m_pet.m_t_pet.skills[1] + m_t_pet.star);
		m_cur_skill.text = t_skill.des;
		m_star.GetComponent<UISprite>().width = 38 * m_pet.get_star ();
		s_t_pet_shengxing t_shengxing = game_data._instance.get_t_pet_shengxing (m_t_pet.star+1);
		if(t_shengxing == null)
		{
			m_gold.text = "----";
			m_next_hp.text = m_pet.get_pet_shengxing_attr(1,m_t_pet.star).ToString("f0");
			m_next_attack.text = m_pet.get_pet_shengxing_attr(2,m_t_pet.star).ToString("f0");
			m_next_wf.text = m_pet.get_pet_shengxing_attr(3,m_t_pet.star).ToString("f0");
			m_next_mf.text =m_pet.get_pet_shengxing_attr(4,m_t_pet.star).ToString("f0");
			m_next_hp_add.text = m_pet.get_pet_shengxing_cz_attr(1,m_t_pet.star).ToString("f0");
			m_next_attack_add.text = m_pet.get_pet_shengxing_cz_attr(2,m_t_pet.star).ToString("f0");
			m_next_wf_add.text = m_pet.get_pet_shengxing_cz_attr(3,m_t_pet.star).ToString("f0");
			m_next_mf_add.text = m_pet.get_pet_shengxing_cz_attr(4,m_t_pet.star).ToString("f0");
			m_next_jn_add.text = m_pet.m_t_pet.shengxing_jncz*m_pet.get_star()*100 + "%";
			s_t_pet_skill _skill = game_data._instance.get_t_pet_skill(m_pet.m_t_pet.skills[1] + m_t_pet.star);
			m_next_skill.text = _skill.des;
			for(int i = 0; i < m_ups.Count;++i)
			{
				m_ups[i].SetActive(false);
				m_add_ups[i].SetActive(false);
			}
			m_level.GetComponent<UILabel>().text = "----";
			m_tishi.SetActive(true);
			sys._instance.remove_child (m_sp_icon);
			int sp_num = sys._instance.m_self.get_item_num((uint)m_pet.get_fragment_id());
			GameObject iicon1 = icon_manager._instance.create_item_icon_ex(m_pet.get_fragment_id(),sp_num,-1);
			iicon1.transform.parent =  m_sp_icon.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			iicon1.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message = iicon1.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_sp_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";

			sys._instance.remove_child (m_cl_icon);
			int cl_num = sys._instance.m_self.get_item_num((uint)stone_id);
			GameObject iicon3 = icon_manager._instance.create_item_icon_ex(stone_id, cl_num, -1);
			iicon3.transform.parent =  m_cl_icon.transform;
			iicon3.transform.localPosition = new Vector3(0,0,0);
			iicon3.transform.localScale = new Vector3(1,1,1);
			iicon3.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message1 = iicon3.transform.GetComponents<UIButtonMessage>();
			message1[0].target = this.gameObject;
			message1[0].functionName = "click_item_icon";
			message1[1].target = null;
			message1[1].functionName = "";
			message1[2].target = null;
			message1[2].functionName = "";
		}
		else
		{
			int index = m_pet.get_color() -2;
			int gold = t_shengxing.gold[index];
			m_next_hp.text = m_pet.get_pet_shengxing_attr(1,m_t_pet.star+1).ToString("f0");
			m_next_attack.text = m_pet.get_pet_shengxing_attr(2,m_t_pet.star+1).ToString("f0");
			m_next_wf.text = m_pet.get_pet_shengxing_attr(3,m_t_pet.star+1).ToString("f0");
			m_next_mf.text =m_pet.get_pet_shengxing_attr(4,m_t_pet.star+1).ToString("f0");
			m_next_hp_add.text = m_pet.get_pet_shengxing_cz_attr(1,m_t_pet.star+1).ToString("f0");
			m_next_attack_add.text = m_pet.get_pet_shengxing_cz_attr(2,m_t_pet.star+1).ToString("f0");
			m_next_wf_add.text = m_pet.get_pet_shengxing_cz_attr(3,m_t_pet.star+1).ToString("f0");
			m_next_mf_add.text = m_pet.get_pet_shengxing_cz_attr(4,m_t_pet.star+1).ToString("f0");
			m_next_jn_add.text = m_pet.m_t_pet.shengxing_jncz*(m_pet.get_star()+1)*100 + "%";
			for(int i = 0; i < m_ups.Count;++i)
			{
				m_ups[i].SetActive(true);
				m_add_ups[i].SetActive(true);
			}
			if(sys._instance.m_self.m_t_player.gold > gold)
			{
				m_gold.text = sys._instance.get_res_color(1) +"x"+gold;
			}
			else
			{
				m_gold.text = "[ff0000]x" + gold;
			}
			m_tishi.SetActive(false);
			if(m_t_pet.level < t_shengxing.need_level)
			{
				m_level.GetComponent<UILabel>().text = "[ff0000]" +t_shengxing.need_level.ToString();
			}
			else
			{
				m_level.GetComponent<UILabel>().text = "[0aff16]" + t_shengxing.need_level.ToString();
			}
			int sp_num = sys._instance.m_self.get_item_num((uint)m_pet.get_fragment_id());
			sys._instance.remove_child (m_sp_icon);
			GameObject iicon1 = icon_manager._instance.create_item_icon_ex(m_pet.get_fragment_id(),sp_num,t_shengxing.sp[index]);
			iicon1.transform.parent =  m_sp_icon.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			iicon1.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message = iicon1.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_sp_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";
			
			sys._instance.remove_child (m_cl_icon);
			int cl_num = sys._instance.m_self.get_item_num((uint)stone_id);
			GameObject iicon3 = icon_manager._instance.create_item_icon_ex(stone_id, cl_num, t_shengxing.stone[index]);
			iicon3.transform.parent =  m_cl_icon.transform;
			iicon3.transform.localPosition = new Vector3(0,0,0);
			iicon3.transform.localScale = new Vector3(1,1,1);
			iicon3.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message1 = iicon3.transform.GetComponents<UIButtonMessage>();
			message1[0].target = this.gameObject;
			message1[0].functionName = "click_item_icon";
			message1[1].target = null;
			message1[1].functionName = "";
			message1[2].target = null;
			message1[2].functionName = "";
			s_t_pet_skill _skill = game_data._instance.get_t_pet_skill(m_pet.m_t_pet.skills[1] + m_t_pet.star+1);
			m_next_skill.text = _skill.des;
		}
	}

	public void click_item_icon(GameObject obj)
	{
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add ((int)stone_id);
		cmessage_center._instance.add_message(message);
	}

	public void click_sp_icon(GameObject obj)
	{
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add ((int)m_pet.get_fragment_id());
		cmessage_center._instance.add_message(message);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "sj")
		{
			s_t_pet_shengxing t_shengxing = game_data._instance.get_t_pet_shengxing (m_t_pet.star+1);
			int index = m_pet.m_t_pet.color - 2;
			if(t_shengxing == null)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_shengxing_gui.cs_220_46"));//[ffc882]该宠物已升至最高星级
				return;
			}
			if(m_t_pet.level < t_shengxing.need_level)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_shengxing_gui.cs_225_46"));//[ffc882]宠物等级不足
				return;
			}
			int cl_num = sys._instance.m_self.get_item_num((uint)stone_id);
			if(cl_num < t_shengxing.stone[index])
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)stone_id);
				cmessage_center._instance.add_message(message);
				return;
			}
			cl_num = sys._instance.m_self.get_item_num((uint)m_pet.get_fragment_id());
			if(cl_num < t_shengxing.sp[index])
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)m_pet.get_fragment_id());
				cmessage_center._instance.add_message(message);
				return;
			}
			if(sys._instance.m_self.m_t_player.gold < t_shengxing.gold[index])
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_jingjie_gui.cs_346_46"));//[ffc882]金币不足
				return;
			}
			protocol.game.cmsg_pet_star _msg = new protocol.game.cmsg_pet_star ();
			_msg.guid = m_guid;
			net_http._instance.send_msg<protocol.game.cmsg_pet_star> (opclient_t.CMSG_PET_STAR, _msg);
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

	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_PET_STAR)
		{
			s_t_pet_shengxing t_shengxing = game_data._instance.get_t_pet_shengxing (m_t_pet.star+1);
			int index = m_pet.m_t_pet.color - 2;
			int gold = t_shengxing.gold[index];
			int sp_num = t_shengxing.sp[index];
			int stone_num = t_shengxing.stone[index];
			int sp_id = m_pet.get_fragment_id();
			sys._instance.m_self.sub_att(e_player_attr.player_gold,gold,game_data._instance.get_t_language ("pet_shengxing_gui.cs_279_63"));//宠物升星消耗
            sys._instance.m_self.remove_item((uint)sp_id, sp_num, game_data._instance.get_t_language ("pet_shengxing_gui.cs_279_63"));//宠物升星消耗
            sys._instance.m_self.remove_item((uint)stone_id, stone_num, game_data._instance.get_t_language ("pet_shengxing_gui.cs_279_63"));//宠物升星消耗
			m_t_pet.star += 1;
			update_ui();
			s_message _message3 = new s_message();
			_message3.m_type = "check_bf";
			cmessage_center._instance.add_message(_message3);
			m_show_Label.SetActive(true);
			Label_time();
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

	public static bool is_shengxing(pet m_pet)
	{
		if(m_pet == null)
		{
			return false;
		}
		s_t_pet_shengxing t_pet_shengxing = game_data._instance.get_t_pet_shengxing (m_pet.get_star ()+1);
		if(t_pet_shengxing == null)
		{
			return false;
		}
		int index = m_pet.m_t_pet.color - 2;
		if(sys._instance.m_self.m_t_player.level < t_pet_shengxing.need_level)
		{
			return false;
		}
		if(sys._instance.m_self.m_t_player.gold < t_pet_shengxing.gold[index])
		{
			return false;
		}
		int cl_num = sys._instance.m_self.get_item_num ((uint)110010001);
		if(cl_num < t_pet_shengxing.stone[index])
		{
			return false;
		}
		cl_num = sys._instance.m_self.get_item_num ((uint)m_pet.get_fragment_id());
		if(cl_num < t_pet_shengxing.sp[index])
		{
			return false;
		}
		return true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
