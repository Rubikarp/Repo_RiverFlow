using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DigingHandler : MonoBehaviour
{
    public InputHandler input;
    public GameGrid grid;
    [Space(10)]
    public UnityEvent onLink;

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
            //Check la ou je touche
            input.endSelectTile = grid.GetTile(grid.PosToTile(input.GetHitPos()));
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

                //End became the new start
                input.startSelectTile = input.endSelectTile;
                input.startSelectTilePos = grid.TileToPos(input.startSelectTile.position);

                onLink?.Invoke();
            }
        }
    }
    public void OnRighClicking()
    {
        input.eraserSelectTile.RemoveAllLinkedTile();
    }

}
