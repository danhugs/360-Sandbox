using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Main : MonoBehaviour {

	private void Awake() {
		//UnityEngine.Screen.orientation = ScreenOrientation.Landscape;
		////Scale res based on device??
		////UnityEngine.Screen.SetResolution(UnityEngine.Screen.width / 2, UnityEngine.Screen.height / 2, FullScreenMode.ExclusiveFullScreen, 30);
		//UnityEngine.Screen.SetResolution(UnityEngine.Screen.width, UnityEngine.Screen.height, FullScreenMode.ExclusiveFullScreen, 60);
		//Application.targetFrameRate = 60;

	}


	private void Start() {
		StartCoroutine(ILoadScenes());


	}
	private IEnumerator ILoadScenes() {
		yield return ILoadScenes(new string[] { "UI"});
	}

	private IEnumerator ILoadScenes(string[] scenes) {
		foreach (string s in scenes) {
			yield return SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);
		}

		foreach (Screen s in GameObject.FindObjectsOfType<Screen>()) {
			ManagerUI.allScreens.Add(s.GetType(), s.gameObject);
			s.transform.localPosition = Vector3.zero;
		}

		ManagerUI.GetScreenObject<ScreenOverlay>().transform.SetAsLastSibling();

		ManagerUI.GetScreenObject<ScreenOverlay>().GetComponent<ScreenOverlay>().OnLoad();

		ManagerUI.GoTo<ScreenHome>();

		ManagerUI.GetScreenObject<ScreenOverlay>().GetComponent<ScreenOverlay>().OnScreenEnter();


	}


	
}