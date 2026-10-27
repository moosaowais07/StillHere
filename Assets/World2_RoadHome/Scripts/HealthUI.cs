using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public RoadPlayerHealth health;
    public Image[] orbs;
    public Color litColor = new Color(1f, 0.85f, 0.3f);
    public Color dimColor = new Color(0.2f, 0.2f, 0.25f);

    // Update is called once per frame

    void Update()
    {
        for (int i = 0; i < orbs.Length; i++)
        {
            orbs[i].color = i < health.currentOrbs ? litColor : dimColor;
        }
    }
}