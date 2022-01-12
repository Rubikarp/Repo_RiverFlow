using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotweenExemple : MonoBehaviour
{
    public Transform Squiz;
    public AnimationCurve TreeCurve;




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R) == true)
        {

            StartCoroutine("SproutSkouiz");
        }
    }

    IEnumerator TreeSkouiz()
    {

        Squiz.localScale = new Vector3(0.3f, 0, 0);
        yield return new WaitForSecondsRealtime(0.1f);
        Squiz.DOScaleY(1f, 0.5f).SetEase(Ease.OutElastic);
        yield return new WaitForSecondsRealtime(0.1f);
        Squiz.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);
    }
    IEnumerator SproutSkouiz()
    {

        Squiz.DOScaleY(0.1f, 0.6f).SetEase(Ease.InElastic);

        yield return new WaitForSecondsRealtime(0.6f);
        StartCoroutine("TreeSkouiz");
    }
}
