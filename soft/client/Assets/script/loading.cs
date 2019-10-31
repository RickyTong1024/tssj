
using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

public class loading: MonoBehaviour,IMessage {
 
    private float m_progress;
    //一组动画的贴图，在编辑器中赋值。
	public GameObject m_texture;
    private int nowFram;
	public UILabel shuoming;
	public GameObject m_pro;
	public GameObject m_gd;
	public bool m_add = false;
	public GameObject m_anim;
	private int m_index = 0;
	public List<Texture> m_textures = new List<Texture>();
    //异步对象
    AsyncOperation m_async;
    void Start()
    {
		cmessage_center._instance.add_handle (this);
    }
	void IMessage.net_message(s_net_message message)
	{

	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "hide_loading")
		{
			root_gui._instance.hide_loading_gui();
		}
	}

	public void OnEnable()
	{
		sys._instance.m_is_loading = true;
		StartCoroutine(loadScene());

		m_pro.GetComponent<UIProgressBar>().value = 0.0f;
		int _id = Random.Range (0, m_textures.Count);
		m_texture.transform.GetComponent<UITexture>().mainTexture = m_textures[_id];
		m_progress = 0;
		shuoming.transform.GetComponent<UILabel>().text = sys._instance.m_load_text;	

		m_anim.GetComponent<Animator>().StopPlayback ();
		m_anim.GetComponent<Animator>().Play ("loading");

		m_index ++;
	}
	
    //注意这里返回值一定是 IEnumerator
    IEnumerator loadScene()
    {
        if (m_add == false)
		{
			m_async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sys._instance.m_load_name);
		}
		else
		{
			m_async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sys._instance.m_load_name, UnityEngine.SceneManagement.LoadSceneMode.Additive);
		}

        yield return m_async;
    }
 
    void Update()
    {
        if (m_async != null)
        {
            float jd = m_async.progress;
            m_progress += Time.deltaTime;
            if (m_progress > jd)
            {
                m_progress = jd;
            }
            m_pro.GetComponent<UIProgressBar>().value = m_progress;
            m_gd.transform.GetComponent<UITexture>().width = (int)(640 * m_progress);
            Rect r = m_gd.transform.GetComponent<UITexture>().uvRect;
            r.x -= Time.deltaTime / 2;
            r.width = m_progress;
            m_gd.transform.GetComponent<UITexture>().uvRect = r;
            if (m_async.isDone && sys._instance.m_is_loading == true && m_progress >= 1.0f)
            {
                root_gui._instance.show_mask();
                sys._instance.load_end();

                this.StopAllCoroutines();

                s_message _msg = new s_message();

                _msg.m_type = "hide_loading";
                _msg.time = 1.0f;

                cmessage_center._instance.add_message(_msg);
            }
 
        }
		
    }
}
