
using UnityEngine;
using System.Collections;

public class honor_shop_card : MonoBehaviour {

	public GameObject m_honor_shop_gui;
	public GameObject m_name;
	public GameObject m_gold;
	public GameObject m_icon;
	public GameObject m_effect;

	public int m_honor_shop_id;
	public int m_item_num;
	
	private s_t_sport_shop m_t_sport_shop;
	private s_t_item m_t_item;
	private string m_ds;

	public UILabel m_goumai;
	// Use this for initialization
	void Start () 
	{

	
	}

	public void click()
	{
		if (m_ds != "")
		{
			if (m_ds == "0")
			{
				root_gui._instance.show_recharge_dialog_box(delegate() {
					m_honor_shop_gui.GetComponent<ui_title_anim>().hide_ui();
				});
			}
			else
			{
				root_gui._instance.show_prompt_dialog_box(m_ds);
			}
			return;
		}
		
		s_message _msg = new s_message ();
		
		_msg.m_type = "buy_honor_item";
		_msg.m_ints.Add (m_honor_shop_id);
		
		cmessage_center._instance.add_message (_msg);
	}

	public void updata_ui()
	{
		m_t_sport_shop = game_data._instance.get_t_sport_shop (m_honor_shop_id);
		
		GameObject _icon = icon_manager._instance.create_reward_icon(m_t_sport_shop.type,m_t_sport_shop.value1,m_t_sport_shop.value2,m_t_sport_shop.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);

		m_ds = "";
		string s = "";
		if ( sys._instance.m_self.get_att(e_player_attr.player_treasure_powder) < m_t_sport_shop.price)
		{
			s = "[ff0000]";
			m_ds = "[ffc882]"+game_data._instance.get_t_language ("equip_buy_gui.cs_1101_75");//荣誉不足
		}
		
		m_gold.GetComponent<UILabel>().text = s + m_t_sport_shop.price.ToString();
		m_name.GetComponent<UILabel>().text = sys._instance.get_res_info (m_t_sport_shop.type,m_t_sport_shop.value1,m_t_sport_shop.value2,m_t_sport_shop.value3);
		
		this.transform.Find("kuang").gameObject.SetActive (false);
		m_effect.SetActive(false);
		if(m_t_sport_shop.value1 == 30010401 && m_t_sport_shop.type == 2)
		{
			m_effect.SetActive(true);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
