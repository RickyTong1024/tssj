
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class huiyi_sub : MonoBehaviour {

    public UILabel m_name;
    public GameObject m_finish;
    public GameObject m_icon_parent;
    public UILabel m_desc;
    public int m_id;
	public GameObject m_button;
	public GameObject m_button_sx;//升星按钮
	public GameObject m_mask;
	public GameObject m_sxyulan;//属性预览
	public List<GameObject> m_starnum =new List<GameObject>();//星星
	public GameObject star_panel;
	//public GameObject  m_huiyi_yulan_panle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void click(GameObject obj)
    {
        if (obj.name == "jihuo")
        {
            s_message _mes = new s_message();
            _mes.m_type = "huiyi_jiesuo";
            _mes.m_ints.Add(m_id);
            cmessage_center._instance.add_message(_mes);
 
        }

		if (obj.name == "shengxing") 
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_huiyi_shengxing)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("huiyi_sub.cs_42_59"));//回忆升星110级开启，请主人努力提升等级
				return;
			}
			s_message _mes =new s_message();
			_mes.m_type = "huiyi_shengxing";
			_mes.m_ints.Add(m_id);
			cmessage_center._instance.add_message(_mes);
		}
		if (obj.name == "yulan") 
		{
			s_message _mes = new s_message();
			_mes.m_type = "huiyi_yulan";
			_mes.m_ints.Add(m_id);

			cmessage_center._instance.add_message(_mes);
		}
 
    }
    void tp_click(GameObject obj)
    {
        int id = obj.transform.GetComponent<item_icon>().m_item_id;
        s_message message = new s_message();
        message.m_type = "show_cl_gui";
        message.m_ints.Add(id);
        cmessage_center._instance.add_message(message);
    }

	public int get_star_num(int id)
	{

		int m_stars_curnum = 0;
		for (int i = 0; i < sys._instance.m_self.m_t_player.huiyi_jihuos.Count; i++) {
			if(sys._instance.m_self.m_t_player.huiyi_jihuos[i] == id)
			{
				m_stars_curnum =sys._instance.m_self.m_t_player.huiyi_jihuo_starts[i];
			}
		}
		return m_stars_curnum;
	}
   public  void reset()
	{	
        s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(m_id);
        m_name.text = _sub.name;
		string s = "";
	
        for (int i = 0; i < _sub.attrs.Count; i++)
        {
			if (i != 0)
			{
				s += "\n";
			}
			s += game_data._instance.get_value_string(_sub.attrs[i],_sub.values[i]+_sub.values2[i]*((float)get_star_num(m_id)),1);
        }
		string[] s1 = s.Split('\n');;

		m_desc.text = "";
		for (int i = 0; i < _sub.attrs.Count; i++) {

			{
				m_desc.text += "[0AF6FF]" + s1[i] +"\n";
			}
		}
	
        int temp = sys._instance.m_self.is_huiyi_finish(m_id);
        sys._instance.remove_child(m_icon_parent);
		int num = 5 - _sub.huiyis.Count;
        for (int i = 0; i < _sub.huiyis.Count; i++)
        {
            GameObject obj = icon_manager._instance.create_item_icon(_sub.huiyis[i], true);
            obj.transform.parent = m_icon_parent.transform;
            obj.transform.localPosition = new Vector3((90 * i + num * 47) -30, -17f, 0);
            obj.transform.localScale = Vector3.one;
			obj.GetComponent<item_icon>().reset ();

            if (temp < 5)
            {
                if (!sys._instance.m_self.m_t_player.item_ids.Contains((uint)_sub.huiyis[i]))
                {
                    GameObject obj1 = (GameObject)Instantiate(m_mask);
                    obj1.transform.parent = m_icon_parent.transform;
					obj1.transform.localPosition = new Vector3((90 * i + num * 47) -30, -17, 0);
					obj1.transform.localScale = Vector3.one;
                    obj1.SetActive(true);

                }

            }
			
        }
		if (temp == 5) 
		{
			m_button.SetActive(false);
			m_finish.SetActive(true);
			star_panel.SetActive(true);
			for (int i = 0; i < get_star_num(m_id); i++) {

				//	m_starnum[i].GetComponent<UISprite>().set_enable(false);
				m_starnum[i].GetComponent<UISprite>().set_enable(false);	

			}
			m_button_sx.SetActive(false);
			m_sxyulan.SetActive(true);
		}
        else if (temp == 2 || temp == 4) //可以升星 或者
        {
            m_button.SetActive(false);
			star_panel.SetActive(true);

			for (int i = 0; i < get_star_num(m_id); i++) {
				m_starnum[i].GetComponent<UISprite>().set_enable(false);	
			}



			m_button_sx.SetActive(true);
			m_sxyulan.SetActive(true);
            
        }
        else if(temp == 3) //未激活
        {
            m_button.SetActive(true);
            m_button.GetComponent<UISprite>().set_enable(false);
            m_finish.SetActive(false);
			m_button_sx.SetActive(false);
			
        }
        else if (temp == 1) //可以激活
        {
            m_button.SetActive(true);
            m_button.GetComponent<UISprite>().set_enable(true);
            m_finish.SetActive(false);
			m_button_sx.SetActive(false);
			
 
        }
    }
   
}
