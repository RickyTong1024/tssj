
using UnityEngine;
using System.Collections;

public class ghost : MonoBehaviour {

	float m_angle = 0; 
	float m_target_angle = 0;
	float m_time = 1;
	float m_height = 0;
	float m_base_height = 0;
	public float m_angle_speed = 180.0f;
	public float m_move_speed = 1.0f;
	public GameObject m_target;
	// Use this for initialization
	void Start () {
	
		this.transform.parent = null;
		m_base_height = this.transform.transform.position.y;
	}

	private double PointToAngle(Vector3 AOrigin, Vector3 APoint, float AEccentricity)
		
	{
		
		if (APoint.x == AOrigin.x)
			
			if (APoint.z > AOrigin.z)
				
				return Mathf.PI * 0.5;
		
		else return Mathf.PI * 1.5;
		
		else if (APoint.z == AOrigin.z)
			
			if (APoint.x > AOrigin.x)
				
				return 0;
		
		else return Mathf.PI;
		
		else
			
		{
			
			float Result = Mathf.Atan((AOrigin.z - APoint.z) /
			                          
			                          (AOrigin.x - APoint.x) * AEccentricity);
			
			if ((APoint.x < AOrigin.x) && (APoint.z > AOrigin.z))
				
				return Result + Mathf.PI;
			
			else if ((APoint.x < AOrigin.x) && (APoint.z < AOrigin.z))
				
				return Result + Mathf.PI;
			
			else if ((APoint.x > AOrigin.x) && (APoint.z < AOrigin.z))
				
				return Result + 2 * Mathf.PI;
			
			else return Result;
			
		}
		
	} 

	// Update is called once per frame
	void Update () {
	
		if(m_target == null)
		{
			Destroy(this.gameObject);
			return;
		}

		Vector3 _pos = this.transform.position;

		if(m_angle < 0)
		{
			m_angle = 360 + m_angle;
		}

		if(m_angle > 360)
		{
			m_angle = m_angle - 360;
		}

		if(Mathf.Abs(m_angle - m_target_angle) > 180)
		{
			if(m_angle > m_target_angle)
			{
				m_angle += Time.deltaTime * m_angle_speed;
			}
			else if(m_angle < m_target_angle)
			{
				m_angle -= Time.deltaTime * m_angle_speed;
			}
		}
		else
		{
			if(m_angle > m_target_angle)
			{
				m_angle -= Time.deltaTime * m_angle_speed;
			}
			else if(m_angle < m_target_angle)
			{
				m_angle += Time.deltaTime * m_angle_speed;
			}
		}

		float _add_x = Mathf.Sin (Mathf.PI / 180 * m_angle );
		float _add_z = Mathf.Cos (Mathf.PI / 180 * m_angle );

		Vector3 _dis = _pos - m_target.transform.position;

		float _speed = _dis.magnitude * m_move_speed;

		if(_speed < 1.0f)
		{
			_speed = 1.0f;
		}

		_pos.x += _add_x * Time.deltaTime * _speed;
		_pos.z -= _add_z * Time.deltaTime * _speed;
		_pos.y = Mathf.Sin (m_height * 0.1f) * 0.5f + m_base_height;

		m_height += Time.deltaTime * 10;

		this.transform.position = _pos;

		if(m_time > 0)
		{
			m_time -= Time.deltaTime + _speed;
		}
		else
		{
			m_time = Random.Range(0.2f,0.5f);
			m_target_angle = (float)PointToAngle (_pos, m_target.transform.position, 1) / Mathf.PI * 180 + 90;
		}
	}
}
