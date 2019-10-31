
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class xq_change : MonoBehaviour {

	public List<ulong> guids = new List<ulong>();
	public List<int> xqs = new List<int>();
	public GameObject m_view;

	public UILabel m_xq_change_Label;
	public UILabel m_tishi;
	public UILabel m_yh_Label;
	// Use this for initialization
	void Start () {

	}

	public void reset()
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_view);
		int _id = 0;
		for(int i = 0; i < guids.Count;++i)
		{
			ccard card = sys._instance.m_self.get_card_guid(guids[i]);
			if(card.get_role().xq == 0)
			{
				card.get_role().xq = 3;
			}
			if(card.get_role().xq == xqs[i] || card == null)
			{
				continue;
			}
			GameObject temp = game_data._instance.ins_object_res ("ui/xq_change_item");
			temp.transform.parent = m_view.transform;
			temp.transform.localPosition = new Vector3 (0 ,127 - _id * 120f ,0);
			temp.transform.localScale = new Vector3(1,1,1);
			temp.GetComponent<xq_change_item>().guid = guids[i];
			temp.GetComponent<xq_change_item>().xq = xqs[i]; 
			temp.GetComponent<xq_change_item>().updata_ui();
			sys._instance.add_pos_anim(temp,0.3f, new Vector3(0, 60, 0), (_id * 2 ) * 0.05f);
			sys._instance.add_alpha_anim(temp,0.3f, 0, 1.0f, (_id * 2) * 0.05f);
			_id ++;
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.transform.GetComponent<ui_show_anim>().hide_ui();
		}
		if(obj.transform.name == "yh")
		{
			this.gameObject.SetActive(false);
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_ROLE_YH_LOOK, _msg);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
