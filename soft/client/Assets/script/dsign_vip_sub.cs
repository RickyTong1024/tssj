
using UnityEngine;
using System.Collections;

public class dsign_vip_sub : MonoBehaviour {

	public s_t_vip_libao t_vip_libao;
	public GameObject m_get;
	public GameObject m_recharge;
	public GameObject m_ylq;
	public GameObject m_view;
	public GameObject m_scro;
	// Use this for initialization
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
        s_t_vip t_vip = game_data._instance.get_t_vip(t_vip_libao.id);
        this.transform.Find("desc").GetComponent<UILabel>().text = t_vip.desc;
		for (int i = 0; i < t_vip_libao.rewards.Count; ++i)
		{
			GameObject _obj = icon_manager._instance.create_reward_icon( t_vip_libao.rewards[i].type , t_vip_libao.rewards[i].value1 , 
			                                                            t_vip_libao.rewards[i].value2 , t_vip_libao.rewards[i].value3);
			if(t_vip_libao.rewards.Count >= 5)
			{
				_obj.AddComponent<UIDragScrollView>().scrollView = m_view.GetComponent<UIScrollView>();
			}
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(-240 + i * 99,-17,0);
			_obj.AddComponent<UIDragScrollView>().scrollView = m_scro.GetComponent<UIScrollView>(); 
		}
		if(sys._instance.m_self.m_t_player.vip < t_vip_libao.id)
		{
			m_get.SetActive(false);
			m_recharge.SetActive(true);
			m_ylq.SetActive(false);
		}
		else
		{
			if(sys._instance.m_self.m_t_player.huodong_vip_libao == 1)
			{
				m_get.SetActive(false);
				m_recharge.SetActive(false);
				m_ylq.SetActive(true);
			}
			else
			{
				m_get.SetActive(true);
				m_recharge.SetActive(false);
				m_ylq.SetActive(false);
			}
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "get")
		{
			s_message _message = new s_message();
			_message.m_type = "vip_libao";
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.name == "recharge")
		{
			s_message _message = new s_message();
			_message.m_type = "huodong_vip_liabo_recharge";
			cmessage_center._instance.add_message(_message);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
