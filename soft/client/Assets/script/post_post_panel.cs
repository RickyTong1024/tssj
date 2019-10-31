
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class post_post_panel : MonoBehaviour, IMessage {

	public GameObject m_view;
	dhc.post_t m_post;
	bool m_is_zd;
	public GameObject m_post_text;
	public GameObject m_empty;
	public List<dhc.post_t> m_posts; 

	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnFinished()
	{

	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public static int comp(dhc.post_t x, dhc.post_t y)
	{
		if (x.sender_date > y.sender_date)
		{
			return 1;
		}
		return -1; 
	}

	public void clear()
	{
		sys._instance.remove_child (m_view);
		m_empty.gameObject.SetActive(true);
	}

	public void reset()
	{
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);

		int num = 0;
		if (sys._instance.m_self.m_t_player.level >= 10)
		{
			for (int i = 0; i < game_data._instance.m_postzd_list.Count; ++i)
			{
				GameObject target = game_data._instance.ins_object_res("ui/post");
				target.transform.parent = m_view.transform;
				target.transform.localPosition = new Vector3(0, 177 - num * 141,0);
				target.transform.localScale = new Vector3(1,1,1);
				target.transform.GetComponent<post>().m_is_zd = true;
				target.transform.GetComponent<post>().m_zd_index = i;
				target.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
				
				sys._instance.add_pos_anim(target,0.3f, new Vector3(-300, 0, 0), num * 0.05f);
				sys._instance.add_alpha_anim(target,0.3f, 0, 1.0f, num * 0.05f);
				num++;
			}
		}

		List<dhc.post_t> posts = new List<dhc.post_t>();
		for (int i = 0; i < m_posts.Count; ++i)
		{
			dhc.post_t post = m_posts[i];
			posts.Add(post);
		}
		posts.Sort(comp);
		for (int i = 0; i < posts.Count; ++i)
		{
			dhc.post_t post = posts[i];
			GameObject target = game_data._instance.ins_object_res("ui/post");
			target.transform.parent = m_view.transform;
			target.transform.localPosition = new Vector3(0, 177 - num * 141,0);
			target.transform.localScale = new Vector3(1,1,1);
			target.transform.GetComponent<post>().m_post = post;
			target.transform.GetComponent<UIButtonMessage>().target = this.gameObject;

			sys._instance.add_pos_anim(target,0.3f, new Vector3(-300, 0, 0), num * 0.05f);
			sys._instance.add_alpha_anim(target,0.3f, 0, 1.0f, num * 0.05f);
			num++;
		}

		if (num > 0)
		{
			m_empty.gameObject.SetActive(false);
		}
		else
		{
			m_empty.gameObject.SetActive(true);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void click(GameObject obj)
	{
		if (obj.name == "post(Clone)")
		{
			m_post = obj.transform.GetComponent<post>().m_post;
			m_is_zd = obj.transform.GetComponent<post>().m_is_zd;
			m_post_text.transform.GetComponent<post_text>().m_post = m_post;
			m_post_text.transform.GetComponent<post_text>().m_is_zd = obj.transform.GetComponent<post>().m_is_zd;
			m_post_text.transform.GetComponent<post_text>().m_zd_index = obj.transform.GetComponent<post>().m_zd_index;
			m_post_text.transform.GetComponent<post_text>().reset();
			m_post_text.SetActive(true);
		}
		else if (obj.name == "get")
		{
			if(obj.transform.Find("Label").GetComponent<UILabel>().text == game_data._instance.get_t_language ("jt_mobai.cs_97_91"))//领取
			{
				m_post_text.transform.Find("frame_big").GetComponent<frame>().hide();
				if (m_is_zd)
				{
					return;
				}
				protocol.game.cmsg_post_get _msg = new protocol.game.cmsg_post_get ();
				_msg.post_guid = m_post.guid;
				net_http._instance.send_msg<protocol.game.cmsg_post_get> (opclient_t.CMSG_POST_GET, _msg);
			}
			else if (obj.transform.Find("Label").GetComponent<UILabel>().text ==  game_data._instance.get_t_language ("jt_mobai.cs_217_99"))//关闭
			{
				m_post_text.transform.Find("frame_big").GetComponent<frame>().hide();
			}
		}
		else if (obj.name == "hide")
		{
			m_post_text.transform.Find("frame_big").GetComponent<frame>().hide();

		}
	}

	void IMessage.message (s_message message)
	{
		if(message.m_type == "delete_post")
		{
			ulong post_guid = (ulong)message.m_long[0];
			for (int i = 0; i < m_posts.Count; ++i)
			{
				if (m_posts[i].guid == post_guid)
				{
					m_post = m_posts[i];
					break;
				}
			}
			protocol.game.cmsg_post_get _msg = new protocol.game.cmsg_post_get ();
			_msg.post_guid = post_guid;
			net_http._instance.send_msg<protocol.game.cmsg_post_get> (opclient_t.CMSG_POST_GET, _msg);
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_POST_GET)
		{
			protocol.game.smsg_post_get _msg = net_http._instance.parse_packet<protocol.game.smsg_post_get> (message.m_byte);
			for(int i =0 ;i< _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
			}
			for (int i = 0; i < _msg.types.Count; ++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg.value1s[i],_msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("post_post_panel.cs_177_101"));//邮件领取
			}
            for (int i = 0; i < _msg.roles.Count; i++)
            {
                sys._instance.m_self.add_card(_msg.roles[i]);
            }
            for (int i = 0; i < _msg.pets.Count; i++)
            {
                sys._instance.m_self.add_pet(_msg.pets[i]);
            }
            for (int i = 0; i < m_posts.Count; ++i)
            {
                if (m_posts[i].guid == m_post.guid)
                {
                    m_posts.RemoveAt(i);
                    break;
                }
            }
			if (m_posts.Count <= 0)
			{
				sys._instance.m_self.is_post = 0;
			}
			reset();
		}
		if (message.m_opcode == opclient_t.CMSG_POST_LOOK)
		{
			m_posts = new List<dhc.post_t>();
			protocol.game.smsg_post_look _msg = net_http._instance.parse_packet<protocol.game.smsg_post_look> (message.m_byte);
			for (int i = 0; i < _msg.posts.Count; ++i)
			{
				m_posts.Add(_msg.posts[i]);
			}

			if (m_posts.Count <= 0)
			{
				sys._instance.m_self.is_post = 0;
			}
			reset();
		}
	}
}
