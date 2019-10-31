
using UnityEngine;
using System.Collections;

public class dsign_week_sub : MonoBehaviour {

	public int id;
	public GameObject m_name;
	public GameObject m_view;
	public GameObject m_scro;
	public GameObject m_desc;
	public GameObject m_discount;
	public GameObject m_get;
	public GameObject m_ylq;
	public GameObject m_price;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		m_price.SetActive (true);
		s_t_week_libao t_week_libao = game_data._instance.get_t_week_libao (id);
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		for (int i = 0; i < t_week_libao.rewards.Count; ++i)
		{
			GameObject _obj = icon_manager._instance.create_reward_icon( t_week_libao.rewards[i].type , t_week_libao.rewards[i].value1 , 
			                                                            t_week_libao.rewards[i].value2 , t_week_libao.rewards[i].value3);
			if(t_week_libao.rewards.Count >= 5)
			{
				_obj.AddComponent<UIDragScrollView>().scrollView = m_view.GetComponent<UIScrollView>(); 
			}
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(-240 + i * 99,-17,0);
			_obj.AddComponent<UIDragScrollView>().scrollView = m_scro.GetComponent<UIScrollView>(); 
		}
		m_name.GetComponent<UILabel>().text = t_week_libao.name;
		if(t_week_libao.discount >= 10 || t_week_libao.discount <= 0)
		{
			m_discount.transform.gameObject.SetActive(false);
		}
		else
		{
			m_discount.transform.gameObject.SetActive(true);
			m_discount.GetComponent<UILabel>().text =  sys._instance.discountChange(t_week_libao.discount) + game_data._instance.get_t_language ("dsign_week_sub.cs_52_80");//折
		}
		int num = 0;
		for(int i = 0; i < sys._instance.m_self.m_t_player.huodong_week_libao.Count;++i)
		{
			if(sys._instance.m_self.m_t_player.huodong_week_libao[i] == id)
			{
				num++;
			}
		}
		m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("dsign_week_sub.cs_62_55"), (t_week_libao.num - num).ToString () , t_week_libao.num.ToString ());//本周剩余次数:{0}/{1}
		if(sys._instance.m_self.m_t_player.jewel < t_week_libao.jewel)
		{
			m_price.GetComponent<UILabel>().text = "[ff0000]" + t_week_libao.jewel.ToString();
		}
		else
		{
			m_price.GetComponent<UILabel>().text = sys._instance.get_res_color(2) + t_week_libao.jewel.ToString();
		}
		if(num >= t_week_libao.num)
		{
			m_price.SetActive (false);
			m_ylq.SetActive(true);
			m_get.SetActive(false);
		}
		else
		{
			m_ylq.SetActive(false);
			m_get.SetActive(true);
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "get")
		{
			s_t_week_libao t_week_libao = game_data._instance.get_t_week_libao (id);
			if(sys._instance.m_self.m_t_player.jewel < t_week_libao.jewel)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
				return;
			}
			s_message _message = new s_message();
			_message.m_type = "huodong_week_libao";
			_message.m_ints.Add (id);
			cmessage_center._instance.add_message(_message);
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
