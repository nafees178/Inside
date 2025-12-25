using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{

    public Transform initialSpawnPoint;
    public GameObject playerPrefab;

    public static SpawnPointManager instance;

    GameObject currentPlayer;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Spawn(initialSpawnPoint);
    }

    public void Spawn(Transform spawnPoint)
    {
        currentPlayer = Instantiate(playerPrefab, spawnPoint.position,spawnPoint.rotation);
    }

    public void Respawn(Transform spawnPoint = null)
    {
        Destroy(currentPlayer);
        if (spawnPoint != null)
        {
            Spawn(spawnPoint);
        }
    }


}
