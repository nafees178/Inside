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

    private int activeTriggerCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsValidTrigger(other)) return;

        activeTriggerCount++;

        if (activeTriggerCount == 1)
        {
            OnTriggered.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsValidTrigger(other)) return;

        activeTriggerCount--;
        activeTriggerCount = Mathf.Max(0, activeTriggerCount);

        if (activeTriggerCount == 0)
        {
            OnTriggeredFalse.Invoke();
        }
    }

    private bool IsValidTrigger(Collider other)
    {
        return
            (other.CompareTag("Player") && canTriggeredByPlayer) ||
            (other.CompareTag("Triggerer") && canTriggeredByTriggerer) ||
            (other.CompareTag("Ghost") && canTriggeredByGhost);
    }
}
