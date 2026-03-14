using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    public Mineral currentMineral;

    public float mineralForgetDelay = 0.12f;

    float lastTimeTouchingMineral;

    void Update()
    {
        if (currentMineral != null && Time.time - lastTimeTouchingMineral > mineralForgetDelay)
        {
            Debug.Log("Exited mineral: " + currentMineral.name);
            currentMineral = null;
        }
    }

    void OnTriggerEnter(Collider detectedCollider)
    {
        RememberMineralIfValid(detectedCollider);
    }

    void OnTriggerStay(Collider detectedCollider)
    {
        RememberMineralIfValid(detectedCollider);
    }

    void RememberMineralIfValid(Collider detectedCollider)
    {
        Transform mineralRootTransform = detectedCollider.transform.parent;

        if (mineralRootTransform == null)
            return;

        Mineral mineral = mineralRootTransform.GetComponent<Mineral>();

        if (mineral == null)
            return;

        if (detectedCollider != mineral.mineralHitZone)
            return;

        if (currentMineral != mineral)
            Debug.Log("Entered mineral: " + mineral.name);

        currentMineral = mineral;
        lastTimeTouchingMineral = Time.time;
    }
}