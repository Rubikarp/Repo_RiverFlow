using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweeningToolTips : MonoBehaviour
{
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
