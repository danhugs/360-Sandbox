using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenView : Screen {

    Button btnTib, btnFem, btnPat, btnMeniscus;
    RectTransform vis;

    Dictionary<eComponent, GameObject> compDict = new Dictionary<eComponent, GameObject>();
    Dictionary<eComponent, Button> btnDict = new Dictionary<eComponent, Button>();
    LineRenderer lr;

    private void Awake() {
        vis = this.Get<RectTransform>("Vis");
        
        btnTib = vis.Get<Button>("VertLayout/btnTib");
        btnFem = vis.Get<Button>("VertLayout/btnFem");
        btnPat = vis.Get<Button>("VertLayout/btnPat");
        btnMeniscus = vis.Get<Button>("VertLayout/btnMenis");

        btnTib.onClick.AddListener(() => { ToggleVisibility(eComponent.eTibia); });
        btnFem.onClick.AddListener(() => { ToggleVisibility(eComponent.eFemur); });
        btnPat.onClick.AddListener(() => { ToggleVisibility(eComponent.ePatella); });
        btnMeniscus.onClick.AddListener(() => { ToggleVisibility(eComponent.eMeniscus); });

        compDict.Add(eComponent.eTibia, GameObject.Find("KneeRig/Tibia"));
        compDict.Add(eComponent.eFemur, GameObject.Find("KneeRig/Femur"));
        compDict.Add(eComponent.ePatella, GameObject.Find("KneeRig/Patella"));
        compDict.Add(eComponent.eMeniscus, GameObject.Find("KneeRig/Meniscus"));

        btnDict.Add(eComponent.eTibia, btnTib);
        btnDict.Add(eComponent.eFemur, btnFem);
        btnDict.Add(eComponent.ePatella, btnPat);
        btnDict.Add(eComponent.eMeniscus, btnMeniscus);

        lr = this.Add<LineRenderer>();
        lr.positionCount = 2;
        lr.widthMultiplier = 0.02f;
        lr.material = Resources.Load<Material>("Line") as Material;
    }

    public override void OnScreenEnter() {
        base.OnScreenEnter();
        ManagerScene.PerspRig.SetActive(true);
        ManagerScene.OrthoRig.SetActive(false);
        //ManagerScene.MainCam.gameObject.SetActive(true);

    }

    private void Update() {
        UpdateInfoPanes();
    }

    private void UpdateInfoPanes() {
        
        //Draw line between Pane x and objecty.
        Transform t = compDict[eComponent.eMeniscus].transform.Find("tear");
        RectTransform rt = this.Get<RectTransform>("Panes/Meniscus");
        //Vector3[] worldCorners = new Vector3[4];
        //rt.GetLocalCorners(worldCorners);
        //lr.SetPosition(0, Camera.main.ScreenToWorldPoint(worldCorners[0]));

        //lr.SetPosition(0, Camera.main.ScreenToWorldPoint(new Vector3(rt.anchoredPosition.x, rt.anchoredPosition.y, 0.2f)));

        lr.SetPosition(0, Camera.main.ScreenToWorldPoint(new Vector3(rt.Find("ref").position.x, rt.Find("ref").position.y, 1f)));
        lr.SetPosition(1, t.position);
        

        
        
    }

    Color activeCol = UtilitiesColor.HexToColor("FFFFFF");
    Color disabledCol = UtilitiesColor.HexToColor("8A8A8A");
    
    private void ToggleVisibility(eComponent comp) {

        Color toBe = compDict[comp].activeSelf ?  disabledCol : activeCol;

        btnDict[comp].Get<Image>().color = toBe;
        compDict[comp].SetActive(!compDict[comp].activeSelf);

        if(comp == eComponent.eMeniscus) {
            bool currentlyOn = this.Get<RectTransform>("Panes/Meniscus").gameObject.activeSelf;
            this.Get<RectTransform>("Panes/Meniscus").gameObject.SetActive(!currentlyOn);
            lr.widthMultiplier = currentlyOn ? 0f : 0.02f;
        }
    }
}
