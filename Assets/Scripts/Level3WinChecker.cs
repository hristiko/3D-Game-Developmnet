using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3WinChecker : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public int requiredMaterials = 33;
    public int requiredGoblinKills = 21;
    public string gameWonSceneName = "GameWon";

    int killedGoblins = 0;
    bool isLoading = false;

    void Update()
    {
        if (isLoading) return;
        if (playerInventory == null) return;

        if (playerInventory.materialsAmount >= requiredMaterials && killedGoblins >= requiredGoblinKills)
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