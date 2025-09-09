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

    //地面のレイヤーの番号
    [SerializeField] private Layers layers;

    //カメラ感度
    [SerializeField] private float cameraSensitivity;

    //player inputからの移動キーの入力保存用
    private Vector2 inputDirection;

    //カメラの回転の変数
    private Vector2 cameraRotation;
    //接地判定のbool変数
    private bool onGround = true;
    private bool onWall = false;
    //射撃中か否かを判定するbool変数
    private bool isFire = false;
    //Rigidbody
    private Rigidbody rb;

    //接触したオブジェクトの保存用変数
    private Collision collisionObjects;

    //壁の法線ベクトル
    private Vector3 wallNormalVector;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //接地判定の確認
        PlayerCollision.Collision(collisionObjects);
        //移動処理を実行
        PlayerMovement.Move(inputDirection, cameraBaseTransform, onGround, onWall,rb);
        //ワイヤー展開中の処理
        PlayerWire.Wire(rb,layers.GroundLayer,layers.GimmickLayer);
        //もし射撃中なら射撃
        if (isFire) PlayerShooting.Shoot(layers.GroundLayer,layers.EnemyLayer,cameraTransform,cameraBaseTransform);
        //y座標が一定以下になったらリセット
        if (transform.position.y < -100) SceneManager.LoadScene("PlayerMotionTestScene");
    }

    //接地判定s
    private void OnCollisionEnter(Collision collision)
    {
        collisionObjects = collision;
    }
    private void OnCollisionStay(Collision collision)
    {
        collisionObjects = collision;
    }
    private void OnCollisionExit(Collision collision)
    {
        collisionObjects = null;
    }

    //player inputから移動キー入力に紐づけ
    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }
    //player inputからジャンプキー入力に紐づけ
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //接地判定の確認
            PlayerCollision.Collision(collisionObjects);
            //地面に接していたらジャンプ
            if (onGround || onWall)
            {
                PlayerMovement.Jump(wallNormalVector,rb);
                if (onWall) PlayerWire.EndWire();
            }
        }
    }
    //player inputからワイヤーのキー入力に紐づけ
    public void OnWire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //ワイヤー起動
            PlayerWire.StartWire(cameraTransform, cameraBaseTransform);
        }
        if (context.canceled)
        {
            //ワイヤー切断
            PlayerWire.EndWire();
        }
    }
    //player inputから射撃キー入力に紐づけ
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed) isFire = true;
        if (context.canceled) isFire = false;
    }

    public void OnCameraRotate(InputAction.CallbackContext context)
    {
        //マウスの移動量をカメラの回転量に加算
        cameraRotation.x = Mathf.Clamp(cameraRotation.x + context.ReadValue<Vector2>().y * cameraSensitivity, -90, 80);
        cameraRotation.y += context.ReadValue<Vector2>().x * cameraSensitivity;
        //上の値をカメラの角度に反映
        cameraBaseTransform.rotation = Quaternion.Euler(-cameraRotation.x, cameraRotation.y, 0);
    }
    public void OnChangeMagic(InputAction.CallbackContext context)
    {
        if (context.performed) PlayerShooting.ChangeMagic(context.ReadValue<Vector2>().y);
    }
    //
    public bool OnGround
    {
        get { return onGround; }
        set { onGround = value; }
    }
    public bool OnWall
    {
        get { return onWall; }
        set { onWall = value; }
    }
    public Vector3 WallNormalVector
    {
        get { return wallNormalVector; }
        set { wallNormalVector = value; }
    }
}
