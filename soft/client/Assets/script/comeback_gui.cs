using UnityEngine;
using System.Collections.Generic;
public class comeback_gui : MonoBehaviour,IMessage {

	public GameObject m_view; // 奖励物品的父对象
	public GameObject m_time;
	private int m_select1 = 1;
	private int m_id;
	public GameObject m_buy_num_gui;
	public GameObject m_name;
	public GameObject m_desc;
	public GameObject m_num;
	public GameObject m_input;
	public GameObject m_icon;
	public bool flag = false;
	public GameObject m_zongjia;
	private int m_input_num;  //兑换数量。
	private int total_num;  //还可以兑换的数量
	void Start ()
    {
		cmessage_center._instance.add_handle (this);
	}
	void OnEnable()
	{
		m_select1 = 1;
		InvokeRepeating ("time", 0.0f, 1.0f);
		reset ();
	}

	void IMessage.message (s_message message)
	{
		if (message.m_type == "Huigui") {
			m_id =(int)message.m_ints[0];
			protocol.game.cmsg_huodong_huigui_reward _msg = new protocol.game.cmsg_huodong_huigui_reward ();
			_msg.id = (int)message.m_ints[0];
			_msg.type = (int)message.m_ints[1];
			_msg.num =(int)message.m_ints[2];
			net_http._instance.send_msg<protocol.game.cmsg_huodong_huigui_reward> (opclient_t.CMSG_HUODONG_HUIGUI_REWARD, _msg);
		}
		if(message.m_type =="resert_comeback_gui")
		{
			reset();
		}
		if (message.m_type == "hide_comeback") 
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if (message.m_type == "show_buy_mun_gui") 
		{
			m_id =(int)message.m_ints[0];
			s_t_comeback m_t_comeback = game_data._instance.get_t_comeback(m_id);

			// def1 1 价格 2折扣 3限购数量 4 等级达到才能显示
			buy_resert();
			total_num = m_t_comeback.def3;
			m_input_num = 1;
			int temp_num = 0;
			temp_num =sys._instance.m_self.m_t_player.jewel / m_t_comeback.def1;
			for (int c = 0; c < sys._instance.m_self.m_t_player.huodong_yxhg_fuli_id.Count; c++) 
			{
				if(m_id == sys._instance.m_self.m_t_player.huodong_yxhg_fuli_id[c])
				{
					total_num = temp_num > m_t_comeback.def3 - sys._instance.m_self.m_t_player.huodong_yxhg_fuli_num[c] ? m_t_comeback.def3 - sys._instance.m_self.m_t_player.huodong_yxhg_fuli_num[c] : temp_num;
					break;
				}
				else
				{
					total_num = temp_num > m_t_comeback.def3 ? m_t_comeback.def3 : temp_num;
				}
			}
			buy_resert();
			m_buy_num_gui.SetActive(true);
		}
	}

	void buy_resert()
	{
		s_t_comeback m_t_comeback = game_data._instance.get_t_comeback(m_id);
		
		m_zongjia.SetActive(true);
		m_input.GetComponent<UILabel>().text = m_input_num.ToString ();
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (1,2,0,0);
		m_desc.GetComponent<UILabel>().text = game_data._instance.get_t_language ("jieri_huodong_gui.cs_1005_42") + " " + sys._instance.m_self.get_item_num (1, 2, 1, 0);//[0af6ff]当前拥有:[-]
		m_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次
		string color = sys._instance.get_res_color (2);
		if(sys._instance.m_self.m_t_player.jewel < m_t_comeback.def1 *m_input_num)
		{
			color = "[ff0000]";
		}
		m_zongjia.GetComponent<UILabel>().text = color + "X" +( m_t_comeback.def1 * m_input_num).ToString ();
		sys._instance.remove_child (m_icon);
		GameObject _obj1 = icon_manager._instance.create_reward_icon(1,2, m_t_comeback.def1 * m_input_num,0);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3 (0, 0, 0);
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_HUIGUI_REWARD) 
		{
			s_t_comeback t_comeback =game_data._instance.get_t_comeback(m_id);
			for (int i = 0; i < t_comeback.rewards.Count; i++) {
				if(t_comeback.type ==2)
				{
					if(!sys._instance.m_self.m_t_player.huodong_yxhg_fuli_id.Contains(t_comeback.id ))
                    {
						sys._instance.m_self.m_t_player.huodong_yxhg_fuli_num.Add(0);
						sys._instance.m_self.m_t_player.huodong_yxhg_fuli_id.Add(t_comeback.id);
					}
					sys._instance.m_self.add_reward(t_comeback.rewards[i].type , t_comeback.rewards[i].value1 , t_comeback.rewards[i].value2 *m_input_num , t_comeback.rewards[i].value3 , game_data._instance.get_t_language ("comeback_gui.cs_145_172"));//英雄回归获得
				}
				else
				{
					sys._instance.m_self.add_reward(t_comeback.rewards[i].type , t_comeback.rewards[i].value1 , t_comeback.rewards[i].value2 , t_comeback.rewards[i].value3 , game_data._instance.get_t_language ("comeback_gui.cs_145_172"));//英雄回归获得
				}
			}
			if(t_comeback.type == 1)
			{
				sys._instance.m_self.m_t_player.huodong_yxhg_buzhu_id.Add(m_id);
			}
            else if(t_comeback.type == 3)
			{
				sys._instance.m_self.m_t_player.huodong_yxhg_haoli_id.Add(m_id);
			}
            else if(t_comeback.type == 2)
			{
				for (int i = 0; i < sys._instance.m_self.m_t_player.huodong_yxhg_fuli_id.Count; i++)
                {
					if(t_comeback.id == sys._instance.m_self.m_t_player.huodong_yxhg_fuli_id[i])
                    {
						sys._instance.m_self.m_t_player.huodong_yxhg_fuli_num[i] += m_input_num;
						sys._instance.m_self.m_t_player.jewel -= (t_comeback.def1 * m_input_num);
					}
				}
			}
			reset();
		}
	}
	void OnDestroy()
	{
		CancelInvoke("time");
		cmessage_center._instance.remove_handle (this);
	}

	public void reset()
	{
		int p = 0;
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(367, -23, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);
        List<int> _ids = new List<int>();
		
		for (int i = 0; i < game_data._instance.m_dbc_comeback.get_y(); i++)
        {
			int id = int.Parse(game_data._instance.m_dbc_comeback.get(0, i));
			s_t_comeback t_comeback =game_data._instance.get_t_comeback(id);
			if(m_select1 == 1)
			{
		        if(t_comeback.type == 1)
		        {
			        _ids.Add(id);
		        }
			}
			else if(m_select1 == 2)
			{
				if(sys._instance.m_self.m_t_player.level < t_comeback.def4)
				{
                    continue;
				}
				else
                {
			        if(t_comeback.type == 2)
			        {
				        _ids.Add(id);
			        }
				}
			}
			else if(m_select1 == 3)
			{
				if(t_comeback.type == 3)
				{
					_ids.Add(id);
				}
			}
		}

		if (m_select1 == 3)
        {  //排序
			_ids.Sort (compare2);
		}
		else if (m_select1 == 1) {
			_ids.Sort (compare);
		}

		for (int j = 0; j < _ids.Count; j++)
        {
			s_t_comeback t_comeback = game_data._instance.get_t_comeback(_ids[j]);
			if(t_comeback.type ==1)
			{
				int temp = sys._instance.m_self.is_finish_huigui_buzhu(t_comeback.id);	
				GameObject _obj = game_data._instance.ins_object_res("ui/hero_comeback_sub");
				_obj.transform.parent = m_view.transform;
				_obj.transform.localScale = new Vector3(1, 1, 1);
				_obj.transform.localPosition = new Vector3(40, 52 - p * 146, 0);
				_obj.GetComponent<comeback_sub>().m_id = t_comeback.id;
				_obj.GetComponent<comeback_sub>().reset(temp);
				sys._instance.add_pos_anim(_obj, 0.3f, new Vector3(-300, 0, 0), p * 0.05f);
				sys._instance.add_alpha_anim(_obj, 0.3f, 0, 1.0f, p * 0.05f);
			}
			else if(t_comeback.type == 2)
			{
				GameObject _obj = game_data._instance.ins_object_res("ui/hero_comeback_sub_ex");
				
				_obj.transform.parent = m_view.transform;
				_obj.transform.localScale = new Vector3(1, 1, 1);
				_obj.transform.localPosition = new Vector3(40, 52 - p * 135, 0);
				_obj.GetComponent<comeback_sub_ex>().m_id = t_comeback.id;
				_obj.GetComponent<comeback_sub_ex>().rewards = t_comeback.rewards;
				for (int i = 0; i < sys._instance.m_self.m_t_player.huodong_yxhg_fuli_id.Count; i++)
                {
					if(t_comeback.id == sys._instance.m_self.m_t_player.huodong_yxhg_fuli_id[i])
					{
						_obj.GetComponent<comeback_sub_ex>().m_num = sys._instance.m_self.m_t_player.huodong_yxhg_fuli_num[i] ;
						break;
					}
                    else
                    {
						_obj.GetComponent<comeback_sub_ex>().m_num =0;
					}
				}
				_obj.GetComponent<comeback_sub_ex>().reset();
				sys._instance.add_pos_anim(_obj, 0.3f, new Vector3(-300, 0, 0), p * 0.05f);
				sys._instance.add_alpha_anim(_obj, 0.3f, 0, 1.0f, p * 0.05f);
			}
            else if(t_comeback.type == 3)
			{
				int temp = sys._instance.m_self.is_finish_huigui_haoli(t_comeback.id);	
				
				GameObject _obj = game_data._instance.ins_object_res("ui/hero_comeback_sub");
				
				_obj.transform.parent = m_view.transform;
				_obj.transform.localScale = new Vector3(1, 1, 1);
				_obj.transform.localPosition = new Vector3(40, 52 - p * 146, 0);
				_obj.GetComponent<comeback_sub>().m_id = t_comeback.id;
				_obj.GetComponent<comeback_sub>().m_num = sys._instance.m_self.m_t_player.huodong_yxhg_rmb;  //设置当天充值数目
				_obj.GetComponent<comeback_sub>().reset(temp);
				
				sys._instance.add_pos_anim(_obj, 0.3f, new Vector3(-300, 0, 0), p * 0.05f);
				sys._instance.add_alpha_anim(_obj, 0.3f, 0, 1.0f, p * 0.05f);
			}
            p++;
		}
	}
	int compare(int x,int y)
	{ 
		int flagX = sys._instance.m_self.is_finish_huigui_buzhu(x);
		int flagY = sys._instance.m_self.is_finish_huigui_buzhu(y);
		if (flagX > flagY)
        {
			return 1;
		}
        else if (flagX < flagY)
        {
			return -1;
		}
        else 
		{
			return x - y;
		}
	}
	int compare2(int x,int y)
	{ 
		int flagX = sys._instance.m_self.is_finish_huigui_haoli (x);
		int flagY = sys._instance.m_self.is_finish_huigui_haoli(y);
		if (flagX > flagY)
        {
			return 1;
		}
        else if (flagX < flagY)
        {
			return -1;
		}
        else 
		{
			return x - y;
		}
	}


	public void click(GameObject obj)
	{
		if (obj.transform.name == "buzhu") 
		{
			m_select1 = 1;
			reset();
		}
		if (obj.transform.name == "fuli") 
		{
			m_select1 = 2;
			reset();
		}
		if (obj.transform.name == "haoli") 
		{
			m_select1 = 3;
			reset();
		}
		if (obj.transform.name == "close") 
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message ();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message (_message);
		}

		if (obj.name == "add") {
			if (m_input_num + 1 <= total_num)
            {
				m_input_num += 1;
				buy_resert ();
			}
		}
        else if (obj.name == "sub")
        {
			if (m_input_num > 1) {
				m_input_num--;
				buy_resert ();
			}
		}
        else if (obj.name == "add10")
        {
			if (m_input_num + 10 <= total_num)
            {
				m_input_num += 10;
			}
            else
            {
				m_input_num = total_num;
			}
			buy_resert ();
		}
        else if (obj.name == "sub10")
        {
			if (m_input_num - 10 >= 1)
            {
				m_input_num -= 10;
			}
            else
            {
				m_input_num = 1;
			}
			buy_resert ();
		}
        else if (obj.name == "queding")
        {
			s_t_comeback m_t_comebacks = game_data._instance.get_t_comeback (m_id);
			m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide ();
			protocol.game.cmsg_huodong_huigui_reward _msg = new protocol.game.cmsg_huodong_huigui_reward ();
			_msg.id = m_t_comebacks.id;
			_msg.type = m_t_comebacks.type;
			_msg.num = m_input_num;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_huigui_reward> (opclient_t.CMSG_HUODONG_HUIGUI_REWARD, _msg);
		}
        else if (obj.name == "hide")
        {
			m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide ();
		}
        else if (obj.name == "cancle")
        {
			m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide ();
		}
	}
	void time()
	{	
		long _time = 0;
		int run_day = timer.run_day (sys._instance.m_self.m_t_player.huodong_yxhg_time) + 1;
		if (run_day <= 3)
		{
			_time = (3 - run_day) * 86400000 + timer.last_time_today();
		}
		string _text = timer.get_time_show_color_ex(_time,"[0AFF16]", "[0AF6FF]");
		m_time.GetComponent<UILabel>().text = _text;
	}
}
