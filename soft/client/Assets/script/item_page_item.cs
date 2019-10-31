
using UnityEngine;
using System.Collections;

public class item_page_item : MonoBehaviour {
	public GameObject m_icon_root;
	public GameObject m_name;
	public GameObject m_pro;
	public GameObject m_zhanli;
	private GameObject m_icon;
	public GameObject m_select;
	public string m_out_message;
	public int m_page;
	private float m_rot;
	private s_t_item m_item;

	// Use this for initialization
	void Start () {
		
		m_rot = Random.Range (0f, 180f);
	}

	
	public void click(GameObject obj)
	{
		if (m_item != null)
		{
			int index = int.Parse(obj.name)*7+m_page;
			s_message _msg = new s_message();
			
			_msg.m_type = m_out_message;
			_msg.m_ints.Add(m_item.id);
			_msg.m_object.Add (m_icon);
			_msg.m_object.Add (this.gameObject);
			
			cmessage_center._instance.add_message (_msg);
			
			if(m_out_message == "common_select_item")
			{
				bool flag = false;
				for(int i = 0; i < sys._instance.m_select_items.Count;++i)
				{
					if(sys._instance.m_select_items[i] == 0)
					{
						flag = true;
						break;
					}
				}
				if(!flag && !m_select.activeSelf)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("item_page_item.cs_50_47"));//[ffc882]选择粮食数量不能超过5个
					return;
				}
				m_select.SetActive(!m_select.activeSelf);
				sys._instance.select_items(m_item.id,!m_select.activeSelf);
				sys._instance.select_item_indexs(index);
			}
		}
	}
	
	void remove_icon(int dir)
	{
		if(m_icon != null)
		{
			if (dir == 0)
			{
				GameObject.Destroy(m_icon);
				return;
			}
			TweenAlpha.Begin (m_icon, 0.5f, 0f);
			TweenScale.Begin (m_icon, 0.5f, new Vector3(0.2f,0.2f,0.2f));
			
			TweenAlpha _alpha = m_icon.GetComponent<TweenAlpha>();
			
			EventDelegate.Add(_alpha.onFinished, delegate() 
			                  {
				GameObject.Destroy(_alpha.gameObject);
			});
			
			TweenPosition _effect = TweenPosition.Begin(m_icon,0.5f,new Vector3(-100f * dir,0,0));
			_effect.method = UITweener.Method.EaseInOut;
		}
	}

	public void set_item(int guid,int dir)
	{
		GetComponent<BoxCollider>().enabled = true;
		m_item = null;
		m_pro.GetComponent<UISprite>().spriteName = "";
		m_name.GetComponent<UILabel>().text = "";
		m_zhanli.GetComponent<UILabel>().text = "";
		m_select.SetActive (false);
		remove_icon (dir);
		
		if(guid == 0)
		{
			return;
		}
		
		m_item = game_data._instance.get_item (guid);
		
		if(m_item == null)
		{
			return;
		}
		
		GameObject _root = game_data._instance.ins_object_res("ui/item_page_icon");
		_root.transform.parent = m_icon_root.transform;
		if (dir == 0)
		{
			_root.transform.localPosition = new Vector3(100 * 0,0,0);
			_root.transform.localScale = new Vector3(1,1,1);
		}
		else
		{
			_root.transform.localPosition = new Vector3(100 * dir,0,0);
			_root.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
			_root.GetComponent<UISprite>().alpha = 0.0f;
		}
		_root.SetActive(true);
		
		GameObject _icon = icon_manager._instance.create_item_icon_ex (guid);
		_icon.transform.parent = _root.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
		_icon.SetActive(true);
		_icon.GetComponent<BoxCollider>().enabled = false;
		
		m_icon = _root;
		
		if (dir != 0)
		{
			sys._instance.add_alpha_anim (m_icon,0.5f,0f,1f,0);
			sys._instance.add_scale_anim (m_icon,0.5f,0.2f,1.0f,0);
			TweenPosition _ceffect = TweenPosition.Begin(m_icon,0.5f,new Vector3(0,0,0));
			_ceffect.method = UITweener.Method.EaseInOut;
		}
		
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(2,m_item.id,0,0);
		m_select.SetActive(false);
		m_pro.GetComponent<UISprite>().spriteName = "cwjbtb_001";//宠物饲料
		m_zhanli.GetComponent<UILabel>().text = game_data._instance.get_t_language ("item_page_item.cs_141_43") + m_item.def_1;//经验 
		int index = int.Parse(transform.name)*7+m_page;
		if(sys._instance.is_select_item(index))
		{
			m_select.SetActive (true);
		}
		else
		{
			m_select.SetActive (false);
		}
	}
	
	
	// Update is called once per frame
	void Update () {
		m_rot += Time.deltaTime * 2f;
		
		Vector3 _pos = m_icon_root.transform.localPosition;
		
		_pos.y = Mathf.Sin (m_rot) * 5f;
		
		m_icon_root.transform.localPosition = _pos;
	}
}
