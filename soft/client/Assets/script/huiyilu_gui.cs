
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class huiyilu_gui : MonoBehaviour,IMessage {
    public GameObject m_view;
    public UILabel m_shoujidu;
    public UILabel m_jihuohuiyi;

    public GameObject m_huiyi_sub_panel;
    public GameObject m_huiyi_sub_view;
    public UILabel m_huiyi_sub_name;
    private int m_sub_id;
    private int m_id;
    public GameObject m_next;
    public GameObject m_continue;
    public bool m_sx_flag = false;

    //chegjiu
    public GameObject m_chengjiu_view;
    public GameObject m_chengjiu_panel;
    public UILabel m_desc;
	public UILabel m_shoujidu_t;
    //========
    //属性总览
    public GameObject m_sx_panel;
    public UILabel m_sx_label1;
    public UILabel m_sx_label2;
    public UILabel m_sx_label3;
    //==============
    //榜单
    public GameObject m_rank_view;
    public GameObject m_rank_panel;
    protocol.game.smsg_role_huiyi_rank m_msg;
    public GameObject m_rank_item;
    //=========================
    int m_target_num = 0;
    public UILabel m_dacheng_des;
    public UILabel m_dialog;

	//属性预览 面板
	private GameObject m_huiyi_star;

	protocol.game.cmsg_role_huiyi_jihuo m_messa;
	void Start () 
    {
       
        int temp = get_shoujidu();
        for (int i = 0; i < game_data._instance.m_dbc_huiyi_chengjiu.get_y(); i++)
        {
            s_t_huiyi_chengjiu _chengjiu = game_data._instance.get_t_huiyi_chengjiu(int.Parse(game_data._instance.m_dbc_huiyi_chengjiu.get(0, i)));
            if (temp >= _chengjiu.mem_value)
            {
                m_target_num++;
            }
        }
        m_dacheng_des.gameObject.SetActive(false);
	}
    void OnEnable()
    {
        cmessage_center._instance.add_handle(this);
        reset();
    }
	// Update is called once per frame
    void reset_sx_panel()
    {
        List<float> _attrs = new List<float>();
        for(int i = 0;i < game_data._instance.m_t_value.get_y() + 1; i ++)
        {
            _attrs.Add(0);
        }

        for (int i = 0; i < sys._instance.m_self.m_t_player.huiyi_jihuos.Count; i++)
        {
            s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(sys._instance.m_self.m_t_player.huiyi_jihuos[i]);

            for (int j = 0; j < _sub.attrs.Count; j++)
            {
                if (_sub.attrs[j] >= _attrs.Count)
                {
                    continue;
                }
				_attrs[_sub.attrs[j]] += _sub.values[j]+_sub.values2[j]*get_star_num(sys._instance.m_self.m_t_player.huiyi_jihuos[i]);
				
			}
        }
        bool flag = false;
        string s = "";
        string s1 = "";
        int num = 0;
        for (int i = 0; i < _attrs.Count; i++)
        {
            if (_attrs[i] == 0)
            {
                continue;
            }
            num++;
            flag = true;
            string text = game_data._instance.get_value_string(i, _attrs[i]);
            if (num % 2 != 0)
            {

				if(text.Contains("-"))
				{
					s += game_data._instance.get_t_language ("huiyilu_gui.cs_97_21") + "" + text.Split('-')[0] + "：[-][0aff16]-" + text.Split('-')[1] + "[-]" + "\n";//[0aabff]全队
				}
				else
				{
					s += game_data._instance.get_t_language ("huiyilu_gui.cs_97_21") + "" + text.Split('+')[0] + "：[-][0aff16]+" + text.Split('+')[1] + "[-]" + "\n";//[0aabff]全队
				}
               
            }
            else
            {
				if(text.Contains("-"))
				{
					s1 += game_data._instance.get_t_language ("huiyilu_gui.cs_97_21") + "" + text.Split('-')[0] + "：[-][0aff16]-" + text.Split('-')[1] + "[-]" + "\n";//[0aabff]全队
				}
				else
				{
					s1 += game_data._instance.get_t_language ("huiyilu_gui.cs_97_21") + "" + text.Split('+')[0] + "：[-][0aff16]+" + text.Split('+')[1] + "[-]" + "\n";//[0aabff]全队
				}
                

 
            }
            
        }
        if (!flag)
        {
            s = game_data._instance.get_t_language ("huiyilu_gui.cs_109_16");//未达成任何属性
            m_sx_label1.gameObject.SetActive(false);
            m_sx_label2.gameObject.SetActive(false);
            m_sx_label3.gameObject.SetActive(true);
            m_sx_label3.text = s;
        }
        else
        {
            m_sx_label1.text = s;
            m_sx_label2.text = s1;
            m_sx_label1.gameObject.SetActive(true);
            m_sx_label2.gameObject.SetActive(true);
            m_sx_label3.gameObject.SetActive(false);

        }
    }
    void reset_chengjiu_panel()
    {
        if (m_chengjiu_view.GetComponent<SpringPanel>() != null)
        {
            m_chengjiu_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_chengjiu_view.transform.localPosition = new Vector3(0, 0, 0);
        m_chengjiu_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_chengjiu_view);
        List<int> _ids = new List<int>();

        for (int i = 0; i < game_data._instance.m_dbc_huiyi_chengjiu.get_y(); i++)
        {
            int _id = int.Parse(game_data._instance.m_dbc_huiyi_chengjiu.get(0, i));
            _ids.Add(_id);
        }
        _ids.Sort(compare_chengjiu);
        for (int i = 0; i < _ids.Count; i++)
        {
            s_t_huiyi_chengjiu _chengjiu = game_data._instance.get_t_huiyi_chengjiu(_ids[i]);
            GameObject obj = game_data._instance.ins_object_res("ui/huiyi_chengjiu_sub");
            obj.transform.parent = m_chengjiu_view.transform;
            obj.transform.localPosition = new Vector3(0, 130 - i * 110, 0);
            obj.transform.localScale = Vector3.one;
            obj.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huiyilu_gui.cs_149_89") , _chengjiu.mem_value);//收集度达到{0}
            obj.transform.Find("progress").GetComponent<UILabel>().text = game_data._instance.get_t_language ("guild_red_target.cs_45_70") + get_shoujidu() + "/" +  _chengjiu.mem_value;//进度：
            obj.transform.Find("sx").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huiyilu_gui.cs_151_73") + game_data._instance.get_value_string(_chengjiu.attr,_chengjiu.value);//全队
            if (is_finish_chengjiu(_ids[i]))
            {
                obj.transform.Find("ylq").gameObject.SetActive(true);
                obj.transform.Find("main").gameObject.SetActive(false);
                obj.transform.Find("main1").gameObject.SetActive(true);

            }
            else
            {
                obj.transform.Find("ylq").gameObject.SetActive(false);
                
                obj.transform.Find("main").gameObject.SetActive(true);
                obj.transform.Find("main1").gameObject.SetActive(false); 
 
            }
        }
		m_shoujidu_t.text = game_data._instance.get_t_language ("huiyilu_gui.cs_175_22") + sys._instance.m_self.m_t_player.huiyi_shoujidu_top.ToString();//历史最高收集度：
        m_desc.text = get_attr();
    }
    string get_attr()
    {
        List<int> _ids = new List<int>();
        for (int i = 0; i < game_data._instance.m_dbc_huiyi_chengjiu.get_y(); i++)
        {
            int _id = int.Parse(game_data._instance.m_dbc_huiyi_chengjiu.get(0, i));
            if (is_finish_chengjiu(_id))
            {
                _ids.Add(_id);
            }
        }
        int value1 = 0;
        int value2 = 0;
        for (int i = 0; i < _ids.Count; i++)
        {
            s_t_huiyi_chengjiu _chengjiu = game_data._instance.get_t_huiyi_chengjiu(_ids[i]);
            if (_chengjiu.attr == 20)
            {
                value1 += _chengjiu.value;
            }
            else if (_chengjiu.attr == 21)
            {
                value2 += _chengjiu.value;
            }
        }
        string s = game_data._instance.get_t_language ("huiyilu_gui.cs_195_19");//属性加成：
        s += game_data._instance.get_t_language ("huiyilu_gui.cs_151_73") + game_data._instance.get_value_string(20,value1) + "  ";//全队
        s += game_data._instance.get_t_language ("huiyilu_gui.cs_151_73") + game_data._instance.get_value_string(21, value2);//全队
        return s;
    }
    int compare_chengjiu(int x, int y)
    {
      bool _x = is_finish_chengjiu(x);
        bool _y = is_finish_chengjiu(y);
        if (_x && !_y)
        {
            return 1;
        }
        else if (!_x && _y)
        {
            return -1;
        }
        else
        {
            return x - y;
        }
 
    }

    public static bool is_finish_chengjiu(int id)
    {
        s_t_huiyi_chengjiu _x = game_data._instance.get_t_huiyi_chengjiu(id);
        if (get_shoujidu_top() >= _x.mem_value)
        {
            return true;
        }
        return false;
    }
    void IMessage.message(s_message message)     
    {

        if (message.m_type == "huiyi_jiesuo")
        {
            m_sub_id = (int)message.m_ints[0];
            s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(m_sub_id);
            bool flag = true;
            for (int i = 0; i < _sub.huiyis.Count; i++)
            {
                if (sys._instance.m_self.get_item_num((uint)_sub.huiyis[i]) < 1)
                {
                    flag = false;
                    break;
                }
 
            }
            if (flag)
            {
                protocol.game.cmsg_role_huiyi_jihuo msg = new protocol.game.cmsg_role_huiyi_jihuo();
                msg.id = m_sub_id;
                net_http._instance.send_msg<protocol.game.cmsg_role_huiyi_jihuo>(opclient_t.CMSG_ROLE_HUIYI_JIHUO, msg);
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_hecheng.cs_539_59"));//材料不足
            }
        }

		if (message.m_type == "huiyi_shengxing") 
		{
			m_sub_id =(int)message.m_ints[0];
			s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(m_sub_id);
			bool flag = true;
			for (int i = 0; i < _sub.huiyis.Count; i++)
			{
				if (sys._instance.m_self.get_item_num((uint)_sub.huiyis[i]) < 1)
				{
					flag = false;
					break;
				}
				
			}
			if (flag)
			{
				//回忆升星消息体  和激活一样
				protocol.game.cmsg_role_huiyi_jihuo msg = new protocol.game.cmsg_role_huiyi_jihuo();
				msg.id = m_sub_id;
				net_http._instance.send_msg<protocol.game.cmsg_role_huiyi_jihuo>(opclient_t.CMSG_ROLE_HUIYI_STAR, msg);
			}
			else
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_hecheng.cs_539_59"));//材料不足
			}

		}

		if (message.m_type == "huiyi_yulan") 
		{
			 m_huiyi_star =game_data._instance.ins_object_res("ui/huiyi_shengxing_yuanlan_ex");
			if(m_huiyi_star != null)
			{
				m_sub_id =(int)message.m_ints[0];
				m_huiyi_star.GetComponent<huiyi_shengxing_yulan>().m_id = m_sub_id;
				m_huiyi_star.AddComponent<gui_remove>();
				//Debug.Log(m_huiyi_star.name);
				m_huiyi_star.GetComponent<huiyi_shengxing_yulan>().reset();
			}

			m_huiyi_star.transform.parent =this.transform;
			m_huiyi_star.transform.localPosition =Vector3.zero;
			m_huiyi_star.transform.localScale =Vector3.one;
			m_huiyi_star.SetActive(true);
		}

		if (message.m_type == "huiyi_chongzhi") 
		{	if(sys._instance.m_self.m_t_player.jewel < 50)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
				return;
				
			}
			protocol.game.cmsg_role_huiyi_jihuo msg = new protocol.game.cmsg_role_huiyi_jihuo();
			msg.id = m_sub_id;
			net_http._instance.send_msg<protocol.game.cmsg_role_huiyi_jihuo>(opclient_t.CMSG_ROLE_HUIYI_RESET, msg);
		}
        
    }
     void IMessage.net_message(s_net_message message)
     {
         if (message.m_opcode == opclient_t.CMSG_ROLE_HUIYI_JIHUO) {
			s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub (m_sub_id);
			for (int i = 0; i < _sub.huiyis.Count; i++) {
				sys._instance.m_self.remove_item ((uint)_sub.huiyis [i], 1, game_data._instance.get_t_language ("huiyilu_gui.cs_263_73"));//回忆激活消耗
			}

			sys._instance.m_self.m_t_player.huiyi_jihuos.Add (m_sub_id);
			sys._instance.m_self.m_t_player.huiyi_shoujidu += _sub.huiyis.Count;
			sys._instance.m_self.m_t_player.huiyi_jihuo_starts.Add (0);

			if(get_shoujidu() > get_shoujidu_top())
			{
				sys._instance.m_self.m_t_player.huiyi_shoujidu_top = get_shoujidu();	
			}

			reset ();
			reset_sub_panel (m_id);
			m_dialog.GetComponent<UILabel>().text = "";
			TypewriterEffect eeff = null;
			eeff = m_dialog.gameObject.GetComponent<TypewriterEffect>();
			if (eeff == null) {
				eeff = m_dialog.gameObject.AddComponent<TypewriterEffect>();

			}
             
			EventDelegate.Add (eeff.onFinished, delegate() {
				m_continue.SetActive (false);
				m_next.SetActive (true);
				m_sx_flag = true;
				Destroy (m_dialog.gameObject.GetComponent<TypewriterEffect>());
			});
			eeff.charsPerSecond = 20;
			eeff.enabled = true;
			eeff.mFullText = _sub.dialog;
			m_sx_flag = false;
			m_dialog.transform.parent.Find("close").GetComponent<UITexture>().mainTexture = get_image_half (game_data._instance.get_t_huiyi (_sub.page).bg);
			m_dialog.transform.parent.gameObject.SetActive (true);
			m_next.SetActive (false);
			m_continue.SetActive (true);
			check_huiyi_target_done ();
            
		} else if (message.m_opcode == opclient_t.CMSG_RANK_HUIYI_VIEW) {
			m_msg = net_http._instance.parse_packet<protocol.game.smsg_role_huiyi_rank> (message.m_byte);
			m_rank_panel.SetActive (true);
			reset_rank_gui ();
		} else if (message.m_opcode == opclient_t.CMSG_ROLE_HUIYI_STAR) { //回忆升星

			s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub (m_sub_id);

			for (int i = 0; i < _sub.huiyis.Count; i++) {
				sys._instance.m_self.remove_item ((uint)_sub.huiyis [i], 1, game_data._instance.get_t_language ("huiyilu_gui.cs_382_64"));//回忆升星消耗
			}
			for (int i = 0; i < sys._instance.m_self.m_t_player.huiyi_jihuos.Count; i++) {

				if (sys._instance.m_self.m_t_player.huiyi_jihuos [i] == m_sub_id) {
					sys._instance.m_self.m_t_player.huiyi_jihuo_starts [i] += 1;
				}
			}

			sys._instance.m_self.m_t_player.huiyi_shoujidu += _sub.huiyis.Count;

			if(get_shoujidu() > get_shoujidu_top())
			{
				sys._instance.m_self.m_t_player.huiyi_shoujidu_top = get_shoujidu();	
			}
			reset ();
			reset_sub_panel (m_id);
			check_huiyi_target_done2 ();//检查

		} else if (message.m_opcode == opclient_t.CMSG_ROLE_HUIYI_RESET) 
		{
			s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub (m_sub_id);
			for (int i = 0; i < _sub.huiyis.Count; i++) {
				sys._instance.m_self.add_reward(2 , _sub.huiyis[i] , get_star_num(m_sub_id) , 0 , game_data._instance.get_t_language ("huiyilu_gui.cs_410_86"));//重置回忆星数获得
			}

			//在将回忆星数重置为0  前先将收集度降下来
			sys._instance.m_self.m_t_player.huiyi_shoujidu -=(_sub.huiyis.Count * get_star_num(m_sub_id));	

			for (int i = 0; i < sys._instance.m_self.m_t_player.huiyi_jihuos.Count; i++) {
				
				if (sys._instance.m_self.m_t_player.huiyi_jihuos [i] == m_sub_id) {
					sys._instance.m_self.m_t_player.huiyi_jihuo_starts [i] = 0;
				}
			}
				
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, 50,game_data._instance.get_t_language ("huiyilu_gui.cs_423_63"));	//重置回忆星数消耗
			reset ();
			reset_sub_panel (m_id);
//			set_attr2 ();//设置属性
			check_huiyi_target_done ();//检查

			m_huiyi_star.transform.Find("frame_big").GetComponent<frame>().hide();
		}
 
     }
	//获取当前升星数量
	public int get_star_num(int id)
	{
		int m_stars_curnum = 0;
		for (int i = 0; i < sys._instance.m_self.m_t_player.huiyi_jihuos.Count; i++) {
			if(sys._instance.m_self.m_t_player.huiyi_jihuos[i] == id)
			{
				m_stars_curnum = sys._instance.m_self.m_t_player.huiyi_jihuo_starts[i];
			}
		}
		return m_stars_curnum;
	}

  
     void reset_rank_gui()
     {
         sys._instance.remove_child(m_rank_view);
         m_rank_view.transform.localPosition = new Vector3(0, 0, 0);
         m_rank_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
         if (m_rank_view.GetComponent<SpringPanel>() != null)
         {
             m_rank_view.GetComponent<SpringPanel>().enabled = false;
         }
         for (int i = 0; i < m_msg.rank_list.player_guid.Count; i++)
         {
             GameObject obj = game_data._instance.ins_object_res("ui/huiyi_rank_sub");
             obj.transform.parent = m_rank_view.transform;
             obj.transform.localPosition = new Vector3(0, 132 - i * 82, 0);
             obj.transform.localScale = Vector3.one;
             obj.transform.Find("rank").GetComponent<UILabel>().text = (i + 1) + "";
             obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(m_msg.rank_list.player_achieve[i]) + m_msg.rank_list.player_name[i].ToString();
             obj.transform.Find("num").GetComponent<UILabel>().text = m_msg.rank_list.value[i] + "";
             obj.transform.Find("hit").GetComponent<UILabel>().text = m_msg.rank_list.player_huiyi[i] + "";

             GameObject _obj1 = icon_manager._instance.create_player_icon((int)m_msg.rank_list.player_template[i], m_msg.rank_list.player_achieve[i], m_msg.rank_list.player_vip[i],m_msg.rank_list.player_nalflag[i]);
            
             _obj1.transform.parent = obj.transform.Find("icon").transform;
             _obj1.transform.localScale = new Vector3(1, 1, 1);
             _obj1.transform.localPosition = new Vector3(0, 0, 0);
			GameObject chenghao = obj.transform.Find("chenghao").gameObject;
			sys._instance.get_chenghao((int)m_msg.rank_list.player_chenghao[i],chenghao);

         }
         int rank = m_msg.rank;
         if (rank == -1)
         {
             m_rank_item.transform.Find("rank").GetComponent<UILabel>().text = game_data._instance.get_t_language ("bingyuan_rank_gui.cs_83_81");//未上榜
         }
         else
         {
             m_rank_item.transform.Find("rank").GetComponent<UILabel>().text = rank + 1 + "";
 
         }
         m_rank_item.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count)
                + sys._instance.m_self.m_t_player.name;
         m_rank_item.transform.Find("num").GetComponent<UILabel>().text = get_shoujidu() + ""; ;
         m_rank_item.transform.Find("hit").GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.huiyi_jihuos.Count + "";
         GameObject _obj2 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id, sys._instance.m_self.m_t_player.dress_achieves.Count,
                                                                     sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);

         _obj2.transform.parent = m_rank_item.transform.Find("icon").transform;
         _obj2.transform.localScale = new Vector3(1, 1, 1);
         _obj2.transform.localPosition = new Vector3(0, 0, 0);
		GameObject _chenghao = m_rank_item.transform.Find("chenghao").gameObject;
		sys._instance.get_chenghao(sys._instance.m_self.m_t_player.chenghao_on,_chenghao);


     }
     void OnDisable()
     {
         TweenAlpha _alp = this.GetComponent<TweenAlpha>();
         if (_alp != null)
         {
             Destroy(_alp);
            
         }
         cmessage_center._instance.remove_handle(this);
     }
     public void set_attr()
     {
         
         s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(m_sub_id);

         string s = "[ffff00]"+ _sub.name + "[-]\n" + game_data._instance.get_t_language ("huiyilu_gui.cs_374_52") + "\n";//激活成功

         for(int i = 0;i < _sub.attrs.Count;i ++)
         {
             s += "[00ff00]" + game_data._instance.get_value_string(_sub.attrs[i], _sub.values[i]) + "[-]\n";
         }
		s += "[00ff00]"+game_data._instance.get_t_language ("huiyilu_gui.cs_522_18")+_sub.huiyis.Count+"\n";//收集度 +
         m_dacheng_des.GetComponent<UILabel>().text = s;
         dacheng();
     }


     void click(GameObject obj)
     {

         if (obj.name == "collect_bang")
         {
             protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
             net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_RANK_HUIYI_VIEW, _msg);
         }
         else if (obj.name == "collect_chengjiu")
         {
             m_chengjiu_panel.SetActive(true);
             reset_chengjiu_panel();
 
         }
         else if (obj.name == "sx_yulan")
         {
             m_sx_panel.SetActive(true);
             reset_sx_panel();
         }
         else if (obj.name == "close")
         {
             if (m_sx_flag)
             {
                 m_dialog.transform.parent.gameObject.SetActive(false);
                 set_attr();
                 m_next.SetActive(false);
                 m_continue.SetActive(true);
             }
            
         }
         else if (obj.name == "continue")
         {
             m_sx_flag = true;
             m_continue.SetActive(false);
             m_next.SetActive(true);
             m_dialog.gameObject.GetComponent<TypewriterEffect>().Finish();
             Destroy(m_dialog.gameObject.GetComponent<TypewriterEffect>());
 
         }

     }
    public static List<int> is_jihuo()
    {
        List<int> numx = new List<int>();
        foreach (int id in game_data._instance.m_dbc_huiyi_sub.m_index.Keys)
        {
            s_t_huiyi_sub sub = game_data._instance.get_t_huiyi_sub(id);
		
			if (sys._instance.m_self.is_huiyi_finish(id) ==5)			
//			if (sys._instance.m_self.m_t_player.huiyi_jihuos.Contains(id) &&  sys._instance.m_self.is_huiyi_finish(m_id) ==5)
			{
				continue;
            }
            bool temp = false;
            for (int j = 0; j < sub.huiyis.Count; j++)
            {
                if (sys._instance.m_self.get_item_num((uint)sub.huiyis[j]) == 0)
                {
                    temp = true;
                }
 
            }
            if (!temp && is_jiesuo(sub.page))
            {

                numx.Add(sub.page);
            }
           
        }
        return numx;
    }
    void reset_sub_panel(int id)
    {
        sys._instance.remove_child(m_huiyi_sub_view);
        m_huiyi_sub_view.transform.localPosition = new Vector3(0, 0, 0);
        if (m_huiyi_sub_view.GetComponent<SpringPanel>() != null)
        {
            m_huiyi_sub_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_huiyi_sub_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        m_huiyi_sub_name.text = game_data._instance.get_t_huiyi(id).name;
        List<int> _ids = new List<int>();
        for (int i = 0; i < game_data._instance.m_dbc_huiyi_sub.get_y(); i++)
        {
          
            int _id = int.Parse(game_data._instance.m_dbc_huiyi_sub.get(0, i));
            if (!(game_data._instance.get_t_huiyi_sub(_id).page == id))
            {
                continue;
 
            }
            _ids.Add(_id);
        }
        _ids.Sort(compare);

        for (int i = 0; i < _ids.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/huiyi_sub");
            obj.transform.parent = m_huiyi_sub_view.transform;
            obj.transform.localPosition = new Vector3(0, 132 - 158 * i, 0);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<huiyi_sub>().m_id = _ids[i];
            obj.GetComponent<huiyi_sub>().reset();
        }
  
    }
    int compare(int x, int y)
    {
        int flag_x = sys._instance.m_self.is_huiyi_finish(x);
        int flag_y = sys._instance.m_self.is_huiyi_finish(y);

		if (flag_x > flag_y) 
		{
			return 1;
		}
		else if (flag_x < flag_y) 
		{
			return -1;
		} 
		else 
		{
			return 0;
		}

//        if (flag_x != flag_y)
//        {
//            return flag_x - flag_y;
//        }
//        else
//        {
//            return x - y;
//        }
    }
    void reset()
    {
        this.transform.parent.GetComponent<memory_gui>().flag = false;
        s_message mes = new s_message();
        mes.m_type = "hide_memory_scene";
        cmessage_center._instance.add_message(mes);
        sys._instance.remove_child(m_view);
        List<int> temp1 = is_jihuo();
        for (int i = 0; i < game_data._instance.m_dbc_huiyi.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_huiyi.get(0,i));
            s_t_huiyi _huiyi = game_data._instance.get_t_huiyi(id);
            GameObject obj = game_data._instance.ins_object_res("ui/huiyi_sub220");
            obj.name = id + "";
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(-345 + i * 230,0,0);
            obj.transform.localScale = Vector3.one;
            obj.transform.Find("name").GetComponent<UILabel>().text = _huiyi.name;
            obj.transform.transform.Find("back").GetComponent<UISprite>().spriteName = _huiyi.icon + ".jpg";
            UIEventListener.Get(obj).onClick = select_page;
            if (is_jiesuo(_huiyi.id))
            {
                obj.transform.Find("tiaojian").gameObject.SetActive(false);
                obj.transform.Find("suo").gameObject.SetActive(false);
                obj.transform.transform.Find("back").GetComponent<UISprite>().depth = 1;
            }
            else
            {
                obj.transform.transform.Find("back").GetComponent<UISprite>().depth = 1;

                obj.transform.Find("tiaojian").gameObject.SetActive(true);
                if (_huiyi.pre_num > 0)
                {
                	obj.transform.Find("tiaojian").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huiyilu_gui.cs_695_98") , _huiyi.level//达到{0}级{nn}上一章激活{1}条回忆
                   , _huiyi.pre_num );
				}
                else
                {
                    obj.transform.Find("tiaojian").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huiyilu_gui.cs_700_101") , _huiyi.level) + "\n";//达到{0}级
                }
               
                obj.transform.Find("suo").gameObject.SetActive(true);
            }
            obj.transform.Find("num").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huiyilu_gui.cs_546_74") //回忆激活数量:
                 + get_num(_huiyi.id) + "/" + get_total_num(_huiyi.id);
          
            obj.transform.Find("effect").gameObject.SetActive(false);
			if (temp1.Contains(_huiyi.id ) )
			{
				obj.transform.Find("effect").gameObject.SetActive(true);
            }
        }
        m_dialog.transform.parent.gameObject.SetActive(false);
        m_jihuohuiyi.text = game_data._instance.get_t_language ("huiyilu_gui.cs_556_28") + " " + sys._instance.m_self.m_t_player.huiyi_jihuos.Count + "";//已激活回忆：[00ff00]
        m_shoujidu.text = game_data._instance.get_t_language ("huiyilu_gui.cs_557_26") + " " + get_shoujidu();//收集度：[00ff00]
        m_jihuohuiyi.text = game_data._instance.get_t_language ("huiyilu_gui.cs_556_28")  +  " " + sys._instance.m_self.m_t_player.huiyi_jihuos.Count + "";//已激活回忆：[00ff00]
        m_shoujidu.text = game_data._instance.get_t_language ("huiyilu_gui.cs_557_26") + " " + get_shoujidu();//收集度：[00ff00]
    }
    void select_page(GameObject obj)
    {
         m_id = int.Parse(obj.name);
         if (!is_jiesuo(m_id))
         {
             return;
         }
         reset_sub_panel(m_id);
         m_huiyi_sub_panel.SetActive(true);
    }
     public static int is_jiaobiao(int id)
     {
         for (int i = 0; i < game_data._instance.m_dbc_huiyi_sub.get_y(); i++)
         {
             int _id = int.Parse(game_data._instance.m_dbc_huiyi_sub.get(0, i));
             if (sys._instance.m_self.m_t_player.huiyi_jihuos.Contains(_id))
             {
                 continue;
             }
             s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(_id);
             if (!is_jiesuo(_sub.page))
             {
                 continue;
             }

             if (!_sub.huiyis.Contains(id))
             {
                 continue;
             }
             else
             {
                if (sys._instance.m_self.m_t_player.item_ids.Contains((uint)id))
                {
                    return 1;
                }
                int num = 0;
                for (int j = 0; j < _sub.huiyis.Count; j++)
                {
                    if (sys._instance.m_self.m_t_player.item_ids.Contains((uint)_sub.huiyis[j]))
                    {
                        num++;
                    }
                }
                if ((num + 1) == _sub.huiyis.Count)
                {
                    return 2;//急需
                }
                else
                {
                   return 3;//需要
                }
           }
        
        }
        return 1;
    }
    public static int get_shoujidu()
    {
        int num = 0;
        num = sys._instance.m_self.m_t_player.huiyi_shoujidu;
        return num;
    }
	public static int get_shoujidu_top()
	{
		int num = 0;
		num = sys._instance.m_self.m_t_player.huiyi_shoujidu_top;
		return num;
	}
	public static bool is_jiesuo(int id)
	{
		s_t_huiyi _huiyi = game_data._instance.get_t_huiyi(id);
		int num = 0;
		for(int i = 0;i <  sys._instance.m_self.m_t_player.huiyi_jihuos.Count; i ++)
		{
			s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(sys._instance.m_self.m_t_player.huiyi_jihuos[i]);
			if(_sub.page == _huiyi.id - 1)
			{
				num++;
			}
		}
		if (sys._instance.m_self.m_t_player.level >= _huiyi.level && num >= _huiyi.pre_num)
		{
			return true;
		}

		return false;

	}
	int get_num(int id)
	{
		s_t_huiyi _huiyi = game_data._instance.get_t_huiyi(id);
		int num = 0;
		for (int i = 0; i < sys._instance.m_self.m_t_player.huiyi_jihuos.Count; i++)
		{
			s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(sys._instance.m_self.m_t_player.huiyi_jihuos[i]);
			if (_sub.page == _huiyi.id)
			{
				num++;
			}
		}
		return num;
	}

	int get_total_num(int id)
	{
		s_t_huiyi _huiyi = game_data._instance.get_t_huiyi(id);
		int num = 0;
		for (int i = 0; i < game_data._instance.m_dbc_huiyi_sub.get_y(); i++)
		{
			int _id = int.Parse(game_data._instance.m_dbc_huiyi_sub.get(0, i));
			s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(_id);
			if (_sub.page == id)
			{
				num++;
			}
		}
		return num;
	}
	public void set_attr2()
	{
		
		s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(m_sub_id);
		
		string s = "[ffff00]"+ _sub.name + "[-]\n" + game_data._instance.get_t_language ("huiyilu_gui.cs_841_45") + "\n";//升星成功
		
		for(int i = 0;i < _sub.attrs.Count;i ++)
		{
			s += "[00ff00]" + game_data._instance.get_value_string(_sub.attrs[i], _sub.values2[i]) + "[-]\n";
		}
		s += "[00ff00]"+game_data._instance.get_t_language ("huiyilu_gui.cs_522_18")+_sub.huiyis.Count+"\n";//收集度 +
		m_dacheng_des.GetComponent<UILabel>().text = s;
		dacheng();
	}
	public void check_huiyi_target_done2()
	{
		int temp = get_shoujidu ();
		int m_num = 0;
		int _id = 0;
		for (int i = 0; i < game_data._instance.m_dbc_huiyi_chengjiu.get_y(); i++) {
			s_t_huiyi_chengjiu _chengjiu = game_data._instance.get_t_huiyi_chengjiu (int.Parse (game_data._instance.m_dbc_huiyi_chengjiu.get (0, i)));
			if (temp >= _chengjiu.mem_value) {
				_id = _chengjiu.id;
				m_num++;
			}
		}
		if (m_num > m_target_num) {
			m_target_num = m_num;
			
//			s_t_huiyi_chengjiu _sub1 = game_data._instance.get_t_huiyi_chengjiu (_id);
//			string s = "[ffff00]" + string.Format (game_data._instance.get_t_language ("huiyilu_gui.cs_149_89"), _sub1.mem_value) + "\n[-]";//收集度达到{0}
//			
//			{
//				s += "[00ff00]" + game_data._instance.get_value_string (_sub1.attr, _sub1.value) + "\n[-]";
//			}
			
			s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub (m_sub_id);
			
			string s = "[ffff00]" + _sub.name + "[-]\n" + game_data._instance.get_t_language ("huiyilu_gui.cs_841_45") + "\n";//升星成功
			
			for (int i = 0; i < _sub.attrs.Count; i ++) {
				s += "[00ff00]" + game_data._instance.get_value_string (_sub.attrs [i], _sub.values2 [i]) + "[-]\n";
			}
			s += "[00ff00]" + game_data._instance.get_t_language ("huiyilu_gui.cs_522_18") + _sub.huiyis.Count + "\n";//收集度 +

			s_t_huiyi_chengjiu _sub1 = game_data._instance.get_t_huiyi_chengjiu (_id);
			s += "[ffff00]" + string.Format (game_data._instance.get_t_language ("huiyilu_gui.cs_149_89"), _sub1.mem_value) + "\n[-]";//收集度达到{0}
			
			{
				s += "[00ff00]" + game_data._instance.get_value_string (_sub1.attr, _sub1.value) + "\n[-]";
			}

			m_dacheng_des.GetComponent<UILabel>().text = s;
			dacheng ();
		} else {
			s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub (m_sub_id);
			
			string s = "[ffff00]" + _sub.name + "[-]\n" + game_data._instance.get_t_language ("huiyilu_gui.cs_841_45") + "\n";//升星成功
			
			for (int i = 0; i < _sub.attrs.Count; i ++) {
				s += "[00ff00]" + game_data._instance.get_value_string (_sub.attrs [i], _sub.values2 [i]) + "[-]\n";
			}
			s += "[00ff00]" + game_data._instance.get_t_language ("huiyilu_gui.cs_522_18") + _sub.huiyis.Count + "\n";//收集度 +
			m_dacheng_des.GetComponent<UILabel>().text = s;
			dacheng ();
		}
	}
	public void check_huiyi_target_done()
		{
			int temp = get_shoujidu();
			int m_num = 0;
			int _id = 0;
			for (int i = 0; i < game_data._instance.m_dbc_huiyi_chengjiu.get_y(); i++)
        {
            s_t_huiyi_chengjiu _chengjiu = game_data._instance.get_t_huiyi_chengjiu(int.Parse(game_data._instance.m_dbc_huiyi_chengjiu.get(0, i)));
            if (temp >= _chengjiu.mem_value)
            {
                _id = _chengjiu.id;
                m_num++;
            }
        }
        if (m_num > m_target_num)
        {
            m_target_num = m_num;
          
            s_t_huiyi_chengjiu _sub1 = game_data._instance.get_t_huiyi_chengjiu(_id);
              string s = "[ffff00]" + string.Format(game_data._instance.get_t_language ("huiyilu_gui.cs_149_89") , _sub1.mem_value) + "\n[-]";//收集度达到{0}
          
            {
                s += "[00ff00]" + game_data._instance.get_value_string(_sub1.attr,_sub1.value) + "\n[-]";
            }

            m_dacheng_des.GetComponent<UILabel>().text = s;
            dacheng();
        }
    }
     public void dacheng()
     {
         if (m_dacheng_des.GetComponent<UILabel>().text != "")
         {
             m_dacheng_des.gameObject.SetActive(true);
             TweenScale _scale = sys._instance.add_scale_anim(m_dacheng_des.gameObject, 0.2f, 0.5f, 1.2f, 0);
             EventDelegate.Add(_scale.onFinished, delegate()
             {
                 s_message _message2 = new s_message();
                 _message2.m_type = "check_bf";
                 cmessage_center._instance.add_message(_message2);
                 hide();
             }, true);
         }
     }
     public void hide()
     {
         hide_Label _hide_Label = m_dacheng_des.GetComponent<hide_Label>();

         if (_hide_Label == null)
         {
             _hide_Label = m_dacheng_des.gameObject.AddComponent<hide_Label>();
         }

         _hide_Label.m_time = 1.6f;
     }

    public Texture2D get_image_half(string name)
    {
        Texture2D _texture = game_data._instance.get_object_res("ui/yh_pic/" + name, typeof(Texture2D)) as Texture2D;
        return _texture;
    }

}
