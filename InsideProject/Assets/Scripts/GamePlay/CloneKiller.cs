using UnityEngine;

public class CloneKiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            Destroy(other.gameObject);
        }
    }
}
