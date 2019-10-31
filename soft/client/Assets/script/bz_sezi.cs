using UnityEngine;

public class bz_sezi : MonoBehaviour
{
    public int m_ds = 0;
    public float m_time = 0.0f;
    void OnEnable()
    {
        this.GetComponent<UISpriteAnimation>().enabled = true;
    }

    public void player_sound()
    {
        sys._instance.play_sound("effect/sound/guzi");
    }

    void finish()
    {
        this.GetComponent<UISpriteAnimation>().enabled = false;
        this.GetComponent<UISprite>().spriteName = "sz00" + Mathf.Abs(m_ds).ToString() + "d";
    }

    void Update()
    {
        if (m_time > 0)
        {
            m_time -= Time.deltaTime;
            if (m_time <= 0)
            {
                finish();
            }
        }
    }
}
