
using UnityEngine;
using System.Collections;

public class defeated_Result : MonoBehaviour {

	public GameObject m_tip;
	public GameObject m_des;
	public GameObject m_effect1;
	public GameObject m_effect2;
	public GameObject m_effect3;
	public GameObject m_effect4;

	// Use this for initialization
	void Start () {


	}

	public void OnEnable()
	{
		string _text = "";

		//1消灭全部敌人 2N回合消灭全部敌人 3我方死亡人数不超过N人 4我方剩余血量不低于总血量的N%
		
		//m_victory_type = _t_ttt.tjtype;
		//m_victory_die = 0;
		//m_victory_hp = 0.0f;

		/*if(battle_logic._instance.m_battle_cur.m_value > battle_logic._instance.get_max_battle_cur())
		{
			_text = game_data._instance.get_t_language ("defeated_Result.cs_31_11");//超过最大回合数
			m_des.SetActive(true);
		}
		else if(battle_logic._instance.m_victory_type == 2)
		{
			_text = game_data._instance.get_t_language ("buttle_result.cs_95_24");//我方全灭
			m_des.SetActive(true);
		}
		else if(battle_logic._instance.m_victory_type == 3)
		{
			_text = game_data._instance.get_t_language ("defeated_Result.cs_41_11") + battle_logic._instance.m_victory_die + game_data._instance.get_t_language ("defeated_Result.cs_41_61");//我方阵亡超过//人
			m_des.SetActive(true);
		}
		else if(battle_logic._instance.m_victory_type == 4)
		{
			_text = game_data._instance.get_t_language ("defeated_Result.cs_46_11") + (int)(battle_logic._instance.m_victory_hp * 100) + "%";//我方剩余血量低于总血量的
			m_des.SetActive(true);
		}
		else
		{
			m_des.SetActive(false);
		}*/


		m_des.GetComponent<UILabel>().text = _text;
	}
	public void set_tip(bool show)
	{
		m_tip.SetActive (show);
		is_effect ();
	}

	public void is_effect()
	{
		bool flag = false;
		int a = 0;
		ccard card;
		for(int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count ; ++i)
		{
			card = sys._instance.m_self.get_card_guid (sys._instance.m_self.m_t_player.zhenxing[i]);
			if(card == null)
			{
				continue;
			}
			if(chong_neng_gui.is_chongneng(card))
			{
				m_effect1.SetActive(true);
				flag = true;
				break;
			}
		}
		if(!flag)
		{
			m_effect1.SetActive(false);
		}
		flag = false;
		for(int i = 0 ; i < sys._instance.m_self.m_t_player.zhenxing.Count ; ++i)
		{
			card = sys._instance.m_self.get_card_guid (sys._instance.m_self.m_t_player.zhenxing[i]);
			if(card == null )
			{
				continue;
			}

			if(a == 5)
			{
				if(tu_po_gui.is_tupo(card))
				{
					m_effect3.SetActive(true);
					flag = true;
					break;
				}
			}
			if(flag)
			{
				break;
			}
		}
		if(!flag)
		{
			m_effect3.SetActive(false);
		}
		flag = false;
		for(int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
		{
			card = sys._instance.m_self.get_card_guid (sys._instance.m_self.m_t_player.zhenxing[i]);
			if(card == null )
			{
				continue;
			}
			for(int j =0; j< 4;++j)
			{
				dhc.equip_t _equip = card.m_equip[j];
				if(_equip == null)
				{
					continue;
				}
				else
				{
					if(equip.is_enhance(_equip))
					{
						m_effect4.SetActive(true);
						flag = true;
						break;
					}
				}
			}
			if(flag)
			{
				break;
			}
		}
		if(!flag)
		{
			m_effect4.SetActive(false);
		}
		flag = false;
		for(int i = 0 ; i < sys._instance.m_self.m_t_player.zhenxing.Count ; ++i)
		{
			card = sys._instance.m_self.get_card_guid (sys._instance.m_self.m_t_player.zhenxing[i]);
			if(card == null )
			{
				continue;
			}
			if(jinjie_gui.is_jinjie(card))
			{
				m_effect2.SetActive(true);
				flag = true;
				break;
			}
		}
		if(!flag)
		{
			m_effect2.SetActive(false);
		}
	}
	public void click(GameObject obj)
	{
		if(sys._instance.m_game_state == "hall")
		{
			return ;
		}

		sys._instance.m_game_state = "hall";
		sys._instance.load_scene_ex (sys._instance.m_hall_name);

		this.transform.gameObject.SetActive (false);


		if(obj.name == "sj")
		{
			s_message _mes = new s_message();
			_mes.m_type = "return_to_main_ex";
			cmessage_center._instance.add_message(_mes);

			_mes = new s_message();
			_mes.m_type = "buzheng_show";
			cmessage_center._instance.add_message(_mes);
		}

		if(obj.name == "sd")
		{
			s_message _mes = new s_message();
			_mes.m_type = "return_to_main_ex";
			cmessage_center._instance.add_message(_mes);

			_mes = new s_message();
			_mes.m_type = "buzheng_show";
			cmessage_center._instance.add_message(_mes);
		}

		if(obj.name == "zb")
		{
			s_message _mes = new s_message();
			_mes.m_type = "return_to_main_ex";
			cmessage_center._instance.add_message(_mes);

			_mes = new s_message();
			_mes.m_type = "buzheng_show";
			cmessage_center._instance.add_message(_mes);
		}

		if(obj.name == "jj")
		{
			s_message _mes = new s_message();
			_mes.m_type = "return_to_main_ex";
			cmessage_center._instance.add_message(_mes);
			
			_mes = new s_message();
			_mes.m_type = "buzheng_show";
			cmessage_center._instance.add_message(_mes);
		}
		battle._instance.result_click (null);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
