using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToNextLevel : MonoBehaviour
{
    public string nextSceneName = "Level2";
    public int requiredMaterials = 16;
    public GameObject notEnoughMessage;
    public float messageDuration = 2f;

    bool isLoading = false;
    bool messageVisible = false;
    float hideMessageTime = 0f;

    void Start()
    {
        if (notEnoughMessage != null)
            notEnoughMessage.SetActive(false);
    }

    void Update()
    {
        if (messageVisible && Time.time >= hideMessageTime)
        {
            notEnoughMessage.SetActive(false);
            messageVisible = false;
        }
    }

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
            {
                notEnoughMessage.SetActive(true);
                messageVisible = true;
                hideMessageTime = Time.time + messageDuration;
            }
        }
    }
}