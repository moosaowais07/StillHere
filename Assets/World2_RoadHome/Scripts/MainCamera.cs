using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public PanicMeter panic;
    public float smoothSpeed = 10f;
    public Vector3 offset = new Vector3(0f, 1f, -10f);
    public float maxShake = 0.15f;
    public float shakeStartsAt = 0.2f;

    Vector3 basePosition;

    void Start()
    {
        basePosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 desired = target.position + offset;
        basePosition = Vector3.Lerp(basePosition, desired, smoothSpeed * Time.deltaTime);

        Vector3 shake = Vector3.zero;
        if (panic != null && panic.panic > shakeStartsAt)
        {
            float t = (panic.panic - shakeStartsAt) / (1f - shakeStartsAt);
            shake = (Vector3)Random.insideUnitCircle * maxShake * t;
        }

        transform.position = basePosition + shake;
    }
}