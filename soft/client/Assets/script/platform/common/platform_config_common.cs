using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class platform_config_common
{
    // 版本号
    public static int m_high_ver = 1;
    // 服务器列表文件名
    public static string m_serverlist_file = "serverlist.xml";
    // 公告文件名
    public static string m_gonggao_file = "gonggao.txt";

    /// <summary>
    /// 此段信息写在oss的配置表里面
    /// </summary>
    
    // 是否审核模式
    // 审核模式下不显示人物和半身像，充值显示部分修改，游戏音乐部分关闭
    public static int m_shenhe = 0;
    // 是否使用test模式
    // 审核或者测试用test_mode
    // test_mode读的是serverlist_test,xml，否则正常读的是platform.xml里面的serverlist或者serverlist.xml
    // test_mode充值不记录后台
    public static int m_test = 1;
    // 系统服地址
    public static string m_sys_ip = "47.244.100.28";
    // 版本过低，打开的市场包(优先)
    public static string m_open_market = "";
    // 版本过低，打开的地址
    public static string m_open_url = "";
    // 配置文件中的额外附加信息，可以写任意
    public static string m_extra_info = "";
    // 公告文本
    public static string m_gonggao;
    // 是否维护状态
    public static int m_weihu = 0;
    // 维护状态的提示
    public static string m_weihu_text;

    /// <summary>
    /// 此段信息可在platform_config的init中修改
    /// </summary>

    // 平台,用于发给服务端
    public static string m_platform = "";
    public static string m_extra = "";
    // 是否使用第三方的登陆
    public static int m_login = 0;
    // 是否显示VIP
    public static int m_vip = 1;
    // 是否显示国旗
    public static int m_nationality = 0;
    // 是否打开礼包按钮
    public static int m_libao = 0;
    // 是否显示五星好评
    public static int m_five_star = 0;
    // 是否使用锁定充值
    // 登陆过的机器72小时不能在别的机器充值，防止第三方黑卡，目前对ios有效
    public static int m_kc = 0;
    public static string m_kc_code = "";
    public static string m_kc_code1 = "";
    public static ulong m_kc_time1 = 0;
    // 是否加载广告
    public static int m_ads = 0;
    // 是否显示版号
    public static int m_isbn = 0;
    // 是否限制分辨率
    public static int m_resolution = 1;
    // 显示半身像 0 为不显示登录场景 为start2 1 为显示 登录场景 为start。
    public static int m_half = 1;
    //版本模式 1 正常版本， 2超V
    public static int game_model = 1;
    public static void init()
    {
        if (m_test == 1)
        {
            m_serverlist_file = "serverlist_test.xml";
        }
        else
        {
            m_serverlist_file = "serverlist.xml";
        }

        if (Application.isEditor)
        {
            platform_config.m_common_url = "http://xzn2.en.oss.yymoon.com/yymoon_new/test/";
        }
    }

    public static string get_url_end()
    {
        return "?t=" + Random.Range(0, 100000);
    }

    public static string get_account_url()
    {
        return "http://" + m_sys_ip + ":10001/";
    }

    public static string get_pay_url()
    {
        return "http://" + m_sys_ip + ":10002/";
    }

    public static string get_libao_url()
    {
        return "http://" + m_sys_ip + ":10003/";
    }

    public static string get_storage_url()
    {
        return "http://" + m_sys_ip + ":10004/";
    }

    public static string get_pt2login_url()
    {
        return "http://" + m_sys_ip + ":10005/";
    }

    public static void read_weihu(string text)
    {
        XDocument _doc = new XDocument();
        StringReader reader = new StringReader(text);
        _doc = XDocument.Load(reader);
        XElement _users = _doc.Element("weihu");
        if (_users.Attribute("on").Value == "1")
        {
            m_weihu = 1;
        }
        else
        {
            m_weihu = 0;
        }
        m_weihu_text = _users.Attribute("text").Value;
        IEnumerable<XElement> nodeList = _users.Elements("user");
        game_data._instance.m_users.Clear();
        foreach (XElement xe in nodeList)
        {
            game_data._instance.m_users.Add(xe.Attribute("id").Value);
        }
        IEnumerable<XElement> nodeList_platform = _users.Elements("platform");
        foreach (XElement xe in nodeList_platform)
        {
            if (xe.Attribute("channel").Value == platform._instance.get_platform_id())
            {
                if (xe.Attribute("on").Value == "1")
                {
                    m_weihu = 1;
                }
                else
                {
                    m_weihu = 0;
                }
            }
        }
    }
}
