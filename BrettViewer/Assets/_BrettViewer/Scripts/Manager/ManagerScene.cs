using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScene : Manager {

    private static GameObject orthoRig;
    public static GameObject OrthoRig {
        get {
            if (orthoRig == null) {
                orthoRig = GameObject.Find("KneeOrtho");
            }
            return orthoRig;
        }

    }

    private static GameObject perspRig;
    public static GameObject PerspRig {
        get {
            if (perspRig == null) {
                perspRig = GameObject.Find("KneeRig");
            }
            return perspRig;
        }

    }

    private static Camera mainCam;
    public static Camera MainCam {
        get {
            if(mainCam == null) {
                mainCam = Camera.main;
            }
            return mainCam;
        }
    }

}
