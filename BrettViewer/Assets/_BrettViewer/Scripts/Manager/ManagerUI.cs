using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUI : Manager {
    public static Dictionary<System.Type, GameObject> allScreens = new Dictionary<System.Type, GameObject>();

	public static void GoTo<T>(float animDelay = 0f) {
		GameObject goToScreen = allScreens[typeof(T)];
		GoTo(goToScreen.Get<Screen>());
	}

	public static bool IsOverUI {
		get {
			return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
		}
	}

	private static void GoTo(Screen goToScreen) {
		foreach (KeyValuePair<System.Type, GameObject> pair in allScreens) {
			if (pair.Key != typeof(ScreenOverlay)) {
				pair.Value.SetActive(pair.Key == goToScreen.GetType());
			}
		}

	}

	public static T Get<T>() where T : Screen {
		return GetScreenObject<T>().GetComponent<T>();
	}

	public static GameObject GetScreenObject<T>() {
		GameObject screen = allScreens[typeof(T)];
		//Scene sceneComp = scene.Get<Scene>();
		return screen;
	}

}
