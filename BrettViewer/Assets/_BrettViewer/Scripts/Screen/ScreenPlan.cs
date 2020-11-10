using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPlan : Screen {

    public override void OnScreenEnter() {
        base.OnScreenEnter();
        ManagerScene.PerspRig.SetActive(false);
        ManagerScene.OrthoRig.SetActive(true);
        //ManagerScene.MainCam.gameObject.SetActive(false);

    }

}
