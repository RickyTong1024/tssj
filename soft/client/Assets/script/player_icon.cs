
using UnityEngine;
using System.Collections;

public class player_icon : MonoBehaviour {

	public int id;
	public int count;
	public int vip;
    public int gq_id;
	// Use this for initialization
	void Start () {
	
	}

	public void OnEnable()
	{
		reset ();
	}

	public void init()
	{
		id = 0;
		count = 0;
		vip = 0;

		UIDragScrollView uisv = this.GetComponent<UIDragScrollView>();
		if (uisv != null)
		{
			Object.Destroy(uisv);
		}
		this.GetComponent<UISprite>().alpha = 1.0f;
		this.transform.localEulerAngles = new Vector3(0,0,0);
	}

	public void reset(int type = 0)
	{
		s_t_class m_class = game_data._instance.get_t_class (id);
		if(m_class == null)
		{
			return;
		}
		
		this.transform.GetComponent<UISprite>().spriteName = m_class.icon;
		
		int color = 0;
		string s = "";
		string s1 = "";
		string s2 = "";
		color = count;
		
		if (color == 0)
		{
			s = "ntx_kt001";
		}
		else if (color == 1)
		{
			s = "ntx_kt002";
		}
		else if (color == 2)
		{
			s = "ntx_kt003";
		}
		else if (color == 3)
		{
			s = "ntx_kt004";
		}
		else if (color == 4)
		{
			s = "ntx_kt005";
		}
		else if (color == 5)
		{
			s = "ntx_kt006";
		}
		else if (color == 6)
		{
			s = "ntx_kt007";
		}
		else if (color == 7)
		{
			s = "ntx_kt008";
		}
	
		this.transform.Find("bg").GetComponent<UISprite>().spriteName = s;

		if(vip <= 3)
		{
			s1 = "txkt_ph001";
			s2 = "txkt_ph00";
		}
		else if(vip <= 7)
		{
			s1 = "txkt_gl001";
			s2 = "txkt_gl00";
		}
		else if(vip <= 10)
		{
			s1 = "txkt_gh001";
			s2 = "txkt_gh00";
		}
		else if(vip <= 15)
		{
			s1 = "txkt_rb001";
			s2 = "txkt_rb00";
		}
		if(vip == 0)
		{
			this.transform.Find("vip_effect").gameObject.SetActive(false);
		}
		else
		{
			this.transform.Find("vip_effect").gameObject.SetActive(true);
			this.transform.Find("vip_effect").GetComponent<UISprite>().spriteName = s1;
			this.transform.Find("vip_effect").GetComponent<UISpriteAnimation>().namePrefix = s2;
		}

        if (gq_id != 0)
        {
            if (platform_config_common.m_nationality == 0)
            {
                this.transform.Find("gg").gameObject.SetActive(false);
            }
            else
            {
                this.transform.Find("gg").gameObject.SetActive(true);
                this.transform.Find("gg").GetComponent<UISprite>().spriteName = sys._instance.returnfirstSpritename(gq_id);
            }
           
        }
        else
        {
            this.transform.Find("gg").gameObject.SetActive(false);
        }
	
	}

   
	// Update is called once per frame
	void Update () {
	
	}
}
