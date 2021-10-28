using UnityEngine;

namespace RiverFlow.V1
{
    public enum TileType
    {
        soil,
        clay,
        sand
    }
    public enum TileState
    {
        Full,
        Hole
    }
    public enum Direction
    {
        BottomLeft,
        Left,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        Null
    }


    public class OldTile : MonoBehaviour
    {
        #region Constructor
        public OldTile(Vector2Int pos, TileType type = TileType.soil, TileState state = TileState.Full)
        {
            this.pos = pos;
            this.type = type;
            this.state = state;
        }
        public OldTile(Vector2Int pos, TileState state = TileState.Full, TileType type = TileType.soil)
        {
            this.pos = pos;
            this.type = type;
            this.state = state;
        }
        #endregion
        
        #region Variable 
        [Header("Data")]
        public Vector2Int pos = new Vector2Int(0, 0);

        [Header("State")]
        public TileState state = TileState.Full;
        public TileType type = TileType.soil;

        [Header("Awarness")]
        public Tile[] neighborTile = new Tile[8];
        #endregion

        public void SetValue(Vector2Int pos, TileType type = TileType.soil, TileState state = TileState.Full)
        {
            this.pos = pos;
            this.type = type;
            this.state = state;
        }

    }
}