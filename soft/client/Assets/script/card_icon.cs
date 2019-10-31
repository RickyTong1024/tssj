using UnityEngine;

public class card_icon : MonoBehaviour
{
    public ulong m_guid_id;
    public ccard m_card;
    public int m_glevel = 0;
    public int m_jlevel = 0;
    public int m_level = 0;
    public s_t_class m_class;
    public string m_out_message;
    public bool m_star = true;

    public void init()
    {
        m_guid_id = 0;
        m_card = null;
        m_glevel = 0;
        m_jlevel = 0;
        m_level = 0;
        m_class = null;
        m_star = true;

        UIDragScrollView uisv = this.GetComponent<UIDragScrollView>();
        if (uisv != null)
        {
            Object.Destroy(uisv);
        }
        this.GetComponent<UISprite>().alpha = 1.0f;
        this.transform.GetComponent<BoxCollider>().enabled = true;
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void reset(int type = 0)
    {
        if (m_guid_id > 0)
        {
            m_card = sys._instance.m_self.get_card_guid(m_guid_id);
        }

        if (m_card != null)
        {
            m_class = m_card.m_t_class;
            if (type == 0)
            {
                m_glevel = m_card.get_glevel();
                m_jlevel = m_card.get_jlevel();
                m_level = m_card.get_level();
            }
            else
            {
                m_glevel = 0;
                m_jlevel = 0;
                m_level = 1;
            }

            m_guid_id = m_card.get_guid();
        }

        if (m_class == null)
        {
            return;
        }

        int pinzhi = m_class.pz * 100;
        if (m_card != null)
        {
            pinzhi = m_card.get_pinzhi();
        }

        this.transform.GetComponent<UISprite>().spriteName = m_class.icon;

        int color = 0;
        string s = "";
        color = m_class.color;

        if (pinzhi <= 300)
        {
            s = "xtbk_lvpt001";
        }
        else if (pinzhi <= 600)
        {
            s = "xtbk_lanpt001";
        }
        else if (pinzhi <= 900)
        {
            s = "xtbk_zipt001";
        }
        else if (pinzhi < 1300)
        {
            s = "xtbk_chpt001";
        }
        else if (pinzhi < 1700)
        {
            s = "xtbk_hopt001";
        }
        else
        {
            s = "xtbk_jinpt001";
        }

        this.transform.Find("bg").GetComponent<UISprite>().spriteName = s;

        if (m_level == 0)
        {
            this.transform.Find("lv").gameObject.SetActive(false);
            this.transform.Find("glevel").gameObject.SetActive(false);
            this.transform.Find("jlevel").gameObject.SetActive(false);
        }
        else
        {
            this.transform.Find("lv").gameObject.SetActive(true);
            this.transform.Find("jlevel").gameObject.SetActive(true);
            this.transform.Find("lv").GetComponent<UILabel>().text = m_level.ToString();

            if (m_glevel == 0)
            {
                this.transform.Find("glevel").gameObject.SetActive(false);
            }
            s_t_jinjie t_jinjie = game_data._instance.get_jinjie(m_jlevel);
            this.transform.Find("jlevel").GetComponent<UISprite>().spriteName = t_jinjie.icon;
            this.transform.Find("jlevel").GetComponent<UISprite>().MakePixelPerfect();
        }

        this.transform.Find("s1").gameObject.SetActive(true);
        this.transform.Find("s2").gameObject.SetActive(true);
        this.transform.Find("s3").gameObject.SetActive(true);
        this.transform.Find("s4").gameObject.SetActive(true);
        this.transform.Find("s5").gameObject.SetActive(true);

        color = 0;

        if (m_card != null)
        {
            color = m_card.get_glevel();
        }

        else if (m_glevel >= 0)
        {
            color = m_glevel;
        }

        for (int i = 1; i <= 5; i++)
        {
            if (i <= color)
            {
                this.transform.Find("s" + i.ToString()).GetComponent<UISprite>().spriteName = "xstar_002";
                this.transform.Find("s" + i.ToString()).gameObject.SetActive(true);
            }
            else
            {
                this.transform.Find("s" + i.ToString()).gameObject.SetActive(false);
            }

            if (i + 5 <= color)
            {
                this.transform.Find("s" + i.ToString()).GetComponent<UISprite>().spriteName = "xstar_003";
            }

            if (i + 10 <= color)
            {
                this.transform.Find("s" + i.ToString()).GetComponent<UISprite>().spriteName = "xstar_001";
            }
        }

        int num_ = color;
        if (num_ > 5)
        {
            num_ = 5;
        }
        this.transform.Find("s1").localPosition = new Vector3(-6 * (num_ - 1), -40, 0);

        if (m_card != null && m_class.color >= 4 && m_guid_id > 0 && m_card.get_role() != null)
        {
            this.transform.Find("xq").gameObject.SetActive(true);
            this.transform.Find("glevel").gameObject.SetActive(false);
            if (m_card.get_role().xq == 1)
            {
                this.transform.Find("xq").GetComponent<UISprite>().spriteName = "hbxq_cha";
            }
            else if (m_card.get_role().xq == 2)
            {
                this.transform.Find("xq").GetComponent<UISprite>().spriteName = "hbxq_js";
            }
            else if (m_card.get_role().xq == 3)
            {
                this.transform.Find("xq").GetComponent<UISprite>().spriteName = "hbxq_pt";
            }
            else if (m_card.get_role().xq == 4)
            {
                this.transform.Find("xq").GetComponent<UISprite>().spriteName = "hbxq_kx";
            }
            else if (m_card.get_role().xq == 5)
            {
                this.transform.Find("xq").GetComponent<UISprite>().spriteName = "hbxq_hao";
            }
        }
        else
        {
            this.transform.Find("xq").gameObject.SetActive(false);
        }
        this.transform.Find("glevel").gameObject.SetActive(false);
    }

    public void click()
    {
        if (m_out_message.Length == 0)
        {
            s_message _message = new s_message();
            _message.m_type = "card_dialog_box";
            _message.m_ints.Add(m_class.id);
            _message.m_ints.Add(1);
            _message.m_object.Add(m_card);
            cmessage_center._instance.add_message(_message);
            return;
        }
    }
}
