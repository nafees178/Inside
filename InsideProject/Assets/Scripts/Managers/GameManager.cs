using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    ResetState[] changingObjects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        changingObjects = FindObjectsByType<ResetState>(FindObjectsSortMode.None);
    }

    public void UpdateStates()
    {
        foreach (ResetState state in changingObjects)
        {
            state.UpdateCurrentState();
        }
    }

    public void ResetStates()
    {
        foreach (ResetState state in changingObjects)
        {
            state.ResetCurrentState();
        }
    }
}
