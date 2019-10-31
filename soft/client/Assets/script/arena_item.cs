using UnityEngine;

public class arena_item : MonoBehaviour {

	public int m_id;
	public GameObject m_icom;
	public protocol.game.msg_sport_player _player;
	public GameObject m_zwuci;

    public void reset(bool gao)
    {
        GameObject sub = this.transform.Find("item").gameObject;
        Transform _name = sub.transform.Find("name").transform;
        Transform _fighting = sub.transform.Find("fighting/fighting").transform;
        Transform _ranking = sub.transform.Find("ranking").transform;
        Transform fighting = sub.transform.Find("fighting").transform;
        Transform back = this.transform;
        string name = game_data._instance.get_name_color(_player.player_achieve) + System.Text.Encoding.UTF8.GetString(_player.player_name, 0, _player.player_name.Length);
        back.GetComponent<UISprite>().spriteName = "xnjjc_xwjsk001";
        if (name == sys._instance.m_self.m_t_player.name)
        {
            back.GetComponent<UISprite>().spriteName = "xnjjc_xwjsk002";
        }
        sys._instance.remove_child(m_icom);
        GameObject _obj1 = icon_manager._instance.create_player_icon((int)_player.player_template, _player.player_achieve, _player.player_vip, _player.player_nalflag);

        _obj1.transform.parent = m_icom.transform;
        _obj1.transform.localScale = new Vector3(1, 1, 1);
        _obj1.transform.localPosition = new Vector3(0, 0, 0);
        _name.GetComponent<UILabel>().text = name;
        _fighting.GetComponent<UILabel>().text = _player.player_bat_eff.ToString();
        if (_player.player_bat_eff >= 1000000)
        {
            _fighting.GetComponent<UILabel>().text = sys._instance.value_to_wan(_player.player_bat_eff);
        }
        _ranking.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_179_81"), _player.player_rank.ToString());//第{0}名
        fighting.GetComponent<UILabel>().text = game_data._instance.get_t_language("arena_item.cs_42_42");//战斗力
        if (arena.is_min(_player))
        {
            back.GetComponent<UISprite>().spriteName = "xnjjc_xwjsk002";
        }
        if (gao)
        {
            m_zwuci.SetActive(false);
        }
        else if (sys._instance.m_self.m_t_player.level < (int)e_open_see.es_sport_zhan5ci && sys._instance.m_self.m_t_player.vip < (int)e_open_vip.ev_sport_zhan5ci)
        {
            m_zwuci.SetActive(false);
        }
    }

    public void select()
    {
        s_message _mes1 = new s_message();
        _mes1.m_type = "select_arena_item";
        _mes1.m_ints.Add(m_id);
        cmessage_center._instance.add_message(_mes1);
    }

    public void zwuci()
    {
        s_message _mes1 = new s_message();
        _mes1.m_type = "select_arena_item1";
        _mes1.m_ints.Add(m_id);
        cmessage_center._instance.add_message(_mes1);
    }
}
