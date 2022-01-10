using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Pelle")]
    public int digAmmount;

    [Header("Item")]
    public int cloudsAmmount;
    public int sourcesAmmount;
    public int lakesAmmount;
    public int tunnelsAmmount;

    private void AddDigAmmo(int bonusDig)
    {
        digAmmount += bonusDig;
    }
}
