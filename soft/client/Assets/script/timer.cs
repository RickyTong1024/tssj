
using UnityEngine;
using System.Collections;
using System;

public class timer {

	static long dtime_;
	static long dtime1_;
	public static ulong start_time_;

	public static void set_server_time(ulong server_time)
	{
		dtime_ = System.DateTime.Now.Ticks / 10000 - (long)server_time;
		dtime1_ = (System.DateTime.Now.Ticks - System.DateTime.Parse ("1/1/1970").Ticks) / 10000 - 28800000 - (long)server_time;
	}

	public static System.DateTime dtnow()
	{
		return System.DateTime.Now.AddTicks(-dtime1_ * 10000);
	}

	public static ulong now()
	{
		return (ulong)(System.DateTime.Now.Ticks / 10000 - dtime_);
	}

	public static ulong native_now()
	{
		return (ulong)(System.DateTime.Now.Ticks / 10000);
	}

	public static System.DateTime time2dt(ulong time)
	{
		long tm = (long)(time + 28800000) * 10000;
		System.DateTime dt = System.DateTime.Parse ("1/1/1970").AddTicks(tm);
		return dt;
	}
	public static DateTime GetTime(string timeStamp)
	{
		DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		long lTime = long.Parse(timeStamp + "0000000");
		TimeSpan toNow = new TimeSpan(lTime);
		return dtStart.Add(toNow);
	}
	public static ulong chatChangeTime(ulong t)
	{
		long time = (System.DateTime.Now.Ticks - System.DateTime.Parse ("1/1/1970").Ticks) / 10000 - 28800000 ;
		return (ulong)time - (now()-t);
	}
	public static int last_time_today()
	{
		System.DateTime dt = System.DateTime.Parse(dtnow().ToShortDateString() + " 23:59:59");
		long tick = (dt.Ticks - dtnow ().Ticks) / 10000;
		tick = tick % 86400000;
		return (int)tick;
	}
	
	public static bool trigger_time(ulong old_time, int hour, int minute)
	{
		System.DateTime old_dt = time2dt (old_time);
		ulong new_time = now();
		System.DateTime new_dt = time2dt (new_time);
		
		if (new_time <= old_time)
		{
			return false;
		}
		
		if (new_time - old_time >= 86400000)
		{
			return true;
		}
		
		bool old_small = is_small(old_dt, hour, minute);
		bool new_small = is_small(new_dt, hour, minute);
		
		if (is_same_day(old_dt, new_dt))
		{
			if (old_small && !new_small)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (!old_small && !new_small)
			{
				return true;
			}
			else if (old_small && new_small)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

    public static bool trigger_qiyu_time(ulong time)
    {
        System.DateTime dt = System.DateTime.Parse(dtnow().Date.ToShortDateString() + " 06:00 :00");
        System.DateTime dt1 = System.DateTime.Parse(dtnow().Date.ToShortDateString() + " 12:00 :00");
        System.DateTime dt2 = System.DateTime.Parse(dtnow().Date.ToShortDateString() + " 18:00 :00");
        DateTime pre_zhen = time2dt(time);
        DateTime now = dtnow();
        if (pre_zhen <= dt && now >= dt || pre_zhen <= dt1 && now >= dt1 || pre_zhen <= dt2 && now >= dt2)
        {
            return true;
        }
        return false;
    }
    public static bool trigger_huiyi_shop_time(ulong time)
    {
        System.DateTime dt = System.DateTime.Parse(dtnow().Date.ToShortDateString() + " 09:00 :00");
        System.DateTime dt1 = System.DateTime.Parse(dtnow().Date.ToShortDateString() + " 12:00 :00");
        System.DateTime dt2 = System.DateTime.Parse(dtnow().Date.ToShortDateString() + " 18:00 :00");
        System.DateTime dt3 = System.DateTime.Parse(dtnow().Date.ToShortDateString() + " 21:00 :00");
        DateTime pre_zhen = time2dt(time);
        DateTime now = dtnow();
        if (pre_zhen <= dt && now >= dt || pre_zhen <= dt1 && now >= dt1 || pre_zhen <= dt2 && now >= dt2 || pre_zhen <= dt3 && now >= dt3)
        {
            return true;
        }
        return false;
 
    }
	public static bool trigger_week_time(ulong old_time)
	{
		System.DateTime old_dt = time2dt (old_time);
		ulong new_time = now();
		System.DateTime new_dt = time2dt (new_time);
		
		if (new_time <= old_time)
		{
			return false;
		}
		
		if (new_time - old_time >= 86400000 * 7)
		{
			return true;
		}
		
		int nw = (int)new_dt.DayOfWeek;
		if (nw == 0)
		{
			nw = 7;
		}
		int ow = (int)old_dt.DayOfWeek;
		if (ow == 0)
		{
			ow = 7;
		}
		
		if (nw < ow)
		{
			return true;
		}
		else if (nw == ow)
		{
			if (new_time - old_time >= 86400000 * 6)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public static bool trigger_month_time(ulong old_time)
	{
		System.DateTime old_dt = time2dt (old_time);
		ulong new_time = now();
		System.DateTime new_dt = time2dt (new_time);
		
		if (new_time <= old_time)
		{
			return false;
		}
		
		if (new_time - old_time >= 86400000L * 31)
		{
			return true;
		}
		
		int nw = new_dt.Month;
		int ow = old_dt.Month;
		
		if (nw != ow)
		{
			return true;
		}
		
		return false;
	}

	private static bool is_same_day(System.DateTime old_dt, System.DateTime new_dt)
	{
		if (old_dt.Year != new_dt.Year)
		{
			return false;
		}
		else if (old_dt.Month != new_dt.Month)
		{
			return false;
		}
		else if (old_dt.Day != new_dt.Day)
		{
			return false;
		}
		return true;
	}
	
	private static bool is_small(System.DateTime dt, int hour, int minute)
	{
		if (dt.Hour < hour)
		{
			return true;
		}
		else if (dt.Hour == hour)
		{
			if (dt.Minute < minute)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}

	public static int run_day(ulong old_time)
	{
		ulong now_time = now();
		if (old_time >= now_time)
		{
			return 0;
		}
		ulong delta_time = now_time - old_time;
		ulong day_num = delta_time / 86400000;
		ulong ltime = old_time + day_num * 86400000;
		if (trigger_time(ltime, 0, 0))
		{
			day_num++;
		}
		return (int)day_num;
	}
    public static long get_time_cuo(int hour,int day = 0)
    {
        System.DateTime date = new DateTime();
        if (hour == 24)
        {
            date = new System.DateTime(dtnow().Year, dtnow().Month, dtnow().Day, 0, 0, 0).AddDays(1) ;
        }
        else
        {
            date = new System.DateTime(dtnow().Year, dtnow().Month, dtnow().Day, hour, 0, 0);
        }
        date = date.AddDays(day);
        long time = 0;
        time = (long)(date.Ticks - System.DateTime.Parse("1/1/1970").Ticks) / 10000 - 28800000;
        return time;
 
    }
    public static string get_time_show_rob(long t)
    {
        if (t < 0)
        {
            t = 0;
        }
        int tt = (int)(t / 1000);
        //计算小时,用毫秒总数除以(1000*60*24),后去掉小数点
        int hour = tt / 3600;
        //计算分钟,用毫秒总数减去小时乘以(1000*60*24)后,除以(1000*60),再去掉小数点
        int min = tt % 3600 / 60;
        //同上
        int sec = tt % 60;
        //ulong msec = t - hour*(1000*60*24*60) - min*(1000*60) - sec*1000;
        //拼接字符串
        string _hour = hour.ToString();
        string _min = min.ToString();
        string _sec = sec.ToString();

        if (_hour.Length < 2)
        {
            _hour = "0" + _hour;
        }

        if (_min.Length < 2)
        {
            _min = "0" + _min;
        }

        if (_sec.Length < 2)
        {
            _sec = "0" + _sec;
        }

        string timeString = string.Format(game_data._instance.get_t_language ("timer.cs_301_42"),_hour , _min , _sec );//{0}小时{1}分钟{2}秒
        return timeString;
    }
	public static string get_time_show(long t)
	{
		if (t < 0)
		{
			t = 0;
		}
		int tt = (int)(t / 1000);
		//计算小时,用毫秒总数除以(1000*60*24),后去掉小数点
		int hour = tt / 3600;
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
		
		string timeString = _hour + ":" + _min +":"+ _sec;
		return timeString;
	}
	public static string get_time_show_mAnds(long t)
	{
		if (t < 0)
		{
			t = 0;
		}
		int tt = (int)(t / 1000);
		//计算小时,用毫秒总数除以(1000*60*24),后去掉小数点
		int hour = tt / 3600;
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
		
		string timeString = _min +":"+ _sec;
		return timeString;
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
		string timeString = string.Format(game_data._instance.get_t_language ("timer.cs_376_36"),day, _hour, _min , _sec );//{0}天{1}小时{2}分{3}秒
		return timeString;
	}
	public static string get_time_show_color_year(ulong t,ulong t2,string timercolor,string textcolor)
	{
		System.DateTime statrT = time2dt (t);
		System.DateTime endT = time2dt (t2);
		string strarYear = timercolor + statrT.Year.ToString () + textcolor;
		string strarMon = timercolor + statrT.Month.ToString () + textcolor;
		string strarDate = timercolor + statrT.Day.ToString () + textcolor;
		string strarHour = timercolor + statrT.Hour.ToString () + textcolor;
		
		string endYear = timercolor + endT.Year.ToString () + textcolor;
		string endMon = timercolor + endT.Month.ToString () + textcolor;
		string endDate = timercolor + endT.Day.ToString () + textcolor;
		string endHour = timercolor + statrT.Hour.ToString () + textcolor;
		
		string timerString = string.Format(game_data._instance.get_t_language ("timer.cs_429_37"),strarYear,strarMon,strarDate,strarHour,endYear,endMon,endDate,endHour);//{0}年{1}月{2}日{3}时-{4}年{5}月{6}日{7}时

		return timerString;
	}
	public static string get_time_show_color_ex(long t,string timercolor,string textcolor)
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

		string timeString = string.Format(game_data._instance.get_t_language ("timer.cs_376_36"),timercolor+day+textcolor, timercolor+_hour+textcolor, timercolor+_min+textcolor , timercolor+_sec+textcolor );//{0}天{1}小时{2}分{3}秒
		return timeString;
	}
	public static string get_show_time(long t)
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
			timeString =string.Format(game_data._instance.get_t_language ("arena.cs_535_41"),7);//{0}天前
		}
		else if(day >= 1 && day <=7)
		{
			timeString = string.Format(game_data._instance.get_t_language ("arena.cs_535_41"),day );//{0}天前
		}
		else if(hour > 0)
		{
			timeString = string.Format(game_data._instance.get_t_language ("arena.cs_530_41"),hour);//{0}小时前
		}
		else if(min > 0)
		{
			timeString = string.Format(game_data._instance.get_t_language ("arena.cs_525_41"),min);//{0}分钟前
		}
		else
		{
			timeString = game_data._instance.get_t_language ("arena.cs_520_27");//刚刚
		}
		
		return timeString;
	}
    public static string get_show(ulong t)
    {
        DateTime time = time2dt(t);
        DateTime now = dtnow();
        TimeSpan h = now - time;
        if (h.TotalDays > 1)
        {
            return string.Format(game_data._instance.get_t_language ("arena.cs_535_41"),(int)h.TotalDays );//{0}天前
        }
        else
        {
            if (h.TotalHours > 1)
            {
                return string.Format(game_data._instance.get_t_language ("arena.cs_530_41"),(int)h.TotalHours );//{0}小时前
            }
            else
            {
                if (h.TotalMinutes > 1)
                {
                    return string.Format(game_data._instance.get_t_language ("arena.cs_525_41"),(int)h.TotalMinutes );//{0}分钟前

                }
                else
                {
                    return string.Format(game_data._instance.get_t_language ("timer.cs_462_41"),(int)h.TotalSeconds);//{0}秒前
 
                }
               
            }
        }
    }
    public static long get_next_mondy()
    {
        int dayOfWeek = Convert.ToInt32(DateTime.Now.DayOfWeek) < 1 ? 7 : Convert.ToInt32(DateTime.Now.DayOfWeek);
        DateTime startWeek = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day).AddDays(1 - dayOfWeek);
        return datetimeconvertcuo(startWeek);
    }
    public static long datetimeconvertcuo(DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
        long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
        return t;
 
    }
	public static string show_sec(long t)
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
		string timeString = string.Format(game_data._instance.get_t_language ("timer.cs_sec"), _sec );//{0}秒
		return timeString;

	}
	public static string get_guild_show(ulong t)
	{
		DateTime time = time2dt(t);
		DateTime now = dtnow();
		TimeSpan h = now - time;
		if (h.TotalDays > 1)
		{
			return string.Format(game_data._instance.get_t_language ("timer.cs_492_24"), (int)h.TotalDays );//离线{0}天
		}
		else
		{
			if (h.TotalHours > 1)
			{
				return string.Format(game_data._instance.get_t_language ("timer.cs_498_25") , (int)h.TotalHours );//离线{0}小时
			}
			else
			{
				if (h.TotalMinutes > 1)
				{
					return string.Format(game_data._instance.get_t_language ("timer.cs_504_26") , (int)h.TotalMinutes);//离线{0}分钟
				}
			}
		}
		return game_data._instance.get_t_language ("timer.cs_508_9");//在线
	}
}
