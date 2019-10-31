
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class dui_huan_sub : MonoBehaviour {

	public s_t_itemhecheng t_itemhecheng;
	public GameObject m_icon;
	public GameObject m_name;
	public List<GameObject> m_cl_icons = new List<GameObject>();
	public List<GameObject> m_cl_names = new List<GameObject>();
	public GameObject m_effect;
	public GameObject m_add;
	public GameObject m_duihuan;
	public UILabel m_duihuan_Label;
	public GameObject m_scro;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		m_duihuan_Label.text = game_data._instance.get_t_language ("dui_huan_sub.cs_24_25");//兑换
		m_duihuan.GetComponent<BoxCollider>().enabled = true;
		m_duihuan.GetComponent<UISprite>().set_enable (true);
		if(t_itemhecheng.type == 7 && sys._instance.m_self.m_t_player.guanghuan.Contains(t_itemhecheng.item_id))
		{
			m_duihuan_Label.text = game_data._instance.get_t_language ("dui_huan_sub.cs_29_26");//已拥有
			m_duihuan.GetComponent<BoxCollider>().enabled = false;
			m_duihuan.GetComponent<UISprite>().set_enable (false);
		}
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(t_itemhecheng.type,t_itemhecheng.item_id,t_itemhecheng.item_num,0);
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (t_itemhecheng.type, t_itemhecheng.item_id,1,0);

		for(int i = 0; i < m_cl_icons.Count && i < t_itemhecheng.cl_type.Count;++i)
		{
			m_add.SetActive(true);
			sys._instance.remove_child (m_cl_icons[i]);
			int cl_num = sys._instance.m_self.get_item_num ((uint)t_itemhecheng.cl_id[i]);
			GameObject _icon1 = icon_manager._instance.create_item_icon_ex(t_itemhecheng.cl_id[i],cl_num,t_itemhecheng.cl_num[i]);
			_icon1.transform.parent = m_cl_icons[i].transform;
			_icon1.transform.localScale = new Vector3 (1, 1, 1);
			_icon1.transform.localPosition = new Vector3 (0, 0, 0);
			_icon1.AddComponent<UIDragScrollView>();
			_icon1.GetComponent<UIDragScrollView>().scrollView = m_scro.GetComponent<UIScrollView>();
			UIButtonMessage[] message = _icon1.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_cl_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";
			m_cl_names[i].GetComponent<UILabel>().text = sys._instance.m_self.get_name (t_itemhecheng.cl_type[i], t_itemhecheng.cl_id[i],1,0);
		}
		if(t_itemhecheng.cl_type.Count < m_cl_icons.Count)
		{
			m_add.SetActive(false);
			m_cl_icons[1].SetActive(false);
			m_cl_names[1].GetComponent<UILabel>().text = "";
		}
		if(effect())
		{
			m_effect.SetActive(true);
		}
		else
		{
			m_effect.SetActive(false);
		}
	}

	public bool effect()
	{
		if(t_itemhecheng.type == 7)
		{
			if(sys._instance.m_self.m_t_player.guanghuan.Contains(t_itemhecheng.item_id))
			{
				return false;
			}
		}
		for(int i = 0; i < t_itemhecheng.cl_type.Count;++i)
		{
			int cl_num = sys._instance.m_self.get_item_num ((uint)t_itemhecheng.cl_id[i]);
			if(cl_num < t_itemhecheng.cl_num[i])
			{
				return false;
			}
		}
		return true;
	}

	public void click_cl_icon(GameObject obj)
	{
		int id = obj.transform.GetComponent<item_icon>().m_item_id;
		s_t_item t_item = game_data._instance.get_item (id);
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add (id);
		cmessage_center._instance.add_message(message);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "duihuan")
		{
			for(int i = 0; i < t_itemhecheng.cl_type.Count;++i)
			{
				int cl_num = sys._instance.m_self.get_item_num ((uint)t_itemhecheng.cl_id[i]);
				if(cl_num < t_itemhecheng.cl_num[i])
				{
					s_message message = new s_message ();
					message.m_type = "show_cl_gui";
					message.m_ints.Add (t_itemhecheng.cl_id[i]);
					cmessage_center._instance.add_message(message);
					return;
				}
			}
			if(t_itemhecheng.type != 7)
			{
				s_message _message = new s_message ();
				_message.m_type = "buy_num_gui";
				_message.m_object.Add(t_itemhecheng);
				_message.m_ints.Add(0);
				_message.m_ints.Add(7);
				cmessage_center._instance.add_message(_message);
			}
			else
			{
				s_message _message = new s_message ();
				_message.m_type = "buy_guanghuan_gui";
				_message.m_object.Add(t_itemhecheng);
				cmessage_center._instance.add_message(_message);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
