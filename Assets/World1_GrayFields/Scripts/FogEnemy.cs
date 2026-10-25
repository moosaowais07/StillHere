using UnityEngine;

public class FogEnemy : MonoBehaviour
{
    public float speed = 1.5f;
    public float detectRange = 8f;
    Transform target;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        if (target == null)
            return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance < detectRange)
        {
            float newX = Mathf.MoveTowards(transform.position.x, target.position.x, speed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
 
}
