using System.Collections;
using UnityEngine;

public class BreatheUnlock : MonoBehaviour
{
    public GameObject prompt;
    public float promptTime = 4f;
    bool used;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;

        Breathe breathe = other.GetComponent<Breathe>();
        if (breathe == null) return;

        used = true;
        breathe.unlocked = true;

        Breathe.everUnlocked = true;

        if (prompt != null) StartCoroutine(ShowPrompt());
    }

    IEnumerator ShowPrompt()
    {
        prompt.SetActive(true);
        yield return new WaitForSecondsRealtime(promptTime);
        prompt.SetActive(false);
    }
}