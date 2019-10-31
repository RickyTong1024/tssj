using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class platform_config_common
{
    // �汾��
    public static int m_high_ver = 1;
    // �������б��ļ���
    public static string m_serverlist_file = "serverlist.xml";
    // �����ļ���
    public static string m_gonggao_file = "gonggao.txt";

    /// <summary>
    /// �˶���Ϣд��oss�����ñ�����
    /// </summary>
    
    // �Ƿ����ģʽ
    // ���ģʽ�²���ʾ����Ͱ����񣬳�ֵ��ʾ�����޸ģ���Ϸ���ֲ��ֹر�
    public static int m_shenhe = 0;
    // �Ƿ�ʹ��testģʽ
    // ��˻��߲�����test_mode
    // test_mode������serverlist_test,xml����������������platform.xml�����serverlist����serverlist.xml
    // test_mode��ֵ����¼��̨
    public static int m_test = 1;
    // ϵͳ����ַ
    public static string m_sys_ip = "47.244.100.28";
    // �汾���ͣ��򿪵��г���(����)
    public static string m_open_market = "";
    // �汾���ͣ��򿪵ĵ�ַ
    public static string m_open_url = "";
    // �����ļ��еĶ��⸽����Ϣ������д����
    public static string m_extra_info = "";
    // �����ı�
    public static string m_gonggao;
    // �Ƿ�ά��״̬
    public static int m_weihu = 0;
    // ά��״̬����ʾ
    public static string m_weihu_text;

    /// <summary>
    /// �˶���Ϣ����platform_config��init���޸�
    /// </summary>

    // ƽ̨,���ڷ��������
    public static string m_platform = "";
    public static string m_extra = "";
    // �Ƿ�ʹ�õ������ĵ�½
    public static int m_login = 0;
    // �Ƿ���ʾVIP
    public static int m_vip = 1;
    // �Ƿ���ʾ����
    public static int m_nationality = 0;
    // �Ƿ�������ť
    public static int m_libao = 0;
    // �Ƿ���ʾ���Ǻ���
    public static int m_five_star = 0;
    // �Ƿ�ʹ��������ֵ
    // ��½���Ļ���72Сʱ�����ڱ�Ļ�����ֵ����ֹ�������ڿ���Ŀǰ��ios��Ч
    public static int m_kc = 0;
    public static string m_kc_code = "";
    public static string m_kc_code1 = "";
    public static ulong m_kc_time1 = 0;
    // �Ƿ���ع��
    public static int m_ads = 0;
    // �Ƿ���ʾ���
    public static int m_isbn = 0;
    // �Ƿ����Ʒֱ���
    public static int m_resolution = 1;
    // ��ʾ������ 0 Ϊ����ʾ��¼���� Ϊstart2 1 Ϊ��ʾ ��¼���� Ϊstart��
    public static int m_half = 1;
    //�汾ģʽ 1 �����汾�� 2��V
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
