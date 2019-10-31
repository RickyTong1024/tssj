
using UnityEngine;
using System.Collections;

public class explore_qiyu_item : MonoBehaviour {

	public protocol.game.tansuo_event m_event;
	public GameObject m_name;
	public GameObject m_time;
	public GameObject m_select;
	// Use this for initialization
	void Start () {
	
	}

	void OnDisable()
	{
		CancelInvoke ("time");
	}
	
	void time()
	{
		long _time = (long)(m_event.qiyu_time - timer.now());
		m_time.GetComponent<UILabel>().text = timer.get_time_show(_time);
		if(_time <= 0)
		{
			m_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chongzhifali_gui.cs_68_42");//活动已结束
			CancelInvoke ("time");
		}
	}

	void OnEnable()
	{
		InvokeRepeating ("time", 0.0f, 1.0f);
	}

	public void reset()
	{
		s_t_manyou t_manyou = game_data._instance.get_t_manyou (m_event.ts_id);
		m_name.GetComponent<UILabel>().text = t_manyou.name;
		this.transform.GetComponent<UISprite>().spriteName = t_manyou.image;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
