
//using UnityEngine;
using System;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;

public class encryption_value
{
	private int m_int_key1;
	private int m_int_key2;
	private int m_int_value_0;
	private int m_int_value_1;

	public int m_value
	{
		get { return get_int(); }
		set { set_int(value); }
	}

	public encryption_value(int value = 0)
	{
		Random _rd = new Random(); 

		m_int_key1 = _rd.Next(10000,65536);
		m_int_key2 = _rd.Next(10000,65536);

		set_int (value);
		//string _temp = 0

	}
	/*
	public void set_double(double value)
	{
		m_double_value_0 = value;
		//m_double_value_0 = value ^ m_double_key1;
		//m_double_value_1 = value ^ m_double_key2;
	}

	public double get_double()
	{
		double _temp_0 = m_double_value_0 ^ m_double_key1;
		double _temp_1 = m_double_value_1 ^ m_double_key2;

		if(_temp_0 != _temp_1)
		{
			return 0;
		}
		return m_double_value_0;
	}
	*/

	public void set_int(int value)
	{
		m_int_value_0 = value ^ m_int_key1;
		m_int_value_1 = value ^ m_int_key2;
	}

	public int get_int()
	{
		int _temp_0 = m_int_value_0 ^ m_int_key1;
		int _temp_1 = m_int_value_1 ^ m_int_key2;
		
		if(_temp_0 != _temp_1)
		{
			game_lock();
			return 0;
		}

		return _temp_0;
	}

	void game_lock()
	{
		while(true)
		{

		}
	}
}

