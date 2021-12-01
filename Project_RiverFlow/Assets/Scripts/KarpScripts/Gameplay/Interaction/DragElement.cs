using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragElement : MonoBehaviour,
    IDragHandler,
    IBeginDragHandler,
    IEndDragHandler
{
    public ElementHandler elementManage;
    public InputHandler input;
    public GameGrid grid;
    public Transform preview;

    //Call Once
    public void OnBeginDrag(PointerEventData eventData)
    {
        //initialize pointer visual
        preview.gameObject.SetActive(true);
    }

    //Call on Event update
    public void OnDrag(PointerEventData eventData)
    {
        preview.position = grid.TileToPos(grid.PosToTile(input.GetHitPos()));
    }

    //Call Once
    public void OnEndDrag(PointerEventData eventData)
    {
        //Init the linked object on grid
        elementManage.SpawnPlantAt(grid.PosToTile(input.GetHitPos()));
        preview.position = Vector3.zero;
        preview.gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
