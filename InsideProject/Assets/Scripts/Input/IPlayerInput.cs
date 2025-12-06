using UnityEngine;

public interface IPlayerInput
{
    Vector2 Move { get; }
    Vector2 Look { get; }  
    bool JumpPressed { get; }
    bool RunHeld { get; }

    bool recordKeyPressed { get; }

    bool DashPressed { get; }

}
