using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpdateLifeUI : MonoBehaviour
{
    public ScoreManager scoreManager;
    public Sprite pvDead;
    
    public Image pv1;
    public Image pv2;
    public Image pv3;
    bool plant1Dead;
    bool plant2Dead;
    bool plant3Dead;
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = ScoreManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

        if (scoreManager.exposedDeadPlants == 1&&plant1Dead ==false)
        {

            plant1Dead = true;
            StartCoroutine(ChangePVState(pv1));

        }
        else if (scoreManager.exposedDeadPlants == 2 && plant2Dead == false)
        {
            plant2Dead = true;
            StartCoroutine(ChangePVState(pv2));

        }
        else if (scoreManager.exposedDeadPlants >= 3 && plant3Dead == false)
        {
            plant3Dead = true;
            StartCoroutine(ChangePVState(pv3));

        }




        IEnumerator ChangePVState(Image PV)
        {
            PV.rectTransform.DOScaleY(0f, 0.2f).SetEase(Ease.InOutElastic);
            PV.rectTransform.DOScaleX(0f, 0.2f).SetEase(Ease.InOutElastic);
            yield return new WaitForSeconds(0.1f);
            PV.sprite = pvDead;
            PV.color = new Color(64f/255 , 87f/255 , 35f/255 , 1);
            PV.rectTransform.DOScaleY(1f, 0.2f).SetEase(Ease.InOutElastic);
            PV.rectTransform.DOScaleX(1f, 0.2f).SetEase(Ease.InOutElastic);
        }
    }
}
