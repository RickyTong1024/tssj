
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class qiyu_gui : MonoBehaviour,IMessage {
    public GameObject m_up;
    public GameObject m_down;
    public GameObject m_buttle_tip;
    public GameObject m_view;

    //buttle_tip
    public GameObject m_topicon;
    public UILabel m_name;
    public UILabel m_desc;
    public UILabel m_tili;
    public UILabel m_gold;
    public UILabel m_exp;
    public GameObject m_icon;
    public UILabel m_num;
    public int m_mission_index;

	// Use this for initialization
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	
	}
    void OnEnable()
    {

        reset();
    }
    public static bool qiyu_effect()
    {
        bool flag = false;
        for (int i = 0; i < sys._instance.m_self.m_t_player.qiyu_suc.Count; i++)
        {
            if (sys._instance.m_self.m_t_player.qiyu_suc[i] == 0)
            {
                flag = true;
            }
        }

        if (flag && sys._instance.m_self.m_t_player.qiyu_mission.Count > 0)
        {
            return true;
        }
        return false;
    }
    void reset()
    {
        m_buttle_tip.SetActive(false);
        bool flag = false;
        for (int i = 0; i < sys._instance.m_self.m_t_player.qiyu_suc.Count; i++)
        {
            if (sys._instance.m_self.m_t_player.qiyu_suc[i] == 0)
            {
                flag = true;
            }
        }
        if (flag)
        {
            m_down.SetActive(true);
            m_up.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);
 
        }
    }
    void IMessage.message(s_message message)
    {
    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_QIYU_FIGHT_END)
        {
            protocol.game.smsg_qiyu_fight_end _msg = net_http._instance.parse_packet<protocol.game.smsg_qiyu_fight_end>(message.m_byte);
            if (_msg.result == 1)
            {
                sys._instance.m_self.m_t_player.qiyu_suc[m_mission_index] = _msg.result;
            }
            battle_logic_ex._instance.set_qiyu_fight_end(_msg);
            s_t_mission _mission = game_data._instance.get_t_mission(sys._instance.m_self.m_t_player.qiyu_mission[m_mission_index]);
            s_t_map _map = game_data._instance.get_t_map(_mission.map_id);
            s_t_qiyu_tiaozhan _qiyu = game_data._instance.get_t_qiyu_tiaozhan(_map.id);

			if (_msg.result == 1)
			{
                sys._instance.m_self.add_active(2400,1);
				int hard = sys._instance.m_self.m_t_player.qiyu_hard[m_mission_index];
            	sys._instance.m_self.sub_att(e_player_attr.player_tili, _qiyu.tili * (hard + 1));
			}
          
			sys._instance.load_scene_ex(_mission.map_name);
            sys._instance.m_game_state = "buttle";
 
        }
    }
	// Update is called once per frame
	void Update () 
    {
	
	}
    void click(GameObject obj)
    {
        if (obj.name == "down")
        {
            m_up.SetActive(true);
            m_down.SetActive(false);
            reset_item();
 
        }
        else if (obj.name == "up1")
        {
            m_up.SetActive(false);
            m_down.SetActive(true);
 
        }
        else if (obj.name == "duixing")
        {
            s_message _message = new s_message();
            _message.m_type = "show_duixing_gui";
			_message.m_bools.Add(true);
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.name == "attack")
        {
            if (m_mission_index < 0 || m_mission_index >= sys._instance.m_self.m_t_player.qiyu_mission.Count)
            {
                m_buttle_tip.transform.Find("frame_big").GetComponent<frame>().hide();
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("qiyu_gui.cs_132_71"));//奇遇挑战已结束
                reset();
                return;
            }
            protocol.game.cmsg_qiyu_fight_end msg = new protocol.game.cmsg_qiyu_fight_end();
            msg.index = m_mission_index;
            net_http._instance.send_msg<protocol.game.cmsg_qiyu_fight_end>(opclient_t.CMSG_QIYU_FIGHT_END, msg);
        }
        else
        {
            m_mission_index = int.Parse(obj.name);
            if (m_mission_index < 0 || m_mission_index >= sys._instance.m_self.m_t_player.qiyu_mission.Count)
            {
                m_buttle_tip.transform.Find("frame_big").GetComponent<frame>().hide();
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("qiyu_gui.cs_132_71"));//奇遇挑战已结束
                reset();
                return;
            }
            if (sys._instance.m_self.m_t_player.qiyu_suc[m_mission_index] == 1)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("qiyu_gui.cs_152_71"));//该女神已被您征服！
                return;
            }
            m_buttle_tip.SetActive(true);
           
            reset_buttle_tip();
        }
 
    }
    int compare(int x, int y)
    {
        s_t_mission _mission_x = game_data._instance.get_t_mission(x);
        s_t_mission _mission_y = game_data._instance.get_t_mission(y);

        return _mission_x.map_id - _mission_y.map_id;
    }

    void reset_item()
    {
        if (sys._instance.m_self.m_t_player.qiyu_mission.Count == 0)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("qiyu_gui.cs_132_71"));//奇遇挑战已结束
            reset();
            return;
        }
        sys._instance.remove_child(m_view);
        List<int> _ids = new List<int>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.qiyu_mission.Count; i++)
        {
            _ids.Add(sys._instance.m_self.m_t_player.qiyu_mission[i]);
        }
        _ids.Sort(compare);
        for (int i = 0; i < _ids.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/qiyu_sub");
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(100 * i, 0, 0);
            obj.transform.localScale = Vector3.one;
            obj.name = return_qiyu_index(_ids[i]) + "";
            s_t_mission _mission = game_data._instance.get_t_mission(_ids[i]);
            s_t_map _map = game_data._instance.get_t_map(_mission.map_id);
            s_t_qiyu_tiaozhan _tiaozhan = game_data._instance.get_t_qiyu_tiaozhan(_map.id);
            GameObject icon = icon_manager._instance.create_card_icon(_map.role_id, 0, 0, 0);
            sys._instance.remove_child(obj.transform.Find("icon").gameObject);
            icon.transform.parent = obj.transform.Find("icon");
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            icon.GetComponent<BoxCollider>().enabled = false;
            icon.name = return_qiyu_index(_ids[i]) + "";
            if (sys._instance.m_self.m_t_player.qiyu_hard[return_qiyu_index(_ids[i])] == 0)
            {
                icon.transform.Find("bg").GetComponent<UISprite>().spriteName = "xtbk_lanpt001";
            }
            else if (sys._instance.m_self.m_t_player.qiyu_hard[return_qiyu_index(_ids[i])] == 1)
            {
                icon.transform.Find("bg").GetComponent<UISprite>().spriteName = "xtbk_zipt001";
            }
            else if (sys._instance.m_self.m_t_player.qiyu_hard[return_qiyu_index(_ids[i])] == 2)
            {
                icon.transform.Find("bg").GetComponent<UISprite>().spriteName = "xtbk_chpt001";
            }
            int hard = sys._instance.m_self.m_t_player.qiyu_hard[return_qiyu_index(_ids[i])];
            string s = "";
            if (hard == 0)
            {
                s = "[0bbbf5]";
            }
            else if (hard == 1)
            {
                s = "[e928b8]";
            }
            else
            {
                s = "[f98c20]";
            }
            UIEventListener.Get(obj).onClick = click;
            obj.transform.Find("name").GetComponent<UILabel>().text = s + _map.boss_name;
            obj.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("qiyu_gui.cs_229_90"),_map.id %10000);//第{0}章
            if (sys._instance.m_self.m_t_player.qiyu_suc[return_qiyu_index(_ids[i])] == 1)
            {
                obj.transform.Find("gou").gameObject.SetActive(true);
            }
            else
            {
                obj.transform.Find("gou").gameObject.SetActive(false);

            }

        }
        if (sys._instance.m_self.m_t_player.qiyu_mission.Count > 0)
        {
            m_view.transform.localPosition = new Vector3(-50 * (sys._instance.m_self.m_t_player.qiyu_mission.Count - 1), 40, 0);
        }
 
    }
    int return_qiyu_index(int mission_id)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.qiyu_mission.Count; i++)
        {
            if (mission_id == sys._instance.m_self.m_t_player.qiyu_mission[i])
            {
                return i;
            }
        }
        return 0;
    }
    
    void reset_buttle_tip()
    {
        sys._instance.remove_child(m_topicon);
        sys._instance.remove_child(m_icon);
        s_t_mission _mission = game_data._instance.get_t_mission(sys._instance.m_self.m_t_player.qiyu_mission[m_mission_index]);
        s_t_map _map = game_data._instance.get_t_map(_mission.map_id);
        s_t_qiyu_tiaozhan _qiyu = game_data._instance.get_t_qiyu_tiaozhan(_map.id);
        GameObject _icon = icon_manager._instance.create_card_icon_ex(_map.role_id, 0, 0, 0);
        _icon.GetComponent<Collider>().enabled = false;
        _icon.transform.parent = m_topicon.transform;
        _icon.transform.localPosition = Vector3.zero;
        _icon.transform.localScale = Vector3.one;
        int hard = sys._instance.m_self.m_t_player.qiyu_hard[m_mission_index];
        if (hard == 0)
        {
            _icon.transform.Find("bg").GetComponent<UISprite>().spriteName = "xtbk_lanpt001";
        }
        else if (hard == 1)
        {
            _icon.transform.Find("bg").GetComponent<UISprite>().spriteName = "xtbk_zipt001";
        }
        else if (hard == 2)
        {
            _icon.transform.Find("bg").GetComponent<UISprite>().spriteName = "xtbk_chpt001";
        }
        string s = "";
        if (hard == 0)
        {
            s = "[0bbbf5]";
        }
        else if (hard == 1)
        {
            s = "[e928b8]";
        }
        else
        {
            s = "[f98c20]";
        }
        m_name.text = s + game_data._instance.get_t_class(_map.role_id).name;
        
        m_tili.text = sys._instance.get_res_color(3)  + _qiyu.tili * (hard + 1) + "";
		m_gold.transform.GetComponent<UILabel>().text = sys._instance.get_res_color(1) + (int)(_qiyu.tili * sys._instance.m_self.m_t_player.level * 10 + 50) + "";
		m_exp.transform.GetComponent<UILabel>().text = sys._instance.get_res_color(4) + _qiyu.tili * 5;
        m_desc.text = _qiyu.desc;
        GameObject obj = icon_manager._instance.create_item_icon(50140001);
        obj.transform.parent = m_icon.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
		s_t_exp t_exp = game_data._instance.get_t_exp (sys._instance.m_self.m_t_player.level);
        if (hard == 0)
        {
			m_num.text = t_exp.dxqhzz + _qiyu.zhuangzhi + "-" + (int)((t_exp.dxqhzz + _qiyu.zhuangzhi) * 1.5f) + "";
        }
        else if (hard == 1)
        {
			m_num.text = (int)((t_exp.dxqhzz + _qiyu.zhuangzhi) * 1.2f) + "-" + (int)((t_exp.dxqhzz + _qiyu.zhuangzhi) * 1.5f * 1.2f) + "";
        }
        else if (hard == 2)
        {
			m_num.text = (int)((t_exp.dxqhzz + _qiyu.zhuangzhi) * 1.5f) + "-" + (int)((t_exp.dxqhzz + _qiyu.zhuangzhi) * 1.5f * 1.5f) + ""; 
        }
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
}
