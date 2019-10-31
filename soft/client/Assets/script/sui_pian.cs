
using UnityEngine;
using System.Collections;

public class sui_pian : MonoBehaviour {

	public uint m_id;
	public GameObject m_icon;
	public GameObject m_name;
	public GameObject m_button;
	private s_t_item m_t_item;
	private int m_num;

	// Use this for initialization
	void Start () {

	}
	public void click()
	{
		if (m_num < m_t_item.def_2)
		{
			string s = game_data._instance.get_t_language ("card_page_item.cs_48_59");//碎片不足
			root_gui._instance.show_prompt_dialog_box("[ffc882]" + s);
			return ;
		}

		s_message _message = new s_message ();
		_message.m_type = "sui_pian_he_cheng";
		_message.m_ints.Add ((int)m_id);
		cmessage_center._instance.add_message (_message);
	}
	public void init()
	{
		m_id = 0;
	}
	public void reset()
	{
		m_t_item = game_data._instance.get_item ((int)m_id);

		m_num = sys._instance.m_self.get_item_num (m_id);
		if (m_num >= m_t_item.def_2)
		{
			m_name.GetComponent<UILabel>().text = m_t_item.name + "[ffff00]" + m_num + "[-]/" + m_t_item.def_2;
			m_button.GetComponent<Collider>().enabled = true;
			m_button.GetComponent<UISprite>().alpha = 1.0f;
		}
		else
		{
			m_name.GetComponent<UILabel>().text = m_t_item.name + "[00ff00]" + m_num + "[-]/" + m_t_item.def_2;
			m_button.GetComponent<Collider>().enabled = true;
			m_button.GetComponent<UISprite>().alpha = 1.0f;
		}

		GameObject _icon = icon_manager._instance.create_item_icon ((int)m_id);
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.GetComponent<BoxCollider>().enabled = false;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
