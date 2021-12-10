using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameTime timer;
    public int cloudsAmmount;
    public int sourcesAmmount;
    public int lakesAmmount;
    public int tunnelsAmmount;
    public int digAmmount;

    void Start()
    {
        digAmmount = 30;
        //timer.getMoreDig.AddListener(AddDigAmmo());
    }

    private void AddDigAmmo(int bonusDig)
    {
        digAmmount += bonusDig;
    }
}
