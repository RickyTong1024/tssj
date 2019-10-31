
using UnityEngine;
using System.Collections;

public class player_random {

	public static ulong m_rd;

	public static void reset_random(ulong rd)
	{
		m_rd = rd;
	}

	public static int get_random(int min, int max)
	{
		m_rd = (1103515245 * m_rd + 12345) % (2L << 32);
		int delta = max - min;
		return (int)(m_rd % (ulong)delta) + min;
	}

	public static float get_random(float min, float max)
	{
		float  _rand = ((float)get_random (0,10000)) * 0.0001f;

		return min + ((max - min) * _rand);
	}
}
