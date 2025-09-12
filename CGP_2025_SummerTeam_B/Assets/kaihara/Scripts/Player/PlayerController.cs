using System.Data;
using System.Runtime.CompilerServices;
using NUnit.Framework.Internal.Execution;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    //カメラの回転を取得する
    [SerializeField] private Transform cameraBaseTransform;
    //カメラの位置を取得する
    [SerializeField] private Transform cameraTransform;
    //プレイヤーの移動をつかさどるクラス
    [SerializeField] private PlayerMovement PlayerMovement;
    //ワイヤー関連をつかさどるクラス
    [SerializeField] private PlayerWire PlayerWire;
    //コリジョン処理をつかさどるクラス
    [SerializeField] private PlayerCollision PlayerCollision;
    //射撃をつかさどるクラス
    [SerializeField] private PlayerShooting PlayerShooting;
    //ゲームの全体を処理するクラス
    [SerializeField] private GameMasterController gameMasterController;

    //地面のレイヤーの番号
    [SerializeField] private Layers layers;

    //カメラ感度
    [SerializeField] private float cameraSensitivity;
    //バレットタイム時の物理挙動の時間の経過速度
    [SerializeField, Range(0f, 3f)] private float bulletTimeSpeed;

    //player inputからの移動キーの入力保存用
    private Vector2 inputDirection;

    //カメラの回転の変数
    private Vector2 cameraRotation;
    //接地判定のbool変数
    private bool onGround = true;
    private bool onWall = false;
    //射撃中か否かを判定するbool変数
    private bool isFire = false;
    //ワイヤー接続中か否か
    private bool isWire = false;
    //Rigidbody
    private Rigidbody rb;

    //壁の法線ベクトル
    private Vector3 wallNormalVector;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //接地判定の確認
        PlayerCollision.Collision();
        //移動処理を実行
        PlayerMovement.Move(inputDirection, cameraBaseTransform, onGround, onWall, rb,isFire,isWire);
        //ワイヤー展開中の処理
        PlayerWire.Wire(rb, layers.GroundLayer, layers.GimmickLayer);
        //もし射撃中なら射撃
        if (isFire) PlayerShooting.Shoot(layers.GroundLayer, layers.EnemyLayer, cameraTransform, cameraBaseTransform);
        //y座標が一定以下になったらリセット
        if (transform.position.y < -100) SceneManager.LoadScene("PlayerMotionTestScene");
    }
    //移動処理
    //player inputから移動キー入力に紐づけ
    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }
    //ジャンプ処理
    //player inputからジャンプキー入力に紐づけ
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //接地判定の確認
            PlayerCollision.Collision();
            //地面に接していたらジャンプ
            if (onGround || onWall)
            {
                PlayerMovement.Jump(wallNormalVector, rb);
                if (onWall) PlayerWire.EndWire();
            }
        }
    }
    //ワイヤー発射処理
    //player inputからワイヤーのキー入力に紐づけ
    public void OnWire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //ワイヤー起動
            PlayerWire.StartWire(cameraTransform, cameraBaseTransform);
            isWire = true;
        }
        if (context.canceled)
        {
            //ワイヤー切断
            PlayerWire.EndWire();
            isWire = false;
        }
    }
    //魔法発射処理
    //player inputから射撃キー入力に紐づけ
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed) isFire = true;
        if (context.canceled) isFire = false;
    }
    //カメラ操作処理
    //player inputからカメラ操作に紐づけ
    //さすがに短いので処理は直接ここに
    public void OnCameraRotate(InputAction.CallbackContext context)
    {
        //マウスの移動量をカメラの回転量に加算
        cameraRotation.x = Mathf.Clamp(cameraRotation.x + context.ReadValue<Vector2>().y * cameraSensitivity, -90, 80);
        cameraRotation.y += context.ReadValue<Vector2>().x * cameraSensitivity;
        //上の値をカメラの角度に反映
        cameraBaseTransform.rotation = Quaternion.Euler(-cameraRotation.x, cameraRotation.y, 0);
    }
    //魔法切り替え処理
    //player inputから切り替え操作に紐づけ
    public void OnChangeMagic(InputAction.CallbackContext context)
    {
        if (context.performed) PlayerShooting.ChangeMagic(context.ReadValue<Vector2>().y);
    }
    //バレットタイム処理
    //player inputからバレットタイム操作に紐づけ
    public void OnBulletTime(InputAction.CallbackContext context)
    {
        //押したときに時間経過減速
        if (context.performed) gameMasterController.PhysicsSpeedSetter(bulletTimeSpeed);
        //離したときに時間経過を戻す
        if (context.canceled) gameMasterController.PhysicsSpeedSetter(1);
    }
    
    //コリジョン系の値の取得変更
    //取得は簡単に
    public bool OnGround { get { return onGround; } }
    public bool OnWall { get { return onWall; } }
    public Vector3 WallNormalVector { get { return wallNormalVector; } }
    //変更は安全性のためちょっと回りくどく
    public void OnGroundSetter(bool value)
    {
        onGround = value;
    }
    public void OnWallSetter(bool value)
    {
        onWall = value;
    }
    public void WallNormalVectorSetter(Vector3 value)
    {
        wallNormalVector = value;
    }
}
