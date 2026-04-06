using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int materialsAmount = 0;

    public void AddMaterials(int amount)
    {
        materialsAmount += amount;
        if (materialsAmount < 0) materialsAmount = 0;

        Debug.Log("Materials: " + materialsAmount);
    }
}