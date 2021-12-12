using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Digging_UI : MonoBehaviour
{
    public TextMeshProUGUI textGUI;
    public InventoryManager Inventory;
    //public DigingHandler dig;

    void Update()
    {
        textGUI.text = Inventory.digAmmount.ToString();
    }
}
