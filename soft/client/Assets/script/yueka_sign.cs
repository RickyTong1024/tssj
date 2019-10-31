
using UnityEngine;
using System.Collections;

public class yueka_sign : MonoBehaviour {

	public int m_index;
	public GameObject m_icon;
	public int m_dang;  //d代表物品属于几挡奖励
	//public GameObject m_vip;
	public GameObject m_scro;
	GameObject m_icon1;
	public int stat_this;
	// Use this for initialization
	void Start () {

	}
	
	public void init(int id,int stat,int curr)
	{
//		int index1 = sys._instance.m_self.m_t_player.daily_sign_reward;
//		int index2 = sys._instance.m_self.m_t_player.daily_sign_index;
		stat_this = stat;
		if (stat == 3) {
			this.transform.Find("mask").gameObject.SetActive (false);
			this.transform.Find("can").gameObject.SetActive (false);
			this.transform.Find("que").gameObject.SetActive (false);
		}
		this.transform.Find("day").gameObject.GetComponent<UILabel>().text =string.Format( game_data._instance.get_t_language ("yueka_sign.cs_28_92"),(m_index + 1));//第{0}天
		if (id == 1) {
			s_t_yueka t_yueka_sign = game_data._instance.get_t_yueka_jijin (m_index + 1);
			m_icon1 = icon_manager._instance.create_reward_icon_ex (t_yueka_sign.type1, t_yueka_sign.value_1_1, t_yueka_sign.value_1_2, t_yueka_sign.value_1_3);
			m_dang =id;
		} else if (id == 2) {

			s_t_yueka t_yueka_sign = game_data._instance.get_t_yueka_jijin (m_index + 1);
			m_icon1 = icon_manager._instance.create_reward_icon_ex (t_yueka_sign.type2, t_yueka_sign.value_2_1, t_yueka_sign.value_2_2, t_yueka_sign.value_2_3);
			m_dang =id;

		}

		if (curr ==(m_index + 1) ) {
			this.transform.Find("can").gameObject.SetActive(true);
			
		}

		if (stat == 0) {
			this.transform.Find("mask").gameObject.SetActive(true);
			this.transform.Find("que").gameObject.SetActive(true);
//			this.transform.Find("can").gameObject.SetActive(true);
			
		}

		if ( stat == 1)
		{
			UIButtonMessage[] meses = m_icon1.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
		}
		Transform picon = m_icon.transform;

		m_icon1.transform.parent = picon;
		m_icon1.transform.localPosition = new Vector3(0,0,0);
		m_icon1.transform.localScale = new Vector3(1,1,1);
		m_icon1.transform.GetComponent<BoxCollider>().enabled = true;
		m_icon1.AddComponent<UIDragScrollView>().scrollView = m_scro.GetComponent<UIScrollView>();
			
		
		
	}
	
	public void click(GameObject obj)
	{
		if (stat_this ==1)
		{
			this.transform.parent.parent.parent.parent.GetComponent<yueka_jijin>().m_curdang = m_dang;
			this.transform.parent.parent.parent.parent.GetComponent<yueka_jijin>().m_day_sign = (m_index+1);
			protocol.game.cmsg_huodong_yueka_reward _msg =new protocol.game.cmsg_huodong_yueka_reward();
			_msg.day = (m_index + 1);
			_msg.level = m_dang;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_yueka_reward>(opclient_t.CMSG_HUODONG_YUEKA_REWARD,_msg);

		}
	}
	void Update () {
	
	}
}
