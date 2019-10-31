
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tanbao_gui : MonoBehaviour,IMessage{

	public static tanbao_gui _instance;
	public GameObject m_touzi;
	public GameObject m_touzis;
	public GameObject[] m_shuzi;
	public GameObject m_one;
	public GameObject m_ten;
	public GameObject m_close;
	public GameObject[] m_effects;
    public GameObject[] m_num_touzis;
	public GameObject m_huodong_time;
	public GameObject m_score;
	public GameObject m_norank;
	public GameObject m_rank;
	public GameObject m_rank_button;
	public GameObject m_reward_button;
	public GameObject m_sm_button;
	public GameObject m_luck_touzi;
	public GameObject m_luck_shop;
	public GameObject m_free_num;
	public GameObject m_one_jewel;
	public GameObject m_ten_jewel;
	public GameObject m_up_down;
	public GameObject m_up;
	public GameObject m_down;
	public GameObject m_use_items_gui;
	public GameObject m_tanbao_shop_gui;
	public GameObject m_luck_touzi_gui;
	public GameObject m_scro;
	public GameObject m_touzi_num;
	public GameObject m_qidian_reward_gui;
	public GameObject m_name;
	public GameObject m_task_point;
	public GameObject m_rank_gui;
	public GameObject m_gold_show;
	public GameObject m_daren_rank_gui;
	public GameObject m_luck_touzi_num;
	public GameObject m_start_num;
	public GameObject m_info_gui;
	public GameObject m_info_scro;
	public static bool press = false;
	public List<GameObject> m_icons = new List<GameObject>();
	public List<GameObject> m_names = new List<GameObject>();
	public List<GameObject> m_scores = new List<GameObject>();
	public List<GameObject> m_chenghaos = new List<GameObject>();
	public ulong m_end_time;
	public static protocol.game.smsg_huodong_tanbao_view  _msg;
	private s_t_tanbao_shop t_tanbao_shop;
	public int score = 0;
	private int touzi_num = 0;
	public int qidian_num = 0;
	private int m_index = 0;
	private List<int> num = new List<int>();
	private float m_time = 0 ;
	private int x_flag = 0;
	public static int site =0;
	private int tou_num;
	private int buy_num = 0 ;
	private int final_site;
	private int my_rank = 0;
	private List<int> yidong = new List<int>();
	private List<int> gezi = new List<int>() ;
	private List<int> m_event = new List<int>();
	private List<int> gold = new List<int>();
	private List<int> item = new List<int>();
	private List<int> shop_types = new List<int>();
    private List<int> shop_types_nums = new List<int>();

	private List<int> shop_ids = new List<int>();
	private List<int> shop_nums = new List<int>();
	public  List<int> rewards = new List<int>();

	//private int shop_type = 0;
	private int select = 0;
    int walknum;
	private float m_yidong_time = 0;
	private int select_num = 0;
	private int m_input_num = 0;
	private List<string> names = new List<string>();
	private List<int> points = new List<int>();
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void Awake()
	{
		_instance = this;
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnEnable()
	{
		InvokeRepeating ("time", 0.0f, 1.0f);
		score = _msg.point;
		qidian_num = _msg.final_num;
		touzi_num = _msg.luck_num;
		site = _msg.gezi;
		tou_num = _msg.dice_num;
		buy_num = _msg.buy_num;

		shop_ids = _msg.shop_ids;
		shop_nums = _msg.shop_nums;
		
      //  shop_type = _msg.shop_type;


        shop_types = _msg.shop_types;
        shop_types_nums = _msg.shop_type_nums;
		
        
        rewards = _msg.rewards;
		my_rank = _msg.rank;
		m_end_time = _msg.time;
		m_one.SetActive (true);
		m_ten.SetActive (true);
		m_one_jewel.SetActive(true);
		m_ten_jewel.SetActive(true);
		reset();
		if(site <= 0)
		{
			m_one.SetActive (false);
			m_ten.SetActive (false);
			m_free_num.SetActive(false);
			m_one_jewel.SetActive(false);
			m_ten_jewel.SetActive(false);
			m_luck_shop.SetActive(false);
			m_luck_touzi.SetActive(false);
			daren_rank();
			m_daren_rank_gui.SetActive(true);
		}
		else
		{
			m_daren_rank_gui.SetActive (false);
		}
	}

	void OnDisable()
	{
		CancelInvoke ("time");
		CancelInvoke ("effect");
	}

	void time()
	{
		long _time = (long)(m_end_time - timer.now());
		m_huodong_time.GetComponent<UILabel>().text = timer.get_time_show_ex(_time);
		if(_time <= 0)
		{
			m_one.SetActive (false);
			m_ten.SetActive (false);
			m_free_num.SetActive(false);
			m_one_jewel.SetActive(false);
			m_ten_jewel.SetActive(false);
			m_luck_shop.SetActive(false);
			m_luck_touzi.SetActive(false);
			daren_rank();
			m_daren_rank_gui.SetActive(true);
			m_huodong_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chongzhifali_gui.cs_68_42");//活动已结束
			CancelInvoke ("time");

		}
	}

	void daren_rank()
	{
		for(int i = 0; i < _msg.names.Count;++i)
		{
			sys._instance.remove_child(m_icons[i]);
			m_icons[i].transform.parent.gameObject.SetActive(true);
			GameObject _obj1 = icon_manager._instance.create_player_icon((int)_msg.ids[i], _msg.achieves[i], _msg.vips[i],_msg.nalflag[i]);
			_obj1.transform.parent = m_icons[i].transform;
			_obj1.transform.localScale = new Vector3(1, 1, 1);
			_obj1.transform.localPosition = new Vector3(0, 0, 0);
			m_names[i].GetComponent<UILabel>().text = game_data._instance.get_name_color(_msg.achieves[i]) + _msg.names[i].ToString();
			m_scores[i].GetComponent<UILabel>().text = game_data._instance.get_t_language ("explore_gui.cs_268_46") + _msg.points[i];//积分:
			sys._instance.get_chenghao(_msg.chenghaos[i],m_chenghaos[i]);
		}
		for(int i = _msg.names.Count ; i < m_icons.Count;++i)
		{
			m_icons[i].transform.parent.gameObject.SetActive(false);
			m_names[i].GetComponent<UILabel>().text = "";
			m_scores[i].GetComponent<UILabel>().text = "";
			sys._instance.get_chenghao(0,m_chenghaos[i]);
		}
	}
	
	public void effect()
	{
		if(m_effects[x_flag].activeSelf)
		{
			m_effects[x_flag].SetActive(false);
			x_flag+=1;
			if(x_flag >= 2)
			{
				x_flag = 0;
			}
			int num = x_flag;
			m_effects [num].SetActive(true);
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			press = false;
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
			sys._instance.m_game_state = "hall";
			sys._instance.load_scene(sys._instance.m_hall_name);
			this.gameObject.SetActive(false);
		}
		if(obj.transform.name == "shop_close")
		{
			press = false;
			m_tanbao_shop_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "one")
		{
			if(tou_num <= 0)
			{
				s_t_price t_price = game_data._instance.get_t_price(buy_num +1);
				if(sys._instance.m_self.m_t_player.jewel < t_price.tanbao_price)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
					return;
				}
			}
			if(shop_types.Count > 0)
			{
				string _des = game_data._instance.get_t_language ("主人,神秘商店还有道具没有购买,继续往前走,就会错过哦~");
				s_message _msg = new s_message();
				_msg.m_type = "one_forward";
				root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_msg);//提示
			}
			else
			{
				select = 0;
				protocol.game.cmsg_tanbao_dice _msg = new protocol.game.cmsg_tanbao_dice();
				_msg.dice = 0;
				_msg.type = 0;
				net_http._instance.send_msg<protocol.game.cmsg_tanbao_dice>(opclient_t.CMSG_HUODONG_TANBAO_DICE,_msg);
			}
		}
		if(obj.transform.name == "ten")
		{
			int jewel = 0;
			for(int i = 1; i <= 10- tou_num;++i)
			{
				s_t_price _price = game_data._instance.get_t_price(i + buy_num);
				jewel += _price.tanbao_price;
			}
			if(sys._instance.m_self.m_t_player.jewel <jewel)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
				return;
			}
			if(shop_types.Count > 0)
			{
				string _des = game_data._instance.get_t_language ("主人,神秘商店还有道具没有购买,继续往前走,就会错过哦~");
				s_message _msg = new s_message();
				_msg.m_type = "ten_forward";
				root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_msg);//提示
			}
			else
			{
				select = 1;
				protocol.game.cmsg_tanbao_dice _msg = new protocol.game.cmsg_tanbao_dice();
				_msg.dice = 0;
				_msg.type = 1;
				net_http._instance.send_msg<protocol.game.cmsg_tanbao_dice>(opclient_t.CMSG_HUODONG_TANBAO_DICE,_msg);
			}
		}
		if(obj.transform.name == "lucky")
		{
			press = true;
			m_luck_touzi_gui.SetActive(true);
            int num = touzi_num;
            if (num > 10)
            {
                num = 10;
            }
            for (int i = 0; i < m_num_touzis.Length; i++)
            {
                m_num_touzis[i].transform.Find("change").Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("tanbao_gui.cs_297_126"), num);//走{0}次
            }
            m_touzi_num.GetComponent<UILabel>().text = touzi_num.ToString();
		}
		if(obj.transform.name == "0" || obj.transform.name == "1" || obj.transform.name == "2"
           || obj.transform.name == "3" || obj.transform.name == "4" || obj.transform.name == "5" || obj.name == "change")
		{
			press = false;
			
            if (obj.name == "change")
            {
                select_num = int.Parse(obj.transform.parent.name) + 1;
            }
            else
            {
                select_num = int.Parse(obj.transform.name) + 1;
 
            }
			if(shop_types.Count > 0)
			{
				string _des = game_data._instance.get_t_language ("主人,神秘商店还有道具没有购买,继续往前走,就会错过哦~");
				s_message _msg = new s_message();
                if (obj.name == "change")
                {
                    _msg.m_ints.Add(3);
                }
                else
                {
                    _msg.m_ints.Add(2);
 
                }
				_msg.m_type = "touzi_forward";
				root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_msg);//提示
			}
			else
			{
				m_one.GetComponent<BoxCollider>().enabled = false;
				m_ten.GetComponent<BoxCollider>().enabled = false;
				m_close.GetComponent<BoxCollider>().enabled = false;
				m_luck_shop.GetComponent<BoxCollider>().enabled = false;
				m_luck_touzi.GetComponent<BoxCollider>().enabled = false;
				m_rank_button.GetComponent<BoxCollider>().enabled = false;
				m_reward_button.GetComponent<BoxCollider>().enabled = false;
				m_sm_button.GetComponent<BoxCollider>().enabled = false;
				m_luck_touzi_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                if (obj.name != "change")
                {
                    select = 2;
                    protocol.game.cmsg_tanbao_dice _msg = new protocol.game.cmsg_tanbao_dice();
                    _msg.dice = select_num;
                    _msg.type = 2;
                    net_http._instance.send_msg<protocol.game.cmsg_tanbao_dice>(opclient_t.CMSG_HUODONG_TANBAO_DICE, _msg);

                }
                else
                {
                    select = 3;
                    protocol.game.cmsg_tanbao_dice _msg = new protocol.game.cmsg_tanbao_dice();
                    _msg.dice = select_num;
                    _msg.type = 3;
                    net_http._instance.send_msg<protocol.game.cmsg_tanbao_dice>(opclient_t.CMSG_HUODONG_TANBAO_DICE, _msg);
                }
				
			}
		}
		if(obj.transform.name == "cancle")
		{
			press = false;
			m_luck_touzi_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "reward")
		{
			press = true;
			m_qidian_reward_gui.SetActive(true);
		}
		if(obj.transform.name == "shop")
		{
			press = true;
			shop ();
			m_tanbao_shop_gui.SetActive(true);
		}
		if(obj.transform.name == "rank")
		{
			press = true;
			m_rank_gui.SetActive(true);
		}
		if(obj.transform.name == "sm")
		{
			if(m_info_scro.transform.GetComponent<SpringPanel>() != null)
			{
				m_info_scro.transform.GetComponent<SpringPanel>().enabled = false;
			}
			m_info_scro.transform.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
			m_info_scro.transform.localPosition = new Vector3 (0, 0, 0);
			press = true;
			m_info_gui.SetActive(true);
		}
		if(obj.transform.name == "sm_close")
		{
			press = false;
			m_info_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	public void reset()
	{
		m_score.GetComponent<UILabel>().text = score.ToString ();
		m_norank.SetActive (true);
		m_rank.SetActive (false);
		m_luck_touzi.SetActive (false);
		m_luck_shop.SetActive (false);
		if(touzi_num > 0)
		{
			m_luck_touzi.SetActive(true);
		}
		if(tou_num > 0)
		{
			m_free_num.SetActive(true);
			m_one_jewel.SetActive(false);
			m_free_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("explore_gui.cs_323_59") , tou_num );//免费{0}次
		}
		else
		{
			s_t_price t_price = game_data._instance.get_t_price(buy_num +1);
			string color = "[ff0000]";
			if(sys._instance.m_self.m_t_player.jewel >= t_price.tanbao_price)
			{
				color = sys._instance.get_res_color(2);
			}
			m_free_num.SetActive(false);
			m_one_jewel.SetActive(true);
			m_one_jewel.transform.Find("one").GetComponent<UILabel>().text = color + "x" + t_price.tanbao_price;
		}
	//	shop_types.Clear ();
	//	for(int i = 1; i <= 3;++i)
	//	{
			//if((shop_type & (1 << i)) != 0)
	//		{
	//			shop_types.Add(i);
	//		}
	//	}
		if(shop_all_buy())
		{
			shop_types.Clear();
            shop_types_nums.Clear();
		}
		int jewel = 0;
		for(int i = 1; i <= 10 - tou_num;++i)
		{
			s_t_price _price = game_data._instance.get_t_price(i + buy_num);
			jewel += _price.tanbao_price;
		}
		string color1 = "[ff0000]";
		if(sys._instance.m_self.m_t_player.jewel >= jewel)
		{
			color1 = sys._instance.get_res_color(2);
		}
		m_ten_jewel.transform.Find("ten").GetComponent<UILabel>().text = color1 + "x" +jewel;
		if(shop_types.Count > 0)
		{
			m_luck_shop.SetActive(true);
		}
		else
		{
			m_luck_shop.SetActive(false);
		}
		if(tanbao_reward_gui.effect())
		{
			m_task_point.SetActive(true);
		}
		else
		{
			m_task_point.SetActive(false);
		}
		if(my_rank == 0)
		{
			m_norank.SetActive(true);
			m_rank.SetActive(false);
		}
		else
		{
			m_norank.SetActive(false);
			m_rank.SetActive(true);
			m_rank.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"), my_rank.ToString() );//第{0}名
		}
		m_luck_touzi_num.GetComponent<UILabel>().text = "x" + touzi_num.ToString ();
		m_start_num.GetComponent<UILabel>().text = qidian_num.ToString ();
	}

	bool shop_all_buy()
	{
        List<s_t_tanbao_shop> tanbao_shops = new List<s_t_tanbao_shop>(); 
		foreach(int id in game_data._instance.m_dbc_tanbao_shop.m_index.Keys)
		{
			s_t_tanbao_shop t_tanbao_shop = game_data._instance.get_t_tanbao_shop(id);
            int index = shop_types.IndexOf(t_tanbao_shop.shop_type, 0);
            if (index != -1)
            {
                t_tanbao_shop.buy_num = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(10, id)) * shop_types_nums[index];
                tanbao_shops.Add(t_tanbao_shop);

            }
		}
		bool flag = false;
        if (shop_ids.Count == tanbao_shops.Count)
		{
			for(int i = 0; i < shop_nums.Count;++i)
			{
				flag = false;
                s_t_tanbao_shop _tanbao_shop = tanbao_shops[i];
				if(_tanbao_shop.buy_num <= shop_nums[i])
				{
					flag = true;
				}
			}
		}
		if(flag)
		{
			return true;
		}
		return false;
	}

	void IMessage.message (s_message message)
	{
		if (message.m_type == "bz_move_finish")
		{
			m_touzi.SetActive(false);
			m_touzis.SetActive(false);
			m_one.GetComponent<BoxCollider>().enabled = true;
			m_ten.GetComponent<BoxCollider>().enabled = true;
			m_close.GetComponent<BoxCollider>().enabled = true;
			m_luck_shop.GetComponent<BoxCollider>().enabled = true;
			m_luck_touzi.GetComponent<BoxCollider>().enabled = true;
			m_rank_button.GetComponent<BoxCollider>().enabled = true;
			m_reward_button.GetComponent<BoxCollider>().enabled = true;
			m_sm_button.GetComponent<BoxCollider>().enabled = true;
			if(select == 0 || select == 2)
			{
                shop_types.Clear();
                shop_types_nums.Clear();
				m_name.GetComponent<UILabel>().text = game_data._instance.get_t_language ("tanbao_gui.cs_479_42");//恭喜获得以下物品
				s_t_tanbao_event t_tanbao_event = game_data._instance.get_t_tanbao(gezi[m_index]);
				if(t_tanbao_event.type == 5)
				{
					sys._instance.m_self.add_att(e_player_attr.player_gold,gold[m_index],false,game_data._instance.get_t_language ("tanbao_gui.cs_483_80"));//探宝获得
					m_gold_show.GetComponent<zhuang_gui>().m_num = gold[m_index];
					m_gold_show.SetActive(true);
				}
				else if(t_tanbao_event.type == 1)
				{
                    sys._instance.m_self.add_reward(t_tanbao_event.rtype, t_tanbao_event.rvalue1, t_tanbao_event.rvalue2, 0, false, game_data._instance.get_t_language ("tanbao_gui.cs_483_80"));//探宝获得
					m_gold_show.GetComponent<zhuang_gui>().m_num = t_tanbao_event.rvalue2;
					m_gold_show.SetActive(true);
				}
				else if(t_tanbao_event.type == 4)
				{
					root_gui._instance.action_guide(t_tanbao_event.juqings[m_event[m_index]]);
				}
				else if(t_tanbao_event.type == 2)
				{
                    sys._instance.m_self.add_reward(t_tanbao_event.rtype, t_tanbao_event.rvalue1, item[m_index], 0, false, game_data._instance.get_t_language ("tanbao_gui.cs_483_80"));//探宝获得
					m_use_items_gui.GetComponent<use_items_gui>().init();
					m_use_items_gui.GetComponent<use_items_gui>().types.Add(t_tanbao_event.rtype);
					m_use_items_gui.GetComponent<use_items_gui>().values1.Add(t_tanbao_event.rvalue1);
					m_use_items_gui.GetComponent<use_items_gui>().values2.Add(item[m_index]);
					m_use_items_gui.GetComponent<use_items_gui>().values3.Add(0);
					m_use_items_gui.SetActive(true);

				}
				else if(t_tanbao_event.type == 6)
				{
					touzi_num += 1;
					m_use_items_gui.GetComponent<use_items_gui>().init();
					m_use_items_gui.GetComponent<use_items_gui>().types.Add(2);
					m_use_items_gui.GetComponent<use_items_gui>().values1.Add(1);
					m_use_items_gui.GetComponent<use_items_gui>().values2.Add(1);
					m_use_items_gui.GetComponent<use_items_gui>().values3.Add(0);
					m_use_items_gui.SetActive(true);
				}
				else if(t_tanbao_event.type == 3)
				{
					shop_types.Clear();
                    shop_types_nums.Clear();
					shop_types.Add(t_tanbao_event.shop_type);
                    shop_types_nums.Add(1);
				//	shop_type = shop_type | (1 << t_tanbao_event.shop_type);
					shop_ids.Clear();
					shop_nums.Clear();
					shop ();
					m_tanbao_shop_gui.SetActive(true);
				}
				else if(t_tanbao_event.type == 7)
				{
					return;
				}
				else
				{
					sys._instance.m_self.add_reward(t_tanbao_event.rtype,t_tanbao_event.rvalue1,t_tanbao_event.rvalue2,0,game_data._instance.get_t_language ("tanbao_gui.cs_483_80"));//探宝获得
					m_use_items_gui.GetComponent<use_items_gui>().init();
					m_use_items_gui.GetComponent<use_items_gui>().types.Add(t_tanbao_event.rtype);
					m_use_items_gui.GetComponent<use_items_gui>().values1.Add(t_tanbao_event.rvalue1);
					m_use_items_gui.GetComponent<use_items_gui>().values2.Add(t_tanbao_event.rvalue2);
					m_use_items_gui.GetComponent<use_items_gui>().values3.Add(0);
					m_use_items_gui.SetActive(true);
				}
			}
			else if(select == 1 || select == 3)
			{
				int m_score = 0;
				for(int i = 0; i < num.Count;i++)
				{
					if(yidong[i] == 1)
					{
						continue;
					}
					m_score += num[i];
				}
                if (select == 1)
                {
                    m_name.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("主人,走10次后获得{0}积分,以及以下奖励,恭喜"), m_score);
                }
                else
                {
                    m_name.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("主人,走{0}次后获得{1}积分,以及以下奖励,恭喜"), walknum,m_score);

                }
			
				shop_types.Clear();
                shop_types_nums.Clear();
				List<s_t_reward> rewards = new List<s_t_reward>();
				for(int i = 0;i < gezi.Count;++i)
				{
					s_t_reward t_reward = new s_t_reward();
					int _num = 0;
					s_t_tanbao_event t_tanbao_event = game_data._instance.get_t_tanbao(gezi[i]);
					if(t_tanbao_event == null)
					{
						continue;
					}
					if(t_tanbao_event.type == 1 || t_tanbao_event.type == 2)
					{
						t_reward.type = t_tanbao_event.rtype;
						t_reward.value1 = t_tanbao_event.rvalue1;
						if(t_tanbao_event.type == 2)
						{
							t_reward.value2 = item[i];
						}
						else
						{
							t_reward.value2 = t_tanbao_event.rvalue2;
						}
						t_reward.value3 = 0;
                        sys._instance.m_self.add_reward(t_reward.type, t_reward.value1, t_reward.value2, 0, false, game_data._instance.get_t_language ("tanbao_gui.cs_483_80"));//探宝获得
						_num = has_reward(rewards,t_reward);
						if( _num != -1)
						{
							rewards[_num].value2 += t_tanbao_event.rvalue2;
						}
						else
						{
							rewards.Add(t_reward);
						}
					}
					else if(t_tanbao_event.type == 3)
					{
						//shop_type = shop_type | (1 << t_tanbao_event.shop_type);
                        if (!shop_types.Contains(t_tanbao_event.shop_type))
                        {
                            shop_types.Add(t_tanbao_event.shop_type);
                            shop_types_nums.Add(1);
                        }
                        else
                        {
                            int index = shop_types.IndexOf(t_tanbao_event.shop_type,0);
                            shop_types_nums[index]++;
                        }
						shop_ids.Clear();
						shop_nums.Clear();
					}
					else if(t_tanbao_event.type == 4)
					{
						_num = has_reward(rewards,t_tanbao_event.rewards[m_event[i]]);
						if(_num != -1)
						{
							rewards[_num].value2 += t_tanbao_event.rewards[m_event[i]].value2;
						}
						else
						{
							t_reward.type = t_tanbao_event.rewards[m_event[i]].type;
							t_reward.value1 = t_tanbao_event.rewards[m_event[i]].value1;
							t_reward.value2 = t_tanbao_event.rewards[m_event[i]].value2;
							t_reward.value3 = t_tanbao_event.rewards[m_event[i]].value3;
							rewards.Add(t_reward);
						}
						sys._instance.m_self.add_reward( t_tanbao_event.rewards[m_event[i]].type, t_tanbao_event.rewards[m_event[i]].value1,
                                                        t_tanbao_event.rewards[m_event[i]].value2, t_tanbao_event.rewards[m_event[i]].value3, false, game_data._instance.get_t_language ("tanbao_gui.cs_483_80"));//探宝获得
					}
					else if(t_tanbao_event.type == 5)
					{
						t_reward.type = 1;
						t_reward.value1 = 1;
						t_reward.value2 = gold[i];
						t_reward.value3 = 0;
						_num = has_reward(rewards,t_reward);
						if( _num != -1)
						{
							rewards[_num].value2 += t_reward.value2;
						}
						else
						{
							rewards.Add(t_reward);
						}
                        sys._instance.m_self.add_reward(t_reward.type, t_reward.value1, t_reward.value2, t_reward.value3, false, game_data._instance.get_t_language ("tanbao_gui.cs_483_80"));//探宝获得
					}
					else if(t_tanbao_event.type == 6)
					{
						t_reward.type = 2;
						t_reward.value1 = 1;
						t_reward.value2 = 1;
						t_reward.value3 = 0;
						_num = has_reward(rewards,t_reward);
						if( _num != -1)
						{
							rewards[_num].value2 += t_reward.value2;
						}
						else
						{
							rewards.Add(t_reward);
						}
						touzi_num ++;
					}
				}
				m_use_items_gui.GetComponent<use_items_gui>().init();
				for(int i = 0; i < rewards.Count;++i)
				{
					m_use_items_gui.GetComponent<use_items_gui>().types.Add(rewards[i].type);
					m_use_items_gui.GetComponent<use_items_gui>().values1.Add(rewards[i].value1);
					m_use_items_gui.GetComponent<use_items_gui>().values2.Add(rewards[i].value2);
					m_use_items_gui.GetComponent<use_items_gui>().values3.Add(rewards[i].value3);
				}
				m_use_items_gui.SetActive(true);
			}
			reset();
		}
		if (message.m_type == "guide_end")
		{
			string name = (string)message.m_string[0];
			if(select == 0 || select == 2)
			{
				s_t_tanbao_event t_tanbao_event = game_data._instance.get_t_tanbao(gezi[m_index]);
				if (name.Length >= 6 && name.Substring(0, 6) == "tanbao")
				{
					int index = m_event[m_index];
                    sys._instance.m_self.add_reward(t_tanbao_event.rewards[index].type, t_tanbao_event.rewards[index].value1, t_tanbao_event.rewards[index].value2, t_tanbao_event.rewards[index].value3, false, game_data._instance.get_t_language ("tanbao_gui.cs_483_80"));//探宝获得
					m_use_items_gui.GetComponent<use_items_gui>().init();
					m_use_items_gui.GetComponent<use_items_gui>().types.Add(t_tanbao_event.rewards[index].type);
					m_use_items_gui.GetComponent<use_items_gui>().values1.Add(t_tanbao_event.rewards[index].value1);
					m_use_items_gui.GetComponent<use_items_gui>().values2.Add(t_tanbao_event.rewards[index].value2);
					m_use_items_gui.GetComponent<use_items_gui>().values3.Add(t_tanbao_event.rewards[index].value3);
					m_use_items_gui.SetActive(true);
				}
			}
		}
		if(message.m_type == "bz_yidong")
		{
			if(select == 0 || select == 2)
			{
				m_one.GetComponent<BoxCollider>().enabled = false;
				m_ten.GetComponent<BoxCollider>().enabled = false;
				m_close.GetComponent<BoxCollider>().enabled = false;
				m_luck_shop.GetComponent<BoxCollider>().enabled = false;
				m_luck_touzi.GetComponent<BoxCollider>().enabled = false;
				m_rank_button.GetComponent<BoxCollider>().enabled = false;
				m_reward_button.GetComponent<BoxCollider>().enabled = false;
				m_sm_button.GetComponent<BoxCollider>().enabled = false;
				m_touzi.SetActive(false);
				m_touzis.SetActive(false);
				m_up_down.SetActive(true);
				m_effects [0].SetActive (true);
				m_effects [1].SetActive (false);
				m_up.SetActive(true);
				m_down.SetActive(true);
				InvokeRepeating ("effect",0.0f,0.3f);
				m_yidong_time = 2.0f;
			}
		}
		if(message.m_type == "tanbao_shop")
		{
			if(shop_types.Count == 0)
			{
				return;
			}
			shop ();
			m_tanbao_shop_gui.SetActive(true);
		}
		if(message.m_type == "one_forward")
		{
			m_luck_shop.SetActive(false);
			select = 0;
			protocol.game.cmsg_tanbao_dice _msg = new protocol.game.cmsg_tanbao_dice();
			_msg.dice = 0;
			_msg.type = 0;
			net_http._instance.send_msg<protocol.game.cmsg_tanbao_dice>(opclient_t.CMSG_HUODONG_TANBAO_DICE,_msg);
		}
		if(message.m_type == "ten_forward")
		{
			m_luck_shop.SetActive(false);
			select = 1;
			protocol.game.cmsg_tanbao_dice _msg = new protocol.game.cmsg_tanbao_dice();
			_msg.dice = 0;
			_msg.type = 1;
			net_http._instance.send_msg<protocol.game.cmsg_tanbao_dice>(opclient_t.CMSG_HUODONG_TANBAO_DICE,_msg);
		}
		if(message.m_type == "touzi_forward")
		{
			m_one.GetComponent<BoxCollider>().enabled = false;
			m_ten.GetComponent<BoxCollider>().enabled = false;
			m_close.GetComponent<BoxCollider>().enabled = false;
			m_luck_shop.GetComponent<BoxCollider>().enabled = false;
			m_luck_touzi.GetComponent<BoxCollider>().enabled = false;
			m_rank_button.GetComponent<BoxCollider>().enabled = false;
			m_reward_button.GetComponent<BoxCollider>().enabled = false;
			m_sm_button.GetComponent<BoxCollider>().enabled = false;
			m_luck_shop.SetActive(false);
			m_luck_touzi_gui.transform.Find("frame_big").GetComponent<frame>().hide();


            if ((int)message.m_ints[0] == 2)
            {
                select = 2;
                protocol.game.cmsg_tanbao_dice _msg = new protocol.game.cmsg_tanbao_dice();
                _msg.dice = select_num;
                _msg.type = 2;
                net_http._instance.send_msg<protocol.game.cmsg_tanbao_dice>(opclient_t.CMSG_HUODONG_TANBAO_DICE, _msg);

            }
            else
            {
                select = 3;
                protocol.game.cmsg_tanbao_dice _msg = new protocol.game.cmsg_tanbao_dice();
                _msg.dice = select_num;
                _msg.type = 3;
                net_http._instance.send_msg<protocol.game.cmsg_tanbao_dice>(opclient_t.CMSG_HUODONG_TANBAO_DICE, _msg);
            }


			
		}
		if(message.m_type == "tanbao_shop_ex")
		{
			t_tanbao_shop = game_data._instance.get_t_tanbao_shop((int)message.m_ints[0]);
			int _shop_num = shop_num(t_tanbao_shop);
			s_message _message = new s_message();
			_message.m_type = "buy_num_gui";
			_message.m_ints.Add((int)message.m_ints[0]);
			_message.m_ints.Add(15);
			_message.m_ints.Add(_shop_num);
			cmessage_center._instance.add_message(_message);
		}
		if(message.m_type == "tanbao_shop_buy")
		{
			m_input_num = (int)message.m_ints[0];
			if (sys._instance.m_self.get_att(e_player_attr.player_jewel) >= t_tanbao_shop.price * m_input_num)
			{
				protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
				_net_msg.item_id = t_tanbao_shop.id;
				_net_msg.num = m_input_num;
				net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_HUODONG_TANBAO_SHOP, _net_msg);
			}
			else
			{
				s_message _mes = new s_message();
				_mes.m_type = "show_recharge";
				cmessage_center._instance.add_message(_mes);
			}
		}
		if(message.m_type == "show_tanbao_task_effect")
		{
			if(tanbao_reward_gui.effect())
			{
				m_task_point.SetActive(true);
			}
			else
			{
				m_task_point.SetActive(false);
			}
		}
		if (message.m_type == "daily_refresh")
		{
			tou_num = 5;
		}
	}

	int has_reward(List<s_t_reward> rewards, s_t_reward t_reward)
	{
		for(int i = 0; i < rewards.Count;++i)
		{
			if(rewards[i].type == t_reward.type && rewards[i].value1 == t_reward.value1)
			{
				return i;
			}
		}
		return -1;
	}

	void shop()
	{
		if(m_scro.transform.GetComponent<SpringPanel>() != null)
		{
			m_scro.transform.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
		m_scro.transform.localPosition = new Vector3 (0, 0, 0);
		List<s_t_tanbao_shop> tanbao_shops = new List<s_t_tanbao_shop>();
		foreach(int id in game_data._instance.m_dbc_tanbao_shop.m_index.Keys)
		{
			s_t_tanbao_shop t_tanbao_shop = game_data._instance.get_t_tanbao_shop(id);
			int index = shop_types.IndexOf(t_tanbao_shop.shop_type,0);
            if (index != -1)
            {
                t_tanbao_shop.buy_num = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(10, id)) * shop_types_nums[index];
                tanbao_shops.Add(t_tanbao_shop);
 
            }
           
		}
		sys._instance.remove_child(m_scro);
		int _id = 0;
		tanbao_shops.Sort (comp);
		for(int i = 0;i < tanbao_shops.Count; i++)
		{
			int row = i / 2;
			int lie =  i % 2;
			GameObject temp = game_data._instance.ins_object_res ("ui/temaihui_card");
			temp.transform.parent = m_scro.transform;
			temp.transform.localPosition = new Vector3 (401 * lie - 164, -138 * row + 106,0);
			temp.transform.localScale = new Vector3(1,1,1);
			temp.GetComponent<temaihui_card>().m_item_shop_gui = this.gameObject;
            temp.GetComponent<temaihui_card>().m_t_tanbao_shop = tanbao_shops[i];
            temp.GetComponent<temaihui_card>().m_shop_id = tanbao_shops[i].id;
			temp.GetComponent<temaihui_card>().m_shell = shop_num(tanbao_shops[i]);
			temp.GetComponent<temaihui_card>().type = 15;
			temp.GetComponent<temaihui_card>().updata_ui();
			sys._instance.add_pos_anim(temp,0.3f, new Vector3(0, 60, 0), _id * 0.05f);
			sys._instance.add_alpha_anim(temp,0.3f, 0, 1.0f, _id * 0.05f);
			_id++;
		}
	}

	int comp(s_t_tanbao_shop t_tanbao_shop1, s_t_tanbao_shop t_tanbao_shop2)
	{
		int num1 = shop_num (t_tanbao_shop1);
		int num2 = shop_num (t_tanbao_shop2);
		if(num1 != 0 && num2 == 0)
		{
			return -1;
		}
		else if(num1 == 0 && num2 != 0)
		{
			return 1;
		}
		else if(t_tanbao_shop1.shop_type < t_tanbao_shop2.shop_type)
		{
			return -1;
		}
		else if(t_tanbao_shop1.shop_type > t_tanbao_shop2.shop_type)
		{
			return 1;
		}
		return t_tanbao_shop1.id - t_tanbao_shop2.id;
	}

	int shop_num(s_t_tanbao_shop t_tanbao_shop)
	{
		int m_shell = 0;
		bool flag = false;
		for(int i = 0; i < shop_ids.Count;++i)
		{
			if(t_tanbao_shop.id == shop_ids[i])
			{
				flag = true;
				m_shell = t_tanbao_shop.buy_num - shop_nums[i];
				break;
			}
		}
		if(!flag)
		{
			m_shell = t_tanbao_shop.buy_num;
		}
		return m_shell;

	}

	void IMessage.net_message (s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_HUODONG_TANBAO_DICE)
		{
			protocol.game.smsg_tanbao_dice _msg = net_http._instance.parse_packet<protocol.game.smsg_tanbao_dice>(message.m_byte);
			//shop_type = 0;
			num = _msg.dianshu;
			gezi = _msg.gezi;
			m_event = _msg.events;
			gold = _msg.gold;
			item = _msg.item;
			yidong = _msg.yidong;
			if(select == 1 || select == 3)
			{
				for(int i = 0; i < _msg.dianshu.Count;i++)
				{
					if(_msg.yidong[i] == 1)
					{
						continue;
					}
					score += _msg.dianshu[i];
				}
			}
			else if(select == 2 || select == 0)
			{
				score += _msg.dianshu[0];
			}
            
			qidian_num += _msg.pass_num;
			final_site = _msg.final_gezi;
			if(select == 0)
			{
				if(tou_num > 0)
				{
					tou_num -= 1;
				}
				else
				{
					buy_num += 1;
				}
				if(buy_num > 0)
				{
					s_t_price t_price = game_data._instance.get_t_price(buy_num);
					sys._instance.m_self.sub_att(e_player_attr.player_jewel,t_price.tanbao_price,game_data._instance.get_t_language ("tanbao_gui.cs_934_82"));//探宝消耗
				}
				m_index = 0;
			}
			else if(select == 1)//1:走十次
			{
				int jewel = 0;
				for(int i = 1; i <= 10-tou_num;++i)
				{
					s_t_price _price = game_data._instance.get_t_price(i + buy_num);
					jewel += _price.tanbao_price;
				}
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, jewel, game_data._instance.get_t_language ("tanbao_gui.cs_934_82"));//探宝消耗
				buy_num += (10 - tou_num);
				tou_num = 0;
			}
			else if (select == 2)
			{
				touzi_num -= 1;
				m_index = 0;
				s_message mes = new s_message();
				mes.m_type = "bz_move";
				mes.m_ints.Add(gezi[m_index]);
				cmessage_center._instance.add_message(mes);
				return;
			}
            else if (select == 3)
            {
                if (touzi_num >= 10)
                {
                    touzi_num -= 10;
                    walknum = 10;
                }
                else
                {
                    walknum = touzi_num;
                    touzi_num = 0;
                }
                s_message mes = new s_message();
                mes.m_type = "bz_move";
                mes.m_ints.Add(final_site);
                cmessage_center._instance.add_message(mes);
                return;
            }
           
			set_num();
		}
		if(message.m_opcode == opclient_t.CMSG_HUODONG_TANBAO_SHOP)
		{
			protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy>(message.m_byte);
			for (int i = 0; i < _msg.equips.Count; i++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i], true);
			}
			for (int i = 0; i < _msg.treasures.Count; i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i], true);
			}
			sys._instance.m_self.add_reward(t_tanbao_shop.rtype, t_tanbao_shop.rvalue1, t_tanbao_shop.rvalue2 * m_input_num, t_tanbao_shop.rvalue3,game_data._instance.get_t_language ("tanbao_gui.cs_973_138"));//探宝商店获得
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, t_tanbao_shop.price * m_input_num, game_data._instance.get_t_language ("tanbao_gui.cs_974_104"));//探宝商店消耗
			bool flag = false;
			for(int i = 0; i < shop_ids.Count;++i)
			{
				if(shop_ids[i] == t_tanbao_shop.id)
				{
					flag = true;
					shop_nums[i] += m_input_num;
					break;
				}
			}
			if(!flag)
			{
				shop_ids.Add(t_tanbao_shop.id);
				shop_nums.Add(m_input_num);
			}
			score += (t_tanbao_shop.score*m_input_num);
			shop();
			reset();
			root_gui._instance.m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	public void set_num()
	{
		m_up_down.SetActive (false);
		if(select == 0 || select == 2)
		{
			m_touzi.SetActive(true);
			m_touzi.GetComponent<bz_sezi>().m_ds = num[m_index];
			m_touzi.GetComponent<bz_sezi>().m_time = 1.0f;
		}
		else if(select == 1)
		{
			m_touzis.SetActive(true);
			int m_id = 0 ;
			for(int i = 0; i < 10;i++)
			{
				m_shuzi[i].GetComponent<bz_sezi>().m_ds = num[m_id];
				m_shuzi[i].GetComponent<bz_sezi>().m_time = 1.0f;
				m_id ++;
				if(yidong[i] == 1)
				{
					m_id += 1;
				}
			}
		}
        else if (select == 3)
        {
            m_touzis.SetActive(true);
            int m_id = 0;
            for (int i = 0; i < tou_num; i++)
            {
                m_shuzi[i].GetComponent<bz_sezi>().m_ds = num[m_id];
                m_shuzi[i].GetComponent<bz_sezi>().m_time = 1.0f;
                m_id++;
                if (yidong[i] == 1)
                {
                    m_id += 1;
                }
            }

        }
		m_time = 2.0f;
		m_one.GetComponent<BoxCollider>().enabled = false;
		m_ten.GetComponent<BoxCollider>().enabled = false;
		m_close.GetComponent<BoxCollider>().enabled = false;
		m_luck_shop.GetComponent<BoxCollider>().enabled = false;
		m_luck_touzi.GetComponent<BoxCollider>().enabled = false;
		m_rank_button.GetComponent<BoxCollider>().enabled = false;
		m_reward_button.GetComponent<BoxCollider>().enabled = false;
		m_sm_button.GetComponent<BoxCollider>().enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (m_time > 0)
		{
			m_time -= Time.deltaTime;
			if (m_time <= 0)
			{
				s_message mes = new s_message();
				mes.m_type = "bz_move";
				if(select == 0 || select == 2)
				{
					mes.m_ints.Add(gezi[m_index]);
				}
				else
				{
					mes.m_ints.Add(final_site);
				}
				cmessage_center._instance.add_message(mes);
			}
		}
		if(m_yidong_time > 0)
		{
			m_yidong_time -= Time.deltaTime;
			if (m_yidong_time <= 0)
			{
				if(select == 0 || select == 2)
				{
					m_index = 1;
					int m_num = num[m_index];
					if(m_num < 0)
					{
						m_up.SetActive(false);
						m_effects[1].SetActive(false);
					}
					else
					{
						m_effects[0].SetActive(false);
						m_down.SetActive(false);
					}
					s_message mes = new s_message();
					mes.m_type = "tanbao_toward";
					mes.m_ints.Add(m_num);
					cmessage_center._instance.add_message(mes);
				}
				CancelInvoke ("effect");
				Invoke("set_num",1.5f);
			}
		}
	}
}
