
using UnityEngine;
using System.Collections;

public class frame : MonoBehaviour {

	private int m_width;
	public int m_height;

	int m_min_width = 450;
	int m_min_height = 90;

	private bool m_show = false;
	private bool m_hide = false;

	private bool m_active;

	public GameObject m_show_root;
	public GameObject m_hide_root;

	public GameObject m_root;
    public delegate void VoidDelegate();
    public VoidDelegate handle;
	public void on_show()
	{
		if(m_show_root != null)
		{
			m_show_root.SetActive(true);
            if (handle != null)
            {
                handle();
                handle = null;
            }
		}
	}

	public void on_hide()
	{
		if(m_hide_root != null)
		{
			m_hide_root.SetActive(false);
		}
	}

	void OnDisable()
	{
		CancelInvoke ("on_hide");
		CancelInvoke ("on_show");
		m_hide = false;

		reast();
	}

	// Use this for initialization
	void Start () {
	
		if(m_root == null)
		{
			m_root = this.gameObject;
		}

		if(m_root.transform.Find("close_back") != null)
		{
			Transform _sub = m_root.transform.Find("close_back").Find("close");
			
			Vector3 _size = new Vector3 ();
			
			_size.x = 60;
			_size.y = 60;
			_size.z = 0;

			//_sub.GetComponent<UISprite>().autoResizeBoxCollider = false;
			_sub.GetComponent<BoxCollider>().size = _size;
		}

		if(m_root.transform.Find("Title") == null)
		{
            if (m_root.GetComponent<UISprite>().width > 337)
            {
                m_min_width = 337;
            }
            else
            {
                m_min_width = 226;
 
            }
			//337 226
			m_min_height = 91;
		}

		m_width = m_root.GetComponent<UISprite>().width;
		m_height = m_root.GetComponent<UISprite>().height;
		reast ();
	}

	public void OnEnable()
	{
		m_show = true;
	}

	public void hide()
	{
		m_hide = true;
	}
	void click(GameObject obj)
	{
		if(obj.transform.name =="close")
		{
			m_hide = true;
		}
	}

	void reast()
	{
		m_root.GetComponent<UISprite>().width = m_min_width;
		m_root.GetComponent<UISprite>().height = m_min_height;
		m_root.GetComponent<UISprite>().alpha = 0.0f;

		if(m_show_root != null)
		{
			m_show_root.SetActive(false);
		}
	}
	// Update is called once per frame
	void Update () {
	
		if(m_show)
		{
			m_show = false;

			reast();

			sys._instance.add_alpha_anim(m_root.gameObject,0.15f,0,1,0);
			sys._instance.add_width_anim (m_root.GetComponent<UIWidget>(), 0.15f , m_min_width,m_width, 0);
			//sys._instance.add_height_anim (m_root.GetComponent<UIWidget>(), 0.15f , m_min_height,m_height, 0.2f);

			root_gui._instance.do_mask (0.6f);

			//this.Invoke("on_show",0.35f);
			
			TweenHeight _th = TweenHeight.Begin (m_root.GetComponent<UIWidget>(), 0.15f, m_height);
			
			_th.method = UITweener.Method.EaseInOut;
			_th.from = m_min_height;
			_th.to = m_height;
			_th.delay = 0.2f;

			EventDelegate.Add(_th.onFinished, delegate() 
			                  {
				on_show();
			},true);


			sys._instance.play_sound ("sound/ui_open");
		}

		if(m_hide)
		{
			m_hide = false;

			if(m_show_root != null)
			{
				m_show_root.SetActive(false);
			}
			sys._instance.add_height_anim (m_root.GetComponent<UIWidget>(), 0.15f , m_height,m_min_height, 0.05f);
			sys._instance.add_width_anim (m_root.GetComponent<UIWidget>(), 0.15f , m_width,m_min_width, 0.2f);
			TweenAlpha _alpha = sys._instance.add_alpha_anim(m_root.gameObject,0.15f,1,0,0.30f);
		
			root_gui._instance.do_mask (0.6f);
			EventDelegate.Add(_alpha.onFinished, delegate() 
			                  {
				on_hide();
			},true);
			//this.Invoke("on_hide",0.45f);

			sys._instance.play_sound ("sound/ui_close");
		}
	}
}
