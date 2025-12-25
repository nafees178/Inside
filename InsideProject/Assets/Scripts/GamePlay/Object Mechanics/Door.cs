using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Door : MonoBehaviour
{
    [Header("Door Positions")]
    public Transform closedPoint;
    public Transform openPoint;

    [Header("Movement")]
    public float moveSpeed = 2f;

    private Coroutine moveRoutine;
    public bool isOpen = false;

    public void OpenDoor()
    {
        if (isOpen) return;
        StartMove(openPoint.position);
        isOpen = true;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, closedPoint.position) < 0.01f)
        {
            isOpen = false;
        } else if(Vector3.Distance(transform.position, openPoint.position) < 0.01f)
        {
            isOpen=true;
        }
    }

    public void CloseDoor()
    {
        if (!isOpen) return;
        StartMove(closedPoint.position);
        isOpen = false;
    }

    private void StartMove(Vector3 targetPosition)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveDoor(targetPosition));
    }

    private IEnumerator MoveDoor(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = target;
    }
}
