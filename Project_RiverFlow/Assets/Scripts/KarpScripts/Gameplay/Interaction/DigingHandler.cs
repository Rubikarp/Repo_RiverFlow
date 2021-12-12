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
    [SerializeField] public GameTile startSelectTile;
    [SerializeField] public Vector3 startSelectTilePos;
    [Space(10)]
    [SerializeField] public GameTile endSelectTile;
    [SerializeField] public Vector3 endSelectPos;
    [Space(10)]
    [SerializeField] public Vector3 dragPos;
    [SerializeField] public Vector3 dragVect;

    [Header("Eraser")]
    [SerializeField] public GameTile eraserSelectTile;

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
    public void OnLeftClick()
    {
        //Initialise start select
        startSelectTile = grid.GetTile(grid.PosToTile(input.GetHitPos()));
        startSelectTilePos = grid.TileToPos(startSelectTile.gridPos);
    }

    public void OnLeftClicking()
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

            //Si j'ai bien 2 tile linkable
            if (startSelectTile != null && endSelectTile != null)
            {
                if (startSelectTile.isLinkable && endSelectTile.isLinkable)
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
            
            //End became the new start
            startSelectTile = endSelectTile;
            startSelectTilePos = grid.TileToPos(startSelectTile.gridPos);
        }
    }

    public void OnLeftClickRelease()
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
    public void OnRighClicking()
    {
        eraserSelectTile = grid.GetTile(grid.PosToTile(input.GetHitPos()));

        if (eraserSelectTile.linkAmount > 0)
        {
            inventory.digAmmount += eraserSelectTile.linkAmount;
            onBreak?.Invoke(eraserSelectTile);
        }
    }
    public void OnRightClickRelease()
    {
        eraserSelectTile = null;
    }



}
