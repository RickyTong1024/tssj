using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class jiyin_gaizao_gui : MonoBehaviour,IMessage {	

	public GameObject m_select_jiyin_name;
	public GameObject m_hc_jiying_name;
	public GameObject m_select_item;
	public GameObject m_select_icon;
	public GameObject m_hc_icon;
	public GameObject m_zs;
	public GameObject m_zh;
	public GameObject m_add;
	public GameObject m_hc_item;
	public GameObject m_effect;
	public GameObject m_gaizao;
	public GameObject m_sm_gui;
	public GameObject m_scro;

	public int select_id = 0;
	public int select_num = 0;
	public int hc_id = 0;
	public int hc_num = 0;
    
	private bool is_play;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{

		cmessage_center._instance.remove_handle (this);
	}

	public void reset()
	{

		if(select_id == 0 || select_num ==0)
		{

			m_add.SetActive(false);
			m_select_jiyin_name.GetComponent<UILabel>().text = "";
			m_hc_item.GetComponent<BoxCollider>().enabled = false;
			sys._instance.remove_child(m_select_icon);
			m_zs.GetComponent<UILabel>().text = "0";
			m_zh.GetComponent<UILabel>().text = "0";
		}
		else
		{
			m_add.SetActive(true);
			m_select_jiyin_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(2, select_id, 0, 0);
			m_hc_item.GetComponent<BoxCollider>().enabled = true;
			sys._instance.remove_child(m_select_icon);
			GameObject _item = icon_manager._instance.create_item_icon_ex(select_id,select_num);
			_item.transform.parent = m_select_icon.transform;
			_item.transform.name = "select_jiyin";
			_item.transform.localPosition = new Vector3(0, 0, 0);
			_item.transform.localScale = new Vector3(1, 1, 1);
			UIButtonMessage[] meses = _item.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "select";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";

		}
		if(hc_id == 0 || hc_num == 0)
		{

			m_hc_jiying_name.GetComponent<UILabel>().text = "";
			sys._instance.remove_child(m_hc_icon);
			m_zs.GetComponent<UILabel>().text = "0";
			m_zh.GetComponent<UILabel>().text = "0";
		}
		else
		{
			s_t_item _t_item = game_data._instance.get_item(hc_id);
            s_t_role_gaizao _t_role_gaizao = game_data._instance.get_t_role_gaizao(_t_item.def_1);

            m_hc_jiying_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(2, hc_id, 0, 0);
			m_zs.GetComponent<UILabel>().text = sys._instance.get_res_color(2) + (_t_role_gaizao.jewel * select_num).ToString();	
			if(sys._instance.m_self.m_t_player.jewel < _t_role_gaizao.jewel * select_num)
			{

				m_zs.GetComponent<UILabel>().text = "[ff0000]" + (_t_role_gaizao.jewel * select_num).ToString();
			}
			m_zh.GetComponent<UILabel>().text = sys._instance.get_res_color(5) + (_t_role_gaizao.jjc_point * select_num).ToString();
			if(sys._instance.m_self.m_t_player.jjc_point < _t_role_gaizao.jjc_point * select_num)
			{

				m_zh.GetComponent<UILabel>().text = "[ff0000]" + (_t_role_gaizao.jjc_point * select_num).ToString();
			}
			sys._instance.remove_child(m_hc_icon);
			GameObject _item = icon_manager._instance.create_item_icon_ex(hc_id, select_num);
			_item.transform.parent = m_hc_icon.transform;
			_item.transform.name = "hc_jiyin";
			_item.transform.localPosition = new Vector3(0,0,0);
			_item.transform.localScale = new Vector3(1,1,1);
			UIButtonMessage[] meses = _item.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "select";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
		}
	}

	public void click(GameObject obj)
	{

		if(obj.transform.name == "close")
		{

			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
			this.transform.GetComponent<gui_remove>().m_remove = true;
		}
		if(obj.transform.name == "gaizao")
		{

			if(select_id == 0 || hc_id == 0)
			{

				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("jiyin_gaizao_gui.cs_142_46"));//[ffc882]请先选择需要改造的伙伴碎片和改造后的伙伴碎片
				return;
			}
            s_t_item _t_item = game_data._instance.get_item(hc_id);
            s_t_role_gaizao _t_role_gaizao = game_data._instance.get_t_role_gaizao(_t_item.def_1);
            if (sys._instance.m_self.m_t_player.jewel < _t_role_gaizao.jewel * select_num)
            {

                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_ronglian_gui.cs_141_59"));//钻石不足
                return;
            }
            if (sys._instance.m_self.m_t_player.jjc_point < _t_role_gaizao.jjc_point * select_num)
            {

                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("item_shop_card.cs_84_24"));//战魂不足
                return;
            }
			protocol.game.cmsg_role_suipian_gaizao _msg = new protocol.game.cmsg_role_suipian_gaizao ();
			_msg.src_suipian = select_id;
			_msg.src_num = select_num;
			_msg.target_suipian = hc_id;
			net_http._instance.send_msg<protocol.game.cmsg_role_suipian_gaizao> (opclient_t.CMSG_ROLE_SUIPIAN_GAIZAO, _msg);
		}
		if(obj.transform.name == "sm")
		{
			if (m_scro.GetComponent<SpringPanel>() != null)
			{
				m_scro.GetComponent<SpringPanel>().enabled = false;
			}
			m_scro.transform.localPosition = new Vector3(0, 0, 0);
			m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			m_sm_gui.SetActive(true);
		}
		if(obj.transform.name == "sm_close")
		{
			m_sm_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}
	
	public void select(GameObject obj)
	{
		if(obj.transform.name == "select_jiyin")
		{
			List<int> self = new List<int>();
			for(int i = 0;i <sys._instance.m_self.m_t_player.item_ids.Count;i ++)
			{
				if(sys._instance.m_self.is_card_fragment(sys._instance.m_self.m_t_player.item_ids[i]))
				{

					uint _item_id = sys._instance.m_self.m_t_player.item_ids[i];
					s_t_item _t_item = game_data._instance.get_item ((int)_item_id);
					if(_t_item.type == 3001)
					{
                      
                        s_t_role_gaizao _t_role_gaizao = game_data._instance.get_t_role_gaizao(_t_item.def_1);
                        s_t_class _t_class = game_data._instance.get_t_class(_t_item.def_1);
                        if (_t_role_gaizao == null)
                        {
                            self.Add((int)_item_id);
                        }
                        else if (sys._instance.m_self.m_t_player.level < (int)e_open_see.es_jiyingaizao_sp2)
                        {
                            if (_t_class.pz != 10 || _t_class.color != 4)
                            {
                                self.Add((int)_item_id);
                            }
                        }
                        else if (sys._instance.m_self.m_t_player.level < (int)e_open_see.es_jiyingaizao_sp3)
                        {
                            if (_t_class.pz > 11 || _t_class.pz < 10 || _t_class.color != 4)
                            {
                                self.Add((int)_item_id);
                            }
                        }
                        else if (sys._instance.m_self.m_t_player.level < (int)e_open_see.es_jiyingaizao_sp4)
                        {
                            if (_t_class.pz > 12 || _t_class.pz < 10 || _t_class.color != 4)
                            {
                                self.Add((int)_item_id);
                            }
                        }
                        else if (sys._instance.m_self.m_t_player.level >= (int)e_open_see.es_jiyingaizao_sp4)
                        {
                            if (_t_class.pz > 13 || _t_class.pz < 10 || _t_class.color < 4 || _t_class.color > 5)
                            {
                                self.Add((int)_item_id);
                            }
                        }
                        
					}
				}
			}
          
			if(hc_id != 0)
			{
				self.Add(hc_id);
			}
			root_gui._instance.show_common_jiyin_panel (game_data._instance.get_t_language ("jiyin_gaizao_gui.cs_247_47") , false, 0, self, "common_select_jiyin", false,this.gameObject);//选择基因
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			this.transform.GetComponent<gui_remove>().m_remove = false;
			reset();
		}
		if(obj.transform.name == "hc_jiyin")
		{
			if(select_id == 0)
			{
				return;
			}
			List<int> self = new List<int>();
			self.Add(select_id);
			s_t_item t_item = game_data._instance.get_item ((int)select_id);
            s_t_role_gaizao t_role_gaizao = game_data._instance.get_t_role_gaizao(t_item.def_1);
            for (int i = 0;i < game_data._instance.m_dbc_item.get_y();i ++)
			{
				int _item_id = int.Parse(game_data._instance.m_dbc_item.get(0,i));
				if(sys._instance.m_self.is_card_fragment((uint)_item_id))
				{
					s_t_item _t_item = game_data._instance.get_item ((int)_item_id);
                    s_t_role_gaizao _t_role_gaizao = game_data._instance.get_t_role_gaizao(_t_item.def_1);
                    if (_t_role_gaizao != null)
                    {
                        if (_t_role_gaizao.type != t_role_gaizao.type)
                        {
                            self.Add((int)_item_id);
                        }
                    }
                    else
                    {
                        self.Add((int)_item_id);
                    }
				}
			}
			root_gui._instance.show_common_jiyin_panel (game_data._instance.get_t_language ("jiyin_gaizao_gui.cs_247_47") , false, 1, self, "common_hc_jiyin", false,this.gameObject);//选择基因
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			this.transform.GetComponent<gui_remove>().m_remove = false;
			s_message message = new s_message();
			message.m_type = "hide_main_gui";
			cmessage_center._instance.add_message(message);
			reset();
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_ROLE_SUIPIAN_GAIZAO)
		{
			sys._instance.m_self.add_item((uint)hc_id, select_num, game_data._instance.get_t_language ("jiyin_gaizao_gui.cs_299_56"));//基因改造获得
            sys._instance.m_self.remove_item((uint)select_id, select_num, game_data._instance.get_t_language ("jiyin_gaizao_gui.cs_300_74"));//基因改造消耗

            s_t_item _t_item = game_data._instance.get_item(hc_id);
            s_t_role_gaizao _t_role_gaizao = game_data._instance.get_t_role_gaizao(_t_item.def_1);
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, _t_role_gaizao.jewel * select_num, game_data._instance.get_t_language("jiyin_gaizao_gui.cs_300_74"));//基因改造消耗
            sys._instance.m_self.sub_att(e_player_attr.player_jjc_point, _t_role_gaizao.jjc_point * select_num);

            select_id = 0;
			select_num = 0;
			hc_id = 0;
			hc_num = 0;
			is_play = true;
			m_effect.SetActive(true);
			m_effect.GetComponent<UISpriteAnimation>().ResetToBeginning();
			m_gaizao.GetComponent<BoxCollider>().enabled = false;
		}
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "common_select_jiyin")
		{
			select_id = (int)message.m_ints[0];
			select_num = (int)message.m_ints[1];
			if(hc_id != 0 && select_id != 0)
			{
				s_t_item t_item = game_data._instance.get_item (select_id);
				s_t_class t_class = game_data._instance.get_t_class(t_item.def_1);
				t_item = game_data._instance.get_item (hc_id);
				s_t_class _t_class = game_data._instance.get_t_class(t_item.def_1);
				if(t_class.pz != _t_class.pz)
				{
					if(t_class.pz != 10)
					{
						hc_id = 0;
					}
					else
					{
						if(_t_class.pz != 11)
						{
							hc_id = 0;
						}
					}
				}
			}
			reset();
		}
		if(message.m_type == "common_hc_jiyin")
		{
			hc_id = (int)message.m_ints[0];
			hc_num = select_num;
			reset();
		}
	}
    
	void Update () {
		if(is_play)
		{
			if(!m_effect.GetComponent<UISpriteAnimation>().isPlaying)
			{
				is_play = false;
				m_effect.SetActive(false);
				reset ();
				m_gaizao.GetComponent<BoxCollider>().enabled = true;
			}
		}
	}
}
