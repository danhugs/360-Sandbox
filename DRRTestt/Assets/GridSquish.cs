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
		
    }

	private void Update() {
		//Reset();
		if (Input.GetKeyDown(KeyCode.Space)) {
			DefineScreenGrid();
		}
		
	}

	List<ScreenGridRow> ScreenGridPoints = new List<ScreenGridRow>();
	private void DefineScreenGrid() {

		ScreenGridPoints.Clear();

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
		List<Vector3> gridPointList = new List<Vector3>();
		
		foreach(ScreenGridRow s in ScreenGridPoints) {
			foreach(Vector3 v in s.Testpoints) {
				pointGridLU.Add(v, s);
				gridPointList.Add(v);
			}
		}
		Vector3[] originalPoints = gridPointList.ToArray();

		//Construct array of test points for NN.  - each Vector3 has it's own ScreenGridRow
		int i = 0;
		Vector3[] points = new Vector3[pointGridLU.Count];
		foreach(KeyValuePair<Vector3, ScreenGridRow> pair in pointGridLU) {
			points[i] = pair.Key;
			i++;
		}

		//Construct array of originalMesh points
		#region MeshPointArrays
		MeshPointArray m0 = new MeshPointArray();
		m0.Points = _0.mesh.vertices;
		m0.Density = 0;

		MeshPointArray m250 = new MeshPointArray();
		m250.Points = _250.mesh.vertices;
		m250.Density = 250;

		MeshPointArray m500 = new MeshPointArray();
		m500.Points = _500.mesh.vertices;
		m500.Density = 500;

		MeshPointArray m750 = new MeshPointArray();
		m750.Points = _750.mesh.vertices;
		m750.Density = 750;

		MeshPointArray m1000 = new MeshPointArray();
		m1000.Points = _1000.mesh.vertices;
		m1000.Density = 1000;

		MeshPointArray m1250 = new MeshPointArray();
		m1250.Points = _1250.mesh.vertices;
		m1250.Density = 1250;

		List<MeshPointArray> allMeshList = new List<MeshPointArray>();
		allMeshList.Add(m0);
		allMeshList.Add(m250);
		allMeshList.Add(m500);
		allMeshList.Add(m750);
		allMeshList.Add(m1000);
		allMeshList.Add(m1250);

		#endregion
		MeshPointArray[] allMeshes = allMeshList.ToArray();

		int pointsLength = (m0.Points.Count() + m250.Points.Count() + m500.Points.Count() + m750.Points.Count() + m1000.Points.Count() + m1250.Points.Count());
		Vector3[] comparedPoints = new Vector3[pointsLength];
		Dictionary<int, MeshPointArray> maxIndexForEachDensity = new Dictionary<int, MeshPointArray>(); //can use vector3 to LU associated row

		//assign the originalmesh point master array, record index of what density it belongs to.  
		int k = 0;
		foreach (MeshPointArray m in allMeshes) {
			foreach(Vector3 p in m.Points) {
				comparedPoints[k] = p;
				if(p == m.Points[m.Points.Length - 1]){
					maxIndexForEachDensity.Add(k, m);
				}
				k++;
			}
		}

		Debug.Log(originalPoints.Length);
		Debug.Log(comparedPoints.Length);

		//GetNNAndDist(originalPoints, comparedPoints);

	}


	int[] neighbors;
	float[] dists2D;
	public void GetNNAndDist(Vector3[] originalPoints, Vector3[] comparedPoints) {
		neighbors = new int[originalPoints.Length];
		dists2D = new float[originalPoints.Length];
		for (int i = 0; i < neighbors.Length; i++) {
			neighbors[i] = 4;
		}

		//dists2D = new float[originalPoints.Length];

		//float threshhold = 10f;
		NearestNeighborInterface.GetNNsandDist(originalPoints, comparedPoints, neighbors, dists2D);

		for (int i = 0; i < 100; i++) {
			Debug.Log(neighbors[i]);

		}

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
