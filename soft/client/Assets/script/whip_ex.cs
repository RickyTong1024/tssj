
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class whip_ex: MonoBehaviour {
	
	public Transform[] m_points;

	public LineRenderer m_line;

	public List<Vector3> m_outs = new List<Vector3>();
	public float m_speed = 1.0f; 

	private Vector3 m_end_track;

	public int m_sub_num = 10; 

	private int m_index = 0;
	// Use this for initialization
	void Start () {
	
		for(int i = 0;i < m_sub_num;i ++)
		{
			Vector3 _new_pos = new Vector3(transform.position.x,transform.position.y,transform.position.z);
			m_outs.Add(_new_pos);
		}

		add_child (transform);
		m_line = transform.GetComponent<LineRenderer>();
		m_line.positionCount = m_sub_num;
	}

	void add_child(Transform obj)
	{
		if(obj.transform.name.IndexOf("Point") == -1)
		{
			return ;
		}

		m_points[m_index] = obj;

		m_index ++;

		for(int i = 0;i < obj.transform.childCount;i ++)
		{
			add_child(obj.transform.GetChild(i));
		}
	}
	private Vector3 CalculatePosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float u = 1.0f - t;
		float tt = t * t;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;
		Vector3 p = uuu * p0; //first term
		p += 3 * uu * t * p1; //second term
		p += 3 * u * tt * p2; //third term
		p += ttt * p3; //fourth term
		return p;
	}


	public Vector3 Interp(float t,Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		int numSections = 1;
		int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
		float u = t * (float)numSections - (float)currPt;
		
		Vector3 a = p0;
		Vector3 b = p1;
		Vector3 c = p2;
		Vector3 d = p3;
		
		return .5f * (
			(-a + 3f * b - 3f * c + d) * (u * u * u)
			+ (2f * a - 5f * b + 4f * c - d) * (u * u)
			+ (-a + c) * u
			+ 2f * b
			);
	}
	
	public Vector3 catmullRom(float t,Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3)
	{
	      float factor = 0.5f;
	      Vector3 c0 = P1;
	      Vector3 c1 =  (P2 - P0) * factor;
	      Vector3 c2 =  (P2 - P1) * 3.0f  -   (P3 - P1) * factor -  (P2 - P0)* 2.0f * factor ;
	      Vector3 c3 =  (P2 - P1) * -2.0f  +   (P3 - P1) * factor +  (P2 - P0) * factor;

	      Vector3 curvePoint = c3 * t * t * t + c2 * t * t + c1 * t + c0;

	    return curvePoint;
	}

	// Update is called once per frame
	void Update () {
	
		int _num = m_outs.Count;

		Vector3 _v0 = m_points[0].transform.position;
		Vector3 _v1 = m_points[1].transform.position;
		Vector3 _v2 = m_points[2].transform.position;
		Vector3 _v3 = m_points[3].transform.position;

		IEnumerable<Vector3> smoothTip = Interpolate.NewCatmullRom (m_points, 8, false);

		List<Vector3> Tmp = new List<Vector3>(smoothTip);

		if(m_sub_num != Tmp.Count)
		{
			m_sub_num = Tmp.Count;
			m_line.positionCount = m_sub_num;
		}

		for(int i = 0;i < Tmp.Count;i ++)
		{
			m_line.SetPosition(i,Tmp[i]);
		}
	}
}
