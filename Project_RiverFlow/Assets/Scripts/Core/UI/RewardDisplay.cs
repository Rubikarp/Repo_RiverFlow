using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class RewardDisplay : MonoBehaviour
{
    

    public TextMeshProUGUI digNumber;
    public InventoryManager Inventory;
    public int digBonus;
    public int cloudBonus;
    public int sourceBonus;
    public int lakeBonus;
    public int tunnelBonus;
    public string soundButtonReward = "SoundButtonReward";

    private void Start()
    {

        transform.DOScaleY(0.8f, 0.2f).SetEase(Ease.OutBack);
        transform.DOScaleX(0.8f, 0.2f).SetEase(Ease.OutBack);
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
        TimeManager.Instance.Pause();
        this.gameObject.GetComponentInParent<RewardManager>().EmptySelectionPanel();
        LevelSoundboard.Instance.PlayRewardUISound(soundButtonReward);
    }

}
