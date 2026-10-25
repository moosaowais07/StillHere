using UnityEngine;

// Put this on an empty object placed in the CENTRE of each room.
// The camera moves to this object's position when Saad enters the room.
public class MazeRoom : MonoBehaviour
{
    [Tooltip("Where Saad appears when he enters this room (and where he goes back to after a pit or spikes).")]
    public Transform startPoint;

    [Tooltip("Where Ayesha appears in this room: next to the door that leads the right way. " +
             "Leave EMPTY for a 'lost' room where Ayesha never appears.")]
    public Transform ayeshaSpot;

    [Header("Ayesha's blinks in this room")]
    [Tooltip("How long her first glow lasts when Saad walks in, so the player gets one good look.")]
    public float firstGlowTime = 1.2f;
    [Tooltip("How long each blink after that lasts.")]
    public float blinkTime = 0.3f;
    [Tooltip("Seconds of total darkness between blinks. Set to 0 to keep her lit the whole time.")]
    public float darkTime = 4f;

    // Draws helper shapes in the Scene view so you can see the room's points.
    void OnDrawGizmos()
    {
        Gizmos.color = ayeshaSpot != null ? new Color(0.3f, 0.6f, 1f, 0.6f) : new Color(1f, 0.3f, 0.3f, 0.6f);
        Gizmos.DrawWireCube(transform.position, new Vector3(16f, 9f, 0f));
        if (startPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPoint.position, 0.4f);
        }
        if (ayeshaSpot != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(ayeshaSpot.position, 0.4f);
        }
    }
}
