
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class czfp_shop_gui : MonoBehaviour,IMessage {

	public GameObject m_scro;
	public GameObject m_score;
	public int score = 0;
	private s_t_chongzhifanpai_shop m_t_shop;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		refresh_gird ();
	}

	public void refresh_gird()
	{
		m_score.GetComponent<UILabel>().text = score.ToString ();
		List<int> shop_ids = new List<int>();
		foreach(int id in game_data._instance.m_dbc_chongzhifanpai_shop.m_index.Keys)
		{
			s_t_chongzhifanpai_shop t_chongzhifanpai_shop = game_data._instance.get_t_chongzhifanpai_shop(id);
			shop_ids.Add(t_chongzhifanpai_shop.id);
		}
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_scro);
		int _id = 0;
		for(int i = 0; i < shop_ids.Count ;i ++)
		{
			int row = i / 2;
			int lie =  i % 2;
			
			GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");
			
			_card.transform.parent = m_scro.transform;
			_card.transform.localPosition = new Vector3(401 * lie - 164, -138 * row + 106,0);
			_card.transform.localScale = new Vector3(1,1,1);
			_card.GetComponent<temaihui_card>().m_shop_id = shop_ids[_id];
			_card.GetComponent<temaihui_card>().currency = score;
			_card.GetComponent<temaihui_card>().type = 19;
			_card.GetComponent<temaihui_card>().updata_ui();
			_card.SetActive (true);
			sys._instance.add_pos_anim(_card,0.3f, new Vector3(0, 60, 0), _id * 0.05f);
			sys._instance.add_alpha_anim(_card,0.3f, 0, 1.0f, _id * 0.05f);
			_id ++;
		}
	}

	void IMessage.message(s_message message)
	{
	
	}
	
	void IMessage.net_message(s_net_message message)
	{
		
	}
	// Update is called once per frame
	void Update () {
	
	}
}
