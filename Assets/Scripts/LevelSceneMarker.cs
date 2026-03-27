using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneMarker : MonoBehaviour
{
    void Start()
    {
        GameState.currentLevelSceneName = SceneManager.GetActiveScene().name;
    }
}