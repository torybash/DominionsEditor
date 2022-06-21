using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
	[Serializable]
	public class DictionarySerializable<T, U>
	{
		[HideInInspector]
		public List<T> keys = new List<T>();
		
		[HideInInspector]
		public List<U> elements = new List<U>();

		public int Count => keys.Count;

		public void Add(T key, U element)
		{
			keys.Add(key);
			elements.Add(element);
		}

		public void Remove(T key)
		{
			int i = keys.IndexOf(key);
			keys.RemoveAt(i);
			elements.RemoveAt(i);
		}

		public bool ContainsKey(T key) { return keys.Contains(key); }
		public bool ContainsElement(U e) { return elements.Contains(e); }

		public int IndexOfKey(T key) { return keys.IndexOf(key); }
		public int IndexOfElement(U element) { return elements.IndexOf(element); }

		public void Clear()
		{
			keys.Clear();
			elements.Clear();
		}

		public T GetKeyAt(int index) { return keys[index]; }
		public void SetKeyAt(int index, T key) { keys[index] = key; }

		public U GetElementAt(int index) { return elements[index]; }
		public void SetElementAt(int index, U element) { elements[index] = element; }

		public U GetElement(T key) { return elements[keys.IndexOf(key)]; }
		public void SetElement(T key, U element) { elements[keys.IndexOf(key)] = element; }

		public bool TryGetElement (T key, out U element)
		{
			bool hasKey = ContainsKey(key);
			element = hasKey ? GetElement(key) : default;
			return hasKey;
		}
	}
}