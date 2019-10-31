
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class huiyi_zhanbu_gui : MonoBehaviour,IMessage {
    protocol.game.smsg_role_huiyi_zhanpu m_msg;
    public List<GameObject> m_objs;
    public GameObject m_start;
    public GameObject m_button1;
    public GameObject m_button2;
    public UILabel m_num;
    public UILabel m_jihuohuiyi;
    public  bool flag = false;
    private int m_change_num = 0;
    private int m_chang_flag = 2;
    public int m_id;
    public bool flag_id = false;
    public GameObject m_gaiyun_jew;
    public GameObject m_gaiyun_free;

	void Start () 
    {
        cmessage_center._instance.add_handle(this);	
	}
    void OnEnable()
    {
        reset();
    }
	// Update is called once per frame
	void Update () 
    {
	
	}
    public void set_flag()
    {
        flag = true;
        for (int i = 0; i < m_objs.Count; i++)
        {
            Destroy(m_objs[i].GetComponent<TweenRotation>());
            m_objs[i].transform.localRotation  = Quaternion.identity;
 
        }
    }
    void IMessage.message(s_message message)
    {

    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_ROLE_HUIYI_ZHANPU)
        {
             m_msg = net_http._instance.parse_packet<protocol.game.smsg_role_huiyi_zhanpu>(message.m_byte);
             if ((game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip).huiyi_zhanbu_num -
                 sys._instance.m_self.m_t_player.huiyi_zhanpu_num) > 0)
             {
                 sys._instance.m_self.m_t_player.huiyi_zhanpu_num++;
             }
             m_num.text = string.Format(game_data._instance.get_t_language ("huiyi_zhanbu_gui.cs_70_40") , (game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip).huiyi_zhanbu_num -//今日还可占卜：{0}次
            sys._instance.m_self.m_t_player.huiyi_zhanpu_num));
             sys._instance.m_self.m_t_player.huiyi_zhanpus.Clear();
             sys._instance.m_self.m_t_player.huiyi_zhanpu_flag = 1;
             sys._instance.m_self.add_active(2300,1);
             for (int i = 0; i < m_msg.ids.Count; i++)
             {
                 sys._instance.m_self.m_t_player.huiyi_zhanpus.Add(m_msg.ids[i]);
             }
      
             m_start.SetActive(false);
             m_button1.SetActive(true);
             m_button2.SetActive(false);
             fanzhuan();
        }
        else if (message.m_opcode == opclient_t.CMSG_ROLE_HUIYI_GAIYUN)
        {
            m_msg = net_http._instance.parse_packet<protocol.game.smsg_role_huiyi_zhanpu>(message.m_byte);
            sys._instance.m_self.m_t_player.huiyi_zhanpus.Clear();
            sys._instance.m_self.m_t_player.huiyi_zhanpu_flag = 1;
            for (int i = 0; i < m_msg.ids.Count; i++)
            {
                sys._instance.m_self.m_t_player.huiyi_zhanpus.Add(m_msg.ids[i]);
            }
            if ((3 - sys._instance.m_self.m_t_player.huiyi_gaiyun_num) > 0)
            {
                sys._instance.m_self.m_t_player.huiyi_gaiyun_num++;
            }
            else
            {
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, 100,game_data._instance.get_t_language ("huiyi_zhanbu_gui.cs_100_77"));//回忆改运消耗
 
            }
            m_start.SetActive(false);
            m_button1.SetActive(true);
            m_button2.SetActive(false);
            if ((3 - sys._instance.m_self.m_t_player.huiyi_gaiyun_num) > 0)
            {
                m_gaiyun_jew.SetActive(false);
                m_gaiyun_free.SetActive(true);
                m_gaiyun_free.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("explore_gui.cs_323_59") , (3 - sys._instance.m_self.m_t_player.huiyi_gaiyun_num));//免费{0}次
            }
            else
            {
                m_gaiyun_jew.SetActive(true);
                m_gaiyun_free.SetActive(false);

            }

          fanzhuan();
        }
        else if (message.m_opcode == opclient_t.CMSG_ROLE_HUIYI_FANPAI)
        {
			sys._instance.m_self.m_is_chou = true;
            sys._instance.m_self.add_item((uint)sys._instance.m_self.m_t_player.huiyi_zhanpus[m_id], 1, false,game_data._instance.get_t_language ("huiyi_zhanbu_gui.cs_124_110"));//回忆翻牌获得
            
            sys._instance.m_self.m_t_player.huiyi_zhanpu_flag = 0;
            flag_id = false;
            fanzhuan2();
        }

    }
    void fanzhuan2()
    {
        flag = false;
        TweenRotation _ro = m_objs[m_id].AddComponent<TweenRotation>();
        _ro.from = m_objs[m_id].transform.localRotation.eulerAngles;
       
        
        _ro.to = new Vector3(0, 180, 0);
        
      
        _ro.duration = 0.5f;
        _ro.enabled = true;
        _ro.AddOnFinished(delegate() {
            GameObject obj = icon_manager._instance.create_item_icon(sys._instance.m_self.m_t_player.huiyi_zhanpus[m_id]);
            obj.transform.parent = m_objs[m_id].transform.Find("icon");
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            m_objs[m_id].GetComponent<UISprite>().spriteName = "huiyi_paimian";
            m_objs[m_id].transform.Find("effect").gameObject.SetActive(true);
            int num = 0;
            for (int i = 0; i < m_objs.Count; i++)
            {
                if (i == m_id)
                {
                    continue;
                }
                num++;
                TweenRotation _ro1 = m_objs[i].AddComponent<TweenRotation>();
                _ro1.from = m_objs[i].transform.localRotation.eulerAngles;
               
                
                 _ro1.to = new Vector3(0, 180, 0);
                
          
                _ro1.duration = 0.5f;
                _ro1.enabled = true;
                if (num == 1)
                {
                    _ro1.AddOnFinished(delegate()
                    {
                        flag = true;
                        for (int j = 0; j < m_objs.Count; j++)
                        {
                            if (j == m_id)
                            {
                                continue;
                            }
                            GameObject obj1 = icon_manager._instance.create_item_icon(sys._instance.m_self.m_t_player.huiyi_zhanpus[j]);
                            obj1.transform.parent = m_objs[j].transform.Find("icon");
                            obj1.transform.localPosition = Vector3.zero;
                            obj1.transform.localScale = Vector3.one;
                            m_objs[j].GetComponent<UISprite>().spriteName = "huiyi_paimian";
                        }
                        m_start.SetActive(false);
                        m_button1.SetActive(false);
                        m_button2.SetActive(true);
                        s_t_huiyi_destiny _sub = game_data._instance.get_t_huiyi_destiny(sys._instance.m_self.m_t_player.huiyi_zhanpus[m_id]);
                        sys._instance.m_self.add_att(e_player_attr.player_huiyi_point,_sub.huiyi_point);
						sys._instance.m_self.check_huiyi_xuyao();
                    });
                }
            }
        });
 
    }
    void fanzhuan(int type = 0)
    {
        flag = false;
        flag_id = false;
        for (int i = 0; i < m_objs.Count; i++)
        {
            TweenRotation _ro = m_objs[i].AddComponent<TweenRotation>();
            _ro.from = m_objs[i].transform.localRotation.eulerAngles;
            if (type == 0)
            {
                _ro.to = new Vector3(0, 180, 0);
            }
            else 
            {
                _ro.to = new Vector3(0, 0, 0);
            }
            _ro.duration = 0.5f;
            _ro.enabled = true;
            if (i == 0)
            {
                if (type == 0)
                {
                    _ro.AddOnFinished(finshed);

                }
                else if (type == 1)
                {
                    _ro.AddOnFinished(finshed1);

                }
                else if (type == 3)
                {
                    _ro.AddOnFinished(finished3);
 
                }
                else
                {
                    _ro.AddOnFinished(finished2);

                }
            }

        }
        
    }
    void finshed()
    {
        for (int i = 0; i < m_objs.Count; i++)
        {
            GameObject obj = icon_manager._instance.create_item_icon(m_msg.ids[i]);
            obj.transform.parent = m_objs[i].transform.Find("icon");
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            m_objs[i].GetComponent<UISprite>().spriteName = "huiyi_paimian";
        }
        flag = true;
        for (int i = 0; i < m_objs.Count; i++)
        {
            Destroy(m_objs[i].GetComponent<TweenRotation>());
        }
 
    }
    void finshed1()
    {
        for (int i = 0; i < m_objs.Count; i++)
        {
            sys._instance.remove_child(m_objs[i].transform.Find("icon").gameObject);
            m_objs[i].GetComponent<UISprite>().spriteName = "huiyi_paibei";
        }
        flag = true;
        for (int i = 0; i < m_objs.Count; i++)
        {
            Destroy(m_objs[i].GetComponent<TweenRotation>());
        }
        if (3 - sys._instance.m_self.m_t_player.huiyi_gaiyun_num > 0)
        {
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_ROLE_HUIYI_GAIYUN, _msg);
        }
        else if (sys._instance.m_self.get_att(e_player_attr.player_jewel) >= 100)
        {
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_ROLE_HUIYI_GAIYUN, _msg);
        }
        else
        {
            root_gui._instance.show_recharge_dialog_box(delegate { });
        }
    }
    void finished3()
    {
        for (int i = 0; i < m_objs.Count; i++)
        {
            sys._instance.remove_child(m_objs[i].transform.Find("icon").gameObject);
            m_objs[i].GetComponent<UISprite>().spriteName = "huiyi_paibei";
        }
        flag = true;
        for (int i = 0; i < m_objs.Count; i++)
        {
            Destroy(m_objs[i].GetComponent<TweenRotation>());
        }
        if ((game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip).huiyi_zhanbu_num - sys._instance.m_self.m_t_player.huiyi_zhanpu_num) > 0)
        {
            protocol.game.cmsg_common msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_ROLE_HUIYI_ZHANPU, msg);

        }
        else
        {
            root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("huiyi_zhanbu_gui.cs_306_54"));//[ffc882]今日次数已耗尽，提升VIP等级可以增加次数
        }

    }
    void finished2()
    {
        for (int i = 0; i < m_objs.Count; i++)
        {
            sys._instance.remove_child(m_objs[i].transform.Find("icon").gameObject);
            m_objs[i].GetComponent<UISprite>().spriteName = "huiyi_paibei";
        }
        for (int i = 0; i < m_objs.Count; i++)
        {
            Destroy(m_objs[i].GetComponent<TweenRotation>());
        }
        Invoke("change",1f);
       
    }
    void reset()
    {
        m_start.SetActive(true);
        m_button1.SetActive(false);
        m_button2.SetActive(false);
        this.transform.parent.GetComponent<memory_gui>().flag = false;
        s_message mes = new s_message();
        mes.m_type = "hide_memory_scene";
        cmessage_center._instance.add_message(mes);
        if (sys._instance.m_self.m_t_player.huiyi_zhanpu_flag == 0)
        {
            for (int i = 0; i < m_objs.Count; i++)
            {
                m_objs[i].GetComponent<UISprite>().spriteName = "huiyi_paibei";
                m_objs[i].transform.Find("effect").gameObject.SetActive(false);
                sys._instance.remove_child(m_objs[i].transform.Find("icon").gameObject);
                m_objs[i].transform.Find("effect").gameObject.SetActive(false);
                m_objs[i].transform.localPosition = new Vector3(-250 + 250 * i, 0, 0);
                TweenRotation _ro = m_objs[i].AddComponent<TweenRotation>();
                _ro.from = m_objs[i].transform.localRotation.eulerAngles;
                _ro.to = new Vector3(0, 360, 0);
                _ro.delay = i * 0.2f;
                _ro.duration = 1f;
                _ro.enabled = true;
                if (i == 0)
                {
                    _ro.AddOnFinished(set_flag);

                }

            }

        }
        else
        {
            flag = true;
            for (int i = 0; i < m_objs.Count; i++)
            {
                m_objs[i].transform.localRotation = Quaternion.Euler(new Vector3(0, -180, 0)); ;
                GameObject obj = icon_manager._instance.create_item_icon(sys._instance.m_self.m_t_player.huiyi_zhanpus[i]);
                sys._instance.remove_child(m_objs[i].transform.Find("icon").gameObject);
                obj.transform.parent = m_objs[i].transform.Find("icon");
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                m_objs[i].GetComponent<UISprite>().spriteName = "huiyi_paimian";
                m_objs[i].transform.Find("effect").gameObject.SetActive(false);
                
                m_button1.SetActive(true);
                m_button2.SetActive(false);
                m_start.SetActive(false);
            }
        }
        if ((3 - sys._instance.m_self.m_t_player.huiyi_gaiyun_num) > 0)
        {
            m_gaiyun_jew.SetActive(false);
            m_gaiyun_free.SetActive(true);
            m_gaiyun_free.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("explore_gui.cs_323_59") , (3 - sys._instance.m_self.m_t_player.huiyi_gaiyun_num) );//免费{0}次

        }
        else
        {
            m_gaiyun_jew.SetActive(true);
            m_gaiyun_free.SetActive(false);

        }
        m_jihuohuiyi.text = game_data._instance.get_t_language ("huiyilu_gui.cs_556_28") + sys._instance.m_self.m_t_player.huiyi_jihuos.Count + "";//已激活回忆：[00ff00]
        m_num.text = string.Format(game_data._instance.get_t_language ("huiyi_zhanbu_gui.cs_70_40")  , (game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip).huiyi_zhanbu_num  - //今日还可占卜：{0}次
            sys._instance.m_self.m_t_player.huiyi_zhanpu_num));
    }
    void click(GameObject obj)
    {
        if (!flag)
        {
            return;
        }
        if (obj.name == "start")
        {
            if ((game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip).huiyi_zhanbu_num - sys._instance.m_self.m_t_player.huiyi_zhanpu_num) > 0)
            {
                protocol.game.cmsg_common msg = new protocol.game.cmsg_common();
                net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_ROLE_HUIYI_ZHANPU, msg);
              
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("huiyi_zhanbu_gui.cs_306_54"));//[ffc882]今日次数已耗尽，提升VIP等级可以增加次数
            }
               
 
        }
       
        else if (obj.name == "gaiyun")
        {
            fanzhuan(1);
           
 
        }
        else if (obj.name == "again")
        {
            for (int i = 0; i < m_objs.Count; i++)
            {
                m_objs[i].transform.Find("effect").gameObject.SetActive(false);
            }
                fanzhuan(3);
        }
        else if (obj.name == "continue")
        {

            fanzhuan(2);
            m_start.SetActive(false);
            m_button1.SetActive(false);
            m_button2.SetActive(false);

        }
        else if (obj.name == "finish")
        {
            m_start.SetActive(true);
            m_button1.SetActive(false);
            m_button2.SetActive(false);
            flag_id = false;
            sys._instance.m_self.m_t_player.huiyi_zhanpus.Clear();
            for (int i = 0; i < m_objs.Count; i++)
            {
                m_objs[i].transform.localRotation = Quaternion.identity;
                m_objs[i].GetComponent<UISprite>().spriteName = "huiyi_paibei";
                m_objs[i].transform.Find("effect").gameObject.SetActive(false);
                sys._instance.remove_child(m_objs[i].transform.Find("icon").gameObject);
            }

        }
        else if (obj.name == "luck_shop")
        {
            s_message mes = new s_message();
            mes.m_type = "show_huiyishop";
            cmessage_center._instance.add_message(mes);

        }
        else
        {
            if (!flag_id)
            {
                return;
            }
            flag = false;
            m_id = int.Parse(obj.name);
            protocol.game.cmsg_role_huiyi_fanpai _msg = new protocol.game.cmsg_role_huiyi_fanpai();
            _msg.id = sys._instance.m_self.m_t_player.huiyi_zhanpus[m_id];
            net_http._instance.send_msg<protocol.game.cmsg_role_huiyi_fanpai>(opclient_t.CMSG_ROLE_HUIYI_FANPAI, _msg);
        }
        
    }
    void change()
    {
        m_chang_flag++;
        if (m_chang_flag < 2)
        {
            return;
        }
        flag = false;
        if (m_change_num == 5)
        {
            for (int i = 0; i < m_objs.Count; i++)
            {
                if (m_objs[i].GetComponent<TweenPosition>() != null)
                {
                    Destroy(m_objs[i].GetComponent<TweenPosition>());
                  
                    if (Mathf.Abs(Mathf.Abs(m_objs[i].transform.localPosition.x) - 250) < 100)
                    {
                        if (m_objs[i].transform.localPosition.x > 0)
                        {
                            m_objs[i].transform.localPosition = new Vector3(250, 0, 0);
                        }
                        else
                        {
                            m_objs[i].transform.localPosition = new Vector3(-250, 0, 0);
 
                        }
                    }
                    else
                    {
                        m_objs[i].transform.localPosition = Vector3.zero;
                    }
                }
               
            }
            m_start.SetActive(false);
            m_button1.SetActive(false);
            m_button2.SetActive(false);
            set_flag();
            flag_id = true;
            m_change_num = 0;
            m_chang_flag = 2;
            return;
        }
        List<int> ids = new List<int>();
        ids.Add(0);
        ids.Add(1);
        ids.Add(2);
        int num = ids[Random.Range(0, ids.Count)];
        ids.Remove(num);
        int num1 = ids[Random.Range(0, ids.Count)];
        if (m_objs[num1].GetComponent<TweenPosition>() != null)
        {
            m_objs[num1].GetComponent<TweenPosition>().onFinished.Clear(); ;
        }
     //   print(m_objs[num].transform.localPosition);
     //   print(m_objs[num1].transform.localPosition);

        TweenPosition.Begin(m_objs[num], 0.5f, m_objs[num1].transform.localPosition).AddOnFinished(change);
        TweenPosition.Begin(m_objs[num1], 0.5f, m_objs[num].transform.localPosition).AddOnFinished(change);
        m_change_num++;
        m_chang_flag = 0;
 
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
 
    }
    void OnDisable()
    {
        flag = false;
        flag_id = false;
        for (int i = 0; i < m_objs.Count; i++)
        {
            Destroy(m_objs[i].GetComponent<TweenRotation>());
        }
        TweenAlpha _alp = this.GetComponent<TweenAlpha>();
        if (_alp != null)
        {
            Destroy(_alp);
        }
    }
}
