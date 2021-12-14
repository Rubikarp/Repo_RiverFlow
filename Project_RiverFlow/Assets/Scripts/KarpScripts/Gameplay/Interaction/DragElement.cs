using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public enum Items
{
    Cloud,
    Source,
    Lake
}
public class DragElement : MonoBehaviour,
    IDragHandler,
    IBeginDragHandler,
    IEndDragHandler
{
    public ElementHandler elementManage;
    public InputHandler input;
    public DigingHandler dig;
    public GameGrid grid;
    [Space(10)]
    public Transform preview;
    public TextMeshProUGUI textGUI;
    [SerializeField]
    private Items Item;
    //[SerializeField]
    //private Button SelfButton;
    [Space(10)]
    [SerializeField]
    private Image SelfImage;
    [SerializeField]
    private GameObject Text;
    public InventoryManager Inventory;

    //Call Once
    public void OnBeginDrag(PointerEventData eventData)
    {
        //initialize pointer visual
        preview.gameObject.SetActive(true);
        dig.canDig = false;
    }

    //Call on Event update
    public void OnDrag(PointerEventData eventData)
    {
        preview.position = grid.TileToPos(grid.PosToTile(input.GetHitPos()));
    }

    //Call Once
    public void OnEndDrag(PointerEventData eventData)
    {
        GameTile dropTile = grid.GetTile(grid.PosToTile(input.GetHitPos()));
        if (!dropTile.isElement)
        {
            //Init the linked object on grid
            switch (Item)
        {
            case Items.Cloud:
                if (Inventory.cloudsAmmount >0)
                {

                    elementManage.SpawnCloudAt(grid.PosToTile(input.GetHitPos()));
                    Inventory.cloudsAmmount--;
                }
                break;
            case Items.Source:
                if (Inventory.sourcesAmmount > 0)
                {
                    elementManage.SpawnWaterSourceAt(grid.PosToTile(input.GetHitPos()));
                    Inventory.sourcesAmmount--;
                }
                break;
            case Items.Lake:
                if (Inventory.lakesAmmount > 0)
                {
                    elementManage.SpawnLakeAt(grid.PosToTile(input.GetHitPos()));
                    Inventory.lakesAmmount--;
                }
                break;
            default:
                break;
        }
        }
        preview.position = Vector3.zero;
        preview.gameObject.SetActive(false);
        dig.canDig = true;
    }

    private void Start()
    {
        preview.gameObject.SetActive(false);
    }
    void Update()
    {
        switch (Item)
        {
            case Items.Cloud:
                if(Inventory.cloudsAmmount <= 0)
                {
                    Text.SetActive(false);
                    SelfImage.color = new Color32(255, 255, 255, 0);
                    textGUI.text = Inventory.cloudsAmmount.ToString();
                
                //SelfButton.interactable = false;
                }
                else
                {
                    Text.SetActive(true);
                    SelfImage.color = new Color32(255, 255, 255, 255);
                    textGUI.text = Inventory.cloudsAmmount.ToString();
                    //SelfButton.interactable = true;
                }
                break;

            case Items.Source:
                if (Inventory.sourcesAmmount <= 0)
                {
                    Text.SetActive(false);
                    SelfImage.color = new Color32(255, 255, 255, 0);
                    textGUI.text = Inventory.sourcesAmmount.ToString();
                    //    SelfButton.interactable = false;
                }
                else
                {
                    Text.SetActive(true);
                    SelfImage.color = new Color32(255, 255, 255, 255);
                    textGUI.text = Inventory.sourcesAmmount.ToString();
                    //    SelfButton.interactable = true;
                }
                break;

            case Items.Lake:
                if (Inventory.lakesAmmount <= 0)
                {
                    Text.SetActive(false);
                    SelfImage.color = new Color32(255, 255, 255, 0);
                    textGUI.text = Inventory.lakesAmmount.ToString();

                    //    SelfButton.interactable = false;
                }
                else
                {
                    Text.SetActive(true);
                    SelfImage.color = new Color32(255, 255, 255, 255);
                    textGUI.text = Inventory.lakesAmmount.ToString();
                    //   SelfButton.interactable = true;
                }
                break;

            default:
                break;
        }
    }
}
