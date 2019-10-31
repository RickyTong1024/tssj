
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class touxiang_item : MonoBehaviour {

	public int t_class_id;
	public s_t_class t_class;
	public GameObject m_xuan;
	// Use this for initialization
	void Start () {

		}


	public void click(GameObject obj)
	{
		if(obj.name == "xuan")
		{
			s_message _msg = new s_message ();
			_msg.m_type = "chang_touxiang";
			_msg.m_ints.Add (t_class_id);
			cmessage_center._instance.add_message (_msg);
		}
	}

	public void reset()
	{
		t_class = game_data._instance.get_t_class (t_class_id);
		this.transform.Find("name").GetComponent<UILabel>().text = ccard.get_color_name(t_class.name, t_class.color);
		bool flag = false;
		for(int i = 0 ; i < sys._instance.m_self.m_t_player.role_template_ids.Count;++i)
		{
			if(t_class_id == sys._instance.m_self.m_t_player.role_template_ids[i])
			{
				flag = true;
				break;
			}
		}
		if(flag)
		{
			transform.Find("locked").gameObject.SetActive(false);
			m_xuan.GetComponent<BoxCollider>().enabled = true;
			m_xuan.GetComponent<UISprite>().set_enable(true);
			m_xuan.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_3064_78");//更换
		}
		else
		{
			transform.Find("locked").gameObject.SetActive(true);
			m_xuan.GetComponent<BoxCollider>().enabled = false;
			m_xuan.GetComponent<UISprite>().set_enable(false);
			m_xuan.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_3064_78");//更换
		}
		if(t_class_id == sys._instance.m_self.m_t_player.template_id)
		{
			transform.Find("locked").gameObject.SetActive(false);
			m_xuan.GetComponent<BoxCollider>().enabled = false;
			m_xuan.GetComponent<UISprite>().set_enable(true);
			m_xuan.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("touxiang_item.cs_59_70");//使用中
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

}
