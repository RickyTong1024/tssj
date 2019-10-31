using UnityEngine;

public class juntuan_number_item_ex : MonoBehaviour,IMessage {
	public GameObject juntuan_number_icon;
	public GameObject juntuan_number_mingchen;
	public GameObject juntuan_number_level;
	public GameObject juntuan_number_zhandouli;
	//public GameObject juntuan_number_qiangdao;	
	public int member_index;
	public int curr_type;
	//public UILabel m_zhandouli;
	

	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public dhc.guild_member_t member
	{
		set
		{
			GameObject _obj1 = icon_manager._instance.create_player_icon((int)value.player_iocn_id, value.player_achieve, value.player_vip,value.nalflag);
			_obj1.transform.parent = juntuan_number_icon.transform;
			_obj1.transform.localScale = new Vector3(1, 1, 1);
			_obj1.transform.localPosition = new Vector3(0, 0, 0);
			juntuan_number_icon.GetComponent<UISprite>().spriteName = player.get_touxiang (value.player_iocn_id);
			juntuan_number_mingchen.GetComponent<UILabel>().text = game_data._instance.get_name_color(value.player_achieve) + value.player_name;
			juntuan_number_level.GetComponent<UILabel>().text = "[ffffff]" + value.player_level.ToString () + "[-]";

			juntuan_number_zhandouli.GetComponent<UILabel>().text = sys._instance.value_to_wan(value.bf).ToString();

			switch (curr_type)
			{
				case 1:
					juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_juandNum.text 
						= string.Format(game_data._instance.get_t_language ("juntuan_number_item_ex.cs_41_22"),ReturnTypeNum(curr_type),game_data._instance.get_guild_fight(curr_type - 1).defendrolenum);//[0AFEFF]左据点守军：[0AFF16]{0}/{1}
					break;
				case 2:
					juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_juandNum.text 
						= string.Format(game_data._instance.get_t_language ("juntuan_number_item_ex.cs_45_22"),ReturnTypeNum(curr_type),game_data._instance.get_guild_fight(curr_type - 1).defendrolenum);//[0AFEFF]右据点守军：[0AFF16]{0}/{1}
					break;
				case 3:
					juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_juandNum.text 
						= string.Format(game_data._instance.get_t_language ("juntuan_number_item_ex.cs_49_22"),ReturnTypeNum(curr_type),game_data._instance.get_guild_fight(curr_type - 1).defendrolenum);//[0AFEFF]主据点守军：[0AFF16]{0}/{1}
					break;
				case 4:
					juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_juandNum.text 
						= string.Format(game_data._instance.get_t_language ("juntuan_number_item_ex.cs_53_22"),ReturnTypeNum(curr_type),game_data._instance.get_guild_fight(curr_type - 1).defendrolenum);//[0AFEFF]司令部守军：[0AFF16]{0}/{1}
					break;
			}
			
		}
	}
	public void click(GameObject obj)
	{

		if (obj.name == "Toggle") 
		{
			bool temp = this.transform.Find("Toggle").GetComponent<UIToggle>().value; 
			if (juntuan_gui._instance.m_zhiwu_t == 2) 
			{
				this.transform.Find("Toggle").GetComponent<UIToggle>().value = !temp;
				root_gui._instance.show_prompt_dialog_box (game_data._instance.get_t_language ("juntuan_number_item_ex.cs_68_47"));//[ffc882]只有军团长和副军团长才能布置驻守人员
				return;
			}

			if(ReturnTypeNum(curr_type) >= game_data._instance.get_guild_fight(curr_type - 1).defendrolenum && temp)
			{
				
				this.transform.Find("Toggle").GetComponent<UIToggle>().value = !temp;
				temp =!temp;
				root_gui._instance.show_prompt_dialog_box (game_data._instance.get_t_language ("juntuan_number_item_ex.cs_77_47"));//[ffc882]勾选人数已达到本据点上限
				return;
			}

			if (temp) 
			{
				if(curr_type ==1)
				{
				juntuan_gui._instance.m_defend_type [member_index] = 1;
				}
				else if(curr_type ==2)
				{
					juntuan_gui._instance.m_defend_type [member_index] = 2;
				}
				else if(curr_type ==3)
				{
					juntuan_gui._instance.m_defend_type [member_index] = 3;
				}
				else if(curr_type ==4)
				{
					juntuan_gui._instance.m_defend_type [member_index] = 4;
				}
			}
			else 
			{
				juntuan_gui._instance.m_defend_type [member_index] = 0;
			}

			switch (curr_type)
			{
				case 1:
					juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_juandNum.text 
						= string.Format(game_data._instance.get_t_language ("juntuan_number_item_ex.cs_41_22"),ReturnTypeNum(curr_type),game_data._instance.get_guild_fight(curr_type - 1).defendrolenum);//[0AFEFF]左据点守军：[0AFF16]{0}/{1}
					break;
				case 2:
					juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_juandNum.text 
						= string.Format(game_data._instance.get_t_language ("juntuan_number_item_ex.cs_45_22"),ReturnTypeNum(curr_type),game_data._instance.get_guild_fight(curr_type - 1).defendrolenum);//[0AFEFF]右据点守军：[0AFF16]{0}/{1}
					break;
				case 3:
					juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_juandNum.text 
						= string.Format(game_data._instance.get_t_language ("juntuan_number_item_ex.cs_49_22"),ReturnTypeNum(curr_type),game_data._instance.get_guild_fight(curr_type - 1).defendrolenum);//[0AFEFF]主据点守军：[0AFF16]{0}/{1}
					break;
				case 4:
					juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_juandNum.text 
						= string.Format(game_data._instance.get_t_language ("juntuan_number_item_ex.cs_53_22"),ReturnTypeNum(curr_type),game_data._instance.get_guild_fight(curr_type - 1).defendrolenum);//[0AFEFF]司令部守军：[0AFF16]{0}/{1}
					break;
			}
//			juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().initItems();
		}
	}

	int ReturnTypeNum(int m_type)
	{
		int temp_num = 0;
		for (int i = 0; i < juntuan_gui._instance.m_defend_type.Count; i++)
		{
			if(juntuan_gui._instance.m_defend_type[i] == m_type)
			{
				temp_num++;
			}
		}
		return temp_num;
	}

	void IMessage.message (s_message message)
	{
		
		
	}
	void IMessage.net_message(s_net_message message)
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
