using UnityEngine;

public class juntuan_active_info : MonoBehaviour,IMessage {
	public GameObject m_boss;
	public GameObject m_shangcehng;
	public GameObject m_qiandao;
	public UIToggle m_button_qiangdao;
	public GameObject m_chujiqiandao;
	public GameObject m_zhongjiqiandao;
	public GameObject m_gaojiqiandao;

	public GameObject m_chuji_tili;
	public GameObject m_chuji_gongxian;
	public GameObject m_chuji_huobi;

	public GameObject m_zhongji_tili;
	public GameObject m_zhongji_gongxian;
	public GameObject m_zhongji_huobi;

	public GameObject m_gaoji_tili;
	public GameObject m_gaoji_gongxian;
	public GameObject m_gaoji_huobi;
    public GameObject m_mobai;
    public GameObject m_guild_mibai;


	public UILabel m_juntuanqiangdao;
	public UILabel m_juntuanqiangdao1;
	public UILabel m_qiandao_label;
	public UILabel m_tili;
	public UILabel m_gongxian;
	public UILabel m_qiandao_label1;
	public UILabel m_tili1;
	public UILabel m_gongxian1;
	public UILabel m_qiandao_label2;
	public UILabel m_tili2;
	public UILabel m_gongxian2;
	public UILabel m_juntuanshangdaon;
	public UILabel m_tanxian;
	public UILabel m_desc;

	public UILabel m_chuji;
	public UILabel m_zhongji;
	public UILabel m_gaoji;
	public UILabel m_desc1;

	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnEnable()
	{
        //m_button_qiangdao.value = true;
        //huodong_active ("qiandao");
        //s_t_guild_sign t_guild_sign = game_data._instance.get_guild_sign(0);
        //m_chuji_tili.GetComponent<UILabel>().text = "+" + t_guild_sign.tili;
        //m_chuji_gongxian.GetComponent<UILabel>().text = "+" + t_guild_sign.gongxian;
        //m_chuji_huobi.GetComponent<UILabel>().text = "×" + t_guild_sign.coin;
        //t_guild_sign = game_data._instance.get_guild_sign(1);
        //m_zhongji_tili.GetComponent<UILabel>().text = "+" + t_guild_sign.tili;
        //m_zhongji_gongxian.GetComponent<UILabel>().text = "+" + t_guild_sign.gongxian;
        //m_zhongji_huobi.GetComponent<UILabel>().text = "×" + t_guild_sign.zuanshi;
        //t_guild_sign = game_data._instance.get_guild_sign(2);
        //m_gaoji_tili.GetComponent<UILabel>().text = "+" + t_guild_sign.tili;
        //m_gaoji_gongxian.GetComponent<UILabel>().text = "+" + t_guild_sign.gongxian;
        //m_gaoji_huobi.GetComponent<UILabel>().text = "×" + t_guild_sign.zuanshi;
        //m_boss.GetComponent<juntuan_boss>().CancelInvoke ();

		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_ACTIVITY, _msg);						
	}

	void IMessage.net_message(s_net_message message)
	{
		
		if (message.m_opcode == opclient_t.CMSG_GUILD_ACTIVITY) 
		{

			m_mobai.GetComponent<jt_mobai>().m_msg = net_http._instance.parse_packet<protocol.game.smsg_guild_activity> (message.m_byte);
            huodong_active("mobai");
            m_mobai.GetComponent<jt_mobai>().updateui();
			juntuan_gui._instance.updategui();
		}
	}

	void huodong_active (string s)
	{
		m_boss.SetActive (false);
		m_shangcehng.SetActive (false);
		m_qiandao.SetActive (false);
		switch (s) 
		{
		case "boss":
			m_boss.SetActive (true);
			break;
		case "mobai":
            m_mobai.SetActive(true);
			break;
		case "shangcheng":
			m_shangcehng.SetActive (true);
			break;
		}
	}

	public void click (GameObject obj)
	{
		protocol.game.cmsg_guild_sign_in _msg = new protocol.game.cmsg_guild_sign_in();
		switch (obj.name) 
		{
		case "boss":
			huodong_active("boss");
			break;
		case "qiandao":
			huodong_active("mobai");
			break;
		case "shangcheng":
			huodong_active("shangcheng");
			break;
		case "chujiqiandao":
			
			_msg.sign_in_type = 0;
			net_http._instance.send_msg<protocol.game.cmsg_guild_sign_in> (opclient_t.CMSG_GUILD_SIGN_IN,_msg);
			break;
		case "zhongjiqiandao":
			
			break;
		case "gaojiqiandao":
			
			break;
        case "mobai":
            m_mobai.SetActive(true);
            break;
        case "close_mobai":
            m_mobai.transform.Find("frame_big").GetComponent<frame>().hide();
            break;
		}
	}

	void IMessage.message (s_message message)
	{
		
		
	}
}
