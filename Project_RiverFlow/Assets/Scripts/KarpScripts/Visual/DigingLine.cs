using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class DigingLine : MonoBehaviour
{
    public InputHandler input;
    public DigingHandler dig;
    [Space(10)]
    public GameGrid grid;
    public Line line;

    void Update()
    {
        if (dig.canDig)
        {
            if (dig.startSelectTilePos != null)
            {
                line.Start = dig.startSelectTilePos;
                line.End = dig.dragPos;
            }
        }

        line.DashOffset += Time.deltaTime;
        line.DashOffset %= 1f;
    }
}
