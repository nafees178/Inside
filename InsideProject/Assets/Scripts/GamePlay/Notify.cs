using UnityEngine;

public class Notify : MonoBehaviour
{
    [SerializeField] string message;
    [SerializeField] float seconds;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<NotificationSystem>().Notify(message, seconds);
        }
    }

}
