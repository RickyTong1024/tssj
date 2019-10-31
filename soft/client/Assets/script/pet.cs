
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pet {
	public s_t_pet m_t_pet;
	public player m_player;
	private dhc.pet_t m_pet;
	public static int m_attr_num = 50;

	public double get_attr(int type)
	{
		List<double> attrs = new List<double>();
		for (int i = 0; i < m_attr_num; ++i)
		{
			attrs.Add(0);
		}
		float add_per = 0.0f ;
		for(int i = 0; i <= m_pet.jinjie;++i)
		{
			s_t_pet_jinjie t_pet_jingjie = game_data._instance.get_t_pet_jinjie (i);
			if(t_pet_jingjie != null && t_pet_jingjie.qsx_add > 0)
			{
				add_per += (float)t_pet_jingjie.qsx_add/100;
			}
			if(t_pet_jingjie != null && t_pet_jingjie.esx_add > 0)
			{
				attrs[m_t_pet.jinjie_add_sx[t_pet_jingjie.esx_add-1].attr] += m_t_pet.jinjie_add_sx[t_pet_jingjie.esx_add-1].value;
			}
		}
		add_per += 1.0f;
		for(int i = 0; i < m_pet.jinjie;++i)
		{
			s_t_pet_jinjie t_jingjie = game_data._instance.get_t_pet_jinjie(i);
			if(t_jingjie != null)
			{
				for(int j = 0; j < t_jingjie.cls.Count;++j)
				{
					s_t_pet_jinjieitem t_jinjieitem = game_data._instance.get_t_pet_jinjieitem(t_jingjie.cls[j]);
					if(t_jinjieitem != null)
					{
						for(int k= 0; k < t_jinjieitem.attrs.Count;++k)
						{
							attrs[t_jinjieitem.attrs[k].attr] += t_jinjieitem.attrs[k].value;
						}
					}
				}
			}
		}
		for(int i = 0; i < m_pet.jinjie_slot.Count;++i)
		{
			int id = m_pet.jinjie_slot[i];
			s_t_pet_jinjieitem t_pet_jingjieitem = game_data._instance.get_t_pet_jinjieitem(id);
			if(t_pet_jingjieitem != null)
			{
				for(int j= 0; j < t_pet_jingjieitem.attrs.Count;++j)
				{
					attrs[t_pet_jingjieitem.attrs[j].attr] += t_pet_jingjieitem.attrs[j].value;
				}
			}
		}
		for (int i = 0; i < 4; ++i)
		{

			double cz = (m_t_pet.cs[i]+(m_t_pet.cscz[i]+m_t_pet.shengxing_cz[i]*get_star())*get_level() + attrs[i+1] + m_t_pet.jinjie_sxcz [i]*m_pet.jinjie + m_t_pet.shengxing_sxcz[i]*get_star()) *add_per;
			attrs [i+1] = cz;
		}
		attrs[3] += attrs[29];
		attrs[4] += attrs[29];
		return attrs [type];
	}

	public double get_guard_attr(int type)
	{
		return (get_attr (type)*m_t_pet.sx_add);
	}

	public double get_pet_shengxing_attr(int type,int star)
	{
		return (get_attr (type) + m_t_pet.shengxing_sxcz[type-1]*star);
	}

	public double get_pet_shengxing_cz_attr(int type,int star)
	{
		return (m_t_pet.cscz[type-1] + m_t_pet.shengxing_cz[type-1]* star);
	}

	public double get_pet_jingjie_attr(int type,int jingjie)
	{
		List<double> attrs = new List<double>();
		for (int i = 0; i < m_attr_num; ++i)
		{
			attrs.Add(0);
		}
		float add_per = 0.0f ;
		for(int i = 0; i <= jingjie;++i)
		{
			s_t_pet_jinjie t_pet_jingjie = game_data._instance.get_t_pet_jinjie (i);
			if(t_pet_jingjie != null && t_pet_jingjie.qsx_add > 0)
			{
				add_per += (float)t_pet_jingjie.qsx_add/100;
			}
			if(t_pet_jingjie != null && t_pet_jingjie.esx_add > 0)
			{
				attrs[m_t_pet.jinjie_add_sx[t_pet_jingjie.esx_add-1].attr] += m_t_pet.jinjie_add_sx[t_pet_jingjie.esx_add-1].value;
			}
		}
		add_per += 1.0f;
		for(int i = 0; i < jingjie;++i)
		{
			s_t_pet_jinjie t_jingjie = game_data._instance.get_t_pet_jinjie(i);
			if(t_jingjie != null)
			{
				for(int j = 0; j < t_jingjie.cls.Count;++j)
				{
					s_t_pet_jinjieitem t_jinjieitem = game_data._instance.get_t_pet_jinjieitem(t_jingjie.cls[j]);
					if(t_jinjieitem != null)
					{
						for(int k= 0; k < t_jinjieitem.attrs.Count;++k)
						{
							attrs[t_jinjieitem.attrs[k].attr] += t_jinjieitem.attrs[k].value;
						}
					}
				}
			}
		}
		if(jingjie <= m_pet.jinjie)
		{
			for(int i = 0; i < m_pet.jinjie_slot.Count;++i)
			{
				int id = m_pet.jinjie_slot[i];
				if(id <= 0)
				{
					continue;
				}
				s_t_pet_jinjieitem t_pet_jingjieitem = game_data._instance.get_t_pet_jinjieitem(id);
				if(t_pet_jingjieitem != null)
				{
					for(int j= 0; j < t_pet_jingjieitem.attrs.Count;++j)
					{
						attrs[t_pet_jingjieitem.attrs[j].attr] += t_pet_jingjieitem.attrs[j].value;
					}
				}
			}
		}
		for (int i = 0; i < 4; ++i)
		{
			
			double cz = (m_t_pet.cs[i]+(m_t_pet.cscz[i]+m_t_pet.shengxing_cz[i]*get_star())*get_level() + attrs[i+1] + m_t_pet.jinjie_sxcz [i]*jingjie + m_t_pet.shengxing_cz[i]) *add_per;
			attrs [i+1] = cz;
		}
		attrs[3] += attrs[29];
		attrs[4] += attrs[29];
		return attrs [type];
	}

	public double get_fight()
	{
		double force = 0;
		List<double> attrs = new List<double>();
		for (int i = 0; i < m_attr_num; ++i)
		{
			attrs.Add(0);
		}
		for(int i = 1; i < 5;++i)
		{
			attrs[i] = get_attr(i);
		}
		force += attrs [1] * 0.25 + attrs [2] * 2 + attrs [3] * 5 + attrs [4] * 5;
		return force;
	}

	public double pet_weiyang_attr(int type,int level)
	{
		List<double> attrs = new List<double>();
		for (int i = 0; i < m_attr_num; ++i)
		{
			attrs.Add(0);
		}
		for (int i = 0; i < 4; ++i)
		{
			double cz = m_t_pet.cs[i]+(m_t_pet.cscz[i]+m_t_pet.shengxing_cz[i]*get_star())*level;
			attrs[i+1] += cz;
		}
		return attrs [type];
	}

	public int get_pet_exp(s_t_exp t_exp)
	{
		int color = get_color();
		if(color == 2)
		{
			return t_exp.pet_exp;
		}
		if(color == 3)
		{
			return t_exp.pet_zi_exp;
		}
		if(color == 4)
		{
			return t_exp.pet_jin_exp;
		}
		if(color == 5)
		{
			return t_exp.pet_hong_exp;
		}
		return 999999;
	}

	//星级
	public int get_star()
	{
		return m_pet.star;
	}

	public int get_exp()
	{
		return m_pet.exp;
	}
	public dhc.pet_t get_pet()
	{
		return m_pet;
	}

	public int get_jlevel()
	{
		return m_pet.jinjie;
	}

	public int get_level()
	{
		return m_pet.level;
	}

	public ulong get_guid()
	{
		if(m_pet != null)
		{
			return m_pet.guid;
		}
		
		return 0;
	}

	public int get_template_id()
	{
		return m_t_pet.id;
	}

	public string get_color_name()
	{
		return get_color_name(m_t_pet.name,m_t_pet.color);
	}

	public static string get_color_name(string name,int color)
	{
		string _name = game_data._instance.get_name_color(color) + name + "[-]";		
		return _name;
	}

	public static int guard_com(pet m_pet1,pet m_pet2)
	{
		double force1 = m_pet1.get_fight ();
		double force2 = m_pet2.get_fight ();
		if(force1 > force2)
		{
			return -1;
		}
		else if(force1 < force2)
		{
			return 1;
		}
		else if(m_pet1.m_t_pet.color > m_pet2.m_t_pet.color)
		{
			return -1;
		}
		else if(m_pet1.m_t_pet.color < m_pet2.m_t_pet.color)
		{
			return 1;
		}
		return  m_pet2.m_t_pet.id - m_pet1.m_t_pet.id;
	}
	
	public void set_pet(dhc.pet_t pet)
	{
		m_pet = pet;
		
		set_class (pet.template_id);
	}

	void set_class(int id)
	{
		m_t_pet = game_data._instance.get_t_pet (id);
		
		if(m_t_pet == null)
		{
			Debug.Log("null class" + id);
			return;
		}
	}

	public static pet get_new_pet(int id)
	{
		pet _pet = new pet ();
		dhc.pet_t _t_pet = new dhc.pet_t();
		
		_t_pet.guid = 0;
		_t_pet.level = 1;
		_t_pet.player_guid = 0;
		_t_pet.template_id = id;
		
		_t_pet.star = 0;
		_t_pet.jinjie = 0;
		for(int i = 0; i < 4;++i)
		{
			_t_pet.jinjie_slot.Add(0);
		}

		
		_pet.set_pet (_t_pet);
		
		return _pet;
	}

	public int get_fragment_id()
	{
		return get_fragment_id (m_t_pet.id);
	}
	
	public static int get_fragment_id(int id)
	{
		for(int i = 0;i < game_data._instance.m_dbc_item.get_y();i ++)
		{
			int _type = int.Parse(game_data._instance.m_dbc_item.get(4,i));
			
			if(_type == 10001)
			{
				int _id = int.Parse(game_data._instance.m_dbc_item.get(9,i));
				
				if(_id == id)
				{
					return int.Parse(game_data._instance.m_dbc_item.get(0,i));
				}
			}
		}
		
		return 0;
	}

	public int get_color()
	{
		return m_t_pet.color;
	}

	public List<s_t_reward> get_jiegu_reward()
	{
		List<s_t_reward> rewards = get_chongsheng_reward();
		return rewards;
	}

	public List<s_t_reward> get_chongsheng_reward()
	{
		List<s_t_reward> rewards = new List<s_t_reward>();
		if (get_chong_sheng_gold() > 0)
		{
			s_t_reward _reward1 = new s_t_reward();
			_reward1.type = 1;
			_reward1.value1 = 1;
			_reward1.value2 = get_chong_sheng_gold();
			_reward1.value3 = 0;
			rewards.Add(_reward1);
		}

		List<s_t_reward> rewards3 = get_chong_sheng_item();
		for (int i = 0; i < rewards3.Count; i++)
		{
			rewards.Add(rewards3[i]);
		}
		return rewards;
	}

	public List<s_t_reward> get_fenjie_reward()
	{
		List<s_t_reward> rewards = new List<s_t_reward>();
		if (get_chong_sheng_gold() > 0)
		{
			s_t_reward _reward1 = new s_t_reward();
			_reward1.type = 1;
			_reward1.value1 = 1;
			_reward1.value2 = get_chong_sheng_gold();
			_reward1.value3 = 0;
			rewards.Add(_reward1);
		}
		s_t_item _item = game_data._instance.get_item(get_fragment_id());
		List<s_t_reward> rewards3 = get_fenjie_item();
		bool flag = false;
		for (int i = 0; i < rewards3.Count; i++)
		{
			if(rewards3[i].type == 1 && rewards3[i].value1 == 27)
			{
				flag = true;
				rewards3[i].value2 += _item.def_2 * m_t_pet.soul_beast/2;
			}
			rewards.Add(rewards3[i]);
		}
		if(!flag)
		{
			s_t_reward _reward = new s_t_reward();
			_reward.type = 1;
			_reward.value1 = 27;
			_reward.value2 = _item.def_2 * m_t_pet.soul_beast/2;
			_reward.value3 = 0;
			rewards.Add(_reward);
		}
		return rewards;
	}

	int get_chong_sheng_gold()
	{
		int gold = 0;
		for (int i = 1; i < this.get_level(); i++)
		{
			s_t_exp _exp = game_data._instance.get_t_exp(i + 1);
			if (_exp != null)
			{
				gold += get_pet_exp(_exp);
			}
		}
		gold += get_exp();
		if(this.get_jlevel() > 0)
		{
			for (int i = 0; i < this.get_jlevel(); i++)
			{
				s_t_pet_jinjie t_jinjie = game_data._instance.get_t_pet_jinjie(i);
				if (t_jinjie != null)
				{
					gold += t_jinjie.gold;
				}
			}
		}
		for (int i = 0; i < this.get_star(); i++)
		{
			s_t_pet_shengxing t_shengxing = game_data._instance.get_t_pet_shengxing(i+1);
			if (t_shengxing != null)
			{
				gold += t_shengxing.gold[get_color() -2];
				
			}
		}
		return gold;
	}

	List<s_t_reward> get_chong_sheng_item()
	{
		List<s_t_reward> rewards = new List<s_t_reward>();

		s_t_reward reward = new s_t_reward();
		reward.type = 2;
		reward.value1 = get_fragment_id();
		reward.value3 = 0;
		for (int i = 1; i <= this.get_star(); i++)
		{
			s_t_pet_shengxing t_shengxing = game_data._instance.get_t_pet_shengxing(i);
			reward.value2 += t_shengxing.sp[get_color()-2];
		}
		s_t_item _item = game_data._instance.get_item (get_fragment_id ());
		reward.value2 += _item.def_2;
		rewards.Add(reward);

		s_t_reward reward_satone = new s_t_reward();
		reward_satone.type = 2;
		reward_satone.value1 = 110010001;
		reward_satone.value2 = 0;
		reward_satone.value3 = 0;
		if (this.get_star() != 0)
		{
			for (int i = 1; i <= this.get_star(); i++)
			{
				s_t_pet_shengxing t_shengxing = game_data._instance.get_t_pet_shengxing(i);
				reward_satone.value2 += t_shengxing.stone[get_color()-2];
			}
		}
		if( reward_satone.value2 > 0 )
		{
			rewards.Add(reward_satone);
		}
		for (int i = 1; i <= this.get_jlevel(); i++)
		{
			s_t_pet_jinjie t_jinjie = game_data._instance.get_t_pet_jinjie(i-1);
			for(int j = 0; j < t_jinjie.cls.Count;++j)
			{
				s_t_reward _reward = new s_t_reward();
				_reward.type = 2;
				_reward.value1 = t_jinjie.cls[j];
				_reward.value2 = 1;
				_reward.value3 = 0;
				bool flag = false;
				for(int k = 0; k < rewards.Count;++k)
				{
					if(rewards[k].type == _reward.type && rewards[k].value1 == _reward.value1)
					{
						flag = true;
						rewards[k].value2 += 1;
						break;
					}
				}
				if(!flag)
				{
					rewards.Add(_reward);
				}
			}
		}
		for(int i = 0; i < m_pet.jinjie_slot.Count;++i)
		{
			int id = m_pet.jinjie_slot[i];
			if(id <= 0)
			{
				continue;
			}
			s_t_reward _reward = new s_t_reward();
			_reward.type = 2;
			_reward.value1 = m_pet.jinjie_slot[i];
			_reward.value2 = 1;
			_reward.value3 = 0;
			bool flag = false;
			for(int k = 0; k < rewards.Count;++k)
			{
				if(rewards[k].type == _reward.type && rewards[k].value1 == _reward.value1)
				{
					flag = true;
					rewards[k].value2 += 1;
					break;
				}
			}
			if(!flag)
			{
				rewards.Add(_reward);
			}
		}
		int exp = 0;
		for (int i = 1; i < this.get_level(); i++)
		{
			s_t_exp _exp = game_data._instance.get_t_exp(i + 1);
			if (_exp != null)
			{
				exp += get_pet_exp(_exp);
			}
		}
		exp += get_exp();
		s_t_item t_dj_item = game_data._instance.get_item (110020003);
		s_t_item t_gj_item = game_data._instance.get_item (110020002);
		s_t_item t_pt_item = game_data._instance.get_item (110020001);
		int num = exp;
		int dj_num = num / t_dj_item.def_1;
		num = exp -  dj_num*t_dj_item.def_1;
		int gj_num = num / t_gj_item.def_1;
		num = exp - dj_num*t_dj_item.def_1- gj_num*t_gj_item.def_1;
		int pt_num = num / t_pt_item.def_1;
		if(dj_num > 0)
		{
			s_t_reward siliao = new s_t_reward();
			siliao.type = 2;
			siliao.value1 = 110020003;
			siliao.value2 = dj_num;
			siliao.value3 = 0;
			rewards.Add(siliao);
		}
		if(gj_num > 0)
		{
			s_t_reward siliao = new s_t_reward();
			siliao.type = 2;
			siliao.value1 = 110020002;
			siliao.value2 = gj_num;
			siliao.value3 = 0;
			rewards.Add(siliao);
		}
		if(pt_num > 0)
		{
			s_t_reward siliao = new s_t_reward();
			siliao.type = 2;
			siliao.value1 = 110020001;
			siliao.value2 = pt_num;
			siliao.value3 = 0;
			rewards.Add(siliao);
		}
		return rewards;
	}

	List<s_t_reward> get_fenjie_item()
	{
		List<s_t_reward> rewards = new List<s_t_reward>();
		
		if (this.get_star() != 0)
		{
			s_t_reward reward = new s_t_reward();
			reward.type = 1;
			reward.value1 = 27;
			reward.value3 = 0;
			s_t_item _item = game_data._instance.get_item(get_fragment_id());
			for (int i = 1; i <= this.get_star(); i++)
			{
				s_t_pet_shengxing t_shengxing = game_data._instance.get_t_pet_shengxing(i);
				reward.value2 += t_shengxing.sp[get_color()-2]*m_t_pet.soul_beast/2;
			}
			rewards.Add(reward);
		}
		s_t_reward reward_satone = new s_t_reward();
		reward_satone.type = 2;
		reward_satone.value1 = 110010001;
		reward_satone.value2 = 0;
		reward_satone.value3 = 0;
		if (this.get_star() != 0)
		{
			for (int i = 1; i <= this.get_star(); i++)
			{
				s_t_pet_shengxing t_shengxing = game_data._instance.get_t_pet_shengxing(i);
				reward_satone.value2 += t_shengxing.stone[get_color()-2];
			}
		}
		if( reward_satone.value2 > 0 )
		{
			rewards.Add(reward_satone);
		}
		for (int i = 1; i <= this.get_jlevel(); i++)
		{
			s_t_pet_jinjie t_jinjie = game_data._instance.get_t_pet_jinjie(i-1);
			for(int j = 0; j < t_jinjie.cls.Count;++j)
			{
				s_t_reward _reward = new s_t_reward();
				_reward.type = 2;
				_reward.value1 = t_jinjie.cls[j];
				_reward.value2 = 1;
				_reward.value3 = 0;
				bool flag = false;
				for(int k = 0; k < rewards.Count;++k)
				{
					if(rewards[k].type == _reward.type && rewards[k].value1 == _reward.value1)
					{
						flag = true;
						rewards[k].value2 += 1;
						break;
					}
				}
				if(!flag)
				{
					rewards.Add(_reward);
				}
			}
		}
		for(int i = 0; i < m_pet.jinjie_slot.Count;++i)
		{
			int id = m_pet.jinjie_slot[i];
			if(id <= 0)
			{
				continue;
			}
			s_t_reward _reward = new s_t_reward();
			_reward.type = 2;
			_reward.value1 = m_pet.jinjie_slot[i];
			_reward.value2 = 1;
			_reward.value3 = 0;
			bool flag = false;
			for(int k = 0; k < rewards.Count;++k)
			{
				if(rewards[k].type == _reward.type && rewards[k].value1 == _reward.value1)
				{
					flag = true;
					rewards[k].value2 += 1;
					break;
				}
			}
			if(!flag)
			{
				rewards.Add(_reward);
			}
		}
		int exp = 0;
		for (int i = 1; i < this.get_level(); i++)
		{
			s_t_exp _exp = game_data._instance.get_t_exp(i + 1);
			if (_exp != null)
			{
				exp += get_pet_exp(_exp);
			}
		}
		exp += get_exp();
		if(exp > 0)
		{
			s_t_item t_dj_item = game_data._instance.get_item (110020003);
			s_t_item t_gj_item = game_data._instance.get_item (110020002);
			s_t_item t_pt_item = game_data._instance.get_item (110020001);
			int num = exp;
			int dj_num = num / t_dj_item.def_1;
			num = exp  - dj_num*t_dj_item.def_1;
			int gj_num = num / t_gj_item.def_1;
			num = exp  - dj_num*t_dj_item.def_1- gj_num*t_gj_item.def_1;
			int pt_num = num / t_pt_item.def_1;
			if(dj_num > 0)
			{
				s_t_reward siliao = new s_t_reward();
				siliao.type = 2;
				siliao.value1 = 110020003;
				siliao.value2 = dj_num;
				siliao.value3 = 0;
				rewards.Add(siliao);
			}
			if(gj_num > 0)
			{
				s_t_reward siliao = new s_t_reward();
				siliao.type = 2;
				siliao.value1 = 110020002;
				siliao.value2 = gj_num;
				siliao.value3 = 0;
				rewards.Add(siliao);
			}
			if(pt_num > 0)
			{
				s_t_reward siliao = new s_t_reward();
				siliao.type = 2;
				siliao.value1 = 110020001;
				siliao.value2 = pt_num;
				siliao.value3 = 0;
				rewards.Add(siliao);
			}
		}
		return rewards;
	}
}
