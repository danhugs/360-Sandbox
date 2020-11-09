using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class UtilitiesResources {

	public static T Instanciate<T>(string path) where T : Component {
		GameObject prefab = Resources.Load<GameObject>(path);
		if(prefab == null) {
			Debug.LogError("Can't instanciate: '" + path + "' as " + typeof(T).ToString());
			return null;
		} else {
			T component = GameObject.Instantiate(prefab).GetComponent<T>();
			component.name = prefab.name;
			component.transform.position = Vector3.zero;
			component.transform.localEulerAngles = Vector3.zero;
			component.transform.localScale = Vector3.one;
			return component;
		}
	}

	public static GameObject Instanciate(string path) {
		GameObject prefab = Resources.Load<GameObject>(path);
		return GameObject.Instantiate(prefab);
	}
	public static T Load<T>(string path) where T:UnityEngine.Object {
		return Resources.Load<T>(path);
	}
}
