using UnityEngine;

using UnityEngine.UI;
public class CameraSensitivity : MonoBehaviour
{
    public Slider cameraSlider;

    public float cameraSensitivity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   cameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity", 2f);
    
        cameraSlider.value = cameraSensitivity;

        cameraSlider.onValueChanged.AddListener(SetVolume);
    }
    void SetVolume(float value)
    {
        cameraSensitivity = value;
    }   
    // Update is called once per frame
    void Update()
    {

    }
}
