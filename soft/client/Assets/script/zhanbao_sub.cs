
using UnityEngine;
using System.Collections;

public class zhanbao_sub : MonoBehaviour{
	public GameObject m_icon;
	public UILabel m_lv_name;
	public UILabel m_bf;
	public UILabel m_result;
	public UISprite m_shengfu;
	public UILabel m_time;

	public int level;
	public string name;
	public int bf;
	public int shengfu;
	public long time;
	public long guid;
	public int m_id;
	public int m_template;
	// Use this for initialization

	public void reset()
	{
		s_t_item _item = game_data._instance.get_item (m_id);
		sys._instance.remove_child (m_icon);
		m_icon.GetComponent<UISprite>().spriteName = game_data._instance.get_t_class (m_template).icon;;
		m_lv_name.text = "Lv." + level + " " + name;
		m_bf.text = game_data._instance.get_t_language ("zhanbao_sub.cs_28_14") + bf;//战力 : 

		if(shengfu == 1)
		{
			m_result.text = string.Format(game_data._instance.get_t_language ("zhanbao_sub.cs_32_33"), sys._instance.get_res_info(2,_item.id,0,0) );//[00baff]你被对方打败，你带着[-][{0}][00b0ff]灰溜溜地逃走了。

		}
		else if(shengfu == 0)
		{
			m_result.text = string.Format(game_data._instance.get_t_language ("zhanbao_sub.cs_37_33"), sys._instance.get_res_info(2,_item.id,0,0) );//[00baff]对方试图抢夺你的[-][{0}][00b0ff]，被你赶走了。[-]

		}
		else if(shengfu == 2)
		{
			m_result.text = string.Format(game_data._instance.get_t_language ("zhanbao_sub.cs_42_33") , sys._instance.get_res_info(2,_item.id,0,0) );//[00baff]你被对方打败，你的[-][{0}][00b0ff]被抢走了。
		}
		else if(shengfu == 3)
		{
			m_result.text = string.Format(game_data._instance.get_t_language ("zhanbao_sub.cs_46_33"), sys._instance.get_res_info(2,_item.id,0,0) );//[00baff]你被对方连续抢夺，你的[-][{0}][00b0ff]被抢走了。
		}
		else if(shengfu == 4)
		{
			m_result.text = string.Format(game_data._instance.get_t_language ("zhanbao_sub.cs_50_33") , sys._instance.get_res_info(2,_item.id,0,0) );//[00baff]对方试图连续抢夺你，你带着[-][{0}][00b0ff]逃走了。
		}


		m_time.text = get_time_show_ex ((long)(timer.now() - (ulong)time));
		if(shengfu == 0)
		{
			m_shengfu.spriteName = "qdzb_sl001";
		}
		else
		{
			m_shengfu.spriteName = "qdzb_sb001";
		}

	}
	public static string get_time_show_ex(long t)
	{
		if (t < 0)
		{
			t = 0;
		}
		int tt = (int)(t / 1000);
		int day = tt / 3600 / 24;
		//计算小时,用毫秒总数除以(1000*60*24),后去掉小数点
		int hour = tt / 3600 % 24;
		//计算分钟,用毫秒总数减去小时乘以(1000*60*24)后,除以(1000*60),再去掉小数点
		int min = tt % 3600 / 60;
		//同上
		int sec = tt % 60;
		//ulong msec = t - hour*(1000*60*24*60) - min*(1000*60) - sec*1000;
		//拼接字符串
		string _hour = hour.ToString ();
		string _min = min.ToString ();
		string _sec = sec.ToString ();
		
		if(_hour.Length < 2)
		{
			_hour = "0" + _hour;
		}
		
		if(_min.Length < 2)
		{
			_min = "0" + _min;
		}
		
		if(_sec.Length < 2)
		{
			_sec = "0" + _sec;
		}
		string timeString = "";
		if(day > 7)
		{
			timeString = game_data._instance.get_t_language ("zhanbao_sub.cs_102_16");//7天前
		}
		else if(day >= 1 && day <=7)
		{
			timeString = string.Format(game_data._instance.get_t_language ("arena.cs_535_41"),day);//{0}天前
		}
		else if(hour > 0)
		{
			timeString = string.Format(game_data._instance.get_t_language ("arena.cs_530_41"),hour );//{0}小时前
		}
		else
		{
			timeString = string.Format(game_data._instance.get_t_language ("arena.cs_525_41"),min);//{0}分钟前
		}

		return timeString;
	}

}
