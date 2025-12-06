using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [Header("Trigger Properties")]
    [SerializeField] bool canTriggeredByPlayer = true;
    [SerializeField] bool canTriggeredByTriggerer = true;

    [Header("Trigger Events")]
    public UnityEvent OnTriggered;
    public UnityEvent OnTriggeredFalse;



    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && canTriggeredByPlayer)
        {
            OnTriggered.Invoke();
        }

        if(other.CompareTag("Triggerer") && canTriggeredByTriggerer)
        {
            OnTriggered.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && canTriggeredByPlayer)
        {
            OnTriggeredFalse.Invoke();
        }

        if (other.CompareTag("Triggerer") && canTriggeredByTriggerer)
        {
            OnTriggeredFalse.Invoke();
        }
    }
}
