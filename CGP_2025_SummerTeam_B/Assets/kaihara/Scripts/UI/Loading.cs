using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using System;

public class Loading : MonoBehaviour
{
    //image
    [SerializeField] private Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Stage");
        

        // シーンの読み込みが完了するまで待機
        while (!asyncLoad.isDone)
        {
            image.rectTransform.rotation = Quaternion.Euler(0, 0, image.rectTransform.rotation.eulerAngles.z + 12);
            if (Mathf.Clamp01(asyncLoad.progress / 0.9f) >= 1.0f)
            {
                yield return new WaitForSeconds(1f);
                image.color = new Color(1, 1, 1, image.color.a - 0.02f);
            }
            yield return null;
        }
    }
}
