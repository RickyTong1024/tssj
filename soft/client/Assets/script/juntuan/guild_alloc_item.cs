using UnityEngine;

public class guild_alloc_item : MonoBehaviour {
    public UILabel m_name;
    public UILabel m_attack;
    public UILabel m_rew_value;
    public alloc_item m_item;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
   
	void Update () {
	
	}
    public void click(GameObject obj)
    {
        if(obj.name == "up")
        {
            if (m_item.value + 1 <= 100)
            {
                m_item.value++;
 
            }
            
        }
        else if (obj.name == "down")
        {
            if (m_item.value - 1 > 0)
            {
                m_item.value--;
            }
        }
        update_ui();
    }
    public void update_ui()
    {
        m_name.text = m_item.name;
        m_attack.text = m_item.attack + "";
        m_rew_value.text = m_item.value + "%";
    }
}
