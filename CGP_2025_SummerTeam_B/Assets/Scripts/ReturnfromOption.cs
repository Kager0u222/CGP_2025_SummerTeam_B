using UnityEngine;

public class ReturnfromOption : MonoBehaviour
{
    [SerializeField] private GameObject OptionPanel;
    [SerializeField] private BGMConection bGMConnection;

    [SerializeField] private CameraSensitivity cameraSensitivity;

    [SerializeField] private SE sE;
    public void onclick()
    {
        OptionPanel.SetActive(false);

        SaveOptionChanges();

        OptionPanel.SetActive(false);
    }

    private void SaveOptionChanges()
    {
        PlayerPrefs.SetFloat("BGMVolume", bGMConnection.audioSource.volume);
        PlayerPrefs.SetFloat("CameraSensitivity", cameraSensitivity.cameraSensitivity);
        PlayerPrefs.SetFloat("SEVolume", sE.audioSource.volume);
    }
}
