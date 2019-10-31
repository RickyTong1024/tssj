
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class expression : MonoBehaviour,IMessage {

	public Mesh m_Mesh;
	public Mesh[] m_Meshes;
	public float m_lerp = 0.0f;
	public GameObject m_base;
	public AnimationClip[] m_anims;
	public int m_src_id = 0;
	public int m_dst_id = 0;
	private Vector3[] m_vest1;
	private Vector3[] m_vest2;
	private bool m_flag = false;

	private int m_old_srcIndex = 0;
	private	int m_old_dstIndex = 0;
	private float m_old_t = 0;
	// Use this for initialization

	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void IMessage.net_message(s_net_message message)
	{
		
	}
	bool is_anim(string name)
	{
		for(int i = 0;i < m_anims.Length;i ++)
		{
			if(m_anims[i].name == name)
			{
				return true;
			}
		}

		return false;
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "face_ex_name")
		{
			string _name = (string)message.m_string[0];

			if(is_anim(_name))
			{
				this.GetComponent<Animator>().Play((string)message.m_string[0]);
			}
		}
	}
	void Awake ()
	{
		MeshFilter filter  = m_base.GetComponent(typeof(MeshFilter)) as MeshFilter;
		
		// Make sure all meshes are assigned!
		for (int i=0;i<m_Meshes.Length;i++)
		{
			if (m_Meshes[i] == null)
			{	
				Debug.Log("MeshMorpher mesh  " + i + " has not been setup correctly");
				return;
			}
		}
		
		//  At least two meshes
		if (m_Meshes.Length < 2)
		{
			Debug.Log ("The mesh morpher needs at least 2 source meshes");

			return;
		}
		
		filter.sharedMesh = m_Meshes[0];
		m_Mesh = filter.mesh;
		int vertexCount = m_Mesh.vertexCount;
		for (int i=0;i<m_Meshes.Length;i++)
		{
			if (m_Meshes[i].vertexCount != vertexCount)
			{	
				Debug.Log("Mesh " + i + " doesn't have the same number of vertices as the first mesh");
				return;
			}
		}
	}
	void SetComplexMorph (int srcIndex, int dstIndex,float t) 
	{ 
		if (m_old_srcIndex == srcIndex && m_old_dstIndex == dstIndex && m_old_t == t) 
			return; 
			
		m_old_srcIndex = srcIndex;
		m_old_dstIndex = dstIndex;
		m_old_t = t;

		Mesh _src = m_Meshes [srcIndex];
		Mesh _dst = m_Meshes [dstIndex];


		Vector3[] v0 = _src.vertices; 
		Vector3[] v1 = _dst.vertices;
		Vector3[] vest;
		if (m_flag)
		{
			if (m_vest1 == null)
			{
				m_vest1 = new Vector3[m_Mesh.vertexCount]; 
			}
			vest = m_vest1;
		}
		else
		{
			if (m_vest2 == null)
			{
				m_vest2 = new Vector3[m_Mesh.vertexCount]; 
			}
			vest = m_vest2;
		}
		m_flag = !m_flag;

		for (int i = 0; i < vest.Length;i ++)
		{
			vest[i] = Vector3.Lerp(v0[i], v1[i], t); 
		}
		     
		m_Mesh.vertices = vest; 
		m_Mesh.RecalculateBounds();
	}
	public void set_target(int dst)
	{
		m_src_id = 0;
		m_dst_id = dst;
	}
	// Update is called once per frame
	void Update () {

		if(m_Meshes.Length <= 0)
		{
			return;
		}

		SetComplexMorph (m_src_id, m_dst_id, m_lerp);
	}
}
