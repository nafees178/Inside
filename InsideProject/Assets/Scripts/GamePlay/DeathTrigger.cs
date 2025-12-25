using UnityEngine;

public class DeathTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnPointManager.instance.Respawn(CheckPointManager.instance.currentCheckPoint);
            GameManager.instance.ResetStates();
        }
        if (other.CompareTag("Ghost"))
        {
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Triggerer"))
        {
            other.GetComponent<ResetState>().ResetCurrentState();
        }
    }
}
