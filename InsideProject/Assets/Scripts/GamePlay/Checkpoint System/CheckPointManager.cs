using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;
    [SerializeField] Transform[] checkPoints;
    public Transform currentCheckPoint;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentCheckPoint = checkPoints[0];
    }

}
