
using UnityEngine;
using System.Collections;

public class player_mp : MonoBehaviour,IMessage {

	public GameObject m_ui_mp_up;
	public GameObject m_ui_mp_back;
	public GameObject[] m_tags;
	private float m_mp_cur = 0.0f;
	private float m_mp_target = 0.0f;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void OnEnable()
	{
		m_mp_cur = 0;
		
	}
	void IMessage.net_message(s_net_message message)
	{
		
	}
	void IMessage.message(s_message message)
	{
	}
	// Update is called once per frame
	void Update () {
	
		float _max_hp = 400.0f;
		float _add = Time.deltaTime * 50;

		m_mp_target = (float)sys._instance.m_self.get_att (e_player_attr.player_mp);

		if(Mathf.Abs(m_mp_cur - m_mp_target) > _add)
		{
			if(m_mp_cur > m_mp_target)
			{
				m_mp_cur -= _add;
			}
			else if(m_mp_cur < m_mp_target)
			{
				m_mp_cur += _add;
			}
		}
		else
		{
			m_mp_cur = m_mp_target;
		}
		m_ui_mp_back.GetComponent<UISprite>().fillAmount = m_mp_target / _max_hp;
		m_ui_mp_up.GetComponent<UISprite>().fillAmount = m_mp_cur / _max_hp;

		for(int i = 0;i < m_tags.Length;i ++)
		{
			if(m_ui_mp_up.GetComponent<UISprite>().fillAmount >= 0.25f * (i + 1))
			{
				m_tags[i].SetActive(true);
			}
			else
			{
				m_tags[i].SetActive(false);
			}
		}

	}
}
