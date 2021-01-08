using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SquishImplement : MonoBehaviour
{

    public MeshFilter _0, _250, _500, _750, _1000, _1250;
	Texture2D tex;
	MeshFilter[] allMeshes;

	void Start()
    {
		tex = new Texture2D(300, 300);

		allMeshes = new MeshFilter[6];
		allMeshes[0] = _0;
		allMeshes[1] = _250;
		allMeshes[2] = _500;
		allMeshes[3] = _750;
		allMeshes[4] = _1000;
		allMeshes[5] = _1250;

		DefineScreenGrid();
    }

	private void DefineScreenGrid() {
			
		float x = tex.width;
		float y = tex.height;
		for (int i = 1; i < x + 1; i++) {
			for (int j = 1; j < y + 1; j++) {

				//Mid points for each grid
				Vector3 midPoint = Camera.main.ViewportToWorldPoint(new Vector3(i / x, j / y, 0));
				GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
				g.transform.position = midPoint;
				g.transform.localScale = Vector3.one * 10f;



				//float xMin = midPoint.x - (50 / x);
				//float xMax = midPoint.x + (50 / x);
				//float yMin = midPoint.y - (50 / y);
				//float yMax = midPoint.y + (50 / y);

				float xMin = midPoint.x - 100;
				float xMax = midPoint.x + 100;
				float yMin = midPoint.y - 100;
				float yMax = midPoint.y + 100;

				Debug.Log($"{xMin} + {xMax} + {yMin} + {yMax}");


				//if within x/y range, add to list? 
				Dictionary<Vector3, MeshFilter> culledPoints = new Dictionary<Vector3, MeshFilter>();
				foreach (MeshFilter m in allMeshes) {
					foreach(Vector3 v in m.mesh.vertices) {
						Vector3 k = m.transform.TransformPoint(v); //converting to world coords

						if (k.x >= xMin && k.x <= xMax && k.y >= yMin && k.y <= yMax) {
							culledPoints.Add(k, m);

							Debug.Log("cond1Hit");
							//GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
							//g.transform.position = k;
							//g.transform.localScale = Vector3.one * 10f;

						}					
					}
				}

				List<Vector3> orderedPoints = culledPoints.OrderByDescending(kp => kp.Value)
													  .Select(kp => kp.Key)
													  .ToList();



				//Get closest to cam
				////Order culledPoints by Z distance??

				//Run a nearest neighbor search for closest point to midPoint in culledPoints.  
				//Get resultant point, assert mesh type of first density pass 
				//Cull everything in culledPoints which is a lower Z
				//repeat




			}
		}



	}
}
