using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasManager : Singleton<CanvasManager>
{
    // Start is called before the first frame update

    public RectTransform uiTools;
    public RectTransform uiScore;
    public RectTransform uiClock;
    public RectTransform uiTime;
    public RectTransform uiLife;

    // Update is called once per frame


    public void Dissapear()
    {
        uiTools.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        uiTools.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
        uiScore.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        uiScore.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        uiClock.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
        uiClock.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
        uiTime.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        uiTime.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        uiLife.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
        uiLife.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
    }
    public void Reapear()
    {
        uiTools.DOScaleY(1f, 0.2f).SetEase(Ease.InOutBack);
        uiTools.DOScaleX(1f, 0.2f).SetEase(Ease.InOutBack);
        uiScore.DOScaleY(1f, 0.2f).SetEase(Ease.InOutBack);
        uiScore.DOScaleY(1f, 0.2f).SetEase(Ease.InOutBack);
        uiClock.DOScaleX(1f, 0.2f).SetEase(Ease.InOutBack);
        uiClock.DOScaleX(1f, 0.2f).SetEase(Ease.InOutBack);
        uiTime.DOScaleY(1f, 0.2f).SetEase(Ease.InOutBack);
        uiTime.DOScaleY(1f, 0.2f).SetEase(Ease.InOutBack);
        uiLife.DOScaleX(0.8f, 0.2f).SetEase(Ease.InOutBack);
        uiLife.DOScaleX(0.8f, 0.2f).SetEase(Ease.InOutBack);
    }
    void Update()
    {

    }
}
