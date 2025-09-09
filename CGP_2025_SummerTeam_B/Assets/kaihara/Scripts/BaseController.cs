using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class BaseController : MonoBehaviour
{
    //プレイヤーのオブジェクト
    [SerializeField] private GameObject player;
    //敵のprefab
    [SerializeField] private GameObject enemyPrefab;
    //弾のプールのクラス
    [SerializeField] private MagicPool magicPool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (EnemyController enemy in EnemyController.enemys)
        {
            enemy.Player = player;
            enemy.magicPool = magicPool;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("PlayerMotionTestScene");
        }
        
    }
}
