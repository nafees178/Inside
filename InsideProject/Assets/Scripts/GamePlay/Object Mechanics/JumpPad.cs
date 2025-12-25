using UnityEngine;

[RequireComponent(typeof(Collider))]
public class JumpPad : MonoBehaviour
{
    [Header("Launch Settings")]
    public float launchHeight = 5f;
    public float extraHorizontalBoost = 0f;
    [SerializeField] bool usePadForwardAsDirection = true;

    [Header("Jump Pad Settings")]
    [SerializeField] bool affectPlayer = true;
    [SerializeField] bool affectPushables = true;
    [SerializeField] string pushableTag = "Ghost";

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryLaunch(other);
    }

    private void TryLaunch(Collider other)
    {
        if (affectPlayer)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player == null)
                player = other.GetComponentInParent<PlayerController>();

            if (player != null)
            {
                Vector3 launchDir = ComputeLaunchDirection();
                player.LaunchFromExternal(launchDir, launchHeight);
            }
        }

        if (affectPushables)
        {
            if (!string.IsNullOrEmpty(pushableTag) && !other.CompareTag(pushableTag))
                return;

            Rigidbody rb = other.attachedRigidbody;
            if (rb == null || rb.isKinematic)
                return;

            LaunchPushable(rb);
        }
    }

    private Vector3 ComputeLaunchDirection()
    {
        Vector3 launchDir = Vector3.up;

        if (usePadForwardAsDirection)
        {
            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();

            launchDir = (Vector3.up + forward * extraHorizontalBoost).normalized;
        }

        return launchDir;
    }

    private void LaunchPushable(Rigidbody rb)
    {
        Vector3 launchDir = ComputeLaunchDirection();

        float g = Physics.gravity.y;
        float verticalSpeed = Mathf.Sqrt(launchHeight * -4f * g);

        Vector3 upComponent = Vector3.up * verticalSpeed;
        Vector3 horizDir = new Vector3(launchDir.x, 0f, launchDir.z).normalized;
        Vector3 horizComponent = horizDir * verticalSpeed * extraHorizontalBoost;
        Vector3 launchVelocity = upComponent + horizComponent;
        rb.linearVelocity = launchVelocity;
    }
}
