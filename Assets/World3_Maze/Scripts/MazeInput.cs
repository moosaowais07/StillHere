using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Reads the keyboard for the World 3 test scripts.
// Works whether your project uses the old Input Manager or the new Input System.
//   Move:  A / D  or  Left / Right arrows
//   Jump:  Space
//   Use a door:  W  or  Up arrow
//   Attack:  J
public static class MazeInput
{
    public static float Horizontal()
    {
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        if (k == null) return 0f;
        float h = 0f;
        if (k.aKey.isPressed || k.leftArrowKey.isPressed) h -= 1f;
        if (k.dKey.isPressed || k.rightArrowKey.isPressed) h += 1f;
        return h;
#else
        return Input.GetAxisRaw("Horizontal");
#endif
    }

    public static bool JumpPressed()
    {
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        return k != null && k.spaceKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Space);
#endif
    }

    public static bool UpPressed()
    {
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        return k != null && (k.wKey.wasPressedThisFrame || k.upArrowKey.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
#endif
    }

    public static bool AttackPressed()
    {
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        return k != null && k.jKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.J);
#endif
    }
}
