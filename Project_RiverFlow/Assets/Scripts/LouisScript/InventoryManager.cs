using UnityEngine;

public class InventoryManager : MonoBehaviour
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
