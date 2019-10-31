
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ts_effect : MonoBehaviour  {
	
	public List<int> types = new List<int>();
	public List<int> values1 = new List<int>();
	public List<int> values2 = new List<int>();
	public List<int> values3= new List<int>();
	public List<Vector3> m_v = new List<Vector3>();
	public List<Vector3> m_v1 = new List<Vector3>();
	public List<Vector3> m_v2 = new List<Vector3>();
	private List<int> m_rewards_pz;
	public GameObject m_icon_root;
	public bool flag = false;
	private int m_type;
	private int m_num;
	private float m_time;
	private List<s_t_reward> m_rewards;
	public GameObject m_label;
	public GameObject m_shell;
	private List<GameObject> m_objs;
	private bool m_start;
	public GameObject m_close_panel;
	
	public UILabel m_hide;
	public UILabel m_name;
	void Start () 
	{

		
	}
	
	void OnEnable() {
		m_start = false;
		m_close_panel.SetActive (false);
		sys._instance.remove_child (m_icon_root);
		int num = types.Count;
		m_rewards = new List<s_t_reward>();
		m_rewards_pz = new List<int>();
		for (int i = 0; i < num; ++i)
		{
			int a = Random.Range(0, num - i);
			int pz = 0;
			s_t_reward t_reward = new s_t_reward();
			if (types[a] == 2)
			{
				s_t_item t_item = game_data._instance.get_item(values1[a]);
				if(t_item.font_color >= 4)
				{
					pz = 1;
				}
			}
			if(types[a] == 4)
			{
				s_t_equip t_equip = game_data._instance.get_t_equip(values1[a]);
				if (t_equip.font_color >= 3)
				{
					pz =1;
				}
			}
			t_reward.type = types[a];
			t_reward.value1 = values1[a];
			t_reward.value2 = values2[a];
			t_reward.value3 = values3[a];
			m_rewards.Add(t_reward);
			m_rewards_pz.Add(pz);
			types.RemoveAt(a);
			values1.RemoveAt(a);
			values2.RemoveAt(a);
			values3.RemoveAt(a);
		}
		if (num == 1)
		{
			m_type = 0;
			m_time = 0.4f;
		}
		else
		{
			m_type = 1;
			m_time = num * 0.3f + 0.1f;
		}
		m_num = 0;
		m_objs = new List<GameObject>();
		m_start = true;
	}
	
	public void click(GameObject obj)
	{
		if(obj.name == "hide")
		{
			this.gameObject.SetActive(false);
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_start)
		{
			return;
		}
		for (int i = 0; i < m_objs.Count; ++i)
		{
			m_objs[i].transform.localEulerAngles = new Vector3(0, 0, m_objs[i].transform.localEulerAngles.z + Time.deltaTime * 720);
		}
		
		if (m_time > 0)
		{
			m_time -= Time.deltaTime;
			if (m_type == 0)
			{
				if (m_num == 0 && m_time <= 0.2f)
				{
					GameObject icon = icon_manager._instance.create_reward_icon_ex(m_rewards[0].type, m_rewards[0].value1, m_rewards[0].value2, m_rewards[0].value3);
					icon.transform.parent = m_icon_root.transform;
					icon.transform.localPosition = new Vector3(0, -20, 0);
					icon.transform.localScale = new Vector3(1, 1, 1);
					
					m_objs.Add(icon);
					
					TweenPosition effect = sys._instance.add_pos_anim(icon,0.3f, new Vector3(0, 230, 0), 0);
					EventDelegate.Add(effect.onFinished, delegate() 
					                  {
						icon.transform.localEulerAngles = new Vector3(0, 0, 0);
						
						GameObject obj = (GameObject)Instantiate(m_label);
						obj.transform.parent = icon.transform;
						obj.transform.localPosition = new Vector3(0, 0, 0);
						obj.transform.localScale = new Vector3(1, 1, 1);
						if(m_rewards[0].type == 4)
						{
							obj.transform.Find("Label").GetComponent<UILabel>().text  = equip.get_equip_real_name (m_rewards[0].value1);
						}
						else if(m_rewards[0].type == 6)
						{
							obj.transform.Find("Label").GetComponent<UILabel>().text  = treasure.get_treasure_real_name (m_rewards[0].value1);
						}
						else if(m_rewards[0].type == 1)
						{
							obj.transform.Find("Label").GetComponent<UILabel>().text = sys._instance.m_self.get_name (m_rewards[0].type,m_rewards[0].value1,m_rewards[0].value2, m_rewards[0].value3);
						}
						else
						{
							s_t_item item = game_data._instance.get_item (m_rewards[0].value1);
							obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(item.name, item.font_color);
						}
						obj.SetActive(true);
						
						if (m_rewards_pz[0] == 1)
						{
							obj = (GameObject)Instantiate(m_shell);
							obj.transform.parent = icon.transform;
							obj.transform.localPosition = new Vector3(0, 0, 0);
							obj.transform.localScale = new Vector3(1, 1, 1);
							obj.SetActive(true);
						}
						
						m_objs.Remove(icon);
					});
					m_num = 1;
				}
			}
			else
			{
				for (int i = 0; i < m_rewards.Count; ++i)
				{
					if (m_num == i && m_time <= m_rewards.Count * 0.3f - i * 0.3f)
					{
						GameObject icon = icon_manager._instance.create_reward_icon_ex(m_rewards[i].type, m_rewards[i].value1, m_rewards[i].value2, m_rewards[i].value3);
						icon.transform.parent = m_icon_root.transform;
						if(m_rewards.Count > 5)
						{
							icon.transform.localPosition = m_v[i];
						}
						else
						{
							if(m_rewards.Count%2 == 0)
							{
								if(m_rewards.Count == 2)
								{
									icon.transform.localPosition = m_v2[2 - i];
								}
								else
								{
									icon.transform.localPosition = m_v2[3 - i];
								}
							}
							else
							{
								if(m_rewards.Count == 3)
								{
									icon.transform.localPosition = m_v1[3 - i];
								}
								else
								{
									icon.transform.localPosition = m_v1[4 - i];
								}
							}
						}
						icon.transform.localScale = new Vector3(1, 1, 1);
						
						m_objs.Add(icon);

						Vector3 v = new Vector3(-m_v[i].x, -m_v[i].y + 210, -m_v[i].z);
						if(m_rewards.Count < 5)
						{
							if(m_rewards.Count%2 == 0)
							{
								if(m_rewards.Count == 2)
								{
									v = new Vector3(-m_v2[2-i].x, 183, 0);
								}
								else
								{
									v = new Vector3(-m_v2[3-i].x, 183, 0);
								}
							}
							else
							{
								if(m_rewards.Count == 3)
								{
									v = new Vector3(-m_v1[3-i].x, 183, 0);
								}
								else
								{
									v = new Vector3(-m_v1[4-i].x, 183, 0);
								}
							}
						}
						
						TweenPosition effect = sys._instance.add_pos_anim(icon,0.3f, v, 0);
						int num = i;
						EventDelegate.Add(effect.onFinished, delegate() 
						                  {
							icon.transform.localEulerAngles = new Vector3(0, 0, 0);
							
							GameObject obj = (GameObject)Instantiate(m_label);
							obj.transform.parent = icon.transform;
							obj.transform.localPosition = new Vector3(0, 0, 0);
							obj.transform.localScale = new Vector3(1, 1, 1);
							if(m_rewards[num].type == 4)
							{
								
								obj.transform.Find("Label").GetComponent<UILabel>().text  = equip.get_equip_real_name (m_rewards[num].value1);
							}
							else if(m_rewards[num].type == 6)
							{
								obj.transform.Find("Label").GetComponent<UILabel>().text  = treasure.get_treasure_real_name (m_rewards[num].value1);
							}
							else if(m_rewards[num].type == 1)
							{
								obj.transform.Find("Label").GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_rewards[num].type, m_rewards[num].value1, m_rewards[num].value2, m_rewards[num].value3);
							}
							else
							{
								s_t_item item = game_data._instance.get_item (m_rewards[num].value1);
								obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(item.name, item.font_color);
							}
							obj.SetActive(true);
							
							if (m_rewards_pz[num] == 1)
							{
								obj = (GameObject)Instantiate(m_shell);
								obj.transform.parent = icon.transform;
								obj.transform.localPosition = new Vector3(0, 0, 0);
								obj.transform.localScale = new Vector3(1, 1, 1);
								obj.SetActive(true);
							}
							
							m_objs.Remove(icon);
						});
						
						m_num = i + 1;
					}
				}
			}
			
			if (m_time <= 0)
			{
				m_close_panel.SetActive (true);
			}
		}
	}
}
