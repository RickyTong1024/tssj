using System.Collections.Generic;
using UnityEngine;

public class guildboss_task_gui : MonoBehaviour,IMessage {

    public GameObject m_view;
    public s_t_guild_mission m_mission;
    public int id;
    int box_index;
    public protocol.game.smsg_guild_mission_look m_msg;
	public UILabel m_time;
	public GameObject m_select_sprite;
    protocol.game.smsg_guild_mission_complete_reward_view m_msg_view;
    public GameObject m_box_view;
    public GameObject sl_info_gui;
    public GameObject yulan_view;
    public UIToggle m_fisrt_shouwei;
    public UILabel m_yulan_time;
    int yulan_id;
    // Use this for initialization
    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }
    void OnEnable()
    {
        protocol.game.cmsg_guild_mission_complete_reward_view view = new protocol.game.cmsg_guild_mission_complete_reward_view();
        view.ceng = m_mission.index;
        net_http._instance.send_msg<protocol.game.cmsg_guild_mission_complete_reward_view>
            (opclient_t.CMSG_GUILD_MISSION_COMPLETE_REWARD_VIEW, view);
    }
    int get_state(int id)
    {
        id = id + 1;
        if (m_msg.mission_rewards.Contains(m_mission.index * 10 + id))
        {
            return 1;//已领取
        }
        else
        {
            if (m_msg.mission.mission_rewards.Contains(m_mission.index * 10 + id))
            {
                return 3;//已达成
            }
            else
            {
                return 2;//未达成
            }

        }

    }
    float get_hp(int id)
    {
        long cur = 0;
        long max = 0;
        for (int i = 0; i < 5; i++)
        {
            cur += m_msg.mission.guild_cur_hps[i + id * 5];
            max += m_msg.mission.guild_max_hps[i + id * 5];
        }
        return (float)cur / max;
    }
    int get_index(int id)
    {
        dhc.guild_box_t box_t = m_msg_view.boxes[id];
        for (int i = 0; i < box_t.reward_guids.Count; i++)
        {
            if (sys._instance.m_self.m_guid == box_t.reward_guids[i])
            {
                return i;
            }
        }
        return -1;
        
    }
    public void reset(int id,int type = 0)
    {
        sys._instance.remove_child(m_view);
        List<GameObject> objs = new List<GameObject>();
       
        this.id = id;
        int state1 = get_state(id);
        
        CancelInvoke();
        if (state1 == 1)
        {
            m_time.text = game_data._instance.get_t_language ("guildboss_task_gui.cs_88_26");//已领取试炼奖励

        }
        else
        {
            InvokeRepeating("refresh_id", 0, 1);
        }
        
		for(int i = 0;i < 4; i ++)
		{
            dhc.guild_box_t box_tt = m_msg_view.boxes[i];
			GameObject obj = game_data._instance.ins_object_res("ui/shouwei_sub");
			obj.transform.Find("name").GetComponent<UILabel>().text = get_name(i);
			GameObject mission = obj.transform.Find("mission").gameObject;
			GameObject hp  = mission.transform.Find("hp").gameObject;
			GameObject mission_icon = mission.transform.Find("icon").gameObject;
			GameObject box = obj.transform.Find("box").gameObject;
            GameObject box_icon = box.transform.Find("icon").gameObject;
            GameObject box_box =  box.transform.Find("box").gameObject;
            GameObject kelingqu = box.transform.Find("reward").gameObject;
            obj.name = i + "";
            UIEventListener.Get(obj).onClick = click;
            sys._instance.remove_child(box_icon);
            sys._instance.remove_child(mission_icon);
            obj.transform.parent = m_view.transform;
			obj.transform.localPosition = new Vector3(-240 + i * 160,9,0);
            obj.transform.localScale = Vector3.one;
            int state = get_state(i);
            if (state == 2)//weidacheng
			{
				mission.SetActive(true);
				box.SetActive(false);
                hp.GetComponent<UISlider>().value = get_hp(i);
                ccard _card = new ccard();
                _card.set_monster(m_mission.ids[i]);
                GameObject obj_icon = icon_manager._instance.create_card_icon_ex(_card.m_t_class.id,0,0,0);
                obj_icon.GetComponent<BoxCollider>().enabled = false;
				obj_icon.transform.parent = mission_icon.transform;
				obj_icon.transform.localScale = Vector3.one;
				obj_icon.transform.localPosition = Vector3.zero;
			}
			else if(state == 1)//yilingqu
			{
				mission.SetActive(false);
				box.SetActive(true);
                int index = get_index(i);
                obj.transform.Find("name").GetComponent<UILabel>().text = "";
                if (index != -1)
                {
                    GameObject icon1 = icon_manager._instance.create_reward_icon
                                      (m_mission.slrewarxds[i].type, m_mission.slrewarxds[i].value1,
                                      0, 0);
                    icon1.transform.parent = box_icon.transform;
                    icon1.transform.localPosition = Vector3.zero;
                    icon1.transform.localScale = Vector3.one;
                    icon1.GetComponent<BoxCollider>().enabled = false;
                   
                }
                kelingqu.GetComponent<UILabel>().text = "";
                box_box.GetComponent<UISprite>().spriteName = "icon_slbxk";
			}
            else if (state == 3)//kelingqu
            {
                mission.SetActive(false);
                box.SetActive(true);
                box_box.GetComponent<UISprite>().spriteName = "icon_slbxg";
                kelingqu.GetComponent<UILabel>().text = game_data._instance.get_t_language ("guildboss_task_gui.cs_154_56");//可领取
                obj.transform.Find("name").GetComponent<UILabel>().text = "";

 
            }
			objs.Add(obj);
		}
        dhc.guild_box_t box_t = m_msg_view.boxes[id];
        GameObject obj1 = null;
        if (type == 0)
        {
            if (m_box_view.GetComponent<SpringPanel>() != null)
            {
                m_box_view.GetComponent<SpringPanel>().enabled = false;
            }
            m_box_view.transform.localPosition = new Vector3(0, 0, 0);
            m_box_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
 
        }
       
        sys._instance.remove_child(m_box_view);
        for (int i = 0; i < box_t.reward_guids.Count; i++)
        {
            if(i % 5 == 0)
            {
                obj1 = game_data._instance.ins_object_res("ui/reward_sub");
                obj1.transform.parent = m_box_view.transform;
                obj1.transform.localPosition = new Vector3(0, 48 - 165 * (i / 5), 0);
                obj1.transform.localScale = Vector3.one;
            }

            GameObject m_icon = obj1.transform.Find("icon").gameObject;
            GameObject box = game_data._instance.ins_object_res("ui/juntuan/reward_sub_box");
            box.transform.parent = m_icon.transform;
            box.transform.localPosition = new Vector3( i % 5 * 125,0, 0);
            box.transform.localScale = Vector3.one;
            GameObject icon_p = box.transform.Find("icon").gameObject;
            sys._instance.remove_child(icon_p);
            UIEventListener.Get(box).onClick = click_lingqu;
            box.name = i + "";
            icon_p.GetComponent<UISpriteAnimation>().enabled = false;
            icon_p.GetComponent<UISprite>().spriteName = "54747";
            box.transform.Find("index").GetComponent<UILabel>().text = "";

            if (box_t.reward_guids[i] != 0)
            {
                box.GetComponent<UISprite>().spriteName = "icon_slbxk";
                box.transform.Find("name").GetComponent<UILabel>().text =  box_t.reward_names[i];
                GameObject icon1 = icon_manager._instance.create_reward_icon_ex
                    (m_mission.slrewarxds[id].type,m_mission.slrewarxds[id].value1, m_mission.slrewarxds[id].value2s[box_t.reward_ids[i]],0);
                icon1.transform.GetComponent<BoxCollider>().enabled = false;
                icon1.transform.parent = icon_p.transform;
                icon1.transform.localPosition = Vector3.zero;
                icon1.transform.localScale = Vector3.one;
                if (box_t.reward_ids[i] == 4 || box_t.reward_ids[i] == 3)
                {
                    icon_p.GetComponent<UISpriteAnimation>().enabled = true;
                }
                else
                {
                    icon_p.GetComponent<UISpriteAnimation>().enabled = false;
                    icon_p.GetComponent<UISprite>().spriteName = "54747";

                }
            }
            else
            {
                box.GetComponent<UISprite>().spriteName = "icon_slbxg";
                box.transform.Find("name").GetComponent<UILabel>().text = "";
                box.transform.Find("index").GetComponent<UILabel>().text = i + 1 + "";
               
 
            }
 

        }
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].transform.Find("select").GetComponent<UISprite>().spriteName = "icon_xzkg";
            if (id == i)
            {
                objs[i].transform.Find("select").GetComponent<UISprite>().spriteName = "icon_xzkk";
            }
        }

		  
    }
    void click_lingqu(GameObject obj)
    {
        int id_ = int.Parse(obj.name);
        if (get_state(id) == 3)
        {
            if (m_msg_view.boxes[id].reward_guids[id_] == 0)
            {
                protocol.game.cmsg_guild_mission_complete_reward re = new protocol.game.cmsg_guild_mission_complete_reward();
                re.ceng = m_mission.index;
                re.index = id;
                re.box = id_;
                box_index = id_;
                net_http._instance.send_msg<protocol.game.cmsg_guild_mission_complete_reward>(opclient_t.CMSG_GUILD_MISSION_COMPLETE_REWARD, re);
 
            }
           
        }
 
    }
	void refresh_id()
	{
        int state = get_state(id);
        if (state == 2)
        {
            long time = timer.get_time_cuo(24) - (long)timer.now();
            m_time.text = string.Format(game_data._instance.get_t_language ("guildboss_task_gui.cs_266_40"),timer.get_time_show_rob(time));//{0}内击杀，可领取
        }
        else if(state == 3)
        {
            long time = timer.get_time_cuo(24) - (long)timer.now();
            m_time.text = string.Format(game_data._instance.get_t_language ("guildboss_task_gui.cs_271_40"),timer.get_time_show_rob(time) );//{0}后消失，请尽快领取
 
        }
		
}
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

	string get_name(int m_id)
	{
		switch (m_id)
		{
		case 0:
			return game_data._instance.get_t_language ("guildboss_task_gui.cs_286_10");//坚苍守卫
		case 1:
			return game_data._instance.get_t_language ("guildboss_task_gui.cs_289_10");//韧战守卫
		case 2:
			return game_data._instance.get_t_language ("guildboss_task_gui.cs_292_10");//迅霆守卫
		case 3:
			return game_data._instance.get_t_language ("guildboss_task_gui.cs_295_10");//布洛守卫
		}
		return "";
	}
    void IMessage.message(s_message message)
    {

    }
    public void click(GameObject obj)
    {
        if (obj.transform.name == "close")
        {
            if (sl_info_gui.activeSelf)
            {
                sl_info_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                m_fisrt_shouwei.value = true;
                return;
            }
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (obj.name == "info")
        {
            if (!sl_info_gui.activeSelf)
            {
                sl_info_gui.SetActive(true);
                reset_yulan(0);
 
            }
            
 
        }
        else
        {
            id = int.Parse(obj.name);
            reset(id);
        }
 
    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_GUILD_MISSION_COMPLETE_REWARD_VIEW)
        {
            m_msg_view = net_http._instance.parse_packet<protocol.game.smsg_guild_mission_complete_reward_view>(message.m_byte);
            reset(id);
            sl_info_gui.SetActive(false);
        }
        else if(message.m_opcode == opclient_t.CMSG_GUILD_MISSION_COMPLETE_REWARD)
        {
            protocol.game.smsg_guild_mission_complete_reward _r = 
                net_http._instance.parse_packet<protocol.game.smsg_guild_mission_complete_reward>(message.m_byte);
            m_msg_view.boxes[id].reward_guids[box_index] = sys._instance.m_self.m_guid;
            m_msg_view.boxes[id].reward_ids[box_index] = _r.reward_index;
            m_msg_view.boxes[id].reward_names[box_index] = sys._instance.m_self.m_t_player.name;
            sys._instance.m_self.add_reward
                (m_mission.slrewarxds[id].type, m_mission.slrewarxds[id].value1, m_mission.slrewarxds[id].value2s[_r.reward_index],0,true,game_data._instance.get_t_language ("guildboss_task_gui.cs_350_138"));//军团boss关卡获得
            m_msg.mission_rewards.Add(m_mission.index * 10 + id + 1);
            reset(id,1);
            s_message mes = new s_message();
            mes.m_type = "refresh_guild_boss";
            cmessage_center._instance.add_message(mes);
        }
        
    }
    void OnDisable()
    {
        CancelInvoke();

    }
    void click_yulan(GameObject obj)
    {
        int id = int.Parse(obj.name);
        yulan_id = id;
        reset_yulan(id);
    }
    int get_num_lingqu(dhc.guild_box_t box,int dang)
    {
        int num = 0;
        for (int i = 0; i < box.reward_guids.Count; i++)
        {
            if (box.reward_guids[i] != 0 && box.reward_ids[i] == dang)
            {
                num++;
 
            }
 
        }
        return num;
    }
    void reset_yulan(int id)
    {
        sys._instance.remove_child(yulan_view);
        dhc.guild_box_t box = m_msg_view.boxes[id];
        s_t_guild guild = game_data._instance.get_guild(juntuan_gui._instance.m_guild_t.level);
        for (int i = 0; i < 5; i++)
        {
            GameObject t_obj = game_data._instance.ins_object_res("ui/juntuan/yulan_sub");
            t_obj.transform.parent = yulan_view.transform;
            t_obj.transform.localPosition = new Vector3(-190 + i % 3 * 190, 70 - i / 3 * 122, 0);
            t_obj.transform.localScale = Vector3.one;
            GameObject icon = t_obj.transform.Find("icon").gameObject;
            sys._instance.remove_child(icon);
            if (i == 0 || i == 1)
            {
                icon.GetComponent<UISpriteAnimation>().enabled = true;
            }
            else
            {
                icon.GetComponent<UISpriteAnimation>().enabled = false;
                icon.GetComponent<UISprite>().spriteName = "54747";
 
            }
            GameObject child = icon_manager._instance.create_reward_icon
                (m_mission.slrewarxds[id].type, m_mission.slrewarxds[id].value1, m_mission.slrewarxds[id].value2s[4 - i], 0);
            child.transform.parent = icon.transform;
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;

            t_obj.transform.Find("num").GetComponent<UILabel>().text = get_num_lingqu(box,4 - i) + "/" + guild.reward_nums[4 - i];

        }
        CancelInvoke("refresh_time");
        InvokeRepeating("refresh_time", 0, 1);
 
    }
    void refresh_time()
    {
        long time = timer.get_time_cuo(24) - (long)timer.now();
        m_yulan_time.text = string.Format(game_data._instance.get_t_language ("guildboss_task_gui.cs_423_42"),timer.get_time_show(time) );//{0}内，每击败一个守卫，都可以领取一次奖励，大家可以从下列奖励中随机抽取一种
    }
}
