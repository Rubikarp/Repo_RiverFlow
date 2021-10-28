using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Direction
{
    //Variable
    public Vector2Int dir;

    //Constructor
    public Direction(Vector2Int value)
    {
        this.dir = value;
        this.dir = ClampDir(dir);
    }
    public Direction(int _x, int _y)
    {
        this.dir = new Vector2Int(_x, _y);
        this.dir = ClampDir(dir);
    }

    public static Direction up = new Direction(new Vector2Int(0, 1));
    public static Direction down = new Direction(new Vector2Int(0, -1));
    public static Direction left = new Direction(new Vector2Int(-1, 0));
    public static Direction right = new Direction(new Vector2Int(1, 0));
    public static Direction upLeft = new Direction(new Vector2Int(-1, 1));
    public static Direction upRight = new Direction(new Vector2Int(1, 1));
    public static Direction downLeft = new Direction(new Vector2Int(-1, -1));
    public static Direction downRight = new Direction(new Vector2Int(1, -1));

    //Methode
    public Direction Inverse(Direction dir)
    {
        return new Direction(dir.dir * (-Vector2Int.one));
    }
    public Vector2Int ClampDir(Vector2Int dir)
    {
        dir.x = Mathf.Clamp(dir.x, -1, 1);
        dir.y = Mathf.Clamp(dir.y, -1, 1);

        return dir;
    }

}
