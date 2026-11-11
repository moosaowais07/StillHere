using UnityEngine;

public class ReleasedOrb : MonoBehaviour
{
    public float riseSpeed = 1.5f;
    public float lifetime = 1.5f;

    SpriteRenderer sr;
    float age;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        age += Time.deltaTime;
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        if (sr != null)
        {
            Color c = sr.color;
            c.a = 1f - (age / lifetime);
            sr.color = c;
        }

        if (age >= lifetime) Destroy(gameObject);
    }
}