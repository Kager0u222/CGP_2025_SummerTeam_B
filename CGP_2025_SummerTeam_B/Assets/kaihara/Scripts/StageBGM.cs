using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageBGM : MonoBehaviour
{
    public static StageBGM instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // すでに存在していれば自分を消す
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "TitleScene")
        {
            Destroy(gameObject);
        }
    }
}
