using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RewardDisplay : MonoBehaviour
{
    

    public Text digNumber;
    public Text itemNumber;
    public InventoryManager Inventory;
    public int digBonus;
    public int cloudBonus;
    public int sourceBonus;
    public int lakeBonus;
    public int tunnelBonus;


    private void Start()
    {
        Inventory = this.gameObject.GetComponentInParent<RewardManager>().Inventory;
        digNumber.text = digBonus.ToString();
        itemNumber.text = cloudBonus + sourceBonus + lakeBonus+tunnelBonus.ToString();
    }
    public void Reward()
    {
        Inventory.digAmmount = Inventory.digAmmount + digBonus;
        Inventory.cloudsAmmount = Inventory.cloudsAmmount + cloudBonus;
        Inventory.sourcesAmmount = Inventory.sourcesAmmount + sourceBonus;
        Inventory.lakesAmmount = Inventory.lakesAmmount + lakeBonus;
        Inventory.tunnelsAmmount = Inventory.tunnelsAmmount + tunnelBonus;
    }
}
