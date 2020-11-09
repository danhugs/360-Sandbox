using UnityEngine;
using System.Collections;

public class GizmoGrid : MonoBehaviour {

	public int columns	= 4;
	public int rows		= 4;

	public Vector2 cellSize = new Vector2(1f, 2f);
	public Color color = Color.gray;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		Gizmos.color = color;

		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				Vector3 A = transform.TransformPoint(Vector3.right * (float)j * cellSize.x + Vector3.forward * (float)i * cellSize.y);
				Vector3 B = A + transform.right * cellSize.x;
				Vector3 C = B + transform.forward * cellSize.y;
				Vector3 D = A + transform.forward * cellSize.y;

				//Gizmos.DrawCube((A+B+C+ D)/4f, Vector3.one * 0.1f);
				Gizmos.DrawLine(A, B);
				Gizmos.DrawLine(B, C);
				Gizmos.DrawLine(C, D);
				Gizmos.DrawLine(D, A);
			}

		}

	}
}
