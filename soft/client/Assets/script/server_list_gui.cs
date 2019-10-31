
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class server_list_gui : MonoBehaviour {

	public GameObject m_round_list;
	public GameObject m_right;
	public GameObject m_top;
	public GameObject m_select;
	private List<GameObject> m_round_objs = new List<GameObject>();
	private List<GameObject> m_round_objs1 = new List<GameObject>();
	private int m_cut = 10;
	public UILabel m_all_server;
	public UILabel m_last_server;

	public void OnEnable()
	{
		reset ();
	}
	string get_channel_round(int i)
	{
		i ++;

		string _start = i.ToString ();

		while(_start.Length < 3)
		{
			_start = "0" + _start;
		}

		i += 9;

		string _end = i.ToString ();
		
		while(_end.Length < 3)
		{
			_end = "0" + _end;
		}

		return string.Format(game_data._instance.get_t_language ("server_list_gui.cs_45_23"),_start , _end );//{0}-{1} 区
	}

	public void close(GameObject obj)
	{
        s_message _message = new s_message();
        _message.m_type = "select_server";
        _message.m_ints.Add(game_data._instance.m_player_data.m_id);
        cmessage_center._instance.add_message(_message);
        GameObject.Destroy(this.gameObject);
    }
    
    public void click_select(GameObject obj)
	{
		s_message _message = new s_message ();
		_message.m_type = "select_server";
		_message.m_ints.Add (int.Parse(obj.transform.name));
		cmessage_center._instance.add_message (_message);
		GameObject.Destroy (this.gameObject);
	}
	public void click(GameObject obj)
	{
		if(obj.name == "close")
		{
            s_message _message = new s_message();
            _message.m_type = "select_server";
            _message.m_ints.Add(game_data._instance.m_player_data.m_id);
            cmessage_center._instance.add_message(_message);
            GameObject.Destroy(this.gameObject);
        }
	}

    public void click_myplayer(GameObject obj)
    {
        m_select.SetActive(true);
        for (int i = 0; i < m_round_objs1.Count; i++)
        {
            string s = "[006bce]";
            string s1 = m_round_objs1[i].transform.Find("name").GetComponent<UILabel>().text.Substring(8);
            m_round_objs1[i].transform.Find("name").GetComponent<UILabel>().text = s + s1;
        }

        Vector3 _pos = obj.transform.localPosition;
        _pos.y += 160;
        string s3 = obj.transform.Find("name").GetComponent<UILabel>().text.Substring(8);
        obj.transform.Find("name").GetComponent<UILabel>().text = "[00ffff]" + s3;
        m_select.transform.localPosition = _pos;
        last_server();
    }

	public void click_round(GameObject obj)
	{
		m_select.SetActive (true);
		for(int i = 0;i < m_round_objs1.Count;i ++)
		{
			string s = "[006bce]";
			string s1 = m_round_objs1[i].transform.Find("name").GetComponent<UILabel>().text.Substring(8);
			m_round_objs1[i].transform.Find("name").GetComponent<UILabel>().text = s + s1;
		}

		Vector3 _pos = obj.transform.localPosition;
		_pos.y += 160;
		string s3 = obj.transform.Find("name").GetComponent<UILabel>().text.Substring (8);
		obj.transform.Find("name").GetComponent<UILabel>().text = "[00ffff]" + s3;
		m_select.transform.localPosition = _pos;


		server_list (int.Parse(obj.transform.name) * 10);
	}
	void show_item(GameObject obj, int id, int type = 0)
	{
		obj.transform.Find("state").GetComponent<UILabel>().color = new Color (255f/255, 255f/255, 255f/255);
		obj.transform.Find("name").GetComponent<UILabel>().color = new Color (255f/255, 255f/255, 255f/255);
		obj.transform.Find("state").GetComponent<UILabel>().gradientBottom = new Color (19f/255, 126f/255, 219f/255);
		obj.transform.Find("name").GetComponent<UILabel>().gradientBottom = new Color (19f/255, 126f/255, 219f/255);
		if(obj.name != "top")
		{
			obj.transform.GetComponent<UISprite>().spriteName = "xdl_yblbdb001";
		}
		else
		{
			obj.transform.Find("state").GetComponent<UILabel>().color = new Color (255f/255, 255/255f, 255f/255);
			obj.transform.Find("name").GetComponent<UILabel>().color = new Color (255f/255, 255f/255, 255f/255);
			obj.transform.Find("state").GetComponent<UILabel>().gradientBottom = new Color (27f/255, 154/255f, 16f/255);
			obj.transform.Find("name").GetComponent<UILabel>().gradientBottom = new Color (27f/255, 154f/255, 16f/255);
		}

        if (type > 0)
        {
            for (int i = 0; i < game_data._instance.m_storage_sid.Count; i++)
            {
                if (game_data._instance.m_storage_sid[i] == id)
                {
                    obj.transform.Find("state").GetComponent<UILabel>().text = string.Format("Lv.{0}", game_data._instance.m_storage_level[i]);
                }
            }
        }
        else
        {
            int m_satre = 0;
            for (int i = 0; i < game_data._instance.m_server_list.Count; i++)
            {
                if (game_data._instance.m_server_list[i].m_id == id.ToString())
                {
                    m_satre = game_data._instance.m_server_list[i].m_state;
                }
            }
            if (m_satre == 1)
            {
                obj.transform.Find("state").GetComponent<UILabel>().text = "【" + game_data._instance.get_t_language("login.cs_130_7") + "】";//流畅
            }
            else if (m_satre == 2)
            {
                obj.transform.Find("state").GetComponent<UILabel>().text = "【" + game_data._instance.get_t_language("login.cs_135_7") + "】";//普通
            }
            else if (m_satre == 3)
            {
                obj.transform.Find("state").GetComponent<UILabel>().text = "【" + game_data._instance.get_t_language("login.cs_140_7") + "】";//爆满
            }
        }

        if (id == game_data._instance.m_player_data.m_id)
		{
			if(obj.name != "top")
			{
				obj.GetComponent<UISprite>().spriteName = "xdl_yblbdb002";
				obj.transform.Find("state").GetComponent<UILabel>().color = new Color (255f/255,255f/255,255f/255);
				obj.transform.Find("name").GetComponent<UILabel>().color = new Color (255f/255,255f/255,255f/255);
				obj.transform.Find("state").GetComponent<UILabel>().gradientBottom = new Color (27f/255,154/255f,16f/255);
				obj.transform.Find("name").GetComponent<UILabel>().gradientBottom = new Color (27f/255,154f/255,16f/255);
			}
		}
        
		for (int i = 0; i < game_data._instance.m_server_list.Count; i++)
        {
			if(game_data._instance.m_server_list[i].m_id == id.ToString())
			{
				obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.m_server_list[i].m_name;
            }
		}
	}

    void last_server()
    {
        sys._instance.remove_child(m_right);

        int _num = 0;
        int _id = 0;

        for (int i = 0; i < game_data._instance.m_storage_sid.Count; ++i)
        {
            _id = game_data._instance.m_storage_sid[i];
            bool flag = false;
            for (int j = 0; j < game_data._instance.m_server_list.Count && _num < 10; ++j)
            {
                if (game_data._instance.m_server_list[j].m_id == _id.ToString())
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                int x = _num % 2;
                int y = _num / 2;
                GameObject _item = game_data._instance.ins_object_res("ui/server_list_item_ex");
                _item.transform.parent = m_right.transform;
                _item.transform.localPosition = new Vector3(-142 + x * 284, -y * 62 + 70, 0);
                _item.transform.localScale = new Vector3(1, 1, 1);
                _item.transform.name = _id.ToString();
                _item.GetComponent<UIButtonMessage>().target = this.gameObject;
                show_item(_item, _id, 1);
                _num++;
            }
        }
        m_right.transform.parent.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language("server_list_gui.cs_server_list_round_my_channel");
    }

	void server_list(int id)
	{

		sys._instance.remove_child (m_right);

		int _num = 0;
		int _id = 0;

		for(int i = id; i < game_data._instance.m_server_list.Count && _num < 10; ++i)
		{
			_id = int.Parse(game_data._instance.m_server_list[i].m_id);

            int x = _num % 2;
            int y = _num / 2;
            GameObject _item = game_data._instance.ins_object_res("ui/server_list_item_ex");
			_item.transform.parent = m_right.transform;
			_item.transform.localPosition = new Vector3(-142 + x * 284, -y * 62 + 70, 0);
            _item.transform.localScale = new Vector3(1, 1, 1);
			_item.transform.name = _id.ToString();
			_item.GetComponent<UIButtonMessage>().target = this.gameObject;
			show_item(_item, _id);
            _num++;
		}
		m_right.transform.parent.Find("name").GetComponent<UILabel>().text ="-" + get_channel_round (id) + "-";
	}
	void reset()
	{
		sys._instance.remove_child (m_round_list);

		int _round = game_data._instance.m_server_list.Count / m_cut;

		if(game_data._instance.m_server_list.Count % m_cut > 0)
		{
			_round ++;
		}

		int _id = 1;
		m_round_objs1.Clear ();

        GameObject my_player_item = game_data._instance.ins_object_res("ui/server_list_round");

        my_player_item.transform.parent = m_round_list.transform;
        my_player_item.transform.localPosition = new Vector3(0,-10, 0);
        my_player_item.transform.localScale = new Vector3(1, 1, 1);
        m_round_objs1.Add(my_player_item);
        m_round_objs.Add(my_player_item);
        my_player_item.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language("server_list_gui.cs_server_list_round_my_channel");
        my_player_item.GetComponent<UIDragScrollView>().scrollView = m_round_list.GetComponent<UIScrollView>();
        my_player_item.GetComponent<UIButtonMessage>().target = this.gameObject;
        my_player_item.GetComponent<UIButtonMessage>().functionName = "click_myplayer";

        for (int i = _round - 1;i >= 0;i --)
		{
			GameObject _item = game_data._instance.ins_object_res("ui/server_list_round");

			_item.transform.parent = m_round_list.transform;
			_item.transform.localPosition = new Vector3(0,- 50 * _id - 10,0);
			_item.transform.localScale = new Vector3(1,1,1);
			m_round_objs1.Add(_item);
			m_round_objs.Add(_item);

			_item.transform.Find("name").GetComponent<UILabel>().text = get_channel_round(i * 10);

			_item.transform.name = i.ToString();

			_item.GetComponent<UIDragScrollView>().scrollView = m_round_list.GetComponent<UIScrollView>();
			_item.GetComponent<UIButtonMessage>().target = this.gameObject;
			_id ++;
		}


		show_item(m_top,game_data._instance.m_player_data.m_id);

		int temp2channel = 0;
		for (int i = 0; i < game_data._instance.m_server_list.Count; i++) {
			if (game_data._instance.m_player_data.m_id.ToString() != game_data._instance.m_server_list[i].m_id)
			{
				temp2channel++;
			}
			else
			{
				break;
			}
		}

		_round = temp2channel / m_cut;
		server_list (_round * 10);
	}

	// Update is called once per frame
	void Update () {
	
		if(m_round_objs.Count > 0)
		{
			m_select.SetActive (true);
			 int temp2round = 0;
			for (int i = 0; i < game_data._instance.m_server_list.Count; i++) {
				if(game_data._instance.m_player_data.m_id.ToString() != game_data._instance.m_server_list[i].m_id)
				{
					temp2round++;
				}
				else
				{
					break;
				}
			}
			int _round = temp2round / m_cut;

			for(int i = 0;i < m_round_objs.Count;i ++)
			{
				string s = "[006bce]";
				if(m_round_objs[i].transform.name == _round.ToString())
				{
					Vector3 _pos = m_round_objs[i].transform.localPosition;
					_pos.y += 160;
					
					m_select.transform.localPosition = _pos;
					s = "[00ffff]";

				}
				m_round_objs[i].transform.Find("name").GetComponent<UILabel>().text = s + m_round_objs[i].transform.Find("name").GetComponent<UILabel>().text;
			}

			m_round_objs.Clear();
		}

	}
}
