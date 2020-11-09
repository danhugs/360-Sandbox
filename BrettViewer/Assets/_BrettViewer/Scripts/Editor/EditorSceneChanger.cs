using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
//using TMPro;
using System.IO;
using UnityEditor.SceneManagement;

//[ExecuteInEditMode]
public class EditorWindowScenes : EditorWindow {


	//Vector2Int p1;
	//bool showBtn = true;

	//[MenuItem("Examples/position")]
	//static void Init() {
	//	GetWindow<EditorWindowScenes>("position");
	//}

	//void OnGUI() {
	//	Rect r = position;
	//	GUILayout.Label("Position: " + r.x + "x" + r.y);

	//	p1 = EditorGUILayout.Vector2IntField("Set the position:", p1);
	//	if (showBtn) {
	//		if (GUILayout.Button("Accept new position")) {
	//			r.x = p1.x;
	//			r.y = p1.y;

	//			position = r;
	//		}
	//	}
	//}


	private static Color colorOK = UtilitiesColor.HexToColor("#85cd65");
	private static Color colorNo = UtilitiesColor.HexToColor("#a25c4e");
	private const int buttonHieght = 52;

	private string lastScenePath;
	private bool isPlaying;

	[MenuItem("Window/Scene Changer &s")]
	private static void Init() {

		Window.maxSize = new Vector2(800, 82);
		Window.Show();
		//Window.Show(true);
	}

	private void OnGUI() {
		
		GUILayout.BeginHorizontal();
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			string name = System.IO.Path.GetFileNameWithoutExtension(scene.path);

			//Scene changed? Dirty?
			bool isDirty = EditorSceneManager.GetSceneByPath(scene.path).isDirty;

			if (isDirty) {
				GUI.color = colorNo;
			} else if (EditorSceneManager.GetActiveScene().path == scene.path) {
				GUI.color = colorOK;
			} else {
				GUI.color = Color.white;
			}

			if (GUILayout.Button(name + (isDirty ? " !" : ""), GUILayout.Height(buttonHieght * 0.75f))) {
				if (Application.isPlaying) {
					EditorApplication.isPlaying = false;
				}
				EditorSceneManager.OpenScene(scene.path);
			}
		}
		GUILayout.EndHorizontal();
		GUI.color = Application.isPlaying ? colorNo : Color.white;
		if (GUILayout.Button(Application.isPlaying ? " ■ " : " ► ", GUILayout.Height(buttonHieght))) {
			OnClickPlayStop();
		}
	}

	private void Update() {
		if (isPlaying == true && !Application.isPlaying) {
			EditorSceneManager.OpenScene(lastScenePath);
			isPlaying = false;
		}
	}

	private void OnClickPlayStop() {
		if (Application.isPlaying) {
			isPlaying = true;
			EditorApplication.isPlaying = false;
		} else {
			lastScenePath = EditorSceneManager.GetActiveScene().path;
			if (EditorBuildSettings.scenes.Length > 0) {
				EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
			}

			EditorApplication.isPlaying = true;
		}
	}

	private static EditorWindowScenes Window {
		get {
			return (EditorWindowScenes)EditorWindow.GetWindow(typeof(EditorWindowScenes));
		}
	}
}