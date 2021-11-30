using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class Plant_Drawer : MonoBehaviour
{
    [Header("Réferences")]
    public Plant plant;

    [Header("Visual")]
    private SpriteRenderer sprRender;
    public PlantSprites_SCO visual;

    [Header("Living")]
    [SerializeField] Camera cam;
    [SerializeField] Canvas canvas;
    [SerializeField] Image imgTiller;

    [Header("Tweening")]
    public AnimationCurve TreeCurve;
    private bool increase =true;
    [Header("Particles")]
    public ParticleSystem Leafs;
    void Start()
    {
        cam = Camera.main;
        canvas.worldCamera = cam;

        sprRender = gameObject.GetComponent<SpriteRenderer>();

        sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
        plant.onStateChange.AddListener(UpdateSkin);
    }

    void Update()
    {
       imgTiller.fillAmount = plant.timer;
    }

    private void UpdateSkin(bool isUp)
    {
        //Debug.Log("test");
        increase = isUp;
        //sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
        //StopAllCoroutines();
        StartCoroutine(SproutSkouiz(plant.currentState));
    }

    IEnumerator TreeSkouiz(PlantState state)
    {

        switch (state)
        {
            case PlantState.Dead:

                transform.localScale = new Vector3(0.3f, 0, 0);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutElastic);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);

                break;
            case PlantState.Agony:

                transform.localScale = new Vector3(0.3f, 0, 0);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutElastic);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);

                break;
            case PlantState.Baby:

                transform.localScale = new Vector3(0.3f, 0, 0);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutElastic);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);

                break;
            case PlantState.Young:
                Leafs.Play(true);
                transform.localScale = new Vector3(0.3f, 0, 0);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutQuart);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);

                break;
            case PlantState.Adult:
                Leafs.Play(true);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);

                break;
            case PlantState.Senior:
                Leafs.Play(true);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);

                break;

            default:
                
                break;
        }

    }
    IEnumerator SproutSkouiz(PlantState state)
    {
        if (increase == true)
        {
            switch (state - 1)
            {
                case PlantState.Dead:
                    transform.DOScaleY(0.2f, 0.4f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                case PlantState.Agony:
                    transform.DOScaleY(0.1f, 0.4f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                case PlantState.Baby:
                    //Debug.Log("babytoadult");
                    transform.DOScaleY(0.2f, 0.4f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.4f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                case PlantState.Young:

                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                case PlantState.Adult:

                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                case PlantState.Senior:

                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                default:
                    ;
                    break;
            
            }
        }
        else
        {
            switch (state + 1)
            {
                case PlantState.Dead:
                    transform.DOScaleY(0.1f, 0.3f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.1f, 0.3f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                case PlantState.Agony:
                    transform.DOScaleY(0.2f, 0.3f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.3f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                case PlantState.Baby:
                    transform.DOScaleY(0.2f, 0.3f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.3f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                case PlantState.Young:

                    transform.DOScaleY(0.2f, 0.3f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.3f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                case PlantState.Adult:
                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    break;

                case PlantState.Senior:
                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
                    
                    break;

                default:
                    break;
                    
            }
        }
        StartCoroutine(TreeSkouiz(plant.currentState));
    }

    
}
