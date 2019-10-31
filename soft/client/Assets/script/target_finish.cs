
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class target_finish : MonoBehaviour {

	private float m_time = 0;
	public int m_id;
    public int m_type;
	public GameObject m_reward;
	public GameObject m_reward1;
	bool m_is_play = false;

	void Start () {

	}

	public void reset()
	{
        if (m_type == 0)
        {
            this.gameObject.SetActive(true);
            sys._instance.play_sound("sound/cj");
            m_time = 3;
            s_t_target t_target = game_data._instance.get_t_target(m_id);
            transform.Find("icon").GetComponent<UISprite>().spriteName = t_target.icon;
            transform.Find("text").GetComponent<UILabel>().text = t_target.desc;
            if (t_target.reward.type == 1 && sys._instance.get_res_samall_icon(t_target.reward.value1) != "")
            {
                m_reward.SetActive(true);
                m_reward1.SetActive(false);
                m_reward.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(t_target.reward.value1);
                m_reward.transform.Find("reward").GetComponent<UILabel>().text = sys._instance.get_res_color(t_target.reward.value1) + "x" + t_target.reward.value2.ToString();
            }
            else
            {
                m_reward.SetActive(false);
                m_reward1.SetActive(true);
				m_reward1.GetComponent<UILabel>().text = sys._instance.get_res_info (t_target.reward.type, t_target.reward.value1, t_target.reward.value2, t_target.reward.value3);
            }
            m_is_play = true;
            transform.Find("effect").gameObject.SetActive(true);
            transform.Find("effect").GetComponent<UISpriteAnimation>().ResetToBeginning();
 
        }
        else if (m_type == 1)
        {
           
 
        }
		
	}

	void Update () {
		if (m_time > 0)
		{
			m_time -= Time.deltaTime;
			if (m_time <= 0)
			{
				transform.GetComponent<ui_show_anim>().hide_ui();
			}
		}
		if (m_is_play)
		{
			if(!transform.Find("effect").GetComponent<UISpriteAnimation>().isPlaying)
			{
				m_is_play = false;
				transform.Find("effect").gameObject.SetActive(false);
			}

		}
	}
}
