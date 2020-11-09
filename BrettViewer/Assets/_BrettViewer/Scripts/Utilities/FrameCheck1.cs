using System;
using System.Collections.Generic;
using UnityEngine;

public class FrameCheck1<K, A> : FrameCheck<K> {
	private Dictionary<K, A> dictionaryA = new Dictionary<K, A>();

	public void Activate(K key, A valueA) {
		base.Activate(key);
		//Update valueA
		if (dictionaryA.ContainsKey(key) == false) {
			dictionaryA.Add(key, valueA);
		} else {
			dictionaryA[key] = valueA;
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

	public void Remove(A valueA) {
		foreach (KeyValuePair<K, A> item in dictionaryA) {
			if (item.Value.Equals(valueA)) {
				dictionaryA.Remove(item.Key);
				Remove(item.Key);
				break;
			}
		}
	}

	public void Get(K key, out A valueA) {
		valueA = dictionaryA[key];
	}
	public A Get(K key) {
		return dictionaryA[key];
	}

	public override void Clear() {
		base.Clear();
		dictionaryA.Clear();
	}

}