using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UtilitiesInput : MonoBehaviour {
	private static Vector2[] touch2Start = new Vector2[0];

	public static bool IsTouchingUI {
		get {
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
				if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
					return true;
				}
			}
			return EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null;
		}
	}

	public static bool Touching() {
		if (Input.touchCount > 0 || Input.GetMouseButton(0)) {
			return true;
		}
		return false;
	}

	public static int TouchCount {
		get {
			return Application.isEditor ? 1 : Input.touchCount;
		}
	}

	#region Drag

	private static Vector2 dragStart = Vector2.zero;
	private static bool dragActive = false;

	public static bool Drag(float minRadius, out Vector2 offset) {
		if (TouchCount == 1 && Input.GetMouseButton(0)) {
			if (!dragActive) {
				dragActive = true;
				dragStart = Input.mousePosition;
			}

			float d = Vector2.Distance(Input.mousePosition, dragStart);
			offset = dragStart - (Vector2)Input.mousePosition;

			return d >= minRadius;
		} else {
			dragActive = false;
			offset = Vector2.zero;
			return false;
		}
	}

	#endregion Drag

	#region Pinch Scale

	//public static bool PinchScale(float maxRadius, out float scale) {
	//	if (Input.GetMouseButton(0)) {

	//		if (touch2Start.Length == 0) {
	//			//Store start touch
	//			touch2Start = new Vector2[] {
	//				new Vector2(1, 1),
	//				Input.mousePosition
	//			};
	//		}

	//		float distanceA = Vector2.Distance(touch2Start[0], touch2Start[1]);
	//		float distanceB = Vector2.Distance(new Vector2(1, 1), Input.mousePosition);
	//		float s = (Mathf.Max(distanceA, distanceB) - Mathf.Min(distanceA, distanceB)) / maxRadius;

	//		scale = distanceB > distanceA ? s : -s;
	//		return true;
	//	}
	//	//No pinch
	//	if (touch2Start.Length != 0) {
	//		touch2Start = new Vector2[0];
	//	}
	//	scale = 0f;
	//	return false;
	//}


	public static bool PinchScale(float maxRadius, out float scale) {
		if (TouchCount == 2) {
			if (touch2Start.Length == 0) {
				//Store start touch
				touch2Start = new Vector2[] {
					Input.GetTouch(0).position,
					Input.GetTouch(1).position
				};
			}

			float distanceA = Vector2.Distance(touch2Start[0], touch2Start[1]);
			float distanceB = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
			float s = (Mathf.Max(distanceA, distanceB) - Mathf.Min(distanceA, distanceB)) / maxRadius;

			scale = distanceB > distanceA ? s : -s;
			return true;
		}
		//No pinch
		if (touch2Start.Length != 0) {
			touch2Start = new Vector2[0];
		}
		scale = 0f;
		return false;
	}

	#endregion Pinch Scale

	#region Tap

	private static float tapTimeLast = 0f;
	private static int tapCount = 0;
	private static Vector2 tapPosition = Vector2.zero;
	private static int tapFrame = 0;

	public static bool Tap(float time, int count = 1, float radius = -1f) {
		radius = radius < 0f ? (16f / 1024f * (float)UnityEngine.Screen.height) : radius;

		if (TouchCount == 1 && Input.GetMouseButtonDown(0)) {
			//Already calculated?
			if (Time.frameCount == tapFrame) {
				return tapCount == count;
			} else {
				tapFrame = Time.frameCount;
			}

			float t = Time.time - tapTimeLast;
			float d = Vector2.Distance(Input.mousePosition, tapPosition);

			if (t > time || d > radius) {
				tapCount = 1;
			} else {
				tapCount++;
			}

			Debug.Log($"Tab: t: {t:0.00}, d:{d:0.0} / {radius}, count:{tapCount}/{count} isTrue: {tapCount == count}");

			//Remember
			tapTimeLast = Time.time;
			tapPosition = Input.mousePosition;

			return tapCount == count;
		}
		return false;
	}

	#endregion Tap

	public static bool TwistAngle(out float angle) {
		if (TouchCount == 2) {
			if (touch2Start.Length == 0) {
				//Store start touch
				touch2Start = new Vector2[] {
					Input.GetTouch(0).position,
					Input.GetTouch(1).position
				};
			}

			//Original Angle
			Vector2 deltaA = touch2Start[1] - touch2Start[0];
			float angleA = Mathf.Atan2(deltaA.y, deltaA.x);

			//New angle
			Vector2 deltaB = Input.GetTouch(1).position - Input.GetTouch(0).position;
			float angleB = Mathf.Atan2(deltaB.y, deltaB.x);

			//Which is shorter?
			angle = Mathf.DeltaAngle(angleA * Mathf.Rad2Deg, angleB * Mathf.Rad2Deg);

			return true;
		}
		//No twist
		if (touch2Start.Length != 0) {
			touch2Start = new Vector2[0];
		}
		angle = 0f;
		return false;
	}
}