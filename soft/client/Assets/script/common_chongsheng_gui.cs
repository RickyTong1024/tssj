
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class common_chongsheng_gui : MonoBehaviour {
    public GameObject m_view;
	public bool flag = false;
    public s_message m_msg = new s_message();
    const int m_consume_jewel = 50;
    public UILabel m_info;
    public UILabel m_jewel;
    public UILabel m_name;
    public UILabel m_name1;
	private int jewel = 0;
    public void reset_card(ccard card)
    {
		flag = true;
        m_info.text = "";
		jewel = m_consume_jewel;
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = Vector3.zero;
        m_view.GetComponent<UIPanel>().clipOffset = Vector3.zero;
        m_jewel.text = sys._instance.get_res_color(2) +  m_consume_jewel + "";
        List<s_t_reward> rewards = card.get_chongsheng_reward();
		s_t_reward _reward4 = new s_t_reward();
		_reward4.type = 3;
		_reward4.value1 = card.get_template_id();
		_reward4.value2 = 1;
		_reward4.value3 = 0;
		rewards.Add(_reward4);
        reward_hebin(rewards);
        for (int i = 0; i < rewards.Count; i++)
        {
            GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(-200 + 100 * (i % 5), 55 - 110 * (i / 5),0);
            obj.transform.localScale = Vector3.one;
            obj.AddComponent<UIDragScrollView>(); 
        }
		m_jewel.text =sys._instance.get_res_color(2) + m_consume_jewel + "";
        m_jewel.transform.parent.gameObject.SetActive(true);
        m_name.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_41_22");//伙伴重生
    }
    public void reset_jiegu_card(ccard card)
    {
        m_info.text = "";
		flag = false;
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = Vector3.zero;
        m_view.GetComponent<UIPanel>().clipOffset = Vector3.zero;
		m_jewel.text = sys._instance.get_res_color(2) + "0";
        List<s_t_reward> rewards = card.get_jiegu_reward();
        for (int i = 0; i < rewards.Count; i++)
        {
            GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(-200 + 100 * (i % 5), 55 - 110 * (i / 5), 0);
            obj.transform.localScale = Vector3.one;
            obj.AddComponent<UIDragScrollView>(); 
        }
        m_jewel.transform.parent.gameObject.SetActive(false);
        m_name.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_61_22");//伙伴分解
    }
    
    public void reset_baowu(dhc.treasure_t treasure1)
    {
        m_info.text = "";
		flag = true;
		jewel = m_consume_jewel;
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = Vector3.zero;
        m_view.GetComponent<UIPanel>().clipOffset = Vector3.zero;
		m_jewel.text = sys._instance.get_res_color(2) + "0";
        List<s_t_reward> rewards = treasure.get_treasure_reward(treasure1);
        reward_hebin(rewards);
        for (int i = 0; i < rewards.Count; i++)
        {
            GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(-200 + 100 * (i % 5), 55 - 110 * (i / 5), 0);
            obj.transform.localScale = Vector3.one;
            obj.AddComponent<UIDragScrollView>(); 
        }
		m_jewel.text = sys._instance.get_res_color(2) +  m_consume_jewel + "";
        m_jewel.transform.parent.gameObject.SetActive(true);
        m_name.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_85_22");//饰品重生
    }
    public void reset_equip_chongsheng(dhc.equip_t equip1)
    {
        m_info.text = "";
		flag = true;
		jewel = m_consume_jewel;
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = Vector3.zero;
        m_view.GetComponent<UIPanel>().clipOffset = Vector3.zero;
		m_jewel.text = sys._instance.get_res_color(2) + "0";
        List<s_t_reward> rewards = equip.get_equip_chongsheng_reward(equip1);
        reward_hebin(rewards);
        for (int i = 0; i < rewards.Count; i++)
        {
            GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(-200 + 100 * (i % 5), 55 - 110 * (i / 5), 0);
            obj.transform.localScale = Vector3.one;
            obj.AddComponent<UIDragScrollView>(); 
        }
		m_jewel.text = sys._instance.get_res_color(2) + m_consume_jewel + "";
        m_jewel.transform.parent.gameObject.SetActive(true);
        m_name.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_108_22");//装备重生
    }
    public void reset_equip_yijian_fenjie(List<dhc.equip_t> equips)
    {
        m_info.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_112_22");//一键分解会自动分解所有未装备的蓝色、绿色装备
		flag = false;
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = Vector3.zero;
        m_view.GetComponent<UIPanel>().clipOffset = Vector3.zero;
		m_jewel.text = sys._instance.get_res_color(2) +  "0";
        List<s_t_reward> rewards = new List<s_t_reward>();
        for (int i = 0; i < equips.Count; i++)
        {
             List<s_t_reward> reward = equip.get_equip_fenjie_reward(equips[i]);
             for (int j = 0; j < reward.Count; j++)
             {
                 rewards.Add(reward[j]);
                 
             }
 
        }
        reward_hebin(rewards);
        for (int i = 0; i < rewards.Count; i++)
        {
            GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(-200 + 100 * (i % 5), 55 - 110 * (i / 5), 0);
            obj.transform.localScale = Vector3.one;
            obj.AddComponent<UIDragScrollView>(); 

        }
        m_jewel.transform.parent.gameObject.SetActive(false);
        m_name.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_140_22");//装备分解
 
    }

    public void reset_role_yijian_fenjie(List<ccard> cards)
    {
        m_info.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_146_22");//一键分解会自动分解所有未上阵的紫色、蓝色伙伴
		flag = false;
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = Vector3.zero;
        m_view.GetComponent<UIPanel>().clipOffset = Vector3.zero;
        m_jewel.text = "0";
        List<s_t_reward> rewards = new List<s_t_reward>();
        for (int i = 0; i < cards.Count; i++)
        {
            List<s_t_reward> reward = cards[i].get_jiegu_reward();
            for (int j = 0; j < reward.Count; j++)
            {
                rewards.Add(reward[j]);
            }
 
        }
        reward_hebin(rewards);
        for (int i = 0; i < rewards.Count; i++)
        {
            GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(-200 + 100 * (i % 5), 55 - 110 * (i / 5), 0);
            obj.transform.localScale = Vector3.one;
            obj.AddComponent<UIDragScrollView>(); 
        }
        m_jewel.transform.parent.gameObject.SetActive(false);
        m_name.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_172_22");//伙伴解雇

 
    }

	public void reset_jiegu_pet(pet pet)
	{
		m_info.text = "";
		flag = true;
		jewel = 200;
		sys._instance.remove_child(m_view);
		m_view.transform.localPosition = Vector3.zero;
		m_view.GetComponent<UIPanel>().clipOffset = Vector3.zero;
		m_jewel.text = sys._instance.get_res_color(2) + "0";
		List<s_t_reward> rewards = pet.get_jiegu_reward();
		for (int i = 0; i < rewards.Count; i++)
		{
			GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
			obj.transform.parent = m_view.transform;
			obj.transform.localPosition = new Vector3(-200 + 100 * (i % 5), 55 - 110 * (i / 5), 0);
			obj.transform.localScale = Vector3.one;
			obj.AddComponent<UIDragScrollView>(); 
		}
		m_jewel.text = sys._instance.get_res_color(2) + "200";
		if(sys._instance.m_self.m_t_player.jewel < 200)
		{
			m_jewel.text = "[ff0000]200";
		}
		m_jewel.transform.parent.gameObject.SetActive(true);
		m_name.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_201_16");//宠物重生
	}

	public void reset_fenjie_pet(pet pet)
	{
		m_info.text = "";
		flag = true;
		jewel = 200;
		sys._instance.remove_child(m_view);
		m_view.transform.localPosition = Vector3.zero;
		m_view.GetComponent<UIPanel>().clipOffset = Vector3.zero;
		m_jewel.text = sys._instance.get_res_color(2) + "0";
		List<s_t_reward> rewards = pet.get_fenjie_reward();
		for (int i = 0; i < rewards.Count; i++)
		{
			GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
			obj.transform.parent = m_view.transform;
			obj.transform.localPosition = new Vector3(-200 + 100 * (i % 5), 55 - 110 * (i / 5), 0);
			obj.transform.localScale = Vector3.one;
			obj.AddComponent<UIDragScrollView>(); 
		}
		m_jewel.text = sys._instance.get_res_color(2) + "200";
		if(sys._instance.m_self.m_t_player.jewel < 200)
		{
			m_jewel.text = "[ff0000]200";
		}
		m_jewel.transform.parent.gameObject.SetActive(true);
		m_name.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_228_16");//宠物分解
	}

	int compare (s_t_reward x, s_t_reward y)
	{
		if (x.type != y.type) 
		{
			return x.type - y.type;

		}
		else
		{
			return x.value2 - y.value2;

		}
	}
    List<s_t_reward> reward_hebin( List<s_t_reward> rewards)
    {
        
        for (int i = 0; i < rewards.Count;i++)
        {
            s_t_reward reward = rewards[i];
            for (int j = i + 1; j < rewards.Count;j++)
            {
                s_t_reward re = rewards[j];
                if ((re.type == reward.type && (re.type == 1 || re.type == 2)) && reward.value1 == re.value1 && reward.value3 != 100)
                {
                    reward.value2 += re.value2;
                    re.value3 = 100;
                }
          
            }
        }
        for (int i = 0; i < rewards.Count;)
        {
            if (rewards[i].value3 == 100)
            {
                rewards.Remove(rewards[i]);
            }
            else
            {
                i++;
            }
        }
		rewards.Sort (compare);
		
        return rewards;
 
    }
    public void reset_equip_fenjie(dhc.equip_t equip1)
    {
		flag = false;
        m_info.text = "";
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = Vector3.zero;
        m_view.GetComponent<UIPanel>().clipOffset = Vector3.zero;
        m_jewel.text = "0";
        List<s_t_reward> rewards = equip.get_equip_fenjie_reward(equip1);
        
        for (int i = 0; i < rewards.Count; i++)
        {
            GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(-200 + 100 * (i % 5), 55 - 110 * (i / 5), 0);
            obj.transform.localScale = Vector3.one;
            obj.AddComponent<UIDragScrollView>(); 
        }
        m_jewel.transform.parent.gameObject.SetActive(false);
        m_name.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_140_22");//装备分解
    }

	public void reset_guanghuan_chongsheng(s_t_guanghuan t_guanghuan)
	{
		m_info.text = "";
		flag = true;
		jewel = 200;
		sys._instance.remove_child(m_view);
		m_view.transform.localPosition = Vector3.zero;
		m_view.GetComponent<UIPanel>().clipOffset = Vector3.zero;
		m_jewel.text = sys._instance.get_res_color(2) + "0";
		List<s_t_reward> rewards = guanghuan_gui.get_guanghuan_chongsheng_reward(t_guanghuan);
		reward_hebin(rewards);
		for (int i = 0; i < rewards.Count; i++)
		{
			GameObject obj = icon_manager._instance.create_reward_icon(rewards[i].type, rewards[i].value1, rewards[i].value2, rewards[i].value3);
			obj.transform.parent = m_view.transform;
			obj.transform.localPosition = new Vector3(-200 + 100 * (i % 5), 55 - 110 * (i / 5), 0);
			obj.transform.localScale = Vector3.one;
			obj.AddComponent<UIDragScrollView>(); 
		}
		m_jewel.text = sys._instance.get_res_color(2) + "200";
		if(sys._instance.m_self.m_t_player.jewel < 200)
		{
			m_jewel.text = "[ff0000]200";
		}
		m_jewel.transform.parent.gameObject.SetActive(true);
		m_name.text = game_data._instance.get_t_language ("common_chongsheng_gui.cs_324_16");//光环重生
	}

    public void click(GameObject obj)
    {
        if (obj.name == "queding")
        {
			if (sys._instance.m_self.m_t_player.jewel < jewel && flag)
            {
                root_gui._instance.show_recharge_dialog_box(hide);
                return;
                   
            }
            cmessage_center._instance.add_message(m_msg);
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (obj.name == "cancel")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
    }
    void hide()
    {
        this.transform.Find("frame_big").GetComponent<frame>().hide();

    }

}
