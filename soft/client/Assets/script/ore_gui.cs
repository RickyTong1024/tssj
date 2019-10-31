
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ore_gui : MonoBehaviour,IMessage {

	public UILabel m_name;
	public UILabel m_tiaozhan;
	public UILabel m_des;
	public GameObject m_select;
	public GameObject m_level;
	public GameObject m_tili;
	int m_index = 0;
	public List<GameObject> m_nds = new List<GameObject>();
	public List<GameObject> m_icons = new List<GameObject>();
	public List<GameObject> m_nums = new List<GameObject>();

	public UILabel m_tioajian;
	public UILabel m_xiaohaotili;
	public UILabel m_yulan;
	///GameObject m_select_box;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
		for (int i = 0; i < m_nds.Count; ++i)
		{
			int index = int.Parse(game_data._instance.m_dbc_ore.get(0, i));
			s_t_ore t_ore = game_data._instance.get_t_ore(index);
			m_nds[i].transform.Find("Label").GetComponent<UILabel>().text = t_ore.nd;
		}
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	public void OnEnable()
	{
		for (int i = 0; i < m_nds.Count; ++i)
		{
			int index = int.Parse(game_data._instance.m_dbc_ore.get(0, i));
			s_t_ore t_ore = game_data._instance.get_t_ore(index);

			if (index > sys._instance.m_self.m_t_player.ore_nindex)
			{
				m_nds[i].transform.Find("lock").gameObject.SetActive(true);
			}
			else
			{
				m_nds[i].transform.Find("lock").gameObject.SetActive(false);
			}
		}
		reset (m_index);
	}

	void reset(int index)
	{
        for (int i = 0; i < m_icons.Count; i++)
        {
            sys._instance.remove_child(m_icons[i]);
        }
        List<GameObject> objs = new List<GameObject>();
        GameObject obj1 = icon_manager._instance.create_reward_icon(1,1,0,0);
        objs.Add(obj1);
        GameObject obj2 = icon_manager._instance.create_reward_icon(2, 50070001, 0, 0);
        objs.Add(obj2);
        GameObject obj3 = icon_manager._instance.create_reward_icon(6, 13001, 0, 0);
        objs.Add(obj3);
        GameObject obj4 = icon_manager._instance.create_reward_icon(2, 50100001, 0, 0);
        objs.Add(obj4);
        for (int i = 0; i < m_icons.Count; i++)
        {
            objs[i].transform.parent = m_icons[i].transform;
            objs[i].transform.localPosition = Vector3.zero;
            objs[i].transform.localScale = Vector3.one;
        }
		s_t_ore t_ore = game_data._instance.get_t_ore(index);
		for(int i = 0; i < m_icons.Count;++i)
		{
			m_icons[i].SetActive(true);
			m_nums[i].SetActive(true);
		}
		int gold = t_ore.bd_gold + t_ore.hx_gold;
		if(gold <= 0)
		{
			m_icons[0].SetActive(false);
			m_nums[0].SetActive(false);
		}
		else
		{
			m_nums[0].GetComponent<UILabel>().text = sys._instance.get_res_color(1) + string.Format(game_data._instance.get_t_language ("ore_gui.cs_93_91"),(t_ore.bd_gold/10000)//金币x{0}万-{1}万
				, (gold/10000));
		}
		if(t_ore.jl_js <= 0)
		{
			m_icons[1].SetActive(false);
			m_nums[1].SetActive(false);
		}
		else
		{
			s_t_item t_item = game_data._instance.get_item(50070001);
			string name = t_item.name + "x" + t_ore.jl_js;
			m_nums[1].GetComponent<UILabel>().text = ccard.get_color_name(name,t_item.font_color);
		}
		if(t_ore.zjs <= 0)
		{
			m_icons[2].SetActive(false);
			m_nums[2].SetActive(false);
		}
		else
		{
			s_t_baowu t_treasure = game_data._instance.get_t_baowu(13001);
			string color = treasure.get_treasure_color(13001);
			m_nums[2].GetComponent<UILabel>().text = color + t_treasure.name + "x" + t_ore.zjs;
		}
		if(t_ore.szjs <= 0)
		{
			m_icons[3].SetActive(false);
			m_nums[3].SetActive(false);
		}
		else
		{
			s_t_item t_item = game_data._instance.get_item(50100001);
			string name = t_item.name + "x" + t_ore.szjs;
			m_nums[3].GetComponent<UILabel>().text = ccard.get_color_name(name,t_item.font_color);
		}
		if(sys._instance.m_self.m_t_player.level < t_ore.level)
		{
			m_level.GetComponent<UILabel>().text = "[ff0000]Lv" + t_ore.level;
		}
		else
		{
			m_level.GetComponent<UILabel>().text = "[0aff16]Lv" + t_ore.level;
		}
		m_tili.GetComponent<UILabel>().text = "x"+t_ore.tili.ToString ();
	}

	void IMessage.net_message(s_net_message message)
	{
        if (message.m_opcode == opclient_t.CMSG_ORE_FIGHT_END)
        {
            protocol.game.smsg_ore_fight_end _msg = net_http._instance.parse_packet<protocol.game.smsg_ore_fight_end>(message.m_byte);

            battle_logic_ex._instance.set_ore_fight_end(_msg);
            s_message _new_msg = new s_message();
            _new_msg.m_type = "ore_index";
            _new_msg.m_ints.Add(m_index);
            cmessage_center._instance.add_message(_new_msg);
			sys._instance.load_scene_ex("ts_fight_xjbz");
            sys._instance.m_game_state = "buttle";
            this.gameObject.SetActive(false);
        }
	}

	void IMessage.message(s_message message)
	{


	}

	public void click(GameObject obj)
	{
		if (obj.name.Length >= 3 && obj.name.Substring(0, 5) == "item_")
		{
			int index = int.Parse(obj.name.Substring(5, obj.name.Length - 5));

			m_select.transform.localPosition = m_nds[index].transform.localPosition;

			m_index = index;
			reset(m_index);
		}
		if (obj.name == "tz")
		{
			s_t_ore t_ore = game_data._instance.get_t_ore(m_index);
			if(m_index > sys._instance.m_self.m_t_player.ore_nindex || sys._instance.m_self.m_t_player.level < t_ore.level)
			{
				string text  = string.Format(game_data._instance.get_t_language ("ore_gui.cs_179_33"),t_ore.level);//通关前一关并达到{0}级开启
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + text);
				return;
			}
			if(sys._instance.m_self.m_t_player.tili < t_ore.tili)
			{
				int item_id = 10010002;
				int num = sys._instance.m_self.get_item_num((uint)item_id);
				if(num > 0)
				{
					root_gui._instance.show_tili_dialog_box(item_id);
					return;
				}
				else
				{
					s_message _message = new s_message();
					_message.m_type = "buy_num_gui";
					_message.m_ints.Add(100200);
					_message.m_ints.Add(2);
					cmessage_center._instance.add_message(_message);
					return;
				}
			}
            if (sys._instance.m_self.bag_full())
            {
                this.gameObject.SetActive(false);
                return;
            }
            if (sys._instance.m_self.treasure_full())
            {
                this.gameObject.SetActive(false);
                return;
            }
			if(obj.transform.name == "duixing")
			{
				s_message _message = new s_message();
				_message.m_type = "show_duixing_gui";
				cmessage_center._instance.add_message(_message);
				return;
			}
			protocol.game.cmsg_ore_fight_end _msg = new protocol.game.cmsg_ore_fight_end ();
			_msg.index = m_index;
			net_http._instance.send_msg<protocol.game.cmsg_ore_fight_end> (opclient_t.CMSG_ORE_FIGHT_END, _msg);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
