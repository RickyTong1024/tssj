using UnityEngine;

public class baowu_yjsaodang_select : MonoBehaviour
{

    public GameObject m_des;
    public GameObject m_gou;
    private int m_bid;
    private bool m_show;

    public void init(int bid)
    {
        m_bid = bid;
        m_show = true;
        m_des.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("baowu_yjsaodang_select.cs_16_53"), sys._instance.get_res_info(6, m_bid, 0, 0));//确定抢夺{0}[-]的全部碎片吗？
        m_gou.SetActive(true);
    }

    public void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        if (obj.name == "ok")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();

            protocol.game.cmsg_treasure_yijian_saodang _msg1 = new protocol.game.cmsg_treasure_yijian_saodang();
            _msg1.treasure_id = m_bid;
            _msg1.use_yaosui = m_show;
            net_http._instance.send_msg<protocol.game.cmsg_treasure_yijian_saodang>(opclient_t.CMSG_TREASURE_YIJIAN_SAODANG, _msg1);
        }
        if (obj.name == "kuang")
        {
            m_show = !m_show;
            m_gou.SetActive(m_show);
        }
    }
}
