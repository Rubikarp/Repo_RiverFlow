using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Canal
{
    public GameTile startNode;
    public GameTile endNode;
    [Space(10)]
    public RiverStrenght flowStrenght;
    public List<GameTile> tiles;

}
