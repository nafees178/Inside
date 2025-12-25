using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [Header("Trigger Properties")]
    [SerializeField] bool canTriggeredByPlayer = true;
    [SerializeField] bool canTriggeredByTriggerer = true;
    [SerializeField] bool canTriggeredByGhost = true;

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

        if(other.CompareTag("Ghost") && canTriggeredByGhost)
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

        if (other.CompareTag("Ghost") && canTriggeredByGhost)
        {
            OnTriggeredFalse.Invoke();
        }
    }
}
