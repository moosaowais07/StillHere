using UnityEngine;

// Runs World 3. Put ONE of these in the scene, on an empty object called "MazeManager".
// It keeps track of which room Saad is in, moves the camera, tells Ayesha where to
// stand, and decides what happens when Saad uses a door.
public class MazeManager : MonoBehaviour
{
    public static MazeManager Instance { get; private set; }

    [Tooltip("All the rooms, in the order Saad goes through them.")]
    public MazeRoom[] rooms;

    public AyeshaGuide ayesha;

    [Tooltip("Leave empty to use the Main Camera.")]
    public Camera cam;

    [Tooltip("How quickly the camera slides to the next room.")]
    public float cameraSpeed = 8f;

    public int CurrentRoomIndex { get; private set; }

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

        if (rooms == null || rooms.Length == 0)
        {
            Debug.LogError("MazeManager: drag your rooms into the Rooms list.");
            return;
        }
        EnterRoom(0, true);
    }

    public void UseDoor(MazeDoor door)
    {
        if (worldComplete) return;

        switch (door.doorType)
        {
            case MazeDoor.DoorType.Right:
                if (CurrentRoomIndex + 1 < rooms.Length) EnterRoom(CurrentRoomIndex + 1, false);
                break;

            case MazeDoor.DoorType.Wrong:
                MovePlayer(rooms[CurrentRoomIndex].startPoint.position);
                ShowMessage("...this isn't the way.");
                break;

            case MazeDoor.DoorType.Exit:
                worldComplete = true;
                ShowMessage("World 3 complete");
                messageTimer = 9999f;
                break;
        }
    }

    void EnterRoom(int index, bool instant)
    {
        CurrentRoomIndex = index;
        MazeRoom room = rooms[index];

        MovePlayer(room.startPoint.position);
        if (ayesha != null && room.ayeshaSpot != null) ayesha.MoveTo(room.ayeshaSpot.position);

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

    void ShowMessage(string text)
    {
        message = text;
        messageTimer = 2f;
    }

    // Simple on screen text for the prototype (no UI setup needed).
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
