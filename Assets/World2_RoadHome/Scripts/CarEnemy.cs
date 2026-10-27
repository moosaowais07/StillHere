using UnityEngine;

public class CarEnemy : MonoBehaviour
{
    public float speed = 6f;
    public float lifetime = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        RoadHomePlayerHealth health = other.GetComponent<RoadHomePlayerHealth>();
        if (health != null) health.TakeHit();
    }
}