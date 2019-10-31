
using UnityEngine;
using System.Collections;

public class yueka_yulan : MonoBehaviour {

	public GameObject m_view; // 奖励物品的父对象
	public GameObject yulam_lab;
	private GameObject m_sign;
	// Use this for initialization
	void OnEnable () {
	//	reset (1);
	}
	public void click(GameObject obj)
	{
		if (obj.transform.name == "close") 
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}
	public void reset(int id,string name,bool flag = false)
	{
		if (!flag) {
			if (m_view.GetComponent<SpringPanel>() != null) {
				m_view.GetComponent<SpringPanel>().enabled = false;
			}
			//			m_view.transform.localPosition = new Vector3 (0, 0, 0);
			//			m_view.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
		}

		yulam_lab.GetComponent<UILabel>().text = name; //预览标题
		sys._instance.remove_child (m_view);
		if (id == 1) {
			dbc m_dbc_yueka_jijin = game_data._instance.m_dbc_yueka_jijin;
			GameObject gsign = null;
			for (int i = 0; i < m_dbc_yueka_jijin.get_y(); ++i) {
				gsign = game_data._instance.ins_object_res ("ui/yueka_jiangli");
				gsign.transform.parent = m_view.transform;
				gsign.transform.localPosition = new Vector3 (-415 + 106 * (i % 7), 165 - 120 * (i / 7));
				gsign.transform.localScale = new Vector3 (1, 1, 1);
				gsign.transform.GetComponent<yueka_sign>().m_index = i;
				gsign.transform.GetComponent<yueka_sign>().m_scro = m_view;
				gsign.transform.GetComponent<yueka_sign>().init(id,3,0);
				
			}
		} else if (id == 2) {
			dbc m_dbc_yueka_jijin = game_data._instance.m_dbc_yueka_jijin;
			GameObject gsign = null;
			for (int i = 0; i < m_dbc_yueka_jijin.get_y(); ++i) {
				
				gsign = game_data._instance.ins_object_res ("ui/yueka_jiangli");
				gsign.transform.parent = m_view.transform;
				gsign.transform.localPosition = new Vector3 (-415 + 106 * (i % 7), 165 - 120 * (i / 7));
				gsign.transform.localScale = new Vector3 (1, 1, 1);
				gsign.transform.GetComponent<yueka_sign>().m_index = i;
				gsign.transform.GetComponent<yueka_sign>().m_scro = m_view;
				gsign.transform.GetComponent<yueka_sign>().init(id,3,0);
				
			}
		}
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
