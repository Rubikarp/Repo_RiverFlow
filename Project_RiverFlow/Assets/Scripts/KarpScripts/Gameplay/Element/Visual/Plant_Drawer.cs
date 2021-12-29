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
    public List <float>twitchFrequency;
    [SerializeField]
    private int currentTwitch = 0;
    private bool increase =true;

    [Header("Particles")]
    public ParticleSystem LeafsDefault;
    public ParticleSystem LeafsDesert;
    public ParticleSystem LeafsSavana;
    public ParticleSystem MiniWave;

    void Start()
    {
        cam = Camera.main;
        canvas.worldCamera = cam;

        sprRender = gameObject.GetComponent<SpriteRenderer>();

        sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
        plant.onStateChange.AddListener(UpdateSkin);
        //apparition
        StartCoroutine(Apparition());
    }

    void Update()
    {
        TwitchTiming();
        ////imgTiller.fillAmount = plant.timer;
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(Twitch());
        //}
    }

    private void UpdateSkin(bool isUp)
    {
        //Debug.Log("test");
        increase = isUp;
        //sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
        //StopAllCoroutines();
        StartCoroutine(SproutSkouiz(plant.currentState));
    }
    IEnumerator Apparition()
    {
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSecondsRealtime(0.1f);
        transform.DOScaleY(1f, 1.3f).SetEase(Ease.OutElastic);
        yield return new WaitForSecondsRealtime(0.1f);
        transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);
        UpgradeFeedback();
    }
    IEnumerator TreeSkouiz(PlantState state)
    {
        currentTwitch = 0;

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
                UpgradeFeedback();

                transform.localScale = new Vector3(0.3f, 0, 0);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutQuart);
                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);

                break;
            case PlantState.Adult:
                UpgradeFeedback();

                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);

                break;
            case PlantState.Senior:
                UpgradeFeedback();

                yield return new WaitForSecondsRealtime(0.1f);
                transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);

                break;
            case PlantState.FruitTree:
                UpgradeFeedback();

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
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.Agony:
                    transform.DOScaleY(0.1f, 0.4f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.Baby:
                    //Debug.Log("babytoadult");
                    transform.DOScaleY(0.2f, 0.4f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.4f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.Young:

                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.Adult:

                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.Senior:

                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.FruitTree:

                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                default:
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
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.Agony:
                    transform.DOScaleY(0.2f, 0.3f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.3f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.Baby:
                    transform.DOScaleY(0.2f, 0.3f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.3f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.Young:

                    transform.DOScaleY(0.2f, 0.3f).SetEase(Ease.InElastic);
                    transform.DOScaleX(0.2f, 0.3f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.Adult:
                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    break;

                case PlantState.Senior:
                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);
                    
                    break;

                case PlantState.FruitTree:
                    transform.DOScaleX(0.4f, 0.4f).SetEase(Ease.InElastic);
                    yield return new WaitForSecondsRealtime(0.3f);
                    sprRender.sprite = visual.GetSprite(plant.currentState, plant.TileOn.type);

                    break;

                default:
                    break;
                    
            }
        }
        StartCoroutine(TreeSkouiz(plant.currentState));
    }
    IEnumerator Twitch()
    {
        //Debug.Log("Boing");
        transform.DOScaleY(1.1f, 0.5f).SetEase(Ease.OutElastic);
        transform.DOScaleX(1.1f, 0.5f).SetEase(Ease.OutElastic);
        yield return new WaitForSecondsRealtime(0.05f);
        transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutElastic);
        transform.DOScaleX(1f, 0.5f).SetEase(Ease.OutElastic);
        MiniWave.Play(true);
    }
    public void TwitchTiming()
    {
        if (plant.isIrrigated)
        {
            if (twitchFrequency[currentTwitch] <= plant.timer%1)
            {
                StartCoroutine(Twitch());

                currentTwitch++;
                currentTwitch = currentTwitch % (twitchFrequency.Count);
            }
        }
    }

    public void UpgradeFeedback()
    {
        switch (plant.TileOn.type)
        {
            case TileType.grass :
                LeafsDefault.Play(true);
                break;
            case TileType.clay:
                LeafsSavana.Play(true);
                break;
            case TileType.sand:
                LeafsDesert.Play(true);
                break;
            default:

                break;
        }

    }
}
