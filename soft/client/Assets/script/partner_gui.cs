
using UnityEngine;
using System.Collections;

public class partner_gui : MonoBehaviour,IMessage {

	public GameObject m_card_page_gui;
	public GameObject m_sui_pian_panel;
	public GameObject m_ji_yin_panel;
	public GameObject m_haogan_gui;
	public GameObject m_effect1;
	public GameObject m_effect2;
	public GameObject m_effect3;
	public string m_main_button;

	public UILabel m_ka_pai_Label1;
	public UILabel m_ka_pai_Label2;
	public UILabel m_ji_yin_Label1;
	public UILabel m_ji_yin_Label2;
	public UILabel m_shui_pian_Label1;
	public UILabel m_shui_pian_Label2;
	public UILabel m_haogan_Label1;
	public UILabel m_haogan_Label2;
	public UILabel m_tishi_Label;
	public UILabel m_fenjie_Label;
	private int m_select = 9;
	// Use this for initialization
	void Start () {
	
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void OnEnable()
	{

	}

	public void init()
	{
		m_card_page_gui = game_data._instance.ins_object_res("ui/card_page_gui");
		m_card_page_gui.transform.parent = this.transform;
		m_card_page_gui.transform.localPosition = new Vector3(0,0,0);
		m_card_page_gui.transform.localScale = new Vector3(1,1,1);
		m_card_page_gui.SetActive (true);

		m_sui_pian_panel.GetComponent<sui_pian_panel>().m_card_page_gui = m_card_page_gui;
		m_ji_yin_panel.GetComponent<ji_yin_panel>().m_card_page_gui = m_card_page_gui;
	}

	public void reset()
	{
		update_button ();
		kapai ();
	}

	public void check(ulong guid)
	{
		m_card_page_gui.GetComponent<card_page_gui>().check (guid);
	}

	void hide_page()
	{
		s_message _message = new s_message ();
		_message.m_type = "hide_show_unit";
		cmessage_center._instance.add_message (_message);

		Transform _obj = this.transform.Find("main_button");

		_obj.Find("ka_pai").GetComponent<UIToggle>().value = true;

		m_card_page_gui.SetActive (false);
		m_haogan_gui.SetActive (false);
		m_sui_pian_panel.SetActive(false);
		m_ji_yin_panel.SetActive(false);
	}

	public void close()
	{
		hide_page();
		this.GetComponent<ui_show_anim>().hide_ui();

		s_message _message = new s_message ();
		_message.m_type = "hide_show_unit";
		cmessage_center._instance.add_message (_message);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "hide_partner")
		{
			close ();

			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);

			Object.Destroy(this.gameObject);
		}

		if(obj.transform.name == "ka_pai")
		{
			kapai();
		}
		if(obj.transform.name == "ji_yin")
		{
			jiyin();
		}
		if(obj.transform.name == "shui_pian")
		{
			sp();
		}
	}

	void IMessage.net_message(s_net_message message)
	{

	}
	
	void IMessage.message(s_message message)
	{
		if(this.gameObject.activeSelf == false)
		{
			return;
		}

		if(message.m_type == "click_card")
		{
			if(m_select == 0)
			{
				ulong _gui = (ulong)message.m_long[0];

				s_message _message = new s_message();
				_message.m_type = "show_unit";
				_message.m_long.Add ((ulong)_gui);
				_message.m_ints.Add ((int)1);
				_message.m_string.Add("show_huoban3");

				cmessage_center._instance.add_message(_message);

				this.GetComponent<ui_show_anim>().hide_ui();

				ccard card = sys._instance.m_self.get_card_guid(_gui);
				if (card.m_t_class.sound != "0")
				{
					sys._instance.play_sound_ex("sound/ts_rwpy/" + card.m_t_class.sound);
					s_message mes = new s_message();
					mes.m_type = "action";
					mes.m_string.Add("win");
					mes.time = 0.5f;
					cmessage_center._instance.add_message(mes);
				}
			}
		}
	}
	
	void sp()
	{
		hide_page ();
		m_select = 2;
		m_sui_pian_panel.SetActive(true);
		m_sui_pian_panel.GetComponent<sui_pian_panel>().update_ui();
		m_card_page_gui.SetActive (true);
	}
	void kapai()
	{
		hide_page ();
		m_select = 0;
		
		m_card_page_gui.SetActive(true);
		m_card_page_gui.GetComponent<card_page_gui>().init();
		m_card_page_gui.GetComponent<card_page_gui>().set_text("");
		m_card_page_gui.GetComponent<card_page_gui>().m_out_message = "click_card";
		m_card_page_gui.GetComponent<card_page_gui>().card_reset();
	}
	void jiyin()
	{
		hide_page ();
		m_select = 1;
		m_ji_yin_panel.SetActive(true);
		m_ji_yin_panel.GetComponent<ji_yin_panel>().update_ui();
		m_card_page_gui.SetActive (true);
	}

	
	public void update_button()
	{
		if (ji_yin_panel.can_duihuan())
		{
			m_effect1.gameObject.SetActive(true);
		}
		else
		{
			m_effect1.gameObject.SetActive(false);
		}

		if (sui_pian_panel.can_hecheng())
		{
			m_effect2.gameObject.SetActive(true);
		}
		else
		{
			m_effect2.gameObject.SetActive(false);
		}
						
	}

	public static bool can_effect()
	{
		if (ji_yin_panel.can_duihuan())
		{
			return true;
		}
		if (sui_pian_panel.can_hecheng())
		{
			return true;
		}

		return false;
	}

	// Update is called once per frame
	void Update () {
	
		if(m_main_button.Length > 0)
		{
			Transform _root = this.transform.Find("main_button");
			
			for(int i = 0;i < _root.childCount;i ++)
			{
				UIToggle _toggle = _root.GetChild(i).GetComponent<UIToggle>();
				
				if(_toggle != null)
				{
					if(_toggle.transform.name == m_main_button)
					{
						_toggle.value = true;
					}
					else
					{
						_toggle.value = false;
					}
				}
			}

			m_main_button = "";
		}
	}
}
