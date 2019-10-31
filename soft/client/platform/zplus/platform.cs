using UnityEngine;

public class platform : platform_common
{
    private static platform _platform;

    public static platform _instance
    {
        get
        {
            if (_platform == null)
            {
                _platform = new platform();
            }
            return _platform;
        }
    }

    public override void init(GameObject obj)
    {
        base.init(obj);
        ZpGameManage zgm = obj.AddComponent<ZpGameManage>();
        zgm._Debug = true;
    }

    public override void scene_loaded()
    {
        Camera cam = sys._instance.get_cam();
        if (cam)
        {
            string name = sys._instance.m_load_name;
            deal_cam(name, cam);
        }
    }

    public override void deal_cam(string name, Camera cam)
    {
        ZpGameStereoCamera zc = cam.gameObject.GetComponent<ZpGameStereoCamera>();
        if (zc == null)
        {
            zc = cam.gameObject.AddComponent<ZpGameStereoCamera>();
        }
        zc._SPUid = name;
        zc._Debug = true;
        zc.SwitchStereoPara();
    }
}
