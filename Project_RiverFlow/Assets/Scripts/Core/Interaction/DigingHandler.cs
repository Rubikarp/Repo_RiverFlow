using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable] public class LinkEvent : UnityEvent<GameTile, GameTile> { }
[Serializable] public class TileEvent : UnityEvent<GameTile> { }

public class DigingHandler : MonoBehaviour
{
    [Header("Reférence")]
    public InputHandler input;
    public ElementHandler element;
    public GameGrid grid;
    public InventoryManager inventory;

    [Header("Event")]
    public LinkEvent onLink;
    public TileEvent onBreak;
    
    [Header("Digging")]
    [SerializeField] public GameTile startSelectTile;
    [SerializeField] public Vector3 startSelectTilePos;
    [Space(10)]
    [SerializeField] private GameTile endSelectTile;
    [SerializeField] public Vector3 endSelectPos;
    [Space(10)]
    [SerializeField] public Vector3 dragPos;
    [SerializeField] public Vector3 dragVect;
    [Space(10)]
    private GameTile lastSelectedTile;

    [Header("Sounds")]
    public string digSound = "Digging";
    public string eraseSound = "Erase";


    void OnEnable()
    {
        input.onInputPress.AddListener(InputPress);
        input.onInputMaintain.AddListener(InputMaintain);
        input.onInputRelease.AddListener(InputRelease);
    }

    private void OnDestroy()
    {
        input.onInputPress.RemoveListener(InputPress);
        input.onInputMaintain.RemoveListener(InputMaintain);
        input.onInputRelease.RemoveListener(InputRelease);
    }

    //InputMode
    private void InputPress(InputMode mode)
    {
        //Initialise start select
        startSelectTile = grid.GetTile(grid.PosToTile(input.GetHitPos()));
        startSelectTilePos = grid.TileToPos(startSelectTile.gridPos);

        switch (mode)
        {
            case InputMode.diging:
                //Nothing
                break;
            case InputMode.eraser:
                if (startSelectTile.linkAmount > 0 || startSelectTile.haveElement)
                {
                    if (lastSelectedTile != startSelectTile)
                    {
                        lastSelectedTile = startSelectTile;
                        onBreak?.Invoke(startSelectTile);
                    }
                }
                break;
                ///////
            case InputMode.source:
                if (inventory.sourcesAmmount > 0 && !startSelectTile.haveElement && startSelectTile.type != TileType.mountain)// 
                {
                    element.SpawnWaterSourceAt(grid.PosToTile(input.GetHitPos()));
                    inventory.sourcesAmmount--;
                    input.ChangeMode(InputMode.diging);
                }
                break;
            case InputMode.cloud:
                if (inventory.cloudsAmmount > 0 && !startSelectTile.haveElement && startSelectTile.type != TileType.mountain)// 
                {
                    if (startSelectTile.flowOut.Count < 2 && startSelectTile.flowIn.Count < 2)
                    {
                        if (startSelectTile.ReceivedFlow() > FlowStrenght._00_) //
                        {
                            element.SpawnCloudAt(grid.PosToTile(input.GetHitPos()));
                            inventory.cloudsAmmount--;
                            input.ChangeMode(InputMode.diging);
                        }
                    }
                }
                break;
            case InputMode.lake:
                if (inventory.lakesAmmount > 0 && !startSelectTile.haveElement && startSelectTile.type != TileType.mountain)// 
                {
                    if (startSelectTile.linkAmount == 2)
                    {
                        if (startSelectTile.ReceivedFlow() > FlowStrenght._00_) //
                        {
                            List<GameTile> testedTileLinks = startSelectTile.GetLinkedTile();
                            //check if vertical
                            if ((testedTileLinks[0] == startSelectTile.neighbors[1] && testedTileLinks[1] == startSelectTile.neighbors[5])
                             || (testedTileLinks[0] == startSelectTile.neighbors[5] && testedTileLinks[1] == startSelectTile.neighbors[1]))
                            {
                                element.SpawnLakeAt(startSelectTile.gridPos, vertical: true);
                                inventory.lakesAmmount--;
                                input.ChangeMode(InputMode.diging);
                            }
                            //check if horizontal
                            else
                            if ((testedTileLinks[0] == startSelectTile.neighbors[3] && testedTileLinks[1] == startSelectTile.neighbors[7])
                             || (testedTileLinks[0] == startSelectTile.neighbors[7] && testedTileLinks[1] == startSelectTile.neighbors[3]))
                            {
                                element.SpawnLakeAt(startSelectTile.gridPos, vertical: false);
                                inventory.lakesAmmount--;
                                input.ChangeMode(InputMode.diging);
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
    }
    private void InputMaintain(InputMode mode)
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

            switch (mode)
            {
                case InputMode.diging:
                    if (inventory.digAmmount > 0)
                    {
                        if (startSelectTile.isLinkable && endSelectTile.isLinkable)
                        {
                            if (!CheckCrossADiagonal(startSelectTile, endSelectTile))
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
                                    //sounds
                                    LevelSoundboard.Instance.ChangePitchDig();
                                    LevelSoundboard.Instance.PlayDigEffectSound(digSound);
                                    LevelSoundboard.Instance.UpPitchDig();
                                    element.UpdateSoundRiver();

                                }
                            }
                        }
                    }
                    break;
                case InputMode.eraser:
                    if (endSelectTile.linkAmount > 0 || endSelectTile.haveElement)
                    {
                        if (lastSelectedTile != endSelectTile)
                        {

                            lastSelectedTile = endSelectTile;
                            onBreak?.Invoke(endSelectTile);
                            LevelSoundboard.Instance.PlayEraseEffectSound(eraseSound);
                            element.UpdateSoundRiver();
                        }
                    }
                    break;
                ///////
                case InputMode.source:
                    //Nothing
                    break;
                case InputMode.cloud:
                    //Nothing
                    break;
                case InputMode.lake:
                    //Nothing
                    break;
                default:
                    break;
            }

            //End became the new start
            startSelectTile = endSelectTile;
            startSelectTilePos = grid.TileToPos(startSelectTile.gridPos);
        }
    }
    private void InputRelease(InputMode mode)
    {
        //sound
        LevelSoundboard.Instance.InitializePitchDig();
        element.UpdateSoundRiver();

        //Moutain end
        if (endSelectTile != null)
        {
            if (endSelectTile.type == TileType.mountain && endSelectTile.linkAmount < 2)
            {
                onBreak?.Invoke(endSelectTile);
            }
        }

        //Reset
        startSelectTile = null;
        startSelectTilePos = Vector3.zero;
        //
        endSelectTile = null;
        endSelectPos = Vector3.zero;
        //
        dragPos = Vector3.zero;
        dragVect = Vector3.zero;
        //
        lastSelectedTile = null;
    }
    
    //
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
