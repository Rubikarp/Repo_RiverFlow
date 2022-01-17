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
    private bool isDead = false;
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
    public float nowaterTime;
    public ParticleSystem LeafsDefault;
    public ParticleSystem LeafsDesert;
    public ParticleSystem LeafsSavana;
    public ParticleSystem MiniWave;
    public ParticleSystem NoWater;


    [Header("Sound")]
    public string growingSound = "Growing";
    public string sproutSpawn = "SproutSpawn";
    //[Header("Fade")]
    //public float lerpToGrey;
    //public float lerpToWhite;

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
        //imgTiller.fillAmount = plant.timer;
        //FadeColors();
        ////imgTiller.fillAmount = plant.timer;
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(Twitch());
        //}
        NoWaterFeedback();
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
        SpawnFeedback();
        
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
        //transform.DOScaleY(1.1f, 0.5f).SetEase(Ease.OutElastic);
        //transform.DOScaleX(1.1f, 0.5f).SetEase(Ease.OutElastic);
        yield return new WaitForSecondsRealtime(0.05f);
        //transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutElastic);
        //transform.DOScaleX(1f, 0.5f).SetEase(Ease.OutElastic);
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
    //public void FadeColors() 
    //{
    //    bool isWhite = true;
    //    if (plant.isIrrigated == false)
    //    {
    //        this.sprRender.color = Color.Lerp(Color.white, Color.grey, ((1-plant.timer) * plant.gameTime.gameTimeSpeed * lerpToGrey));
    //        if (isWhite == true)
    //        {
    //            isWhite = false;
    //        }

    //    }
    //    else if (plant.isIrrigated == true && isWhite ==false)
    //    {
    //        this.sprRender.color = Color.Lerp(Color.gray, Color.white, (plant.gameTime.gameTimer * lerpToWhite));
    //        if (this.sprRender.color == Color.white)
    //        {
    //            isWhite = true;
    //        }
    //    }

    //}

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
        LevelSoundboard.Instance.ChangeGrowPitch();
        LevelSoundboard.Instance.PlayGrowEffectSound(growingSound);
    }
    public void SpawnFeedback()
    {
        switch (plant.TileOn.type)
        {
            case TileType.grass:
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
        LevelSoundboard.Instance.PlaySpawnEffectSound(sproutSpawn);

    }
    public void NoWaterFeedback()
    {
        
        //Debug.Log(plant.hasDiedRecently);
        bool playing = false;
        if (plant.timeWithoutIrrigation >=nowaterTime && playing ==false && plant.hasDiedRecently == false && isDead==false)
        {
            NoWater.Play(true);
            playing = true;
        }
        else if (plant.timeWithoutIrrigation < nowaterTime)
        {
            NoWater.Stop(true);
            playing = false;
        }
        else if (plant.hasDiedRecently == true&&isDead==false)
        {
            NoWater.Stop(true);
            Destroy(NoWater);
            isDead = true;
        }

    }
}
