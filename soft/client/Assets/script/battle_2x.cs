using UnityEngine;

public class battle_2x : MonoBehaviour
{

    public void OnEnable()
    {
        if ((int)game_data._instance.m_player_data.m_speed == 1)
        {
            this.GetComponent<UISprite>().spriteName = "sdsz_001";
        }
        else if ((int)game_data._instance.m_player_data.m_speed == 2)
        {
            this.GetComponent<UISprite>().spriteName = "sdsz_002";

        }
        else if ((int)game_data._instance.m_player_data.m_speed == 3)
        {
            this.GetComponent<UISprite>().spriteName = "sdsz_003";
        }
    }

    public void click()
    {
        if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_2x)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("battle_2x.cs_31_72"), (int)e_open_level.el_2x));//该功能{0}级开启
            return;
        }

        if ((int)game_data._instance.m_player_data.m_speed == 1)
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_2x)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("battle_2x.cs_31_72"), (int)e_open_level.el_2x));//该功能{0}级开启
                return;
            }

            game_data._instance.m_player_data.m_speed = 2;
        }
        else if ((int)game_data._instance.m_player_data.m_speed == 2)
        {
            if (sys._instance.m_self.m_t_player.vip >= 1 || sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_3x)
            {
                game_data._instance.m_player_data.m_speed = 3;
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("battle_2x.cs_53_59"));//达到20级或者VIP1开启3倍速
                game_data._instance.m_player_data.m_speed = 1;
            }
        }
        else if ((int)game_data._instance.m_player_data.m_speed == 3)
        {
            game_data._instance.m_player_data.m_speed = 1;
        }

        if ((int)game_data._instance.m_player_data.m_speed == 1)
        {
            this.GetComponent<UISprite>().spriteName = "sdsz_001";
        }
        else if ((int)game_data._instance.m_player_data.m_speed == 2)
        {
            this.GetComponent<UISprite>().spriteName = "sdsz_002";
        }
        else if ((int)game_data._instance.m_player_data.m_speed == 3)
        {
            this.GetComponent<UISprite>().spriteName = "sdsz_003";
        }

        Time.timeScale = sys._instance.get_game_speed();
        game_data._instance.save();
    }
}
