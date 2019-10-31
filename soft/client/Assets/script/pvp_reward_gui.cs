
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class pvp_reward_gui : MonoBehaviour {

    public GameObject m_scro;
    public GameObject m_view;
    public UIToggle m_yulan;

    public protocol.game.smsg_rank_view _msg;
     protocol.game.smsg_pvp_view m_msg;
    public GameObject m_desc1;
    public GameObject m_desc2;
    public GameObject m_top;
    public GameObject m_top1;
    private List<ulong> m_guids = new List<ulong>();
    // Use this for initialization
    void Start()
    {

    }

    public void reset_yulan()
    {
        m_top.SetActive(false);
        m_top1.SetActive(true);
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);
        for (int i = 0; i < game_data._instance.m_dbc_pvp_reward.get_y(); i++)
        {
            int _id = int.Parse(game_data._instance.m_dbc_pvp_reward.get(0, i));
            s_t_pvp_reward t_pvp_reward = game_data._instance.get_t_pvp_reward(_id);
            GameObject temp = game_data._instance.ins_object_res("ui/pvp_item");
            if (t_pvp_reward.id1 == t_pvp_reward.id2)
            {
                temp.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"),t_pvp_reward.id1.ToString());//第{0}名

            }
            else
            {
                temp.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_182_82"),//第{0}-{1}名
                    t_pvp_reward.id1.ToString() ,t_pvp_reward.id2);

            }

            temp.transform.parent = m_view.transform;
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.transform.localPosition = new Vector3(0, -i * 119 + 101, 0);
            sys._instance.remove_child(temp.transform.Find("icon").gameObject);
            for (int c = 0; c < t_pvp_reward.rewards.Count; c++)
            {
                GameObject _icon = icon_manager._instance.create_reward_icon(t_pvp_reward.rewards[c].type, 
                    t_pvp_reward.rewards[c].value1, t_pvp_reward.rewards[c].value2, t_pvp_reward.rewards[c].value3);
                _icon.transform.parent = temp.transform.Find("icon");
                _icon.transform.localScale = new Vector3(1, 1, 1);
                _icon.transform.localPosition = new Vector3(c * 100, 0, 0);
            }
            if (t_pvp_reward.rewards.Count == 4 || t_pvp_reward.rewards.Count == 5)
            {
                temp.transform.Find("icon").localPosition = new Vector3(-100, 0, 0);
            }
            else
            {
                temp.transform.Find("icon").localPosition = new Vector3(50 - t_pvp_reward.rewards.Count * 50, 0, 0);
 
            }

        }
        m_desc1.SetActive(true);
        m_desc2.SetActive(false);
    }
    public void reset_hurt(int type)
    {
        m_top.SetActive(true);
        m_top1.SetActive(false);
        m_guids.Clear();
        if (m_scro.GetComponent<SpringPanel>() != null)
        {
            m_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_scro);
        if (type == 1)
        {
            for (int i = 0; i < _msg.rank_list.player_name.Count; i++)
            {
                GameObject _obj = game_data._instance.ins_object_res("ui/boss_list_sub");
                _obj.transform.name = i.ToString();
                _obj.transform.parent = m_scro.transform;
                _obj.transform.localScale = new Vector3(1, 1, 1);
                _obj.transform.localPosition = new Vector3(0, 141 - i * 68, 1);

                int _rank = i + 1;
                string _rank_show = _rank.ToString();
                GameObject _rank_obj = _obj.transform.Find("rank").gameObject;

                if (_rank == 1)
                {
                    _rank_show = game_data._instance.get_t_language ("look_jl_gui.cs_99_18");//第1名
                    //_rank_obj.GetComponent<UILabel>().color = new Color(1,0,0);
                }
                else if (_rank == 2)
                {
                    _rank_show = game_data._instance.get_t_language ("look_jl_gui.cs_104_18");//第2名
                    //_rank_obj.GetComponent<UILabel>().color = new Color(1,0.8f,0.4f);
                }
                else if (_rank == 3)
                {
                    _rank_show = game_data._instance.get_t_language ("look_jl_gui.cs_109_18");//第3名
                }
                m_guids.Add(_msg.rank_list.player_guid[i]);
                _rank_obj.GetComponent<UILabel>().text = _rank_show;
                _obj.transform.Find("look").GetComponent<UIButtonMessage>().target = this.gameObject;
                _obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(_msg.rank_list.player_achieve[i]) + _msg.rank_list.player_name[i].ToString();
                _obj.transform.Find("level").GetComponent<UILabel>().text = sys._instance.get_server(_msg.rank_list.player_level[i]).m_name.ToString();
                _obj.transform.Find("attack").GetComponent<UILabel>().text = sys._instance.value_to_wan(_msg.rank_list.player_bf[i]);
                _obj.transform.Find("jf").GetComponent<UILabel>().text = sys._instance.value_to_wan(_msg.rank_list.value[i]);
                _obj.transform.Find("look").gameObject.SetActive(false);
				sys._instance.get_chenghao(_msg.rank_list.player_chenghao[i], _obj.transform.Find("chenhao").gameObject);

                
                GameObject m_icon = _obj.transform.Find("icon").gameObject;
                sys._instance.remove_child(m_icon);
                GameObject _obj1 = icon_manager._instance.create_player_icon((int)_msg.rank_list.player_template[i], _msg.rank_list.player_achieve[i], _msg.rank_list.player_vip[i],_msg.rank_list.player_nalflag[i]);

                _obj1.transform.parent = m_icon.transform;
                _obj1.transform.localScale = new Vector3(1, 1, 1);
                _obj1.transform.localPosition = new Vector3(0, 0, 0);
            }

        }
       

        m_desc1.SetActive(false);
        m_desc2.SetActive(true);
        int rank = get_rank();
        if (rank != -1)
        {

            m_desc2.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"),(rank + 1));//第{0}名
            m_desc2.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count)
                + sys._instance.m_self.m_t_player.name;
            m_desc2.transform.Find("hurt").GetComponent<UILabel>().text = sys._instance.value_to_wan(sys._instance.m_self.m_t_player.bf) + "";
            m_desc2.transform.Find("jf").GetComponent<UILabel>().text = sys._instance.value_to_wan(_msg.rank_list.value[rank]) + "";
        }
        else
        {
            m_desc2.transform.Find("rank").GetComponent<UILabel>().text = game_data._instance.get_t_language ("bingyuan_rank_gui.cs_83_81");//未上榜
            m_desc2.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count)
                + sys._instance.m_self.m_t_player.name;
            m_desc2.transform.Find("jf").GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.pvp_total + "";
            
            m_desc2.transform.Find("hurt").GetComponent<UILabel>().text = sys._instance.value_to_wan(sys._instance.m_self.m_t_player.bf) + "";
        }
        GameObject m_icon1 = m_desc2.transform.Find("icon").gameObject;
        sys._instance.remove_child(m_icon1);
        GameObject _obj2 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id, sys._instance.m_self.m_t_player.dress_achieves.Count,
                                                                    sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);

        _obj2.transform.parent = m_icon1.transform;
        _obj2.transform.localScale = new Vector3(1, 1, 1);
        _obj2.transform.localPosition = new Vector3(0, 0, 0);
        m_desc2.transform.Find("level").GetComponent<UILabel>().text = sys._instance.m_sname.ToString();
		GameObject chenghao = m_desc2.transform.Find("chenhao").gameObject;
		sys._instance.get_chenghao(sys._instance.m_self.m_t_player.chenghao_on,chenghao);
    }
 
   public  int get_rank()
    {
        for (int i = 0; i < _msg.rank_list.player_guid.Count; i++)
        {
            if (sys._instance.m_self.m_t_player.guid == _msg.rank_list.player_guid[i])
            {
                return i;
            }
        }
        return -1;
    }
    public void click(GameObject obj)
    {
        if (obj.transform.name == "close")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
            m_yulan.value = true;
        }
        else if (obj.transform.name == "yulan")
        {
            reset_yulan();
        }
        else if (obj.transform.name == "hurt")
        {
            reset_hurt(1);

        }
        else if (obj.transform.name == "top")
        {
            reset_hurt(2);
        }
        else if (obj.transform.name == "look")
        {
            int id = int.Parse(obj.transform.parent.name);
            protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look();
            _msg.target_guid = m_guids[id];
            net_http._instance.send_msg<protocol.game.cmsg_player_look>(opclient_t.CMSG_PLAYER_LOOK, _msg);
        }
    }

}
