using UnityEngine;

public class ResetState : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private Quaternion rotation;
    void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
    }

    public void UpdateCurrentState()
    {
        if(position != transform.position)
        {
            position = transform.position;
        }
        if (rotation != transform.rotation)
        {
            rotation = transform.rotation;
        }
    }

    public void ResetCurrentState()
    {
        if(transform.rotation != rotation)
        {
            transform.rotation = rotation;
        }
        if(transform.position != position)
        {
            transform.position = position;
        }
    }
}
