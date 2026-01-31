using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] float time;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(sceneChanger());
        }
    }

    IEnumerator sceneChanger()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }
}
