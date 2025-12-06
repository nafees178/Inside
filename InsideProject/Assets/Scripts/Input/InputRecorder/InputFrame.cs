using UnityEngine;

[System.Serializable]
public class InputFrame
{
    public float time;
    public Vector2 move;
    public Vector2 look;
    public bool jumpPressed;
    public bool runHeld;
    public bool dashPressed;
}
