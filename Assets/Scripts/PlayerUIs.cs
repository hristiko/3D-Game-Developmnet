using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIs : MonoBehaviour
{
    public Health playerHealth;
    public PlayerInventory playerInventory;

    public Slider healthSlider;
    public TMP_Text hpText;
    public TMP_Text materialsText;

    void Start()
    {
        healthSlider.minValue = 0;
        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.currentHealth;
        
        playerInventory = playerHealth.GetComponentInParent<PlayerInventory>();
    }

    void Update()
    {
        int hp = playerHealth.currentHealth;
        healthSlider.value = hp;
        hpText.text = "HP: " + hp;
        
        materialsText.text = "Materials: " + playerInventory.materialsAmount;
    }
}