
using UnityEngine;
using System.Collections;

public class ui_bloom_anim : MonoBehaviour {
	
	bool m_start;
	int m_width;
	int m_height;
	int m_height1;
	bool m_active = false;
	float m_time;
	int m_state;
	GameObject title;
	GameObject kuang;
	GameObject bottom;
	GameObject back;
	// Use this for initialization
	void Start () {
		
	}
	
	public void OnEnable()
	{
		if (!m_start)
		{
			m_start = true;
			title = this.transform.Find("top").gameObject;
			bottom= title.transform.Find("bottom").gameObject;
			kuang= title.transform.Find("kuang").gameObject;
			back = this.transform.Find("back").gameObject;
			m_width = title.GetComponent<UISprite>().width;
			m_height =(int)-bottom.transform.localPosition.y;
			m_height1 = (int)title.transform.localPosition.y;
		}
		show_ui ();
	}
	
	public void onFinished()
	{
		this.gameObject.SetActive(m_active);
	}
	
	public void show_ui()
	{
		if (m_active)
		{
			return;
		}
		m_active = true;
		m_time = 0;
		m_state = 0;
		title = this.transform.Find("top").gameObject;
		bottom= title.transform.Find("bottom").gameObject;
		kuang= title.transform.Find("kuang").gameObject;
		back = this.transform.Find("back").gameObject;
		title.GetComponent<UISprite>().alpha = 0;

		if(back.GetComponent<UISprite>() != null)
		{
			back.GetComponent<UISprite>().alpha = 0;
		}
		else
		{
			back.GetComponent<UIWidget>().alpha = 0;
		}

		kuang.GetComponent<UISprite>().alpha = 0;
		bottom.GetComponent<UISprite>().alpha = 0;
		sys._instance.play_sound ("sound/ui_open");

		root_gui._instance.do_mask (0.6f);
	}
	
	public void hide_ui()
	{
		if (!m_active)
		{
			return;
		}
		m_active = false;
		m_time = 0;
		m_state = 0;
		
		sys._instance.play_sound ("sound/ui_close");

		root_gui._instance.do_mask (0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
		m_time += Time.deltaTime;
		if (m_active)
		{
			
			if (m_state == 0 && m_time > 0.15f)
			{
				sys._instance.add_alpha_anim (title, 0.1f, 0, 1, 0);
				sys._instance.add_alpha_anim (bottom, 0.1f, 0, 1, 0);
				sys._instance.add_alpha_anim (kuang, 0.1f, 0, 1, 0);
			
				title.transform.localPosition = new Vector3 ((int)title.transform.localPosition.x , 0, 0);
				sys._instance.add_width_anim (title.GetComponent<UISprite>(), 0.15f , 280 , m_width, 0);
				bottom.transform.localPosition = new Vector3 ((int)bottom.transform.localPosition.x, -50, 0);
				sys._instance.add_width_anim (bottom.GetComponent<UISprite>(), 0.15f , 280 , m_width-25 , 0);
				sys._instance.add_width_anim (kuang.GetComponent<UISprite>(), 0.15f , 280 , m_width-40 , 0);
				kuang.GetComponent<UISprite>().height = 82;
				kuang.transform.localPosition = new Vector3 (0, -20, 0);
				m_state = 1;
				m_time = 0;
			}
			if (m_state == 1 && m_time > 0.15f)
			{
				kuang.transform.localPosition = new Vector3 ((int)kuang.transform.localPosition.x, -m_height / 2 + 5, 0);
				sys._instance.add_pos_anim (kuang, 0.15f, new Vector3(0, m_height / 2 - 25 , 0), 0);
				sys._instance.add_height_anim (kuang.GetComponent<UISprite>(), 0.15f , 82, m_height + 32, 0);
				bottom.transform.localPosition = new Vector3 ((int)bottom.transform.localPosition.x, -m_height, 0);
				sys._instance.add_pos_anim (bottom, 0.15f, new Vector3(0, m_height - 50 , 0), 0);
				title.transform.localPosition = new Vector3 ((int)title.transform.localPosition.x, m_height1, 0);
				sys._instance.add_pos_anim (title, 0.15f, new Vector3(0, -m_height1, 0), 0);

				m_state = 2;
				m_time = 0;
			}
			
			if (m_state ==2 && m_time > 0.15f)
			{
				sys._instance.add_alpha_anim (back, 0.1f, 0, 1, 0);
				m_state =3;
				m_time = 0;
			}
		}
		else
		{
			if (m_state == 0)
			{
				sys._instance.add_alpha_anim (back, 0.1f, 1, 0, 0);
				m_state = 1;
				m_time = 0;
			}
			if (m_state == 1 && m_time > 0.15f)
			{
				kuang.transform.localPosition = new Vector3 ((int)kuang.transform.localPosition.x, -20, 0);
				sys._instance.add_pos_anim (kuang, 0.15f, new Vector3(0, -m_height / 2 + 25 , 0), 0);
				sys._instance.add_height_anim (kuang.GetComponent<UISprite>(), 0.15f, m_height + 32, 82, 0);
				bottom.transform.localPosition = new Vector3 (0, -50, 0);
				sys._instance.add_pos_anim (bottom, 0.15f, new Vector3(0, -m_height + 50 , 0), 0);
				title.transform.localPosition = new Vector3 ((int)title.transform.localPosition.x, 0, 0);
				sys._instance.add_pos_anim (title, 0.15f, new Vector3(0, m_height1 , 0), 0);
				m_state = 2;
				m_time = 0;
			}
			if (m_state == 2 && m_time > 0.15f )
			{

				sys._instance.add_alpha_anim (title, 0.1f, 0, 1, 0);
				sys._instance.add_alpha_anim (bottom, 0.1f, 0, 1, 0);
				sys._instance.add_alpha_anim (kuang, 0.1f, 0, 1, 0);

				sys._instance.add_width_anim (title.GetComponent<UISprite>(), 0.15f , m_width,280, 0);
				sys._instance.add_width_anim (bottom.GetComponent<UISprite>(), 0.15f ,  m_width-25 ,280, 0);
				sys._instance.add_width_anim (kuang.GetComponent<UISprite>(), 0.15f ,  m_width-40 ,280, 0);

					
				m_state = 3;
				m_time = 0;
			}
			if (m_state == 3 && m_time > 0.05f)
			{
				TweenAlpha ta = sys._instance.add_alpha_anim (title, 0.15f, 1, 0, 0);
				EventDelegate.Add (ta.onFinished, onFinished);
				m_state = 4;
				m_time = 0;
			}
		}
	}
}


