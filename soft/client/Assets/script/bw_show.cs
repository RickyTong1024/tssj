using System.Collections.Generic;
using UnityEngine;

public class bw_show : MonoBehaviour, IMessage
{
    public List<GameObject> m_icon_root = new List<GameObject>();
    public List<GameObject> m_icon_root_0 = new List<GameObject>();
    public List<GameObject> m_icon_root_1 = new List<GameObject>();
    public List<GameObject> m_icon_root_2 = new List<GameObject>();
    public List<GameObject> m_icon_root_3 = new List<GameObject>();
    private Camera m_camer;
    public List<GameObject> m_icons = new List<GameObject>();
    public List<GameObject> m_nums = new List<GameObject>();
    public GameObject m_icon;

    public List<GameObject> m_effect_0 = new List<GameObject>();
    public List<GameObject> m_effect_1 = new List<GameObject>();
    private bool m_hecheng = false;
    private int m_id;
    private int m_effect_id = 0;
    public GameObject m_hecheng_obj;
    public GameObject m_hecheng_obj_cn;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
        m_camer = this.transform.Find("UI Root (3D)/Camera").GetComponent<Camera>();
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    void Awake()
    {
        if (game_data._instance.m_language == e_language.English)
        {
            if (m_hecheng_obj_cn.activeSelf)
            {
                m_hecheng_obj_cn.SetActive(false);
            }

            if (!m_hecheng_obj.activeSelf)
            {
                m_hecheng_obj.SetActive(true);
            }
        }
        else
        {
            if (m_hecheng_obj.activeSelf)
            {
                m_hecheng_obj.SetActive(false);
            }
            if (!m_hecheng_obj_cn.activeSelf)
            {
                m_hecheng_obj_cn.SetActive(true);
            }
        }
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_TREASURE_HECHENG)
        {
            m_effect_1[m_effect_id].SetActive(true);

            hide_time _hide = m_effect_1[m_effect_id].GetComponent<hide_time>();

            if (_hide == null)
            {
                _hide = m_effect_1[m_effect_id].AddComponent<hide_time>();
            }
            _hide.m_time = 3.0f;
            root_gui._instance.do_mask(3.0f);
        }
    }

    void fragment_show(int id)
    {
        m_id = id;
        s_t_baowu _t_baowu = game_data._instance.get_t_baowu(id);
        int _count = 0;
        int _max = 1;
        for (int i = 0; i < _t_baowu.fragments.Count; i++)
        {
            if (_t_baowu.fragments[i] != 0)
            {
                _count++;
            }
        }

        List<GameObject> _list = null;
        for (int i = 0; i < m_effect_0.Count; i++)
        {
            m_effect_0[i].SetActive(false);
        }

        if (_count == 3)
        {
            _list = m_icon_root_3;
            m_effect_0[0].SetActive(true);

            m_effect_id = 0;
        }
        else if (_count == 4)
        {
            _list = m_icon_root_2;
            m_effect_0[1].SetActive(true);

            m_effect_id = 1;
        }
        else if (_count == 5)
        {
            _list = m_icon_root_1;
            m_effect_0[2].SetActive(true);

            m_effect_id = 2;
        }
        else if (_count == 6)
        {
            _list = m_icon_root_0;
            m_effect_0[3].SetActive(true);

            m_effect_id = 3;
        }
        for (int i = 0; i < m_icon_root.Count; i++)
        {
            m_icon_root[i].SetActive(false);
            m_icons[i].SetActive(false);
            m_nums[i].SetActive(false);
        }

        for (int i = 0; i < _count; i++)
        {
            int _fragment_id = _t_baowu.fragments[i];
            int _fragment_num = sys._instance.m_self.get_item_num((uint)_fragment_id);

            GameObject _icon = m_icons[int.Parse(_list[i].transform.name) - 1];
            GameObject _num = m_nums[int.Parse(_list[i].transform.name) - 1];

            _icon.SetActive(true);
            _num.SetActive(true);
            if (_icon.transform.childCount > 0)
            {
                UIButtonMessage[] _messages = _icon.transform.GetChild(0).GetComponents<UIButtonMessage>();
                for (int j = 0; j < _messages.Length; j++)
                {
                    if (_messages[j].functionName == "qianduo")
                    {
                        DestroyImmediate(_messages[j]);
                        break;
                    }
                }
            }

            sys._instance.remove_child(_icon);
            GameObject _fragment_icon = icon_manager._instance.create_item_icon_ex(_fragment_id, _fragment_num, _max);

            _list[i].SetActive(true);
            _fragment_icon.transform.parent = _icon.transform;

            _fragment_icon.transform.localPosition = new Vector3(0, 0, 0);
            _fragment_icon.transform.localScale = new Vector3(2, 2, 2);
            UIButtonMessage[] messages = _fragment_icon.GetComponents<UIButtonMessage>();
            for (int j = 0; j < messages.Length; j++)
            {
                messages[j].enabled = false;
            }
            UIButtonMessage _mes = _fragment_icon.AddComponent<UIButtonMessage>();
            _mes.target = this.gameObject;
            _mes.functionName = "qianduo";
            _mes.trigger = UIButtonMessage.Trigger.OnClick;

            if (_fragment_num >= _max)
            {
                _num.transform.GetComponent<UILabel>().text = "[00ff00]" + _fragment_num;
            }
            else
            {
                _num.transform.GetComponent<UILabel>().text = "[ff0000]" + _fragment_num;
            }
        }
        GameObject _baowu_icon = icon_manager._instance.create_treasure_icon_ex(id);

        sys._instance.remove_child(m_icon);
        _baowu_icon.transform.parent = m_icon.transform;
        _baowu_icon.transform.localPosition = Vector3.zero;
        _baowu_icon.transform.localScale = new Vector3(2, 2, 2);

        s_t_baowu _baowu = game_data._instance.get_t_baowu(id);
        int num = 0;

        for (int i = 0; i < _baowu.count; i++)
        {

            for (int j = 0; j < sys._instance.m_self.m_t_player.item_ids.Count; j++)
            {
                if (sys._instance.m_self.m_t_player.item_ids[j] == _baowu.fragments[i])
                {
                    num++;
                }
            }
        }

        if (num == _baowu.count)
        {
            if (m_hecheng_obj.activeSelf)
            {
                m_hecheng_obj.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (m_hecheng_obj_cn.activeSelf)
            {
                m_hecheng_obj_cn.transform.GetChild(0).gameObject.SetActive(true);
            }
            m_hecheng = true;
        }
        else
        {
            if (m_hecheng_obj.activeSelf)
            {
                m_hecheng_obj.transform.GetChild(0).gameObject.SetActive(false);
            }
            if (m_hecheng_obj_cn.activeSelf)
            {
                m_hecheng_obj_cn.transform.GetChild(0).gameObject.SetActive(false);
            }

            m_hecheng = false;
        }
        m_camer.cullingMask = -1;
    }

    void qianduo(GameObject obj)
    {
        if (obj.name == "he_cheng" || obj.name == "he_cheng_cn")
        {
            if (m_hecheng)
            {
                if (m_hecheng)
                {

                    s_message _mes = new s_message();
                    _mes.m_type = "baowu_hecheng";
                    _mes.m_ints.Add(m_id);
                    cmessage_center._instance.add_message(_mes);
                }
                else
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_hecheng.cs_539_59"));//材料不足
                }
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_hecheng.cs_539_59"));//材料不足
            }
            return;
        }
        int _id = obj.GetComponent<item_icon>().m_item_id;

        if (!sys._instance.m_self.m_t_player.item_ids.Contains((uint)_id))
        {
            s_message _mes = new s_message();
            _mes.m_type = "show_baowu_qiangduo_gui";
            _mes.m_ints.Add(_id);
            cmessage_center._instance.add_message(_mes);
        }
        else
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bw_show.cs_256_67"));//你已拥有该碎片
        }
    }

    void show(int id)
    {
        fragment_show(id);
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "bw_show")
        {
            show((int)message.m_ints[0]);
        }
        else if (message.m_type == "hide_hecheng")
        {
            this.gameObject.SetActive(false);
        }
        else if (message.m_type == "show_hecheng")
        {
            this.gameObject.SetActive(true);
        }
    }
}
