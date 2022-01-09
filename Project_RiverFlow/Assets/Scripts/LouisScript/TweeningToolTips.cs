using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TweeningToolTips : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Appear()
    {
        transform.DOScaleY(1f, 0.2f).SetEase(Ease.InOutBack);
        transform.DOScaleX(1f, 0.2f).SetEase(Ease.InOutBack);
    }
    public void Dissappear()
    {
        transform.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        transform.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
    }
}
