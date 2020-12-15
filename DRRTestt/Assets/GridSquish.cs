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

	List<ScreenGridRow> screenGridRows = new List<ScreenGridRow>();
	private void DefineScreenGrid() {

		screenGridRows.Clear();

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
					screenGridRows.Add(s);
				}

			}
		}

		TestScreenGridPoints();
	}

	private void TestScreenGridPoints() {

		//Assign each point an associated row										
		Dictionary<Vector3, ScreenGridRow> pointGridLU = new Dictionary<Vector3, ScreenGridRow>(); //can use vector3 to LU associated row
		List<Vector3> gridPointList = new List<Vector3>();
		
		foreach(ScreenGridRow s in screenGridRows) {
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
		m0.gameobj = _0.gameObject;

		MeshPointArray m250 = new MeshPointArray();
		m250.Points = _250.mesh.vertices;
		m250.Density = 250;
		m250.gameobj = _250.gameObject;


		MeshPointArray m500 = new MeshPointArray();
		m500.Points = _500.mesh.vertices;
		m500.Density = 500;
		m500.gameobj = _500.gameObject;

		MeshPointArray m750 = new MeshPointArray();
		m750.Points = _750.mesh.vertices;
		m750.Density = 750;
		m750.gameobj = _750.gameObject;


		MeshPointArray m1000 = new MeshPointArray();
		m1000.Points = _1000.mesh.vertices;
		m1000.Density = 1000;
		m1000.gameobj = _1000.gameObject;


		MeshPointArray m1250 = new MeshPointArray();
		m1250.Points = _1250.mesh.vertices;
		m1250.Density = 1250;
		m1250.gameobj = _1250.gameObject;


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
				comparedPoints[k] = m.gameobj.transform.TransformPoint(p);
				if(p == m.Points[m.Points.Length - 1]){
					maxIndexForEachDensity.Add(k, m);
				}
				k++;
			}
		}


		GetNNAndDist(originalPoints, comparedPoints);

		//to get compared MeshPointArray.density, just need neighbor[q]
		//To get the ScreenGridRow - use the Vector3 to lookup the object, increment the val

		//need to cull irrelevant data - beyond a certain dist thresh.
		for (int q = 0; q < neighbors.Length; q++) {
			if (dists[q] < 0.25) {  //relevant data
				//GameObject c = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				//c.transform.position = comparedPoints[neighbors[q]];
				//c.transform.localScale = Vector3.one * 0.1f;


			}
		}

	}



	int[] neighbors;
	float[] dists;
	public void GetNNAndDist(Vector3[] originalPoints, Vector3[] comparedPoints) {
		NearestNeighborInterface.GetNNsandDist(comparedPoints, originalPoints, out neighbors, out dists);
	}
}

class ScreenGridRow {
	public Vector3 MidPoint;
	public Vector3[] Testpoints;
	public Color outputColour;
}

class MeshPointArray {
	public GameObject gameobj;
	public Vector3[] Points;
	public int Density;
}
