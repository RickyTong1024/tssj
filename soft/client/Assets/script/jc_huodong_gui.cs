
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class jc_huodong_gui : MonoBehaviour ,IMessage{
    
	public GameObject m_view;
    GameObject czjh_pag;
    GameObject huoyue_pag;
    GameObject zkfs_pag;
    GameObject land_gift_pag;
    GameObject chongzhifanli_pag;
    GameObject dbcz_pag;
    GameObject ykjj_kq_pag;
    GameObject ykjj_lq_pag;
    GameObject pttq_pag;
    GameObject czlb_pag;
    [System.NonSerialized]
    public UILabel top_lable_front;
    [System.NonSerialized]
    public UILabel top_title_lable;
    [System.NonSerialized]
    public GameObject m_select_p;
    List<GameObject> leftpag = new List<GameObject>();
    int select = 0;
	// Use this for initialization
    void Awake()
    {
        _instance = this;
    }
    static public jc_huodong_gui _instance;
   
	void Start () {
        cmessage_center._instance.add_handle(this);

        czjh_pag = this.gameObject.transform.Find("back/right/czjh_pag").gameObject;
        huoyue_pag = this.gameObject.transform.Find("back/right/huoyue_pag").gameObject;
        zkfs_pag = this.gameObject.transform.Find("back/right/zkfs_pag").gameObject;
        land_gift_pag = this.gameObject.transform.Find("back/right/land_gift_pag").gameObject;
        chongzhifanli_pag = this.gameObject.transform.Find("back/right/chongzhifanli_pag").gameObject;
        dbcz_pag = this.gameObject.transform.Find("back/right/dbcz_pag").gameObject;
        ykjj_kq_pag = this.gameObject.transform.Find("back/right/ykjj_kq_pag").gameObject;
        ykjj_lq_pag = this.gameObject.transform.Find("back/right/ykjj_lq_pag").gameObject;
        pttq_pag = this.gameObject.transform.Find("back/right/pttq_pag").gameObject;
        czlb_pag = this.gameObject.transform.Find("back/right/czlb_pag").gameObject;

        top_lable_front = this.gameObject.transform.Find("back/top/title").gameObject.GetComponent<UILabel>();
        top_title_lable = this.gameObject.transform.Find("back/top/title/title").gameObject.GetComponent<UILabel>();

        m_select_p = this.gameObject.transform.Find("back/left/Scroll View/select").gameObject;

	}
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    void IMessage.message (s_message message)
	{
        
        if (message.m_type == "show_czjh")
        {
            select = (int)message.m_ints[0];
            pag_control(9,message.m_bools[0]);

        }
        else if (message.m_type == "show_huoyue")
        {
            huoyue_pag.GetComponent<huo_yue_gui>().m_huodong_id = (int)message.m_ints[0];
            select = (int)message.m_ints[0];
            pag_control(4, message.m_bools[0]);
        }
        else if (message.m_type == "show_zkfs")
        {
            select = (int)message.m_ints[0];
            zkfs_pag.GetComponent<zkfs_gui>().m_huodong_id = (int)message.m_ints[0];
            pag_control(7, message.m_bools[0]);
        }
        else if (message.m_type == "show_land")
        {
            select = (int)message.m_ints[0];
            land_gift_pag.GetComponent<land_gift_gui>().m_huodong_id = (int)message.m_ints[0];
            pag_control(6, message.m_bools[0]);
        }
        else if (message.m_type == "show_czfl")
        {
            select = (int)message.m_ints[0];
            chongzhifanli_pag.GetComponent<chongzhifali_gui>().m_huodong_id = (int)message.m_ints[0];
            pag_control(3, message.m_bools[0]);
        }
        else if (message.m_type == "show_dbcz")
        {
            select = (int)message.m_ints[0];
            dbcz_pag.GetComponent<dbcz_gui>().m_huodong_id = (int)message.m_ints[0];
            pag_control(5, message.m_bools[0]);
        }
        else if (message.m_type == "show_ykjj")
        {
            select = (int)message.m_ints[0];
            top_lable_front.text = game_data._instance.get_t_jc_huodong((int)message.m_ints[1]).name;
            top_title_lable.text = game_data._instance.get_t_jc_huodong((int)message.m_ints[1]).name;
            pag_control(15, message.m_bools[0]);
            
        }
        else if (message.m_type == "show_pttq")
        {
            select = (int)message.m_ints[0];
            top_lable_front.text = game_data._instance.get_t_jc_huodong(select).name;
            top_title_lable.text = game_data._instance.get_t_jc_huodong(select).name;
            pag_control(2, message.m_bools[0]);
        }
        else if (message.m_type == "resert_jc_huodong_pag" || message.m_type == "daily_refresh")
        {
            int first_id = select >= 10000 ? select / 10000 : select;
            s_t_jc_huodong m_t_jc_huodong = game_data._instance.get_t_jc_huodong(first_id);
            s_message mes = new s_message();
            mes.m_type = m_t_jc_huodong.message;
            mes.m_ints.Add(select);
            mes.m_ints.Add(first_id);
            mes.m_string.Add(top_lable_front.text);
            mes.m_bools.Add(true);
            cmessage_center._instance.add_message(mes);  

        }
        else if (message.m_type == "show_czlb")
        {
            select = (int)message.m_ints[0];
            czlb_pag.GetComponent<czlb_pag>().m_huodong_id = (int)message.m_ints[0];
            pag_control(16, message.m_bools[0]);
        }
        
	}
    void selectItem()
    {
        for (int i = 0; i < leftpag.Count; i++)
        {
            if (leftpag[i].GetComponent<jc_huodong_sub>().m_huodong_id != select)
            {
                leftpag[i].transform.Find("select").gameObject.SetActive(false);
            }
            else
            {
                leftpag[i].transform.Find("select").gameObject.SetActive(true);
            }
        }
    }

    void pag_control(int id,bool resert)
    {
        selectItem();
        string _text;
        
        switch(id)
        {

            case 9:
                czjh_pag.SetActive(true);
                czjh_pag.GetComponent<czjh_gui>().reset();
                
                _text = game_data._instance.get_t_language("426");
                top_lable_front.text = _text;
                top_title_lable.text = _text;
                huoyue_pag.SetActive(false);
                czjh_pag.SetActive(true);
                zkfs_pag.SetActive(false);
                land_gift_pag.SetActive(false);
                chongzhifanli_pag.SetActive(false);
                dbcz_pag.SetActive(false);
                ykjj_kq_pag.SetActive(false);
                ykjj_lq_pag.SetActive(false);
                pttq_pag.SetActive(false);
                czlb_pag.SetActive(false);
                break;
            case 4:
                
                huoyue_pag.GetComponent<huo_yue_gui>().init(resert);
                huoyue_pag.SetActive(true);
                czjh_pag.SetActive(false);
                zkfs_pag.SetActive(false);
                land_gift_pag.SetActive(false);
                chongzhifanli_pag.SetActive(false);
                dbcz_pag.SetActive(false);
                ykjj_kq_pag.SetActive(false);
                ykjj_lq_pag.SetActive(false);
                pttq_pag.SetActive(false);
                czlb_pag.SetActive(false);
                break;
            case 7:
                huoyue_pag.SetActive(false);
                czjh_pag.SetActive(false);
                
                zkfs_pag.GetComponent<zkfs_gui>().init(resert);
                zkfs_pag.SetActive(true);
                land_gift_pag.SetActive(false);
                chongzhifanli_pag.SetActive(false);
                dbcz_pag.SetActive(false);
                ykjj_kq_pag.SetActive(false);
                ykjj_lq_pag.SetActive(false);
                pttq_pag.SetActive(false);
                czlb_pag.SetActive(false);
                break;
            case 6:
                huoyue_pag.SetActive(false);
                czjh_pag.SetActive(false);
                zkfs_pag.SetActive(false);
               
                land_gift_pag.GetComponent<land_gift_gui>().init(resert);
                land_gift_pag.SetActive(true);
                chongzhifanli_pag.SetActive(false);
                dbcz_pag.SetActive(false);
                ykjj_kq_pag.SetActive(false);
                ykjj_lq_pag.SetActive(false);
                pttq_pag.SetActive(false);
                czlb_pag.SetActive(false);
                break;
            case 3:
                huoyue_pag.SetActive(false);
                czjh_pag.SetActive(false);
                zkfs_pag.SetActive(false);
                land_gift_pag.SetActive(false);
                dbcz_pag.SetActive(false);
                
                chongzhifanli_pag.GetComponent<chongzhifali_gui>().init(resert);
                chongzhifanli_pag.SetActive(true);
                ykjj_kq_pag.SetActive(false);
                ykjj_lq_pag.SetActive(false);
                pttq_pag.SetActive(false);
                czlb_pag.SetActive(false);
                break;
            case 5:
                huoyue_pag.SetActive(false);
                czjh_pag.SetActive(false);
                zkfs_pag.SetActive(false);
                land_gift_pag.SetActive(false);
                chongzhifanli_pag.SetActive(false);
                
                dbcz_pag.GetComponent<dbcz_gui>().init(resert);
                dbcz_pag.SetActive(true);
                ykjj_kq_pag.SetActive(false);
                ykjj_lq_pag.SetActive(false);
                pttq_pag.SetActive(false);
                czlb_pag.SetActive(false);
                break;
            case 15:
                //月卡基金活动开启

                for (int i = 0; i < sys._instance.m_self.m_huodong_ids.Count; i++)
                {
                    if (sys._instance.m_self.m_huodong_ids[i] / 10000 == 15)
                    {
                        if (sys._instance.m_self.m_end_time[i] > timer.now())
                        {
                            
                            ykjj_kq_pag.GetComponent<yueka_jijin_kaiqi>().init(resert);
                            ykjj_kq_pag.SetActive(true);
                            ykjj_lq_pag.SetActive(false);
                        }
                        else
                        {
                            ykjj_kq_pag.SetActive(false);
                          
                            ykjj_lq_pag.GetComponent<yueka_jijin>().init(resert);
                            ykjj_lq_pag.SetActive(true);
                            
                        }
                    }
                }
                huoyue_pag.SetActive(false);
                czjh_pag.SetActive(false);
                zkfs_pag.SetActive(false);
                land_gift_pag.SetActive(false);
                chongzhifanli_pag.SetActive(false);
                dbcz_pag.SetActive(false);
                pttq_pag.SetActive(false);
                czlb_pag.SetActive(false);
                break;
            case 2:
                
                pttq_pag.GetComponent<pttq_gui>().init(resert);
                pttq_pag.SetActive(true);
                ykjj_kq_pag.SetActive(false);
                ykjj_lq_pag.SetActive(false);
                huoyue_pag.SetActive(false);
                czjh_pag.SetActive(false);
                zkfs_pag.SetActive(false);
                land_gift_pag.SetActive(false);
                chongzhifanli_pag.SetActive(false);
                dbcz_pag.SetActive(false);
                czlb_pag.SetActive(false);
                break;
            case 16:
               
                czlb_pag.GetComponent<czlb_pag>().init(resert);
                czlb_pag.SetActive(true);
                pttq_pag.SetActive(false);
                ykjj_kq_pag.SetActive(false);
                ykjj_lq_pag.SetActive(false);
                huoyue_pag.SetActive(false);
                czjh_pag.SetActive(false);
                zkfs_pag.SetActive(false);
                land_gift_pag.SetActive(false);
                chongzhifanli_pag.SetActive(false);
                dbcz_pag.SetActive(false);
                break;
            default:
                break;
        }

       
    }

    void IMessage.net_message(s_net_message message)
    { 
    }

	void OnEnable()
    {
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_view.transform.localPosition = new Vector3(-38, -76, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);
        List<int> m_huodong_ids = new List<int>();
        for (int i = 0; i < game_data._instance.m_dbc_jc_huodong.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_jc_huodong.get(0, i));
            if (!has(id))
            {
                continue;
            }
            m_huodong_ids.Add(id);
        }
        for (int i = 0; i < sys._instance.m_self.m_huodong_ids.Count; ++i)
        {
            m_huodong_ids.Add(sys._instance.m_self.m_huodong_ids[i]);
        }
        m_huodong_ids.Sort(comp);
        int c = 0;
        for (int i = 0; i < m_huodong_ids.Count; ++i)
        {
            int id = m_huodong_ids[i];
            GameObject target = game_data._instance.ins_object_res("ui/jc_huodong_sub");

            target.transform.parent = m_view.transform;
            target.transform.localPosition = new Vector3(0, 138 - c * 78, 0);
            target.transform.localScale = new Vector3(1, 1, 1);
            target.transform.GetComponent<jc_huodong_sub>().m_id = id;
            if (id >= 10000)
            {
                target.transform.GetComponent<jc_huodong_sub>().m_id = id / 10000;
            }
            target.transform.GetComponent<jc_huodong_sub>().m_huodong_id = id;
            target.transform.GetComponent<jc_huodong_sub>().m_end_time = get_time(id);
            target.transform.GetComponent<jc_huodong_sub>().huodong_name = get_name(id);
            target.transform.GetComponent<jc_huodong_sub>().m_jc_huodong_gui = this.gameObject;
            target.transform.GetComponent<jc_huodong_sub>().reset();
            leftpag.Add(target);
            sys._instance.add_pos_anim(target, 0.3f, new Vector3(-300, 0, 0), c * 0.05f);
            sys._instance.add_alpha_anim(target, 0.3f, 0, 1.0f, c * 0.05f);
            c++;

        }

        int first_id = m_huodong_ids[0] >= 10000 ? m_huodong_ids[0] / 10000 : m_huodong_ids[0];
        s_t_jc_huodong m_t_jc_huodong = game_data._instance.get_t_jc_huodong(first_id);
        if (m_t_jc_huodong.message != "0")
        {
            s_message mes = new s_message();
            mes.m_type = m_t_jc_huodong.message;
            mes.m_ints.Add(m_huodong_ids[0]);
            mes.m_ints.Add(first_id);
            mes.m_bools.Add(false);
            cmessage_center._instance.add_message(mes);
        }

    }

	public int comp(int id1,int id2)
	{
		if(is_effect(id1) && !is_effect(id2))
		{
			return -1;
		}
		else if(!is_effect(id1) && is_effect(id2))
		{
			return 1;
		}
		else if(is_forever(id1) && !is_forever(id2))
		{
			return -1;
		}
		else if(!is_forever(id1) && is_forever(id2))
		{
			return 1;
		}
		return 0;
	}

	public bool is_forever(int id)
	{
		for(int i = 0; i < sys._instance.m_self.m_huodong_ids.Count;++i)
		{
			if(id == sys._instance.m_self.m_huodong_ids[i])
			{
				return true;
			}
		}
		return false;
	}

	public void click(GameObject obj)
	{
        this.GetComponent<ui_show_anim>().hide_ui();
		s_message _message = new s_message();
		_message.m_type = "show_main_gui";
		cmessage_center._instance.add_message(_message);
	}

	public ulong get_time(int id)
	{
		ulong _time = 0;
		for(int i = 0; i < sys._instance.m_self.m_huodong_ids.Count;++i)
		{
			if(id == sys._instance.m_self.m_huodong_ids[i])
			{
				_time = sys._instance.m_self.m_end_time[i];
				break;
			}
		}
		return _time;
	}

	public string get_name(int id)
	{
		string _name = "";
		for(int i = 0; i < sys._instance.m_self.m_huodong_ids.Count;++i)
		{
			if(id == sys._instance.m_self.m_huodong_ids[i])
			{
				_name = sys._instance.m_self.m_huodong_name[i];
				break;
			}
		}
		return _name;
	}

	public bool has(int id)
	{
		if(id == 1)
	    {
			return true;
		}
		if (id == 9) 
		{
			if( sys._instance.m_self.m_t_player.huodong_czjh_index < game_data._instance.m_dbc_huodong_czjh.get_y())
			{
				return true;
			}
		}
		return false;
	}

	public bool is_effect(int id)
	{
		if (id == 2) 
		{
			if (sys._instance.m_self.m_can_pttq == 1)
			{
				return true;
			}
		}

		if (id == 9) 
		{
			if (czjh_gui.is_effect() )
			{
				return true;
			}
		}

		for(int i = 0; i <  sys._instance.m_self.m_huodong_ids.Count;++i)
		{
			if(id == sys._instance.m_self.m_huodong_ids[i] || id == sys._instance.m_self.m_huodong_ids[i]/10000)
			{
				return true;
			}
		}


		return false;
	}
	
	public static bool is_effect()
	{
		// 普天同庆
		if (sys._instance.m_self.m_can_pttq == 1)
		{
			return true;
		}

		if (czjh_gui.is_effect() )
		{
			return true;
		}

		if(sys._instance.m_self.m_huodong_ids.Count > 0)
		{
			return true;
		}

		return false;
	}
}
