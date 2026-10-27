using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxOrbs = 3;
    public int currentOrbs;
    public float invincibleTime = 1f;
    public float failScreenTime = 2f;
    public GameObject failScreen;
    public GameObject releasedOrbPrefab;

    float invincibleUntil;
    bool dead;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        currentOrbs = maxOrbs;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeHit()
    {
        if (dead || Time.time < invincibleUntil) return;

        currentOrbs--;
        invincibleUntil = Time.time + invincibleTime;

        if (releasedOrbPrefab != null)
        {
            Instantiate(releasedOrbPrefab, transform.position, Quaternion.identity);
        }

        if (currentOrbs <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        dead = true;

        PlayerMovement move = GetComponent<PlayerMovement>();
        Breathe breathe = GetComponent<Breathe>();
        if (move != null) move.enabled = false;
        if (breathe != null) breathe.enabled = false;
        rb.linearVelocity = Vector2.zero;

        if (failScreen != null) failScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(failScreenTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}