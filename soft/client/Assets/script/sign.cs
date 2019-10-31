
using UnityEngine;
using System.Collections;

public class sign : MonoBehaviour {

	public int m_index;
	public GameObject m_icon;
	public GameObject m_vip;
	public GameObject m_scro;
	GameObject m_icon1;
	// Use this for initialization
	void Start () {
		init ();
	}

	public void init()
	{
		int index1 = sys._instance.m_self.m_t_player.daily_sign_reward;
		int index2 = sys._instance.m_self.m_t_player.daily_sign_index;
		this.transform.Find("main1").gameObject.SetActive(false);
        this.transform.Find("xuan").gameObject.SetActive(true);
        this.transform.Find("gou").gameObject.SetActive(true);
        if (index2 % 30 != 0)
        {
            if (m_index + 1 <= index1 % 30)
            {

                this.transform.Find("main1").gameObject.SetActive(true);
            }
            else
            {

                this.transform.Find("gou").gameObject.SetActive(false);
            }
 
        }
        else if (index2 % 30 == 0)
        {
            int value = 0;
            if (index1 != index2)
            {
                value = 29;
            }
            else
            {
                value = 30;
 
            }
            if (m_index + 1 <= value)
            {
                this.transform.Find("main1").gameObject.SetActive(true);
               
            }
            else
            {
                this.transform.Find("gou").gameObject.SetActive(false);
 
            }
        }
		s_t_daily_sign t_daily_sign = game_data._instance.get_t_daily_sign (m_index + 1);
		Transform picon = m_icon.transform;
        if (index1 < 30)
        {
            m_icon1 = icon_manager._instance.create_reward_icon_ex(t_daily_sign.type, t_daily_sign.value1, t_daily_sign.value2, t_daily_sign.value3);
            if (t_daily_sign.vip == 0)
            {
                m_vip.transform.gameObject.SetActive(false);
            }
            else
            {
                m_vip.transform.GetComponent<UISprite>().spriteName = "sb_n0" + t_daily_sign.vip.ToString();
            }
        }
        else
        {
            m_icon1 = icon_manager._instance.create_reward_icon_ex(t_daily_sign.type1, t_daily_sign.value11, t_daily_sign.value21, t_daily_sign.value31);
            if (t_daily_sign.vip1 == 0)
            {
                m_vip.transform.gameObject.SetActive(false);
            }
            else
            {
                m_vip.transform.GetComponent<UISprite>().spriteName = "sb_n0" + t_daily_sign.vip1.ToString();
            }
        }
		
		m_icon1.transform.parent = picon;
		m_icon1.transform.localPosition = new Vector3(0,0,0);
		m_icon1.transform.localScale = new Vector3(1,1,1);
		m_icon1.transform.GetComponent<BoxCollider>().enabled = true;
		m_icon1.AddComponent<UIDragScrollView>().scrollView = m_scro.GetComponent<UIScrollView>();
		if(sys._instance.m_self.m_t_player.daily_sign_reward != sys._instance.m_self.m_t_player.daily_sign_index
		   && sys._instance.m_self.m_t_player.daily_sign_reward % 30 == m_index)
		{
			UIButtonMessage[] meses = m_icon1.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
		}
        if (index2 % 30 != 0)
        {
            if (index2 % 30 != m_index + 1)
            {
                this.transform.Find("xuan").gameObject.SetActive(false);
            }
 
        }
        else if (index2 % 30 == 0)
        {
            if (m_index != 29)
            {
                this.transform.Find("xuan").gameObject.SetActive(false);
 
            }
        }
       
		
	}

	public void click(GameObject obj)
	{
        if (sys._instance.m_self.m_t_player.daily_sign_reward != sys._instance.m_self.m_t_player.daily_sign_index
            && sys._instance.m_self.m_t_player.daily_sign_reward % 30 == m_index)
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_DAILY_SIGN, _msg);
		}
	}

	/*public void press(GameObject obj)
	{
		if (sys._instance.m_self.m_t_player.daily_sign_reward != sys._instance.m_self.m_t_player.daily_sign_index
		    && sys._instance.m_self.m_t_player.daily_sign_reward % 30 == m_index)
		{
			return;
		}
		s_t_daily_sign t_daily_sign = game_data._instance.get_t_daily_sign (m_index + 1);
		if (t_daily_sign.type == 1)
		{
			m_icon1.transform.GetComponent<resource_icon>().press();
		}
		else if (t_daily_sign.type == 2)
		{
			m_icon1.transform.GetComponent<item_icon>().press();
		}
		else if (t_daily_sign.type == 3)
		{
            if (sys._instance.m_self.m_t_player.daily_sign_index > 30)
            {
                m_icon1.transform.GetComponent<item_icon>().press();

            }
            else
            {
				m_icon1.transform.GetComponent<card_icon>().press();
 
            }
			
		}
		else if (t_daily_sign.type == 4)
		{
			m_icon1.transform.GetComponent<equip_icon>().press();
		}
		else if (t_daily_sign.type == 5)
		{
			m_icon1.transform.GetComponent<dressrole_icon>().press();
		}
	}

	public void release(GameObject obj)
	{
		s_t_daily_sign t_daily_sign = game_data._instance.get_t_daily_sign (m_index + 1);
		if (t_daily_sign.type == 1)
		{
			m_icon1.transform.GetComponent<resource_icon>().release();
		}
		else if (t_daily_sign.type == 2)
		{
			m_icon1.transform.GetComponent<item_icon>().release();
		}
		else if (t_daily_sign.type == 3)
		{
            if (sys._instance.m_self.m_t_player.daily_sign_index > 30)
            {
                m_icon1.transform.GetComponent<item_icon>().release();

            }
            else
            {
                m_icon1.transform.GetComponent<card_icon>().release();

            }
		}
		else if (t_daily_sign.type == 4)
		{
			m_icon1.transform.GetComponent<equip_icon>().release();
		}
		else if (t_daily_sign.type == 5)
		{
			m_icon1.transform.GetComponent<dressrole_icon>().release();
		}
	}*/
	
	// Update is called once per frame
	void Update () {
	
	}
}
