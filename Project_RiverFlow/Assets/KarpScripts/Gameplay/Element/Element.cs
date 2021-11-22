using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public virtual GameTile TileOn { get; set; }
    public virtual GameTile[] TilesOn { get; set; }
    public virtual bool isLinkable { get { return false; } }
    public virtual bool IsLinkable()
    {
        return false;
    }

    public virtual void AddLink(Direction dir) { }

}
