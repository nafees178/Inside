using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string doorOpenAnimationClip;
    [SerializeField] string doorCloseAnimationClip;

    public bool isOpen;

    public void OpenDoor()
    {
        if (isOpen) return;
        animator.CrossFade(doorOpenAnimationClip, 0.15f);
        isOpen = true;
    }

    public void CloseDoor()
    {
        if (!isOpen) return;
        animator.CrossFade(doorCloseAnimationClip, 0.15f);
        isOpen = false;
    }
}
