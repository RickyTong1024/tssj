using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class select_nationality : MonoBehaviour,IMessage {

	// Use this for initialization

    public GameObject m_src;
    public UIAtlas gq_uiatlas;
    bool nationality = false;
    public int type;

    int m_select_nationality_id;
	void Start () {
        cmessage_center._instance.add_handle(this);
	}
    void OnEnable()
    {
        m_src.GetComponent<UIScrollView>().onDragFinished += OnDragFinished;
        init();
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
        m_src.GetComponent<UIScrollView>().onDragFinished -= OnDragFinished;
        if (platform_config_common.m_nationality == 0)
        {
            
        }
        else
        {
            if (type == 1)
            {
                root_gui._instance.action_guide("player_name_end");
            }
        }
    }
    public void init()
    {
        if (m_src.GetComponent<SpringPanel>() != null)
        {
            m_src.GetComponent<SpringPanel>().enabled = false;
        }
        m_src.transform.localPosition = new Vector3(0, 0, 0);
        m_src.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
       
        sys._instance.remove_child(m_src);



        for (int i = 0; i < gq_uiatlas.GetComponent<UIAtlas>().spriteList.Count; i++)
        {

            GameObject _obj = game_data._instance.ins_object_res("ui/gq_sub");
            _obj.transform.parent = m_src.transform;
            _obj.transform.localScale = new Vector3(1, 1, 1);
            _obj.transform.localPosition = new Vector3(-280 + (i % 8) * 80, 95 - (i / 8) * 57, 0);
            _obj.GetComponent<gq_sub>().m_gq_id = i;
            _obj.GetComponent<gq_sub>().init(gq_uiatlas.GetComponent<UIAtlas>().spriteList[i].name);
            _obj.AddComponent<UIDragScrollView>();
            _obj.AddComponent<UIButtonMessage>();
            _obj.GetComponent<UIButtonMessage>().target = _obj.gameObject;
            _obj.GetComponent<UIButtonMessage>().functionName = "select_id";

        }
    }
    void IMessage.message(s_message message)
    {
        if (message.m_type == "select_nationality")
        {
            m_select_nationality_id = (int)message.m_ints[0];
            nationality = true;
            
        }
    }


    void OnDragFinished()
    {

        m_src.GetComponent<UIScrollView>().UpdateScrollbars(true);

        Vector3 constraint = m_src.GetComponent<UIScrollView>().panel.CalculateConstrainOffset(m_src.GetComponent<UIScrollView>().bounds.min, m_src.GetComponent<UIScrollView>().bounds.min);

        if (constraint.y <= 3f)
        {
            this.transform.Find("back/Sprite/down").gameObject.SetActive(false);
        }
        else
        {
            this.transform.Find("back/Sprite/down").gameObject.SetActive(true);
        }

        

    }
    public void click(GameObject obj)
    {
        if (obj.name == "gq_ok")
        {
            if (m_select_nationality_id == sys._instance.m_self.m_t_player.nalflag)
            {
                this.transform.Find("frame_big").GetComponent<frame>().hide();
            }
            else
            {
                if (nationality)
                {
                    protocol.game.cmsg_player_nalflag _msg = new protocol.game.cmsg_player_nalflag();
                    _msg.nalflag = m_select_nationality_id;
                    net_http._instance.send_msg<protocol.game.cmsg_player_nalflag>(opclient_t.CMSG_CHANGE_NALFLAG, _msg);
                }
                else
                {
                    this.transform.Find("frame_big").GetComponent<frame>().hide();

                    s_message _mes_nationality = new s_message();
                    _mes_nationality.m_type = "nationality_change";
                    cmessage_center._instance.add_message(_mes_nationality);
                }

            }
        }
        else if (obj.name == "gq_close")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_CHANGE_NALFLAG)
        {
            sys._instance.m_self.m_t_player.nalflag = m_select_nationality_id;
            this.transform.Find("frame_big").GetComponent<frame>().hide();

            s_message _mes_nationality = new s_message();
            _mes_nationality.m_type = "nationality_change";
            cmessage_center._instance.add_message(_mes_nationality);

        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
