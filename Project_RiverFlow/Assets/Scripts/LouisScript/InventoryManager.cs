using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int cloudsAmmount;
    public int sourcesAmmount;
    public int lakesAmmount;
    public int tunnelsAmmount;
    public int digAmmount;

    void Start()
    {
        digAmmount = 30;

    }

    private void AddDigAmmo(int bonusDig)
    {
        digAmmount += bonusDig;
    }
}
