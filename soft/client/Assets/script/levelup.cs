
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class levelup : MonoBehaviour {
	
	public GameObject m_plevel;
	public GameObject m_nlevel;
	public GameObject m_ptl;
	public GameObject m_tl;
	public GameObject m_cptl;
	public GameObject m_ctl;
	public GameObject m_cpts;
	public GameObject m_cts;
	public GameObject m_leve_icon;
	public GameObject m_level_icon1;
	public GameObject m_name;
	public GameObject m_name1;
	public GameObject m_kaqi;
	public GameObject m_kaqi1;

	public void reset(int ptili, int pts)
	{
		sys._instance.play_sound ("sound/wjsj01");
		this.transform.GetComponent<Animator>().Play ("levelup");
		int level = sys._instance.m_self.get_att(e_player_attr.player_level);
		s_t_exp t_exp = game_data._instance.get_t_exp (level);
		s_t_exp t_exp1 = game_data._instance.get_t_exp (level - 1);
		s_t_vip t_vip = game_data._instance.get_t_vip (sys._instance.m_self.m_t_player.vip);
		m_nlevel.transform.GetComponent<UILabel>().text = level.ToString();
		m_plevel.transform.GetComponent<UILabel>().text =  (level - 1).ToString();
		m_cptl.transform.GetComponent<UILabel>().text = ptili.ToString();
		m_ctl.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.tili.ToString();
		m_cpts.transform.GetComponent<UILabel>().text = pts.ToString();
		m_cts.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.energy.ToString();
		gongneng_kaiqi ();
	}

	public void gongneng_kaiqi()
	{
		List<int> levels = new List<int>();
		for(int i = 0; i < game_data._instance.m_dbc_exp.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_exp.get(0,i));
			s_t_exp _t_exp = game_data._instance.get_t_exp(id);
			if(_t_exp.icon != "0" || _t_exp.icon2 != "0")
			{
				levels.Add(_t_exp.level);
			}
		}
		bool flag = false;
		for(int i = 0; i < levels.Count ;++i)
		{
			if(sys._instance.m_self.m_t_player.level <= levels[i])
			{
				s_t_exp _t_exp = game_data._instance.get_t_exp(levels[i]);
				m_leve_icon.transform.GetComponent<UISprite>().spriteName = _t_exp.icon;
				m_name.GetComponent<UILabel>().text = _t_exp.desc;
				m_kaqi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("levelup.cs_77_42");//[已开启]
				m_kaqi.GetComponent<UILabel>().gradientTop = new Color(1.0f,1.0f,1.0f);
				m_kaqi.GetComponent<UILabel>().gradientBottom = new Color(85/255f,239/255f,42/255f);
				if(i+1 >= levels.Count)
				{
					s_t_exp t_t_exp = game_data._instance.get_t_exp(levels[i-1]);
					m_leve_icon.transform.GetComponent<UISprite>().spriteName = t_t_exp.icon;
					m_name.GetComponent<UILabel>().text = t_t_exp.desc;
				}
				else if(sys._instance.m_self.m_t_player.level < _t_exp.level )
				{
					m_kaqi.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("levelup.cs_88_57"), _t_exp.level);//[{0}级开启]
					m_kaqi.GetComponent<UILabel>().gradientBottom = new Color(1.0f,51/255f,0);
				}
				if(_t_exp.icon2 != "0")
				{
					m_level_icon1.transform.GetComponent<UISprite>().spriteName = _t_exp.icon2;
					m_kaqi1.GetComponent<UILabel>().text = game_data._instance.get_t_language ("levelup.cs_77_42");//[已开启]
					m_kaqi1.GetComponent<UILabel>().gradientTop = new Color(1.0f,1.0f,1.0f);
					m_kaqi1.GetComponent<UILabel>().gradientBottom = new Color(85/255f,239/255f,42/255f);
					m_name1.GetComponent<UILabel>().text = _t_exp.desc2;
					if(sys._instance.m_self.m_t_player.level < _t_exp.level)
					{
                        m_kaqi1.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("levelup.cs_88_57"), _t_exp.level);//[{0}级开启]
						m_kaqi1.GetComponent<UILabel>().gradientBottom = new Color(1.0f,51/255f,0);
					}
				}
				else
				{
					int num = i+1;
					if(num >= levels.Count)
					{
						num = levels.Count -1;
					}
					if(sys._instance.m_self.m_t_player.level >= levels[num] )
					{
						s_t_exp t_t_exp = game_data._instance.get_t_exp(levels[num]);
						m_level_icon1.transform.GetComponent<UISprite>().spriteName = t_t_exp.icon;
						m_kaqi1.GetComponent<UILabel>().text = game_data._instance.get_t_language ("levelup.cs_77_42");//[已开启]
						m_kaqi1.GetComponent<UILabel>().gradientTop = new Color(1.0f,1.0f,1.0f);
						m_kaqi1.GetComponent<UILabel>().gradientBottom = new Color(85/255f,239/255f,42/255f);
						m_name1.GetComponent<UILabel>().text = t_t_exp.desc;
					}
					else
					{
						s_t_exp t_t_exp = game_data._instance.get_t_exp(levels[num]);

						m_level_icon1.transform.GetComponent<UISprite>().spriteName = t_t_exp.icon;
						m_kaqi1.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("levelup.cs_88_57"), t_t_exp.level);//[{0}级开启]
						m_kaqi1.GetComponent<UILabel>().gradientBottom = new Color(1.0f,51/255f,0);
						m_kaqi1.GetComponent<UILabel>().gradientTop = new Color(1.0f,1.0f,1.0f);
						m_name1.GetComponent<UILabel>().text = t_t_exp.desc;
					}
				}
				flag = true;
				break;
			}
		}
		if(!flag)
		{
			int id = levels.Count -1;
			s_t_exp _t_exp = game_data._instance.get_t_exp(levels[id]);
			if(_t_exp.icon2 != "0")
			{
				m_leve_icon.transform.GetComponent<UISprite>().spriteName = _t_exp.icon;
				m_name.GetComponent<UILabel>().text = _t_exp.desc;
				m_kaqi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("levelup.cs_77_42");//[已开启]
				m_kaqi.GetComponent<UILabel>().gradientTop = new Color(1.0f,1.0f,1.0f);
				m_kaqi.GetComponent<UILabel>().gradientBottom = new Color(85/255f,239/255f,42/255f);
				m_level_icon1.transform.GetComponent<UISprite>().spriteName = _t_exp.icon2;
				m_kaqi1.GetComponent<UILabel>().text = game_data._instance.get_t_language ("levelup.cs_77_42");//[已开启]
				m_kaqi1.GetComponent<UILabel>().gradientTop = new Color(1.0f,1.0f,1.0f);
				m_kaqi1.GetComponent<UILabel>().gradientBottom = new Color(85/255f,239/255f,42/255f);
				m_name1.GetComponent<UILabel>().text = _t_exp.desc2;
			}
			else
			{
				id = levels.Count -2;
				_t_exp = game_data._instance.get_t_exp(levels[id]);
				m_leve_icon.transform.GetComponent<UISprite>().spriteName = _t_exp.icon;
				m_kaqi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("levelup.cs_77_42");//[已开启]
				m_kaqi.GetComponent<UILabel>().gradientTop = new Color(1.0f,1.0f,1.0f);
				m_kaqi.GetComponent<UILabel>().gradientBottom = new Color(85/255f,239/255f,42/255f);
				m_name.GetComponent<UILabel>().text = _t_exp.desc;
				id = levels.Count -1;
				_t_exp = game_data._instance.get_t_exp(levels[id]);
				m_level_icon1.transform.GetComponent<UISprite>().spriteName = _t_exp.icon;
				m_kaqi1.GetComponent<UILabel>().text = game_data._instance.get_t_language ("levelup.cs_77_42");//[已开启]
				m_kaqi1.GetComponent<UILabel>().gradientTop = new Color(1.0f,1.0f,1.0f);
				m_kaqi1.GetComponent<UILabel>().gradientBottom = new Color(85/255f,239/255f,42/255f);
				m_name1.GetComponent<UILabel>().text = _t_exp.desc;
			}
		}
	}

	public void click(GameObject obj)
	{
		int level = sys._instance.m_self.get_att(e_player_attr.player_level);
		s_t_exp t_exp = game_data._instance.get_t_exp (level);
		if (obj.name == "ok")
		{
			this.gameObject.SetActive(false);
            if(game_data._instance.m_guaji == 0)
            {
                if (t_exp.tg != "0")
                {
                    if (sys._instance.m_game_state != "hall")
                    {
                        sys._instance.m_game_state = "hall";
                        sys._instance.load_scene(sys._instance.m_hall_name);
                    }
                    s_message _mes = new s_message();
                    _mes.m_type = "return_to_main";
                    cmessage_center._instance.add_message(_mes);

                    root_gui._instance.action_guide(t_exp.tg);
                }
            }
		}
		else
		{
			this.gameObject.SetActive(false);
            if (game_data._instance.m_guaji == 0)
            {
                if (t_exp.tg != "0")
                {
                    if (sys._instance.m_game_state != "hall")
                    {
                        sys._instance.m_game_state = "hall";
                        sys._instance.load_scene(sys._instance.m_hall_name);
                    }
                    s_message _mes = new s_message();
                    _mes.m_type = "return_to_main";
                    cmessage_center._instance.add_message(_mes);

                    root_gui._instance.action_guide(t_exp.tg);
                }
            }
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
