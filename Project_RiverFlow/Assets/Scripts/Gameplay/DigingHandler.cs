using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class linkEvent : UnityEvent<GameTile, GameTile> { }

public class DigingHandler : MonoBehaviour
{
    public InputHandler input;
    public GameGrid grid;
    [Space(10)]
    public linkEvent onLink;

    void Start()
    {
        input.onLeftClicking.AddListener(OnLeftClicking);
        input.onRightClicking.AddListener(OnRighClicking);
    }
    void Update()
    {
        
    }

    public void OnLeftClicking()
    {
        //Have drag a certainDistance        
        if (Mathf.Abs(input.dragVect.x) > grid.cellSize || Mathf.Abs(input.dragVect.y) > grid.cellSize)
        {
            Vector3 drag = input.GetHitPos() - input.startSelectTilePos;
            //Si je dépasse de plus d'une case d'écart
            if (drag.magnitude > (1.5f *grid.cellSize))
            {
                drag = drag.normalized * (1.5f * grid.cellSize);
            }
            //Check la ou je touche
            input.endSelectTile = grid.GetTile(grid.PosToTile(input.startSelectTilePos + drag));
            input.endSelectPos = input.dragPos;

            //Si j'ai bien 2 tile linkable
            if (input.startSelectTile != null && input.endSelectTile != null)
            {
                if (input.startSelectTile.isLinkable && input.endSelectTile.isLinkable)
                {
                    //Make the Link
                    input.startSelectTile.isDuged = true;
                    input.startSelectTile.AddLinkedTile(input.endSelectTile);
                    ///TODO :startSelectTileGround.flowOut.Add();
                    
                    input.endSelectTile.isDuged = true;
                    input.endSelectTile.AddLinkedTile(input.startSelectTile);
                    ///TODO :endSelectTileGround.flowIn.Add();

                }

                //Event
                onLink?.Invoke(input.startSelectTile, input.endSelectTile);

                //End became the new start
                input.startSelectTile = input.endSelectTile;
                input.startSelectTilePos = grid.TileToPos(input.startSelectTile.position);
            }
        }
    }
    public void OnRighClicking()
    {
        input.eraserSelectTile.RemoveAllLinkedTile();
    }

}
