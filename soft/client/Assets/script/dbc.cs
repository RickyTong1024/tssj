
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class dbc
{
	private int m_x_num = 0;
	private int	m_y_num = 0;
	private byte[] m_data;
	private List<int> m_records = new List<int>();
	public Dictionary<long,int> m_index = new Dictionary<long,int>();
	private string m_name;
	public void load_txt(string name,int index)
	{
		m_x_num = 0;
		m_y_num = 0;
		m_records.Clear ();
		m_index.Clear ();

		m_name = name;

		TextAsset _txt_data = game_data._instance.get_object_res("config_txt/" + name, typeof(TextAsset)) as TextAsset;
		if(_txt_data == null)
		{
			Debug.Log("err config__" + name); 
			return;
		}

		m_data = _txt_data.bytes;

		for(int i = 0;i < m_data.Length;i ++)
		{

			if(m_data[i] == '\n')
			{
				m_y_num ++;
			}

			if((m_data[i] == '\n' || m_data[i] == '\t') && m_y_num > 1)
			{
				m_records.Add(i + 1);
			}

		}

		for (int i = 0;i < m_data.Length;i ++)
		{
			if(m_data[i] == '\t')
			{
				m_x_num ++;
			}

			else if(m_data[i] == '\n')
			{
				break;
			}

		}

		m_x_num ++;
		m_y_num -= 2;

		if (index < 0)
		{
			return;
		}

		for(int y = 0;y < m_y_num;y ++)
		{
			if(get(index,y).Length > 0)
			{
				try{
					m_index[long.Parse(get(index,y))] = y;
				}catch(System.Exception)
				{
					Debug.Log("key" + get(index,y));
				}
			}
			else
			{
				Debug.Log ("not font id:" + y + " " + name);
			}
		}

	}
	public string get_index(int x,int index)
	{
		if(!m_index.ContainsKey(index))
		{
			Debug.Log("not find key " + index + " " + m_name);
			return "0";
		}
		return get (x,m_index[index]);
	}

	public bool has_index(int index)
	{
		return m_index.ContainsKey (index);
	}

    public int get_near_key(int index)
    {
        if (index <= 1)
        {
            return -1;
        }

        int temp = 2;
        int cout = 0;
        if (!m_index.ContainsKey(index))
        {
            foreach (var item in m_index)
            {
                if (index > (int)(item.Key))
                {
                    temp = (int)(item.Key);
                    cout++;                  
                }
                else
                {
                    index = temp;
                }
            }
            if (cout == m_index.Count)
            {
                index = temp;
                cout = 0;
            }
            temp = 2;
        }
      return index;      
    }

	public string get(int x, int y)
	{
		int _num = 0;
		int _id = y * m_x_num + x;

		if(_id >= m_records.Count) return "";

		for (int i = m_records [y * m_x_num + x];m_data[i] != '\t' && m_data[i] != '\n' && m_data[i] != '\r';i ++)
		{
			_num ++;
		}


		//byte[] _byte = new byte[_num];

		_num = 0;

		for (int i = m_records [y * m_x_num + x];m_data[i] != '\t' && m_data[i] != '\n' && m_data[i] != '\r';i ++)
		{
			//_byte[_num] = m_data[i];

			_num ++;
		}

		string _out = System.Text.Encoding.UTF8.GetString (m_data,m_records [y * m_x_num + x],_num);
		if(_out.Length == 0)
		{
			return "0";
		}
		_out = _out.Replace ("{nn}", "\n");
		return _out;
	}
	public int get_x()
	{
		return m_x_num;
	}
	public int get_y()
	{
		return m_y_num;
	}
}
