
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class saodang_sub : MonoBehaviour {
	
	// Use this for initialization
	public protocol.game.smsg_mission_fight_end m_mission;
	public int m_num;

	void Start () {

	}

	public void init() {
        int exp = 0;
        int gold = 0;
        int yuanli = 0;
        for (int i = 0; i < m_mission.types.Count; i++)
        {
            if (m_mission.types[i] == 1)
            {
                if (m_mission.value1s[i] == 4)
                {
                    exp += m_mission.value2s[i];
                }
                else if (m_mission.value1s[i] == 1)
                {
                    gold += m_mission.value2s[i];
                }
                else if (m_mission.value1s[i] == 7)
                {
                    yuanli += m_mission.value2s[i];
                }
            }
        }
		this.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena_saodang_sub.cs_35_81"), m_num.ToString());//第{0}战
		this.transform.Find("exp").GetComponent<UILabel>().text = exp.ToString();
		this.transform.Find("gold").GetComponent<UILabel>().text = gold.ToString();
		this.transform.Find("yuanli").GetComponent<UILabel>().text = yuanli.ToString();
		int num = 0;
		for (int i = 0; i < m_mission.types.Count; ++i)
		{
			if (m_mission.types[i] == 1)
			{
				if (m_mission.value1s[i] == 4)
				{
					continue;
				}
				else if (m_mission.value1s[i] == 1)
				{
					continue;
				}
				else if (m_mission.value1s[i] == 7)
				{
					continue;
				}
			}
			num++;
			GameObject _icon = icon_manager._instance.create_reward_icon(m_mission.types[i], m_mission.value1s[i], m_mission.value2s[i], m_mission.value3s[i]);
			_icon.transform.parent = this.transform.Find("icon_" + num.ToString());
			_icon.transform.localPosition = new Vector3(0,0,0);
			_icon.transform.localScale = new Vector3(1,1,1);

			sys._instance.add_scale_anim(_icon,0.5f, 0, 1.2f, (num - 1) * 0.05f);
			sys._instance.add_scale_anim(_icon,0.1f, 1.2f, 1, (num - 1) * 0.05f + 0.3f);
			_icon.transform.localScale = new Vector3 (0, 0, 0);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
