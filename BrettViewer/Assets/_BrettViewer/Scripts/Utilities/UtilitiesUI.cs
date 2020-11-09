using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Source: http://wiki.unity3d.com/index.php?title=DrawLine
/// </summary>
public class UtilitiesUI {
	
	//public Vector3 WorldToCanvas(Vector3 world, Canvas canvas) {

	//}

	public Vector3 TransformToLocal(Transform point, Transform transformLocal) {
		return transformLocal.InverseTransformPoint(point.position);
	}
	
}
