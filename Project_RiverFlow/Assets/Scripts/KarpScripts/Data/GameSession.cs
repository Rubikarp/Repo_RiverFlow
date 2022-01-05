using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : Singleton<GameSession>
{
    [Header("Level state")]
    public List<Canal> canals = new List<Canal>();
    [Header("Level element")]
    public List<Plant> plants = new List<Plant>();
    public List<WaterSource> sources = new List<WaterSource>();
    [Header("Level item posed")]
    public List<Cloud> clouds = new List<Cloud>();
    public List<Lake> lakes = new List<Lake>();
    public List<MagicTree> magicTrees = new List<MagicTree>();
    [Space(20)]
    [Header("Player Inventory")]
    public int digAmmount;
    [Header("Player Item")]
    public int sourcesAmmount;
    public int lakesAmmount;
    public int cloudsAmmount;
    public int tunnelsAmmount;
}
