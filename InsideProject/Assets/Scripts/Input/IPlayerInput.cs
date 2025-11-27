using UnityEngine;

public interface IPlayerInput
{
    Vector2 Move { get; }       
    bool JumpPressed { get; }
    bool RunHeld { get; }
}
