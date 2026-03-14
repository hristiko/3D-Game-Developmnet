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
        if (playerHealth != null && healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = playerHealth.maxHealth;
            healthSlider.value = playerHealth.currentHealth;
        }

        playerInventory = playerHealth.GetComponentInParent<PlayerInventory>();
    }

    void Update()
    {
        if (playerHealth != null && healthSlider != null && hpText != null)
        {
            int hp = playerHealth.currentHealth;
            healthSlider.value = hp;
            hpText.text = "HP: " + hp;
        }

        if (playerInventory != null && materialsText != null)
        {
            materialsText.text = "Materials: " + playerInventory.materials;
        }
    }
}