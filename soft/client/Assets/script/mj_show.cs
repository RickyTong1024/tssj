
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mj_show : MonoBehaviour,IMessage {

	public List<GameObject> m_roots = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
		//m_root = this.transform.Find("root").gameObject;

		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "mj_show_unit")
		{
			for(int i = 0;i < m_roots.Count;i ++)
			{
				sys._instance.remove_child(m_roots[i]);
			}

			for(int i = 0;i < message.m_ints.Count / 3;i ++)
			{
				GameObject _unit = sys._instance.create_class((int)message.m_ints[i * 3], 0, 0, m_roots[i]);

				if((int)message.m_ints[i * 3 + 1] > 0)
				{
					_unit.GetComponent<unit>().set_bh(2.0f);
				}

				for(int c = 1;c < 4;c ++)
				{
					GameObject _star = m_roots[i].transform.parent.Find("xhmj_starg").gameObject;

					_star = _star.transform.Find(c.ToString()).Find("star").gameObject;

					if((int)message.m_ints[i * 3 + 1] >= c)
					{
						_star.SetActive(true);
					}
					else
					{
						_star.SetActive(false);
					}
				}

				for(int c = 1;c < 4;c ++)
				{
					GameObject _num = m_roots[i].transform.parent.Find("xhmj_starg").Find("xhmj_stardb00" + c.ToString()).gameObject;
					
					if((int)message.m_ints[i * 3 + 2] == c)
					{
						_num.SetActive(true);
					}
					else
					{
						_num.SetActive(false);
					}
				}
			}
		}
	}

	void IMessage.net_message(s_net_message message)
	{

	}
	void Update () {
	
	}
}
