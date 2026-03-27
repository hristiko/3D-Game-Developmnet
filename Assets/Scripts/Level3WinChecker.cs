using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3WinChecker : MonoBehaviour
{
    public GoblinSpawner goblinSpawner;

    bool gameWonLoaded = false;

    void Update()
    {
        if (gameWonLoaded) return;

        bool allSpawnsFinished = true;
        if (goblinSpawner != null)
            allSpawnsFinished = goblinSpawner.FinishedSpawning;

        bool noGoblinsLeft = GameObject.FindGameObjectsWithTag("Goblin").Length == 0;
        bool noMineralsLeft = GameObject.FindGameObjectsWithTag("Mineral").Length == 0;

        if (allSpawnsFinished && noGoblinsLeft && noMineralsLeft)
        {
            gameWonLoaded = true;
            SceneManager.LoadScene("GameWon");
        }
    }
}