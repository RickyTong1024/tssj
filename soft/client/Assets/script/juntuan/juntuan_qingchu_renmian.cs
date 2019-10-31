using UnityEngine;

public class juntuan_qingchu_renmian : MonoBehaviour,IMessage {
    public GameObject m_juntuan_leader;
	public GameObject m_juntuan_senator;
	public GameObject m_juntuan_common;

	void Start () 
	{
        cmessage_center._instance.add_handle(this);
		UIEventListener.Get(m_juntuan_leader).onClick = ButtonClick;
		UIEventListener.Get(m_juntuan_senator).onClick = ButtonClick;
		UIEventListener.Get(m_juntuan_common).onClick = ButtonClick;
	
	}
    void IMessage.message(s_message message)
    {


    }
    void IMessage.net_message(s_net_message message)
    {
       

    }
    void ButtonClick(GameObject obj)
    {
     
        switch (obj.name)
        {
            case "guild_leader":
                break;
            case "guild_senator":
                break;
            case"guild_common":
                break;
        }
 
	}
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
}
