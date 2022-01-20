using UnityEngine;

public static class PositionRendererSorter
{
    public static void SortTreePositionOderInLayer(Renderer rendererToSort, Transform objectWorldPosition)
    {
        rendererToSort.sortingOrder = (int)(Mathf.Floor(objectWorldPosition.position.y)) * 4 * -1;
    }

    public static void SortPositionOrderInLayer(Renderer rendererToSort, Renderer attachedTreeRenderer, Transform objectWorldPosition, bool isTreeShadow)
    {
        if (isTreeShadow)
        {
            rendererToSort = attachedTreeRenderer;
        }
    }
}
