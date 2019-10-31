using System.Collections.Generic;
using UnityEngine;

public class guild_alloc_gui : MonoBehaviour ,IMessage{

	// Use this for initialization
    public GameObject m_scrollow;
    public List<alloc_item> m_items = new List<alloc_item>();
    public GameObject m_alloc_buttons;
  //  protocol.game.smsg_guild_reward_look m_msg; 
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	}
	
	
    void OnEnable()
    {
        protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
       // net_http._instance.send_msg_ex<protocol.game.cmsg_common>(opclient_t.CMSG_GUILD_REWARD_LOOK, _msg);
    }
     void IMessage.message(s_message message)
    {
        
       
    }
     public void click(GameObject obj)
     {
         if (juntuan_gui._instance.m_zhiwu_t == (int)e_guild_member_type.e_member_type_leader)
         {
          //   protocol.game.cmsg_guild_reward_auto_fenpei _msg = new protocol.game.cmsg_guild_reward_auto_fenpei();
          //   _msg.type = 1;
           //  net_http._instance.send_msg_ex<protocol.game.cmsg_guild_reward_auto_fenpei>(opclient_t.CMSG_GUILD_REWARD_AUTO_FENPEI, _msg);

         }
 
     }
     void reset()
     {
         m_items.Clear();
         GameObject obj = game_data._instance.ins_object_res("allcok_reward");
         alloc_item _item = new alloc_item();
         obj.GetComponent<guild_alloc_item>().m_item = _item;
         m_items.Add(_item);
         obj.transform.parent = m_scrollow.transform;
         obj.transform.localPosition = Vector3.zero;
         obj.transform.localScale = Vector3.one;

     }
     void IMessage.net_message(s_net_message message)
     {
         //if (message.m_opcode == opclient_t.CMSG_GUILD_REWARD_LOOK)
         //{
         //     m_msg = net_http._instance.parse_packet<protocol.game.smsg_guild_reward_look>(message.m_byte);

         //}
     }
     void OnDestroy()
     {
         cmessage_center._instance.remove_handle(this);
     }
}
public class alloc_item
{
    public string name;
    public int attack;
    public int value;
}

