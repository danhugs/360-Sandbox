using System.Collections.Generic;

public class FrameCheck2<K, A, B> : FrameCheck<K> {
	private Dictionary<K, A> dictionaryA = new Dictionary<K, A>();
	private Dictionary<K, B> dictionaryB = new Dictionary<K, B>();

	public void Activate(K key, A valueA, B valueB) {
		base.Activate(key);
		//Update valueA
		if (dictionaryA.ContainsKey(key) == false) {
			dictionaryA.Add(key, valueA);
		} else {
			dictionaryA[key] = valueA;
		}
		//Update valueB
		if (dictionaryB.ContainsKey(key) == false) {
			dictionaryB.Add(key, valueB);
		} else {
			dictionaryB[key] = valueB;
		}
	}

	public K IndexOf(A valueA, K defaultKey) {
		foreach (KeyValuePair<K, A> item in dictionaryA) {
			if (item.Value.Equals(valueA)) {
				return item.Key;
			}
		}
		return defaultKey;
	}

	public K IndexOf(B valueB, K defaultKey) {
		foreach (KeyValuePair<K, B> item in dictionaryB) {
			if (item.Value.Equals(valueB)) {
				return item.Key;
			}
		}
		return defaultKey;
	}

	public void Remove(A valueA) {
		foreach (KeyValuePair<K, A> item in dictionaryA) {
			if (item.Value.Equals(valueA)) {
				dictionaryA.Remove(item.Key);
				dictionaryB.Remove(item.Key);
				Remove(item.Key);
				break;
			}
		}
	}

	public void Remove(B valueB) {
		foreach (KeyValuePair<K, B> item in dictionaryB) {
			if (item.Value.Equals(valueB)) {
				dictionaryA.Remove(item.Key);
				dictionaryB.Remove(item.Key);
				Remove(item.Key);
				break;
			}
		}
	}

	public void Get(K key, out A valueA, out B valueB) {
		valueA = dictionaryA[key];
		valueB = dictionaryB[key];
	}

	public override void Clear() {
		base.Clear();
		dictionaryA.Clear();
		dictionaryB.Clear();
	}
}