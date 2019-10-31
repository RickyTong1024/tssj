
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class czjh_gui : MonoBehaviour ,IMessage{

	public GameObject m_view;
	public GameObject m_buy;
	public GameObject m_recharge;
	public GameObject m_ybuy;
    public UILabel m_desc;
    public int m_lingquid;
    public GameObject m_lingqu;
    public UILabel m_renshu;
    public GameObject m_effect1;
    public GameObject m_effect2;
    public UIToggle m_button;

 
	// Use this for initialization
	void Awake () {

		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
    void OnDisable()
    {
        sys._instance.remove_child(m_view);
    }
	public void reset()
	{
        int re_jewel = 0;
        for (int i = 0; i < game_data._instance.m_dbc_huodong_czjh.get_y(); ++i)
        {
            int jewel = int.Parse(game_data._instance.m_dbc_huodong_czjh.get(2, i));
            re_jewel += jewel;
        }
        m_desc.text = string.Format(game_data._instance.get_t_language ("czjh_gui.cs_39_22"), re_jewel);//只要购买成长计划，即刻获得总计[ffa000]{0}[-]钻石返利。达到[ffa000]VIP2[-]即可购买
		m_buy.transform.Find("Label").GetComponent<UILabel>().text =string.Format("x {0}", game_data._instance.get_const_vale((int)opclient_t.CONST_PLAN));
        m_view.GetComponent<UIScrollView>().movement = UIScrollView.Movement.Horizontal;
        if (sys._instance.m_self.m_huodong_czjh_count >= 1000)
        {
            m_renshu.text = sys._instance.m_self.m_huodong_czjh_count + "";
        }
        else if (sys._instance.m_self.m_huodong_czjh_count >= 100)
        {
            m_renshu.text = "0" + sys._instance.m_self.m_huodong_czjh_count + "";
        }
        else if (sys._instance.m_self.m_huodong_czjh_count >= 10)
        {
            m_renshu.text = "00" + sys._instance.m_self.m_huodong_czjh_count + "";
        }
        else if (sys._instance.m_self.m_huodong_czjh_count >= 0)
        {
            m_renshu.text = "000" + sys._instance.m_self.m_huodong_czjh_count + "";
        }
		if(czjh_sub.is_has())
		{
			m_buy.SetActive(false);
			m_recharge.SetActive(false);
			m_ybuy.SetActive(true);
		}
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		for(int i = 0;i < game_data._instance.m_dbc_huodong_czjh.get_y();++i )
		{
			int id = int.Parse(game_data._instance.m_dbc_huodong_czjh.get (0,i));
			
			GameObject _obj = game_data._instance.ins_object_res("ui/czjh_sub");
			
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
            _obj.transform.localPosition = new Vector3(-212 + i * 203, 0, 1);
			_obj.GetComponent< czjh_sub>().m_id = id;
			_obj.GetComponent<czjh_sub>().reset();
			
			sys._instance.add_pos_anim(_obj,0.3f, new Vector3(0, 300, 0), i * 0.05f);
			sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, i * 0.05f);
			
		}
       // m_lingqu.gameObject.SetActive(true);
        //m_renshu.gameObject.SetActive(false);
        if (is_effect_3())
        {
            m_effect1.SetActive(true);

        }
        else
        {
            m_effect1.SetActive(false);
        }
        if (is_effect2() && sys._instance.m_self.m_t_player.huodong_czjh_buy ==1)
        {
            m_effect2.SetActive(true);
        }
        else
        {
            m_effect2.SetActive(false);
        }
	}
    public static bool is_effect2()
    {
        foreach (int id in game_data._instance.m_huo_dong_czjhrs.Keys)
        {
            if (get_state(id) == 3)
            {
                return true;
            }
        }
        return false;
    }
    public void reset_qmfl()
    {
        m_view.GetComponent<UIScrollView>().movement = UIScrollView.Movement.Vertical;
        m_desc.text = game_data._instance.get_t_language ("czjh_gui.cs_102_22");//与小伙伴一同成长，全服玩家共享豪华大礼！{nn}购买人数越多，奖励就越多哦
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
       // m_lingqu.gameObject.SetActive(false);
       // m_renshu.gameObject.SetActive(true);
       // m_ybuy.SetActive(false);
        if (sys._instance.m_self.m_huodong_czjh_count >= 1000)
        {
            m_renshu.text = sys._instance.m_self.m_huodong_czjh_count + "";
        }
        else if (sys._instance.m_self.m_huodong_czjh_count >= 100)
        {
            m_renshu.text = "0" + sys._instance.m_self.m_huodong_czjh_count + "";
        }
        else if (sys._instance.m_self.m_huodong_czjh_count >= 10)
        {
            m_renshu.text = "00" + sys._instance.m_self.m_huodong_czjh_count + "";
        }
        else if (sys._instance.m_self.m_huodong_czjh_count >= 0)
        {
            m_renshu.text = "000" + sys._instance.m_self.m_huodong_czjh_count + "";
        }
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);
        List<int> ids = new List<int>();
        foreach(int id in game_data._instance.m_huo_dong_czjhrs.Keys)
        {
            ids.Add(id);
           
        }
        ids.Sort(com);
        for (int i = 0; i < ids.Count; i++)
        {
            s_t_huodong_czjhrs cz = game_data._instance.get_t_huodong_czjhrs(ids[i]);

            GameObject _obj = game_data._instance.ins_object_res("ui/czjhrs_sub");

            _obj.transform.parent = m_view.transform;
            _obj.transform.localScale = new Vector3(1, 1, 1);
            _obj.transform.localPosition = new Vector3(4, 77 - i * 161, 1);
            _obj.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("czjh_gui.cs_145_90"), cz.buy_count);//购买人数达到{0}可领取
            _obj.transform.Find("reward").GetComponent<UILabel>().text = sys._instance.get_res_info(cz.type, cz.value1, cz.value2, cz.value3);
            UIEventListener.Get(_obj.transform.Find("get").gameObject).onClick = click;
            _obj.name = ids[i] + "";
            int state = get_state(ids[i]);
            if (state == 1)
            {
                _obj.transform.Find("ylq").gameObject.SetActive(true);
                _obj.transform.Find("get").gameObject.SetActive(false);
            }
            else if (state == 2)
            {
               
                _obj.transform.Find("ylq").gameObject.SetActive(false);
                _obj.transform.Find("get").gameObject.SetActive(true);
                _obj.transform.Find("get").GetComponent<UISprite>().set_enable(false);


            }
            else
            {
               
               
                if (sys._instance.m_self.m_t_player.huodong_czjh_index1 + 1 ==  ids[i])
                {
                     _obj.transform.Find("ylq").gameObject.SetActive(false);
                     _obj.transform.Find("get").gameObject.SetActive(true);
                     _obj.transform.Find("get").GetComponent<UISprite>().set_enable(true);
 
                }
                else
                {
                    _obj.transform.Find("ylq").gameObject.SetActive(false);
                    _obj.transform.Find("get").gameObject.SetActive(true);
                    _obj.transform.Find("get").GetComponent<UISprite>().set_enable(false);


                }

          
            }
            GameObject icon = icon_manager._instance.create_reward_icon(cz.type, cz.value1, cz.value2, cz.value3);
            sys._instance.remove_child(_obj.transform.Find("icon").gameObject);
            icon.transform.parent = _obj.transform.Find("icon");
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            sys._instance.add_pos_anim(_obj, 0.3f, new Vector3(-300, 0, 0), i * 0.05f);
            sys._instance.add_alpha_anim(_obj, 0.3f, 0, 1.0f, i * 0.05f);
 
        }
        if (is_effect_3())
        {
            m_effect1.SetActive(true);

        }
        else
        {
            m_effect1.SetActive(false);
        }
        if (is_effect2() && sys._instance.m_self.m_t_player.huodong_czjh_buy == 1)
        {
            m_effect2.SetActive(true);
        }
        else
        {
            m_effect2.SetActive(false);
        }
 
    }
    int com(int x, int y)
    {
        int x_state = get_state(x);
        int y_state = get_state(y);
        if (x_state != y_state)
        {
            return -(x_state - y_state);
        }
        else
        {
            return x - y;
        }
    }
    static int get_state(int id)
    {
        s_t_huodong_czjhrs cz = game_data._instance.get_t_huodong_czjhrs(id);
        if (sys._instance.m_self.m_huodong_czjh_count < cz.buy_count)
        {
            return 2;
        }
        else
        {
            if (sys._instance.m_self.m_t_player.huodong_czjh_index1 >= cz.index)
            {
                return 1;//已领取
            }
            else
            {
                return 3;
            }
        }
 
    }
	public void init()
	{
		sys._instance.remove_child (m_view);
		for(int i = 0;i < game_data._instance.m_dbc_huodong_czjh.get_y();++i )
		{
			int id = int.Parse(game_data._instance.m_dbc_huodong_czjh.get (0,i));
			
			GameObject _obj = game_data._instance.ins_object_res("ui/czjh_sub");
			
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
            _obj.transform.localPosition = new Vector3(-212 + i * 203, 0, 1);
			_obj.GetComponent< czjh_sub>().m_id = id;
			_obj.GetComponent<czjh_sub>().reset();
			
		}
	}
    static bool is_effect_3()
    {
        int num = 0;
        for (int i = 0; i < game_data._instance.m_dbc_huodong_czjh.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_huodong_czjh.get(0, i));
            s_t_huodong_czjh t_huodong_czjh = game_data._instance.get_t_huodong_czjh(id);
            if (sys._instance.m_self.m_t_player.level >= t_huodong_czjh.level)
            {
                num = id;
            }
        }
        if (sys._instance.m_self.get_vip() >= 2 && sys._instance.m_self.m_t_player.huodong_czjh_buy == 1)
        {
            if (num > sys._instance.m_self.m_t_player.huodong_czjh_index)
            {
                return true;
            }
        }
        return false;
 
    }
	public static bool is_effect1()
	{
		int num = 0;
		if(sys._instance.m_self.m_t_player.huodong_czjh_buy == 0)
		{
			return true;
		}
		for(int i = 0; i < game_data._instance.m_dbc_huodong_czjh.get_y(); ++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_huodong_czjh.get (0,i));
			s_t_huodong_czjh t_huodong_czjh = game_data._instance.get_t_huodong_czjh(id);
			if(sys._instance.m_self.m_t_player.level >= t_huodong_czjh.level)
			{
				num = id;
			}
		}
		if(sys._instance.m_self.get_vip() >= 2 && sys._instance.m_self.m_t_player.huodong_czjh_buy ==1)
		{
			if(num > sys._instance.m_self.m_t_player.huodong_czjh_index)
			{
				return true;
			}
		}
		return false;
	}
    public static bool is_effect()
    {
        if (is_effect1() || is_effect2())
        {
            return true;
        }
        return false;
 
    }

	public void click(GameObject obj)
	{
		if(obj.transform.name == "buy")
		{
			if(!is_has())
			{
				if(sys._instance.m_self.get_vip() < 2)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" +game_data._instance.get_t_language ("czjh_gui.cs_329_59") );//VIP等级不足
					return;
				}
                if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_PLAN))
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
					return;
				}
			}
			else
			{
                string _des = string.Format(game_data._instance.get_t_language("czjh_gui.cs_340_18"), game_data._instance.get_const_vale((int)opclient_t.CONST_PLAN)); //是否花费[00ffff]{0}钻石[-]购买成长计划
				s_message _msg = new s_message();
				_msg.m_type = "czjh_show";
				root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_msg);//提示
			}
		}
        else if(obj.transform.name == "recharge")
		{
			s_message _mes = new s_message();
			_mes.m_type = "show_recharge";
			cmessage_center._instance.add_message(_mes);
			s_message _mes1 = new s_message();
			_mes1.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_mes1);
		}
		else if (obj.transform.name == "close") 
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
            m_button.value = true;
			s_message _message = new s_message ();
			_message.m_type = "show_jc_huodong";
			cmessage_center._instance.add_message (_message);
			
		}
        else  if (obj.name == "czjh")
        {
            reset();
           
        }
        else if (obj.name == "qmfl")
        {
            reset_qmfl();
            
        }
        else
        {
            m_lingquid = int.Parse(obj.transform.parent.name);
            protocol.game.cmsg_common common = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG_CZJH_RS, common);
        }
	}
	
	bool is_has()
	{
		if(sys._instance.m_self.get_vip() >= 2 && sys._instance.m_self.get_att(e_player_attr.player_jewel) >= game_data._instance.get_const_vale((int)opclient_t.CONST_PLAN))
		{
			return true;
		}
		return false;
	}
	
	void IMessage.message (s_message message)
	{
		if (message.m_type == "huodong_czjh_lj") 
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();			
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_CZJH_GET, _msg);
		}
		if(message.m_type == "czjh_show")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_CZJH_BUY, _msg);
		}
	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_CZJH_GET)
		{
			sys._instance.m_self.m_t_player.huodong_czjh_index += 1;
			s_t_huodong_czjh t_huodong_czjh = game_data._instance.get_t_huodong_czjh (sys._instance.m_self.m_t_player.huodong_czjh_index);
			sys._instance.m_self.add_att(e_player_attr.player_jewel, t_huodong_czjh.jewel,game_data._instance.get_t_language ("czjh_gui.cs_411_81"));//充值计划获得
			init();
		}
		if (message.m_opcode == opclient_t.CMSG_HUODONG_CZJH_BUY)
		{
			sys._instance.m_self.m_t_player.huodong_czjh_buy = 1;
            sys._instance.m_self.m_huodong_czjh_count++;
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, game_data._instance.get_const_vale((int)opclient_t.CONST_PLAN), game_data._instance.get_t_language ("czjh_gui.cs_418_65"));//购买充值计划消耗
			reset();
		}
        if (message.m_opcode == opclient_t.CMSG_HUODONG_CZJH_RS)
        {
            protocol.game.smsg_huodong_reward _treasures = net_http._instance.parse_packet<protocol.game.smsg_huodong_reward>(message.m_byte);
            for (int i = 0; i < _treasures.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_treasures.treasures[i]);
            }
            for (int i = 0; i < _treasures.equips.Count; i++)
            {
                sys._instance.m_self.add_equip(_treasures.equips[i]);
            }
            for (int i = 0; i < _treasures.pets.Count; i++)
            {
                sys._instance.m_self.add_pet(_treasures.pets[i]);
            }
            for (int i = 0; i < _treasures.types.Count; i++)
            {
                sys._instance.m_self.add_reward(_treasures.types[i],_treasures.value1s[i],_treasures.value2s[i],_treasures.value3s[i],game_data._instance.get_t_language ("czjh_gui.cs_411_81"));//充值计划获得
            }
            sys._instance.m_self.m_t_player.huodong_czjh_index1++;
            reset_qmfl();
          
 
        }
	}

	// Update is called once per frame
	void Update () {
	
	}
}
