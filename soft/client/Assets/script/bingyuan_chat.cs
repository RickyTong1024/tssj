using System.Collections.Generic;
using UnityEngine;

public class bingyuan_chat : MonoBehaviour
{
    public List<GameObject> m_chat_labels;
    private ulong m_hanhua_time;

    private void Start()
    {
        for (int i = 0; i < m_chat_labels.Count; ++i)
        {
            m_chat_labels[i].GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_chat_" + i.ToString());
        }
    }

    public void click(GameObject obj)
    {
        if (m_hanhua_time <= timer.now())
        {
            m_hanhua_time = timer.now() + 5 * 1000;
            int id = int.Parse(obj.name);
            protocol.team.cmsg_hanhua _msg = new protocol.team.cmsg_hanhua();
            _msg.index = id - 1;
            net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_hanhua>(opclient_t.CMSG_TEAM_URGE, _msg);
        }
        else
        {
            ulong time = m_hanhua_time - timer.now();
            root_gui._instance.show_prompt_dialog_box("[ffc884]" + string.Format(game_data._instance.get_t_language("bingyuan_chat.cs_123_89"), time / 1000));//使用快捷喊话太过频繁，可以在{0}秒后再次使用
        }
    }
}
