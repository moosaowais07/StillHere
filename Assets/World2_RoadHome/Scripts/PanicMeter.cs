using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PanicMeter : MonoBehaviour
{
    [Range(0f, 1f)] public float panic = 0f;
    public float risePerSecond = 0.02f;
    public float maxVignette = 0.6f;
    public Volume volume;

    Vignette vignette;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if (volume != null && volume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
        }
    }

    // Update is called once per frame

    void Update()
    {
        panic = Mathf.Clamp01(panic + risePerSecond * Time.deltaTime);

        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(0f, maxVignette, panic);
        }
    }

    public void Calm(float amount)
    {
        panic = Mathf.Clamp01(panic - amount);
    }

    public void ResetPanic()
    {
        panic = 0f;
    }
}