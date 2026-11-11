using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Ending: MonoBehaviour
{
    public ParticleSystem rain;
    public CarSpawner carSpawner;
    public PanicMeter panicMeter;
    public Light2D globalLight;
    public TruckBoss truckBoss;
    public GameObject completeScreen;
    public float brightenTo = 1f;
    public float brightenTime = 4f;
    public float breatheSeconds = 5f;

    bool done;

    void OnTriggerStay2D(Collider2D other)
    {
        if (done) return;
        if (other.GetComponent<PlayerMovement>() == null) return;
        if (truckBoss != null && !truckBoss.finished) return;

        done = true;
        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence()
    {
        if (carSpawner != null) carSpawner.enabled = false;

        foreach (CarEnemy car in FindObjectsByType<CarEnemy>(FindObjectsSortMode.None))
        {
            Destroy(car.gameObject);
        }

        if (rain != null) rain.Stop();

        if (panicMeter != null)
        {
            panicMeter.risePerSecond = 0f;
            panicMeter.ResetPanic();
        }

        if (globalLight != null)
        {
            float start = globalLight.intensity;
            float t = 0f;
            while (t < brightenTime)
            {
                t += Time.deltaTime;
                globalLight.intensity = Mathf.Lerp(start, brightenTo, t / brightenTime);
                yield return null;
            }
        }

        float held = 0f;
        while (held < breatheSeconds)
        {
            if (Keyboard.current.lKey.isPressed)
                held += Time.unscaledDeltaTime;
            else
                held = Mathf.Max(0f, held - Time.unscaledDeltaTime);

            yield return null;
        }

        if (completeScreen != null) completeScreen.SetActive(true);
    }
}