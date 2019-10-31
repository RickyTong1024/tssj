using UnityEngine;

public class arena_max : MonoBehaviour {

	public GameObject m_last_pm;
	public GameObject m_now_pm;
	public GameObject m_ss;
	public GameObject m_jl;

	public int m_rank1;
	public int m_rank2;


    void OnEnable()
    {
        reset();
    }

    void reset()
    {
        m_last_pm.transform.GetComponent<UILabel>().text = m_rank2.ToString();
        m_now_pm.transform.GetComponent<UILabel>().text = m_rank1.ToString();
        m_ss.transform.GetComponent<UILabel>().text = (m_rank2 - m_rank1).ToString();
        m_ss.SetActive(false);
        m_ss.SetActive(true);

        int rr1 = m_rank1;
        int rr2 = m_rank2;
        float jewel = 0;
        for (int i = 0; i < game_data._instance.m_dbc_sport_rank.get_y(); i++)
        {
            s_t_sport_rank t_sport_rank = new s_t_sport_rank();
            t_sport_rank.rank_1 = int.Parse(game_data._instance.m_dbc_sport_rank.get(0, i));
            t_sport_rank.rank_2 = int.Parse(game_data._instance.m_dbc_sport_rank.get(1, i));
            t_sport_rank.zs_pm = float.Parse(game_data._instance.m_dbc_sport_rank.get(4, i));
            int r1 = 0;
            int r2 = 0;
            bool flag = false;
            if (rr1 >= t_sport_rank.rank_1 && rr1 <= t_sport_rank.rank_2)
            {
                r1 = rr1;
                rr1 = t_sport_rank.rank_2 + 1;
            }
            else
            {
                continue;
            }
            if (rr2 >= t_sport_rank.rank_1 && rr2 <= t_sport_rank.rank_2)
            {
                r2 = rr2;
                flag = true;
            }
            else
            {
                r2 = t_sport_rank.rank_2;
            }
            jewel += (r2 - r1 + 1) * t_sport_rank.zs_pm;
            if (flag)
            {
                break;
            }
        }
        int j = (int)jewel;
        if (j <= 0)
        {
            j = 1;
        }
        m_jl.transform.GetComponent<UILabel>().text = sys._instance.get_res_color(2) + "x" + j.ToString();
    }

    public void click(GameObject obj)
    {
        sys._instance.player_need_check();
        transform.GetComponent<ui_show_anim>().hide_ui();
    }

}
