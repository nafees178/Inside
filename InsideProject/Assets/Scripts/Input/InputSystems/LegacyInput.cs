using UnityEngine;

public class LegacyInput : IPlayerInput
{
    public Vector2 Move => new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical")
    );

    public bool JumpPressed => Input.GetButtonDown("Jump");
    public bool RunHeld => Input.GetKey(KeyCode.LeftShift);
}
