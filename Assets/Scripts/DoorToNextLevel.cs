using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToNextLevel : MonoBehaviour
{
    public string nextSceneName = "Level2";
    public int requiredMaterials = 16;
    public GameObject notEnoughMessage;
    public float messageDuration = 2f;

    bool isLoading = false;

    void OnTriggerEnter(Collider detectedCollider)
    {
        if (isLoading) return;

        if (!detectedCollider.CompareTag("Player"))
            return;

        PlayerInventory inventory = detectedCollider.GetComponentInParent<PlayerInventory>();

        if (inventory == null)
            return;

        if (inventory.materials >= requiredMaterials)
        {
            isLoading = true;
            SceneManager.LoadSceneAsync(nextSceneName);
        }
        else
        {
            if (notEnoughMessage != null)
                StartCoroutine(ShowMessageTemporarily());
        }
    }

    IEnumerator ShowMessageTemporarily()
    {
        notEnoughMessage.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        notEnoughMessage.SetActive(false);
    }
}