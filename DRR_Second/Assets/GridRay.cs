using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GridRay : MonoBehaviour
{

	public MeshFilter _0, _250, _500, _750, _1000, _1250;
	Texture2D tex;
	public RawImage img;
	public Transform femurGroup;

	Dictionary<GameObject, int> densityLU = new Dictionary<GameObject, int>();

	void Start()
    {
		tex = new Texture2D(320, 180);
		//tex = new Texture2D(320*10, 180*10);

		densityLU.Add(_0.gameObject, 0);
		densityLU.Add(_250.gameObject, 250);
		densityLU.Add(_500.gameObject, 500);
		densityLU.Add(_750.gameObject, 750);
		densityLU.Add(_1000.gameObject, 1000);
		densityLU.Add(_1250.gameObject, 1250);
    }

	Vector3 lastMousePos;
	private void Update() {
		//Reset();
		if (Input.GetKeyDown(KeyCode.Space)) {
			DefineScreenGrid();
		}

		if (Input.GetMouseButtonDown(0)) {
			img.color = Color.clear;
			lastMousePos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0)) {
			DefineScreenGrid();
		}

		if (Input.GetMouseButton(0)) {
			Vector3 moveDelta = lastMousePos - Input.mousePosition;

			femurGroup.eulerAngles += new Vector3(-moveDelta.y/10, moveDelta.x/10);
			lastMousePos = Input.mousePosition;
		}
		
	}

	List<ScreenGridRow> screenGridRows = new List<ScreenGridRow>();

	private void DefineScreenGrid() {

		screenGridRows.Clear();

		//Determine furthest point of mesh (from camera)
		//divide that by the number of slices to determine cut offset
		//handle out-of-frustrum issues

		float x = tex.width;
		float y = tex.height;

		for (int i = 1; i < x + 1; i++) {
			for (int j = 1; j < y + 1; j++) {
				//Mid points for each grid
				Vector3 midPoint = Camera.main.ViewportToWorldPoint(new Vector3(i / x, j / y, 0));

				//if midpoint x/y is outside of bounds?  
				if(Physics.Raycast(new Ray(midPoint, transform.forward))) {
					//cast a new ray
					ScreenGridRow s = new ScreenGridRow();
					s.MidPoint = midPoint;
					s.pixelIndex = ConvertCoordsToIndex(i, j);
					s.rayNum = 0;
					screenGridRows.Add(s);
				}

			}
		}

		TestScreenGridPoints();

		
	}

	private void TestScreenGridPoints() {

		foreach(ScreenGridRow s in screenGridRows) {
			//cast a ray at this point
			CastGridRay(s.MidPoint, s);

		}

		foreach (ScreenGridRow s in screenGridRows) {
			if(s.val == 0f) {
				s.outputColour = Color.green;
			} else {
				s.outputColour = Color.Lerp(Color.black, Color.white, (s.val / 1000));
			}
			
		}

		SetPixelsOfTex();
	}

	//each screenGridrow tracks whcih ray it is 
	void CastGridRay(Vector3 startPoint, ScreenGridRow s) {
		RaycastHit hit;
		if(Physics.Raycast(startPoint, transform.forward, out hit)){
			s.rayNum++;


			if (s.rayNum%2 == 0) {

				s.val += densityLU[hit.collider.gameObject] * hit.distance;
			}

			if (s.val == 0) {
				Debug.Log("what " + hit.collider.gameObject.name);
			}


			CastGridRay(hit.point+transform.forward, s);
		}
	}


	void SetPixelsOfTex() {
		Color[] cols = tex.GetPixels();
		for (int i = 0; i < cols.Length; i++) {
			cols[i] = Color.red;
		}
	

		for (int i = 0; i < screenGridRows.Count; i++) {
			cols[screenGridRows[i].pixelIndex] = screenGridRows[i].outputColour;
		}

		tex.SetPixels(cols);
		tex.Apply();

		img.texture = tex;
		img.color = Color.white;

	}


	int ConvertCoordsToIndex(int x, int y) {
		return y * tex.width + x;
	}



	//int[] neighbors;
	//float[] dists;
	//public void GetNNAndDist(Vector3[] originalPoints, Vector3[] comparedPoints) {
	//	NearestNeighborInterface.GetNNsandDist(comparedPoints, originalPoints, out neighbors, out dists);
	//}



}

//struct RaycastHitObjectThing {
//	public Vector3 gridPoint;
	
//}

class ScreenGridRow {
	public Vector3 MidPoint;
	//public Vector3[] Testpoints;
	public Color outputColour;
	public float val = 0f;
	internal int pixelIndex;
	public int rayNum;
}

//class MeshPointArray {
//	public GameObject gameobj;
//	public Vector3[] Points;
//	public int Density;
//}
