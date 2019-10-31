
using UnityEngine;
using System.Collections;

public class item_shop_card : MonoBehaviour {

	public GameObject m_item_shop_gui;
	public GameObject m_image;
	public GameObject m_name;
	public GameObject m_gold;
	public GameObject m_icon;
	public GameObject m_money_image;

	public int m_shop_id;
	public int m_item_num;
	public int m_shell = 0;

	private s_t_role_shop m_t_shop;
	private s_t_item m_t_item;
	private string m_ds;
	public UILabel m_goumai;

	// Use this for initialization
	void Start () 
	{

	}

	public void click(GameObject obj)
	{
		if(m_shell == 1)
		{
			root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("item_shop_card.cs_32_58"));//此商品已售罄
			return;
		}
		if (m_ds != "")
		{
			if (m_ds == "0")
			{
				root_gui._instance.show_recharge_dialog_box(delegate() {
					m_item_shop_gui.transform.Find("frame_big").GetComponent<frame>().hide();
				});
			}
			else
			{
				root_gui._instance.show_prompt_dialog_box(m_ds);
			}
			return;
		}
		int id = int.Parse (obj.transform.parent.name);
		s_message _msg = new s_message ();

		_msg.m_type = "buy_item";
		_msg.m_ints.Add (m_shop_id);
		_msg.m_ints.Add (id);
		cmessage_center._instance.add_message (_msg);
	}
	public void updata_ui()
	{

		m_t_shop = game_data._instance.get_role_shop (m_shop_id);

		GameObject _icon = icon_manager._instance.create_reward_icon(m_t_shop.sell_type,m_t_shop.sell_value_0,m_t_shop.sell_value_1,m_t_shop.sell_value_2);

		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);

		m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(m_t_shop.money_type);
		m_ds = "";
		string s = "";
		if (m_t_shop.money_type == 1 && sys._instance.m_self.get_att(e_player_attr.player_gold) < m_t_shop.price)
		{
			s = "[ff0000]";
			m_ds = "[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
		}
		else if (m_t_shop.money_type == 2 && sys._instance.m_self.get_att(e_player_attr.player_jewel) < m_t_shop.price)
		{
			s = "[ff0000]";
			m_ds = "0";
		}
		else if (m_t_shop.money_type == 5 && sys._instance.m_self.get_att(e_player_attr.player_jjc_point) < m_t_shop.price)
		{
			s = "[ff0000]";
			m_ds = "[ffc882]" +  game_data._instance.get_t_language ("item_shop_card.cs_84_24");//战魂不足
		}
        else if (m_t_shop.money_type == 6 && sys._instance.m_self.get_att(e_player_attr.player_hj) < m_t_shop.price)
		{
			s = "[ff0000]";
			m_ds = "[ffc882]" +  game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75");//合金不足
		}

		m_gold.GetComponent<UILabel>().text = s + m_t_shop.price.ToString();
		m_name.GetComponent<UILabel>().text = sys._instance.get_res_info (m_t_shop.sell_type,m_t_shop.sell_value_0,m_t_shop.sell_value_1,m_t_shop.sell_value_2);

		this.transform.Find("kuang").gameObject.SetActive (false);
		if(m_shell == 1)
		{
			this.transform.Find("kuang").gameObject.SetActive (true);
		}

	}
	// Update is called once per frame
	void Update () {
	
	}
}
