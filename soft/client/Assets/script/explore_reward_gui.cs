
using UnityEngine;
using System.Collections;

public class explore_reward_gui : MonoBehaviour {

	public GameObject m_item;
	public GameObject m_scro;
	public GameObject m_view;
	public UILabel m_title;
	public int type = 0;
	// Use this for initialization
	void Start () {
	
	}

	void OnEnable()
	{

		reset (type);
	}

	void reset(int type)
	{
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0,0);
		m_scro.transform.localPosition = new Vector3(0,0,0);
		sys._instance.remove_child (m_scro);
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0,0);
		m_view.transform.localPosition = new Vector3(0,0,0);
		sys._instance.remove_child (m_view);
		int index = 0;
		int _index = 0;
		if(type == 0)
		{
			m_title.text = game_data._instance.get_t_language ("explore_reward_gui.cs_42_18"); //探索可能获得下面一种道具
			foreach(int id in game_data._instance.m_dbc_manyou.m_index.Keys)
			{
				s_t_manyou t_manyou = game_data._instance.get_t_manyou(id);
				if(t_manyou.type == 1)
				{
					GameObject item = (GameObject)Object.Instantiate(m_item);
					item.transform.parent = m_scro.transform;
					item.transform.localPosition = new Vector3(0,121- 83*index,0);
					item.transform.localScale = new Vector3(1,1,1);

					item.transform.Find("name").GetComponent<UILabel>().text = sys._instance.m_self.get_name(t_manyou.reward.type,t_manyou.reward.value1,t_manyou.reward.value2,t_manyou.reward.value3);
					GameObject icon = item.transform.Find("icon").gameObject;
					sys._instance.remove_child(icon);
					GameObject obj = icon_manager._instance.create_reward_icon(t_manyou.reward.type,t_manyou.reward.value1,t_manyou.reward.value2,t_manyou.reward.value3);
					obj.transform.parent = icon.transform;
					obj.transform.localPosition = new Vector3(0,0,0);
					obj.transform.localScale = new Vector3(1,1,1);
					item.SetActive(true);
					index++;
				}
				if(t_manyou.type == 2)
				{
					GameObject item = (GameObject)Object.Instantiate(m_item);
					item.transform.parent = m_view.transform;
					item.transform.localPosition = new Vector3(0,121- 83*_index,0);
					item.transform.localScale = new Vector3(1,1,1);

					item.transform.Find("name").GetComponent<UILabel>().text = sys._instance.m_self.get_name(t_manyou.reward.type,t_manyou.reward.value1,t_manyou.reward.value2,t_manyou.reward.value3);
					GameObject icon = item.transform.Find("icon").gameObject;
					sys._instance.remove_child(icon);
					GameObject obj = icon_manager._instance.create_reward_icon(t_manyou.reward.type,t_manyou.reward.value1,t_manyou.reward.value2,t_manyou.reward.value3);
					obj.transform.parent = icon.transform;
					obj.transform.localPosition = new Vector3(0,0,0);
					obj.transform.localScale = new Vector3(1,1,1);
					item.SetActive(true);
					_index++;
				}
			}
		}
		else
		{
			m_title.text = game_data._instance.get_t_language ("explore_reward_gui.cs_84_18");//开启宝箱可能获得下面一种道具
			foreach(int id in game_data._instance.m_dbc_manyou_qiyu.m_index.Keys)
			{
				s_t_manyou_qiyu t_manyou_qiyu = game_data._instance.get_t_manyou_qiyu(id);
				if(t_manyou_qiyu.type == 3002 &&t_manyou_qiyu.def1 == type && t_manyou_qiyu.def2 == 1)
				{
					GameObject item = (GameObject)Object.Instantiate(m_item);
					item.transform.parent = m_scro.transform;
					item.transform.localPosition = new Vector3(0,121- 83*index,0);
					item.transform.localScale = new Vector3(1,1,1);
					
					item.transform.Find("name").GetComponent<UILabel>().text = sys._instance.m_self.get_name(t_manyou_qiyu.rewards[0].type,t_manyou_qiyu.rewards[0].value1,t_manyou_qiyu.rewards[0].value2,t_manyou_qiyu.rewards[0].value3);
					GameObject icon = item.transform.Find("icon").gameObject;
					sys._instance.remove_child(icon);
					GameObject obj = icon_manager._instance.create_reward_icon(t_manyou_qiyu.rewards[0].type,t_manyou_qiyu.rewards[0].value1,t_manyou_qiyu.rewards[0].value2,t_manyou_qiyu.rewards[0].value3);
					obj.transform.parent = icon.transform;
					obj.transform.localPosition = new Vector3(0,0,0);
					obj.transform.localScale = new Vector3(1,1,1);
					item.SetActive(true);
					index++;
				}
				if(t_manyou_qiyu.type == 3002 &&t_manyou_qiyu.def1 == type && t_manyou_qiyu.def2 == 0)
				{
					GameObject item = (GameObject)Object.Instantiate(m_item);
					item.transform.parent = m_view.transform;
					item.transform.localPosition = new Vector3(0,121- 83*_index,0);
					item.transform.localScale = new Vector3(1,1,1);
					
					item.transform.Find("name").GetComponent<UILabel>().text = sys._instance.m_self.get_name(t_manyou_qiyu.rewards[0].type,t_manyou_qiyu.rewards[0].value1,t_manyou_qiyu.rewards[0].value2,t_manyou_qiyu.rewards[0].value3);
					GameObject icon = item.transform.Find("icon").gameObject;
					sys._instance.remove_child(icon);
					GameObject obj = icon_manager._instance.create_reward_icon(t_manyou_qiyu.rewards[0].type,t_manyou_qiyu.rewards[0].value1,t_manyou_qiyu.rewards[0].value2,t_manyou_qiyu.rewards[0].value3);
					obj.transform.parent = icon.transform;
					obj.transform.localPosition = new Vector3(0,0,0);
					obj.transform.localScale = new Vector3(1,1,1);
					item.SetActive(true);
					_index++;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
