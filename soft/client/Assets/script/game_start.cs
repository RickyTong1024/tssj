
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class game_start : MonoBehaviour {

    public static game_start _instance;
    public GameObject m_node;
    void Awake()
    {
        _instance = this;
        init();
    }

    void Start () {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(m_node.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLoad");
    }

    public void init()
    {
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            objs.Add(this.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < objs.Count; i++)
        {
            Object.Destroy(objs[i]);
        }
        GameObject node = Instantiate(m_node);
        node.transform.parent = this.transform;
        node.name = "game_node";
        node.AddComponent<cmessage_center>();
        node.AddComponent<icon_manager>();
        node.AddComponent<game_data>();
        game_data._instance.load_txt();
        node.AddComponent<platform_object>();
        node.AddComponent<platform_recharge_object>();
#if APPSFLYER
        node.AddComponent<Appsflyer_management>();
        analytics_management.analytics_config();
#endif
        platform_config_common.init();
    }
}
