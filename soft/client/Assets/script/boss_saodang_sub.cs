using UnityEngine;

public class boss_saodang_sub : MonoBehaviour
{
    public GameObject m_gold_obj;
    public GameObject m_exp_obj;

    public void init(int xunzhang, long shanghai, int level, int num)
    {
        this.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("boss_saodang_sub.cs_12_86"), num.ToString(), level);//挑战了{0}次魔王({1}级)
        m_gold_obj.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_resource(13).namecolor + game_data._instance.get_t_language("boss_saodang_sub.cs_13_130");//获得勋章
        m_exp_obj.transform.Find("Label").GetComponent<UILabel>().text = sys._instance.get_res_color_name(100);
        m_gold_obj.GetComponent<UILabel>().text = game_data._instance.get_t_resource(13).namecolor + xunzhang;
        m_gold_obj.transform.Find("Sprite").GetComponent<UISprite>().spriteName = game_data._instance.get_t_resource(13).smallicon;
        m_exp_obj.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "";
        m_exp_obj.GetComponent<UILabel>().text = "[ffb391]" + shanghai;
    }
}
