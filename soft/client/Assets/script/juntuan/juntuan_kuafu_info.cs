using System.Collections.Generic;
using UnityEngine;

public class juntuan_kuafu_info : MonoBehaviour,IMessage {
	public GameObject juntuan_number_items; //跨服战 成员勾选列表
	public GameObject m_button_yijian;
	public GameObject m_button_save;
	public GameObject m_button_off;
	//按钮
	public List<int> num0 =new List<int>();
	public List<int> m_yijian =new List<int>();
	public int m_index ;
	public UILabel m_juandNum;
	void Awake()
	{
//		m_index = 1;

	}
	// Use this for initialization
	void Start () {
	
	}
	void OnEnable()
	{
		juntuan_number_items.GetComponent<UIScrollView>().ResetPosition ();

	}

	public void initItems()
	{
		num0.Clear ();
		m_yijian.Clear ();
		if(juntuan_number_items.GetComponent<SpringPanel>() != null)
		{
			juntuan_number_items.GetComponent<SpringPanel>().enabled = false;
		}
		if (juntuan_gui._instance.m_zhiwu_t != 2) 
		{
			m_button_yijian.SetActive (true);
			m_button_save.SetActive (true);
			m_button_off.SetActive (false);
		} 
		else 
		{
			m_button_yijian.SetActive (false);
			m_button_save.SetActive (false);
			m_button_off.SetActive (true);
		}
		sys._instance.remove_child (juntuan_number_items);
//		juntuan_number_items.GetComponent<UIScrollView>().ResetPosition ();
		juntuan_number_items.transform.localPosition = new Vector3(367, 5, 0);
		juntuan_number_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		int num = 0;
		for (int i = 0; i < juntuan_gui._instance.m_guild_member_t.Count; i++)
		{
			if(juntuan_gui._instance.m_defend_type[i] == 5)
			{
				continue;
			}
			else if(juntuan_gui._instance.m_defend_type[i] == 0)
			{
				num0.Add(i);
				m_yijian.Add(i);
				continue;
			}

			int temp = juntuan_gui._instance.m_defend_type[i];
		
			if(m_index == 1)
			{
				//transform.Find("back/left_2").GetComponent<UIToggle>().value =true;
				if(temp != 1 )
				{
					continue;

				}
				m_yijian.Add(i);

			}
			else if(m_index == 2)
			{
				if(temp != 2  )
				{
					continue;
				}
				m_yijian.Add(i);
				
			}
			else if(m_index == 3)
			{
				if(temp != 3 )
				{
					continue;
				}
				m_yijian.Add(i);
				
			}
			else if(m_index == 4)
			{
				if(temp != 4  )
				{
					continue;
				}
				m_yijian.Add(i);
				
			}

			GameObject target = game_data._instance.ins_object_res("ui/juntuan/juntuan_number_item_ex");

			target.transform.Find("Toggle").GetComponent<UIToggle>().value =true;
			target.GetComponent<juntuan_number_item_ex>().curr_type = m_index; 
			
			target.GetComponent<juntuan_number_item_ex>().member_index = i;
			target.GetComponent<juntuan_number_item_ex>().member = juntuan_gui._instance.m_guild_member_t[i];
			target.transform.parent = juntuan_number_items.transform;
			target.transform.localPosition = new Vector3(-20,74 - num * 120,0);
			target.transform.localScale = Vector3.one;
			num++;
		}

		for (int j = 0; j < num0.Count; j++) 
		{
			GameObject target = game_data._instance.ins_object_res("ui/juntuan/juntuan_number_item_ex");
			target.GetComponent<juntuan_number_item_ex>().member_index = num0[j];
			target.GetComponent<juntuan_number_item_ex>().curr_type = m_index;
			target.GetComponent<juntuan_number_item_ex>().member = juntuan_gui._instance.m_guild_member_t[ num0[j]];
			target.GetComponent<juntuan_number_item_ex>().curr_type = m_index; 
			target.transform.parent = juntuan_number_items.transform;
			target.transform.localPosition = new Vector3(-20,74 - num * 120,0);
			target.transform.localScale = Vector3.one;
			num++;
		}


	}

	public void show_Ui()
	{

	}
	int ReturnTypeNum(int m_type)
	{
		int temp_num = 0;
		for (int i = 0; i < juntuan_gui._instance.m_defend_type.Count; i++)
		{
			if(juntuan_gui._instance.m_defend_type[i] == m_type)
			{
				temp_num++;
			}
		}
		return temp_num;
	}
	public void yijianDef()
	{
		if (m_index == 1) {

			for (int j = 0; j < m_yijian.Count; j++) 
			{
				juntuan_gui._instance.m_defend_type[m_yijian[j]] = 0;
			}

			for (int i = 0; i < 7; i++) 
			{
				if(i >= m_yijian.Count)
				{
					break;
				}
				juntuan_gui._instance.m_defend_type[m_yijian[i]] = 1;
				
			}
		} 
		else if (m_index == 2) 
		{
			for (int j = 0; j < m_yijian.Count; j++) 
			{
				juntuan_gui._instance.m_defend_type[m_yijian[j]] = 0;
			}

			for (int i = 0; i < 7; i++) 
			{
				if(i >= m_yijian.Count)
				{
					break;
				}

				juntuan_gui._instance.m_defend_type[m_yijian[i]] = 2;

			}
		}
		else if(m_index == 3)
		{
			for (int j = 0; j < m_yijian.Count; j++) 
			{
				juntuan_gui._instance.m_defend_type[m_yijian[j]] = 0;
			}

			for (int i = 0; i < 7; i++) 
			{
				if(i >= m_yijian.Count)
				{
					break;
				}

				juntuan_gui._instance.m_defend_type[m_yijian[i]] =3;

			}
		}	
		else if(m_index == 4)
		{
			for (int j = 0; j < m_yijian.Count; j++) 
			{
				juntuan_gui._instance.m_defend_type[m_yijian[j]] = 0;
			}

			for (int i = 0 ; i < 1; i++) 
			{
				if(i >= m_yijian.Count)
				{
					break;
				}
			
				juntuan_gui._instance.m_defend_type[m_yijian[i]] = 4;

			}
		}
		
		initItems ();
	}
	void IMessage.message (s_message message)
	{

	}
	void IMessage.net_message(s_net_message message)
	{

	}
	public void click(GameObject obj)
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
