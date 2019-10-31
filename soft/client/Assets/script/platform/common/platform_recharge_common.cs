using UnityEngine;

public class platform_recharge_common
{
    public int m_id;
    public int m_ios_hd_mid;
    public int m_ios_hd_eid;
    public bool m_need_check = false;
    public float m_wait = 0.0f;

    public virtual void init()
    {

    }

    public virtual void update()
    {

    }

    public virtual void do_check()
    {
        m_need_check = true;
        m_wait = 30.0f;
        platform_recharge_object._instance.m_is_wait = true;
    }

    public virtual void time()
    {
        if (!m_need_check)
        {
            return;
        }
        if (m_wait > 0)
        {
            m_wait -= 2;
            protocol.game.cmsg_common_ex _msg = new protocol.game.cmsg_common_ex();
            net_http._instance.send_msg_ex<protocol.game.cmsg_common_ex>(opclient_t.CMSG_RECHARGE_CHECK_EX, _msg);
        }
        else
        {
            platform_recharge_object._instance.m_is_wait = false;
            m_need_check = false;
            root_gui._instance.wait(false);
        }
    }

    public virtual void do_buy(int id, int huodongMid = 0, int huodongEid = 0)
    {
        m_id = id;
        if (Application.isEditor)
        {
            platform_recharge._instance.m_id = id;
            protocol.game.cmsg_recharge _msg = new protocol.game.cmsg_recharge();
            _msg.id = id;
            _msg.huodong_id = huodongMid;
            _msg.entry_id = huodongEid;
            net_http._instance.send_msg<protocol.game.cmsg_recharge>(opclient_t.CMSG_RECHARGE, _msg);
            return;
        }
        buy(id, huodongMid, huodongEid);
    }

    protected virtual void buy(int id, int huodongMid = 0, int huodongEid = 0)
    {

    }

    public virtual void recharge_done(string s)
    {

    }

    public virtual void recharge_cancel(string s)
    {

    }

    public virtual void recharge_product(string s)
    {

    }

    public virtual void recharge_onOderNo(string s)  //Éú³É¶©µ¥
    {

    }

}
