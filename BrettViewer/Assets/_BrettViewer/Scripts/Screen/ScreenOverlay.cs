using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ScreenOverlay : Screen {
    Button btnReview, btnPlan;

    Color colActive, colInactive;
    private void Awake() {
        btnReview = this.Get<Button>("btnReview");
        btnPlan = this.Get<Button>("btnPlan");

        colActive = btnReview.Get<Image>().color;
        colInactive = btnPlan.Get<Image>().color;

        btnReview.onClick.AddListener(ActiveReview);
        btnPlan.onClick.AddListener(ActivatePlan);
        
    }

    float animTime = 0.3f;

    private void ActivatePlan() {
        btnPlan.DOKill();
        btnReview.DOKill();
        btnPlan.Get<RectTransform>().DOAnchorPosX(0, animTime);
        btnReview.Get<RectTransform>().DOAnchorPosX(140, animTime);

        btnReview.Get<Image>().DOColor(colInactive, animTime);
        btnPlan.Get<Image>().DOColor(colActive, animTime);

        btnReview.Get<TextMeshProUGUI>("txt").DOColor(Color.black, animTime);
        btnPlan.Get<TextMeshProUGUI>("txt").DOColor(Color.white, animTime);

        ManagerUI.GoTo<ScreenPlan>();
    }

    private void ActiveReview() {
        btnPlan.DOKill();
        btnReview.DOKill();
        btnPlan.Get<RectTransform>().DOAnchorPosX(140, animTime);
        btnReview.Get<RectTransform>().DOAnchorPosX(0, animTime);

        btnReview.Get<Image>().DOColor(colActive, animTime);
        btnPlan.Get<Image>().DOColor(colInactive, animTime);

        btnReview.Get<TextMeshProUGUI>("txt").DOColor(Color.white, animTime);
        btnPlan.Get<TextMeshProUGUI>("txt").DOColor(Color.black, animTime);


        ManagerUI.GoTo<ScreenView>();
    }
}
