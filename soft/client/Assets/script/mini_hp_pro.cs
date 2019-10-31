
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mini_hp_pro : MonoBehaviour {

	public GameObject m_hp;
	public GameObject m_mp_label;
	public List<GameObject> m_mps = new List<GameObject>();
	private GameObject m_unit;
	public GameObject m_att;
	public float m_show_time = 0;
	public List<GameObject> m_buffer = new List<GameObject>();
	public GameObject m_star;
	public GameObject m_name;
	private float m_zskill_time;
	public GameObject m_zskill;
    public GameObject m_hp_hundun;

	private bool m_start = true;
	// Use this for initialization

	public void init(GameObject unit, string name)
	{
        m_unit = unit;
		m_name.GetComponent<UILabel>().text = name;
	}
    public void init_dir(GameObject unit, string name)
    {
          m_unit = unit;
		  m_name.GetComponent<UILabel>().text = name;
          m_name.SetActive(true);
    }
	public void show_time(float time)
	{
		m_show_time = time;
		this.gameObject.SetActive (true);
	}

	public void set_buffer(List<s_t_skill> skills, List<int> cols)
	{
		for(int i = 0;i < m_buffer.Count;i ++)
		{
			m_buffer[i].SetActive(false);
		}

		int _id = 0;
		for(int i = 0;i < skills.Count && _id < m_buffer.Count;i ++)
		{
			if(skills[i].buffer_types[cols[i]] == 1)
			{
				if(skills[i].buffer_attack_types[cols[i]] == 3)
				{
					m_buffer[_id].SetActive(true);
					//s = game_data._instance.get_t_language ("mini_hp_pro.cs_52_9");//疗
					m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_heal";
					//m_buffer[_id].transform.GetComponent<UILabel>().color = new Color(0,1,0);
				}
				else
				{
					m_buffer[_id].SetActive(true);
					//s = game_data._instance.get_t_language ("mini_hp_pro.cs_59_9");//伤
					m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_heal_b";
					//m_buffer[_id].transform.GetComponent<UILabel>().color = new Color(1,0,0);
				}

				_id ++;
			}
			else
			{
				m_buffer[_id].SetActive(true);
			}
		}

		for(int i = 0;i < skills.Count && _id < m_buffer.Count;i ++)
		{
			int _att = skills[i].buffer_modify_att_types[cols[i]];
			float _val = skills[i].buffer_modify_att_vals[cols[i]];

			switch(_att)
			{
				case 7:
					{
						m_buffer[_id].SetActive(true);
						//s = game_data._instance.get_t_language ("mini_hp_pro.cs_82_10");//攻
						//m_buffer[_id].transform.GetComponent<UILabel>().text = s;

						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_attach";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_attach_b";
						}
					}
					break;
				case 8:
				case 9:
					{
						m_buffer[_id].SetActive(true);
						//s = game_data._instance.get_t_language ("card_dialog_box.cs_372_10");//防
						//m_buffer[_id].transform.GetComponent<UILabel>().text = s;
						
						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_fangs";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_fangs_b";
						}
					}
					break;
				case 14:
                    m_buffer[_id].SetActive(true);
						//s = game_data._instance.get_t_language ("mini_hp_pro.cs_114_10");//免
						//m_buffer[_id].transform.GetComponent<UILabel>().text = s;
						
						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_immunity";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_immunity_b";
						}
                        break;
				case 15:
					{
						m_buffer[_id].SetActive(true);
						//s = game_data._instance.get_t_language ("mini_hp_pro.cs_114_10");//免
						//m_buffer[_id].transform.GetComponent<UILabel>().text = s;
						
						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_immunity";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_immunity_b";
						}
					}
					break;
                case 16:
                    m_buffer[_id].SetActive(true);
						//s = game_data._instance.get_t_language ("mini_hp_pro.cs_144_10");//命
						//m_buffer[_id].transform.GetComponent<UILabel>().text = s;
						
						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_hp";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_hp_b";
						}
                    break;
                case 17:
                     m_buffer[_id].SetActive(true);
						//s = game_data._instance.get_t_language ("mini_hp_pro.cs_158_10");//闪
						//m_buffer[_id].transform.GetComponent<UILabel>().text = s;
						
						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_dodge";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_dodge_b";
						}
                    break;
                case 18:
                     m_buffer[_id].SetActive(true);
						/*s = game_data._instance.get_t_language ("mini_hp_pro.cs_172_10");//抗
						m_buffer[_id].transform.GetComponent<UILabel>().text = s;*/
						

						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_resistance";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_resistance_b";
						}
                    break;
                case 19:
                     m_buffer[_id].SetActive(true);
						/*s = game_data._instance.get_t_language ("mini_hp_pro.cs_186_10");//穿
						m_buffer[_id].transform.GetComponent<UILabel>().text = s;*/
						
						if(_val > 0)
						{

							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_renetration";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_renetration_b";
						}
                    break;
				case 11:
					{
						m_buffer[_id].SetActive(true);
					/*	s = game_data._instance.get_t_language ("mini_hp_pro.cs_201_10");//暴
						m_buffer[_id].transform.GetComponent<UILabel>().text = s;*/
						
						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_crit";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_crit_b";
						}
					}
					break;
				case 12:
					{
						m_buffer[_id].SetActive(true);
						/*s = game_data._instance.get_t_language ("mini_hp_pro.cs_217_10");//挡
						m_buffer[_id].transform.GetComponent<UILabel>().text = s;*/
						
						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_parry";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_parry_b";
						}
					}
					break;
				case 10:
					{
						m_buffer[_id].SetActive(true);
						/*s = game_data._instance.get_t_language ("mini_hp_pro.cs_233_10");//速
						m_buffer[_id].transform.GetComponent<UILabel>().text = s;*/
						
						if(_val > 0)
						{

							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_speed";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_speed_b";
						}
					}
					break;
				case 20:
					{
						m_buffer[_id].SetActive(true);
						/*s = game_data._instance.get_t_language ("mini_hp_pro.cs_82_10");//攻
						m_buffer[_id].transform.GetComponent<UILabel>().text = s;*/
						
						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_attach";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_attach_b";
						}
					}
					break;
				case 21:
					{
						m_buffer[_id].SetActive(true);
						/*s = game_data._instance.get_t_language ("mini_hp_pro.cs_114_10");//免
						m_buffer[_id].transform.GetComponent<UILabel>().text = s;*/
						
						if(_val > 0)
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_immunity";
						}
						else
						{
							m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_immunity_b";
						}
					}
					break;
				case 13:
					{
						m_buffer[_id].SetActive(true);
						//s = game_data._instance.get_t_language ("mini_hp_pro.cs_281_10");//晕
						m_buffer[_id].transform.GetComponent<UISprite>().spriteName = "zdbftb_dizzy_b";
					}
					break;
			};
            if (_att != 0)
            {
                _id++;
            }
		}
	}
    public void set_hudun(int self,double hp)
    {
        if (self == 1)
        {
            if (hp > 0)
            {
                m_hp_hundun.SetActive(true);
                m_hp_hundun.GetComponent<UIProgressBar>().value = (float)hp;
            }
            else
            {
                m_hp_hundun.SetActive(false);
            }

        }
        else
        {
            m_hp_hundun.SetActive(false);
        }
       
        
 
    }
	public void set_zskill(string name,string icon)
	{
		m_zskill_time = 2.0f;
		m_zskill.SetActive (true);
		m_zskill.GetComponent<UILabel>().text = name;
		m_zskill.transform.Find("icon").gameObject.GetComponent<UISprite>().spriteName = icon;
	}
	// Update is called once per frame
	void Update () {
	
		if (m_unit == null)
		{
			return;
		}

		if(m_start == true)
		{
			int _color = m_unit.GetComponent<unit>().m_glevel;
			
			for(int i = 1; i <= 5;i ++)
			{
				if(i <= _color)
				{
					this.transform.Find("s" + i.ToString()).GetComponent<UISprite>().spriteName = "xstar_002";
					this.transform.Find("s" + i.ToString()).gameObject.SetActive(true);
				}
				else
				{
					this.transform.Find("s" + i.ToString()).gameObject.SetActive(false);
				}
				
				if(i + 5 <= _color)
				{
					this.transform.Find("s" + i.ToString()).GetComponent<UISprite>().spriteName = "xstar_003";
				}
				
				if(i + 10 <= _color)
				{
					this.transform.Find("s" + i.ToString()).GetComponent<UISprite>().spriteName = "xstar_001";
				}
			}

			int num_ = _color;
			if(num_ > 5)
			{
				num_ = 5;
			}
			this.transform.Find("s1").localPosition = new Vector3(-9 * (num_ - 1) + 10, 50, 0);

			s_t_class _t_class  = m_unit.GetComponent<unit>().m_t_class;

			if(_t_class == null)
			{
				return;
			}

			m_start = false;

			m_att.SetActive(true);

			string _pro = "nzdjm_sxbs002";

			if(_t_class.job == 2)
			{
				_pro = "nzdjm_sxbs001";
			}
			else if(_t_class.job == 3)
			{
				_pro = "nzdjm_sxbs003";
			}

			m_att.GetComponent<UISprite>().spriteName = _pro;
		}

		double _max_hp = (float)m_unit.GetComponent<unit>().m_max_hp;
		double _cur_hp = (float)m_unit.GetComponent<unit>().m_cur_hp;

        double _max_hd = (float)m_unit.GetComponent<unit>().m_max_hd;
        double _cur_hd = (float)m_unit.GetComponent<unit>().m_cur_hd;
        

		double _hp = _cur_hp / _max_hp;
        double _hd = _cur_hd / _max_hd;
        if(_cur_hp > 0)
		{
			m_hp.SetActive(true);
            
		}
		else if(m_hp.activeSelf == true)
		{
			m_hp.SetActive(false);
		}
        if (_hd > 0)
        {
            m_hp_hundun.GetComponent<UISprite>().width = (int)(m_unit.GetComponent<unit>().m_max_hd / m_unit.GetComponent<unit>().m_max_hp * m_hp.GetComponent<UISprite>().width);

        }
        else 
        {
        }
        if (NGUITools.GetActive(m_hp_hundun))
        {
            m_hp_hundun.GetComponent<UIProgressBar>().value = (float)_hd;
        }

		if(NGUITools.GetActive(m_hp))
		{
			m_hp.GetComponent<UIProgressBar>().value = (float)_hp;
		}

		for(int i = 0;i < m_mps.Count;i ++)
		{
			if(m_unit.GetComponent<unit>().m_cur_mp > i && m_mps[i].activeSelf == false)

			{
				m_mps[i].SetActive(true);
			}
			else if(m_unit.GetComponent<unit>().m_cur_mp <= i && m_mps[i].activeSelf == true)
			{
				m_mps[i].SetActive(false);
			}
		}

		if(m_unit.GetComponent<unit>().m_cur_mp > 4)
		{
			m_mp_label.SetActive(true);
			m_mp_label.GetComponent<UILabel>().text = "x" + m_unit.GetComponent<unit>().m_cur_mp;
		}
		else
		{
			m_mp_label.SetActive(false);
		}

		if (m_zskill_time > 0)
		{
			m_zskill_time -= Time.deltaTime;
			if (m_zskill_time > 1.5f)
			{
				float t = (m_zskill_time - 1.5f) / 0.5f;
				m_zskill.transform.localPosition = new Vector3(-t * 70, 70, 0);
				m_zskill.GetComponent<UILabel>().alpha = 1 - t;
			}
			else if (m_zskill_time < 0.5f)
			{
				float t = (0.5f - m_zskill_time) / 0.5f;
				m_zskill.transform.localPosition = new Vector3(t * 70, 70, 0);
				m_zskill.GetComponent<UILabel>().alpha = 1 - t;
			}
			else
			{
				m_zskill.transform.localPosition = new Vector3(0, 70, 0);
				m_zskill.GetComponent<UILabel>().alpha = 1.0f;
			}
			if (m_zskill_time < 0)
			{
				m_zskill.SetActive(false);
			}
		}
	}
}
