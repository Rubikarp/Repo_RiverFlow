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

    void Update()
    {
        if (dig.startSelectTilePos != null)
        {
            line.positionCount = 2;

            line.SetPosition(0, dig.startSelectTilePos);
            line.SetPosition(1, dig.dragPos);
        }
        else
        {
            line.positionCount = 0;
        }

    }
}
