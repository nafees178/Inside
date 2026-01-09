using UnityEngine;

public class DisableClone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloneSpawnner playerCloner = other.GetComponent<CloneSpawnner>();
            if (playerCloner != null)
            {
                playerCloner.canSpawnClone = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloneSpawnner playerCloner = other.GetComponent<CloneSpawnner>();
            if (playerCloner != null)
            {
                playerCloner.canSpawnClone = true;
            }
        }
    }
}
