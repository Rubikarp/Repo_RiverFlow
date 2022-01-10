using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Digging_UI : MonoBehaviour
{
    public TextMeshProUGUI textGUI;
    public InventoryManager Inventory;
    public Sprite on;
    public Sprite off;
    public Image selfImage;
    [SerializeField]
    private Items Item;
    //public DigingHandler dig;

    private void Awake()
    {
        Inventory = InventoryManager.Instance;
    }
    void Update()
    {
        
            switch (Item)
            {
                case Items.Dig:
                if (Inventory.digAmmount <= 0)
                {
                    textGUI.text = string.Empty;
                    selfImage.sprite = off;
                }
                else
                {
                    textGUI.text = Inventory.digAmmount.ToString();
                    selfImage.sprite = on;
                }
                    break;
            case Items.Tunnel:
                if (Inventory.tunnelsAmmount <= 0)
                {
                    textGUI.text = string.Empty;
                    selfImage.sprite = off;
                }
                else
                {
                    textGUI.text = Inventory.tunnelsAmmount.ToString();
                    selfImage.sprite = on;
                }
                break;
            default:
                break;
            }

    }

}
