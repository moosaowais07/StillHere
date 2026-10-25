using UnityEngine;

// Ayesha's ghost. In each room she appears next to the door that leads the right way.
// The room is completely dark. She glows once when Saad walks in, then only BLINKS for
// a split second every few seconds. The player has to remember what they saw.
// Each room sets its own timing (see MazeRoom). In "lost" rooms she doesn't appear at all.
//
// Give her a child object with a Light 2D on it (big radius, so a blink lights the whole
// room), and drag that child into "Light Object".
public class AyeshaGuide : MonoBehaviour
{
    [Tooltip("The child object that holds her Light 2D.")]
    public GameObject lightObject;

    [Tooltip("How see-through she is between blinks (0 = invisible).")]
    [Range(0f, 1f)] public float fadedAlpha = 0f;

    [Header("Gentle floating")]
    public float floatHeight = 0.15f;
    public float floatSpeed = 2f;

    public bool IsGrabbed { get; private set; }
    public bool IsGlowing { get; private set; }
    public bool IsPresent { get; private set; } = true;

    Vector3 basePosition;
    SpriteRenderer sprite;
    Color baseColor;

    float firstGlow = 1.2f;
    float blink = 0.3f;
    float dark = 4f;
    float timer;
    bool firstGlowDone;

    void Awake()
    {
        basePosition = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) baseColor = sprite.color;
    }

    // Called by the MazeManager when Saad enters a room where she appears.
    public void AppearAt(Vector3 position, float firstGlowSeconds, float blinkSeconds, float darkSeconds)
    {
        IsPresent = true;
        IsGrabbed = false;
        basePosition = position;
        transform.position = position;
        firstGlow = Mathf.Max(0.1f, firstGlowSeconds);
        blink = Mathf.Max(0.05f, blinkSeconds);
        dark = Mathf.Max(0f, darkSeconds);
        timer = 0f;
        firstGlowDone = false;
    }

    // Called by the MazeManager when Saad enters a lost room.
    public void Disappear()
    {
        IsPresent = false;
        IsGrabbed = false;
    }

    // Called by a dark hand when it catches her: no more blinks until she's freed.
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
        float bob = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = basePosition + new Vector3(0f, bob, 0f);

        bool glowing = false;
        if (IsPresent && !IsGrabbed)
        {
            timer += Time.deltaTime;
            if (!firstGlowDone)
            {
                glowing = timer < firstGlow;
                if (timer >= firstGlow + dark) { firstGlowDone = true; timer = 0f; }
            }
            else if (dark <= 0f)
            {
                glowing = true;
            }
            else
            {
                float cycle = blink + dark;
                if (timer >= cycle) timer -= cycle;
                glowing = timer < blink;
            }
        }
        IsGlowing = glowing;

        if (lightObject != null && lightObject.activeSelf != glowing) lightObject.SetActive(glowing);

        if (sprite != null)
        {
            Color c = baseColor;
            c.a = baseColor.a * (glowing ? 1f : (IsPresent ? fadedAlpha : 0f));
            sprite.color = c;
        }
    }
}
