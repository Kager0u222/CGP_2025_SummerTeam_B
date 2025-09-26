using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BGMConection : MonoBehaviour
{
    public Slider volumeSlider;

    public AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        audioSource.volume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        
        volumeSlider.value = audioSource.volume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float value)
    {
        audioSource.volume = value;
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
