
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class top_res : MonoBehaviour {

	private GameObject m_gold;
	private GameObject m_zs;
	private GameObject m_yl;
    private GameObject m_gx;
    private GameObject m_hor;
    private GameObject m_attack;
    private GameObject m_xunzhuang;
    private GameObject m_tili;
    private GameObject m_nl;
    private GameObject m_hejin;
    private GameObject m_zh;

	private int m_gold_value = 0;
	private int m_zs_value = 0;
	private int m_yl_value = 0;
    private int m_gx_value = 0;
    private int m_hor_value = 0;
    private int m_attack_value = 0;
    private int m_xunzhang_value = 0;
    private int m_nl_value;
    private int m_hj_value;
    private int m_zh_value;

	bool bool_yl;
    bool bool_gx;
    bool bool_hor;
    bool bool_attack;
    bool bool_xunzhang;
    bool bool_tili;
    bool bool_nl;
    bool bool_hj;
    bool bool_zh;


    bool m_show_tili;
    bool m_show_nl;

    public GameObject m_clone;
    public List<int> types;
    private List<GameObject> m_clones;
    List<int> values;
	// Use this for initialization
	void Start () 
    {
        clone();
        arrange();
	}
	
    void clone()
    {
        values = new List<int>();
        m_clones = new List<GameObject>();
        for (int i = 0; i < types.Count; i++)
        {
            GameObject obj = Instantiate(m_clone) as GameObject;
            obj.transform.parent = m_clone.transform.parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
            m_clones.Add(obj);
            if (types[i] != 1000)
            {
                values.Add(sys._instance.m_self.get_att((e_player_attr)types[i]));

            }
            else
            {
                values.Add(sys._instance.m_self.m_t_player.bf);
            }
           
        }
        
 
    }
    void arrange()
    {
        for (int i = 0; i < types.Count; i++)
        {
            if (types[i] != 1000)
            {
                s_t_resource res = game_data._instance.get_t_resource(types[i]);
                int value = sys._instance.m_self.get_att((e_player_attr)types[i]);
               
                {
                    m_clones[i].transform.Find("name").GetComponent<UILabel>().text = res.namecolor + sys._instance.value_to_wan(value);
 
                }
                if (types[i] == 3)
                {
					m_clones[i].transform.Find("name").GetComponent<UILabel>().text = res.namecolor + sys._instance.value_to_wan((long)value) + "/" + sys._instance.value_to_wan((long)sys._instance.m_self.get_max_tili());
 
                }
                if (types[i] == 15)
                {
					m_clones[i].transform.Find("name").GetComponent<UILabel>().text = res.namecolor + sys._instance.value_to_wan((long)value) + "/" + sys._instance.value_to_wan((long)sys._instance.m_self.get_max_nl());
 
                }
                m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = res.smallicon;
            }
            else
            {
                int value = sys._instance.m_self.m_t_player.bf;
                m_clones[i].transform.Find("name").GetComponent<UILabel>().text = "[F06F28]" + sys._instance.value_to_wan(value);
                if (game_data._instance.m_language == e_language.English)
                {
                    m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = "spzdl_xtb-fb";
                }
                else if (game_data._instance.m_language == e_language.Simplified)
                {
                    m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = "spzdl_xtb";
                }
                else
                {
                    m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = "spzdl_xtb_overseas";
                }
                    

                if (game_data._instance.m_language == e_language.English)
                {
					m_clones[i].transform.Find("icon").GetComponent<UISprite>().width = 50;
					if(m_clones[i].transform.Find("icon").GetComponent<UISprite>().atlas.name == "common_icon")
					{
						m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = "spzdl_xtb-fb";
					}
                    m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = "spzdl_xtb-fb";
                       
                }
                else if (game_data._instance.m_language == e_language.Simplified)
                {
                    m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = "spzdl_xtb";
                }
                else
                {
                    m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = "spzdl_xtb_overseas";
                }               
            }
            m_clones[i].transform.Find("add").gameObject.SetActive(true);
            if (types[i] == 1)
            {
                m_clones[i].transform.Find("add").name = "add_gold";
                if (this.transform.parent.name == "guild_pvp_gui(Clone)")
                {
                    m_clones[i].transform.Find("add_gold").gameObject.SetActive(false);
                }
 
            }
            else if (types[i] == 2)
            {
                m_clones[i].transform.Find("add").name = "add_zs";
                if (this.transform.parent.name == "guild_pvp_gui(Clone)")
                {
                    m_clones[i].transform.Find("add_zs").gameObject.SetActive(false);
                }
 
            }
            else if (types[i] == 3)
            {
                m_clones[i].transform.Find("add").name = "add_tl"; 
            }
            else if (types[i] == 15)
            {
                m_clones[i].transform.Find("add").name = "add_jl";
            }
            else if (types[i] == 5)
            {
                m_clones[i].transform.Find("add").name = "add_zh";
                if (this.transform.parent.name == "ying_jiu_gui(Clone)")
                {
                    m_clones[i].transform.Find("add_zh").gameObject.SetActive(false);
                }
 
            }
            else if (types[i] == 6)
            {
                m_clones[i].transform.Find("add").name = "add_hj";
                if (this.transform.parent.name == "mi_jing_gui(Clone)")
                {
                    m_clones[i].transform.Find("add_hj").gameObject.SetActive(false);
                }

            } 
            else
            {
                m_clones[i].transform.Find("add").gameObject.SetActive(false);
            }
            if (i >= 1 && m_clones[i - 1].transform.Find("add") == null)
            {
                m_clones[i].transform.localPosition = new Vector3(m_clones[i - 1].transform.localPosition.x + 155, 2, 0);
            }
            else
            {
                if (i >= 1)
                {
                    m_clones[i].transform.localPosition = new Vector3(m_clones[i - 1].transform.localPosition.x + 150, 2, 0);
                }
                else
                {
                    m_clones[i].transform.localPosition = new Vector3(0, 2, 0);
 
                }
                
            }
            
        } 
    }
	void click(GameObject obj)
	{
		if(obj.transform.name == "add_gold")
		{
			s_message _mes = new s_message();
			_mes.m_type = "show_jd_gui";
			cmessage_center._instance.add_message(_mes);
		}
		else if(obj.transform.name == "add_zs")
		{
			s_message _mes = new s_message();
			_mes.m_type = "show_recharge";
			cmessage_center._instance.add_message(_mes);
		}
		else if(obj.transform.name == "add_tl")
		{
			int item_id = 10010002;
			int num = sys._instance.m_self.get_item_num((uint)item_id);
			if(num> 0)
			{
				root_gui._instance.show_tili_dialog_box(item_id);
				return;
			}
			else
			{
				s_message _message = new s_message();
				_message.m_type = "buy_num_gui";
				_message.m_ints.Add(100200);
				_message.m_ints.Add(2);
				cmessage_center._instance.add_message(_message);
				return;
			}
		}
        else if (obj.transform.name == "add_jl")
        {
            int item_id = 10010007;
            int num = sys._instance.m_self.get_item_num((uint)item_id);
            if (num > 0)
            {
                root_gui._instance.show_tili_dialog_box(item_id);
                return;
            }
            else
            {
                s_message _message = new s_message();
                _message.m_type = "buy_num_gui";
                _message.m_ints.Add(100300);
                _message.m_ints.Add(2);
                cmessage_center._instance.add_message(_message);
                return;
            }
        }
        else if (obj.name == "add_hj")
        {
            s_message _mes = new s_message();
            _mes.m_type = "bag_recycle";
            _mes.m_ints.Add(0);
            _mes.m_object.Add(this.transform.parent.gameObject);
            cmessage_center._instance.add_message(_mes);
			sys._instance.message_clear();
			sys._instance.m_message_type.Add ("show_bag");
			this.transform.parent.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (obj.name == "add_zh")
        {
            s_message _mes = new s_message();
            _mes.m_type = "show_bag1";
            _mes.m_ints.Add(3);
            _mes.m_object.Add(this.transform.parent.gameObject);
            cmessage_center._instance.add_message(_mes);
            if (this.transform.parent.name == "bag_gui(Clone)")
            {

            }
            else
            {
                this.transform.parent.gameObject.SetActive(false);
 
            }
 
        }
	}
    void tili_show()
    {
        last_time();

        if (m_show_tili == true)
        {
            int _tili = sys._instance.m_self.get_max_tili();
            if (m_tili != null)
            {
                m_tili.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.tili + "/" + _tili;
            }
        }
        else
        {
            long _time = 360000 - (int)(timer.now() - sys._instance.m_self.m_t_player.last_tili_time);
            if (m_tili != null)
            {
                m_tili.GetComponent<UILabel>().text = timer.get_time_show(_time);
            }
            

        }

        if (m_show_nl == true)
        {
            int _max_nl = sys._instance.m_self.get_max_nl();
            if (m_nl != null)
            {
				m_nl.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)sys._instance.m_self.m_t_player.energy) + "/" + sys._instance.value_to_wan((long)_max_nl);
            }
           
        }
        else
        {
            long _time = 1800000 - (int)(timer.now() - sys._instance.m_self.m_t_player.last_energy_time);
            if (m_nl != null)
            {
                m_nl.GetComponent<UILabel>().text = timer.get_time_show(_time);
 
            }
            
        }
    }
    void last_time()
    {
        if (sys._instance.m_self == null)
        {
            return;
        }

        if (sys._instance.m_self.m_t_player == null)
        {
            return;
        }

        if (m_show_tili == true)
        {
            if (sys._instance.m_self.m_t_player.tili < sys._instance.m_self.get_max_tili())
            {
                m_show_tili = false;
            }
        }
        else
        {
            m_show_tili = true;
        }

        if (m_show_nl == true)
        {
            if (sys._instance.m_self.m_t_player.energy < sys._instance.m_self.get_max_nl())
            {
                m_show_nl = false;
            }
        }
        else
        {
            m_show_nl = true;
        }
    }

	public static void add_scale_anim(GameObject obj)
	{
		TweenScale _scale = TweenScale.Begin (obj,0.25f,new Vector3(1.0f,1.0f,1.0f));
		
		_scale.method = UITweener.Method.EaseInOut;
		_scale.from = new Vector3 (1.5f, 1.5f, 1.5f);
		_scale.to = new Vector3(1.0f,1.0f,1.0f);
		_scale.delay = 0;
	}

    void Update()
    {
        if (values != null)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] != sys._instance.m_self.get_att((e_player_attr)types[i]))
                {
                    string color = "";
                    int value = 0;
                    if (types[i] != 1000)
                    {
                        s_t_resource res = game_data._instance.get_t_resource(types[i]);
                        values[i] = sys._instance.m_self.get_att((e_player_attr)types[i]);
                        color = res.namecolor;
                        value = values[i];
                    }
                    else
                    {
                        values[i] = sys._instance.m_self.m_t_player.bf;
                        color = "[daaf28]";
                        value = values[i];
                    }
                    m_clones[i].transform.Find("name").GetComponent<UILabel>().text = color + sys._instance.value_to_wan(value);
                    if (types[i] == 3)
                    {
						m_clones[i].transform.Find("name").GetComponent<UILabel>().text = color + sys._instance.value_to_wan((long)value) + "/" + sys._instance.value_to_wan((long)sys._instance.m_self.get_max_tili());
                    }
                    if (types[i] == 15)
                    {
						m_clones[i].transform.Find("name").GetComponent<UILabel>().text = color + sys._instance.value_to_wan((long)value) + "/" + sys._instance.value_to_wan((long)sys._instance.m_self.get_max_nl());
                    }
					if (types[i] == 1000)
					{
                        if (game_data._instance.m_language == e_language.English)
                        {
                            m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = "spzdl_xtb-fb";
                        }
                        else if (game_data._instance.m_language == e_language.Simplified)
                        {
                            m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = "spzdl_xtb";
                        }
                        else
                        { 
                            m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = "spzdl_xtb_overseas";
                        }
					}
					else
					{
						s_t_resource res1 = game_data._instance.get_t_resource(types[i]);
						m_clones[i].transform.Find("icon").GetComponent<UISprite>().spriteName = res1.smallicon;
					}
                    add_scale_anim(m_clones[i].transform.Find("name").gameObject);

                }
                
            }
 
 
        }
       
    }
	void Update1 () {
	
		if( sys._instance.m_self == null)
		{
			return;
		}

		if(m_gold_value != sys._instance.m_self.get_att(e_player_attr.player_gold))
		{
			m_gold_value = sys._instance.m_self.get_att(e_player_attr.player_gold);
            m_gold.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_gold_value);			
		}

		if(m_zs_value != sys._instance.m_self.get_att(e_player_attr.player_jewel))
		{
			m_zs_value = sys._instance.m_self.get_att(e_player_attr.player_jewel);
            m_zs.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_zs_value);
			add_scale_anim(m_zs);
		}
		if(bool_yl)
		{
			if(m_yl_value != sys._instance.m_self.get_att(e_player_attr.player_yuanli))
			{
				m_yl_value = sys._instance.m_self.get_att(e_player_attr.player_yuanli);
                m_yl.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_yl_value);
				add_scale_anim(m_yl);
			}

		}
        if (bool_gx)
        {
            if (m_gx_value != sys._instance.m_self.get_att(e_player_attr.player_contribution))
            {
                m_gx_value = sys._instance.m_self.get_att(e_player_attr.player_contribution);
                m_gx.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_gx_value);
                add_scale_anim(m_gx);
            }

        }
        if (bool_hor)
        {
            if (m_hor_value != sys._instance.m_self.get_att(e_player_attr.player_guild_hor))
            {
                m_hor_value = sys._instance.m_self.get_att(e_player_attr.player_guild_hor);
                m_hor.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_hor_value);
                add_scale_anim(m_hor);
            }

        }
        if (bool_attack)
        {
            if (m_attack_value != sys._instance.m_self.m_t_player.bf)
            {
                m_attack_value = sys._instance.m_self.m_t_player.bf;
                m_attack.GetComponent<UILabel>().text = m_attack_value.ToString();
                m_attack.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_attack_value);
                add_scale_anim(m_attack);
            }
 
        }
        if (bool_xunzhang)
        {
            if (m_xunzhang_value != sys._instance.m_self.get_att(e_player_attr.player_medal_point))
            {
                m_xunzhang_value = sys._instance.m_self.get_att(e_player_attr.player_medal_point);
                m_xunzhuang.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_xunzhang_value);
                add_scale_anim(m_xunzhuang);
            }

        }
       
        if (bool_nl)
        {
            if (m_nl_value != sys._instance.m_self.get_att(e_player_attr.player_treasure_energy))
            {
                m_nl_value = sys._instance.m_self.get_att(e_player_attr.player_treasure_energy);
                m_nl.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_nl_value);
                add_scale_anim(m_nl);
            }

        }
        if (bool_hj)
        {
            if (m_hj_value != sys._instance.m_self.get_att(e_player_attr.player_hj))
            {
                m_hj_value = sys._instance.m_self.get_att(e_player_attr.player_hj);
                m_hejin.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_hj_value);
                add_scale_anim(m_hejin);
            }

        }
        if (bool_zh)
        {
            if (m_zh_value != sys._instance.m_self.get_att(e_player_attr.player_jjc_point))
            {
                m_zh_value = sys._instance.m_self.get_att(e_player_attr.player_jjc_point);
                m_zh.GetComponent<UILabel>().text = m_zh_value.ToString();
                add_scale_anim(m_zh);
            }

        }
	}
}
