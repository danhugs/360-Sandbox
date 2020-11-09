using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class UtilitiesGizmos {


	public static void TargetTransform(Transform transform) {
		Gizmos.color = Color.red;
		Gizmos.DrawCube(transform.position, Vector3.one * 0.02f);

		float size = 0.2f;
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position+transform.forward * size);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + transform.right * size);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + transform.up * size);
	}

	public static void Text(Vector3 pos, string text) {
		Text(pos, text, Gizmos.color);
	}


	public static void Text(Vector3 pos, string text, Color color) {
#if UNITY_EDITOR
		GUIStyle style = new GUIStyle();
		style.fontSize = 12;
		//style.fontStyle = isBold ? FontStyle.Bold : FontStyle.Normal;
		style.normal.textColor = color;
		Handles.Label(pos, text.Replace("\n","\n  "), style);
#endif
	}


	public static void DrawCross(Vector3 pos, float radius) {
		Gizmos.DrawLine(pos - Vector3.right * radius, pos + Vector3.right * radius);
		Gizmos.DrawLine(pos - Vector3.up * radius, pos + Vector3.up * radius);
		Gizmos.DrawLine(pos - Vector3.forward * radius, pos + Vector3.forward * radius);

	}


	public static void DrawCircle(Vector3 pos, float radius, Vector3 dirY, Vector3 dirX) {
		DrawCircle(pos, radius, dirY, dirX, 16);
	}

	public static void DrawCircle(Vector3 pos, float radius, Vector3 dirY, Vector3 dirX, int steps) {
		DrawCircle(pos, radius, dirY, dirX, steps, Gizmos.color);
	}

	public static void DrawCircle(Vector3 pos, float radius, Vector3 dirY, Vector3 dirX, int steps, Color color) {

		for (int i = 0; i < steps; i++) {
			float a = (float)(i + 0) / (float)steps * (Mathf.PI * 2f);
			float b = (float)(i + 1) / (float)steps * (Mathf.PI * 2f);

			Vector2 v2_A = new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius);
			Vector2 v2_B = new Vector3(Mathf.Cos(b) * radius, Mathf.Sin(b) * radius);
			//Vector3 diff = (v2_B - v2_A).normalized;

			//Vector3 perp = Vector3.Cross(diff, diff + up * 8f).normalized * radius;

			Vector3 A = pos + dirX * v2_A.x + dirY * v2_A.y;
			Vector3 B = pos + dirX * v2_B.x + dirY * v2_B.y;
			//Vector3 B = Vector3.Cross(v2_B, v2_B + up * 8f).normalized * radius;
			//Vector3 B = A + Vector3.up * 0.5f;



			Gizmos.color = color;
			Gizmos.DrawLine(A, B);
			//Vector3 A = pos + perp * v2_A;
			//Vector3 B = pos + perp * v2_B;

			/*
			Vector3 B_0 = (B - A).normalized;
		Vector3 B_1 = Vector3.Cross(B_0, new Vector3(B_0.x, B_0.y, B_0.z) + up * 8f).normalized;
			*/
			/*
			Vector3 A = pos + new Vector3(Mathf.Cos(a) * radius, 0f, Mathf.Sin(a) * radius);
			Vector3 B = pos + new Vector3(Mathf.Cos(b) * radius, 0f, Mathf.Sin(b) * radius);

			*/
		}

	}

	public static void DrawArc(Vector3 pos, float radius, Vector3 dirY, Vector3 dirX) {
		DrawArc(pos, radius, dirY, dirX, 16);
	}

	public static void DrawArc(Vector3 pos, float radius, Vector3 dirY, Vector3 dirX, int steps) {
		DrawArc(pos, radius, dirY, dirX, steps, Color.white);
	}

	public static void DrawArc(Vector3 pos, float radius, Vector3 dirY, Vector3 dirX, int steps, Color color) {
		DrawArc(pos, radius, dirY, dirX, steps, color, Mathf.PI / 2f);
	}

	public static void DrawArc(Vector3 pos, float radius, Vector3 dirY, Vector3 dirX, int steps, Color color, float initialGap) {
		for (int i = 0; i < steps; i++) {
			float a = (float)(i + 0) / (float)steps * (Mathf.PI) - initialGap;
			float b = (float)(i + 1) / (float)steps * (Mathf.PI) - initialGap;

			Vector2 v2_A = new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius);
			Vector2 v2_B = new Vector3(Mathf.Cos(b) * radius, Mathf.Sin(b) * radius);

			Vector3 A = pos + dirX * v2_A.x + dirY * v2_A.y;
			Vector3 B = pos + dirX * v2_B.x + dirY * v2_B.y;

			Gizmos.color = color;
			Gizmos.DrawLine(A, B);
		}
	}

	/// <summary>
	/// Render an arrow in the Gizmo view (XZ Top view)
	/// </summary>
	/// <param name="tipWidthLengh">Tip width (x) and length (y)</param>
	public static void DrawArrow(Vector3 A, Vector3 B, Vector2 tipWidthLengh) {
		DrawArrow(A, B, tipWidthLengh, Vector3.up);
	}

	/// <summary>
	/// Render an arrow in the gizmo view
	/// </summary>
	/// <param name="tipWidthLengh">Tip width (x) and length (y)</param>
	/// <param name="up">Up vector for the direction of the arrows</param>
	public static void DrawArrow(Vector3 A, Vector3 B, Vector2 tipWidthLengh, Vector3 up) {

		tipWidthLengh *= 0.5f;
		B += (A - B).normalized * tipWidthLengh.y;

		Gizmos.DrawLine(A, B);

		Vector3 B_0 = (B - A).normalized;
		Vector3 B_1 = Vector3.Cross(B_0, new Vector3(B_0.x, B_0.y, B_0.z) + up * 8f).normalized;

		Vector3 C = B + B_0 * -tipWidthLengh.y + B_1 * tipWidthLengh.x;
		Vector3 D = B + B_0 * -tipWidthLengh.y - B_1 * tipWidthLengh.x;

		Gizmos.DrawLine(B, C);
		Gizmos.DrawLine(C, D);
		Gizmos.DrawLine(D, B);

		Vector3 C_1 = Vector3.Cross((A - B).normalized, new Vector3((A - B).x, (A - B).y + 8f, (A - B).z)).normalized;

		Vector3 E = A + C_1 * tipWidthLengh.x;
		Vector3 F = A - C_1 * tipWidthLengh.x;

		Gizmos.DrawLine(E, F);
	}
}
