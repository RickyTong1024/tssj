using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class czlb_sub : MonoBehaviour {

    public int main_id;
    public int m_id;
    public int m_can_get = 0;
    public int m_limit_num = 0;
    public int m_rid = 0;
    public GameObject m_get;
    public GameObject m_ylq;
    public GameObject m_desc;
    public GameObject m_num;
    public GameObject m_view;
    public GameObject m_scro;
    public List<s_t_reward> rewards = new List<s_t_reward>();
    public UILabel m_vip_exp;
    public UILabel m_price;
    int m_click_id = 0;
    string order = "";
    // Use this for initialization
    void Start()
    {

    }

    string changeName(int rid,int ios_id)
    {
        s_t_recharge t_recharge_id = game_data._instance.get_t_recharge(rid);
        if (game_data._instance.m_id_price != "")
        {
            string[] m_skus = game_data._instance.m_id_price.Split('?');
            if (m_skus.Length > 0)
            {
                for (int i = 0; i < m_skus.Length; i++)
                {
                    if (m_skus[i].Split('_')[0].Equals(ios_id.ToString()))
                    {
                        return m_skus[i].Split('_')[1];
                    }
                }
            }
            else
            {
                return t_recharge_id.rmb;
            }

        }
        return t_recharge_id.rmb;
    }

    public void reset()
    {
        m_num.GetComponent<UILabel>().text = (m_can_get - m_limit_num).ToString();

        for (int i = 0; i < rewards.Count; ++i)
        {
            GameObject _obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
            
            _obj.AddComponent<UIDragScrollView>().scrollView = m_view.GetComponent<UIScrollView>();
            
            _obj.transform.parent = m_view.transform;
            _obj.transform.localScale = new Vector3(1, 1, 1);
            _obj.transform.localPosition = new Vector3(-225 + i * 99, -17, 0);
            _obj.AddComponent<UIDragScrollView>().scrollView = m_scro.GetComponent<UIScrollView>();
        }
        s_t_recharge t_recharge_id = game_data._instance.get_t_recharge(m_rid);
        m_price.text = changeName(m_rid, t_recharge_id.ios_id);
        if ((m_can_get - m_limit_num) == 0)
        {
            m_get.SetActive(true);
            m_get.GetComponent<BoxCollider>().enabled = false;
            m_get.GetComponent<UISprite>().set_enable(false);
        }
        else
        {
            m_get.SetActive(true);
            m_get.GetComponent<BoxCollider>().enabled = true;
            m_get.GetComponent<UISprite>().set_enable(true);
        }
        
    }

    public void click(GameObject obj)
    {
		m_click_id = m_rid;
		s_t_recharge t_recharge1 = game_data._instance.get_t_recharge(m_click_id);
		if(t_recharge1 == null)
		{
			return;
		}
        platform_recharge._instance.do_buy(m_click_id, main_id, m_id);
    }
}