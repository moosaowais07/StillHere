using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LightGlow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float minIntensity = 1f;
    public float maxIntensity = 2f;
    public float glowSpeed = 2f;
    Light2D glow;
    void Start()
    {
        glow = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float t = (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f;
        glow.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}
