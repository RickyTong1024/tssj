
using UnityEngine;
using System.Collections;

public class ui_title_anim : MonoBehaviour {

	bool m_start;
	int m_width;
	int m_height;
	bool m_active = false;
	float m_time;
	int m_state;
	GameObject title ;
	GameObject back;
	GameObject kuang ;
	GameObject top1 ;
	GameObject bottom1 ;
	GameObject top2 ;
	GameObject bottom2 ;
	// Use this for initialization
	void Start () {
	
	}

	public void OnEnable()
	{
		if (!m_start)
		{
			m_start = true;
			title = this.transform.Find("title").gameObject;
			back = this.transform.Find("back").gameObject;
			kuang = title.transform.Find("kuang").gameObject;
			top1 = title.transform.Find("top1").gameObject;
			bottom1 = title.transform.Find("bottom1").gameObject;
			top2 = title.transform.Find("top2").gameObject;
			bottom2 = title.transform.Find("bottom2").gameObject;
			m_width = (top1.GetComponent<UISprite>().width)*2;
			m_height =(int)-bottom1.transform.localPosition.y;
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
		title = this.transform.Find("title").gameObject;
		back = this.transform.Find("back").gameObject;
		kuang = title.transform.Find("kuang").gameObject;
		top1 = title.transform.Find("top1").gameObject;
		bottom1 = title.transform.Find("bottom1").gameObject;
		top2 = title.transform.Find("top2").gameObject;
		bottom2 = title.transform.Find("bottom2").gameObject;
		title.GetComponent<UISprite>().alpha = 0;
		back.GetComponent<UIPanel>().alpha = 0;
		kuang.GetComponent<UISprite>().alpha = 0;
		top1.GetComponent<UISprite>().alpha = 0;
		bottom1.GetComponent<UISprite>().alpha = 0;
		top2.GetComponent<UISprite>().alpha = 0;
		bottom2.GetComponent<UISprite>().alpha = 0;

		sys._instance.play_sound ("sound/ui_open");

		root_gui._instance.do_mask (0.7f);
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

		root_gui._instance.do_mask (0.6f);
	}
	
	// Update is called once per frame
	void Update () {

		m_time += Time.deltaTime;
		if (m_active)
		{
			if (m_state == 0)
			{
				sys._instance.add_alpha_anim (title, 0.1f, 0, 1, 0);
				sys._instance.add_width_anim (title.GetComponent<UISprite>(), 0.15f, 244, 416, 0);
				m_state = 1;
				m_time = 0;
			}
			if (m_state == 1 && m_time > 0.15f)
			{
				sys._instance.add_width_anim (title.GetComponent<UISprite>(), 0.05f, 416, 400, 0);
				m_state = 2;
				m_time = 0;
			}
			if (m_state == 2 && m_time > 0.05f)
			{
				sys._instance.add_width_anim (title.GetComponent<UISprite>(), 0.05f, 400, 416, 0);
				m_state = 3;
				m_time = 0;
			}
			if (m_state == 3 && m_time > 0.05f)
			{
				int cwd = 242;
				int ccwd = cwd * 2;
				sys._instance.add_alpha_anim (top1, 0.1f, 0, 1, 0);
				sys._instance.add_alpha_anim (bottom1, 0.1f, 0, 1, 0);
				sys._instance.add_alpha_anim (top2, 0.1f, 0, 1, 0);
				sys._instance.add_alpha_anim (bottom2, 0.1f, 0, 1, 0);
				sys._instance.add_alpha_anim (kuang, 0.1f, 0, 1, 0);
				top1.transform.localPosition = new Vector3 (m_width / 2, -35, 0);
				top2.transform.localPosition = new Vector3 (-m_width / 2, -35, 0);
				sys._instance.add_pos_anim (top1, 0.15f, new Vector3(cwd - m_width / 2, 0, 0), 0);
				sys._instance.add_pos_anim (top2, 0.15f, new Vector3(-cwd + m_width / 2, 0, 0), 0);
				sys._instance.add_width_anim (top1.GetComponent<UISprite>(), 0.15f, cwd, m_width / 2, 0);
				sys._instance.add_width_anim (top2.GetComponent<UISprite>(), 0.15f, cwd, m_width / 2, 0);
				bottom1.transform.localPosition = new Vector3 (m_width / 2 - 45, -75, 0);
				bottom2.transform.localPosition = new Vector3 (-m_width / 2 + 45, -75, 0);
				sys._instance.add_pos_anim (bottom1, 0.15f, new Vector3(cwd - m_width / 2, 0, 0), 0);
				sys._instance.add_pos_anim (bottom2, 0.15f, new Vector3(-cwd + m_width / 2, 0, 0), 0);
				kuang.GetComponent<UISprite>().height = -105;
				sys._instance.add_width_anim (kuang.GetComponent<UISprite>(), 0.15f, ccwd - 58, m_width - 58, 0);
				m_state = 4;
				m_time = 0;
			}
			if (m_state == 4 && m_time > 0.15f)
			{
				bottom1.transform.localPosition = new Vector3 (m_width / 2 - 45, -m_height, 0);
				bottom2.transform.localPosition = new Vector3 (-m_width / 2 + 45, -m_height, 0);
				sys._instance.add_pos_anim (bottom1, 0.15f, new Vector3(0, m_height - 75, 0), 0);
				sys._instance.add_pos_anim (bottom2, 0.15f, new Vector3(0, m_height - 75, 0), 0);
				sys._instance.add_height_anim (kuang.GetComponent<UISprite>(), 0.15f, 105, m_height + 30, 0);
				m_state = 5;
				m_time = 0;
			}
			if (m_state == 5 && m_time > 0.15f)
			{
				sys._instance.add_alpha_anim (back, 0.1f, 0, 1, 0);
				m_state = 6;
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
			if (m_state == 1 && m_time > 0.1f)
			{
				bottom1.transform.localPosition = new Vector3 (m_width / 2 - 45, -75, 0);
				bottom2.transform.localPosition = new Vector3 (-m_width / 2 + 45, -75, 0);
				sys._instance.add_pos_anim (bottom1, 0.15f, new Vector3(0, -m_height + 75, 0), 0);
				sys._instance.add_pos_anim (bottom2, 0.15f, new Vector3(0, -m_height + 75, 0), 0);
				sys._instance.add_height_anim (kuang.GetComponent<UISprite>(), 0.15f, m_height + 30, 105, 0);
				m_state = 2;
				m_time = 0;
			}
			if (m_state == 2 && m_time > 0.15f)
			{
				int cwd = 242;
				int ccwd = cwd * 2;
				sys._instance.add_alpha_anim (top1, 0.15f, 1, 0, 0);
				sys._instance.add_alpha_anim (bottom1, 0.15f, 1, 0, 0);
				sys._instance.add_alpha_anim (top2, 0.15f, 1, 0, 0);
				sys._instance.add_alpha_anim (bottom2, 0.15f, 1, 0, 0);
				sys._instance.add_alpha_anim (kuang, 0.15f, 1, 0, 0);
				top1.transform.localPosition = new Vector3 (cwd, -35, 0);
				top2.transform.localPosition = new Vector3 (-cwd, -35, 0);
				sys._instance.add_pos_anim (top1, 0.15f, new Vector3(-cwd + m_width / 2, 0, 0), 0);
				sys._instance.add_pos_anim (top2, 0.15f, new Vector3(cwd - m_width / 2, 0, 0), 0);
				sys._instance.add_width_anim (top1.GetComponent<UISprite>(), 0.15f, m_width / 2, cwd, 0);
				sys._instance.add_width_anim (top2.GetComponent<UISprite>(), 0.15f, m_width / 2, cwd, 0);
				bottom1.transform.localPosition = new Vector3 (cwd - 45, -75, 0);
				bottom2.transform.localPosition = new Vector3 (-cwd + 45, -75, 0);
				sys._instance.add_pos_anim (bottom1, 0.15f, new Vector3(-cwd + m_width / 2, 0, 0), 0);
				sys._instance.add_pos_anim (bottom2, 0.15f, new Vector3(cwd - m_width / 2, 0, 0), 0);
				sys._instance.add_width_anim (kuang.GetComponent<UISprite>(), 0.15f, m_width - 58, ccwd - 58, 0);
				m_state = 3;
				m_time = 0;
			}
			if (m_state == 3 && m_time > 0.15f)
			{
				TweenAlpha ta = sys._instance.add_alpha_anim (title, 0.15f, 1, 0, 0);
				sys._instance.add_width_anim (title.GetComponent<UISprite>(), 0.15f, 416, 244, 0);
				EventDelegate.Add (ta.onFinished, onFinished);
				m_state = 5;
				m_time = 0;
			}
		}
	}
}
