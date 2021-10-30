using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Element 
{
    public GameTile[] TilesOn { get; set; }
    public bool isLinkable { get; }
}
