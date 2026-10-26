
using UnityEngine;

public class WeightSystem : MonoBehaviour
{
    public float timeToReachMaximumWeight = 120f;
    public float weightIncreaseRate = 1f;
    public float speedLossAtMaximumWeight = 3f;
    public float jumpLossAtMaximumWeight = 1f;
    private float weight = 0f;
    private PlayerController player;

    void Start(){
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (player == null)
            return;

        weight += weightIncreaseRate * Time.deltaTime;
        weight = Mathf.Clamp(weight, 0f, timeToReachMaximumWeight);
        float progress = weight / timeToReachMaximumWeight;
        player.moveSpeed = Mathf.Max(player.minimumMoveSpeed,5f - progress * speedLossAtMaximumWeight);
        player.jumpForce = Mathf.Lerp(10f,1.5f,progress);
    }

    public void CollectMemory(){
        weight = Mathf.Max(0f, weight - 30f);
    }
}
