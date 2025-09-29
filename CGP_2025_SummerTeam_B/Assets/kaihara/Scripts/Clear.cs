using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    [SerializeField] private AudioClip clearSE;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Layers layers;
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private UnityEngine.UI.Image blackImage;
    private bool isCleared = false;
    private float isClearedTime = 0f;
    private AudioSource bgmSource;
    //inputActionのラッパークラス
    private InputSystem_Actions inputActions;

    private void OnTriggerEnter(Collider other)
    {
        if (isCleared) return;
        if (other.gameObject.layer == layers.PlayerLayer)
        {
            audioSource.PlayOneShot(clearSE);
            isCleared = true;
            isClearedTime = Time.time;
            clearPanel.SetActive(true);
            bgmSource = StageBGM.instance.GetComponent<AudioSource>();
        }
    }
    private void Update()
    {
        if (isCleared && Time.time - isClearedTime > 8f)
        {
            blackImage.color = new Color(0, 0, 0, blackImage.color.a + 0.03f);
            bgmSource.volume -= 0.02f;

            if (blackImage.color.a >= 1)
            {
                inputActions = new InputSystem_Actions();
                inputActions.Player.Disable();
                inputActions.Dispose();
                SceneManager.LoadScene("ClearLoad");
            }
            
        }
    }
}
