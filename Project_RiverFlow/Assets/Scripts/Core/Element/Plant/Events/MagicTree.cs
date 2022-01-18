using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicTree : Element
{
    #region Element
    [Header("Element Data")]
    public GameTile tileOn;
    public override GameTile TileOn
    {
        get
        {
            if (tileOn == null)
            {
                Debug.LogError("Can't find the tile where is this WaterSource", this);
                return null;
            }
            return tileOn;
        }
        set
        { tileOn = value; }
    }
    public override GameTile[] TilesOn
    {
        get
        {
            if (tileOn == null)
            {
                Debug.LogError("Can't find the tile where is this WaterSource", this);
                return new GameTile[1] { null };
            }
            return new GameTile[1] { tileOn };
        }
        set { tileOn = value[0]; }
    }
    //Don't overide other methode because is not linkable
    #endregion

    public TimeManager gameTime;

    [Header("Scoring")]
    public ScoreManager scoreManager;
    public int magicTreeScoring;
    private float scoringTimer;
    public int scoringTick;

    [Header("Sprites")]
    public Sprite forestSprite;
    public Sprite savanaSprite;
    public Sprite desertSprite;
    public SpriteRenderer sprite;


    private void Start()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        gameTime = TimeManager.Instance;
        scoreManager = ScoreManager.Instance;

        switch (tileOn.type)
        {
            case TileType.grass:
                sprite.sprite = forestSprite;
                break;

            case TileType.clay:
                sprite.sprite = savanaSprite;
                break;

            case TileType.sand:
                sprite.sprite = desertSprite;
                break;

            default:
                break;

        }


        // layer
        PositionRendererSorter.SortTreePositionOderInLayer(sprite,this.transform);
        //tweening

        transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutQuart);
        transform.DOScaleX(1f, 1.3f).SetEase(Ease.OutElastic);
    }

    private void Update()
    {
        Scoring();
    }

    private void Scoring()
    {
        scoringTimer += Time.deltaTime * gameTime.gameTimeSpeed;

        if (scoringTimer >= scoringTick && gameTime.isPaused == false)
        {
            scoreManager.gameScore += magicTreeScoring;

            scoringTimer = 0;
        }

    }
}
