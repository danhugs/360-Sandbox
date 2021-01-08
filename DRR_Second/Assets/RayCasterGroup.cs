using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCasterGroup : MonoBehaviour {


    public RawImage img;
    Texture2D tex;
    void Start() {
        tex = new Texture2D(300, 300);

        SetPixelsOfTex();
    }

    private void Update() {
		//if (Input.GetKeyDown(KeyCode.Space)) {
            SetPixelsOfTex();
        //}
    }


	void StartRaycast() {



		float x = tex.width;
		float y = tex.height;
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				//Debug.Log(i / x + " " + j / y);
				Ray r = Camera.main.ViewportPointToRay(new Vector3(i / x, j / y, 0));
				Debug.DrawRay(r.origin, r.direction, Color.Lerp(Color.red, Color.blue, i / x), 15f);
				
				RaycastHit[] hit = Physics.RaycastAll(r);

			}


		}



		//for (int i = 0; i < texArray.Length; i++) {
		//Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward);
		//}
		//}
	}

	int count;
	private void RayCastThing(Ray r) {
		
		RaycastHit hit;
		count++;
		if (Physics.Raycast(r, out hit) && (count < 100)) {
			Debug.Log(hit.collider.gameObject.name);
			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			g.transform.position = hit.point;
			RayCastThing(new Ray(hit.point, r.direction));
		}

	}

	int mmm = 1;
    void SetPixelsOfTex() {
        Color[] cols = tex.GetPixels();

        if (mmm > 5) {
            mmm = 1;
		} else {
            mmm++;
        }
        
		for (int i = 0; i < cols.Length; ++i) {
			cols[i] = Color.white * (UnityEngine.Random.Range(0.0f, 1.0f));

			//if (i % 2 == 0) {
			//	cols[i] = Color.white;
			//} else if (i > 1000*mmm && i < (1000*mmm) + 500) {
			//	cols[i] = Color.red;
			//} else {
			//	cols[i] = Color.black;
			//}

		}


		tex.SetPixels(cols);
        tex.Apply();

        img.texture = tex;

    }


	void ConvertCoordsToIndex(int x, int y, Texture2D tex) {
        int indexOfPixel = y * tex.width + x;
    }
}




