
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class qiang_duo_saodang : MonoBehaviour {

	public GameObject m_view;
    List<protocol.game.smsg_treasure_fight_end> m_rewards;
	public GameObject m_close;
	public GameObject m_close1;
	private float m_time;
	private int m_index;
	private int m_len;
	// Use this for initialization
	
	void OnFinished()
	{
		
	}

    public void init(List<protocol.game.smsg_treasure_fight_end> rewards)//List<int> golds,List<int> powders,List<int> suipians
	{
		m_rewards = rewards;
		
		m_close.GetComponent<BoxCollider>().enabled = false;
		m_close.GetComponent<UISprite>().alpha = 0.5f;
		m_close1.GetComponent<BoxCollider>().enabled = false;
		m_close1.GetComponent<UISprite>().alpha = 0.5f;
		
		if (m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_view);
		
		m_time = 0.5f;
		m_index = -1;
		m_len = 0;
	}

	public void click(GameObject obj)
	{
		if (obj.name == "close" || obj.name == "hide")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

    void deal()
    {
	    GameObject obj = game_data._instance.ins_object_res("ui/qiangduo_saodang_sub");
	    obj.transform.parent = m_view.transform;
	    obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = new Vector3(0, 93 - m_index * 158,0);
		obj.GetComponent<qiangduo_saodang_sub>().reward = m_rewards[m_index];
		obj.GetComponent<qiangduo_saodang_sub>().m_num = m_index + 1;
		obj.GetComponent<qiangduo_saodang_sub>().init();
		
		m_len += 158;
		int y = 372 - m_len;
		if (y < 0)
		{
			SpringPanel.Begin(m_view.GetComponent<UIScrollView>().panel.cachedGameObject,
			                  new Vector3(0, -y, 0), 8.0f).onFinished = OnFinished;
		}
		
		m_time = 0.5f;
    }

	void end()
	{
		m_close.GetComponent<BoxCollider>().enabled = true;
		m_close.GetComponent<UISprite>().alpha = 1.0f;
		m_close1.GetComponent<BoxCollider>().enabled = true;
		m_close1.GetComponent<UISprite>().alpha = 1.0f;
	}

	void Update () {
		if (m_index >= m_rewards.Count - 1)
		{
			return;
		}
		if (m_time > 0)
		{
			m_time -= Time.deltaTime;
			if (m_time <= 0)
			{
				m_index++;
				deal();
				if (m_index >= m_rewards.Count - 1)
				{
					end();
					return;
				}
			}
		}
	}
}
