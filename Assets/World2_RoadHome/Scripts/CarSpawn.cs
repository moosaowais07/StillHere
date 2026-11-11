using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;
    public Transform player;
    public float spawnDistanceAhead = 15f;
    public float spawnY = -1f;
    public float minDelay = 2f;
    public float maxDelay = 4f;
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        timer = Random.Range(minDelay, maxDelay);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Vector3 pos = new Vector3(player.position.x + spawnDistanceAhead, spawnY, 0f);
            Instantiate(carPrefab, pos, Quaternion.identity);
            timer = Random.Range(minDelay, maxDelay);
        }
    }
}