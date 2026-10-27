using UnityEngine;

//it keeps track of which room Saad is in, moves the camera, tells Ayesha where to
//appear, and decides what happens when Saad uses a door.
public class MazeManager : MonoBehaviour
{
    public static MazeManager Instance { get; private set; }

    [Tooltip("The rooms on the RIGHT path, in order. The first one is where Saad starts. " +
             "Lost rooms (dead ends) don't need to be in this list.")]
    public MazeRoom[] rooms;

    public AyeshaGuide ayesha;

    [Tooltip("Leave empty to use the Main Camera.")]
    public Camera cam;

    [Tooltip("How quickly the camera slides to the next room.")]
    public float cameraSpeed = 8f;

    public MazeRoom CurrentRoom { get; private set; }

    Transform player;
    Rigidbody2D playerBody;
    Vector3 cameraTarget;
    bool worldComplete;
    string message = "";
    float messageTimer;

    void Awake()
    {
        Instance = this;
        if (cam == null) cam = Camera.main;
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null)
        {
            Debug.LogError("MazeManager: no object with the tag Player was found.");
            return;
        }
        player = p.transform;
        playerBody = p.GetComponent<Rigidbody2D>();

        if (rooms == null || rooms.Length == 0 || rooms[0] == null)
        {
            Debug.LogError("MazeManager: drag your rooms into the Rooms list.");
            return;
        }
        EnterRoom(rooms[0], true);
    }

    public void UseDoor(MazeDoor door)
    {
        if (worldComplete) return;

        if (door.doorType == MazeDoor.DoorType.Exit)
        {
            worldComplete = true;
            ShowMessage("World 3 complete", 9999f);
            return;
        }

        // A door that has a room dragged into "Leads To" always goes there.
        if (door.leadsTo != null)
        {
            EnterRoom(door.leadsTo, false);
            return;
        }

        if (door.doorType == MazeDoor.DoorType.Right)
        {
            int index = System.Array.IndexOf(rooms, CurrentRoom);
            if (index >= 0 && index + 1 < rooms.Length) EnterRoom(rooms[index + 1], false);
            else Debug.LogWarning("MazeManager: this Right door has nowhere to go. Fill in its Leads To.");
        }
        else
        {
            SendBackToRoomStart("...this isn't the way.");
        }
    }

    //used by wrong doors with no Leads To, and by pits and spikes.
    public void SendBackToRoomStart(string text)
    {
        if (CurrentRoom == null) return;
        MovePlayer(CurrentRoom.startPoint.position);
        if (!string.IsNullOrEmpty(text)) ShowMessage(text, 2f);
    }

    void EnterRoom(MazeRoom room, bool instant)
    {
        CurrentRoom = room;
        MovePlayer(room.startPoint.position);

        if (ayesha != null)
        {
            if (room.ayeshaSpot != null)
                ayesha.AppearAt(room.ayeshaSpot.position, room.firstGlowTime, room.blinkTime, room.darkTime);
            else
                ayesha.Disappear(); // a lost room: she isn't here
        }

        cameraTarget = new Vector3(room.transform.position.x, room.transform.position.y, cam.transform.position.z);
        if (instant) cam.transform.position = cameraTarget;
    }

    void MovePlayer(Vector3 position)
    {
        if (playerBody != null)
        {
#if UNITY_6000_0_OR_NEWER
            playerBody.linearVelocity = Vector2.zero;
#else
            playerBody.velocity = Vector2.zero;
#endif
            playerBody.position = position;
        }
        player.position = position;
    }

    void LateUpdate()
    {
        if (cam == null) return;
        cam.transform.position = Vector3.Lerp(cam.transform.position, cameraTarget, cameraSpeed * Time.deltaTime);
        if (messageTimer > 0f) messageTimer -= Time.deltaTime;
    }

    void ShowMessage(string text, float seconds)
    {
        message = text;
        messageTimer = seconds;
    }

    // Simple on screen text for the prototype
    void OnGUI()
    {
        GUIStyle small = new GUIStyle(GUI.skin.label);
        small.fontSize = 20;
        small.alignment = TextAnchor.UpperRight;
        small.normal.textColor = new Color(1f, 0.85f, 0.55f);
        GUI.Label(new Rect(0, 15, Screen.width - 25, 40), "Follow Ayesha.", small);

        if (messageTimer > 0f)
        {
            GUIStyle big = new GUIStyle(GUI.skin.label);
            big.fontSize = 34;
            big.alignment = TextAnchor.MiddleCenter;
            big.normal.textColor = Color.white;
            GUI.Label(new Rect(0, Screen.height * 0.35f, Screen.width, 60), message, big);
        }
    }
}
