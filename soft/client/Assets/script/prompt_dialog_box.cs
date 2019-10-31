
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class s_prompt_dialog_box_tile
{
	public float time;
	public GameObject obj;
	public GameObject label1;
	public GameObject icon;
	public GameObject label2;
	public bool tdestory;
}

public class prompt_dialog_box : MonoBehaviour,IMessage {

	public List<s_prompt_dialog_box_tile> m_tiles = new List<s_prompt_dialog_box_tile>();

	public List<s_message> m_messages = new List<s_message>();
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void OnEnable()
	{
		InvokeRepeating("add_tile", 0.0f,0.2f);
	}
	
	void OnDisable()
	{
		CancelInvoke ("add_tile");
	}

	void IMessage.net_message(s_net_message message)
	{
		
	}

	void IMessage.message(s_message message)
	{
		//SpringPosition.Begin(_tile.obj, Vector3, 15f).
		if(message.m_type == "show_prompt_dialog_box")
		{
			m_messages.Add(message);
		}
	}
	void add_tile()
	{

		if(m_messages.Count > 0)
		{
			s_prompt_dialog_box_tile _tile = new s_prompt_dialog_box_tile();
			
			_tile.time = 3.0f;
			_tile.tdestory = false;

			_tile.obj = game_data._instance.ins_object_res("ui/prompt_dialog_box_tile");
			_tile.obj.SetActive(true);
			_tile.obj.transform.parent = this.transform;
			_tile.obj.transform.localPosition = new Vector3(0,-500,0);
			_tile.obj.transform.localScale = new Vector3(1,1,1);
			_tile.label1 = _tile.obj.transform.Find("label1").gameObject;
			_tile.icon = _tile.obj.transform.Find("icon").gameObject;
			_tile.label2 = _tile.obj.transform.Find("label2").gameObject;

			_tile.label1.GetComponent<UILabel>().text = (string)m_messages[0].m_string[0];
			string s = _tile.label1.GetComponent<UILabel>().processedText;
			int w1 = _tile.label1.GetComponent<UILabel>().width;

			sys._instance.remove_child(_tile.icon);
			int m = (int)m_messages[0].m_ints[0];
			if (m == 0)
			{
				s = (string)m_messages[0].m_string[1];
				_tile.icon.GetComponent<UISprite>().spriteName = s;
			}
			else
			{
				s = (string)m_messages[0].m_string[1];
				int sid = int.Parse(s);
				GameObject icon = icon_manager._instance.create_reward_icon_ex(m, sid, 0, 0);
				icon.transform.parent = _tile.icon.transform;
				icon.transform.localPosition = new Vector3(44,0,0);
				icon.transform.localScale = new Vector3(1,1,1);
				icon.GetComponent<BoxCollider>().enabled = false;
			}
			int w2 = 0;
			if (s != "")
			{
				w1 += 10;
				w2 = 40;
			}
			_tile.label2.GetComponent<UILabel>().text = (string)m_messages[0].m_string[2];
			s = _tile.label2.GetComponent<UILabel>().processedText;
			int w3 = _tile.label2.GetComponent<UILabel>().width;
			int w = w1 + w2 + w3;
			_tile.label1.transform.localPosition = new Vector3(-w / 2, 0, 0);
			_tile.icon.transform.localPosition = new Vector3(-w / 2 + w1, 0, 0);
			_tile.label2.transform.localPosition = new Vector3(-w / 2 + w1 + w2, 0, 0);
			
			m_tiles.Add(_tile);
			
			update_ui();

			m_messages.RemoveAt(0);
		}
	}
	void update_ui()
	{
		for(int i = 0;i < m_tiles.Count;i ++ )
		{
			s_prompt_dialog_box_tile _tile = m_tiles[i];
			if (_tile.tdestory == true)
			{
				continue;
			}
			SpringPosition.Begin(_tile.obj,new Vector3(0,(m_tiles.Count - i) * 40f,0), 5.5f);
		}
	}

	// Update is called once per frame
	void Update () {
	
		for(int i = 0;i < m_tiles.Count;)
		{
			s_prompt_dialog_box_tile _tile = m_tiles[i];

			if(_tile.time > 0)
			{
				_tile.time -= Time.deltaTime;

				if(_tile.time < 1.0f)
				{
					_tile.obj.GetComponent<UIWidget>().alpha = _tile.time;
					if (_tile.tdestory == false)
					{
						_tile.tdestory = true;
						SpringPosition.Begin(_tile.obj,new Vector3(0,500,0), 5.5f);
					}
				}

				if(_tile.time <= 0.0f)
				{
					Object.Destroy (_tile.obj);
					m_tiles.Remove(_tile);
					continue;
				}
			}

			i ++;
		}
	}
}
