using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Trigger : MonoBehaviour
{
    [Header("Trigger Properties")]
    [SerializeField] bool canTriggeredByPlayer = true;
    [SerializeField] bool canTriggeredByTriggerer = true;
    [SerializeField] bool canTriggeredByGhost = true;

    [Header("Trigger Events")]
    public UnityEvent OnTriggered;
    public UnityEvent OnTriggeredFalse;

    private HashSet<Collider> activeColliders = new HashSet<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (!IsValidTrigger(other)) return;

        if (activeColliders.Add(other))
        {
            OnTriggered.Invoke();
        }
    }
    private void Update()
    {
        int before = activeColliders.Count;

        activeColliders.RemoveWhere(c => c == null);

        int after = activeColliders.Count;

        // Only fire if something was actually removed
        if (before > 0 && after == 0)
        {
            OnTriggeredFalse.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsValidTrigger(other)) return;

        if (activeColliders.Remove(other))
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