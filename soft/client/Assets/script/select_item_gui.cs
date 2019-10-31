
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class select_item_gui : MonoBehaviour,IMessage{

	public GameObject m_pl;
    public List<GameObject> m_icons;
    public int m_id;
	public int type = 0;
	public int m_total_num = 0;
    private int m_num;
	public GameObject m_back;
	public GameObject m_frame;
	public UILabel m_pl_Label;
	public GameObject m_ok;
    public List<s_t_reward> rewards = new List<s_t_reward>();
    public int value = 1;
    public UILabel m_input;
    public s_message m_mes;

	void Start () {
        
	
	}
    void IMessage.message(s_message mes)
    {

 
    }
    void IMessage.net_message(s_net_message mes)
    {
 
    }
    public void reset()
    {
		if(type == 0)
		{
			m_pl_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language ("select_item_gui.cs_38_45");//批量使用
        	m_num = sys._instance.m_self.get_item_num((uint)m_id);
        	m_input.text = value + "";
			m_pl.SetActive(true);
			m_back.GetComponent<UISprite>().height = 346;
			m_back.transform.localPosition = new Vector3(0,170,0);
			m_frame.GetComponent<UISprite>().height = 442;
			m_frame.transform.localPosition = new Vector3(0,-38,0);
			m_ok.transform.localPosition = new Vector3(0,-378,0);
		}
		else if(type == 2)
		{
			m_pl_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language ("select_item_gui.cs_50_45");//批量兑换
			m_num = m_total_num;
			m_input.text = value + "";
			m_pl.SetActive(true);
			m_back.GetComponent<UISprite>().height = 346;
			m_back.transform.localPosition = new Vector3(0,170,0);
			m_frame.GetComponent<UISprite>().height = 442;
			m_frame.transform.localPosition = new Vector3(0,-38,0);
			m_ok.transform.localPosition = new Vector3(0,-378,0);
		}
		else if(type == 1)
		{
			m_pl.SetActive(false);
			m_back.GetComponent<UISprite>().height = 271;
			m_back.transform.localPosition = new Vector3(0,135,0);
			m_frame.GetComponent<UISprite>().height = 371;
			m_frame.transform.localPosition = new Vector3(0,-36,0);
			m_ok.transform.localPosition = new Vector3(0,-303,0);
		}
        int i;
        for ( i = 0; i < rewards.Count && i <  m_icons.Count; i++)
        {
            Transform par = m_icons[i].transform.Find("icon");
            sys._instance.remove_child(par.gameObject);
            GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
            obj.transform.parent = par;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            UILabel m_name = m_icons[i].transform.Find("name").GetComponent<UILabel>();
			if(type == 0 || type == 2)
			{
            	m_name.text = sys._instance.m_self.get_name(rewards[i].type,rewards[i].value1,0,rewards[i].value3);
			}
			else if(type == 1)
			{
				m_name.text = sys._instance.m_self.get_name(rewards[i].type,rewards[i].value1,0,0);
			}
            m_icons[i].transform.Find("Toggle").gameObject.SetActive(true);
        }
        for (; i < m_icons.Count; i++)
        {
            m_icons[i].transform.Find("name").GetComponent<UILabel>().text = "";

            m_icons[i].transform.Find("Toggle").gameObject.SetActive(false);
 
        }
        
    }
	void Update () 
    {
	
	}
    void click(GameObject obj)
    {
        if (obj.name == "add")
        {
            if (m_num > 100)
            {
                if (value + 1 <= 100)
                {
                    value++;
                }
            }
            else
            {
                if (value + 1 <= m_num)
                {
                    value++;
                }
            }


        }
        else if (obj.name == "sub")
        {
            if (value - 1 > 0)
            {
                value--;
            }
 
        }
        else if(obj.name == "add10")
        {
            if (m_num > 100)
            {
                if (value + 10 <= 100)
                {
                    value += 10;
                }
                else
                {
                    value = 100;
                }
            }
            else
            {
                if (value + 10 <= m_num)
                {
                    value += 10;
                }
                else
                {
                    value = m_num;
                }
            }
            

        }
        else if (obj.name == "sub10")
        {
            if (value - 10 > 0)
            {
                value -= 10;
            }
            else
            {
                value = 1;
            }
 
        }
        else if(obj.name == "queding")
        {
            if (m_mes != null && type == 0)
            {
                for (int i = 0; i < rewards.Count; i++)
                {
                   
                    if (m_icons[i].transform.Find("Toggle").GetComponent<UIToggle>().value)
                    {
                        m_mes.m_ints.Add(i);
                    }
                }

                m_mes.m_ints.Add(value);
                this.transform.Find("frame_big").GetComponent<frame>().hide();
                cmessage_center._instance.add_message(m_mes);
            }
			else if(m_mes != null && type == 2)
			{
				for (int i = 0; i < rewards.Count; i++)
				{
					
					if (m_icons[i].transform.Find("Toggle").GetComponent<UIToggle>().value)
					{
						m_mes.m_ints.Add(i);
					}
				}
				
				m_mes.m_ints.Add(m_id);
				m_mes.m_ints.Add(value);
				this.transform.Find("frame_big").GetComponent<frame>().hide();
				cmessage_center._instance.add_message(m_mes);
			}
			else if(m_mes != null && type == 1)
			{
				for (int i = 0; i < rewards.Count; i++)
				{
					
					if (m_icons[i].transform.Find("Toggle").GetComponent<UIToggle>().value)
					{
						m_mes.m_ints.Add(i);
					}
				}
				
				m_mes.m_ints.Add(m_id);
				this.transform.Find("frame_big").GetComponent<frame>().hide();
				cmessage_center._instance.add_message(m_mes);
			}
        }
        reset();

    }
}
