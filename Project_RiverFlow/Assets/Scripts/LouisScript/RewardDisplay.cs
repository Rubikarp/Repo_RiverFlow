using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RewardDisplay : MonoBehaviour
{
    

    public TextMeshProUGUI digNumber;
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
    }
    public void Reward()
    {
        Inventory.digAmmount = Inventory.digAmmount + digBonus;
        Inventory.cloudsAmmount = Inventory.cloudsAmmount + cloudBonus;
        Inventory.sourcesAmmount = Inventory.sourcesAmmount + sourceBonus;
        Inventory.lakesAmmount = Inventory.lakesAmmount + lakeBonus;
        Inventory.tunnelsAmmount = Inventory.tunnelsAmmount + tunnelBonus;
        GameTime.Instance.Pause();
        this.gameObject.GetComponentInParent<RewardManager>().EmptySelectionPanel();
    }
}