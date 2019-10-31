
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class recharge_gui : MonoBehaviour {

	public GameObject recharge_panel_;
	public GameObject vip_panel_;
	public int m_type = 0;
	public int m_vip = -1;
	public GameObject m_zcz;
	public GameObject m_now_vip;
	public GameObject m_title;
	public GameObject m_chongzhi;
	public GameObject m_tequan;
	public GameObject m_exp_bar;
	public GameObject m_yueka;
	public GameObject m_gjyueka;
	
	// Use this for initialization
	void Start () {
       
        
	}

	public void OnEnable()
	{
		InvokeRepeating("time", 0.0f,1.0f);
		reset (m_type, m_vip);
	}

	void OnDisable()
	{
		CancelInvoke ("time");
	}

	public void reset(int type, int vip = -1)
	{
		m_vip = -1;
		m_type = type;
		s_t_vip t_nvip = game_data._instance.get_t_vip (sys._instance.m_self.m_t_player.vip);
        m_now_vip.transform.GetComponent<UILabel>().text = t_nvip.desc;
        s_t_vip t_vip = game_data._instance.get_t_vip (sys._instance.m_self.m_t_player.vip + 1);
		if (t_vip == null)
		{
			m_zcz.SetActive(false);
			m_exp_bar.transform.Find("num").GetComponent<UILabel>().text
				= t_nvip.recharge.ToString() + "/" + t_nvip.recharge.ToString();
			m_exp_bar.transform.Find("bar").GetComponent<UIProgressBar>().value = 1;
		}
		else
		{
			m_zcz.SetActive(true);
			m_exp_bar.transform.Find("num").GetComponent<UILabel>().text
				= sys._instance.m_self.m_t_player.vip_exp.ToString() + "/" + t_vip.recharge.ToString();
			m_exp_bar.transform.Find("bar").GetComponent<UIProgressBar>().value
				= (float)(sys._instance.m_self.m_t_player.vip_exp) / t_vip.recharge;
			m_zcz.transform.Find("rnum").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("recharge_gui.cs_58_82"), (t_vip.recharge - sys._instance.m_self.m_t_player.vip_exp).ToString(),t_vip.level.ToString());//再充值{0}钻石即可成为VIP{1}
		}
		if(sys._instance.m_self.m_t_player.vip_exp == 0)
		{
			m_exp_bar.transform.Find("GameObject").gameObject.SetActive(false);
		}
		else
		{
			m_exp_bar.transform.Find("GameObject").gameObject.SetActive(true);
		}

		if (m_type == 0)
		{
			recharge_panel_.SetActive(true);
			recharge_panel_.GetComponent<recharge_panel>().reset();
			vip_panel_.SetActive(false);
			m_tequan.SetActive(true);
			m_chongzhi.SetActive(false);
			m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language ("recharge_gui.cs_76_42");//充值
			m_tequan.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("recharge_gui.cs_77_72");//特权
		}
		else
		{
			recharge_panel_.SetActive(false);
			vip_panel_.SetActive(true);
			if (vip == -1)
			{
				vip_panel_.GetComponent<vip_panel>().reset(sys._instance.m_self.m_t_player.vip);
			}
			else
			{
				vip_panel_.GetComponent<vip_panel>().reset(vip);
			}
			m_tequan.SetActive(false);
			m_chongzhi.SetActive(true);
			m_chongzhi.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("recharge_gui.cs_76_42");//充值
			m_title.GetComponent<UILabel>().text = "VIP";
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			transform.Find("frame_big").GetComponent<frame>().hide();

			s_message _message = new s_message();
			_message.m_type = "resert_comeback_gui";
			cmessage_center._instance.add_message(_message);


            s_message _message1 = new s_message();
            _message1.m_type = "resert_jc_huodong_pag";
            cmessage_center._instance.add_message(_message1);

		}
		else if (obj.transform.name == "tequan" || obj.transform.name == "chongzhi")
		{
			m_type = 1 - m_type;
			reset(m_type);
		}
		else if(obj.transform.name == "vip_toupiao")
		{
			transform.GetComponent<ui_title_anim>().hide_ui();
			s_message _message = new s_message();
			_message.m_type = "show_vip_toupiao";
			cmessage_center._instance.add_message(_message);
		}
	}

	void time()
	{
		if (sys._instance.m_self.m_t_player.yueka_time > timer.now())
		{
			m_yueka.SetActive(true);
			long t = (long)(sys._instance.m_self.m_t_player.yueka_time - timer.now());
			m_yueka.GetComponent<UILabel>().text = game_data._instance.get_t_language ("recharge_gui.cs_133_42") + timer.get_time_show_ex(t);//月卡剩余:
		}
		else
		{
			m_yueka.SetActive(false);
		}
		if (sys._instance.m_self.m_t_player.zhouka_time > timer.now())
		{
			m_gjyueka.SetActive(true);
			long t = (long)(sys._instance.m_self.m_t_player.zhouka_time - timer.now());
			m_gjyueka.GetComponent<UILabel>().text = game_data._instance.get_t_language ("recharge_gui.cs_143_44") + timer.get_time_show_ex(t);//高级月卡剩余:
		}
		else
		{
			m_gjyueka.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

}
