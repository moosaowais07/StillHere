using UnityEngine;

public class MemoryPickup : MonoBehaviour
{
    public float lightenAmount = 0.3f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.Lighten(lightenAmount);
            Destroy(gameObject);
        }
    }

}
