
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class platform_config
{
    // ≈‰÷√±Ìµÿ÷∑
    public static string m_common_url = "http://xzn2.en.oss.yymoon.com/yymoon/ios/ios_international/";

    public static void init()
    {
        platform_config_common.m_platform = "ios";
        platform_config_common.m_login = 0;
        platform_config_common.m_isbn = 1;
    }
}
