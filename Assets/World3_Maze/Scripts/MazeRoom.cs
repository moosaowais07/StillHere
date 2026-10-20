using UnityEngine;

// Put this on an empty object placed in the CENTRE of each room.
// The camera moves to this object's position when Saad enters the room.
public class MazeRoom : MonoBehaviour
{
    [Tooltip("Where Saad appears when he enters this room (or goes through a wrong door).")]
    public Transform startPoint;

    [Tooltip("Where Ayesha stands in this room: next to the right door.")]
    public Transform ayeshaSpot;

    // Draws helper shapes in the Scene view so you can see the room's points.
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.3f, 0.6f, 1f, 0.6f);
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
