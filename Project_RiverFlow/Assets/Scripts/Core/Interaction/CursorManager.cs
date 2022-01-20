using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CursorManager : MonoBehaviour
{
    public GameGrid grid;
    public Transform preview;
    public SpriteRenderer previewSprite;
    public InputHandler inputHandler;
    [Header("SpritesPreview")]
    public GameObject cloudPreview;
    public Sprite lakePreview;
    public Sprite sourcePreview;
    [Header("Texture cursor")]
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot;
    public Texture2D defaultCursor;
    public Texture2D digCursor;
    public Texture2D eraserCursor;


    // Start is called before the first frame update
    void Awake()
    {
        grid = GameGrid.Instance;
        inputHandler = InputHandler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (InputHandler.Instance.shortCutErasing == true)
        {
            OverRideCursorChange();
        }
        else
        {
            CursorChange();
        }
       

     
    }
    void OverRideCursorChange()
    {
        UnityEngine.Cursor.SetCursor(eraserCursor, hotSpot, cursorMode);
        preview.transform.localRotation = Quaternion.Euler(0, 0, 0);
        preview.gameObject.SetActive(false);
    }
    void CursorChange()
    {
        GameTile testedTile = grid.GetTile(grid.PosToTile(inputHandler.GetHitPos()));
        switch (inputHandler.mode)
        {
            case InputMode.diging:
                UnityEngine.Cursor.SetCursor(digCursor, hotSpot, cursorMode);
                preview.transform.localRotation = Quaternion.Euler(0, 0, 0);
                preview.gameObject.SetActive(false);
                break;
            case InputMode.eraser:
                UnityEngine.Cursor.SetCursor(eraserCursor, hotSpot, cursorMode);
                preview.transform.localRotation = Quaternion.Euler(0, 0, 0);
                preview.gameObject.SetActive(false);
                break;
            case InputMode.nothing:
                UnityEngine.Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
                preview.transform.localRotation = Quaternion.Euler(0, 0, 0);
                preview.gameObject.SetActive(false);

                break;
            case InputMode.lake:
                UnityEngine.Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
                cloudPreview.SetActive(false);
                preview.position = grid.TileToPos(grid.PosToTile(inputHandler.GetHitPos()));
                if (testedTile.ReceivedFlow() > FlowStrenght._00_)
                {

                    if (testedTile.linkAmount == 2)
                    {
                        Debug.Log("test");
                        preview.gameObject.SetActive(true);
                        List<GameTile> testedTileLinks = testedTile.GetLinkedTile();
                        //check if vertical
                        if ((testedTileLinks[0] == testedTile.neighbors[1] && testedTileLinks[1] == testedTile.neighbors[5])
                         || (testedTileLinks[0] == testedTile.neighbors[5] && testedTileLinks[1] == testedTile.neighbors[1]))
                        {
                            preview.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            previewSprite.sprite = lakePreview;
                            previewSprite.color = new Color(1, 1, 1, 1);
                        }
                        //check if horizontal
                        else
                        if ((testedTileLinks[0] == testedTile.neighbors[3] && testedTileLinks[1] == testedTile.neighbors[7])
                         || (testedTileLinks[0] == testedTile.neighbors[7] && testedTileLinks[1] == testedTile.neighbors[3]))
                        {
                            preview.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            previewSprite.sprite = lakePreview;
                            previewSprite.color = new Color(1, 1, 1, 0.6f);
                        }
                    }
                }
                break;
            case InputMode.cloud:
                UnityEngine.Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
                preview.transform.localRotation = Quaternion.Euler(0, 0, 0);
                preview.position = grid.TileToPos(grid.PosToTile(inputHandler.GetHitPos()));
                if (testedTile.ReceivedFlow() > FlowStrenght._00_)
                {
                    if (!testedTile.haveElement && testedTile.type != TileType.mountain)
                    {
                        if (testedTile.flowOut.Count < 2 && testedTile.flowIn.Count < 2)
                        {
                            preview.gameObject.SetActive(true);
                            previewSprite.color = new Color(1, 1, 1, 0);
                            cloudPreview.SetActive(true);
                        }
                    }
                }
                break;
            case InputMode.source:
                UnityEngine.Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
                cloudPreview.SetActive(false);
                preview.transform.localRotation = Quaternion.Euler(0, 0, 0);
                preview.position = grid.TileToPos(grid.PosToTile(inputHandler.GetHitPos()));

                if (testedTile.ReceivedFlow() == FlowStrenght._00_)
                {
                    if (!testedTile.haveElement && testedTile.type != TileType.mountain)
                    {
                        preview.gameObject.SetActive(true);
                        previewSprite.sprite = sourcePreview;
                        previewSprite.color = new Color(1, 1, 1, 0.6f);
                    }
                }

                break;
        }
    }
}