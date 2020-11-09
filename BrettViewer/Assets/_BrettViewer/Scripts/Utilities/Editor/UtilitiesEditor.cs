using UnityEngine;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class UtilitiesEditor {

	public static string GetPathRelative(string path) {
		path = path.Replace("\\", "/");

		if (path.Contains(Application.dataPath)) {
			path = path.Replace(Application.dataPath, "");
			path = path.Substring(1);
		}

		return path;
	}

	public static string GetPathAbsolute(string path) {
		return Application.dataPath + "/" + path;
	}

	/// <summary>
	/// Load an asset
	/// </summary>
	/// <typeparam name="T">type of asset</typeparam>
	/// <param name="path">Filename of the file tot load with extention</param>
	/// <returns></returns>
	public static T LoadAsset<T>(string path) where T : UnityEngine.Object {
		path = GetPathRelative(path);
		path = "Assets/" + path;

		T asset = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));

		return asset;
	}

	public static string[] GetFileNames(string localPath, params string[] ignoreContaining) {
		string path = Application.dataPath + "/" + localPath;

		//MAC / PC PATH CONVERSION
		path = Path.GetDirectoryName(path);

		//CONTINUE WHEN THE PATH EXISTS
		if (Directory.Exists(path)) {
			List<string> fileNames = new List<string>();

			string[] filePaths = Directory.GetFiles(path);
			foreach (string file in filePaths) {
				FileInfo fileInfo = new FileInfo(file);
				if (fileInfo.Extension != ".meta") {
					bool isIgnore = false;
					foreach (string ignore in ignoreContaining) {
						if (fileInfo.Name.Contains(ignore)) {
							isIgnore = true;
							continue;
						}
					}

					if (isIgnore == false) {
						fileNames.Add(fileInfo.Name);
					}
				}
			}
			return fileNames.ToArray();
		} else {
			Debug.LogError("Can't find: " + path);
			return new string[0];
		}
	}
}