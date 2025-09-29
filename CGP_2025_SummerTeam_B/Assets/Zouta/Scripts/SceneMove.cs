using UnityEngine;

public class SceneMove : MonoBehaviour
{
    //シーン移動の演出中か否か
    private bool isSceneMoveing = false;
    //暗転用Image 
    [SerializeField] private UnityEngine.UI.Image fadeImage;
    //Audio
    [SerializeField] private AudioSource audioSource;
    //SE
    [SerializeField] private AudioClip sceneMoveSE;
    //BGMのソース
    [SerializeField] private AudioSource bgmSource;
    public void MoveScene()
    {
        if (isSceneMoveing) return;
        isSceneMoveing = true;
        audioSource.PlayOneShot(sceneMoveSE);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //暗転
        if (isSceneMoveing)
        {
            fadeImage.color += new Color(0, 0, 0, Time.deltaTime);
            bgmSource.volume -= 0.01f;
        }
        //暗転しきったらシーン移動
        if (fadeImage.color.a >= 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
