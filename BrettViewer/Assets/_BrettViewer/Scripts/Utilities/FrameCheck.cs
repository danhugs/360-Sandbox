using System;
using System.Collections.Generic;
using System.Linq;

public class FrameCheck<K>{
	private List<K> keys = new List<K>();
	protected Dictionary<K, bool> isMarked = new Dictionary<K, bool>();

	public void Activate(K key) {
		if(keys.Contains(key) == false) {
			keys.Add(key);
		}
		if (isMarked.ContainsKey(key) == false) {
			isMarked.Add(key, true);
		} else {
			isMarked[key] = true;
		}
	}


	public K GetKey(int index) {
		return keys[index];
	}


	/// <summary>
	/// Call this at the end of a frame
	/// </summary>
	public void EndFrame() {
		if(isMarked.Count > 0) {
			foreach (K key in isMarked.Keys.ToArray()) {
				//Mark all false
				isMarked[key] = false;
			}
		}
	}

	public bool IsActive(K key) {
		return isMarked.ContainsKey(key) && isMarked[key] == true;
	}

	public bool Contains(K key) {
		return isMarked.ContainsKey(key);
	}

	public int Length {
		get {
			return isMarked.Count;
		}
	}

	public virtual void Clear() {
		isMarked.Clear();
	}

	public void Remove(K key) {
		isMarked.Remove(key);
	}
}