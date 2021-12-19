using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable] public class LinkEvent : UnityEvent<GameTile, GameTile> { }
[Serializable] public class TileEvent : UnityEvent<GameTile> { }

public class DigingHandler : MonoBehaviour
{
    [Header("Reférence")]
    public InputHandler input;
    public GameGrid grid;
    public GameTime timer;

    [Header("Event")]
    public LinkEvent onLink;
    public TileEvent onBreak;
    
    [Header("Digging")]
    [SerializeField] public bool canDig = true;
    [SerializeField] public GameTile startSelectTile;
    [SerializeField] public Vector3 startSelectTilePos;
    [Space(10)]
    [SerializeField] private GameTile endSelectTile;
    [SerializeField] public Vector3 endSelectPos;
    [Space(10)]
    [SerializeField] public Vector3 dragPos;
    [SerializeField] public Vector3 dragVect;

    [Header("Eraser")]
    [SerializeField] public GameTile eraserSelectTile;
    private GameTile lastEraserSelectTile;

    [Header("Digging")]
    public InventoryManager inventory;

    void Start()
    {
        input.onLeftClickDown.AddListener(OnLeftClick);
        input.onLeftClicking.AddListener(OnLeftClicking);
        input.onLeftClickUp.AddListener(OnLeftClickRelease);

        input.onRightClicking.AddListener(OnRighClicking);
        input.onRightClickUp.AddListener(OnRightClickRelease);

    }

    private void OnDestroy()
    {
        input.onLeftClickDown.RemoveListener(OnLeftClick);
        input.onLeftClicking.RemoveListener(OnLeftClicking);
        input.onLeftClickUp.RemoveListener(OnLeftClickRelease);

        input.onRightClicking.RemoveListener(OnRighClicking);
        input.onRightClickUp.RemoveListener(OnRightClickRelease);

    }

    //Digging
    private void OnLeftClick()
    {
        //Initialise start select
        startSelectTile = grid.GetTile(grid.PosToTile(input.GetHitPos()));
        startSelectTilePos = grid.TileToPos(startSelectTile.gridPos);
    }
    private void OnLeftClicking()
    {
        dragPos = input.GetHitPos();
        dragVect = (dragPos - startSelectTilePos);

        //Have drag a certainDistance        
        if (Mathf.Abs(dragVect.x) > grid.cellSize || Mathf.Abs(dragVect.y) > grid.cellSize)
        {
            Vector3 drag = input.GetHitPos() - startSelectTilePos;
            //Si je dépasse de plus d'une case d'écart
            if (drag.magnitude > (1.5f * grid.cellSize))
            {
                drag = drag.normalized * (1.5f * grid.cellSize);
            }
            //Check la ou je touche
            endSelectTile = grid.GetTile(grid.PosToTile(startSelectTilePos + drag));
            endSelectPos = dragPos;

            if (canDig)
            {
                if (startSelectTile.isLinkable && endSelectTile.isLinkable)
                {
                    if (!CheckCrossADiagonal(startSelectTile, endSelectTile))
                    {

                        if (inventory.digAmmount > 0)
                        {
                            Vector2Int tileToMe = endSelectTile.gridPos - startSelectTile.gridPos;
                            Direction linkDir = new Direction(tileToMe);

                            if (startSelectTile.IsLinkInDir(linkDir, FlowType.flowOut))
                            {
                                ///link in the same sens
                                //Do nothing
                            }
                            else
                            if (startSelectTile.IsLinkInDir(linkDir, FlowType.flowIn))
                            {
                                ///TODO : link in the opposite sens
                                //Do nothing
                            }
                            else
                            {
                                //Event
                                onLink?.Invoke(startSelectTile, endSelectTile);
                                inventory.digAmmount--;
                            }
                        }
                    }
                }

            }
            //End became the new start
            startSelectTile = endSelectTile;
            startSelectTilePos = grid.TileToPos(startSelectTile.gridPos);
        }
    }

    private bool CheckCrossADiagonal(GameTile tileA, GameTile tileB)
    {
        Vector2Int A2B = tileB.gridPos - tileA.gridPos;
        Direction dir = new Direction(A2B);
        if (!Direction.IsDiagonal(dir))
        {
            return false;
        }
        else
        {
            bool cross = false;
            switch (dir.dirEnum)
            {
                case DirectionEnum.upLeft:
                    cross = tileA.GetNeighbor(
                            new Direction(DirectionEnum.up)).
                            IsLinkTo(tileA.GetNeighbor(
                            new Direction(DirectionEnum.left)
                            ));
                    break;
                case DirectionEnum.upRight:
                    cross = tileA.GetNeighbor(
                            new Direction(DirectionEnum.up)).
                            IsLinkTo(tileA.GetNeighbor(
                            new Direction(DirectionEnum.right)
                            ));
                    break;
                case DirectionEnum.downRight:
                    cross = tileA.GetNeighbor(
                            new Direction(DirectionEnum.down)).
                            IsLinkTo(tileA.GetNeighbor(
                            new Direction(DirectionEnum.right)
                            ));
                    break;
                case DirectionEnum.downLeft:
                    cross = tileA.GetNeighbor(
                            new Direction(DirectionEnum.down)).
                            IsLinkTo(tileA.GetNeighbor(
                            new Direction(DirectionEnum.left)
                            ));
                    break;
                default:
                    Debug.LogError("ça va pas");
                    break;
            }
            return cross;
        }
    }

    private void OnLeftClickRelease()
    {
        //Reset
        startSelectTile = null;
        startSelectTilePos = Vector3.zero;
        //
        endSelectTile = null;
        endSelectPos = Vector3.zero;
        //
        dragPos = Vector3.zero;
        dragVect = Vector3.zero;
    }

    //Undigging
    private void OnRighClicking()
    {
        if (canDig)
        {
            eraserSelectTile = grid.GetTile(grid.PosToTile(input.GetHitPos()));

            if (eraserSelectTile.linkAmount > 0 || eraserSelectTile.haveElement)
            {
                if (lastEraserSelectTile != eraserSelectTile)
                {
                    lastEraserSelectTile = eraserSelectTile;
                    inventory.digAmmount += eraserSelectTile.linkAmount;

                    onBreak?.Invoke(eraserSelectTile);
                }
            }
        }
    }
    private void OnRightClickRelease()
    {
        eraserSelectTile = null;
        lastEraserSelectTile = null;

    }

    public void RemoveElement(Element element)
    {
        if (element is WaterSource || element is Plant)
        {
            Debug.LogError("Nope tu peux pas");
            return;
        }
        else if (element is Cloud)
        {
            element.TileOn = null;
            element.TileOn = null;

            Destroy(element.gameObject);
            inventory.cloudsAmmount++;
        }
        else if (element is Lake)
        {
            for (int i = 0; i < element.TilesOn.Length; i++)
            {
                element.TilesOn[i].element = null;
                element.TilesOn[i] = null;

                Destroy(element.gameObject);
            }
            inventory.lakesAmmount++;
        }
    }
}
