using System.Collections;
using UnityEngine;

public class TruckBoss : MonoBehaviour
{
    public GameObject truckPrefab;
    public Transform player;
    public CarSpawner carSpawner;
    public int passes = 5;
    public float startSpeed = 10f;
    public float speedIncrease = 1.5f;
    public float spawnDistanceAhead = 30f;
    public float spawnY = -0.75f;
    public float delayBetweenPasses = 3f;

    public bool finished;
    bool started;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (started) return;
        if (other.GetComponent<PlayerMovement>() == null) return;

        started = true;
        StartCoroutine(RunBoss());
    }

    IEnumerator RunBoss()
    {
        if (carSpawner != null) carSpawner.enabled = false;

        float speed = startSpeed;
        for (int i = 0; i < passes; i++)
        {
            yield return new WaitForSeconds(delayBetweenPasses);

            Vector3 pos = new Vector3(player.position.x + spawnDistanceAhead, spawnY, 0f);
            GameObject truck = Instantiate(truckPrefab, pos, Quaternion.identity);

            CarEnemy car = truck.GetComponent<CarEnemy>();
            car.speed = speed;
            car.lifetime = 60f;

            while (truck != null && truck.transform.position.x > player.position.x - 10f)
            {
                yield return null;
            }

            if (truck != null) Destroy(truck);
            speed += speedIncrease;
        }

        finished = true;
        if (carSpawner != null) carSpawner.enabled = true;
    }
}