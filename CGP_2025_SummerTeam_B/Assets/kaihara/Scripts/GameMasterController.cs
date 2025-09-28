using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMasterController : MonoBehaviour
{
    //プレイヤーのオブジェクト
    [SerializeField] private GameObject player;
    //弾のプールのクラス
    [SerializeField] private MagicPool magicPool;
    //バレットタイム用のゲーム全体(主に物理挙動)の速度
    [Range(0f, 3f)] private float physicsSpeed = 1;
    //ロードで画面隠す用
    [SerializeField] private Image blackImage;
    //現在のチェックポイント
    public static Vector3 currentCheckPoint;
    //ロード中かどうか
    private bool isLoading = false;

    void Start()
    {
        foreach (EnemyController enemy in EnemyController.enemys)
        {
            //敵にプレイヤーのオブジェクトと魔法のpoolとこのクラスを渡す
            enemy.PlayerSetter(player);
            enemy.MagicPoolSetter(magicPool);
            enemy.GameMasterSetter(this);
        }

        foreach (MagicController magic in MagicController.magics)
        {
            //魔法の弾にこのクラスを渡す
            magic.GameMasterSetter(this);
        }
        //チェックポイントを通ってたならそこに沸く
        if (currentCheckPoint != Vector3.zero)
            player.transform.position = currentCheckPoint;
        //そうでなければ初期位置
        else player.transform.position = new Vector3(0, 3.5f, 0);
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
            StartLoadScene();
        }
        //画面の黒いやつ薄くする
        if (blackImage.color.a > 0)
            blackImage.color = new Color(0, 0, 0, blackImage.color.a - 0.02f);
        //ロード中なら黒くする
        if (isLoading && blackImage.color.a < 1)
            blackImage.color = new Color(0, 0, 0, blackImage.color.a + 0.04f);
        //黒くなりきったらシーンをロード
        if (isLoading && blackImage.color.a >= 1)
            SceneManager.LoadScene("Stage");
    }
    //ロード
    public void StartLoadScene()
    {
        isLoading = true;
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
    public float PhysicsSpeed { get { return physicsSpeed; } }
    
}
