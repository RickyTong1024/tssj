
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class huiyi_shengxing_yulan : MonoBehaviour {


	public int m_id; //回忆id
	public GameObject m_frame;
	public List<GameObject> m_starnum =new List<GameObject>();//星星
	public List<UILabel> m_desc =new List<UILabel>(); //星星属性；
	public GameObject m_tishi;
	public GameObject m_chongzhi_button;//回忆重置按钮
	public GameObject m_queding_button;
	// Use this for initialization
	public int max_stars =5;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void click(GameObject obj)
	{
		if (obj.name == "queding")
		{
			m_frame.GetComponent<frame>().hide();
		}
		
		if (obj.name == "chongzhi") 
		{
			m_tishi.SetActive(true);
		}

		if (obj.name == "close")
		{
			m_frame.GetComponent<frame>().hide();
			
		}
		if (obj.name == "yes")
		{
			s_message _mes = new s_message();
			_mes.m_type = "huiyi_chongzhi";
			_mes.m_ints.Add(m_id);
			
			cmessage_center._instance.add_message(_mes);
		}
		if (obj.name == "no") 
		{
			m_tishi.SetActive(false);
		}
		
	}
	//获取当前升星数量
	public int get_star_num(int id)
	{
		int m_stars_curnum = 0;
		for (int i = 0; i < sys._instance.m_self.m_t_player.huiyi_jihuos.Count; i++) {
			if(sys._instance.m_self.m_t_player.huiyi_jihuos[i] == id)
			{
				m_stars_curnum = sys._instance.m_self.m_t_player.huiyi_jihuo_starts[i];
			}
		}
		return m_stars_curnum;
	}

	public  void reset()
	{
		s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(m_id);
		string s = "";
		for (int j = 0; j < m_desc.Count; j++) 
		{	
		for (int i = 0; i < _sub.attrs.Count; i++)
		{
			if (i != 0)
			{
				s += "\n";
			}
		s += game_data._instance.get_value_string(_sub.attrs[i],_sub.values[i]+_sub.values2[i]*((float)j+1.0f),1);
				
		}
		
		if(get_star_num(m_id) > 0)
		{
			m_starnum[get_star_num(m_id)-1].GetComponent<UISprite>().set_enable(false);
		}
		
			//设置回忆升星重置按钮出现的条件
		if(get_star_num(m_id) <= 0)
		{
			m_queding_button.transform.localPosition = new Vector3(0,-270,0);
			m_chongzhi_button.SetActive(false);
		}
		string[] s1 = s.Split('+','\n');
		m_desc[j].text = "";

		for (int i = 0; i < _sub.attrs.Count; i++) {
			if(i%2 == 0)
			{
					m_desc[j].text += "[0AF6FF]"+s1[i*2]+"[0aff16]+"+s1[i*2+1]+"    ";
			}
			else
			{
					m_desc[j].text += "[0AF6FF]"+s1[i*2]+"[0aff16]+"+s1[i*2+1]+"\n";
			}
		}
		s = "";
	}	
}
	
}
