using UnityEngine;

public enum TileState
{
    Full,
    Hole
}
public enum TileType
{
    soil,
    clay,
    sand
}

public class Tile : MonoBehaviour
{
    public Vector2Int pos;
    public TileState state;
    public TileType type;

    //Constructor
    public Tile(Vector2Int pos, TileType type = TileType.soil, TileState state = TileState.Full)
    {
        this.pos = pos;
        this.type = type;
        this.state = state;
    }
    public Tile(Vector2Int pos, TileState state = TileState.Full, TileType type = TileType.soil)
    {
        this.pos = pos;
        this.type = type;
        this.state = state;
    }

    public void SetValue(Vector2Int pos, TileType type = TileType.soil, TileState state = TileState.Full)
    {
        this.pos = pos;
        this.type = type;
        this.state = state;
    }

}