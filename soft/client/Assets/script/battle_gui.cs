using UnityEngine;

public class battle_gui : MonoBehaviour
{
    public GameObject m_ui_ficht;
    public GameObject m_ui_round;
    public GameObject m_ui_buttle;
    public GameObject m_win_result;
    public GameObject m_win_gui;
    public GameObject m_win_result_2;
    public GameObject m_battle_main;
    public GameObject m_defeated_result_0;
    public GameObject m_defeated_result_1;
    public GameObject m_buttle_vs;
    public GameObject m_skip;
    public GameObject m_ui_hit;
    public GameObject m_exit_guaji;

    public void OnEnable()
    {
        m_win_result.SetActive(false);
        m_win_gui.SetActive(false);
        m_defeated_result_0.SetActive(false);
        m_defeated_result_1.SetActive(false);
        m_ui_hit.SetActive(false);

        battle._instance.get_battle_gui(true);

        if (battle_logic_ex._instance.m_msg_fight_text.can_jump)
        {
            m_skip.SetActive(true);
        }
        else
        {
            m_skip.SetActive(false);
        }
        if (game_data._instance.m_guaji > 0)
        {
            m_exit_guaji.SetActive(true);
        }
        else
        {
            m_exit_guaji.SetActive(false);
        }
        battle._instance.set_battle_index(1, battle_logic_ex._instance.m_max_battle_num);
        battle_logic_ex._instance.start_logic();
    }

    public void OnDestroy()
    {
        battle._instance.get_battle_gui(false);
    }

    void click(GameObject obj)
    {
        if (obj.transform.name == "skip")
        {
            s_message _msg = new s_message();
            _msg.m_type = "skip_battle";
            cmessage_center._instance.add_message(_msg);

            obj.SetActive(false);
        }
        if (obj.transform.name == "exit_guaji")
        {
            game_data._instance.m_guaji = 0;
            m_exit_guaji.SetActive(false);
        }
    }

    void click_hall()
    {
        sys._instance.m_game_state = "hall";
        sys._instance.load_scene(sys._instance.m_hall_name);
    }

    void result_click(GameObject obj)
    {
        battle._instance.result_click(obj);
    }
}
