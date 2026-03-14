using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int materials = 0;

    public void AddMaterials(int amount)
    {
        materials += amount;
        if (materials < 0) materials = 0;

        Debug.Log("Materials: " + materials);
    }
}