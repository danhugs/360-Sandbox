using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridSquish : MonoBehaviour
{

	public MeshFilter _0, _250, _500, _750, _1000, _1250;
	Texture2D tex;
	GameObject femurGroup;

	void Start()
    {
		tex = new Texture2D(100, 100);
		femurGroup = GameObject.Find("FemurGroup");
		
		DefineScreenGrid();
    }


	List<ScreenGridRow> ScreenGridPoints = new List<ScreenGridRow>();
	private void DefineScreenGrid() {
			
		float x = tex.width;
		float y = tex.height;
		for (int i = 1; i < x + 1; i++) {
			for (int j = 1; j < y + 1; j++) {
				//Mid points for each grid
				Vector3 midPoint = Camera.main.ViewportToWorldPoint(new Vector3(i / x, j / y, 0));

				//if midpoint x/y is outside of bounds?  
				if(Physics.Raycast(new Ray(midPoint, transform.forward))) {

					ScreenGridRow s = new ScreenGridRow();
					s.MidPoint = midPoint;
					s.Testpoints = new Vector3[10];

					//define test points?
					for (int q = 0; q < 10; q++) {
						s.Testpoints[q] = midPoint + new Vector3(0, 0, q + 1 * 10);
					}
					ScreenGridPoints.Add(s);
				}

			}
		}

		TestScreenGridPoints();
	}

	private void TestScreenGridPoints() {

		//Assign each point an associated row										
		Dictionary<Vector3, ScreenGridRow> pointGridLU = new Dictionary<Vector3, ScreenGridRow>(); //can use vector3 to LU associated row
		foreach(ScreenGridRow s in ScreenGridPoints) {
			foreach(Vector3 v in s.Testpoints) {
				pointGridLU.Add(v, s);
			}
		}

		//Construct array of test points for NN.
		int i = 0;
		Vector3[] points = new Vector3[pointGridLU.Count];
		foreach(KeyValuePair<Vector3, ScreenGridRow> pair in pointGridLU) {
			points[i] = pair.Key;
			i++;
		}

		//Construct array of originalMesh points





	}

}

class ScreenGridRow {
	public Vector3 MidPoint;
	public Vector3[] Testpoints;
	public Color outputColour;
}

class MeshPointArray {
	public Vector3[] Points;
	public int Density;
}
