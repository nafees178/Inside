using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Type")]
    public bool rotatableDoor = false;

    [Header("Sliding Door Positions")]
    public Transform closedPoint;
    public Transform openPoint;

    [Header("Rotating Door Parts")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Rotation Settings")]
    public float openAngle = 90f;
    public float rotationSpeed = 120f;

    [Header("Sliding Settings")]
    public float moveSpeed = 2f;

    [Header("Trigger Requirement")]
    public bool useTriggerRequirement = false;
    public int requiredTriggerCount = 1;

    [SerializeField] private int currentTriggerCount = 0;
    private bool isOpen = false;
    private Coroutine moveRoutine;

    private Quaternion leftDefaultRot;
    private Quaternion rightDefaultRot;

    private void Start()
    {
        if (rotatableDoor)
        {
            if (leftDoor != null)
                leftDefaultRot = leftDoor.localRotation;

            if (rightDoor != null)
                rightDefaultRot = rightDoor.localRotation;
        }
    }

    // ================= TRIGGER FUNCTIONS =================

    public void AddTrigger()
    {
        currentTriggerCount++;

        if (useTriggerRequirement)
            EvaluateDoorState();
        else
            OpenDoor();
    }

    public void RemoveTrigger()
    {
        currentTriggerCount--;
        if (currentTriggerCount < 0)
            currentTriggerCount = 0;

        if (useTriggerRequirement)
            EvaluateDoorState();
        else
            CloseDoor();
    }

    private void EvaluateDoorState()
    {
        if (currentTriggerCount >= requiredTriggerCount)
            OpenDoor();
        else
            CloseDoor();
    }

    // ================= DOOR CONTROL =================

    public void OpenDoor()
    {
        if (isOpen) return;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        if (rotatableDoor)
            moveRoutine = StartCoroutine(RotateDoor(true));
        else
            moveRoutine = StartCoroutine(MoveDoor(openPoint.position));

        isOpen = true;
    }

    public void CloseDoor()
    {
        if (!isOpen) return;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        if (rotatableDoor)
            moveRoutine = StartCoroutine(RotateDoor(false));
        else
            moveRoutine = StartCoroutine(MoveDoor(closedPoint.position));

        isOpen = false;
    }

    // ================= SLIDING =================

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

    // ================= ROTATING =================

    private IEnumerator RotateDoor(bool opening)
    {
        Quaternion leftTarget = leftDefaultRot;
        Quaternion rightTarget = rightDefaultRot;

        if (opening)
        {
            if (leftDoor != null)
                leftTarget = leftDefaultRot * Quaternion.Euler(0, -openAngle, 0);

            if (rightDoor != null)
                rightTarget = rightDefaultRot * Quaternion.Euler(0, openAngle, 0);
        }

        while (true)
        {
            bool done = true;

            if (leftDoor != null)
            {
                leftDoor.localRotation = Quaternion.RotateTowards(
                    leftDoor.localRotation,
                    leftTarget,
                    rotationSpeed * Time.deltaTime
                );

                if (Quaternion.Angle(leftDoor.localRotation, leftTarget) > 0.1f)
                    done = false;
            }

            if (rightDoor != null)
            {
                rightDoor.localRotation = Quaternion.RotateTowards(
                    rightDoor.localRotation,
                    rightTarget,
                    rotationSpeed * Time.deltaTime
                );

                if (Quaternion.Angle(rightDoor.localRotation, rightTarget) > 0.1f)
                    done = false;
            }

            if (done)
                break;

            yield return null;
        }

        if (leftDoor != null)
            leftDoor.localRotation = leftTarget;

        if (rightDoor != null)
            rightDoor.localRotation = rightTarget;
    }
}