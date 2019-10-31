
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ttt_saodang : MonoBehaviour {
	public List<protocol.game.smsg_ttt_fight_end> m_rewards;
    public GameObject m_view;
	public GameObject m_close;
	private float m_time;
	private int m_index;
	private int m_len;
	private int m_type;
	private int m_zt;
	// Use this for initialization
	void Start () {
	
	}

	void OnFinished()
	{
		
	}

	void OnDisable()
	{
		s_message _msg = new s_message();
		_msg.m_type = "reset_mijing_gui";
		cmessage_center._instance.add_message (_msg);
	}

    public void init(List<protocol.game.smsg_ttt_fight_end> rewards, int type)
    {
		m_type = type;
		m_rewards = rewards;
		
		m_close.GetComponent<BoxCollider>().enabled = false;
		m_close.GetComponent<UISprite>().alpha = 0.5f;

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
		m_zt = 0;
    }

	void deal()
    {
	    int gold = 0;
	    int hj = 0;
		for (int j = 0; j < m_rewards[m_index].types.Count; j++)
	    {
			if (m_rewards[m_index].value1s[j] == 1)
            {
				gold += m_rewards[m_index].value2s[j];
            }
			if (m_rewards[m_index].value1s[j] == 6)
            {
				hj += m_rewards[m_index].value2s[j];
            }
        }
	    GameObject obj = game_data._instance.ins_object_res("ui/ttt_saodang_sub");
	    obj.transform.parent = m_view.transform;
	    obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = new Vector3(0, 130 - m_len, 0);

		Transform tf = obj.transform.Find("sub1");
		tf.gameObject.SetActive (true);
		tf.Find("gold").GetComponent<UILabel>().text = game_data._instance.get_t_resource(1).namecolor + gold + baoji(m_rewards[m_index].baoji1);
		tf.Find("exp").GetComponent<UILabel>().text = game_data._instance.get_t_resource(6).namecolor + hj + baoji(m_rewards[m_index].baoji2);
		obj.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("boss.cs_234_75"), m_rewards[m_index].index );//第{0}关

		m_len += 140;
		int y = 420 - m_len;
		if (y < 0)
		{
			SpringPanel.Begin(m_view.GetComponent<UIScrollView>().panel.cachedGameObject,
			                  new Vector3(0, -y, 0), 8.0f).onFinished = OnFinished;
		}
		
		m_time = 0.5f;
    }

	void end()
	{
		GameObject obj1 = game_data._instance.ins_object_res("ui/ttt_saodang_sub");
		obj1.transform.parent = m_view.transform;
		obj1.transform.localScale = Vector3.one;
		obj1.transform.localPosition = new Vector3(0, 130 - m_len, 0);

		Transform tf = obj1.transform.Find("sub2");
		tf.gameObject.SetActive (true);
		GameObject icon1 = tf.Find("icon1").gameObject;
		GameObject icon2 = tf.Find("icon2").gameObject;
		GameObject icon3 = tf.Find("icon3").gameObject;
        sys._instance.remove_child(icon1);
        sys._instance.remove_child(icon2);
        sys._instance.remove_child(icon3);

		obj1.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language ("ttt_saodang.cs_105_66");//宝箱奖励
		GameObject obj2 = icon_manager._instance.create_reward_icon(m_rewards[m_index].types[0],m_rewards[m_index].value1s[0],m_rewards[m_index].value2s[0],m_rewards[m_index].value3s[0]);
		obj2.transform.parent = icon1.transform;
		obj2.transform.localPosition = Vector3.zero;
		obj2.transform.localScale = Vector3.one;
        if (m_rewards[m_index].types.Count > 1)
        {
            obj2 = icon_manager._instance.create_reward_icon(m_rewards[m_index].types[1], m_rewards[m_index].value1s[1], m_rewards[m_index].value2s[1], m_rewards[m_index].value3s[1]);
            obj2.transform.parent = icon2.transform;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.localScale = Vector3.one;
        }
		if (m_rewards[m_index].types.Count > 2)
		{
			obj2 = icon_manager._instance.create_reward_icon(m_rewards[m_index].types[2], m_rewards[m_index].value1s[2], m_rewards[m_index].value2s[2], m_rewards[m_index].value3s[2]);
			obj2.transform.parent = icon3.transform;
			obj2.transform.localPosition = Vector3.zero;
			obj2.transform.localScale = Vector3.one;
		}
        if (m_rewards[m_index].types.Count == 1)
        {
            icon1.transform.localPosition = Vector3.zero;
        }
		else if (m_rewards[m_index].types.Count == 2)
		{
			icon1.transform.localPosition = new Vector3(-50, 0, 0);
			icon2.transform.localPosition = new Vector3(50, 0, 0);
		}

		m_len += 140;
		int y = 420 - m_len;
		if (y < 0)
		{
			SpringPanel.Begin(m_view.GetComponent<UIScrollView>().panel.cachedGameObject,
			                  new Vector3(0, -y, 0), 8.0f).onFinished = OnFinished;
		}

		m_close.GetComponent<BoxCollider>().enabled = true;
		m_close.GetComponent<UISprite>().alpha = 1.0f;

		m_time = 0.5f;
	}

	void end1()
	{
		s_t_ttt_value t_ttt_value = game_data._instance.get_t_ttt_value (m_rewards[m_index].nd);

		GameObject obj = game_data._instance.ins_object_res("ui/ttt_saodang_sub");
		obj.transform.parent = m_view.transform;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = new Vector3(0, 130 - m_len, 0);
		
		Transform tf = obj.transform.Find("sub3");
		tf.gameObject.SetActive (true);
		tf.Find("jc").GetComponent<UILabel>().text = game_data._instance.get_value_string(t_ttt_value.sxtype, t_ttt_value.sxvalue);
		tf.Find("xin").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("ttt_saodang.cs_160_67"),t_ttt_value.xh );//{0}星
		obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language ("ttt_saodang.cs_161_65");//加成选择
		
		m_len += 140;
		int y = 420 - m_len;
		if (y < 0)
		{
			SpringPanel.Begin(m_view.GetComponent<UIScrollView>().panel.cachedGameObject,
			                  new Vector3(0, -y, 0), 8.0f).onFinished = OnFinished;
		}
		
		m_time = 0.5f;
		m_close.GetComponent<BoxCollider>().enabled = true;
		m_close.GetComponent<UISprite>().alpha = 1.0f;
	}

    string baoji(int i)
    {
        string s = "";
        if (i == 1)
        {
            s = game_data._instance.get_t_language ("ttt_saodang.cs_181_16");// [2fa4ff](暴击)
        }
		else if (i == 2)
        {
            s = game_data._instance.get_t_language ("ttt_saodang.cs_185_16");// [ff00ff](大暴击)
        }
		else if (i == 3)
        {
            s = game_data._instance.get_t_language ("ttt_saodang.cs_189_16");// [ff6100](幸运暴击)
        }
        return s;

    }
    
	// Update is called once per frame
	void Update () {
		if (m_index >= m_rewards.Count - 1 && m_zt == 0)
		{
            m_close.GetComponent<BoxCollider>().enabled = true;
            m_close.GetComponent<UISprite>().alpha = 1.0f;
			return;
		}
		if (m_time > 0)
		{
			m_time -= Time.deltaTime;
			if (m_time <= 0)
			{
				if (m_zt == 0)
				{
					m_index++;
				}
				if (m_rewards[m_index].index != 0)
				{
					deal();
				}
				else
				{
					if (m_zt == 0)
					{
						end();
						m_zt = 1;
					}
					else if (m_type == 1)
					{
						end1();
						m_zt = 0;
					}
					else
					{
						m_zt = 0;
					}
				}
				if (m_index >= m_rewards.Count - 1)
				{
					return;
				}
			}
		}
	}
}
