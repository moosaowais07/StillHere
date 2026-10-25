using UnityEngine;

// A dark hand that comes out of the wall and tries to grab Ayesha.
// If it reaches her, her light goes out. Saad hits it (J) to make it let go and pull back.
// It only reaches for her when she is in the same room (within "Reach Range").
public class DarkHand : MonoBehaviour, IHittable
{
    public AyeshaGuide ayesha;

    [Header("Timing")]
    public float waitBeforeReaching = 3f;
    public float reachSpeed = 1.5f;
    public float pullBackSpeed = 8f;

    [Header("Distances")]
    [Tooltip("How close it has to get to grab her.")]
    public float grabDistance = 0.5f;
    [Tooltip("It ignores Ayesha if she is further away than this (she's in another room).")]
    public float reachRange = 9f;

    enum State { Waiting, Reaching, Holding, PullingBack }
    State state = State.Waiting;
    Vector3 home;
    float timer;

    void Start()
    {
        home = transform.position;
        if (ayesha == null)
        {
#if UNITY_2023_1_OR_NEWER
            ayesha = FindFirstObjectByType<AyeshaGuide>();
#else
            ayesha = FindObjectOfType<AyeshaGuide>();
#endif
        }
    }

    void Update()
    {
        if (ayesha == null) return;

        // Ayesha isn't in this part of the house (Saad is in a lost room): pull back and wait.
        if (!ayesha.IsPresent && state != State.PullingBack && state != State.Waiting)
        {
            if (state == State.Holding) ayesha.Release();
            state = State.PullingBack;
        }

        Vector3 target = ayesha.transform.position;
        float distanceFromHome = Vector3.Distance(home, target);

        switch (state)
        {
            case State.Waiting:
                if (distanceFromHome > reachRange || !ayesha.IsPresent) { timer = 0f; break; }
                timer += Time.deltaTime;
                if (timer >= waitBeforeReaching) state = State.Reaching;
                break;

            case State.Reaching:
                if (distanceFromHome > reachRange) { state = State.PullingBack; break; }
                transform.position = Vector3.MoveTowards(transform.position, target, reachSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, target) <= grabDistance)
                {
                    ayesha.Grab();
                    state = State.Holding;
                }
                break;

            case State.Holding:
                // If Ayesha has moved to another room, let go.
                if (Vector3.Distance(transform.position, target) > grabDistance * 3f)
                {
                    ayesha.Release();
                    state = State.PullingBack;
                }
                break;

            case State.PullingBack:
                transform.position = Vector3.MoveTowards(transform.position, home, pullBackSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, home) < 0.01f)
                {
                    timer = 0f;
                    state = State.Waiting;
                }
                break;
        }
    }

    // Called by the player's attack.
    public void Hit()
    {
        if (state == State.Holding) ayesha.Release();
        if (state == State.Reaching || state == State.Holding) state = State.PullingBack;
    }
}
