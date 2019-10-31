
using UnityEngine;
using System.Collections;

struct s_anim
{
	Vector2 pos;
}
public class texture_anim : MonoBehaviour {

	public float m_dur = 0.1f;
	public Texture[] m_textures;
	public ArrayList m_anims = new ArrayList();
	public float m_height = 1;
	public float m_width = 1;
	public float m_cut_height = 0;
	public float m_cut_width = 0;

	public int m_anim_num = 1;
	public bool m_loop = true;
	int m_anim_cur = 0;
	Vector2 m_scale;


	// Use this for initialization
	void Start () {
		if(m_textures.Length > 0)
		{
			return ;
		}

		int _anim = 0;

		if(m_cut_height > 1 || m_cut_width > 1)
		{
			m_height = GetComponent<Renderer>().material.mainTexture.height / m_cut_height;
			m_width = GetComponent<Renderer>().material.mainTexture.width / m_cut_width;
		}

		//Debug.Log ("texelSize:" + m_height);

		for(float x = 0;x <= GetComponent<Renderer>().material.mainTexture.width;x += m_width)
		{
			for(float y = 0;y <= GetComponent<Renderer>().material.mainTexture.height;y += m_height)
			{
				if(_anim > m_anim_num) break;

				m_anims.Add(new Vector2(x / GetComponent<Renderer>().material.mainTexture.width,y / GetComponent<Renderer>().material.mainTexture.height));
				_anim ++;
			}
		}

		GetComponent<Renderer>().material.mainTextureScale = new Vector2 (m_width / GetComponent<Renderer>().material.mainTexture.width,m_height / GetComponent<Renderer>().material.mainTexture.height);
	}

	void OnEnable()
	{
		InvokeRepeating("update_anim", m_dur,m_dur);
	}
	
	void OnDisable()
	{
		CancelInvoke ("update_anim");
	}

	void update_anim()
	{
		if(m_textures.Length > 0)
		{
			if (m_anim_cur >= m_textures.Length - 1)
			{
				if(m_loop)
				{
					m_anim_cur = 0;
				}
			}
			else
			{
				m_anim_cur ++;
			}

			GetComponent<Renderer>().material.mainTexture = m_textures[m_anim_cur];
		}
		else
		{

			if (m_anim_cur >= m_anims.Count - 1)
			{
				if(m_loop)
				{
					m_anim_cur = 0;
				}
			}
			else
			{
				m_anim_cur ++;
			}


			GetComponent<Renderer>().material.mainTextureOffset = (Vector2)m_anims [m_anim_cur];
		}

	}
	// Update is called once per frame
	void Update () {
	
	}
}
