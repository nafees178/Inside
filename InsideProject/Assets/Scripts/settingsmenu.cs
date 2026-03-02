using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class settingsmenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeSlider.value = AudioListener.volume;
    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
    // Update is called once per frame
    

