using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

/// 
/// This is a wrapper class that written to interface with Plugin I generated from this C++ Library: http://www.cs.umd.edu/~mount/ANN/
/// It requires an array of points to compare against (and that array's length), the original points we're testing against (and that array's length)
/// as well as an error bound, which determines the accuracy/speed of the result.  
/// 
/// |================================================================================================================================================|
/// |Note:  This is assessing the 2D nearest neighbor, so the splayed components laid on top of each other in X/Y dimension, and assesssing the      |
/// |nearest point in the compared component array.  This does not account for "Progression" - or the Z dimension of the PolarPoint.                 |
/// |================================================================================================================================================|
/// 


public static class NearestNeighborInterface {

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal", EntryPoint = "GetNN")]
#else
    [DllImport("ANN", EntryPoint = "GetNN")]
#endif
    private static extern IntPtr GetNeighborArray(/*float[,]*/float[] comparedPoints, int comparedCount, /*float[,]*/float[] originalPoints, int originalCount, double errorBound);

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("ANN", EntryPoint = "GetDists")]
#endif
    private static extern IntPtr GetDists();

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
    [DllImport("ANN", EntryPoint = "FreeMem")]
#endif
    public static extern IntPtr FreeMem(IntPtr ptr);


    /// <summary>
    /// This returns an array of Nearest Neighbor Indices, ordered by the original point indices. 
    /// That is, the returnedNN[i] is the index number in meshPointsCompared[] - which is the nearest neighbor to meshPointsOriginal[i] 
    /// e.g.
    ///  for (int i = 0; i < v3sOriginal.Length; i++){
    ///     Vector3 NNPoint = v3sCompared[returnedNN[i]];
    ///  }
    /// </summary>
    /// <param name="meshPointsCompared"></param>
    /// <param name="meshPointsOriginal"></param>
    /// <param name="returnedNNs"></param>
    /// <param name="returnedDists"></param>
    public static void GetNNsandDist(Vector3[] meshPointsCompared, Vector3[] meshPointsOriginal, out int[] returnedNNs, out float[] returnedDists) {
        ////Compared Mesh points
        //float[,] pointsCompared = new float[meshPointsCompared.Length, 2];
        //for (int i = 0; i < meshPointsCompared.Length; i++) {
        //    pointsCompared[i, 0] = meshPointsCompared[i].x;
        //    pointsCompared[i, 1] = meshPointsCompared[i].y;
        //}

        ////original mesh points
        //float[,] pointsOrig = new float[meshPointsOriginal.Length, 2];
        //for (int i = 0; i < meshPointsOriginal.Length; i++) {
        //    pointsOrig[i, 0] = meshPointsOriginal[i].x;
        //    pointsOrig[i, 1] = meshPointsOriginal[i].y;

        // }

        //Compared Mesh points
        float[] pointsCompared = new float[meshPointsCompared.Length * 3];
        for (int i = 0; i < meshPointsCompared.Length; i++) {
            pointsCompared[3 * i] = meshPointsCompared[i].x;
            pointsCompared[3 * i + 1] = meshPointsCompared[i].y;
            pointsCompared[3 * i + 2] = meshPointsCompared[i].z;
        }

        //original mesh points
        float[] pointsOrig = new float[meshPointsOriginal.Length * 3];
        for (int i = 0; i < meshPointsOriginal.Length; i++) {
            pointsOrig[3 * i] = meshPointsOriginal[i].x;
            pointsOrig[3 * i + 1] = meshPointsOriginal[i].y;
            pointsOrig[3 * i + 2] = meshPointsOriginal[i].z;
        }

        double errorBound = 0;

        IntPtr returnedIntPtr = GetNeighborArray(pointsCompared, meshPointsCompared.Length, pointsOrig, meshPointsOriginal.Length, errorBound);
        returnedNNs = new int[meshPointsOriginal.Length];
        Marshal.Copy(returnedIntPtr, returnedNNs, 0, meshPointsOriginal.Length);

        IntPtr returnedFloatPtr = GetDists();
        returnedDists = new float[meshPointsOriginal.Length];
        Marshal.Copy(returnedFloatPtr, returnedDists, 0, meshPointsOriginal.Length);

        FreeMem(returnedIntPtr);
        FreeMem(returnedFloatPtr);//TODO create another one
    }
}


