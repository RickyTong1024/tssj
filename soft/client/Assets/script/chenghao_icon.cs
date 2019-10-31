
using UnityEngine;
using System.Collections;

public class chenghao_icon : MonoBehaviour {

    public int m_chenhao_id;
    private s_t_chenghao m_t_chenhao;
    public UISprite m_bg;
    public string m_out_message;
    public GameObject m_main;
    public UILabel m_name;
    public UILabel m_desc;
    public UISprite m_icon;
    public void click()
    {
        if (m_out_message.Length == 0)
        {
            s_message _message = new s_message();

            _message.m_type = "chenhao_dialog_box";
            _message.m_ints.Add(m_t_chenhao.id);
            _message.m_ints.Add(2);
            cmessage_center._instance.add_message(_message);
            return;
        }
        s_message m_message = new s_message();
        m_message.m_type = m_out_message;
        m_message.m_ints.Add(m_chenhao_id);
        m_message.m_object.Add(gameObject);
        m_message.m_string.Add(m_t_chenhao.name);
        cmessage_center._instance.add_message(m_message);

    }

    

    public void reset(int id,int type = 0)
    {
        m_chenhao_id = id;
        m_t_chenhao = game_data._instance.get_t_chenhao(m_chenhao_id);
        if (m_chenhao_id == 0)
        {
            return;
        }
        if(type == 1)
        {
            m_bg.gameObject.SetActive(true);
            m_main.SetActive(false);
            m_main.GetComponent<UISprite>().spriteName = "icon_chjl";
            int color =game_data._instance.get_color_index(m_t_chenhao.color);
            string s = "";
            if (color == 0)
			{
				s = "xtbk_fupt001";
			}
			else if (color == 1)
			{
				s = "xtbk_lvpt001";
			}
			else if (color == 2)
			{
				s = "xtbk_lanpt001";
			}
			else if (color == 3)
			{
				s = "xtbk_zipt001";
			}
			else if (color == 4)
			{
				s = "xtbk_chpt001";
			}
			else if (color == 5)
			{
				s = "xtbk_hopt001";
			}
			else if (color == 6)
			{
				s = "xtbk_jinpt001";
			}
		
		    m_bg.spriteName = s;
        }
        else if(type == 0)
        {
            m_bg.gameObject.SetActive(false);
            m_main.SetActive(true);
            sys._instance.get_chenghao(m_chenhao_id,m_main);
            m_desc.text = m_t_chenhao.condition;
            sys._instance.get_chenghao(m_chenhao_id, m_main);
    
           
            
        }
    }

       
}
