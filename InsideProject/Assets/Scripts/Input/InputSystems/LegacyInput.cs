using UnityEngine;
using UnityEngine.InputSystem;

public class LegacyInput : MonoBehaviour, IPlayerInput
{
    public Vector2 Move => new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical")
    );

    public Vector2 Look => Mouse.current.delta.ReadValue();

    public bool JumpPressed => Input.GetKeyDown(KeyCode.Space);
    public bool RunHeld => Input.GetKey(KeyCode.LeftShift);

    public bool recordKeyPressed => Input.GetKeyDown(KeyCode.R);

    public bool DashPressed => Input.GetKeyDown(KeyCode.LeftControl);


}
