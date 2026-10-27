using UnityEngine;

// Makes World 3 pitch black, whatever the lighting settings are.
// It puts a huge black sheet over the level, with a small soft hole around Saad so
// he can just about see himself. When Ayesha blinks, the black sheet disappears for a
// moment and the whole room is visible. Then it goes black again.

public class DarknessOverlay : MonoBehaviour
{
    [Tooltip("Saad. Leave empty to find the object tagged Player.")]
    public Transform player;

    [Tooltip("Ayesha. Leave empty to find her automatically.")]
    public AyeshaGuide ayesha;

    [Header("Saad's small circle of sight")]
    [Tooltip("How far Saad can see around himself in the dark (in Unity units).")]
    public float visionRadius = 1.2f;
    [Tooltip("How soft the edge of his circle of sight is.")]
    public float visionSoftness = 0.8f;

    [Header("Blinks")]
    [Tooltip("How fast the darkness disappears when Ayesha blinks, and comes back after.")]
    public float fadeSpeed = 12f;

    [Header("Drawing order")]
    [Tooltip("Must be drawn on top of everything. Only change this if you use other Sorting Layers.")]
    public string sortingLayerName = "Default";
    public int sortingOrder = 1000;

    const float sheetSize = 60f;   //the black sheet is 60 x 60 units, much bigger than the screen
    const int textureSize = 1024;

    SpriteRenderer sheet;
    float darkness = 1f;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        if (ayesha == null)
        {
#if UNITY_2023_1_OR_NEWER
            ayesha = FindFirstObjectByType<AyeshaGuide>();
#else
            ayesha = FindObjectOfType<AyeshaGuide>();
#endif
        }
        CreateSheet();
    }

    void CreateSheet()
    {
        Texture2D tex = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] pixels = new Color32[textureSize * textureSize];
        float unitsPerPixel = sheetSize / textureSize;
        float half = textureSize / 2f;
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float dx = (x - half) * unitsPerPixel;
                float dy = (y - half) * unitsPerPixel;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                float a = Mathf.InverseLerp(visionRadius, visionRadius + visionSoftness, distance);
                a = a * a * (3f - 2f * a); // smooth edge
                pixels[y * textureSize + x] = new Color32(0, 0, 0, (byte)(a * 255f));
            }
        }
        tex.SetPixels32(pixels);
        tex.Apply();

        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, textureSize, textureSize),
                                      new Vector2(0.5f, 0.5f), textureSize / sheetSize);

        GameObject go = new GameObject("Darkness (made by DarknessOverlay)");
        sheet = go.AddComponent<SpriteRenderer>();
        sheet.sprite = sprite;
        sheet.sortingLayerName = sortingLayerName;
        sheet.sortingOrder = sortingOrder;
    }

    void LateUpdate()
    {
        if (sheet == null) return;

        // Follow Saad so the hole is always around him.
        if (player != null)
            sheet.transform.position = new Vector3(player.position.x, player.position.y, 0f);

        // Black unless Ayesha is glowing right now.
        bool lit = ayesha != null && ayesha.IsGlowing;
        darkness = Mathf.MoveTowards(darkness, lit ? 0f : 1f, fadeSpeed * Time.deltaTime);
        sheet.color = new Color(1f, 1f, 1f, darkness);
    }
}
