
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip {

	public static string get_equip_name(dhc.equip_t equip)
	{
		return get_equip_name (equip.template_id, equip.enhance);
	}

	public static string get_equip_value(dhc.equip_t equip)
	{
		return get_equip_value (equip.template_id, equip.enhance, equip.star);
	}

	public static string get_equip_random_max(int id, s_t_equip t_equip, dhc.equip_t equip)
	{
		string s1 = "";
		int num  = 0;
		s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color, equip.star);

		for ( int j = 0 ; j < t_equip.eeattr.Count; ++j)
		{
			if(id == t_equip.eeattr[j].attr)
			{
				if(t_equip_sx != null)
				{
					num = (int)(t_equip.eeattr[j].value2 + t_equip.eeattr[j].value2 * t_equip_sx.enhance_rate * equip.enhance);
				}
				else
				{
					num = (int)(t_equip.eeattr[j].value2);
				}
				s1 = "(" + string.Format(game_data._instance.get_t_language ("equip.cs_35_29")  , num.ToString())  + ")";//{0}最大
				break;
			}
		}

		return s1;
	}
	public static int get_equip_random_max1(int id, s_t_equip t_equip, dhc.equip_t equip, int enchance)
	{
		int num  = 0;
		s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color, equip.star);
		
		for ( int j = 0 ; j < t_equip.eeattr.Count; ++j)
		{
			if(id == t_equip.eeattr[j].attr)
			{
				if(t_equip_sx != null)
				{
					num = (int)(t_equip.eeattr[j].value2 + t_equip.eeattr[j].value2 * t_equip_sx.enhance_rate * enchance);
				}
				else
				{
					num = (int)(t_equip.eeattr[j].value2);
				}

				break;
			}
		}
		
		return num;
	}
	public static int get_equip_random_levelmax(int id, s_t_equip t_equip, dhc.equip_t equip)
	{

		int num  = 0;
		s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color, equip.star);
		
		for ( int j = 0 ; j < t_equip.eeattr.Count; ++j)
		{
			if(id == t_equip.eeattr[j].attr)
			{
				if(t_equip_sx != null)
				{
					num = (int)(t_equip.eeattr[j].value2 + t_equip.eeattr[j].value2 * t_equip_sx.enhance_rate * equip.enhance);
				}
				else
				{
					num = (int)(t_equip.eeattr[j].value2);
				}
				break;
			}
		}
		
		return num;
	}

	public static string get_equip_random_value2(int value1, int id, s_t_equip t_equip,dhc.equip_t equip)
	{
		string attr = "";
		string s = "";
		string s1 = "";
		int num  = 0;
		int value = value1;
		s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color, equip.star);

		for ( int j = 0 ; j < t_equip.eeattr.Count; ++j)
		{
			if(id == t_equip.eeattr[j].attr)
			{
				if(t_equip_sx != null)
				{
					num = (int)(t_equip.eeattr[j].value2 + t_equip.eeattr[j].value2 * t_equip_sx.enhance_rate * equip.enhance);
				}
				else
				{
					num = (int)(t_equip.eeattr[j].value2);
				}
				break;
			}
		}

		if(value <= num * 0.4)
		{
			s = "[18f312]";
		}
		else if(value <= num * 0.7)
		{
			s = "[28f4fe]";
		}
		else if(value <= num * 0.9)
		{
			s = "[c44bf9]";
		}
		else
		{
			s = "[ffff00]";
		}
		attr += s + game_data._instance.get_value_string(id, value1) + "[-]" + "[ffff00]"+ s1;

		return attr;
	}
	public static string get_equip_random_color(int value1, int id, s_t_equip t_equip,dhc.equip_t equip)
	{
		string s = "";
		int num  = 0;
		int value = value1;
		s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color, equip.star);
		
		for ( int j = 0 ; j < t_equip.eeattr.Count; ++j)
		{
			if(id == t_equip.eeattr[j].attr)
			{
				if(t_equip_sx != null)
				{
					num = (int)(t_equip.eeattr[j].value2 + t_equip.eeattr[j].value2 * t_equip_sx.enhance_rate * equip.enhance);
				}
				else
				{
					num = (int)(t_equip.eeattr[j].value2);
				}
				break;
			}
		}
		
		if(value <= num * 0.4)
		{
			s = "[18f312]";
		}
		else if(value <= num * 0.7)
		{
			s = "[28f4fe]";
		}
		else if(value <= num * 0.9)
		{
			s = "[c44bf9]";
		}
		else
		{
			s = "[ffff00]";
		}

		
		return s;
	}
	public static string get_equip_enhance(int id, int enhance)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (id);
		string s = "[ffffff]";

		if (enhance == 0)
		{
			return s;
		}
		return s + " +" + enhance;
	}
    public static List<s_t_reward> get_equip_chongsheng_reward(dhc.equip_t equip)
    {
        List<s_t_reward> rewards = new List<s_t_reward>();
        if (get_equip_chongsheng_gold(equip) > 0)
        {
            s_t_reward reward = new s_t_reward();
            reward.type = 1;
            reward.value1 = 1;
            reward.value2 = get_equip_chongsheng_gold(equip);
            reward.value3 = 0;
            rewards.Add(reward);
 
        }
       
        List<s_t_reward> rewards1 = get_equip_chong_shneg_item(equip);
        for (int i = 0; i < rewards1.Count; i++)
        {
            rewards.Add(rewards1[i]);
        }
        List<s_t_reward> rewards2 = get_equip_chong_shneg_sp(equip);
        for (int i = 0; i < rewards2.Count; i++)
        {
            rewards.Add(rewards2[i]);
        }
        return rewards;
    }
    public static List<s_t_reward> get_equip_fenjie_reward(dhc.equip_t equip)
    {
        List<s_t_reward> rewards = new List<s_t_reward>();
        if (get_equip_chongsheng_gold(equip) > 0)
        {
            s_t_reward reward = new s_t_reward();
            reward.type = 1;
            reward.value1 = 1;
            reward.value2 = get_equip_chongsheng_gold(equip);
            reward.value3 = 0;
            rewards.Add(reward);
 
        }
       
        List<s_t_reward> rewards1 = get_equip_chong_shneg_item(equip);
        for (int i = 0; i < rewards1.Count; i++)
        {
            rewards.Add(rewards1[i]);
        }
        List<s_t_reward> rewards2 = get_equip_fenjie_hj(equip);
        for (int i = 0; i < rewards2.Count; i++)
        {
            rewards.Add(rewards2[i]);
        }

        return rewards; 
 
    }
    public static List<s_t_reward> get_equip_fenjie_hj(dhc.equip_t equip)
    {
        List<s_t_reward> rewards = new List<s_t_reward>();
        int value = get_equip_frame_num(equip);
        s_t_item value1 = game_data._instance.get_item(get_equip_frame_id(equip));
        int temp = get_equip_frame_id(equip);

        int gold = 0;

        if (temp == -1)
        {
            gold = game_data._instance.get_item(70010302).gold;
        }
        else
        {
            gold = game_data._instance.get_item(temp).gold;
        }
        s_t_equip _equip = game_data._instance.get_t_equip(equip.template_id);
        if (get_equip_frame_num(equip) * gold> 0)
        {
            s_t_reward reward = new s_t_reward();
            reward.type = 1;
            reward.value1 = 6;
            reward.value3 = 0;
            reward.value2 = _equip.sell;
            rewards.Add(reward);
        }
       
      
        if (_equip.sell_jh > 0)
        {
            s_t_reward reward1 = new s_t_reward();
            reward1.type = 2;
            reward1.value1 = 50120001;
            reward1.value2 = _equip.sell_jh;
            reward1.value3 = 0;
            rewards.Add(reward1);
        }

        return rewards;
    }
    public static List<s_t_reward> get_equip_chong_shneg_sp(dhc.equip_t equip)
    {
        List<s_t_reward> rewards = new List<s_t_reward>();
        //碎片
        if (get_equip_frame_id(equip) != -1)
        {
            s_t_reward reward1 = new s_t_reward();
            reward1.type = 2;
            reward1.value1 = get_equip_frame_id(equip);
            reward1.value2 = get_equip_frame_num(equip);
            reward1.value3 = 0;
            rewards.Add(reward1);
           
           
        }
        else
        {
            s_t_reward reward1 = new s_t_reward();
            reward1.type = 4;
            reward1.value1 = equip.template_id;
            rewards.Add(reward1);
        }
        
        return rewards;
    }
    public static List<s_t_reward> get_equip_chong_shneg_item(dhc.equip_t equip)
    {
        int m_stone_id = 50070001;
        List<s_t_reward> rewards = new List<s_t_reward>();
        //精炼
        s_t_reward reward = new s_t_reward();
        reward.type = 2;
        reward.value1 = m_stone_id;
        reward.value3 = 0;
        for (int i = 0; i < equip.jilian; i++)
        {
            s_t_equip _equip = game_data._instance.get_t_equip(equip.template_id);
            s_t_equip_jl _equip_jl = game_data._instance.get_t_equip_jl(i + 1);
           
            reward.value2 += _equip_jl.stones[_equip.font_color - 1];
            //sys._instance.m_self.remove_item(m_stone_id, _equip_jl.stones[_equip.font_color - 1]);
           
           
        }
        if (reward.value2 > 0)
        {
            rewards.Add(reward);
 
        }
        
        //改造
        if (equip.gaizao_counts > 0)
        {
            s_t_reward reward2 = new s_t_reward();
            reward2.type = 2;
            reward2.value1 = 50010001;
            reward2.value2 = equip.gaizao_counts;
            reward2.value3 = 0;
            rewards.Add(reward2);
 
        }
       //升星
        s_t_reward reward1 = new s_t_reward();
        reward1.type = 2;
        reward1.value1 = get_equip_frame_id(equip);
        reward1.value2 = 0;
        reward1.value3 = 0;
       
        for (int i = 0; i < equip.star; i++)
        {
            s_t_equip _equip = game_data._instance.get_t_equip(equip.template_id);
            s_t_equip_sx _equip_sx = game_data._instance.get_t_equip_sx(_equip.font_color, i + 1);
            reward1.value2 += _equip_sx.sp_num;
        }
        if (reward1.value2 > 0)
        {
            rewards.Add(reward1);
        }
        
        return rewards;
 
    }
    public static int get_equip_chongsheng_gold(dhc.equip_t equip)
    {
        int gold = 0;
        //强化
        for (int i = 0; i < equip.enhance; i++)
        {
            s_t_equip _equip = game_data._instance.get_t_equip(equip.template_id);
            gold += game_data._instance.get_enhance(i + 1, _equip.font_color);
 
        }

       //精炼
        for (int i = 0; i < equip.jilian; i++)
        {
            s_t_equip _equip = game_data._instance.get_t_equip(equip.template_id);
            s_t_equip_jl _equip_jl = game_data._instance.get_t_equip_jl(i + 1);
            gold += _equip_jl.golds[_equip.font_color - 1];
            //sys._instance.m_self.sub_att(e_player_attr.player_gold, _equip_jl.golds[_equip.font_color - 1]);
            //sys._instance.m_self.remove_item(m_stone_id, _equip_jl.stones[_equip.font_color - 1]);
 
        }
      
        //升星
        for (int i = 0; i < equip.star; i++)
        {
            s_t_equip _equip = game_data._instance.get_t_equip(equip.template_id);
            s_t_equip_sx _equip_sx = game_data._instance.get_t_equip_sx(_equip.font_color,i + 1);
            gold += _equip_sx.gold;
        }

        return gold;
    }
    public static int get_equip_frame_id(dhc.equip_t equip)
    {
        for (int i = 0; i < game_data._instance.m_dbc_item.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_item.get(0,i));
            s_t_item item = game_data._instance.get_item(id);
            if (item.def_1 == equip.template_id)
            {
                return item.id;
            }
 
        }
        return -1;
    }
    public static int get_equip_frame_num(dhc.equip_t equip)
    {
        for (int i = 0; i < game_data._instance.m_dbc_item.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_item.get(0, i));
            s_t_item item = game_data._instance.get_item(id);
            if (item.def_1 == equip.template_id)
            {
                return item.def_2;
            }

        }
        return 32;
 
    }
	public static string get_equip_real_name(int id)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (id);
		string s = game_data._instance.get_name_color (t_equip.font_color);
		return s + t_equip.name + "[-]";
	}

	public static string get_equip_color(int id)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (id);
		string s = game_data._instance.get_name_color (t_equip.font_color);
		return s;
	}

	public static string get_equip_name(int id, int enhance)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (id);
		string s = game_data._instance.get_name_color (t_equip.font_color);
		if (enhance == 0)
		{
			return s + t_equip.name + "[-]";
		}
		return s + t_equip.name + " +" + enhance + "[-]";
	}
	
	public static string get_equip_value(int id, int enhance, int star)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (id);
		s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx (t_equip.font_color, star);
		string text = "";
		if(t_equip_sx != null)
		{
			text = game_data._instance.get_value_string (t_equip.eattr.attr, ((int)(t_equip.eattr.value + t_equip.eattr.value * t_equip_sx.enhance_rate * enhance)));
		}
		else
		{
			text = game_data._instance.get_value_string (t_equip.eattr.attr, (int)(t_equip.eattr.value));
		}
		return text;
	}

	public static string get_equip_value_text(int id, int enhance, int star,int i)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (id);
		s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx (t_equip.font_color, star);
		string _text = "";
		List <string> text = new List<string>();
		if(t_equip_sx != null)
		{
			_text = game_data._instance.get_value_string (t_equip.eattr.attr, ((int)(t_equip.eattr.value + t_equip.eattr.value * t_equip_sx.enhance_rate * enhance)));
		}
		else
		{
			_text = game_data._instance.get_value_string (t_equip.eattr.attr, (int)(t_equip.eattr.value));
		}
		text.Add (_text);
		return text[0];
	}

	public static string get_equip_type(int id)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (id);
		string s = "sjjtb_003"; //武器
		if (t_equip.type == 2)
		{
			s =  "sjjtb_002";//衣服
		}
		else if (t_equip.type == 3)
		{
			s =  "sjjtb_004";//手套
		}
		else if (t_equip.type == 4)
		{
			s =  "sjjtb_001";//鞋子
		}
		return s ;
	}

	public static string get_equip_part_e(int id)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (id);
		string s = game_data._instance.get_t_language ("equip.cs_513_13");//武器
		
		if (t_equip.type == 2)
		{
			s =  game_data._instance.get_t_language ("equip.cs_517_8");//衣服
		}
		else if (t_equip.type == 3)
		{
			s =  game_data._instance.get_t_language ("equip.cs_521_8");//手套
		}
		else if (t_equip.type == 4)
		{
			s =  game_data._instance.get_t_language ("equip.cs_525_8");//鞋子
		}
		return s ;
	}

	public static string get_equip_part(int id)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (id);
		string s = "sjjtb_003"; //武器
		
		if (t_equip.type == 2)
		{
			s =  "sjjtb_002";//衣服
		}
		else if (t_equip.type == 3)
		{
			s =  "sjjtb_004";//手套
		}
		else if (t_equip.type == 4)
		{
			s =  "sjjtb_001";//鞋子
		}
		return s ;
	}

	public static int comp(dhc.equip_t x, dhc.equip_t y)
	{
		if (x.template_id == 999999 && y.template_id == 999999)
		{
			return 0;
		}
		else if (x.template_id == 999999 && y.template_id != 999999)
		{
			return -1;
		}
		else if (x.template_id != 999999 && y.template_id == 999999)
		{
			return 1;
		}
		s_t_equip t_equip_x = game_data._instance.get_t_equip(x.template_id);
		s_t_equip t_equip_y = game_data._instance.get_t_equip(y.template_id);
		if(x.role_guid != 0 && y.role_guid == 0)
		{
			return -1;
		}
		else if(x.role_guid == 0 && y.role_guid != 0)
		{
			return 1;
		}
		else if (t_equip_x.font_color > t_equip_y.font_color)
		{
			return -1;
		}
		else if (t_equip_x.font_color < t_equip_y.font_color)
		{
			return 1;
		}
		else if (t_equip_x.type < t_equip_y.type)
		{
			return -1;
		}
		else if (t_equip_x.type > t_equip_y.type)
		{
			return 1;
		}
		else if (t_equip_x.id < t_equip_y.id)
		{
			return -1;
		}
		else if (t_equip_x.id > t_equip_y.id)
		{
			return 1;
		}
		else if (x.star > y.star)
		{
			return -1;
		}
		else if (x.star < y.star)
		{
			return 1;
		}
		else if (x.enhance > y.enhance)
		{
			return -1;
		}
		else if (x.enhance < y.enhance)
		{
			return 1;
		}
		
		return 0; 
	}

	public static bool is_enhance(dhc.equip_t _equip)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (_equip.template_id);
		int gold = game_data._instance.get_enhance (_equip.enhance + 1, t_equip.font_color);
		int enhance_up = Mathf.Min ( sys._instance.m_self.m_t_player.level,game_data._instance.m_dbc_enhance.get_y() -1);
		if(gold <= sys._instance.m_self.get_att(e_player_attr.player_gold) && _equip.enhance < enhance_up)
		{
			return true;
		}
		return false;
	}

	public static bool is_jinglian(dhc.equip_t m_equip)
	{
		s_t_equip_jl _equip_jl_next = game_data._instance.get_t_equip_jl (m_equip.jilian +1);
		if(_equip_jl_next == null)
		{
			return false;
		}
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		int stone_num = sys._instance.m_self.get_item_num((uint)50070001);
		if(stone_num >= _equip_jl_next.stones[t_equip.font_color -1] && sys._instance.m_self.get_att(e_player_attr.player_gold) >= _equip_jl_next.golds[t_equip.font_color -1])
		{
			return true;
		}
		return false;
	}

	public static bool gold_enough(dhc.equip_t _equip)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (_equip.template_id);
		int gold = game_data._instance.get_enhance (_equip.enhance + 1, t_equip.font_color);
		if(gold > sys._instance.m_self.get_att(e_player_attr.player_gold) && _equip.enhance < ((sys._instance.m_self.m_t_player.level + 10) / 10 * 10))
		{
			return true;
		}
		return false;
	}

	public static string get_baoshi_color(int type)
	{
		string s = "";
		if (type == 1)
		{
			s = "[f52410]";
		}
		else if (type == 2)
		{
			s = "[f1f020]";
		}
		else if (type == 3)
		{
			s = "[28f4fe]";
		}
		else if (type == 4)
		{
			s = "[18f312]";
		}
		else
		{
			s = "[c44bf9]";
		}

		return s;
	}

	
	public static string get_equip_sx_text(dhc.equip_t m_equip, int star,int i)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip(m_equip.template_id);
		s_t_equip_sx _equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color,star);
		List <string> text = new List<string>();
		string _text = "";
		int value = 0;
		int value1 = 0;
		if(_equip_sx != null)
		{
			value1 = (int)(t_equip.eattr.value + (t_equip.eattr.value*_equip_sx.enhance_rate*m_equip.enhance));
		}
		else
		{
			value1 = (int)(t_equip.eattr.value);
		}
		_text = game_data._instance.get_value_string (t_equip.eattr.attr, value1);
		text.Add (_text);
		if(_equip_sx != null)
		{
			value = (int)(t_equip.eattr.value*_equip_sx.enhance_rate);
			_text =  game_data._instance.get_value_string (t_equip.eattr.attr, value);
			text.Add (_text);
		}
		else
		{
			_text = "0";
			text.Add (_text);
		}
		if(i >= text.Count)
		{
			return "0";
		}
		return text[i];
	}

	public static string get_equip_jl_text(dhc.equip_t m_equip, int jinglian,int i)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip(m_equip.template_id);
		List <string> text = new List<string>();
		string _text = "";
		int value = 0;
		value = (int)(t_equip.ejlattr [0].value * jinglian);
		_text = game_data._instance.get_value_string (t_equip.ejlattr[0].attr, value);
		text.Add (_text);
		value = (int)(t_equip.ejlattr[1].value*jinglian);
		_text = game_data._instance.get_value_string (t_equip.ejlattr[1].attr, value);
		text.Add (_text);
		
		return text[i];
	}

	public static string get_equip_jl_text(int equip_id, int jinglian,int i)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip(equip_id);
		List <string> text = new List<string>();
		string _text = "";
		int value = 0;
		value = (int)(t_equip.ejlattr [0].value * jinglian);
		_text = game_data._instance.get_value_string (t_equip.ejlattr[0].attr, value);
		text.Add (_text);
		value = (int)(t_equip.ejlattr[1].value*jinglian);
		_text = game_data._instance.get_value_string (t_equip.ejlattr[1].attr, value);
		text.Add (_text);
		
		return text[i];
	}

	public static List<string> get_equip_sx_text(dhc.equip_t m_equip)
	{
		s_t_equip t_equip = game_data._instance.get_t_equip(m_equip.template_id);
		s_t_equip_sx _equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color,m_equip.star);
		Dictionary<int,int> m_attrs = new Dictionary<int, int>();
		if(t_equip.eattr.attr <= 5)
		{
			//int value = t_equip.eattr.value + (int)(t_equip.eattr.value + (t_equip.eattr.value*_equip_sx.enhance_rate*m_equip.enhance));
			m_attrs.Add (t_equip.eattr.attr, t_equip.eattr.value);
		}
		if(m_attrs.ContainsKey(t_equip.ejlattr[0].attr))
		{
			m_attrs[t_equip.ejlattr[0].attr] += (int)(t_equip.ejlattr[0].value* m_equip.jilian);
		}
		else if(!m_attrs.ContainsKey(t_equip.ejlattr[0].attr) && t_equip.ejlattr[0].value <= 5)
		{
			m_attrs.Add(t_equip.ejlattr[0].attr,t_equip.ejlattr[0].value* m_equip.jilian);
		}
		if(m_attrs.ContainsKey(t_equip.ejlattr[1].attr))
		{
			m_attrs[t_equip.ejlattr[1].attr] += (int)(t_equip.ejlattr[1].value* m_equip.jilian);
		}
		else if(!m_attrs.ContainsKey(t_equip.ejlattr[1].attr) && t_equip.ejlattr[1].attr <= 5)
		{
			m_attrs.Add(t_equip.ejlattr[1].attr,t_equip.ejlattr[1].value* m_equip.jilian);
		}
		s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color, m_equip.star);
		int num = 0;
		for(int i =0;i < m_equip.rand_ids.Count;++i)
		{
			for ( int j = 0 ; j < t_equip.eeattr.Count; ++j)
			{
				if(m_equip.rand_ids[i] == t_equip.eeattr[j].attr)
				{
					if(t_equip_sx != null)
					{
						num = (int)(t_equip.eeattr[j].value2 + t_equip.eeattr[j].value2 * t_equip_sx.enhance_rate * m_equip.enhance);
					}
					else
					{
						num = (int)(t_equip.eeattr[j].value2);
					}
					if(m_attrs.ContainsKey( t_equip.eeattr[j].attr))
					{
						m_attrs[ t_equip.eeattr[j].attr] += num;
					}
					else if(!m_attrs.ContainsKey( t_equip.eeattr[j].attr) && t_equip.eeattr[j].attr <= 5)
					{
						m_attrs.Add( t_equip.eeattr[j].attr,num);
					}
					break;
				}
			}
		}
		List <string> text = new List<string>();
		string _text = "";
		foreach(int key in m_attrs.Keys)
		{
			_text = game_data._instance.get_value_string (key, m_attrs[key]);
			text.Add (_text);
		}
		
		return text;
	}

	public static string equip_skill(int type,int jinlian,int m_type = 0)
	{
		string text = "";
		List<s_t_equip_skill> equip_skills = new List<s_t_equip_skill>();
		for(int i = 0 ; i < game_data._instance.m_dbc_equip_skill.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_equip_skill.get(0,i));
			s_t_equip_skill t_equip_skill = game_data._instance.get_t_equip_skill (id);
			if(t_equip_skill.part == type)
			{
				equip_skills.Add(t_equip_skill);
			}
		}
		bool first = false;
		for(int i = 0; i < equip_skills.Count;++i)
		{
			if(equip_skills[i].type == 1)
			{
				if (first)
				{
					text += "\n";
				}
				if(jinlian < equip_skills[i].jinglian && m_type != 1)
				{
					text += "[777777]";
					text += "["+ equip_skills[i].name + "] " + game_data._instance.get_value_string(equip_skills[i].def1,equip_skills[i].def2) 
						+ " (" + string.Format(game_data._instance.get_t_language ("equip.cs_844_29") , equip_skills[i].jinglian ) +  ")";//精炼至{0}阶开启
				}
				else
				{
					string[] s = game_data._instance.get_value_string(equip_skills[i].def1,equip_skills[i].def2).Split('+');
					text += "["+ equip_skills[i].name + "] " + "[0aabff]" + s[0] + "[-]" + "[0aff16]+" +s[1] + "[-]"
						+ " [ffde00](" + string.Format(game_data._instance.get_t_language ("equip.cs_850_37") , equip_skills[i].jinglian );//精炼至[-][0aff16]{0}阶[-][ffde00]开启)[-]
				}
				first = true;
			}
			else if(equip_skills[i].type == 2)
			{
				if (first)
				{
					text += "\n";
				}
				if(jinlian < equip_skills[i].jinglian && m_type != 1)
				{
					text += "[777777]";
					text += "["+ equip_skills[i].name + "] " + equip_skills[i].desc.Replace("{{n1}}",equip_skills[i].def1.ToString())
						+ " (" + string.Format(game_data._instance.get_t_language ("equip.cs_864_29") , equip_skills[i].jinglian);//精炼至{0}阶开启)
				}
				else
				{
					equip_skills[i].desc = equip_skills[i].desc.Replace("%","");
					text += "["+ equip_skills[i].name + "] " + "[0aabff]" + equip_skills[i].desc.Replace("{{n1}}", "[-][0aff16]"+ equip_skills[i].def1.ToString() + "%[-][0aabff]")
						+ " [-][ffde00](" + string.Format(game_data._instance.get_t_language ("equip.cs_850_37") , equip_skills[i].jinglian);//精炼至[-][0aff16]{0}阶[-][ffde00]开启)[-]
				}
				first = true;
			}
			else if(equip_skills[i].type == 3 || equip_skills[i].type == 4 || equip_skills[i].type == 5 
			        || equip_skills[i].type == 6 || equip_skills[i].type == 7)
			{
				string s = "";
				if (first)
				{
					text += "\n";
				}
				if(jinlian < equip_skills[i].jinglian && m_type != 1)
				{
					text += "[777777]";
					s =  equip_skills[i].desc.Replace("{{n1}}",equip_skills[i].def1.ToString());
					s = s.Replace("{{n2}}",equip_skills[i].def2.ToString());
					text += "["+ equip_skills[i].name + "] " + s + " (" + string.Format(game_data._instance.get_t_language ("equip.cs_864_29") , equip_skills[i].jinglian);//精炼至{0}阶开启)
				}
				else
				{
					equip_skills[i].desc =  equip_skills[i].desc.Replace("%","");
					s =  equip_skills[i].desc.Replace("{{n1}}", "[-][0aff16]"+equip_skills[i].def1.ToString()+ "%[-][0aabff]");
					s = s.Replace("{{n2}}","[-][0aff16]" + equip_skills[i].def2.ToString()+ "%[-][0aabff]");
					text += "["+ equip_skills[i].name + "] " + "[0aabff]" + s + " [-][ffde00](" + string.Format(game_data._instance.get_t_language ("equip.cs_850_37"), equip_skills[i].jinglian);//精炼至[-][0aff16]{0}阶[-][ffde00]开启)[-]
				}
				first = true;
			}
		}
		return text;
	}
}
