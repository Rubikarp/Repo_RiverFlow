using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject forestSprite;
    public GameObject savanaSprite;
    public GameObject desertSprite;

    private void Start()
    {
        switch (tileOn.type)
        {
            case TileType.grass:
                forestSprite.SetActive(true);
                savanaSprite.SetActive(false);
                desertSprite.SetActive(false);
                break;

            case TileType.clay:
                forestSprite.SetActive(false);
                savanaSprite.SetActive(true);
                desertSprite.SetActive(false);
                break;

            case TileType.sand:
                forestSprite.SetActive(false);
                savanaSprite.SetActive(false);
                desertSprite.SetActive(true);
                break;

            default:
                break;

        }
    }
}
