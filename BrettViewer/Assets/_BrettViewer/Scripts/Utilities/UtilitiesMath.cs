using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilitiesMath{

	public static float GetAngleDifference(float a, float b) {
		return Mathf.Atan2(Mathf.Sin(b - a), Mathf.Cos(b - a));
	}

	public static float GetSineTime01(float interval) {
		return (GetSineTime(Time.time, interval)+1f)/2f;
	}

	public static float GetSineTime(float interval) {
		return GetSineTime(Time.time, interval);
	}

	public static float GetSineTime(float time, float interval) {
		float p = time % interval / interval;
		return Mathf.Sin(p * Mathf.PI * 2f);
	}
}
