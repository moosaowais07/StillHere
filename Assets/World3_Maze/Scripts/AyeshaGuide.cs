using UnityEngine;

// Ayesha's ghost. She waits next to the right door in each room and glows.
// Give her a child object with a Light 2D on it, and drag that child into "Light Object".
public class AyeshaGuide : MonoBehaviour
{
    [Tooltip("The child object that holds her Light 2D.")]
    public GameObject lightObject;

    [Header("Gentle floating")]
    public float floatHeight = 0.15f;
    public float floatSpeed = 2f;

    public bool IsGrabbed { get; private set; }

    Vector3 basePosition;
    Vector3 normalScale;
    float appearTimer = 1f;

    void Awake()
    {
        basePosition = transform.position;
        normalScale = transform.localScale;
    }

    // Called by the MazeManager when Saad enters a new room.
    public void MoveTo(Vector3 position)
    {
        Release();
        basePosition = position;
        transform.position = position;
        appearTimer = 0f; // she fades in by growing from small to normal size
    }

    // Called by a dark hand when it catches her: her light goes out.
    public void Grab()
    {
        IsGrabbed = true;
        if (lightObject != null) lightObject.SetActive(false);
    }

    // Called when Saad hits the hand away.
    public void Release()
    {
        IsGrabbed = false;
        if (lightObject != null) lightObject.SetActive(true);
    }

    void Update()
    {
        float bob = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = basePosition + new Vector3(0f, bob, 0f);

        if (appearTimer < 1f)
        {
            appearTimer += Time.deltaTime * 3f;
            transform.localScale = normalScale * Mathf.Clamp01(appearTimer);
        }
    }
}
