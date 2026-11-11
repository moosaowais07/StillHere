using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoints : MonoBehaviour
{
    public Light2D flameLight;
    public SpriteRenderer flame;
    bool lit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        lit = PlayerHealth.hasCheckpoint &&
              transform.position.x <= PlayerHealth.checkpointPosition.x + 0.01f;
        SetFlame(lit);
    }

    void OnTriggerEnters2D(Collider2D other)
    {
        if (lit) return;
        if (other.GetComponent<PlayerHealth>() == null) return;

        lit = true;
        PlayerHealth.hasCheckpoint = true;
        PlayerHealth.checkpointPosition = transform.position + Vector3.up * 0.5f;
        SetFlame(true);
    }

    void SetFlame(bool on)
    {
        if (flameLight != null) flameLight.enabled = on;
        if (flame != null) flame.enabled = on;
    }
}