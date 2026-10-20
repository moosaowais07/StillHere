using UnityEngine;

// Put this on every door. The door needs a BoxCollider2D with "Is Trigger" ticked.
// Saad stands in front of the door and presses W (or Up arrow) to go through.
[RequireComponent(typeof(Collider2D))]
public class MazeDoor : MonoBehaviour
{
    public enum DoorType
    {
        Wrong,  // sends Saad back to the start of the same room
        Right,  // takes Saad to the next room (Ayesha stands next to this one)
        Exit    // the Locked Door at the end: finishes World 3
    }

    public DoorType doorType = DoorType.Wrong;

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

    // Shows a small hint while Saad is standing at a door.
    void OnGUI()
    {
        if (!playerInside) return;
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 22;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(0, Screen.height - 70, Screen.width, 40), "Press W to go through the door", style);
    }
}
