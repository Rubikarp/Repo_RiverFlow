using System;
using UnityEngine;

[Serializable]
public struct Direction
{
    [Header("Variable")]
    public Vector2Int dirValue;
    public DirectionEnum dirEnum;
    public int x { set { dirValue.x = value; } }
    public int y { set { dirValue.y = value; } }

    #region Constructor
    public Direction(int _x, int _y)
    {
        this.dirValue = new Vector2Int(_x, _y);
        this.dirEnum = DirectionEnum.center;

        DirectionEnum temp = Vec2ToDirEnum(dirValue);
        this.dirEnum = temp;
    }
    public Direction(Vector2Int _value)
    {
        this.dirValue = _value;
        this.dirEnum = DirectionEnum.center;

        DirectionEnum temp = Vec2ToDirEnum(dirValue);
        this.dirEnum = temp;
    }
    public Direction(DirectionEnum _dirEnum)
    {
        this.dirValue = Vector2Int.zero;
        this.dirEnum = _dirEnum;

        Vector2Int temp = DirEnumToVec2(_dirEnum);
        this.dirValue = temp;
    }
    public Direction(int _intDir)
    {
        this.dirValue = Vector2Int.zero;
        this.dirEnum = (DirectionEnum)_intDir;

        Vector2Int temp = DirEnumToVec2((DirectionEnum)_intDir);
        this.dirValue = temp;
    }
    public Direction(Vector2Int _value, DirectionEnum _dirEnum)
    {
        this.dirValue = _value;
        this.dirEnum = _dirEnum;
    }
    #endregion

    #region StaticVariable
    public static Direction up = new Direction(new Vector2Int(0, 1), DirectionEnum.up);
    public static Direction down = new Direction(new Vector2Int(0, -1), DirectionEnum.down);
    public static Direction left = new Direction(new Vector2Int(-1, 0), DirectionEnum.left);
    public static Direction right = new Direction(new Vector2Int(1, 0), DirectionEnum.right);
    public static Direction center = new Direction(new Vector2Int(0, 0), DirectionEnum.center);
    public static Direction upLeft = new Direction(new Vector2Int(-1, 1), DirectionEnum.upLeft);
    public static Direction upRight = new Direction(new Vector2Int(1, 1), DirectionEnum.upRight);
    public static Direction downLeft = new Direction(new Vector2Int(-1, -1), DirectionEnum.downLeft);
    public static Direction downRight = new Direction(new Vector2Int(1, -1), DirectionEnum.downRight);
    #endregion

    //Conversion
    public static DirectionEnum Vec2ToDirEnum(Vector2Int dir)
    {
        dir.x = Mathf.Clamp(dir.x, -1, 1);
        dir.y = Mathf.Clamp(dir.y, -1, 1);

        #region HandSwitch
        if (dir == upLeft.dirValue)
        {
            return upLeft.dirEnum;
        }
        else
        if (dir == up.dirValue)
        {
            return up.dirEnum;
        }
        else
        if (dir == upRight.dirValue)
        {
            return upRight.dirEnum;
        }
        else
        if (dir == right.dirValue)
        {
            return right.dirEnum;
        }
        else
        if (dir == downRight.dirValue)
        {
            return downRight.dirEnum;
        }
        else
        if (dir == down.dirValue)
        {
            return down.dirEnum;
        }
        else
        if (dir == downLeft.dirValue)
        {
            return downLeft.dirEnum;
        }
        else
        if (dir == left.dirValue)
        {
            return left.dirEnum;
        }
        #endregion

        return center.dirEnum;
    }
    public static Vector2Int DirEnumToVec2(DirectionEnum dir)
    {
        switch (dir)
        {
            case DirectionEnum.upLeft:
                return upLeft.dirValue;

            case DirectionEnum.up:
                return up.dirValue;

            case DirectionEnum.upRight:
                return upRight.dirValue;

            case DirectionEnum.right:
                return right.dirValue;

            case DirectionEnum.downRight:
                return downRight.dirValue;

            case DirectionEnum.down:
                return down.dirValue;

            case DirectionEnum.downLeft:
                return downLeft.dirValue;

            case DirectionEnum.left:
                return left.dirValue;

            case DirectionEnum.center:
                return center.dirValue;

            default:
                return center.dirValue;
        }
    }
    public static Direction DirFromInt(int _dirIndex)
    {
        return new Direction(_dirIndex);
    }
    public static Quaternion ToQuat2D(DirectionEnum _dirIndex)
    {
        switch (_dirIndex)
        {
            case DirectionEnum.left:
                return new Quaternion(0, 0, 1f, 0f).normalized;
                break;
            case DirectionEnum.right:
                return new Quaternion(0, 0, 0f, 1f).normalized;
                break;
            case DirectionEnum.up:
                return new Quaternion(0, 0, 0.7071068f, 0.7071068f).normalized;
                break;
            case DirectionEnum.down:
                return new Quaternion(0, 0, -0.7071068f, 0.7071068f).normalized;
                break;

            case DirectionEnum.upLeft:
                return new Quaternion(0, 0, 0.9238796f, 0.3826834f).normalized;
                break;
            case DirectionEnum.upRight:
                return new Quaternion(0, 0, 0.3826834f, 0.9238796f).normalized;
                break;
            case DirectionEnum.downRight:
                return new Quaternion(0, 0, -0.3826834f, 0.9238796f).normalized;
                break;
            case DirectionEnum.downLeft:
                return new Quaternion(0, 0, -0.9238796f, 0.3826834f).normalized;
                break;

            case DirectionEnum.center:
                return Quaternion.identity;
                break;

            default:
                return Quaternion.identity;
                break;
        }
    }

    //Methode
    public static bool IsDiagonal(Direction dir)
    {
        return ((int)dir.dirEnum % 2) == 0;
    }
    public static Direction Inverse(Direction dir)
    {
        return new Direction(dir.dirValue * (-Vector2Int.one));
    }
    public static Vector2Int ClampDir(Vector2Int dir)
    {
        dir.x = Mathf.Clamp(dir.x, -1, 1);
        dir.y = Mathf.Clamp(dir.y, -1, 1);

        return dir;
    }


}
