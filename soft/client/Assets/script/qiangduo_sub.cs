
using UnityEngine;
using System.Collections;

public class qiangduo_sub : MonoBehaviour ,IMessage{
	public string m_name;
	public int m_level;
	public int m_rate;
	public int m_bf;
	public ulong m_id;
	public ulong m_guid;
	public int m_vip;
	public int m_achieve;
    public int gq;
	public bool m_qiangduo_5 = false;
	public int m_index;
    public bool is_npc;

	
	public GameObject m_player_level;
	public GameObject m_player_name;
	public GameObject m_rate_obj;
	public GameObject m_bf_obj;
	public GameObject m_icon_obj;
	public GameObject m_button_1;
	public GameObject m_button_2;
    public GameObject m_level_obj;

	public UILabel m_qiangduo_Label;
	public UILabel m_qiangduo_5_Label;

	// Use this for initialization
	void Start() 
	{
		cmessage_center._instance.add_handle (this);

	}
	void IMessage.net_message(s_net_message message)
	{
		
	}
	void IMessage.message(s_message message)
	{

	}
	public void reset()
	{
		m_player_level.GetComponent<UILabel>().text =  m_level + "";
		m_player_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(m_achieve) + m_name;
        m_bf_obj.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_bf);
		
		if(m_rate >= 50)
		{
			m_rate_obj.GetComponent<UILabel>().text = game_data._instance.get_t_language ("qiangduo_sub.cs_55_45");//[ff6100]高概率
		}
		else if(m_rate >= 30)
		{
			m_rate_obj.GetComponent<UILabel>().text = game_data._instance.get_t_language ("qiangduo_sub.cs_59_45");//[ff00ff]较高概率
		}
		else if(m_rate >= 20)
		{
			m_rate_obj.GetComponent<UILabel>().text = game_data._instance.get_t_language ("qiangduo_sub.cs_63_45");//[0000ff]一般概率
		}
		else if(m_rate <= 19)
		{
            m_rate_obj.GetComponent<UILabel>().text = game_data._instance.get_t_language ("qiangduo_sub.cs_67_54");//[00ff00]较低概率
		}

		sys._instance.remove_child (m_icon_obj);
        GameObject _obj1 = icon_manager._instance.create_player_icon((int)m_id, m_achieve, m_vip, gq);
		
		_obj1.transform.parent = m_icon_obj.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
		if(m_qiangduo_5)
		{
			m_button_2.SetActive(true);

			m_button_2.transform.localPosition = new Vector3(56,0f,0);
		}
		else
		{
			m_button_2.SetActive(false);
			m_button_1.transform.localPosition = m_button_2.transform.localPosition;
		}
	

	}
	void click(GameObject obj)
	{
		s_message _mes = new s_message ();
	
		if(obj.name == "1" || obj.name == "button_qiangduo")
		{
           
		   //每日[00ffff]01:00[-]至[00ffff]9:00[-]无法对其他玩家进行抢夺
			_mes.m_type = "qiangduo_1";
			_mes.m_ints.Add(m_index);
            _mes.m_bools.Add(is_npc);
			cmessage_center._instance.add_message(_mes);
		}
		else if(obj.name == "5")
		{
			_mes.m_type = "qiangduo_5";
			_mes.m_ints.Add(m_index);
            _mes.m_bools.Add(is_npc);
			cmessage_center._instance.add_message(_mes);
		}

	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
}
