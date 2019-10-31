
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class platform_config
{
    // ≈‰÷√±Ìµÿ÷∑
    public static string m_common_url = "http://xzn2.en.oss.yymoon.com/yymoon_new/test/";

    public static void init()
    {
        platform_config_common.m_platform = "official_BT";
        platform_config_common.m_login = 1;
        platform_config_common.m_isbn = 1;
        platform_config_common.game_model = 2;
    }
}
