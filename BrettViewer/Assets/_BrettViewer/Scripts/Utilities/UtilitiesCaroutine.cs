using UnityEngine;
using System.Collections;

public class UtilitiesCaroutine {

	public static IEnumerator WaitForRealSeconds(float seconds) {
		//	USAGE: 
		//	yield return StartCoroutine(CoroutineTools.WaitForRealSeconds(0.05f));
		
		
        var ms = seconds * 1000.0f;
        var stopwatch = new System.Diagnostics.Stopwatch();
 
        stopwatch.Start();
 
        while (stopwatch.ElapsedMilliseconds < ms) {
            yield return null;
        }
    }
}
