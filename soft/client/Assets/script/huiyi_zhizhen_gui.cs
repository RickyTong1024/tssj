
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class huiyi_zhizhen_gui : MonoBehaviour,IMessage {

    List<int> m_ids = new List<int>();
    List<GameObject> m_objs = new List<GameObject>();
    public GameObject m_view;
    public GameObject m_use_gui;
    public GameObject m_scro;
    s_t_huiyi_luckshop m_t_shop;
    public GameObject m_luck_shop;
    public UILabel m_jihuohuiyi;
    int m_chou_type;
    private int num = 0;
    private int num_liang = 0;
    private  float rate = 2f;
    private float sin = 30;
    private GameObject m_obj;
    public GameObject m_liang;
    protocol.game.smsg_role_huiyi_chou m_msg;
    int type = 1;
    public UILabel m_desc;
    public GameObject m_zhuan_jew;
    public GameObject m_zhuan_free;
    public GameObject m_button_toggle;
    public UILabel m_zhuanone_jewel;
    public UILabel m_zhuantwo_jewel;
    private int m_shop_type;
    bool flag = true;

    bool m_zhuan_flag = false;
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
        this.transform.Find("back").Find("Texture").gameObject.SetActive(true);
	}

    void OnEnable()
    {
        reset();
    }

     void IMessage.message(s_message message)
    {
        if (message.m_type == "refresh_zhizhen")
        {
            type = 1;
            num = 0;
            sin = 30;
            flag = true;
            if ((int)message.m_ints[0] == 1)
            {
                if (this.gameObject.activeSelf)
                {
                    StartCoroutine("roll");
                    StartCoroutine("liang");
                }
            }
            if ((1 - sys._instance.m_self.m_t_player.huiyi_chou_num) > 0)
            {
                m_zhuan_free.SetActive(true);
                m_zhuan_jew.SetActive(false);
            }
            else
            {
                m_zhuan_free.SetActive(false);
                m_zhuan_jew.SetActive(true);
            }
            memory_gui._instance.set_mask(false);
        }
        else if (message.m_type == "buy_luck_shop_item")
        {
            m_t_shop = game_data._instance.get_t_huiyi_luckshop((int)message.m_ints[0]);
            s_message _message = new s_message();
            _message.m_type = "buy_num_gui";
            _message.m_ints.Add((int)message.m_ints[0]);
            _message.m_ints.Add(13);
            cmessage_center._instance.add_message(_message);
        }
        else if (message.m_type == "refresh_luck_shop_gui")
        {
            refresh_shop(m_shop_type);
        }
    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_ROLE_HUIYI_CHOU)
        {
            m_msg = net_http._instance.parse_packet<protocol.game.smsg_role_huiyi_chou>(message.m_byte);
            if (m_chou_type == 1 || m_chou_type  == 3)
            {
                sys._instance.m_self.add_active(2200,1);
                type = 2;
                sys._instance.m_self.m_t_player.huiyi_fan_num++;
                if (sys._instance.m_self.m_t_player.huiyi_fan_num >= 5)
                {
                    sys._instance.m_self.m_t_player.huiyi_fan_num = 0;
                }
            }
            else if(m_chou_type  == 2)
            {
                sys._instance.m_self.add_active(2200, 5);

                for(int i = 0;i < m_msg.ids.Count;i ++)
                {
                    m_use_gui.GetComponent<use_items_gui>().types.Add(2);
                    m_use_gui.GetComponent<use_items_gui>().values1.Add(m_msg.ids[i]);
                    m_use_gui.GetComponent<use_items_gui>().values2.Add(1);
                    m_use_gui.GetComponent<use_items_gui>().values3.Add(0);
                }
                m_use_gui.SetActive(true);
			sys._instance.m_self.m_is_chou = true;
                for (int i = 0; i < m_msg.ids.Count; i++)
                {
                    sys._instance.m_self.add_item((uint)m_msg.ids[i], 1, false,game_data._instance.get_t_language ("huiyi_zhizhen_gui.cs_135_80"));//回忆指针获得
                    sys._instance.m_self.add_att(e_player_attr.player_luck_point, game_data._instance.get_t_huiyi_lunpan(m_msg.ids[i]).luck_point);
                }
            }
            flag = false;
             
            if (m_chou_type == 1)
            {
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, game_data._instance.get_const_vale((int)opclient_t.CONST_MINGYUN), game_data._instance.get_t_language ("huiyi_zhizhen_gui.cs_143_78"));//回忆指针消耗
            }
            else if(m_chou_type == 2)
            {
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, game_data._instance.get_const_vale((int)opclient_t.CONST_MINGYUN_FIVE),game_data._instance.get_t_language ("huiyi_zhizhen_gui.cs_143_78"));//回忆指针消耗
            }
            else if (m_chou_type == 3)
            {
                sys._instance.m_self.m_t_player.huiyi_chou_num++;
            }
        }
    }
     void click(GameObject obj)
     {
         if(!flag)
         {
             return;
         }
         if (obj.name == "rollone")
         {
             if ((1 - sys._instance.m_self.m_t_player.huiyi_chou_num) > 0)
             {
                 protocol.game.cmsg_role_huiyi_chou msg = new protocol.game.cmsg_role_huiyi_chou();
                 msg.type = 0;
                 m_chou_type = 3;
                 net_http._instance.send_msg<protocol.game.cmsg_role_huiyi_chou>(opclient_t.CMSG_ROLE_HUIYI_CHOU, msg);
                 memory_gui._instance.set_mask(true);
             }
             else if (sys._instance.m_self.m_t_player.jewel >= game_data._instance.get_const_vale((int)opclient_t.CONST_MINGYUN))
             {

                 protocol.game.cmsg_role_huiyi_chou msg = new protocol.game.cmsg_role_huiyi_chou();
                 msg.type = 0;
                 m_chou_type = 1;
                 net_http._instance.send_msg<protocol.game.cmsg_role_huiyi_chou>(opclient_t.CMSG_ROLE_HUIYI_CHOU, msg);
                 memory_gui._instance.set_mask(true);
             }
             else
             {
                 root_gui._instance.show_recharge_dialog_box(delegate()
                 {

                 });
             }
         }
         else if (obj.name == "rolltwo")
         {
             if (sys._instance.m_self.m_t_player.jewel >= game_data._instance.get_const_vale((int)opclient_t.CONST_MINGYUN_FIVE))
             {
                 protocol.game.cmsg_role_huiyi_chou msg = new protocol.game.cmsg_role_huiyi_chou();
                 msg.type = 1;
                 m_chou_type = 2;
                 net_http._instance.send_msg<protocol.game.cmsg_role_huiyi_chou>(opclient_t.CMSG_ROLE_HUIYI_CHOU, msg);
                 memory_gui._instance.set_mask(true);
             }
             else
             {
                 root_gui._instance.show_recharge_dialog_box(delegate()
                 {

                 });
             }
         }
         else if (obj.name == "luck_shop")
         {
             m_luck_shop.SetActive(true);
             refresh_shop(1);
         }
         else if (obj.name == "huiyilu")
         {
             s_message mes = new s_message();
             mes.m_type = "show_huiyilu";
             cmessage_center._instance.add_message(mes);
         }
         else if(obj.name == "zi")
         {
             refresh_shop(1);
         }
         else if (obj.name == "cheng")
         {
             refresh_shop(2);
         }
         else if (obj.name == "hong")
         {
             refresh_shop(3);
         }
     }

     public void refresh_shop(int type)
     {
         this.transform.parent.GetComponent<memory_gui>().flag = false;

         m_shop_type = type;
         List<int> shop4_ids = new List<int>();
         dbc m_dbc_shop_xg = game_data._instance.m_dbc_huiyi_luckshop;
         for (int k = 0; k < m_dbc_shop_xg.get_y(); ++k)
         {
             int xg_id = int.Parse(m_dbc_shop_xg.get(0, k));
             s_t_huiyi_luckshop _t_shop_xg = game_data._instance.get_t_huiyi_luckshop(xg_id);
             shop4_ids.Add(_t_shop_xg.id);
         }
         
         int i = 0;
         m_scro.SetActive(true);
         if (m_scro.GetComponent<SpringPanel>() != null)
         {
             m_scro.GetComponent<SpringPanel>().enabled = false;
         }
         m_scro.transform.localPosition = new Vector3(0, 0, 0);
         m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
         sys._instance.remove_child(m_scro);
         int _id = 0;
         for (i = 0; i < shop4_ids.Count; i++)
         {
             s_t_huiyi_luckshop _t_shop_xg = game_data._instance.get_t_huiyi_luckshop(shop4_ids[i]);
             if (_t_shop_xg.page != type)
             {
                 continue;
             }
             int row = _id / 2;
             int lie = _id % 2;
             int xg_id = int.Parse(m_dbc_shop_xg.get(0, i));
             
             GameObject temp = game_data._instance.ins_object_res("ui/temaihui_card");
             temp.transform.parent = m_scro.transform;
             temp.transform.localPosition = new Vector3(401 * lie - 164, -138 * row + 106, 0);
             temp.transform.localScale = new Vector3(1, 1, 1);
             temp.GetComponent<temaihui_card>().m_item_shop_gui = null;
             temp.GetComponent<temaihui_card>().m_shop_id = _t_shop_xg.id;
             temp.GetComponent<temaihui_card>().type = 13;
             temp.GetComponent<temaihui_card>().updata_ui();
             sys._instance.add_pos_anim(temp, 0.3f, new Vector3(0, 60, 0), _id * 0.05f);
             sys._instance.add_alpha_anim(temp, 0.3f, 0, 1.0f, _id * 0.05f);
             _id++;
         }
     }

     void reset()
     {
         this.transform.parent.GetComponent<memory_gui>().flag = false;
         s_message mes = new s_message();
         mes.m_type = "hide_memory_scene";
         cmessage_center._instance.add_message(mes);
         sys._instance.remove_child(m_view);
         for (int i = 0; i < 6; i++)
         {
             int id = get_id();
             GameObject obj = icon_manager._instance.create_item_icon(id);
             obj.transform.parent = m_view.transform;
             obj.transform.localPosition = new Vector3(i * 95, 0, 0);
             obj.transform.localScale = Vector3.one;
             UIButtonMessage[] mess = obj.GetComponents<UIButtonMessage>();
             for (int j = 0; j < mess.Length; j++)
             {
                 mess[j].enabled = false;
             }
             m_objs.Add(obj);
         }
         if ((1 - sys._instance.m_self.m_t_player.huiyi_chou_num) > 0)
         {
             m_zhuan_free.SetActive(true);
             m_zhuan_jew.SetActive(false);

         }
         else
         {
             m_zhuan_free.SetActive(false);
             m_zhuan_jew.SetActive(true);
         }
         m_zhuanone_jewel.text = "x" + game_data._instance.get_const_vale((int)opclient_t.CONST_MINGYUN);
         m_zhuantwo_jewel.text = "x" + game_data._instance.get_const_vale((int)opclient_t.CONST_MINGYUN_FIVE);

         int temp = (5 - sys._instance.m_self.m_t_player.huiyi_fan_num) - 1;
         if (temp > 0)
         {
             m_desc.text = string.Format(game_data._instance.get_t_language ("huiyi_zhizhen_gui.cs_326_41"), temp);//使用命运指针可获得幸运点和伙伴回忆{nn}{0}次后必得橙色或红色回忆
         }
         else
         {
             m_desc.text = game_data._instance.get_t_language ("huiyi_zhizhen_gui.cs_330_27");//使用命运指针可获得幸运点和伙伴回忆{nn}本次后必得橙色或红色回忆
 
         }        
         m_jihuohuiyi.text = game_data._instance.get_t_language ("huiyilu_gui.cs_556_28") + sys._instance.m_self.m_t_player.huiyi_jihuos.Count + "";//已激活回忆：[00ff00]
         StopAllCoroutines();
         StartCoroutine("roll");
         StartCoroutine("liang");
     }
    IEnumerator  liang()
     {
         while (true)
         {
             m_liang.transform.localPosition = new Vector3(m_liang.transform.localPosition.x, -256f + 18 * num_liang, 0);
             num_liang++;
             if (num_liang == 29)
             {
                 num_liang = 0;
                 m_liang.transform.localPosition = new Vector3(m_liang.transform.localPosition.x, -256f, 0);
             }
             yield return new WaitForSeconds(1f  -  Mathf.Sin(Mathf.Deg2Rad * sin) / 2.5f);
         }
     }
     IEnumerator roll()
     {
         while (true)
         {
             for (int i = 0; i < m_objs.Count; i++)
             {
                 m_objs[i].transform.localPosition -= new Vector3(get_rate(type) * 50 * Time.deltaTime, 0, 0);
                
             }
             if (!m_zhuan_flag)
             {
                 if (m_objs[0].transform.localPosition.x < -95)
                 {
                     Destroy(m_objs[0]);
                     m_objs.RemoveAt(0);
                     int id = get_id();
                     GameObject obj = icon_manager._instance.create_item_icon_ex(id);
                     obj.transform.parent = m_view.transform;
                     obj.transform.localPosition = new Vector3(m_objs[m_objs.Count - 1].transform.localPosition.x + 95, 0, 0);
                     obj.transform.localScale = Vector3.one;
                     UIButtonMessage[] mess = obj.GetComponents<UIButtonMessage>();
                     for (int i = 0; i < mess.Length; i++)
                     {
                         mess[i].enabled = false;
                     }
                     m_objs.Add(obj);
                 }
             }
             else
             {
                 if (m_objs[0].transform.localPosition.x <= -95)
                 {
                     Destroy(m_objs[0]);
                     m_objs.RemoveAt(0);
                     int id;
                     if (num == 0)
                     {
                         id = m_msg.ids[0];
                     }
                     else
                     {
                         id = get_id();
                     }
                     num++;
                     GameObject obj = icon_manager._instance.create_item_icon(id);
                     obj.transform.parent = m_view.transform;
                     obj.transform.localPosition = new Vector3(m_objs[m_objs.Count - 1].transform.localPosition.x + 95, 0, 0);
                     obj.transform.localScale = Vector3.one;
                     UIButtonMessage[] mess = obj.GetComponents<UIButtonMessage>();
                     for (int i = 0; i < mess.Length; i++)
                     {
                         mess[i].enabled = false;
                     }
                     if (num == 1)
                     {
                         m_obj = obj;
                     }
                     m_objs.Add(obj);

                     if (m_obj != null)
                     {
                         if (m_obj.transform.localPosition.x < 190)
                         {
                             float temp = 190 - m_obj.transform.localPosition.x;
                             for (int i = 0; i < m_objs.Count; i++)
                             {
                                 m_objs[i].transform.localPosition = new Vector3(temp + m_objs[i].transform.localPosition.x,
                                     m_objs[i].transform.localPosition.y, m_objs[i].transform.localPosition.z);

                             }
                             show_diag();
                             
                             m_obj = null;
                             StopAllCoroutines();
                             m_zhuan_flag = false;
                             type = 1;
                         }
                     }
                 }

             }
            
             if (Mathf.Abs(get_rate(type)) >= Mathf.Abs(rate + 25) && type == 2)
             {
                 m_zhuan_flag = true;
             }
             yield return  null;
         }
     }
     void show_diag()
     {
         s_message _mes = new s_message();
         _mes.m_type = "ok_msg";
         for (int i = 0; i < m_msg.ids.Count; i++)
         {
             m_use_gui.GetComponent<use_items_gui>().types.Add(2);
             m_use_gui.GetComponent<use_items_gui>().values1.Add(m_msg.ids[i]);
             m_use_gui.GetComponent<use_items_gui>().values2.Add(1);
             m_use_gui.GetComponent<use_items_gui>().values3.Add(0);
         }
         m_use_gui.SetActive(true);
		sys._instance.m_self.m_is_chou = true;
         for (int i = 0; i < m_msg.ids.Count; i++)
         {
             sys._instance.m_self.add_item((uint)m_msg.ids[i], 1, false,game_data._instance.get_t_language ("huiyi_zhizhen_gui.cs_135_80"));//回忆指针获得
             sys._instance.m_self.add_att(e_player_attr.player_luck_point, game_data._instance.get_t_huiyi_lunpan(m_msg.ids[i]).luck_point);
         }
         int temp = (5 - sys._instance.m_self.m_t_player.huiyi_fan_num) - 1;
         if (temp > 0)
         {
             m_desc.text = string.Format(game_data._instance.get_t_language ("huiyi_zhizhen_gui.cs_326_41"), temp );//使用命运指针可获得幸运点和伙伴回忆{nn}{0}次后必得橙色或红色回忆
         }
         else
         {
             m_desc.text = game_data._instance.get_t_language ("huiyi_zhizhen_gui.cs_330_27");//使用命运指针可获得幸运点和伙伴回忆{nn}本次后必得橙色或红色回忆

         }
     }
    
     float get_rate(int type)
     {
         float temp = rate;
         if (type == 1)
         {
             return temp;
         }
         else
         {
             if (sin >= 180)
             {
                 sin = 30;
             }
             temp = rate + 30 * Mathf.Sin(Mathf.Deg2Rad * sin);
             sin += 0.5f;

         }
         return temp;
 
     }
     int get_id()
     {
         if (m_ids.Count == 0)
         {
             for (int i = 0; i < game_data._instance.m_dbc_huiyi_lunpan.get_y(); i++)
             {
                 m_ids.Add(int.Parse(game_data._instance.m_dbc_huiyi_lunpan.get(0,i)));
             }
             
         }
         int id = Random.Range(0, m_ids.Count);
         int temp = m_ids[id];
         m_ids.Remove(temp);
         return temp;
       
     }
     void OnDestroy()
     {
         cmessage_center._instance.remove_handle(this);
     }
    public void close()
     {
         m_button_toggle.GetComponent<UIToggle>().value = true;

     }
     public  void OnDisable()
     {
         StopCoroutine("roll");
         StopCoroutine("liang");
         for (int i = 0; i < m_objs.Count; i++)
         {
             Destroy(m_objs[i]);
         }
         m_objs.Clear();
         sin = 30f;
         type = 1;
         num = 0;
         flag = true;
         TweenAlpha _alp = this.GetComponent<TweenAlpha>();
         if (_alp != null)
         {
             Destroy(_alp);
         }
         
     }
}
