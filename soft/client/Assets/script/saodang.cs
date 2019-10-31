
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class saodang : MonoBehaviour {

	public GameObject m_view;
	public List<protocol.game.smsg_mission_fight_end> m_saodangs;
	private float m_time;
	private int m_index;
	private int m_len;
	private int m_mission_id;
	public GameObject m_ok;
	public UILabel m_name_Label;
	public UILabel m_sure_Label;
    public UILabel m_desc;
    public int m_id;
    public int m_num = 0;
	// Use this for initialization
	void Start () {

	}

	void OnFinished()
	{
        m_id = 0;
        m_num = 0;
	}

	public void init(int mission_id, List<protocol.game.smsg_mission_fight_end> saodangs)
	{
		m_mission_id = mission_id;
		s_t_mission _mission = game_data._instance.get_t_mission (mission_id);
		m_saodangs = saodangs;
		this.gameObject.SetActive (true);
		m_time = 0.5f;
		m_index = -1;
		m_len = 0;

		this.transform.Find("back").Find("hide_box").Find("hide").GetComponent<BoxCollider>().enabled = false;
		this.transform.Find("back").Find("hide_box").Find("hide").GetComponent<UISprite>().alpha = 0.5f;
		m_ok.GetComponent<BoxCollider>().enabled = false;
		m_ok.GetComponent<UISprite>().set_enable(false);
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		sys._instance.remove_child (m_view.gameObject);
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        show_suipian();
        show_suipian_num();
	}
    void show_suipian()
    {
		m_id = 0;
        s_t_mission _mission = game_data._instance.get_t_mission(m_mission_id);
        m_desc.text = "";
        for (int i = 0; i < _mission.items.Count; i++)
        {

            if ((_mission.items[i].reward.type == 2) && 
                (sys._instance.m_self.is_card_fragment((uint)_mission.items[i].reward.value1) || 
                sys._instance.m_self.is_equip_fragment((uint)_mission.items[i].reward.value1) || 
                game_data._instance.get_item(_mission.items[i].reward.value1).type == 4003))
            {
                int num = 0;
                for (int j = 0; j < m_saodangs.Count; j++)
                {
                    for (int k = 0; k < m_saodangs[j].types.Count; k++)
                    {
                        if(m_saodangs[j].types[k] == _mission.items[i].reward.type && _mission.items[i].reward.value1 ==  m_saodangs[j].value1s[k])
                        {
                            num += m_saodangs[j].value2s[k];
                        }
 
                    }
                }
                m_id = _mission.items[i].reward.value1;
                m_num = sys._instance.m_self.get_item_num((uint)m_id) - num;
               
                break;
            }
        }
 
    }
    void show_suipian_num()
    {
		if (m_id == 0)
        {
            m_desc.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            m_desc.transform.parent.gameObject.SetActive(true);
            if (game_data._instance.get_item(m_id).type != 4003)
            {
                m_desc.text = game_data._instance.get_item(m_id).name +
          " : " + m_num + "/"
          + game_data._instance.get_item(m_id).def_2;

            }
            else
            {
                m_desc.text = game_data._instance.get_item(m_id).name +
                 " : " + m_num;
 
            }
           
        }
       
    }
	public void click(GameObject obj)
	{
		if (obj.name == "hide" || obj.name == "sao_close")
		{
			s_message _msg = new s_message();
			_msg.m_type = "mission_lock";
			_msg.m_ints.Add(m_mission_id);
			cmessage_center._instance.add_message(_msg);

			s_message _message = new s_message();
			_message.m_type = "select_map";
			cmessage_center._instance.add_message(_message);

			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	void deal()
	{
		int tnum = m_saodangs[m_index].types.Count - 3;
		int lens = 273;
		string s = "ui/saodang_sub";
		if (tnum == 0)
		{
			lens = 120;
			s = "ui/saodang_sub2";
		}
		else if (tnum <= 4)
		{
			lens = 175;
			s = "ui/saodang_sub1";
		}

		GameObject saodang_sub = game_data._instance.ins_object_res(s);
		saodang_sub.transform.parent = m_view.transform;
		saodang_sub.transform.localPosition = new Vector3(0, 172 - m_len, 0);
		saodang_sub.transform.localScale = new Vector3(1, 1, 1);
		saodang_sub.transform.GetComponent<saodang_sub>().m_mission = m_saodangs[m_index];
		saodang_sub.transform.GetComponent<saodang_sub>().m_num = m_index + 1;
		saodang_sub.transform.GetComponent<saodang_sub>().init ();
        int num = m_num;
        for (int i = 0; i < m_saodangs[m_index].types.Count; i++)
        {
            if (m_saodangs[m_index].types[i] == 2 && m_saodangs[m_index].value1s[i] == m_id)
            {
                m_num += m_saodangs[m_index].value2s[i];
            }
        }
        if (num != m_num)
        {
            show_suipian_num();
            top_res.add_scale_anim(m_desc.gameObject);
 
        }
       
        if (tnum == 0)
        {
            string text = game_data._instance.get_t_language ("saodang.cs_170_26");//本次扫荡未获得物品
            saodang_sub.transform.Find("end_Label").gameObject.GetComponent<UILabel>().text = text;
        }
		m_len += lens;
		int y = 370 - m_len;
		if (y < 0)
		{
			SpringPanel.Begin(m_view.GetComponent<UIScrollView>().panel.cachedGameObject,
		                  new Vector3(0, -y, 0), 8.0f).onFinished = OnFinished;
		}
		m_time = 0.5f;
	}

	void end()
	{
		GameObject saodang_sub = game_data._instance.ins_object_res("ui/saodang_sub3");
		saodang_sub.GetComponent<UISprite>().spriteName = "sdwc_word";
		saodang_sub.transform.parent = m_view.transform;
		saodang_sub.transform.localPosition = new Vector3(0, 172 - m_len, 0);
		saodang_sub.transform.localScale = new Vector3(1, 1, 1);

		int y = 370 - m_len - 90;
		if (y < 0)
		{
			SpringPanel.Begin(m_view.GetComponent<UIScrollView>().panel.cachedGameObject,
			                  new Vector3(0, -y, 0), 8.0f).onFinished = OnFinished;
		}

		this.transform.Find("back").Find("hide_box").Find("hide").GetComponent<BoxCollider>().enabled = true;
		this.transform.Find("back").Find("hide_box").Find("hide").GetComponent<UISprite>().alpha = 1.0f;
		m_ok.GetComponent<BoxCollider>().enabled = true;
		m_ok.GetComponent<UISprite>().set_enable(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (m_saodangs == null)
		{
			return;
		}
		if (m_index >= m_saodangs.Count)
		{
			return;
		}
		if (m_time > 0)
		{
			m_time -= Time.deltaTime;
			if (m_time <= 0)
			{
				m_index++;
				if (m_index >= m_saodangs.Count)
				{
					end();
					return;
				}
				deal();
			}
		}
	}
}
