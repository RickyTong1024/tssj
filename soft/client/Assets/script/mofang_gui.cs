
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mofang_gui : MonoBehaviour,IMessage{
    public UILabel m_daojishi;
    public UILabel m_his_jifen;
    public UILabel m_leave_jifen;
    public UILabel m_all_jifen;
    public UILabel m_chou_jewel;
    public UILabel m_yijian_jewel;

    public GameObject m_yulan_gui;
    public GameObject m_info_gui;
    public GameObject m_quanfu_gui;
    public GameObject m_shop_gui;
    public GameObject m_mofang;
    public GameObject m_chouqu;
    public GameObject m_yijian_chouqu;
    public GameObject m_reset;
    public GameObject m_refresh;
    public GameObject m_use_items;
    public GameObject m_target_effect;
    public GameObject m_jiesu;
    public GameObject m_main;
    public protocol.game.smsg_huodong_mofang_view m_view_data;
    public static mofang_gui _instance;
    int select_id;
    public GameObject m_mask;
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
        _instance = this;
	}
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    void IMessage.message(s_message mes)
    {
        if (mes.m_type == "daily_refresh")
        {

            if (m_view_data != null)
            {
                m_view_data.num = 0;
                m_view_data.free_num = 0;
                m_view_data.shop_ids.Clear();
                m_view_data.shop_nums.Clear();
                reset();
            }


        }
        else if (mes.m_type == "refresh_mofang")
        {
            reset();
        }


 
    }
     void IMessage.net_message(s_net_message msg)
    {
        if (msg.m_opcode == opclient_t.CMSG_HUODONG_MOFANG_VIEW)
        {
            m_view_data = net_http._instance.parse_packet<protocol.game.smsg_huodong_mofang_view>(msg.m_byte);
            reset();
        }
        else if (msg.m_opcode == opclient_t.CMSG_HUODONG_MOFANG_REFRESH || msg.m_opcode == opclient_t.CMSG_HUODONG_MOFANG_RESET)
        {
            protocol.game.smsg_huodong_mofang_refresh refresh = net_http._instance.parse_packet<protocol.game.smsg_huodong_mofang_refresh>(msg.m_byte);
            m_view_data.ids.Clear();
            for (int i = 0; i < refresh.ids.Count; i++)
            {
                m_view_data.ids.Add(refresh.ids[i]);
 
            }
            m_view_data.stat = 0;
            if (msg.m_opcode == opclient_t.CMSG_HUODONG_MOFANG_REFRESH)
            {
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, 20, game_data._instance.get_t_language ("mofang_gui.cs_82_77"));//九宫魔方刷新消耗
            }
            for (int i = 0; i < m_view_data.ids.Count; i++)
            {
                add_effect(m_mofang.transform.Find(i + "").gameObject);
                m_mofang.transform.Find(i + "").GetComponent<BoxCollider>().enabled = false;
            }
            Invoke("reset", 1);
        }
        else if (msg.m_opcode == opclient_t.CMSG_HUODONG_MOFANG_CHOU)
        {
            protocol.game.smsg_huodong_mofang mofang = net_http._instance.parse_packet<protocol.game.smsg_huodong_mofang>(msg.m_byte);
            if (mofang.refresh_ids.Count == 0)
            {
                m_view_data.ids[select_id] = mofang.id;

            }
            else
            {
                m_view_data.ids.Clear();
                for (int i = 0; i < mofang.refresh_ids.Count; i++)
                {
                    m_view_data.ids.Add(mofang.refresh_ids[i]);

                }
                m_view_data.stat = 0;
 
            }
            if (m_view_data.free_num < 5)
            {
                m_view_data.free_num++;
            }
            else
            {
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, game_data._instance.get_t_price(++m_view_data.num).mofang,game_data._instance.get_t_language ("mofang_gui.cs_116_131"));//九宫魔方抽取消耗
            }
           
            s_t_mofang mo = game_data._instance.get_t_mofang(mofang.id);
            GameObject o = m_mofang.transform.Find(select_id + "").gameObject;
            if (mo.leixing == 1)
            {
                m_view_data.left_point += 15;
                m_view_data.server_point += 15;
                m_view_data.total_point += 15;
 
            }
            else if (mo.leixing == 2)
            {
                m_view_data.left_point += 5;
                m_view_data.server_point += 5;
                m_view_data.total_point += 5;

            }
            else
            {
                m_view_data.left_point += 1;
                m_view_data.server_point += 1;
                m_view_data.total_point += 1;
            }
            if (mo.leixing == 1)
            {
                m_mofang.transform.Find(select_id + "").Find("shell").gameObject.SetActive(true);
            }
            add_role(o).AddOnFinished(
                delegate()
                {
                    sys._instance.m_self.add_reward(mo.type, mo.value1, mo.value2, mo.value3,game_data._instance.get_t_language ("mofang_gui.cs_148_93"));//九宫魔方抽取
                    GameObject icon = m_mofang.transform.Find(select_id + "").Find("icon").gameObject;
                    sys._instance.remove_child(icon);
                    m_mofang.transform.Find(select_id + "").Find("pai").transform.localEulerAngles = Vector3.zero;
                    m_mofang.transform.Find(select_id + "").Find("effect").gameObject.SetActive(false);
                    {
                        GameObject obj = icon_manager._instance.create_reward_icon(mo.type, mo.value1, mo.value2, mo.value3);
                        obj.transform.parent = icon.transform;
                        obj.transform.localPosition = Vector3.zero;
                        obj.transform.localScale = Vector3.one;
                        UISprite kuang = null;
                        if (mo.type == 1)
                        {
                            kuang = obj.transform.Find("kuang").GetComponent<UISprite>();

                        }
                        else
                        {
                            kuang = obj.transform.Find("bg").GetComponent<UISprite>();
                        }
                        if (mo.leixing == 1)
                        {
                            kuang.spriteName = "xtbk_chpt001";
                        }
                        else if (mo.leixing == 2)
                        {
                            kuang.spriteName = "xtbk_zipt001";
                        }
                        else if (mo.leixing == 3)
                        {
                            kuang.spriteName = "xtbk_lanpt001";
                        }
                        m_mofang.transform.Find(select_id + "").GetComponent<BoxCollider>().enabled = false;
                        if (mo.leixing == 1)
                        {
                            m_mofang.transform.Find(select_id + "").Find("shell").gameObject.SetActive(true);
                        }

                    }
                    if (mofang.refresh_ids.Count == 0)
                    {
                        reset();
                    }
                    else
                    {
                        Invoke("add_all_effect",1);
                        Invoke("reset", 2);
                    }
                }

                );
        }
        else if (msg.m_opcode == opclient_t.CMSG_HUODONG_MOFANG_START)
        {
            GameObject o = null;
            for (int i = 0; i < m_mofang.transform.childCount; i++)
            {
                o = m_mofang.transform.Find(i + "").gameObject;
                add_role(o);
            }
            m_view_data.stat = 1;
            m_view_data.ids.Clear();
            for (int i = 0; i < 9; i++)
            {
                m_view_data.ids.Add(0);

            }

            o.transform.Find("pai").GetComponent<TweenRotation>().AddOnFinished(

                delegate()
                {
                    reset();
                }

                );

 
        }
        else if(msg.m_opcode == opclient_t.CMSG_HUODONG_MOFANG_ALL)
         {
             m_use_items.GetComponent<use_items_gui>().types.Clear();
             m_use_items.GetComponent<use_items_gui>().values1.Clear();
             m_use_items.GetComponent<use_items_gui>().values2.Clear();
             m_use_items.GetComponent<use_items_gui>().values3.Clear();

             for (int i = 0; i < m_view_data.ids.Count; i++)
             {
                 s_t_mofang mofang = game_data._instance.get_t_mofang(m_view_data.ids[i]);
                 m_use_items.GetComponent<use_items_gui>().types.Add(mofang.type);
                 m_use_items.GetComponent<use_items_gui>().values1.Add(mofang.value1);
                 m_use_items.GetComponent<use_items_gui>().values2.Add(mofang.value2);
                 m_use_items.GetComponent<use_items_gui>().values3.Add(mofang.value3);
                 sys._instance.m_self.add_reward(mofang.type, mofang.value1, mofang.value2, mofang.value3, false,game_data._instance.get_t_language ("mofang_gui.cs_241_113"));//九宫魔方一键获得
                 if (mofang.leixing == 1)
                 {
                     m_view_data.left_point += 15;
                     m_view_data.server_point += 15;
                     m_view_data.total_point += 15;

                 }
                 else if (mofang.leixing == 2)
                 {
                     m_view_data.left_point += 5;
                     m_view_data.server_point += 5;
                     m_view_data.total_point += 5;

                 }
                 else
                 {
                     m_view_data.left_point += 1;
                     m_view_data.server_point += 1;
                     m_view_data.total_point += 1;
                 }
             }
             m_use_items.SetActive(true);
             int yijian_jewel = 0;
             for (int i = 0; i < 9; i++)
             {
                 yijian_jewel += game_data._instance.get_t_price(m_view_data.num + 1 + i).mofang;
             }
             yijian_jewel = (int)(yijian_jewel * 0.75f) / 10  * 10;
             m_view_data.num += 9;
             sys._instance.m_self.sub_att(e_player_attr.player_jewel, yijian_jewel, game_data._instance.get_t_language ("mofang_gui.cs_271_84"));//九宫魔方一键消耗
             protocol.game.smsg_huodong_mofang_refresh refresh = net_http._instance.parse_packet<protocol.game.smsg_huodong_mofang_refresh>(msg.m_byte);
             m_view_data.ids.Clear();
             for (int i = 0; i < refresh.ids.Count; i++)
             {
                 m_view_data.ids.Add(refresh.ids[i]);

             }
             reset();
         }

 
    }
     void add_all_effect()
     {
         for (int i = 0; i < m_view_data.ids.Count; i++)
         {
             add_effect(m_mofang.transform.Find(i + "").gameObject);
             m_mofang.transform.Find(i + "").GetComponent<BoxCollider>().enabled = false;
         }
     }
     public void reset()
     {
         CancelInvoke();
         InvokeRepeating("time", 0, 1);
         m_his_jifen.text = string.Format(game_data._instance.get_t_language ("mofang_gui.cs_296_42"), m_view_data.total_point);//我的历史积分：{0}
         m_leave_jifen.text = string.Format(game_data._instance.get_t_language ("mofang_gui.cs_297_44"), m_view_data.left_point);//我的剩余积分：{0}
         m_all_jifen.text = string.Format(game_data._instance.get_t_language ("mofang_gui.cs_298_42"), m_view_data.server_point);//全服累计积分：{0}
         int yijian_jewel = 0;
         m_mask.SetActive(false);
         for (int i = 0; i < 9; i++)
         {
             yijian_jewel += game_data._instance.get_t_price(m_view_data.num + 1 + i).mofang;
         }
         yijian_jewel = (int)(yijian_jewel * 0.75f) / 10 * 10;
         m_yijian_jewel.text = "x" + yijian_jewel;
         if (m_view_data.free_num < 5)
         {
             m_chou_jewel.gameObject.SetActive(false);
             m_chou_jewel.transform.parent.Find("free").gameObject.SetActive(true);

         }
         else
         {
             m_chou_jewel.gameObject.SetActive(true);
             m_chou_jewel.transform.parent.Find("free").gameObject.SetActive(false);
             m_chou_jewel.text = "x" + game_data._instance.get_t_price(m_view_data.num + 1).mofang + "";
         }
         for (int i = 0; i < m_view_data.ids.Count; i++)
         {
             GameObject icon = m_mofang.transform.Find(i + "").Find("icon").gameObject;
             sys._instance.remove_child(icon);
             m_mofang.transform.Find(i + "").Find("pai").transform.localEulerAngles = Vector3.zero;
             m_mofang.transform.Find(i + "").name = i + "";
             m_mofang.transform.Find(i + "").Find("effect").gameObject.SetActive(false);
             m_mofang.transform.Find(i + "").Find("shell").gameObject.SetActive(false);
             if (m_view_data.ids[i] != 0)
             {
                 s_t_mofang mofang = game_data._instance.get_t_mofang(m_view_data.ids[i]);
                 GameObject obj = icon_manager._instance.create_reward_icon(mofang.type, mofang.value1, mofang.value2, mofang.value3);
                 obj.transform.parent = icon.transform;
                 obj.transform.localPosition = Vector3.zero;
                 obj.transform.localScale = Vector3.one;
                 UISprite kuang = null;
                 if (mofang.type == 1)
                 {
                     kuang = obj.transform.Find("kuang").GetComponent<UISprite>();
 
                 }
                 else
                 {
                     kuang = obj.transform.Find("bg").GetComponent<UISprite>();
                 }
                 if (mofang.leixing == 1)
                 {
                     kuang.spriteName = "xtbk_chpt001";
                 }
                 else if (mofang.leixing == 2)
                 {
                     kuang.spriteName = "xtbk_zipt001";
                 }
                 else if (mofang.leixing == 3)
                 {
                     kuang.spriteName = "xtbk_lanpt001";
                 }
                 m_mofang.transform.Find(i + "").GetComponent<BoxCollider>().enabled = false;
                 if (mofang.leixing == 1)
                 {
                     m_mofang.transform.Find(i + "").Find("shell").gameObject.SetActive(true);
                 }

             }
             else
             {
                 m_mofang.transform.Find(i + "").GetComponent<BoxCollider>().enabled = true;

             }
       
         }
         m_main.SetActive(true);
         m_jiesu.SetActive(false);
         if (m_view_data.stat == 0)
         {
             m_chouqu.SetActive(true);
             m_yijian_chouqu.SetActive(true);
             m_reset.SetActive(false);
             m_refresh.SetActive(true);
             m_chou_jewel.transform.parent.gameObject.SetActive(false);
         }
         else
         {
             m_refresh.SetActive(false);
             m_chouqu.SetActive(false);
             m_yijian_chouqu.SetActive(false);
             m_chou_jewel.transform.parent.gameObject.SetActive(true);

             if (is_chou())
             {
                 m_reset.SetActive(true);
             }
             else
             {
                 m_reset.SetActive(false);
             }
         }

         if (mofang_target_gui.is_effect())
         {
             m_target_effect.SetActive(true);
         }
         else
         {
             m_target_effect.SetActive(false);
         }
 
     }
     bool is_chou()
     {
         for (int i = 0; i < m_view_data.ids.Count; i++)
         {
             if (m_view_data.ids[i] != 0)
             {
                 return true;
             }
         }
         return false;
     }
     void select(GameObject obj)
     {
         select_id = int.Parse(obj.name);
        
         if (m_view_data.free_num  >= 5)
         {
             int jewel = game_data._instance.get_t_price(m_view_data.num + 1).mofang;
             if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < jewel)
             {
                 root_gui._instance.show_recharge_dialog_box(

                     delegate()
                     {
                         this.gameObject.SetActive(true);
                     }
                     );
                 return;
 
             }
 
         }

         protocol.game.cmsg_huodong_mofang mofang = new protocol.game.cmsg_huodong_mofang();
         mofang.index = select_id;
         net_http._instance.send_msg<protocol.game.cmsg_huodong_mofang>(opclient_t.CMSG_HUODONG_MOFANG_CHOU, mofang);
 
     }
     void time()
     {
         long time = (long)(m_view_data.time - timer.now());
         if (time > 0)
         {
             m_daojishi.text = string.Format(game_data._instance.get_t_language ("mofang_gui.cs_450_45"), timer.get_time_show_ex((long)time));//活动倒计时：{0}
         }
         else
         {
             m_daojishi.text = game_data._instance.get_t_language ("mofang_gui.cs_454_31");//活动倒计时：已结束
             m_main.SetActive(false);
             m_jiesu.SetActive(true);
             CancelInvoke();
 
         }

 
     }
     void OnEnable()
     {
         protocol.game.cmsg_common com = new protocol.game.cmsg_common();
         net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG_MOFANG_VIEW, com);
     }
    public void chongzhi_mofang()
    {


         

    }
    TweenRotation add_role(GameObject obj)
    {
        obj.transform.Find("effect").gameObject.SetActive(true);
        obj = obj.transform.Find("pai").gameObject;        
        TweenRotation cur = obj.GetComponent<TweenRotation>();
        DestroyImmediate(cur);
        TweenRotation _ro = obj.AddComponent<TweenRotation>();
        _ro.from = obj.transform.localRotation.eulerAngles;
        _ro.to = new Vector3(0, 180, 0);
        _ro.duration = 1f;
        _ro.enabled = true;
        m_mask.SetActive(true);
        return _ro;
       
    }
    void add_effect(GameObject obj)
    {
        m_mask.SetActive(true);
        obj.transform.Find("effect").gameObject.SetActive(true);
    }
    public void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            if (m_info_gui.activeSelf)
            {
                m_info_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                return;
            }
            else if (m_yulan_gui.activeSelf)
            {
                m_yulan_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                return;
            }
            else if (m_shop_gui.activeSelf)
            {
                m_shop_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                return;
            }
            else if (m_quanfu_gui.activeSelf)
            {
                m_quanfu_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                return;
            }
            s_message mes = new s_message();
            mes.m_type = "show_main_gui";
            cmessage_center._instance.add_message(mes);
            Destroy(this.gameObject);

        }
        else if (obj.name == "info")
        {
            m_info_gui.SetActive(true);
 
        }
        else if (obj.name == "shop")
        {
            m_shop_gui.SetActive(true);
        }
        else if (obj.name == "yulan")
        {
            m_yulan_gui.SetActive(true);
 
        }
        else if(obj.name == "rank")
        {
            m_quanfu_gui.SetActive(true);
        }
        else if (obj.name == "yijian")
        {
            int yijian_jewel = 0;
            for (int i = 0; i < 9; i++)
            {
                yijian_jewel += game_data._instance.get_t_price(m_view_data.num + 1 + i).mofang;
            }
         	yijian_jewel = (int)(yijian_jewel * 0.75f) / 10 * 10;
			
            if (sys._instance.m_self.get_att(e_player_attr.player_jewel) >= yijian_jewel)
            {
                protocol.game.cmsg_common common = new protocol.game.cmsg_common();
                net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG_MOFANG_ALL, common);

            }
            else
            {
                root_gui._instance.show_recharge_dialog_box(
                    delegate()
                    {

                    }
                    );
 
            }
 
        }
        else if (obj.name == "select")
        {
            if (m_view_data.stat == 0)
            {
                protocol.game.cmsg_common com = new protocol.game.cmsg_common();
                net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG_MOFANG_START, com);
 
            }
          
           
 
        }
        else if (obj.name == "reset")
        {
            protocol.game.cmsg_common com = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG_MOFANG_RESET, com);

        }
        else if(obj.name == "refresh")
        {
            if (sys._instance.m_self.get_att(e_player_attr.player_jewel) >= 20)
            {
                protocol.game.cmsg_common com = new protocol.game.cmsg_common();
                net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG_MOFANG_REFRESH, com);
            }
            else
            {
                root_gui._instance.show_recharge_dialog_box(
                    delegate()
                    {
                        this.gameObject.SetActive(false);

                    }
                    );
 
            }
           
        }

    }
	
}
