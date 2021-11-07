using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigingLine : MonoBehaviour
{
    public InputHandler input;
    public DigingHandler dig;
    [Space(10)]
    public GameGrid grid;
    public LineRenderer line;

    void Start()
    {
        input.onLeftClickDown.AddListener(OnLeftClickPress);
        input.onLeftClicking.AddListener(OnLeftClicking);
        input.onLeftClickUp.AddListener(OnLeftClickRelease);

        dig.digMove.AddListener(OnDigMove);
    }

    void Update()
    {
        
    }

    public void OnLeftClickPress()
    {
        line.positionCount = 1;
        line.SetPosition(0, input.startSelectTilePos);
    }
    public void OnLeftClicking()
    {
        line.positionCount = 2;
        line.SetPosition(1, input.dragPos);
    }
    public void OnLeftClickRelease()
    {
        line.positionCount = 0;
    }

    public void OnDigMove(GameTile startSelectTile, GameTile endSelectTile)
    {
        line.SetPosition(0, grid.TileToPos(endSelectTile.data.gridPos));
    }

}
