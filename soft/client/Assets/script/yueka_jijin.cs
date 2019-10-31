
using UnityEngine;
using System.Collections;

public class yueka_jijin : MonoBehaviour ,IMessage{

	public GameObject m_view; // 奖励物品的父对象
	public GameObject m_toggle_98;
	public GameObject m_toggle_328;
	private GameObject m_sign;
	public GameObject m_kaiqitimer;
	public int m_curdang; //当前加载的档次
	public int m_day_sign;
	private protocol.game.smsg_huodong_yueka_view _msg1;
	private bool m_timer_show =false;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
					
	}
	void OnDestroy()
	{
		CancelInvoke("time");
		cmessage_center._instance.remove_handle (this);
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
	void IMessage.message (s_message message)
	{
		
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_YUEKA_VIEW) {
			
			_msg1 =net_http._instance.parse_packet<protocol.game.smsg_huodong_yueka_view>(message.m_byte);
			if (_msg1.level ==1) {  //判断玩家获得的基金档位  98档
				reset (1);
				m_toggle_328.SetActive(false);
			} else if (_msg1.level ==2) {//判断玩家获得的基金档位  328档
				reset (2);
				m_toggle_328.transform.localPosition =m_toggle_98.transform.localPosition;
				m_toggle_328.GetComponent<UIToggle>().startsActive =true;
				m_toggle_98.SetActive(false);
			} else {	//判断玩家获得的基金档位  648档
				if(m_curdang==1){
				reset(1);
				return;
				}
				else if(m_curdang == 2)
				{
				reset(2);
				return;
				}
				reset(1);
			}
			m_timer_show =true;
			
			InvokeRepeating ("time", 0.0f, 1.0f);

		}

		if (message.m_opcode == opclient_t.CMSG_HUODONG_YUEKA_REWARD) {
			s_t_yueka t_yueka_sign = game_data._instance.get_t_yueka_jijin (m_day_sign);
			protocol.game.smsg_huodong_reward _msg2 =net_http._instance.parse_packet<protocol.game.smsg_huodong_reward>(message.m_byte);
			if(m_curdang ==1){
				

				for(int i = 0 ;i< _msg2.treasures.Count;i++)
				{
					sys._instance.m_self.add_treasure(_msg2.treasures[i]);
				}
				for (int i = 0; i < _msg2.equips.Count; ++i)
				{
					sys._instance.m_self.add_equip(_msg2.equips[i]);
				}
				
				for (int i = 0; i < _msg2.types.Count; ++i)
				{
					sys._instance.m_self.add_reward( _msg2.types[i], _msg2.value1s[i],_msg2.value2s[i], _msg2.value3s[i],game_data._instance.get_t_language ("yueka_jijin.cs_96_106"));//月卡基金奖励
				}
				
				for(int i = 0;i < _msg2.roles.Count;i ++)
				{
					sys._instance.m_self.add_card(_msg2.roles[i], true);
				}
				protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
				net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_YUEKA_VIEW, _msg);

//				reset(1);

				
			}

			if(m_curdang ==2)
			{
				
				for(int i = 0 ;i< _msg2.treasures.Count;i++)
				{
					sys._instance.m_self.add_treasure(_msg2.treasures[i]);
				}
				for (int i = 0; i < _msg2.equips.Count; ++i)
				{
					sys._instance.m_self.add_equip(_msg2.equips[i]);
				}
				
				for (int i = 0; i < _msg2.types.Count; ++i)
				{
					sys._instance.m_self.add_reward( _msg2.types[i], _msg2.value1s[i],_msg2.value2s[i], _msg2.value3s[i],game_data._instance.get_t_language ("yueka_jijin.cs_96_106"));//月卡基金奖励
				}
				
				for(int i = 0;i < _msg2.roles.Count;i ++)
				{
					sys._instance.m_self.add_card(_msg2.roles[i], true);
				}

				protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
				net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_YUEKA_VIEW, _msg);

//				reset(2);

			}
		}
	}

	public void reset(int id,bool flag = false)
	{
		m_curdang = id;
		int currday = 0;
		if (!flag) {
			if (m_view.GetComponent<SpringPanel>() != null) {
				m_view.GetComponent<SpringPanel>().enabled = false;
			}
//			m_view.transform.localPosition = new Vector3 (0, 0, 0);
//			m_view.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
		}
		sys._instance.remove_child (m_view);
		//if (id == 1) {
			dbc m_dbc_yueka_jijin = game_data._instance.m_dbc_yueka_jijin;
			GameObject gsign = null;
			for (int i = 0; i < m_dbc_yueka_jijin.get_y(); ++i) {
				
				gsign = game_data._instance.ins_object_res ("ui/yueka_jiangli");
				gsign.transform.parent = m_view.transform;
				gsign.transform.localPosition = new Vector3 (-225 + 103 * (i % 6), 80 - 125 * (i / 6));
				gsign.transform.localScale = new Vector3 (1, 1, 1);
				gsign.transform.GetComponent<yueka_sign>().m_index = i;
				gsign.transform.GetComponent<yueka_sign>().m_scro = m_view;

			    if(id == 1)  //对不同档位进行判断，领取情况， 1代表可领取但是还没领取，0代表领取了，3代表没有领取资格
			    {
			
					    int m_get= 0;
					    if(i < _msg1.reward1.Count){
					    if(_msg1.reward1[i] ==1){
						    m_get=1;
						    currday++;
					    }else
					    {
						    m_get=0;
					    }
					    if(_msg1.reward1.Count > 20){
						    gsign.transform.GetComponent<yueka_sign>().init(id,m_get,20);
					    }else{
						    gsign.transform.GetComponent<yueka_sign>().init(id,m_get,_msg1.reward1.Count);
						
					    }
					    continue;
				    }

				    if(_msg1.reward1.Count > 20){
					    gsign.transform.GetComponent<yueka_sign>().init(id,3,20);
				    }else{
					    gsign.transform.GetComponent<yueka_sign>().init(id,3,_msg1.reward1.Count);
					
				    }
			    }
			    else if(id ==2)
			    {
				    int m_get= 0;
				    if(i < _msg1.reward2.Count){
					    if(_msg1.reward2[i] ==1){
						    m_get=1;
						    currday++;
					    }else
					    {
						    m_get=0;
					    }

					    if(_msg1.reward2.Count > 20){
						    gsign.transform.GetComponent<yueka_sign>().init(id,m_get,20);
					    }else{
						    gsign.transform.GetComponent<yueka_sign>().init(id,m_get,_msg1.reward2.Count);
						
					    }
					    continue;
				    }
				
				    if(_msg1.reward2.Count > 20){
					    gsign.transform.GetComponent<yueka_sign>().init(id,3,20);
				    }else{
					    gsign.transform.GetComponent<yueka_sign>().init(id,3,_msg1.reward2.Count);
					
				    }
				
			    }
    //				if (i == 0) {
    //					fsign = gsign;
    //					
    //				}
    //				if (i == sys._instance.m_self.m_t_player.daily_sign_index % 30 - 1) {
    //					m_sign = gsign;
    //				}
    //			}
    //			if (sys._instance.m_self.m_t_player.daily_sign_index % 30 == 0) {
    //				m_sign = gsign;
    //			}
			    }
		    //}


	}

	public void click(GameObject obj)
	{
		if (obj.transform.name == "98dang") 
		{
			reset(1);
		}
		if (obj.transform.name == "328dang") 
		{
			reset(2);
		}

		if (obj.transform.name == "close") 
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message ();
			_message.m_type = "show_jc_huodong";
			cmessage_center._instance.add_message (_message);
		}
	}
	void time()
	{
		if (m_timer_show) 
		{
			m_kaiqitimer.GetComponent<UILabel>().text =timer.get_time_show_color_year(_msg1.rem_time,_msg1.end_time,"[0AFF16]", "[0AF6FF]");
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
