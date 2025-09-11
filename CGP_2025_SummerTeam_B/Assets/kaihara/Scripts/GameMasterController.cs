using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameMasterController : MonoBehaviour
{
    //プレイヤーのオブジェクト
    [SerializeField] private GameObject player;
    //敵のprefab
    [SerializeField] private GameObject enemyPrefab;
    //弾のプールのクラス
    [SerializeField] private MagicPool magicPool;
    //バレットタイム用のゲーム全体(主に物理挙動)の速度
    [Range(0f, 3f)] private float physicsSpeed = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (EnemyController enemy in EnemyController.enemys)
        {
            //敵にプレイヤーのオブジェクトと魔法のpoolの情報を渡す
            enemy.PlayerSetter(player);
            enemy.MagicPoolSetter(magicPool);
        }

        foreach (MagicController magic in MagicController.magics)
        {
            //魔法の弾にこのクラスを渡す
            magic.GameMasterSetter(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //画面左クリックでカーソル消す
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        //escでカーソル戻す
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        //Rでシーンをロード
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("PlayerMotionTestScene");
        }
    }
    void FixedUpdate()
    {
        //スクリプトから物理シミュレーションを行う場合はここで
        if (physicsSpeed != 1)
            Physics.Simulate(Time.fixedDeltaTime * physicsSpeed);
    }
    //物理挙動の処理速度を変更する用のメソッド
    public void PhysicsSpeedSetter(float value)
    {
        //値を変更
        physicsSpeed = value;
        //もし通常速度でなければ物理シミュレーションをスクリプトで行う
        if (physicsSpeed != 1) Physics.simulationMode = SimulationMode.Script;
        //速度が通常速度なら普通に処理する
        else Physics.simulationMode = SimulationMode.FixedUpdate;
    }
    public float PhysicsSpeed {get{ return physicsSpeed; }}
}
