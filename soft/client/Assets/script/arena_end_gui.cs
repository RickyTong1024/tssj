using UnityEngine;
using System.Collections.Generic;

public class arena_end_gui : MonoBehaviour {

	public List<UISprite> m_sprits;
	public int m_index;
	public int m_index1;
	public List<int> m_rewards = new List<int>();
    bool m_flag = false;
	GameObject curren;
	List<GameObject> m_re_ne ;
	List<int> m_rews;
	public GameObject _back;
	public List<UISprite> m_texiao;
	public GameObject shell;
	public GameObject m_label;
	public GameObject m_label1;
	bool m_isfinshed = false;
    public string m_scence = "";

    public int gold = 0;
    public void finshed_animation()
    {
        m_label.SetActive(true);
        for (int i = 0; i < m_texiao.Count; i++)
        {
            m_texiao[i].gameObject.SetActive(true);
        }
        m_flag = true;

    }

    void click(GameObject obj)
    {
        if (m_flag)
        {
            if (obj.name == "back")
            {
                return;
            }
            for (int i = 0; i < m_texiao.Count; i++)
            {
                m_texiao[i].gameObject.SetActive(false);
            }
            m_flag = false;
            this.GetComponent<Animator>().enabled = false;
            curren = obj;
            m_index1 = int.Parse(obj.name);
            TweenRotation _ro = curren.AddComponent<TweenRotation>();
            _ro.from = curren.transform.localRotation.eulerAngles;
            _ro.to = new Vector3(0, 90, 0);
            _ro.duration = 0.5f;
            _ro.enabled = true;
            _ro.AddOnFinished(finshed);
            m_re_ne = new List<GameObject>();
            for (int i = 0; i < m_sprits.Count; i++)
            {
                if (i == m_index1)
                {
                    continue;
                }
                m_re_ne.Add(m_sprits[i].gameObject);
            }
            m_rews = new List<int>();
            for (int i = 0; i < m_rewards.Count; i++)
            {
                if (i == m_index)
                {
                    continue;
                }
                m_rews.Add(m_rewards[i]);
            }
        }
        else
        {
            if (m_isfinshed)
            {
                if (obj.name == "back")
                {
                    if (m_scence == "ts_game_duobao")
                    {
                        sys._instance.m_game_state = "bao_wu";
                        s_message _msg2 = new s_message();
                        _msg2.m_type = "show_hecheng";
                        cmessage_center._instance.add_message(_msg2);
                    }
                    else
                    {
                        sys._instance.m_game_state = "hall";
                    }

                    s_message _msg = new s_message();
                    _msg.m_type = "load_scene";
                    _msg.m_string.Add(m_scence);
                    Time.timeScale = 1;
                    s_message _msg1 = new s_message();
                    _msg1.m_type = "arena_look";
                    _msg.m_next = _msg1;
                    cmessage_center._instance.add_message(_msg);
                    Object.Destroy(this.gameObject);
                }
            }
        }
    }

	void finshed()
	{
		curren.GetComponent<UISprite>().spriteName = "bwsp_ck002";
		s_t_sport_card _re = game_data._instance.get_t_sport_card (m_rewards [m_index]);
        GameObject _reward = null;
        if (_re.type == 1 && _re.value1 == 1)
        {
            _reward = icon_manager._instance.create_reward_icon_ex(_re.type, _re.value1, gold, _re.value3);
        }
        else
        {
            _reward = icon_manager._instance.create_reward_icon_ex(_re.type, _re.value1, _re.value2, _re.value3);
        }
		TweenRotation _ro = curren.AddComponent<TweenRotation>();
		_ro.from = curren.transform.localRotation.eulerAngles;
		_ro.to = new Vector3(0,0,0);
		_ro.duration = 0.5f;
		_ro.enabled = true;
		_ro.AddOnFinished(finshed1);
		_reward.transform.parent = curren.transform.Find("icon").transform;
		curren.transform.Find("icon").gameObject.SetActive (true);
		curren.transform.Find("text").gameObject.SetActive (true);
        if (_re.type == 1 && _re.value1 == 1)
        {
            curren.transform.Find("text").GetComponent<UILabel>().text = sys._instance.get_res_info(_re.type, _re.value1, 0, _re.value3);
        }
        else
        {
            curren.transform.Find("text").GetComponent<UILabel>().text = sys._instance.get_res_info(_re.type, _re.value1, 0, _re.value3);
        }
		
		_reward.transform.localPosition = Vector3.zero;
		_reward.transform.localEulerAngles = new Vector3(0,0,0);
		_reward.transform.localScale = Vector3.one;
		curren.transform.Find("effect").gameObject.SetActive (true);
		if(curren.GetComponent<TweenRotation>() != null)
		{
			DestroyImmediate(curren.GetComponent<TweenRotation>());
		}
	}

    void finshed1()
    {
        Invoke("back", 0.5f);
    }

    void back()
    {
        for (int i = 0; i < m_rews.Count; i++)
        {
            TweenRotation _ro = m_re_ne[i].AddComponent<TweenRotation>();
            _ro.from = m_re_ne[i].transform.localEulerAngles;
            _ro.to = new Vector3(0, 90, 0);
            _ro.duration = 0.5f;
            _ro.enabled = true;
            if (i == 0)
            {
                _ro.AddOnFinished(finshed2);
            }
        }
    }

    void finshed2()
    {
        for (int i = 0; i < m_rews.Count; i++)
        {
            s_t_sport_card _re = game_data._instance.get_t_sport_card(m_rews[i]);
            GameObject _reward = null;
            if (_re.type == 1 && _re.value1 == 1)
            {
                _reward = icon_manager._instance.create_reward_icon_ex(_re.type, _re.value1, gold, _re.value3);
            }
            else
            {
                _reward = icon_manager._instance.create_reward_icon_ex(_re.type, _re.value1, _re.value2, _re.value3);
            }
            _reward.transform.parent = m_re_ne[i].transform.Find("icon").transform;
            _reward.transform.localPosition = Vector3.zero;
            _reward.transform.localEulerAngles = new Vector3(0, 0, 0);
            _reward.transform.localScale = Vector3.one;
            m_re_ne[i].transform.Find("text").GetComponent<UILabel>().text = sys._instance.get_res_info(_re.type, _re.value1, 0, _re.value3);
            m_re_ne[i].transform.Find("icon").gameObject.SetActive(true);
            m_re_ne[i].GetComponent<UISprite>().spriteName = "bwsp_ck002";
            if (m_re_ne[i].GetComponent<TweenRotation>() != null)
            {
                DestroyImmediate(m_re_ne[i].GetComponent<TweenRotation>());
            }
            TweenRotation _ro = m_re_ne[i].AddComponent<TweenRotation>();
            _ro.from = m_re_ne[i].transform.localEulerAngles;
            _ro.to = new Vector3(0, 0, 0);
            _ro.duration = 0.5f;
            _ro.enabled = true;
            if (i == 1)
            {
                _ro.AddOnFinished(finshed3);
            }
        }
    }

    void finshed3()
    {
        m_isfinshed = true;
        m_label.SetActive(false);
        m_label1.SetActive(true);
    }
}
