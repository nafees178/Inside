using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] bool deactivateAfterCaptured = true;
    bool isDeactivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isDeactivated) return;
            CheckPointManager.instance.currentCheckPoint = transform;
            if (deactivateAfterCaptured)
            {
                isDeactivated = true;
            }
        }
    }
}
