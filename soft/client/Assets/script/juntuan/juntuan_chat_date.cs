using UnityEngine;

public class juntuan_chat_date : MonoBehaviour {

	public protocol.game.smsg_chat m_scs;
	// Use this for initialization
	void Start () {
	
	}

	public void init()
	{
		System.DateTime dt = timer.time2dt(m_scs.time);
		this.transform.Find("time").GetComponent<UILabel>().text = dt.ToString ("D");
		this.transform.Find("time").GetComponent<UILabel>().text = dt.ToString ("HH:mm:ss");
		this.transform.Find("name").GetComponent<UILabel>().text = "[00ff00]" + m_scs.player_name;
		this.transform.Find("text").GetComponent<UILabel>().text = m_scs.color + m_scs.text;
		
		if (m_scs.type == 2 && m_scs.player_guid == sys._instance.m_self.m_guid)
		{
			this.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("juntuan_chat_date.cs_22_83") , m_scs.target_name);//发送给 [00ff00]{0}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
