using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenHome : Screen {

    Button btnTib, btnFem, btnPat, btnMeniscus;
    RectTransform topLeft;

    Dictionary<eComponent, GameObject> compDict = new Dictionary<eComponent, GameObject>();
    Dictionary<eComponent, Button> btnDict = new Dictionary<eComponent, Button>();

    private void Awake() {
        topLeft = this.Get<RectTransform>("TopLeftFrame");
        
        btnTib = topLeft.Get<Button>("btnTib");
        btnFem = topLeft.Get<Button>("btnFem");
        btnPat = topLeft.Get<Button>("btnPat");
        btnMeniscus = topLeft.Get<Button>("btnMenis");

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


    }

    Color activeCol = UtilitiesColor.HexToColor("FFFFFF");
    Color disabledCol = UtilitiesColor.HexToColor("8A8A8A");
    
    private void ToggleVisibility(eComponent comp) {

        Color toBe = compDict[comp].activeSelf ?  disabledCol : activeCol;

        btnDict[comp].Get<Image>().color = toBe;
        compDict[comp].SetActive(!compDict[comp].activeSelf);
    }
}
