using System.Data;
using System.Runtime.CompilerServices;
using NUnit.Framework.Internal.Execution;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //rigidbody用変数
    private Rigidbody rb;
    //移動速度の変数
    [SerializeField]
    private float playerSpeed;
    //移動処理のaddforceの倍率(大きいほど移動がキー入力に追従しやすくなる)
    [SerializeField]
    private float moveAddforcePower;
    //空中での移動処理のaddforceの倍率(大きいほど移動がキー入力に追従しやすくなる)
    [SerializeField]
    private float moveAddforcePowerinAir;
    //ジャンプ力の変数
    [SerializeField]
    private float jumpPower;
    //重力の倍率
    [SerializeField]
    private float gravityPower;

    //カメラの回転を取得する
    [SerializeField]
    private Transform cameraBaseTransform;
    //カメラの位置を取得する
    [SerializeField]
    private Transform cameraPosition;
    //ワイヤーの発射位置用オブジェクトの位置取得
    [SerializeField]
    private Transform wireFireposition;
    //弾の発射位置用オブジェクトの位置取得
    [SerializeField]
    private Transform bulletFirePosition;
    //地面判定の補助用オブジェクトのスクリプト取得
    [SerializeField]
    private OnGroundChcker subOnGroundChecker;
    //ワイヤーの長さ
    [SerializeField]
    private float wireLength;
    //ワイヤーの引っ張る強さ
    [SerializeField]
    private float wirePower;
    //ワイヤーの力の減衰
    [SerializeField]
    private float wireDamping;
    //ワイヤーの長さ(MAX)
    [SerializeField]
    private float wireMaxLength;
    //ワイヤーの長さ(Min)
    [SerializeField]
    private float wireMinLength;
    //ワイヤーの太さ
    [SerializeField]
    private float wireWidth;
    //ワイヤーの色
    [SerializeField]
    private Color wireColor;
    //ワイヤーのマテリアル
    [SerializeField]
    private Material wireMaterial;
    //射撃のクールタイム
    [SerializeField]
    private float fireCoolTime;
    //弾の射程
    [SerializeField]
    private float bulletLength;
    //弾のprefab
    [SerializeField]
    private GameObject bulletPrefab;
    //地面のレイヤーの番号
    [SerializeField]
    private int groundLayer;
    //敵のレイヤーの番号
    [SerializeField]
    private int enemyLayer;
    //壁ジャンプしたときに真上方向のベクトルからどれくらい斜めに飛ぶようにするか
    [SerializeField]
    private float wallJumpAngle;

    //player inputからの移動キーの入力保存用
    private Vector2 inputDirection;
    //前回射撃時の時間保存用変数
    private float lastFiredTime;
    //接地判定のbool変数
    private bool onGround = true;
    private bool onWall = false;
    //射撃中か否かを判定するbool変数
    private bool isFire = false;

    //接触したオブジェクトの保存用変数
    private Collision collisionObjects;
    //ワイヤーがつながってる時の情報を保存するやつ
    private SpringJoint sj;
    //linerendererの情報保存用
    private LineRenderer lr;
    //壁の法線ベクトルを保存
    private Vector3 wallNomrlVector;

    void Start()
    {
        //rigidbody代入
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //ワイヤー展開中ならワイヤーの表示の視点を更新する
        if (lr)
        {
            lr.SetPosition(0, wireFireposition.position);
            lr.SetPosition(1, sj.connectedBody.transform.TransformPoint(sj.connectedAnchor));
        }
        //接地判定の確認
        Collision();
        //移動処理を実行
        Move();
        //もし射撃中なら射撃
        if (isFire) Fire();

        //y座標が一定以下になったらリセット
        if (transform.position.y < -100) SceneManager.LoadScene(0);
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

    //接地判定の処理
    private void Collision()
    {
        onGround = false;
        onWall = false;
        wallNomrlVector = Vector3.zero;
        if (collisionObjects == null) return;
        foreach (ContactPoint contactPoint in collisionObjects.contacts)
        {
            //床なら床のboolをtrue
            if (Vector3.Angle(Vector3.down, contactPoint.normal) >= 0 && Vector3.Angle(Vector3.down, contactPoint.normal) < 85)
                onGround = true;
            //壁なら壁のboolをtrue
            if (Vector3.Angle(Vector3.down, contactPoint.normal) <= 95 && Vector3.Angle(Vector3.down, contactPoint.normal) >= 85)
            {
                onWall = true;
                wallNomrlVector = contactPoint.normal;
            }
        }
        if (onGround==false) onGround = subOnGroundChecker.OnGroundGetter();
    }
    //移動処理
    private void Move()
    {
        //向いている向きをカメラに合わせる
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraBaseTransform.eulerAngles.y, transform.eulerAngles.z);
        //入力を変換して視点依存の移動ベクトルに
        Vector3 moveDirection = transform.forward * inputDirection.y + transform.right * inputDirection.x;
        //計算後のベクトルに従って力を加える
        //地面にいるとき
        if (onGround)
            rb.AddForce((moveDirection.x * playerSpeed - rb.linearVelocity.x) * moveAddforcePower, 0, (moveDirection.z * playerSpeed - rb.linearVelocity.z) * moveAddforcePower);
        //空中にいるとき
        else
        {
            rb.AddForce((moveDirection.x * playerSpeed - rb.linearVelocity.x / 4) * moveAddforcePowerinAir, 0, (moveDirection.z * playerSpeed - rb.linearVelocity.z / 4) * moveAddforcePowerinAir);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y - 9.8f * gravityPower * Time.fixedDeltaTime, rb.linearVelocity.z);
        }
    }

    //ワイヤー起動
    private void StartWire()
    {
        //レイヤーマスクの設定
        LayerMask layerMask = 1 << groundLayer;

        //レイの起点と向きの指定
        Ray ray = new Ray(cameraPosition.position, cameraBaseTransform.forward);

        //射出先情報の保存用変数
        RaycastHit hit;

        //ワイヤー射出先の情報の保存と射出可能かの判断用bool値の保存
        bool canFireWire = Physics.Raycast(ray, out hit, wireLength, layerMask);

        //射程内につなげるとこがなければ終了
        if (!canFireWire) return;

        //playerObjectにspring jointをつけていろいろ設定
        var spring = gameObject.AddComponent<SpringJoint>();
        //接地点とplayerObjectを接続
        spring.connectedBody = hit.rigidbody;
        spring.connectedAnchor = hit.transform.InverseTransformPoint(hit.point);

        spring.autoConfigureConnectedAnchor = false;

        //弾性力と減衰の設定
        spring.spring = wirePower;
        spring.damper = wireDamping;
        spring.maxDistance = wireMaxLength;
        spring.minDistance = wireMinLength;
        spring.enableCollision = true;
        //springjointを保存
        sj = gameObject.GetComponent<SpringJoint>();

        //ワイヤーを表示
        var wire = gameObject.AddComponent<LineRenderer>();
        //ワイヤーの設定
        //ワイヤーの両端を指定
        wire.SetPosition(0, wireFireposition.position);
        wire.SetPosition(1, hit.point);
        //ワイヤーの太さ
        wire.startWidth = wireWidth;
        wire.endWidth = wireWidth;
        //ワイヤーの色
        wire.material = wireMaterial;
        wire.startColor = wireColor;
        wire.endColor = wireColor;
        lr = gameObject.GetComponent<LineRenderer>();
    }
    //ワイヤー切断
    private void EndWire()
    {
        Destroy(sj);
        Destroy(lr);
        sj = null;
        lr = null;
    }
    //射撃
    private void Fire()
    {

        //前回射撃時から十分時間がたっていなければ終了
        if (Time.time - lastFiredTime < fireCoolTime) return;

        //レイヤーマスクの設定
        LayerMask layerMask = 1 << groundLayer;
        layerMask += 1 << enemyLayer; 

        //レイの起点と向きの指定
        Ray ray = new Ray(cameraPosition.position, cameraBaseTransform.forward);
        //射出先情報の保存用変数
        RaycastHit hit;

        //弾射出先の情報の保存
        bool isObjectAtAim = Physics.Raycast(ray, out hit, bulletLength, layerMask);
        //射程内にオブジェクトなしなら射程ギリギリにオブジェクトがあるときと同じにする
        if (!isObjectAtAim) hit.point = cameraPosition.position + cameraPosition.forward * bulletLength;
        //弾召喚
        GameObject bullet = Instantiate(bulletPrefab, bulletFirePosition.position, Quaternion.LookRotation((hit.point-bulletFirePosition.position).normalized,Vector3.up));
        bullet.GetComponent<BulletController>().BulletLenghtSetter(bulletLength);
        //発射した時間を保存
        lastFiredTime = Time.time;
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
            Collision();
            //地面に接していたらジャンプ
            if (onGround)
                rb.AddForce(0, jumpPower + 9.8f * gravityPower / 2, 0, ForceMode.Impulse);
        }
    }
    //player inputからワイヤーのキー入力に紐づけ
    public void OnWire(InputAction.CallbackContext context)
    {
        if (context.performed) StartWire();
        if (context.canceled) EndWire();
    }
    //player inputから射撃キー入力に紐づけ
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed) isFire = true;
        if (context.canceled) isFire = false;
    }
    //弾の射程のgetter
    public float BulletLengthGetter()
    {
        return bulletLength;
    }
}
