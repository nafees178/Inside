using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationSystem : MonoBehaviour
{
    public TMP_Text notificationText;
    Coroutine clearRoutine;

    void Start()
    {
        notificationText.text = "";
    }

    public void Notify(string message, float seconds)
    {
        notificationText.text = message;

        if (clearRoutine != null)
            StopCoroutine(clearRoutine);

        clearRoutine = StartCoroutine(ClearAfter(seconds));
    }

    IEnumerator ClearAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        notificationText.text = "";
    }
}
