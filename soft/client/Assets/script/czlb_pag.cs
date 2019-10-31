using UnityEngine;
using System.Collections;

public class czlb_pag : MonoBehaviour, IMessage{

    public GameObject m_view;
    public GameObject m_num;
    public GameObject m_time;
    public GameObject m_title;
    public GameObject m_title1;
    public int m_huodong_id;
    public static czlb_pag _instance;
    private int m_id;
    public static ulong m_end_time;
    protocol.game.smsg_huodong_reward_view _msg;
    // Use this for initialization


    void Awake()
    {

        _instance = this;
        cmessage_center._instance.add_handle(this);
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    void OnEnable()
    {

        InvokeRepeating("time", 0.0f, 1.0f);
        protocol.game.cmsg_huodong_reward_view _msg = new protocol.game.cmsg_huodong_reward_view();
        _msg.id = m_huodong_id;
        net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view>(opclient_t.CMSG_HUODONG_ZHICHONG_VIEW, _msg);

    }
    public void init(bool resert)
    {
        if (resert || this.gameObject.activeSelf)
        {
            protocol.game.cmsg_huodong_reward_view _msg = new protocol.game.cmsg_huodong_reward_view();
            _msg.id = m_huodong_id;
            net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view>(opclient_t.CMSG_HUODONG_ZHICHONG_VIEW, _msg);
        }

    }
    void OnDisable()
    {
        CancelInvoke("time");

        sys._instance.remove_child(m_view);

    }

    public void click(GameObject obj)
    {
        if (obj.transform.name == "close")
        {
            s_message _message = new s_message();
            _message.m_type = "show_jc_huodong";
            cmessage_center._instance.add_message(_message);

        }
    }

    void time()
    {
        m_end_time = 0;
        for (int i = 0; i < sys._instance.m_self.m_huodong_ids.Count; ++i)
        {
            if (m_huodong_id == sys._instance.m_self.m_huodong_ids[i])
            {
                m_end_time = sys._instance.m_self.m_end_time[i];
                break;
            }
        }
        long _time = (long)(m_end_time - timer.now());
        if (_time <= 0)
        {
            m_time.GetComponent<UILabel>().text = game_data._instance.get_t_language("chongzhifali_gui.cs_68_42");//活动已结束
        }
        else
        {
            m_time.GetComponent<UILabel>().text = timer.get_time_show_ex(_time);
        }
    }

    public void reset()
    {
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);
        _msg.subs.Sort(comp);
        for (int i = 0; i < _msg.subs.Count; ++i)
        {

            GameObject _obj = game_data._instance.ins_object_res("ui/czlb_sub");

            _obj.transform.parent = m_view.transform;
            _obj.transform.localScale = new Vector3(1, 1, 1);
            _obj.transform.localPosition = new Vector3(0, 106 - i * 154, 1);
            _obj.GetComponent<czlb_sub>().m_id = _msg.subs[i].id;
            _obj.GetComponent<czlb_sub>().main_id = m_huodong_id;
            _obj.GetComponent<czlb_sub>().m_rid = _msg.subs[i].arg1;
            _obj.GetComponent<czlb_sub>().m_can_get = _msg.subs[i].arg2;
            _obj.GetComponent<czlb_sub>().m_limit_num = _msg.subs[i].arg4;
            _obj.GetComponent<czlb_sub>().m_vip_exp.text = _msg.subs[i].arg3.ToString();
            _obj.GetComponent<czlb_sub>().m_scro = m_view;
            _obj.GetComponent<czlb_sub>().rewards.Clear();
            for (int j = 0; j < _msg.subs[i].types.Count; ++j)
            {
                s_t_reward t_reward = new s_t_reward();
                t_reward.type = _msg.subs[i].types[j];
                t_reward.value1 = _msg.subs[i].value1s[j];
                t_reward.value2 = _msg.subs[i].value2s[j];
                t_reward.value3 = _msg.subs[i].value3s[j];
                _obj.GetComponent<czlb_sub>().rewards.Add(t_reward);
            }
            _obj.GetComponent<czlb_sub>().reset();

            sys._instance.add_pos_anim(_obj, 0.3f, new Vector3(-300, 0, 0), i * 0.05f);
            sys._instance.add_alpha_anim(_obj, 0.3f, 0, 1.0f, i * 0.05f);

        }

    }

    public int comp(protocol.game.huodong_reward_sub sub1, protocol.game.huodong_reward_sub sub2)
    {
        if (sub1.arg2 != 1 && sub2.arg2 == 1)
        {
            return -1;
        }
        else if (sub1.arg2 == 1 && sub2.arg2 != 1)
        {
            return 1;
        }
        else if (sub1.arg1 <= sub1.arg3 && sub2.arg1 > sub2.arg3)
        {
            return -1;
        }
        else if (sub1.arg1 > sub1.arg3 && sub2.arg1 <= sub2.arg3)
        {
            return 1;
        }
        else if (sub1.arg1 < sub2.arg1)
        {
            return -1;
        }
        else if (sub2.arg1 < sub1.arg1)
        {
            return 1;
        }
        return 0;
    }

    void IMessage.message(s_message message)
    {

    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_HUODONG_ZHICHONG_VIEW)
        {
            _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_reward_view>(message.m_byte);
            m_title.GetComponent<UILabel>().text = _msg.name;
            m_title1.GetComponent<UILabel>().text = _msg.name;
            reset();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
