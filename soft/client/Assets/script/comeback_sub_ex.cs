
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class comeback_sub_ex : MonoBehaviour {

	public int m_id;
	public int m_num;
	public GameObject m_get;//领取按钮
//	public GameObject m_recharge;
//	public GameObject m_ylq;
//	public GameObject m_name;//商品名称
//	public GameObject m_main;
//	public GameObject m_main1;
//	public UILabel m_lq;
//	public UILabel m_cz;
	public GameObject m_view;
	
//	public GameObject m_pirce;//商品价格
	public GameObject m_icons;//砖石图标
	public GameObject m_discount;// 折扣
	public GameObject m_xiangou; //限购
	private s_t_comeback t_comeback;
	public List<s_t_reward> rewards = new List<s_t_reward>();
	
	void Start () {
	}
	
	public void reset()
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		for (int i = 0; i < rewards.Count; ++i)
		{
			GameObject _obj = icon_manager._instance.create_reward_icon(rewards[i].type ,rewards[i].value1 ,rewards[i].value2 ,rewards[i].value3);
			if(rewards.Count > 3)
			{
				_obj.AddComponent<UIDragScrollView>().scrollView = m_view.GetComponent<UIScrollView>();
			}
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(-106 + i * 99,0,0);
//			_obj.AddComponent<UIDragScrollView>().scrollView = m_scro.GetComponent<UIScrollView>(); 
		}

//		m_ylq.SetActive(false);
		m_get.SetActive(true);
		m_get.GetComponent<UISprite>().set_enable (false);
		m_get.GetComponent<BoxCollider>().enabled = false;
//		m_recharge.SetActive(false);
//		m_main.SetActive(true);
//		m_main1.SetActive(false);
		t_comeback = game_data._instance.get_t_comeback (m_id);
		
//		m_name.GetComponent<UILabel>().text = t_comeback.desc;
//		m_pirce.GetComponent<UILabel>().text = string.Format("{0}{1}","[2FD4FF]",t_comeback.def1.ToString());// def1 1 价格 2折扣 3限购数量 4 等级达到才能显示
		m_xiangou.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("comeback_sub_ex.cs_61_59"),t_comeback.def3-m_num,t_comeback.def3 );//可兑换次数：{0}/{1}
		if (t_comeback.def2 < 100) {
			m_discount.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"),sys._instance.discountChange(((float)t_comeback.def2 / 10)));//{0}折
		} 
		else 
		{
			m_discount.SetActive(false);
		}

		GameObject _obj1 = icon_manager._instance.create_reward_icon(1,2,t_comeback.def1,0);

		_obj1.transform.parent = m_icons.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);

		if (m_num >= t_comeback.def3 || sys._instance.m_self.m_t_player.jewel < t_comeback.def1) {
			m_get.GetComponent<UISprite>().set_enable (false);
			
			m_get.GetComponent<BoxCollider>().enabled = false;
				
		}else
		{
			m_get.GetComponent<UISprite>().set_enable (true);
			
			m_get.GetComponent<BoxCollider>().enabled = true;

		}

	}
	
	public void click(GameObject obj)
	{
		if (obj.name == "get_fuli")
		{
			s_message _message = new s_message();
			_message.m_type = "show_buy_mun_gui";
			_message.m_ints.Add(t_comeback.id);
			_message.m_ints.Add(t_comeback.type);
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.name == "recharge")
		{

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
