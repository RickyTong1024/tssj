
using UnityEngine;
using System.Collections;

public class recharge : MonoBehaviour {

	public int m_recharge_id;
	public GameObject m_icon;
	public UISprite m_kuang;

	void Start () {
		init ();
	}
    string changeName(int ios_id)
    {
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
                return "";
            }
            
        }
        return "";
    }
	public void init()
	{
		s_t_recharge t_recharge = game_data._instance.get_t_recharge(m_recharge_id);
        
		transform.Find("name").GetComponent<UILabel>().text = t_recharge.name;
		transform.Find("desc").GetComponent<UILabel>().text = t_recharge.desc;
        transform.Find("money").GetComponent<UILabel>().text = changeName(t_recharge.ios_id).Equals("") ? t_recharge.rmb : changeName(t_recharge.ios_id);
		m_icon.transform.GetComponent<UISprite>().spriteName = t_recharge.icon;
		if(m_recharge_id == 20)
		{
			m_kuang.spriteName = "xtbk_jinpt001";
		}
		else if(m_recharge_id == 25)
		{
			m_kuang.spriteName = "xtbk_hopt001";
		}
		else
		{
			m_kuang.spriteName = "xtbk_lanpt001";
		}
	}
}
