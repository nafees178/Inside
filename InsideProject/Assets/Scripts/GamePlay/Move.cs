using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 direction;

    public void MoveTo()
    {
        transform.Translate(direction);
        Debug.Log("Moving");
    }
}
