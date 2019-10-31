
using UnityEngine;
using System.Collections;

public class select_cishu_box : MonoBehaviour,IMessage{
    public UILabel m_select_num;
    public UIToggle m_toggle;
    public UILabel m_boss_num;
    public UILabel m_name;
    public int m_input_num;
    public int m_item_num;
    public UILabel m_desc;
    public UILabel m_toggle_desc;
    public int m_type;
    public uint m_item_id;
    public UISprite m_icon;
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	
	}
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    void IMessage.message(s_message message)
    {
        if (message.m_type == "refresh_select_num")
        {
            refresh();
        }

    }
    void IMessage.net_message(s_net_message message)
    {
 
    }
    public void click(GameObject obj)
    {
        if (obj.name == "add")
        {
              m_input_num += 1;
              if (m_input_num > 100)
              {
                  m_input_num = 100;
              }
        }

        else if (obj.name == "sub")
        {
            if (m_input_num > 1)
            {
                m_input_num--;
            }

        }
        else if (obj.name == "add10")
        {
             m_input_num += 10;
             if (m_input_num > 100)
             {
                 m_input_num = 100;
             }
        }
        else if (obj.name == "sub10")
        {
            if (m_input_num - 10 >= 1)
            {
                m_input_num -= 10;
            }
            else
            {
                m_input_num = 1;
            }
        }
        else if (obj.name == "hide")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
           
        }
        else if (obj.name == "queding")
        {
            if (m_toggle.value)
            {
                if(m_type == 0)
                {
                    if (m_input_num <= sys._instance.m_self.m_t_player.boss_num + m_item_num)
                    {
                        s_message _mes = new s_message();
                        _mes.m_type = "yijian_mowang";
                        _mes.m_ints.Add(m_input_num);
                        _mes.m_bools.Add(m_toggle.value);
                        cmessage_center._instance.add_message(_mes);
                        this.gameObject.SetActive(false);

                    }
                    else
                    {
                        int num = sys._instance.m_self.get_item_num(m_item_id);
                        if (num > 0)
                        {
                            root_gui._instance.show_tili_dialog_box((int)m_item_id);
                            return;
                        }
                        else
                        {
                            s_message _message = new s_message();
                            _message.m_type = "buy_num_gui";
                            _message.m_ints.Add(100600);
                            _message.m_ints.Add(2);
                            cmessage_center._instance.add_message(_message);
                        }
                    }

                }
                else if (m_type == 1)
                {
                    if (m_input_num * 2 <= sys._instance.m_self.m_t_player.energy + m_item_num * 10)
                    {
                        s_message _mes = new s_message();
                        _mes.m_type = "yijian_arena";
                        _mes.m_ints.Add(m_input_num);
                        _mes.m_bools.Add(m_toggle.value);
                        cmessage_center._instance.add_message(_mes);
                        this.gameObject.SetActive(false);
                    }
                    else
                    {

                        if (m_item_num > 0)
                        {
                            root_gui._instance.show_tili_dialog_box((int)m_item_id);
                            return;
                        }
                        else
                        {
                            s_message _message = new s_message();
                            _message.m_type = "buy_num_gui";
                            _message.m_ints.Add(100300);
                            _message.m_ints.Add(2);
                            cmessage_center._instance.add_message(_message);
                            return;
                        }
                    }
 
                }
              
            }
            else
            {
                if (m_type == 0)
                {
                    if (m_input_num <= sys._instance.m_self.m_t_player.boss_num)
                    {
                        s_message _mes = new s_message();
                        _mes.m_type = "yijian_mowang";
                        _mes.m_ints.Add(m_input_num);
                        _mes.m_bools.Add(m_toggle.value);
                        cmessage_center._instance.add_message(_mes);
                        this.gameObject.SetActive(false);

                    }
                    else
                    {
                        int item_id = 10010009;
                        int num = sys._instance.m_self.get_item_num((uint)item_id);
                        if (num > 0)
                        {
                            root_gui._instance.show_tili_dialog_box(item_id);
                            return;
                        }
                        else
                        {
                            s_message _message = new s_message();
                            _message.m_type = "buy_num_gui";
                            _message.m_ints.Add(100600);
                            _message.m_ints.Add(2);
                            cmessage_center._instance.add_message(_message);
                        }
                    }
 
                }
                else if (m_type == 1)
                {
                    if (m_input_num * 2 <= sys._instance.m_self.m_t_player.energy)
                    {
                        s_message _mes = new s_message();
                        _mes.m_type = "yijian_arena";
                        _mes.m_ints.Add(m_input_num);
                        _mes.m_bools.Add(m_toggle.value);
                        cmessage_center._instance.add_message(_mes);
                        this.gameObject.SetActive(false);

                    }
                    else
                    {

                        if (m_item_num > 0)
                        {
                            root_gui._instance.show_tili_dialog_box((int)m_item_id);
                            return;
                        }
                        else
                        {
                            s_message _message = new s_message();
                            _message.m_type = "buy_num_gui";
                            _message.m_ints.Add(100300);
                            _message.m_ints.Add(2);
                            cmessage_center._instance.add_message(_message);
                            return;
                        }
                    }
 
 
                }
               
            }
        }
        refresh();

    }
    public void init(int type)
    {
        
     
        m_input_num = 1;
        m_type = type;
        refresh();
    }
    void refresh()
    {
        if (m_type == 0)
        {
            m_item_id = 10010009;
            m_desc.text = game_data._instance.get_t_language ("select_cishu_box.cs_234_26");//当前讨伐次数
            m_toggle_desc.text = game_data._instance.get_t_language ("select_cishu_box.cs_235_33");//自动消耗魔王邀请函补充次数
            m_boss_num.text = sys._instance.m_self.m_t_player.boss_num + "";
            if (sys._instance.m_self.m_t_player.boss_num >= m_input_num)
            {
                m_select_num.text = m_input_num + "";
            }
            else
            {
                m_select_num.text = "[ff0000]" + m_input_num;
            }
            m_name.text = game_data._instance.get_t_language ("select_cishu_box.cs_245_26");//设置挑战次数
            m_icon.spriteName = "mwyqkx_001";
        }
        else if(m_type == 1)
        {
            m_name.text = game_data._instance.get_t_language ("select_cishu_box.cs_245_26");//设置挑战次数
            m_desc.text = game_data._instance.get_t_language ("select_cishu_box.cs_251_26");//需要消耗能量
            m_toggle_desc.text = game_data._instance.get_t_language ("select_cishu_box.cs_252_33");//自动消耗能量药水补充能量
            m_item_id = 10010007;
            m_boss_num.text = m_input_num * 2 + "";
            if (sys._instance.m_self.m_t_player.energy >= m_input_num * 2)
            {
                m_select_num.text = m_input_num + "";
            }
            else
            {
                m_select_num.text = "[ff0000]" + m_input_num;
            }

            m_icon.spriteName = game_data._instance.get_t_resource(15).smallicon;
        }
       
        m_item_num = sys._instance.m_self.get_item_num(m_item_id);
        
    
        
 
    }
	
}
