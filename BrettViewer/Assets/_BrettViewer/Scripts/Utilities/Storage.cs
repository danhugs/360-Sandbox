using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage {
	private static Dictionary<string, object> cachedValues = new Dictionary<string, object>();

	public Storage() {
	}

	#region Encryption

	public static void Set<T>(string key, T value) {
		if (value == null) { 
			PlayerPrefs.SetString(key, "");//ENCRYPT
		} else { 
			PlayerPrefs.SetString(key, Encrypt(value.ToString()));//ENCRYPT
		}
		//CACHE VALUES FOR FASTER ACCESS
		if (cachedValues.ContainsKey(key)) {
			cachedValues[key] = value;
		} else {
			cachedValues.Add(key, value);
		}
	}

	public static T Get<T>(string key, T valueDefault) {
		if (!cachedValues.ContainsKey(key)) {
			if (PlayerPrefs.HasKey(key)) {
				if (string.IsNullOrEmpty(PlayerPrefs.GetString(key)))
					cachedValues.Add(key, default(T));//EMPTY STRING, USUALLY DEFAULTS TO EMPTY STRING, 0, 0f ETC.
				else
					cachedValues.Add(key, Convert<T>(Decrypt(PlayerPrefs.GetString(key))));//DECRYPT
			} else
				cachedValues.Add(key, valueDefault);//DECRYPT
		}

		//cachedValues
		return (T)cachedValues[key];
	}

	public static bool Has<T>(string key) {
		if (cachedValues.ContainsKey(key))
			return true;
		else if (PlayerPrefs.HasKey(key)) {
			cachedValues.Add(key, Convert<T>(Decrypt(PlayerPrefs.GetString(key))));//DECRYPT
			return true;
		} else
			return false;
	}


	private static string Encrypt(string input) {
		return input;
	}
	private static string Decrypt(string input) {
		return input;
	}



	/// <summary>
	/// Delete a key and its value
	/// </summary>
	public static void Delete(string key) {
		if (cachedValues.ContainsKey(key))
			cachedValues.Remove(key);

		PlayerPrefs.DeleteKey(key);
	}

	public static void DeleteAll() {
		PlayerPrefs.DeleteAll();
		cachedValues.Clear();
		Debug.Log("Deleted Player Preferences");
	}

	public static T Convert<T>(string val) {
		if (val == "" && typeof(T) != typeof(string)) {
			//          return null;
			return default(T);
		}

		if (typeof(T).IsEnum) {//CONVERT ENUM
			if (System.Enum.GetNames(typeof(T)).Contains(val)) {
				return (T)System.Enum.Parse(typeof(T), val, true);
			} else {
				return default(T);
			}
		} else if (typeof(T) == typeof(bool)) {
			return (T)System.Convert.ChangeType(bool.Parse(val), typeof(T));
		} else if (typeof(T) == typeof(long)) {
			return (T)System.Convert.ChangeType(long.Parse(val), typeof(T));
		} else if (typeof(T) == typeof(int)) {
			return (T)System.Convert.ChangeType(int.Parse(val), typeof(T));
		} else if (typeof(T) == typeof(double)) {
			return (T)System.Convert.ChangeType(double.Parse(val), typeof(T));
		} else if (typeof(T) == typeof(float)) {
			return (T)System.Convert.ChangeType(float.Parse(val), typeof(T));
		} else if (typeof(T) == typeof(string)) {
			return (T)System.Convert.ChangeType(val, typeof(T));
		} else if (typeof(T) == typeof(Color)) {
			int A = val.IndexOf("(") + 1;
			int B = val.LastIndexOf(")");
			string substring = val.Substring(A, B - A);
			float[] rgba = System.Array.ConvertAll<string, float>(substring.Split(','), float.Parse);

			return (T)System.Convert.ChangeType(new Color(rgba[0], rgba[1], rgba[2], rgba[3]), typeof(T));
		} else {
			Debug.LogError("Not supported: " + typeof(T).Name);
		}

		return (T)System.Convert.ChangeType(val, typeof(T));
	}

	#endregion Encryption

	#region StorageExtensions

	/// <summary>
	/// Save an array to storage
	/// </summary>
	public static void SetArray<T>(string key, T[] array, bool cleanUpFirst = false) {
		if (cleanUpFirst) {
			DeleteArray(key, array == null ? 0 : array.Length, false);
		}

		if (array == null) {
			Set<int>(key + "_count", 0);
			return;
		}

		Set<int>(key + "_count", array.Length);
		for (int i = 0; i < array.Length; i++) {
			Set<T>(key + "_" + i.ToString("000"), array[i]);
		}
	}

	/// <summary>
	/// Gets an array from storage
	/// </summary>
	/// <param name="canReturnNullArray">If <true> will return null if no data, otherwise will return an empty array</param>
	public static T[] GetArray<T>(string key, T valueDefault, bool canReturnNullArray = false) {
		int count = Get<int>(key + "_count", 0);
		if (count <= 0) {
			if (canReturnNullArray)
				return null;
			else
				return new T[0];
		} else {
			T[] array = new T[count];
			for (int i = 0; i < count; i++) {
				array[i] = Get<T>(key + "_" + i.ToString("000"), valueDefault);
			}

			return array;
		}
	}

	/// <summary>
	/// Is array even stored
	/// </summary>
	public static bool HasArray<T>(string key) {
		return Has<T>(key + "_count");
	}

	/// <summary>
	/// Delete an array of values
	/// </summary>
	public static void DeleteArray(string key) {
		DeleteArray(key, 0, true);
	}

	/// <summary>
	/// Will delete part of an array from storage
	/// </summary>
	private static void DeleteArray(string key, int startIndex, bool deleteCountKey) {
		int oldLength = Get<int>(key + "_count", 0);
		if (oldLength > startIndex) {
			for (int i = startIndex; i < oldLength; i++) {
				Delete(key + "_" + i.ToString("000"));
			}
		}

		if (deleteCountKey)
			Delete(key + "_count");
	}

	/// <summary>
	/// Save a dictionary to storage
	/// </summary>
	public static void SetDict<TKey, TValue>(string key, Dictionary<TKey, TValue> dictionary, bool cleanUpFirst = false) {
		if (cleanUpFirst) {
			DeleteDict(key, dictionary == null ? 0 : dictionary.Count, false);
		}

		if (dictionary == null) {
			Set<int>(key + "_count", 0);
			return;
		}

		Set<int>(key + "_count", dictionary.Count);
		int index = 0;
		foreach (KeyValuePair<TKey, TValue> kvp in dictionary) {
			Set<TKey>(key + "_key_" + index.ToString("000"), kvp.Key);
			Set<TValue>(key + "_val_" + index.ToString("000"), kvp.Value);

			index++;
		}
	}

	/// <summary>
	/// Gets dictionary from storage
	/// </summary>
	/// <param name="canReturnNullDict">If <true> will return null if no data, otherwise will return an empty dictionary</param>
	public static Dictionary<TKey, TValue> GetDict<TKey, TValue>(string key, TValue valueDefault, bool canReturnNullDict) {
		int count = Get<int>(key + "_count", 0);
		if (count <= 0) {
			if (canReturnNullDict)
				return null;
			else
				return new Dictionary<TKey, TValue>();
		} else {
			Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
			for (int i = 0; i < count; i++) {
				if (Has<TKey>(key + "_key_" + i.ToString("000"))) {
					dict.Add(Get<TKey>(key + "_key_" + i.ToString("000"), default(TKey)), Get<TValue>(key + "_val_" + i.ToString("000"), valueDefault));
				}
			}

			return dict;
		}
	}

	/// <summary>
	/// Is dictionary even stored
	/// </summary>
	public static bool HasDict<T>(string key) {
		return Has<T>(key + "_count");
	}

	/// <summary>
	/// Delete a dictionary of values
	/// </summary>
	public static void DeleteDict(string key) {
		DeleteDict(key, 0, true);
	}

	/// <summary>
	/// Will delete part of a dictionary from storage
	/// </summary>
	private static void DeleteDict(string key, int startIndex, bool deleteCountKey) {
		int oldLength = Get<int>(key + "_count", 0);
		if (oldLength > startIndex) {
			for (int i = startIndex; i < oldLength; i++) {
				Delete(key + "_key_" + i.ToString("000"));
				Delete(key + "_val_" + i.ToString("000"));
			}
		}

		if (deleteCountKey)
			Delete(key + "_count");
	}

	#endregion StorageExtensions

}