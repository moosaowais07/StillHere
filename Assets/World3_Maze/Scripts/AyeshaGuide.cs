using UnityEngine;

// Ayesha's ghost. She waits next to the right door in each room.
// She doesn't glow the whole time: her light comes on for a few seconds, then fades,
// then comes back. While she's faded the room is dark, so the player has to remember
// which door she was at and how to get there. Each room sets its own timing (see MazeRoom).
//
// Give her a child object with a Light 2D on it, and drag that child into "Light Object".
public class AyeshaGuide : MonoBehaviour
{
    [Tooltip("The child object that holds her Light 2D.")]
    public GameObject lightObject;

    [Tooltip("How see-through she is while faded (0 = invisible, 1 = fully visible).")]
    [Range(0f, 1f)] public float fadedAlpha = 0.08f;

    [Tooltip("How quickly she fades in and out.")]
    public float fadeSpeed = 4f;

    [Header("Gentle floating")]
    public float floatHeight = 0.15f;
    public float floatSpeed = 2f;

    public bool IsGrabbed { get; private set; }
    public bool IsGlowing { get; private set; }

    Vector3 basePosition;
    Vector3 normalScale;
    SpriteRenderer sprite;
    Color baseColor;
    float alpha = 1f;
    float appearTimer = 1f;

    float visibleTime = 3f;
    float hiddenTime = 0f;
    float cycleTimer;

    void Awake()
    {
        basePosition = transform.position;
        normalScale = transform.localScale;
        sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) baseColor = sprite.color;
    }

    // Called by the MazeManager when Saad enters a new room.
    public void MoveTo(Vector3 position, float visibleSeconds, float hiddenSeconds)
    {
        IsGrabbed = false;
        basePosition = position;
        transform.position = position;
        visibleTime = Mathf.Max(0.5f, visibleSeconds);
        hiddenTime = Mathf.Max(0f, hiddenSeconds);
        cycleTimer = 0f;   // she always starts glowing when a room begins
        appearTimer = 0f;  // and grows in from small to normal size
    }

    // Called by a dark hand when it catches her: her light goes out until she's freed.
    public void Grab()
    {
        IsGrabbed = true;
    }

    // Called when Saad hits the hand away.
    public void Release()
    {
        IsGrabbed = false;
    }

    void Update()
    {
        // Floating up and down.
        float bob = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = basePosition + new Vector3(0f, bob, 0f);

        // Growing in when she appears in a new room.
        if (appearTimer < 1f)
        {
            appearTimer += Time.deltaTime * 3f;
            transform.localScale = normalScale * Mathf.Clamp01(appearTimer);
        }

        // Glow on, fade out, glow on again...
        bool glowing;
        if (hiddenTime <= 0f)
        {
            glowing = true;
        }
        else
        {
            cycleTimer += Time.deltaTime;
            float cycleLength = visibleTime + hiddenTime;
            if (cycleTimer >= cycleLength) cycleTimer -= cycleLength;
            glowing = cycleTimer < visibleTime;
        }
        if (IsGrabbed) glowing = false;
        IsGlowing = glowing;

        if (lightObject != null && lightObject.activeSelf != glowing) lightObject.SetActive(glowing);

        // Smoothly fade her sprite in and out.
        float targetAlpha = glowing ? 1f : fadedAlpha;
        alpha = Mathf.MoveTowards(alpha, targetAlpha, fadeSpeed * Time.deltaTime);
        if (sprite != null)
        {
            Color c = baseColor;
            c.a = baseColor.a * alpha;
            sprite.color = c;
        }
    }
}
