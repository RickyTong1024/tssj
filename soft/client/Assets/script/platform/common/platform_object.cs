using UnityEngine;
using System.Collections;

public class platform_object : MonoBehaviour
{
    public static platform_object _instance;
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        platform._instance.init(this.gameObject);
    }

    public void save_photo(string message)
    {
        System.DateTime now = System.DateTime.Now;
        string name = string.Format("image{0}{1}{2}{3}{4}{5}.png", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        StartCoroutine(do_save_photo(name, message));
    }

    IEnumerator do_save_photo(string name, string message)
    {
        ScreenCapture.CaptureScreenshot(name);
        yield return new WaitForSeconds(platform._instance.m_photo_time);

        platform._instance.do_save_photo_platform(name, message);
    }

    void platform_login_success(string s)
    {
        root_gui._instance.wait(false);
        platform._instance.platform_login_success(s);
    }

    void platform_login_fail(string s)
    {
        root_gui._instance.wait(false);
    }

    void platform_logout(string s)
    {
        platform._instance.platfrom_logout_success(s);
    }
}
