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
    //public DigingHandler dig;

    void Update()
    {
        
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
    }

}
