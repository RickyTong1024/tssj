using UnityEngine;

public class card_page_item : MonoBehaviour
{
    public GameObject m_icon_root;
    public GameObject m_name;
    public GameObject m_sz;
    public GameObject m_dh;
    public GameObject m_hc;
    public GameObject m_sell;
    public GameObject m_pro;
    private GameObject m_icon;
    public GameObject m_select;
    public string m_out_message;
    private float m_rot;
    private ccard m_card;
    private int m_sp_id;
    private s_t_item m_t_item;
    private int m_sp_num;

    void Start()
    {
        m_rot = Random.Range(0f, 180f);
    }

    void hide()
    {
        m_dh.SetActive(false);
        m_hc.SetActive(false);
        m_sell.SetActive(false);
        m_sz.SetActive(false);
        m_dh.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language("card_page_item.cs_36_69");//合 成
        m_sz.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language("card_page_item.cs_37_69");//已上阵
        m_hc.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language("card_page_item.cs_38_69");//兑 换
        m_sell.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language("card_page_item.cs_39_71");//分 解
    }

    public void click(GameObject obj)
    {
        if (obj.transform.name == "dh")
        {
            if (m_sp_num < m_t_item.def_2)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("card_page_item.cs_48_59"));//碎片不足
                return;
            }
            s_message _message = new s_message();
            _message.m_type = "sui_pian_he_cheng";
            _message.m_ints.Add((int)m_sp_id);
            cmessage_center._instance.add_message(_message);
            return;
        }

        if (obj.transform.name == "hc")
        {
            s_message _message = new s_message();
            _message.m_type = "ji_yin_dui_huan";
            _message.m_ints.Add((int)m_sp_id);
            cmessage_center._instance.add_message(_message);
            return;
        }

        if (obj.transform.name == "sell")
        {
            int num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
            s_message _message = new s_message();
            _message.m_type = "ji_yin_sell";
            _message.m_ints.Add((int)m_sp_id);
            if (m_t_item.font_color >= 4)
            {
                root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language("arena.cs_104_45"), string.Format(game_data._instance.get_t_language("card_page_item.cs_79_66"), m_t_item.gold * num + ""), _message);//提示//是否分解全部基因获得[00ffff]{0}[-][ff66e3]战魂[-]{nn}([ff0000]基因也是伙伴突破的材料[-])
            }
            else
            {
                root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language("arena.cs_104_45"), string.Format(game_data._instance.get_t_language("card_page_item.cs_83_66"), m_t_item.gold * num + ""), _message);//提示//是否分解全部基因获得[00ffff]{0}[-][ff66e3]战魂[-]
            }
            return;
        }

        if (m_out_message == "select_jiyin")
        {
            s_message _msg = new s_message();
            _msg.m_type = m_out_message;
            _msg.m_long.Add(m_sp_id);
            _msg.m_object.Add(m_icon);
            cmessage_center._instance.add_message(_msg);
            m_select.SetActive(!m_select.activeSelf);
            return;
        }
        if (m_out_message == "select_jiegu")
        {
            if (m_card != null)
            {
                s_message _msg = new s_message();
                _msg.m_type = m_out_message;
                _msg.m_long.Add(m_card.get_template_id());
                _msg.m_object.Add(m_icon);
                cmessage_center._instance.add_message(_msg);
                m_select.SetActive(!m_select.activeSelf);
                return;
            }

        }
        if (m_out_message == "common_select_jiyin")
        {
            if (m_sp_id != 0)
            {
                root_gui._instance.show_tili_dialog_box(m_sp_id, 1);
                return;
            }
        }
        if (m_out_message == "common_hc_jiyin")
        {
            if (m_sp_id != 0)
            {
                s_message _msg = new s_message();

                _msg.m_type = m_out_message;
                _msg.m_ints.Add(m_sp_id);

                cmessage_center._instance.add_message(_msg);
                return;
            }
        }

        if (m_card != null)
        {
            s_message _msg = new s_message();

            _msg.m_type = m_out_message;
            _msg.m_long.Add(m_card.get_guid());
            _msg.m_object.Add(m_icon);
            cmessage_center._instance.add_message(_msg);
        }

        if (m_sp_id != 0 && m_t_item.type == 3001)
        {
            s_message _msg = new s_message();
            _msg.m_type = "show_cl_gui";
            _msg.m_ints.Add((int)m_sp_id);
            cmessage_center._instance.add_message(_msg);
        }
    }

    void remove_icon(int dir)
    {
        if (m_icon != null)
        {
            if (dir == 0)
            {
                GameObject.Destroy(m_icon);
                return;
            }
            TweenAlpha.Begin(m_icon, 0.5f, 0f);
            TweenScale.Begin(m_icon, 0.5f, new Vector3(0.2f, 0.2f, 0.2f));

            TweenAlpha _alpha = m_icon.GetComponent<TweenAlpha>();

            EventDelegate.Add(_alpha.onFinished, delegate ()
                              {
                                  GameObject.Destroy(_alpha.gameObject);
                              });

            TweenPosition _effect = TweenPosition.Begin(m_icon, 0.5f, new Vector3(-100f * dir, 0, 0));
            _effect.method = UITweener.Method.EaseInOut;
        }
    }

    public void set_sp(int id, int dir, bool flag = false)
    {
        GetComponent<BoxCollider>().enabled = true;
        remove_icon(dir);
        hide();
        m_card = null;
        m_sp_id = 0;
        m_pro.GetComponent<UISprite>().spriteName = "";
        m_name.GetComponent<UILabel>().text = "";
        m_select.SetActive(false);
        if (id == 0)
        {
            return;
        }
        m_sp_id = id;

        m_t_item = game_data._instance.get_item((int)m_sp_id);
        m_name.GetComponent<UILabel>().text = ccard.get_color_name(m_t_item.name, m_t_item.font_color);

        m_sp_num = sys._instance.m_self.get_item_num((uint)m_sp_id);

        if (m_sp_num < m_t_item.def_2)
        {
            m_dh.SetActive(false);
        }

        GameObject _root = game_data._instance.ins_object_res("ui/card_page_icon");
        _root.transform.parent = m_icon_root.transform;
        if (dir == 0)
        {
            _root.transform.localPosition = new Vector3(100 * 0, 0, 0);
            _root.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            _root.transform.localPosition = new Vector3(100 * dir, 0, 0);
            _root.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            _root.GetComponent<UISprite>().alpha = 0.0f;
        }
        _root.SetActive(true);
        if (flag)
        {
            GameObject _icon = icon_manager._instance.create_item_icon((int)m_sp_id, m_sp_num);
            _icon.transform.parent = _root.transform;
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            GameObject _icon = icon_manager._instance.create_item_icon((int)m_sp_id, 0);
            _icon.transform.parent = _root.transform;
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.GetComponent<BoxCollider>().enabled = false;
        }

        m_icon = _root;

        if (dir != 0)
        {
            sys._instance.add_alpha_anim(m_icon, 0.5f, 0f, 1f, 0);
            sys._instance.add_scale_anim(m_icon, 0.5f, 0.2f, 1.0f, 0);
            TweenPosition _ceffect = TweenPosition.Begin(m_icon, 0.5f, new Vector3(0, 0, 0));
            _ceffect.method = UITweener.Method.EaseInOut;
        }
        if (recycle_gui.is_injiyins((int)m_sp_id))
        {
            m_select.SetActive(true);
        }
        else
        {
            m_select.SetActive(false);
        }
        m_pro.GetComponent<UISprite>().spriteName = "szzjsj_001";//基
    }

    public void set_card(ulong guid, int dir)
    {
        GetComponent<BoxCollider>().enabled = true;
        hide();
        m_card = null;
        m_sp_id = 0;
        m_pro.GetComponent<UISprite>().spriteName = "";
        m_name.GetComponent<UILabel>().text = "";
        m_select.SetActive(false);
        m_hc.SetActive(false);
        remove_icon(dir);

        if (guid == 0)
        {
            return;
        }

        m_card = sys._instance.m_self.get_card_guid(guid);

        if (m_card == null)
        {
            return;
        }

        GameObject _root = game_data._instance.ins_object_res("ui/card_page_icon");
        _root.transform.parent = m_icon_root.transform;
        if (dir == 0)
        {
            _root.transform.localPosition = new Vector3(100 * 0, 0, 0);
            _root.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            _root.transform.localPosition = new Vector3(100 * dir, 0, 0);
            _root.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            _root.GetComponent<UISprite>().alpha = 0.0f;
        }
        _root.SetActive(true);

        GameObject _icon = icon_manager._instance.create_card_icon_ex(guid);
        _icon.transform.parent = _root.transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.SetActive(true);
        _icon.GetComponent<BoxCollider>().enabled = false;

        m_icon = _root;

        if (dir != 0)
        {
            sys._instance.add_alpha_anim(m_icon, 0.5f, 0f, 1f, 0);
            sys._instance.add_scale_anim(m_icon, 0.5f, 0.2f, 1.0f, 0);
            TweenPosition _ceffect = TweenPosition.Begin(m_icon, 0.5f, new Vector3(0, 0, 0));
            _ceffect.method = UITweener.Method.EaseInOut;
        }

        m_pro.GetComponent<UISprite>().spriteName = m_card.get_profession();
        m_name.GetComponent<UILabel>().text = m_card.get_color_name();

        if (m_card.is_zheng())
        {
            m_sz.SetActive(true);
        }
        else
        {
            m_sz.SetActive(false);
        }

        if (!recycle_gui.is_jiegus((ulong)m_card.get_template_id()))
        {
            m_select.SetActive(false);
        }

        m_dh.SetActive(false);
    }

    public void set_jy(int id, int dir)
    {
        GetComponent<BoxCollider>().enabled = true;
        remove_icon(dir);
        hide();
        m_card = null;
        m_sp_id = 0;
        m_pro.GetComponent<UILabel>().text = "";
        m_name.GetComponent<UILabel>().text = "";

        if (id == 0)
        {
            return;
        }

        m_sp_id = id;

        m_t_item = game_data._instance.get_item((int)m_sp_id);
        m_name.GetComponent<UILabel>().text = ccard.get_color_name(m_t_item.name, m_t_item.font_color);

        m_sp_num = sys._instance.m_self.get_item_num((uint)m_sp_id);

        if (sys._instance.m_self.has_card(m_t_item.def_1))
        {
            m_sell.SetActive(true);
            m_hc.SetActive(false);
        }
        else
        {
            m_hc.SetActive(true);
            m_sell.SetActive(false);
        }

        GameObject _root = game_data._instance.ins_object_res("ui/card_page_icon");
        _root.transform.parent = m_icon_root.transform;
        if (dir == 0)
        {
            _root.transform.localPosition = new Vector3(100 * 0, 0, 0);
            _root.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            _root.transform.localPosition = new Vector3(100 * dir, 0, 0);
            _root.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            _root.GetComponent<UISprite>().alpha = 0.0f;
        }
        _root.SetActive(true);

        GameObject _icon = icon_manager._instance.create_item_icon((int)m_sp_id, m_sp_num);
        _icon.transform.parent = _root.transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);

        m_icon = _root;

        if (dir != 0)
        {
            sys._instance.add_alpha_anim(m_icon, 0.5f, 0f, 1f, 0);
            sys._instance.add_scale_anim(m_icon, 0.5f, 0.2f, 1.0f, 0);
            TweenPosition _ceffect = TweenPosition.Begin(m_icon, 0.5f, new Vector3(0, 0, 0));
            _ceffect.method = UITweener.Method.EaseInOut;
        }
        m_pro.GetComponent<UISprite>().spriteName = "szzjsj_001";//基
    }

    void Update()
    {
        m_rot += Time.deltaTime * 2f;
        Vector3 _pos = m_icon_root.transform.localPosition;
        _pos.y = Mathf.Sin(m_rot) * 5f;
        m_icon_root.transform.localPosition = _pos;
    }
}
