
using UnityEngine;
using System.Collections;
using System;
public class yueka_jijin_kaiqi : MonoBehaviour,IMessage {

	public GameObject m_residue_time; //剩余时间
	public GameObject m_kaiqi_time;   //基金开启时间
	public GameObject m_yulan_panel; //预览面板
	public GameObject m_ishave_98;
	public GameObject m_ishave_328;
	private bool m_show_time =false;
	private protocol.game.smsg_huodong_yueka_view _msg1;
	// Use this for initialization

	void Awake () {
		cmessage_center._instance.add_handle (this);	
		
	}
    void OnEnable()
    {
        protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
        net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG_YUEKA_VIEW, _msg);
    }
    public void init(bool resert)
    {
        if (resert || this.gameObject.activeSelf)
        {
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG_YUEKA_VIEW, _msg);
        }
 
    }
	void OnDestroy()
	{
		CancelInvoke ("time");
		cmessage_center._instance.remove_handle (this);
	}
	void IMessage.message (s_message message)
	{
		if (message.m_type == "resert_yuekakaiqi_gui") {
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_YUEKA_VIEW, _msg);
		}
	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_YUEKA_VIEW) {

			_msg1 =net_http._instance.parse_packet<protocol.game.smsg_huodong_yueka_view>(message.m_byte);

			if (_msg1.level == 1) {
				reset (1);
			} else if (_msg1.level == 2) {
				reset (2);
			} else if (_msg1.level == 3) {
				reset (3);
			} else {
				reset(0);
			}

			m_show_time = true;
			InvokeRepeating ("time", 0.0f, 1.0f);
			
		}
	}
	public void reset(int id){

		if (id == 1) {
			m_ishave_98.GetComponent<UISprite>().set_enable (false);
            m_ishave_98.transform.Find("Label").GetComponent<UILabel>().text = "[FFAF49]" + game_data._instance.get_t_language("temaihui_card.cs_1246_263"); //解锁
            m_ishave_328.transform.Find("Label").GetComponent<UILabel>().text = "[E649FF]" + game_data._instance.get_t_language("hb_dress_page_item.cs_188_58"); //
		} else if (id == 2) {
			m_ishave_328.GetComponent<UISprite>().set_enable (false);
            m_ishave_328.transform.Find("Label").GetComponent<UILabel>().text = "[FFAF49]" + game_data._instance.get_t_language("temaihui_card.cs_1246_263"); //解锁
            m_ishave_98.transform.Find("Label").GetComponent<UILabel>().text = "[E649FF]" + game_data._instance.get_t_language("hb_dress_page_item.cs_188_58"); //w解锁

		} else if (id == 3) {
			m_ishave_328.GetComponent<UISprite>().set_enable (false);
			m_ishave_98.GetComponent<UISprite>().set_enable (false);

            m_ishave_98.transform.Find("Label").GetComponent<UILabel>().text = "[FFAF49]" + game_data._instance.get_t_language("temaihui_card.cs_1246_263"); //解锁
            m_ishave_328.transform.Find("Label").GetComponent<UILabel>().text = "[FFAF49]" + game_data._instance.get_t_language("temaihui_card.cs_1246_263"); //
		} else if (id == 0) {
			m_ishave_328.GetComponent<UISprite>().set_enable (true);
			m_ishave_98.GetComponent<UISprite>().set_enable (true);
            m_ishave_98.transform.Find("Label").GetComponent<UILabel>().text = "[E649FF]" + game_data._instance.get_t_language("hb_dress_page_item.cs_188_58"); //w
            m_ishave_328.transform.Find("Label").GetComponent<UILabel>().text = "[E649FF]" + game_data._instance.get_t_language("hb_dress_page_item.cs_188_58"); //w

           
		}
	}
	// Update is called once per frame
	void Update () {

	}
	void time()
	{
		if (m_show_time) {
			m_kaiqi_time.GetComponent<UILabel>().text = timer.get_time_show_color_year(timer.chatChangeTime(_msg1.rem_time),timer.chatChangeTime(_msg1.end_time),"[0AFF16]", "[0AF6FF]");// 显示活动剩余时间
			
			m_residue_time.GetComponent<UILabel>().text = timer.get_time_show_color_ex((long)timer.chatChangeTime((_msg1.rem_time -timer.now())),"[0AFF16]", "[0AF6FF]"); //显示活动下阶段开启时间
		}
	}
	public void click(GameObject obj)
	{
		if (obj.transform.name == "close") {
			s_message _message = new s_message ();
			_message.m_type = "show_jc_huodong";
			cmessage_center._instance.add_message (_message);
			return;
		}

		if (timer.run_day(_msg1.stat_time) >= 5) {
			root_gui._instance.show_prompt_dialog_box ("[ffc882]" + game_data._instance.get_t_language ("yueka_jijin_kaiqi.cs_106_59"));//请重新打开活动界面
			return;
		} 
		else {

			if (obj.transform.name == "yulan_button_98") {
				m_yulan_panel.GetComponent<yueka_yulan>().reset (1, game_data._instance.get_t_language ("yueka_jijin_kaiqi.cs_112_57"));//98元档基金奖励预览
				m_yulan_panel.SetActive (true);
			}
			if (obj.transform.name == "yulan_button_328") {
				m_yulan_panel.GetComponent<yueka_yulan>().reset (2, game_data._instance.get_t_language ("yueka_jijin_kaiqi.cs_116_57"));//328元档基金奖励预览
				m_yulan_panel.SetActive (true);
			}
			if (obj.transform.name == "button_chongqian") {
				s_message _message = new s_message ();
				_message.m_type = "show_recharge";
				cmessage_center._instance.add_message (_message);
			}
		}
	}
}
