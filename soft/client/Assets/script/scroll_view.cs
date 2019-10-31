
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scroll_view : MonoBehaviour {

	public GameObject m_view;
	public GameObject m_grid;
	public int m_max_page = 0;

	List<GameObject> m_pages = new List<GameObject>();
	int m_cur_page = 0;
	// Use this for initialization
	void Start () {
		m_view.GetComponent<UIScrollView>().onDragFinished = OnDragFinished;
		update_ui ();
	}
	
	void OnDestroy()
	{
	}
	
	void OnFinished()
	{
		Debug.Log (game_data._instance.get_t_language ("scroll_view.cs_25_13"));//移动结束
	}
	
	public void update_ui()
	{
		if(sys._instance.m_self == null)
		{
			return ;
		}
		sys._instance.remove_child (m_grid);		
		m_pages.Clear ();
		
		for(int i = 0;i < m_max_page;i ++)
		{
			GameObject _page = game_data._instance.ins_object_res("ui/page");
			
			_page.transform.name = "page_" + i;
			_page.transform.parent = m_grid.transform;
			_page.transform.localPosition = new Vector3(m_grid.GetComponent<UIGrid>().cellWidth * i,0,0);
			_page.transform.localScale = new Vector3(1,1,1);
			
			m_pages.Add(_page);
		}
		
		OnDragFinished ();
	}
	public Transform get_card_pos(int id)
	{
		int _page = id / 10;
		int _id = id % 10;
		
		GameObject _object = m_pages[_page];
		
		return _object.transform.Find("card_" + _id);
	}
	void OnDragFinished () {
		
		float _max_dis = 1000000;
		for(int i = 0;i < m_pages.Count;i ++)
		{
			float _x = m_pages[i].transform.localPosition.x + m_view.transform.localPosition.x;
			
			if(Mathf.Abs(_x) < _max_dis)
			{
				_max_dis = Mathf.Abs(_x);
				m_cur_page = i;
			}
		}

	}

	public void set_page(int page)
	{
		m_cur_page = page;

		map_move ();
	}

	public int get_page()
	{
		return m_cur_page;
	}

	void map_move()
	{
		if (m_cur_page < 0)
		{
			m_cur_page = 0;
		}
		
		if(m_cur_page > m_grid.transform.childCount)
		{
			m_cur_page = m_grid.transform.childCount;
		}
		
		SpringPanel.Begin(m_view.GetComponent<UIScrollView>().panel.cachedGameObject,
		                  new Vector3(- m_cur_page * m_grid.GetComponent<UIGrid>().cellWidth,0), 8.0f).onFinished = OnFinished;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
