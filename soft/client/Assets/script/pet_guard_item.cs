
using UnityEngine;
using System.Collections;

public class pet_guard_item : MonoBehaviour {

	public pet m_pet;
	public GameObject m_icon;
	public UILabel m_name;
	public UILabel m_hp;
	public UILabel m_attack;
	public UILabel m_mf;
	public UILabel m_wf;
	// Use this for initialization
	void Start () {
	
	}

	public void update_ui()
	{
		m_name.text = m_pet.get_color_name ();
		m_hp.text = m_pet.get_guard_attr (1).ToString("f0");
		m_attack.text = m_pet.get_guard_attr (2).ToString("f0");
		m_wf.text = m_pet.get_guard_attr (3).ToString("f0");
		m_mf.text = m_pet.get_guard_attr (4).ToString("f0");
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_pet_icon (m_pet.get_guid());
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "select")
		{
			s_message _message = new s_message();
			_message.m_type = "select_pet_guard";
			_message.m_long.Add(m_pet.get_guid());
			cmessage_center._instance.add_message(_message);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
