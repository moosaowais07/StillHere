using UnityEngine;
using UnityEngine.InputSystem;

public class Breathe : MonoBehaviour
{
    public bool unlocked = true;
    public float slowScale = 0.5f;
    public float calmPerSecond = 0.25f;

    PanicMeter panicMeter;
    float normalFixedDelta;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panicMeter = GetComponent<PanicMeter>();
        normalFixedDelta = Time.fixedDeltaTime;
    }

    // Update is called once per frame

    void Update()
    {
        bool holding = unlocked && Keyboard.current.lKey.isPressed;

        if (holding)
        {
            Time.timeScale = slowScale;
            if (panicMeter != null) panicMeter.Calm(calmPerSecond * Time.unscaledDeltaTime);
        }
        else
        {
            Time.timeScale = 1f;
        }

        Time.fixedDeltaTime = normalFixedDelta * Time.timeScale;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = normalFixedDelta;
    }
}