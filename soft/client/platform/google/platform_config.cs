
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class platform_config
{
    // ≈‰÷√±Ìµÿ÷∑
    public static string m_common_url = "http://xzn2.en.oss.yymoon.com/yymoon_new/android/google_Calling/";

    public static void init()
    {
        platform_config_common.m_platform = "google";
        platform_config_common.m_vip = 0;
        platform_config_common.m_five_star = 1;
        platform_config_common.m_ads = 1;
        platform_config_common.m_half = 0;
    }
}
