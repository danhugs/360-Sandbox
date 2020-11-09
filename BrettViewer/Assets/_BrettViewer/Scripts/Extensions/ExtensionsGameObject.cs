using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsGameObject {

	#region Get

	public static T Get<T>(this GameObject gameObject, string path) where T : Component {
		return Get<T>(gameObject.transform, path);
	}

	public static T Get<T>(this MonoBehaviour monoBehaviour, string path) where T : Component {
		return Get<T>(monoBehaviour.transform, path);
	}

	public static T Get<T>(this GameObject gameObject) where T : Component {
		return Get<T>(gameObject.transform, "");
	}

	public static T Get<T>(this MonoBehaviour monoBehaviour) where T : Component {
		return Get<T>(monoBehaviour.transform, "");
	}

	public static T Get<T>(this Transform transform, string path) where T : Component {
		if (path == "") {
			return transform.GetComponent<T>();
		}

#if UNITY_EDITOR
		if (transform.Find(path) == null) {
			Debug.LogError(string.Format("Can't find '{1}' at '{0}'", transform.name, path), transform.gameObject);
			return null;
			//Debug.Break();
		}
		if (transform.Find(path).GetComponent<T>() == null) {
			Debug.LogError(string.Format("{0}, Can't Get Component of '{1}'", transform.Find(path).name, typeof(T).ToString()), transform.gameObject);
			return null;
			//Debug.Break();
		}
#endif

		return transform.Find(path).GetComponent<T>();
	}

	#endregion Get

	#region Add

	public static T Add<T>(this Component component, string name = "") where T : Component {
		return component.transform.Add<T>(name);
	}

	public static T Add<T>(this Transform transform, string name = "") where T : Component {
		//Nested Object
		if (name.Length > 0) {
			GameObject gameObject = new GameObject(name);
			gameObject.transform.SetParent(transform, false);
			return gameObject.AddComponent<T>();
		}
		//Same Object
		T component = transform.gameObject.AddComponent<T>();
		return component;
	}

	#endregion Add

	//#region AddChild

	//public static GameObject AddChild(this Component component, string name = "gameObject", Transform parent = null) {
	//	return component.transform.AddChild(name, parent);
	//}

	//public static GameObject AddChild(this Transform transform, string name = "gameObject", Transform parent = null) {
	//	GameObject gameObject = new GameObject(name);
	//	if (parent != null) {
	//		gameObject.transform.SetParent(parent);
	//		gameObject.transform.localPosition = Vector3.zero;
	//		gameObject.transform.localScale = Vector3.one;
	//		gameObject.transform.localEulerAngles = Vector3.zero;
	//	}
	//	return gameObject;
	//}

	//#endregion AddChild
}