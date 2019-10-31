
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class protocall {

	public static int key1 = 0;
	public static int key2 = 0;

}

public class protoList<T> {
	public List<T> plist = new List<T>();

	public T this[int index]
	{
		get
		{
			return plist[index];
		}
		set
		{
			plist[index] = value;
		}
	}

	public List<T> get_all()
	{
		return plist;
	}

	public int Count
	{
		get { return plist.Count; }
	}

	public void Add(T item)
	{
		plist.Add (item);
	}

	public void Remove(T item)
	{
		plist.Remove (item);
	}
}
