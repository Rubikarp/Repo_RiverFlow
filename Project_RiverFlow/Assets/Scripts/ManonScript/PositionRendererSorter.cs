using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRendererSorter : MonoBehaviour
{
    public void SortTreePositionOderInLayer(Renderer rendererToSort, Transform objectWorldPosition)
    {
            rendererToSort.sortingOrder = (int)(Mathf.Floor(objectWorldPosition.position.y)) * 4 * -1;        
    }

    public void SortPositionOrderInLayer(Renderer rendererToSort, Renderer attachedTreeRenderer, Transform objectWorldPosition, bool isTreeShadow)
    {
        if (isTreeShadow)
        {
            rendererToSort = attachedTreeRenderer;
        }
    }
}
