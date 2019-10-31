
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure : MonoBehaviour {

	// Use this for initialization

	public static string get_treasure_name(dhc.treasure_t treasure)
	{
		return get_treasure_name (treasure.template_id, treasure.enhance);
	}

	public static string get_treasure_name(int id, int enhance)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (id);
		string s = game_data._instance.get_name_color (t_treasure.font_color);
		if (enhance == 0)
		{
			return s + t_treasure.name + "[-]";
		}
		return s + t_treasure.name + " +" + enhance + "[-]";
	}
	
	public static string get_treasure_value(int id, int enhance,int jinglian)
	{
		string text = "";
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (id);
		s_t_baowu_jl _treasure_jl = game_data._instance.get_t_baowu_jl(jinglian);
		text = game_data._instance.get_t_language ("boss.cs_178_59") +" " + enhance.ToString ();//等级
		if(enhance >= sys._instance.m_self.m_t_player.level)
		{
			text = game_data._instance.get_t_language ("treasure.cs_33_10");//等级已达上限
		}
        if (t_treasure.type != 5)
        {
            text += "\n" + "[-]" + game_data._instance.get_float_string(t_treasure.attr1, ((float)(t_treasure.value1 + enhance * t_treasure.value1)));
            if (t_treasure.attr2 > 0)
            {
                text += "\n" + "[-]" + game_data._instance.get_float_string(t_treasure.attr2, ((float)(t_treasure.value2 + enhance * t_treasure.value2)));
            }

        }
        else
        {
            text += "\n" + string.Format(game_data._instance.get_t_language ("treasure.cs_46_41") , t_treasure.exp );//饰品强化时提供{0}点饰品经验
        }
		
		return text;
	}

	public static string get_treasure_value1(int id, int enhance,int jinglian = 0)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (id);
		float value = (float)(t_treasure.value1 + enhance* t_treasure.value1);
		string text = game_data._instance.get_float_string (t_treasure.attr1, value);
		return text;
	}
    public static int get_treasure_gold(dhc.treasure_t treasure)
    {
        int gold = 0;




        return gold;
    }
    public static List<s_t_reward> get_treasure_reward(dhc.treasure_t treasure)
    {
        List<s_t_reward> rewards = new List<s_t_reward>();
        int _num = treasure.enhance_counts / 10000;
        int bag_num = _num / 5;
        int zi_num = _num % 5;
        int lan_num = treasure.enhance_counts % 10000 / 3000;
        if (treasure.star_gold > 0)
        {
            s_t_reward reward = new s_t_reward();
            reward.type = 1;
            reward.value1 = 1;
            reward.value2 = treasure.star_gold;
            rewards.Add(reward);
        }
        int gold = get_treasure_chongsheng_gold(treasure);
        if (gold > 0)
        {
            s_t_reward reward = new s_t_reward();
            reward.type = 1;
            reward.value1 = 1;
            reward.value2 = gold;
            rewards.Add(reward);
        }
        if(bag_num > 0)
        {
            s_t_reward reward = new s_t_reward();
            reward.type = 2;
            reward.value1 = 10010062;
            reward.value2 = bag_num;
            rewards.Add(reward);
        }
        if (zi_num > 0)
        {
            for (int i = 0; i < zi_num; i++)
            {
                s_t_reward reward = new s_t_reward();
                reward.type = 6;
                reward.value1 = 13001;
                reward.value2 = 1;
                rewards.Add(reward);
 
            }
           
 
        }
        if (lan_num > 0)
        {
            for (int i = 0; i < lan_num; i++)
            {
                s_t_reward reward = new s_t_reward();
                reward.type = 6;
                reward.value1 = 12001;
                reward.value2 = 1;
                rewards.Add(reward);
            }
           
        }
        int num = 0;
        int num1 = 0;
        for(int i = 0 ;i < treasure.jilian;i++)
        {
            s_t_baowu _treasure = game_data._instance.get_t_baowu(treasure.template_id);
            s_t_baowu_jl _baowu_jl = game_data._instance.get_t_baowu_jl(i + 1);
            num += _baowu_jl.num;
            num1 += _baowu_jl.stone;
        }
        if (num > 0)
        {
            for (int i = 0; i < num; i++)
            {
                s_t_reward reward1 = new s_t_reward();
                reward1.type = 6;
                reward1.value1 = treasure.template_id;
                reward1.value2 = 1;
                rewards.Add(reward1);
 
            }
           
 
        }
        if (num1 > 0)
        {
            s_t_reward reward1 = new s_t_reward();
            reward1.type = 2;
            reward1.value1 = 50100001;
            reward1.value2 = num1;
            rewards.Add(reward1);
 
        }
		s_t_reward reward4 = new s_t_reward();
		reward4.type = 6;
		reward4.value1 = treasure.template_id;
		reward4.value2 = 1;
		rewards.Add(reward4);
        return rewards;
    }
    public static int get_treasure_chongsheng_gold(dhc.treasure_t treasure)
    {
        int gold = 0;
        for (int i = 0; i < treasure.jilian; i++)
        {
            s_t_baowu _treasure = game_data._instance.get_t_baowu(treasure.template_id);
            s_t_baowu_jl _baowu_jl = game_data._instance.get_t_baowu_jl(i + 1);
            gold += _baowu_jl.cost;
        }
        gold += treasure.enhance_counts;
        return gold;
 
    }
	public static string get_treasure_value2(int id, int enhance,int jinglian = 0)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (id);
		float value = (float)(t_treasure.value2 + enhance* t_treasure.value2);
		string text = game_data._instance.get_float_string (t_treasure.attr2, value);
		return text;
	}

	public static int comp(dhc.treasure_t x, dhc.treasure_t y)
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
		s_t_baowu t_treasure_x = game_data._instance.get_t_baowu(x.template_id);
		s_t_baowu t_treasure_y = game_data._instance.get_t_baowu(y.template_id);
		if(x.role_guid != 0 && y.role_guid == 0)
		{
			return -1;
		}
		else if(x.role_guid == 0 && y.role_guid != 0)
		{
			return 1;
		}
		if (t_treasure_x.font_color > t_treasure_y.font_color)
		{
			return -1;
		}
		else if (t_treasure_x.font_color < t_treasure_y.font_color)
		{
			return 1;
		}
		else if (t_treasure_x.type < t_treasure_y.type)
		{
			return -1;
		}
		else if (t_treasure_x.type > t_treasure_y.type)
		{
			return 1;
		}
		else if (t_treasure_x.id < t_treasure_y.id)
		{
			return -1;
		}
		else if (t_treasure_x.id > t_treasure_y.id)
		{
			return 1;
		}
		else if (x.jilian > y.jilian)
		{
			return -1;
		}
		else if (x.jilian < y.jilian)
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

	public static string get_treasure_type_e(int id)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (id);
		string s = "zsdtb_004";//game_data._instance.get_t_language ("treasure.cs_258_13")//坠
		
		if (t_treasure.type == 2)
		{
			s = "zsdtb_002";//game_data._instance.get_t_language ("treasure.cs_262_7")//徽
		}
		else if (t_treasure.type == 3)
		{
			s = "zsdtb_001";//game_data._instance.get_t_language ("treasure.cs_266_7")//护
		}
		else if (t_treasure.type == 4)
		{
			s = "zsdtb_003";//game_data._instance.get_t_language ("treasure.cs_270_7");//戒
		}
		else if (t_treasure.type == 5)
		{
			s = "zsdtb_002";//game_data._instance.get_t_language ("treasure.cs_274_7");//晶
		}
		return s ;
	}

	public static string get_treasure_type(int id)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (id);
		string s = game_data._instance.get_t_language ("treasure.cs_258_13");//坠
		
		if (t_treasure.type == 2)
		{
			s = game_data._instance.get_t_language ("treasure.cs_262_7");//徽
		}
		else if (t_treasure.type == 3)
		{
			s = game_data._instance.get_t_language ("treasure.cs_266_7");//护
		}
		else if (t_treasure.type == 4)
		{
			s = game_data._instance.get_t_language ("treasure.cs_270_7");//戒
		}
		else if (t_treasure.type == 5)
		{
			s = game_data._instance.get_t_language ("treasure.cs_274_7");//晶
		}
		return s ;
	}

	public static string get_treasure_part(int id)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (id);
		string s = game_data._instance.get_t_language ("treasure.cs_282_13");//攻击饰品
		
		if (t_treasure.type == 2)
		{
			s =  game_data._instance.get_t_language ("treasure.cs_286_8");//防御饰品
		}
		return s ;
	}

	public static string get_treasure_real_name(int id)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (id);
		string s = game_data._instance.get_name_color (t_treasure.font_color);
		return s + t_treasure.name + "[-]";
	}

	public static string get_treasure_color(int id)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (id);
		string s = game_data._instance.get_name_color (t_treasure.font_color);
		return s;
	}


	static bool is_up(dhc.treasure_t _treasure)
	{
		s_t_baowu treasure = game_data._instance.get_t_baowu (_treasure.template_id);
		int exp = game_data._instance.get_treasure_enhance(_treasure.enhance+1, treasure.font_color);
		int gold = 0;
		int treasure_exp = 0;
		for(int i = 0; i < sys._instance.m_self.m_t_player.treasures.Count;++i)
		{
			dhc.treasure_t _t_treasure = sys._instance.m_self.get_treasure_guid(sys._instance.m_self.m_t_player.treasures[i]);
			s_t_baowu t_treasure = game_data._instance.get_t_baowu (_t_treasure.template_id);
			if( _t_treasure.role_guid>0 || _t_treasure.guid == _treasure.guid || _t_treasure.locked == 1 || _t_treasure.jilian > 0)
			{
				continue;
			}
			treasure_exp += t_treasure.exp;
			treasure_exp += _t_treasure.enhance_exp;
			treasure_exp += game_data._instance.get_total_treasure_enhance(_t_treasure.enhance, t_treasure.font_color);
			if(_t_treasure.jilian > 0)
			{
				treasure_exp += t_treasure.exp*_t_treasure.jilian;
			}
			int num = sys._instance.m_self.m_t_player.level;
			if(num > (game_data._instance.m_dbc_treasure_enhance.get_y() - 1))
			{
				num = game_data._instance.m_dbc_treasure_enhance.get_y() - 1;
			}
			int exp1 = game_data._instance.get_total_treasure_enhance(_treasure.enhance, treasure.font_color);
			if(treasure_exp + _treasure.enhance_exp > game_data._instance.get_total_treasure_enhance(num,t_treasure.font_color)- exp1 )
			{
				gold = game_data._instance.get_total_treasure_enhance(num,t_treasure.font_color)- exp1 - _treasure.enhance_exp;
			}
			else
			{
				gold = exp - _treasure.enhance_exp;
			}
			if(treasure_exp + _treasure.enhance_exp > exp )
			{
				if(gold <= sys._instance.m_self.get_att(e_player_attr.player_gold) && _treasure.enhance+1 <= sys._instance.m_self.get_att(e_player_attr.player_level))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool is_enhance(dhc.treasure_t _treasure)
	{
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasue_qh)
		{
			return false;
		}
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (_treasure.template_id);
		if(_treasure.enhance >= game_data._instance.m_dbc_treasure_enhance.get_y() -1)
		{
			return false;
		}
		if( _treasure.enhance < sys._instance.m_self.get_att(e_player_attr.player_level)&& is_up(_treasure))
		{
			return true;
		}
		return false;
	}

	public static bool is_jinglian(dhc.treasure_t m_treasure)
	{
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure_jl)
		{
			return false;
		}
		s_t_baowu_jl _treasure_jl_next = game_data._instance.get_t_baowu_jl (m_treasure.jilian +1);
		if(_treasure_jl_next == null)
		{
			return false;
		}
		s_t_baowu_jl _treasure_jl = game_data._instance.get_t_baowu_jl (m_treasure.jilian);
		int stone_num = sys._instance.m_self.get_item_num((uint)50100001);
		if(stone_num >= _treasure_jl_next.stone && sys._instance.m_self.get_att(e_player_attr.player_gold) >= _treasure_jl_next.cost
		   && jl_guids(m_treasure) >= _treasure_jl_next.num)
		{
			return true;
		}
		return false;
	}

	static int jl_guids(dhc.treasure_t m_treasure)
	{
		int count = 0;
		for(int i = 0; i < sys._instance.m_self.m_t_player.treasures.Count;++i)
		{
			dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_guid(sys._instance.m_self.m_t_player.treasures[i]);
			if(_treasure.role_guid !=0 || _treasure.locked == 1 || _treasure.jilian != 0 || _treasure.enhance != 0 || m_treasure.guid  == sys._instance.m_self.m_t_player.treasures[i])
			{
				continue;
			}
			if(_treasure.template_id == m_treasure.template_id)
			{
				count ++;
			}
		}
		return count;
	}

	public static string get_treasure_attr_text(int id, int enhance,int i)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu(id);
		List <string> text = new List<string>();
		string _text = "";
		float value = 0;
		value = (float)(t_treasure.value1 +enhance*  t_treasure.value1);
		_text =  game_data._instance.get_float_string (t_treasure.attr1, value);
		string [] text_ = _text.Split('+');
		text.Add (text_[1]);
		if(t_treasure.attr2 > 0)
		{
			value = (float)(t_treasure.value2 +enhance*  t_treasure.value2);
			_text =  game_data._instance.get_float_string (t_treasure.attr2, value);
			text_ = _text.Split('+');
			text.Add (text_[1]);
		}
		
		return text[i];
	}

	public static List<string> get_treasure_attr(int id, int enhance)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu(id);
		List <string> text = new List<string>();
		string _text = "";
		float value = 0;
		value = (float)(t_treasure.value1 +enhance*  t_treasure.value1);
		_text =  game_data._instance.get_float_string (t_treasure.attr1, value);
		text.Add (_text);
		if(t_treasure.attr2 > 0)
		{
			value = (float)(t_treasure.value2 +enhance*  t_treasure.value2);
			_text =  game_data._instance.get_float_string (t_treasure.attr2, value);
			text.Add (_text);
		}
		
		return text;
	}

	
	public static string get_treasure_jl_text(dhc.treasure_t m_treasure,int jinglian,int i)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu(m_treasure.template_id);
		List <string> text = new List<string>();
		string _text = "";
		float value = 0;		
		value = (float)(t_treasure.jl_value_0* jinglian);
		_text = game_data._instance.get_float_string (t_treasure.jl_type_0, value);
		text.Add (_text);
		value = (float)(t_treasure.jl_value_1*jinglian);
		_text = game_data._instance.get_float_string (t_treasure.jl_type_1, value);
		text.Add (_text);
		value = (float)(t_treasure.jl_value_2*jinglian);
		_text = game_data._instance.get_float_string (t_treasure.jl_type_2, value);
		text.Add (_text);
		
		return text[i];
	}

	public static string get_treasure_jl_text(int id ,int jinglian,int i)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu(id);
		List <string> text = new List<string>();
		string _text = "";
		float value = 0;		
		value = (float)(t_treasure.jl_value_0* jinglian);
		_text = game_data._instance.get_float_string (t_treasure.jl_type_0, value);
		text.Add (_text);
		value = (float)(t_treasure.jl_value_1*jinglian);
		_text = game_data._instance.get_float_string (t_treasure.jl_type_1, value);
		text.Add (_text);
		value = (float)(t_treasure.jl_value_2*jinglian);
		_text = game_data._instance.get_float_string (t_treasure.jl_type_2, value);
		text.Add (_text);
		
		return text[i];
	}


	public static List<string> get_treasure_sx_text(dhc.treasure_t m_treasure)
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu(m_treasure.template_id);
		s_t_baowu_jl _treasure_jl = game_data._instance.get_t_baowu_jl(m_treasure.jilian);
		Dictionary<int,float> m_attrs = new Dictionary<int, float>();

		float value = t_treasure.value1 + (t_treasure.value1 + (t_treasure.value1*m_treasure.enhance));
		m_attrs.Add (t_treasure.attr1, value);
		float value1 = t_treasure.value2 + (t_treasure.value2 + (t_treasure.value2*m_treasure.enhance));
		m_attrs.Add (t_treasure.attr2, value1);
		if(m_attrs.ContainsKey(t_treasure.jl_type_0))
		{
			m_attrs[t_treasure.jl_type_0] += t_treasure.jl_value_0* m_treasure.jilian;
		}
		else if(!m_attrs.ContainsKey(t_treasure.jl_type_0))
		{
			m_attrs.Add(t_treasure.jl_type_0,t_treasure.jl_value_0* m_treasure.jilian);
		}
		if(m_attrs.ContainsKey(t_treasure.jl_type_1))
		{
			m_attrs[t_treasure.jl_type_1] += (t_treasure.jl_value_1* m_treasure.jilian);
		}
		else if(!m_attrs.ContainsKey(t_treasure.jl_type_1))
		{
			m_attrs.Add(t_treasure.jl_type_1,t_treasure.jl_value_1* m_treasure.jilian);
		}
		if(m_attrs.ContainsKey(t_treasure.jl_type_2))
		{
			m_attrs[t_treasure.jl_type_2] += (t_treasure.jl_value_2* m_treasure.jilian);
		}
		else if(!m_attrs.ContainsKey(t_treasure.jl_type_2))
		{
			m_attrs.Add(t_treasure.jl_type_2,t_treasure.jl_value_2* m_treasure.jilian);
		}
		List <string> text = new List<string>();
		string _text = "";
		foreach(int key in m_attrs.Keys)
		{
			_text = game_data._instance.get_float_string (key, m_attrs[key]);
			text.Add (_text);
		}
		
		return text;
	}

	public static string get_baowu_sx_text(dhc.treasure_t m_treasure,int star)
	{
		string text = "";
		s_t_baowu t_treasure = game_data._instance.get_t_baowu(m_treasure.template_id);
		s_t_baowu_sx t_baowu_sx = game_data._instance.get_t_baowu_sx (star, t_treasure.font_color);
		text = game_data._instance.get_value_string (t_treasure.attr1, m_treasure.star_var);
		if(t_treasure.attr1 == 1)
		{
			text = text.Replace(game_data._instance.get_t_language ("treasure.cs_548_23"),game_data._instance.get_t_language ("treasure.cs_548_28"));//生命//防御
		}
		return text;
	}

	public static string get_next_baowu_sx_text(dhc.treasure_t m_treasure,int star,int type)
	{
		string text = "";
		int value = 0;
		s_t_baowu t_treasure = game_data._instance.get_t_baowu(m_treasure.template_id);
		s_t_baowu_sx t_baowu_sx = game_data._instance.get_t_baowu_sx (star, t_treasure.font_color);
		s_t_baowu_sx t_baowu_next_sx = game_data._instance.get_t_baowu_sx (star+1, t_treasure.font_color);
		if(t_baowu_sx == null)
		{
			value = 0;
		}
		else
		{
			value = t_baowu_sx.valuemax;
		}
		if(type == 1)
		{
			text = game_data._instance.get_value_string (t_treasure.attr1, value +t_baowu_next_sx.value1*t_baowu_next_sx.process);
		}
		else if(type == 2)
		{
			text =game_data._instance.get_t_language ("treasure.cs_574_9") + game_data._instance.get_value_string (t_treasure.attr1, t_baowu_next_sx.value2*t_baowu_next_sx.process);//额外
		}
		if(t_treasure.attr1 == 1)
		{
			text = text.Replace(game_data._instance.get_t_language ("treasure.cs_548_23"),game_data._instance.get_t_language ("treasure.cs_548_28"));//生命//防御
		}
		return text;
	}

}
