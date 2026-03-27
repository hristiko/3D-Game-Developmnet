using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3WinChecker : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public int requiredMaterials = 33;
    public int requiredGoblinKills = 21;
    public string gameWonSceneName = "GameWon";

    int startMaterials = 0;
    int killedGoblins = 0;
    bool isLoading = false;

    void Start()
    {
        if (playerInventory == null)
            playerInventory = FindObjectOfType<PlayerInventory>();

        if (playerInventory != null)
            startMaterials = playerInventory.materials;
    }

    void Update()
    {
        if (isLoading) return;
        if (playerInventory == null) return;

        int collectedThisLevel = playerInventory.materials - startMaterials;

        if (collectedThisLevel < 0)
            collectedThisLevel = 0;

        if (collectedThisLevel >= requiredMaterials && killedGoblins >= requiredGoblinKills)
        {
            isLoading = true;
            SceneManager.LoadSceneAsync(gameWonSceneName);
        }
    }

    public void RegisterGoblinKill()
    {
        killedGoblins++;
    }
}