
using UnityEngine;
using System.Collections;

public class memory_gui : MonoBehaviour,IMessage {

    public GameObject m_huiyi_zhizhen;
    public GameObject m_huiyilu;
    public GameObject m_huiyi_shop;
    public GameObject m_huiyi_zhanbu;

    public GameObject m_button_zhizhen;
    public GameObject m_button_huiyilu;
    public GameObject m_button_zhanbu;
    public GameObject m_button_shop;
    public static memory_gui _instance;
    public GameObject m_buttons;
    private Camera m_mem_camer;
    public  bool flag = true;
    bool flag_start = true;
    public string m_default_active;

    public GameObject m_mask;

	void Start () 
    {
        if (root_gui._instance.m_default_active == "show_huiyilu")
        {
            m_huiyi_shop.SetActive(false);
            m_huiyi_zhanbu.SetActive(false);
            m_huiyi_zhizhen.SetActive(false);
            m_huiyilu.SetActive(true);
            transform.Find("back").gameObject.SetActive(false);
            flag_start = false;
            root_gui._instance.m_default_active = "";
        }
        else if (root_gui._instance.m_default_active == "show_huiyishop")
        {
            m_huiyi_shop.SetActive(true);
            m_huiyi_zhanbu.SetActive(false);
            m_huiyi_zhizhen.SetActive(false);
            m_huiyilu.SetActive(false);
            m_huiyi_shop.GetComponent<huiyi_shop>().reset();
            transform.Find("back").gameObject.SetActive(false);
            flag_start = false;
            root_gui._instance.m_default_active = "";

        }
        else if (root_gui._instance.m_default_active == "show_zhizhen")
        {
            m_huiyi_shop.SetActive(false);
            m_huiyi_zhanbu.SetActive(false);
            m_huiyi_zhizhen.SetActive(true);
            m_huiyilu.SetActive(false);
            transform.Find("back").gameObject.SetActive(false);
            flag_start = false;
            root_gui._instance.m_default_active = "";

        }
        else if (root_gui._instance.m_default_active == "show_zhanbu")
        {
            m_huiyi_shop.SetActive(false);
            m_huiyi_zhanbu.SetActive(true);
            m_huiyi_zhizhen.SetActive(false);
            m_huiyilu.SetActive(false);
            transform.Find("back").gameObject.SetActive(false);
            flag_start = false;
            root_gui._instance.m_default_active = "";

        }
        else if (root_gui._instance.m_default_active == "show_luckshop")
        {
            m_huiyi_shop.SetActive(false);
            m_huiyi_zhanbu.SetActive(false);
            m_huiyi_zhizhen.SetActive(true);
            m_huiyi_zhizhen.GetComponent<huiyi_zhizhen_gui>().m_luck_shop.SetActive(true);
            m_huiyilu.SetActive(false);
            m_huiyi_zhizhen.GetComponent<huiyi_zhizhen_gui>().refresh_shop(1);
            transform.Find("back").gameObject.SetActive(false);
            flag_start = false;
            root_gui._instance.m_default_active = "";

        }
        _instance = this;
	}
    public void add_handle()
    {
        cmessage_center._instance.add_handle(this);
    }
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButton(0))
        {
            if (m_mem_camer == null)
            {
                GameObject set = GameObject.Find("sn/setting");
                if (set != null)
                {
                    m_mem_camer = set.transform.Find("MainCamera").GetComponent<Camera>();

                }
 
            }
            if (m_mem_camer != null)
            {
                Ray mRay = m_mem_camer.ScreenPointToRay(Input.mousePosition);
				RaycastHit mHi;
                if (Physics.Raycast(mRay, out mHi))
                {
                    if (!flag)
                    {
                        return;
                    }
                   
                  
                    
                    if (mHi.collider.name == "position_MemoryRoom_compass")
                    {
                        TweenScale _scale = TweenScale.Begin(mHi.collider.gameObject, 0.35f, new Vector3(1.1f, 1.1f, 1.1f));
                        _scale.updateTable = true;
                        _scale.method = UITweener.Method.Linear;
                        _scale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
                        flag = false;
                        TweenAlpha _alp = m_huiyi_zhizhen.AddComponent<TweenAlpha>();
                        _alp.from = 0.5f;
                        _alp.to = 1;
                        _alp.enabled = true;
                        _scale.onFinished.Clear();
                        _scale.AddOnFinished(delegate
                        {
                            m_huiyi_zhizhen.SetActive(true);
                            s_message mes = new s_message();
                            mes.m_type = "hide_memory_scene";
                            cmessage_center._instance.add_message(mes);
                        });
                        
                    }
                    else if (mHi.collider.name == "position_MemoryRoom_Sphere")
                    {
                        TweenScale _scale = TweenScale.Begin(mHi.collider.gameObject, 0.35f, new Vector3(1.1f, 1.1f, 1.1f));
                        _scale.updateTable = true;
                        _scale.method = UITweener.Method.Linear;
                        _scale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
                        flag = false;
                        TweenAlpha _alp = m_huiyi_zhanbu.AddComponent<TweenAlpha>();
                        _alp.from = 0.5f;
                        _alp.to = 1;
                        _alp.enabled = true;
                        _scale.onFinished.Clear();
                        _scale.AddOnFinished(delegate
                        {
                            m_huiyi_zhanbu.SetActive(true);
                            s_message mes = new s_message();
                            mes.m_type = "hide_memory_scene";
                            cmessage_center._instance.add_message(mes);
                        });
                      //  m_huiyi_zhanbu.SetActive(true);
 
                    }
                    else if (mHi.collider.name == "position_MemoryRoom_book")
                    {
                        TweenScale _scale = TweenScale.Begin(mHi.collider.gameObject, 0.35f, new Vector3(1.1f, 1.1f, 1.1f));
                        _scale.updateTable = true;
                        _scale.method = UITweener.Method.Linear;
                        _scale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
                        flag = false;
                        TweenAlpha _alp = m_huiyi_shop.AddComponent<TweenAlpha>();
                        _alp.from = 0.5f;
                        _alp.to = 1;
                        _alp.enabled = true;
                        _scale.onFinished.Clear();

                        _scale.AddOnFinished(delegate
                        {
                            m_huiyi_shop.SetActive(true);
                            s_message mes = new s_message();
                            mes.m_type = "hide_memory_scene";
                            cmessage_center._instance.add_message(mes);
                        });

                      //  m_huiyi_shop.SetActive(true);
                    }
                    else if (mHi.collider.name == "position_MemoryRoom_cube")
                    {
                        TweenScale _scale = TweenScale.Begin(mHi.collider.gameObject, 0.35f, new Vector3(1.1f, 1.1f, 1.1f));
                        _scale.updateTable = true;
                        _scale.method = UITweener.Method.Linear;
                        _scale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
                        flag = false;
                        _scale.onFinished.Clear();
                        TweenAlpha _alp = m_huiyilu.AddComponent<TweenAlpha>();
                        _alp.from = 0.5f;
                        _alp.to = 1;
                        _alp.enabled = true;
                        _scale.AddOnFinished(delegate
                        {
                            m_huiyilu.SetActive(true);
                            s_message mes = new s_message();
                            mes.m_type = "hide_memory_scene";
                            cmessage_center._instance.add_message(mes);
                        });
                    }
                }
            }
        }
	}

	void IMessage.message(s_message message)
    {
        if (message.m_type == "show_huiyilu")
        {
            m_huiyi_shop.SetActive(false);
            m_huiyi_zhanbu.SetActive(false);
            m_huiyi_zhizhen.SetActive(false);
            m_huiyilu.SetActive(true);
            transform.Find("back").gameObject.SetActive(false);
            flag_start = false;
        }
        else if (message.m_type == "show_huiyishop")
        {
            m_huiyi_shop.SetActive(true);
            m_huiyi_zhanbu.SetActive(false);
            m_huiyi_zhizhen.SetActive(false);
            m_huiyilu.SetActive(false);
            m_huiyi_shop.GetComponent<huiyi_shop>().reset();
            transform.Find("back").gameObject.SetActive(false);
            flag_start = false;

        }
        else if (message.m_type == "show_zhizhen")
        {
            m_huiyi_shop.SetActive(false);
            m_huiyi_zhanbu.SetActive(false);
            m_huiyi_zhizhen.SetActive(true);
            m_huiyilu.SetActive(false);
            transform.Find("back").gameObject.SetActive(false);
            flag_start = false;

        }
        else if (message.m_type == "show_zhanbu")
        {
            m_huiyi_shop.SetActive(false);
            m_huiyi_zhanbu.SetActive(true);
            m_huiyi_zhizhen.SetActive(false);
            m_huiyilu.SetActive(false);
            transform.Find("back").gameObject.SetActive(false);
            flag_start = false;

        }
        else if (message.m_type == "show_luckshop")
        {
            m_huiyi_shop.SetActive(false);
            m_huiyi_zhanbu.SetActive(false);
            m_huiyi_zhizhen.SetActive(true);
            m_huiyi_zhizhen.GetComponent<huiyi_zhizhen_gui>().m_luck_shop.SetActive(true);
            m_huiyilu.SetActive(false);
            m_huiyi_zhizhen.GetComponent<huiyi_zhizhen_gui>().refresh_shop(1);
            transform.Find("back").gameObject.SetActive(false);
            flag_start = false;
        }
        else if (message.m_type == "show_memory_scene")
        {
            transform.Find("back").gameObject.SetActive(true);
        }
	}

	void IMessage.net_message(s_net_message message)
	{
		
	}

	public void set_mask(bool state)
	{
		m_mask.SetActive(state);
	}

	void OnEnable()
	{
		reset();
	}

    void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            reset();
            if (m_huiyi_zhizhen.gameObject.activeSelf)
            {
                if (m_huiyi_zhizhen.GetComponent<huiyi_zhizhen_gui>().m_luck_shop.activeSelf)
                {
                    m_huiyi_zhizhen.GetComponent<huiyi_zhizhen_gui>().close();
                    m_huiyi_zhizhen.GetComponent<huiyi_zhizhen_gui>().m_luck_shop.transform.Find("frame_big").GetComponent<frame>().hide();
                    return;
                }
                flag = true;
                m_huiyi_zhizhen.SetActive(false);
                s_message mes = new s_message();
                mes.m_type = "show_memory_scene";
                cmessage_center._instance.add_message(mes);
                return;
            }
            else if (m_huiyilu.gameObject.activeSelf)
            {
                if (m_huiyilu.GetComponent<huiyilu_gui>().m_dialog.gameObject.transform.parent.gameObject.activeSelf)
                {
                    if (m_huiyilu.GetComponent<huiyilu_gui>().m_sx_flag)
                    {
                        m_huiyilu.GetComponent<huiyilu_gui>().m_dialog.transform.parent.gameObject.SetActive(false);
                        m_huiyilu.GetComponent<huiyilu_gui>().set_attr();
                        Destroy(m_huiyilu.GetComponent<huiyilu_gui>().m_dialog.GetComponent<TypewriterEffect>());
                        return;
                    }
                    return;
                }
                else if (m_huiyilu.GetComponent<huiyilu_gui>().m_huiyi_sub_panel.activeSelf)
                {
                    m_huiyilu.GetComponent<huiyilu_gui>().m_huiyi_sub_panel.transform.Find("frame_big").GetComponent<frame>().hide();
                    return;
                }
                else if (m_huiyilu.GetComponent<huiyilu_gui>().m_chengjiu_panel.activeSelf)
                {
                    m_huiyilu.GetComponent<huiyilu_gui>().m_chengjiu_panel.transform.Find("frame_big").GetComponent<frame>().hide();
                    return;
                }
                else if (m_huiyilu.GetComponent<huiyilu_gui>().m_sx_panel.activeSelf)
                {
                    m_huiyilu.GetComponent<huiyilu_gui>().m_sx_panel.transform.Find("frame_big").GetComponent<frame>().hide();
                    return;
                }
                flag = true;
                s_message mes = new s_message();
                mes.m_type = "show_memory_scene";
                cmessage_center._instance.add_message(mes);
                m_huiyilu.SetActive(false);
                return;
            }
            else if (m_huiyi_shop.activeSelf)
            {
                flag = true;
                m_huiyi_shop.transform.Find("frame_big").GetComponent<frame>().hide();
                s_message mes = new s_message();
                mes.m_type = "show_memory_scene";
                cmessage_center._instance.add_message(mes);
                return;
            }
            else if (m_huiyi_zhanbu.activeSelf)
            {
                if (m_huiyi_zhanbu.GetComponent<huiyi_zhanbu_gui>().flag)
                {
                    flag = true;
                    s_message mes = new s_message();
                    mes.m_type = "show_memory_scene";
                    cmessage_center._instance.add_message(mes);
                    m_huiyi_zhanbu.SetActive(false);
                }

                return;
            }
            sys._instance.m_game_state = "hall";
            sys._instance.load_scene(sys._instance.m_hall_name);
            s_message _mes = new s_message();
            _mes.m_type = "show_main_gui";
            cmessage_center._instance.add_message(_mes);
            this.gameObject.SetActive(false);
        }
    }

    void reset()
    {
        GameObject set = GameObject.Find("sn/setting");
        if (set != null)
        {
            m_mem_camer = set.transform.Find("MainCamera").GetComponent<Camera>();
 
        }
         m_button_huiyilu.transform.Find("effect").gameObject.SetActive(false);
         m_button_shop .transform.Find("effect").gameObject.SetActive(false);
         m_button_zhizhen.transform.Find("effect").gameObject.SetActive(false);
         m_button_zhanbu.transform.Find("effect").gameObject.SetActive(false);
        if ((1 - sys._instance.m_self.m_t_player.huiyi_chou_num) > 0)
        {
            m_button_zhizhen.transform.Find("effect").gameObject.SetActive(true);
        }
        if (huiyilu_gui.is_jihuo().Count > 0)
        {
            m_button_huiyilu.transform.Find("effect").gameObject.SetActive(true);
        }
        if((game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip).huiyi_zhanbu_num - sys._instance.m_self.m_t_player.huiyi_zhanpu_num) > 0)
        {
            m_button_zhanbu.transform.Find("effect").gameObject.SetActive(true);
        }
        if (sys._instance.m_self.m_item_shop_effect > 0)
        {
            m_button_shop.transform.Find("effect").gameObject.SetActive(true);
        }
        if (flag_start)
        {
            flag_start = false;
            GameObject setting = GameObject.Find("setting");
            if (setting != null)
            {
                TweenPosition tween = setting.AddComponent<TweenPosition>();
                tween.from = new Vector3(0, 0, 3);
                tween.to = new Vector3(0, 0, 0);
                tween.duration = 2f;
                tween.enabled = true;

                TweenScale scale = m_buttons.AddComponent<TweenScale>();
                scale.from = new Vector3(0.6f, 0.6f, 0.6f);
                scale.to = new Vector3(1, 1, 1);
                scale.duration = 2f;
                scale.enabled = true;
                scale.AddOnFinished(delegate() { Destroy(scale); });
                TweenPosition pos = m_buttons.AddComponent<TweenPosition>();
                pos.from = new Vector3(0, 55, 0);
                pos.to = Vector3.zero;
                pos.duration = 2f;
                pos.enabled = true;
                pos.AddOnFinished(delegate() { Destroy(pos); });
            }
        }
    }
    void OnDisable()
    {
        flag_start = true;
        Destroy(m_buttons.GetComponent<TweenPosition>());
        Destroy(m_buttons.GetComponent<TweenScale>());
        Destroy(this.gameObject);
    }
     void OnDestroy()
     {
         cmessage_center._instance.remove_handle(this);
     }
}
