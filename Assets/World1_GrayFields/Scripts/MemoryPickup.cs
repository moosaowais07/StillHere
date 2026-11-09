
using UnityEngine;

public class MemoryPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        WeightSystem weightSystem = other.GetComponent<WeightSystem>();

        if (weightSystem != null)
            weightSystem.CollectMemory();

        Destroy(gameObject);
    }
}
