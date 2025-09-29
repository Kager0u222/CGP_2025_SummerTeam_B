using UnityEngine;

public class OptionController : MonoBehaviour
{
    //カメラ感度スライダー
    [SerializeField] private UnityEngine.UI.Slider sensitivitySlider;
    //BGM音量スライダー
    [SerializeField] private UnityEngine.UI.Slider bgmSlider;
    //SE音量スライダー
    [SerializeField] private UnityEngine.UI.Slider seSlider;
    //オプションパネル
    [SerializeField] private GameObject optionPanel;
    //AudioMixer
    [SerializeField] private UnityEngine.Audio.AudioMixer audioMixer;
    //audio
    [SerializeField] private AudioSource audioSource;
    //uicansel
    [SerializeField] private AudioClip uiCancel;
    //uidicide
    [SerializeField] private AudioClip uiDecide;
    //ゲームの全体を処理するクラス
    [SerializeField] private GameMasterController gameMaster;

    //パネル展開中か否か
    private bool isOpen = false;
    //↑のゲッター
    public bool IsOpen { get { return isOpen; } }
    public void CameraSensitivity()
    {
        PlayerPrefs.SetFloat("CameraSensitivity", sensitivitySlider.value);
    }
    public void BGM()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        audioMixer.SetFloat("BGM", bgmSlider.value);
    }
    public void SE()
    {
        PlayerPrefs.SetFloat("SEVolume", seSlider.value);
        audioMixer.SetFloat("SE", seSlider.value);
    }
    //起動
    public void UIOpen()
    {
        if(isOpen) return;
        isOpen = true;
        optionPanel.SetActive(true);
        optionPanel.transform.localScale = new Vector3(0, 0, 1);
        audioSource.PlayOneShot(uiDecide);
    }
    //終了
    public void UIClose()
    {
        isOpen = false;
        audioSource.PlayOneShot(uiCancel);
        if (gameMaster == null) return;
        gameMaster.PhysicsSpeedSetter(1);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        if (isOpen && optionPanel.transform.localScale.x < 1)
        {
            optionPanel.transform.localScale += new Vector3(0.1f, 0.1f, 0);
        }
        if (!isOpen && optionPanel.activeSelf)
        {
            optionPanel.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            if (optionPanel.transform.localScale.x <= 0)
            {
                optionPanel.SetActive(false);
            }
        }
    }
    void Start()
    {
        optionPanel.SetActive(false);
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0f);
        audioMixer.SetFloat("BGM", bgmSlider.value);
        seSlider.value = PlayerPrefs.GetFloat("SEVolume", 0f);
        audioMixer.SetFloat("SE", seSlider.value);
        sensitivitySlider.value = PlayerPrefs.GetFloat("CameraSensitivity", 1f);  
    }
}
