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
    private Sprite on;
    [SerializeField]
    private Sprite off;
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
        if (!dropTile.haveElement)
        {
            //Init the linked object on grid
            switch (Item)
            {
                case Items.Cloud:
                    if (Inventory.cloudsAmmount > 0)
                    {
                        GameTile testedTile = grid.GetTile(grid.PosToTile(input.GetHitPos()));
                        if (testedTile.ReceivedFlow() > FlowStrenght._00_)
                        {
                            elementManage.SpawnCloudAt(grid.PosToTile(input.GetHitPos()));
                            Inventory.cloudsAmmount--;
                        }
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
                        GameTile testedTile = grid.GetTile(grid.PosToTile(input.GetHitPos()));
                        if (testedTile.ReceivedFlow() > FlowStrenght._00_)
                        {
                            if (testedTile.linkAmount == 2)
                            {
                                List<GameTile> testedTileLinks = testedTile.GetLinkedTile();
                                //check if vertical
                                if ((testedTileLinks[0] == testedTile.neighbors[1] && testedTileLinks[1] == testedTile.neighbors[5])
                                 || (testedTileLinks[0] == testedTile.neighbors[5] && testedTileLinks[1] == testedTile.neighbors[1]))
                                {
                                    elementManage.SpawnLakeAt(testedTile.gridPos, vertical: true);
                                    Inventory.lakesAmmount--;
                                }
                                //check if horizontal
                                else
                                if ((testedTileLinks[0] == testedTile.neighbors[3] && testedTileLinks[1] == testedTile.neighbors[7])
                                 || (testedTileLinks[0] == testedTile.neighbors[7] && testedTileLinks[1] == testedTile.neighbors[3]))
                                {
                                    elementManage.SpawnLakeAt(testedTile.gridPos, vertical: false);
                                    Inventory.lakesAmmount--;
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            RiverManager.Instance.FlowStep();
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
                    SelfImage.sprite = off;
                    textGUI.text = Inventory.cloudsAmmount.ToString();
                
                //SelfButton.interactable = false;
                }
                else
                {
                    Text.SetActive(true);
                    SelfImage.sprite = on;
                    textGUI.text = Inventory.cloudsAmmount.ToString();
                    //SelfButton.interactable = true;
                }
                break;

            case Items.Source:
                if (Inventory.sourcesAmmount <= 0)
                {
                    Text.SetActive(false);
                    SelfImage.sprite = off;
                    textGUI.text = Inventory.sourcesAmmount.ToString();
                    //    SelfButton.interactable = false;
                }
                else
                {
                    Text.SetActive(true);
                    SelfImage.sprite = on;
                    textGUI.text = Inventory.sourcesAmmount.ToString();
                    //    SelfButton.interactable = true;
                }
                break;

            case Items.Lake:
                if (Inventory.lakesAmmount <= 0)
                {
                    Text.SetActive(false);
                    SelfImage.sprite = off;
                    textGUI.text = Inventory.lakesAmmount.ToString();

                    //    SelfButton.interactable = false;
                }
                else
                {
                    Text.SetActive(true);
                    SelfImage.sprite = on;
                    textGUI.text = Inventory.lakesAmmount.ToString();
                    //   SelfButton.interactable = true;
                }
                break;

            default:
                break;
        }
    }
}
