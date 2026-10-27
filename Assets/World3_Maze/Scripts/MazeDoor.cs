using UnityEngine;


// Saad stands in front of the door and presses W to go through.
//

[RequireComponent(typeof(Collider2D))]
public class MazeDoor : MonoBehaviour
{
    public enum DoorType
    {
        Wrong,  // if Leads To is empty: sends Saad back to the start of the same room
        Right,  // if Leads To is empty: goes to the next room in the MazeManager list
        Exit    // the Locked Door at the end: finishes World 3
    }

    public DoorType doorType = DoorType.Wrong;

    [Tooltip("The room this door takes Saad to. If you fill this in, it is used instead of the Door Type.")]
    public MazeRoom leadsTo;

    bool playerInside;

    // Runs when you first add the script: ticks Is Trigger for you.
    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other)) playerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayer(other)) playerInside = false;
    }

    void Update()
    {
        if (playerInside && MazeInput.UpPressed())
        {
            playerInside = false;
            if (MazeManager.Instance != null) MazeManager.Instance.UseDoor(this);
            else Debug.LogError("There is no MazeManager in the scene.");
        }
    }

    bool IsPlayer(Collider2D other)
    {
        if (other.CompareTag("Player")) return true;
        return other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player");
    }

    //shows a small hint while Saad is standing at a door.
    void OnGUI()
    {
        if (!playerInside) return;
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 22;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(0, Screen.height - 70, Screen.width, 40), "Press W to go through the door", style);
    }

    //in the Scene view, draws a line from this door to the room it leads to.
    void OnDrawGizmos()
    {
        if (leadsTo == null) return;
        Gizmos.color = doorType == DoorType.Right ? Color.yellow : Color.red;
        Gizmos.DrawLine(transform.position, leadsTo.transform.position);
    }
}
