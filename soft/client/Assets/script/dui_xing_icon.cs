
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class dui_xing_icon : UIDragDropItem
{
	/// <summary>
	/// Prefab object that will be instantiated on the DragDropSurface if it receives the OnDrop event.
	/// </summary>
	
	public Transform m_parent;
	public dui_xing_icon m_item;
	public List<GameObject> m_icons = new List<GameObject>();
	bool flag = false;
	/// <summary>
	/// Drop a 3D game object onto the surface.
	/// </summary>
	dui_xing_icon get_item(GameObject surface)
	{
		dui_xing_icon _item = null;
		if(surface.transform.childCount != 0)
		{
			_item = surface.transform.GetChild(0).GetComponent<dui_xing_icon>();

			if(_item != null)
			{
				return _item;
			}
		}
		_item = surface.transform.GetComponent<dui_xing_icon>();
		
		if(_item != null)
		{
			return _item;
		}
		return null;
	}
	protected override void OnDragDropRelease (GameObject surface)
	{
		flag = false;
		m_item.GetComponent<UIPanel>().depth -= 1;
		if (surface != null)
		{
			dui_xing_icon _item = get_item(surface);
			
			if (_item != null)
			{
				s_message _message = new s_message();

				_message.m_type = "dui_xing";
				_message.m_ints.Add(int.Parse(m_item.gameObject.transform.name));
				_message.m_ints.Add(int.Parse(_item.gameObject.transform.name));
				_message.m_ints.Add(int.Parse(m_item.transform.parent.gameObject.transform.name));
				_message.m_ints.Add(int.Parse(_item.transform.parent.gameObject.transform.name));
				cmessage_center._instance.add_message(_message);

				m_item = _item;
				base.OnDragDropRelease(surface);

				GameObject.Destroy(this.gameObject);

				return;
			}
		}
		base.OnDragDropRelease(surface);
		this.transform.parent = m_parent;
		this.transform.localPosition = new Vector3(0,0,0);
	}

	protected override void OnDragDropStart()
	{
		flag = true;
		m_item.GetComponent<UIPanel>().depth += 1;
		base.OnDragDropStart();
	}
	
	void Update () {
		if(flag)
		{
			m_item.transform.position = UICamera.mainCamera.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
		}
	}

}
