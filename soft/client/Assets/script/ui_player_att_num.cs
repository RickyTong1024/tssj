
using UnityEngine;
using System.Collections;

public class ui_player_att_num : MonoBehaviour {

	public GameObject m_label;

	public int m_att_type = 0;

	public float m_old_value = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		float _value = sys._instance.m_self.get_att((e_player_attr)m_att_type);

		if(m_old_value != _value)
		{
			m_old_value = _value;
			m_label.GetComponent<UILabel>().text = ((int)_value).ToString();
		}

	}
}
