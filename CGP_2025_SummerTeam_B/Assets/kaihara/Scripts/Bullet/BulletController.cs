using System.Xml.Serialization;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Pool;

public class BulletController : MonoBehaviour
{
    //地面のレイヤーの番号
    [SerializeField]
    private int groundLayer;
    //ギミックのレイヤーの番号
    [SerializeField]
    private int gimmickLayer;
    //敵のレイヤーの番号
    [SerializeField]
    private int enemyLayer;
    //プレイヤーのレイヤーの番号
    [SerializeField]
    private int playerLayer;
    //マテリアルの配列
    [SerializeField] private Material[] bulletMaterials;
    //rigidbody
    private Rigidbody rb;
    //マテリアル用の変数
    private Renderer bulletMaterial;

    //オブジェクトプールのクラス
    private BulletPool bulletPool;
    //弾の挙動のインスタンスを保存する用の変数
    private BulletBehavior behavior;
    //このクラス
    [SerializeField]private BulletController bulletController;
    //Startだとpoolの方で非アクティブ化するため動かないのでAwakeにしている
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bulletMaterial = GetComponent<Renderer>();
    }

    //inspectorで設定した情報などを受け渡す必要があるためこちらでOnTriggerEnter
    void OnTriggerEnter(Collider other)
    {
        behavior.Collision(other, groundLayer, enemyLayer, playerLayer, gimmickLayer);
    }
    //弾発射時に呼び出し
    public void LaunchBullet(Vector3 launchPosition, Quaternion launchRotation, BulletPool.BulletType type, BulletPool pool)
    {
        //オブジェクトプールのインスタンスを保存
        bulletPool = pool;
        //発射位置に移動
        transform.position = launchPosition;
        //回転
        transform.rotation = launchRotation;
        //受け取った弾の種類から弾の挙動を決定
        SetBulletType(type);
        //ステータスや見た目などの初期設定
        behavior.Launch(bulletController,rb);

    }
    //壁衝突などで呼び出し
    public void EndBullet()
    {
        //behaviorインスタンスの削除
        Destroy(behavior);
        //弾の返還
        bulletPool.ReturnBullet(bulletController);
    }
    //弾の挙動決定
    private void SetBulletType(BulletPool.BulletType type)
    {
        //弾にすでに弾の挙動のインスタンスがあったら消す
        if (behavior != null) Destroy(behavior);
        //弾の種類に応じて分岐
        switch (type)
        {
            //プレイヤーの通常の弾
            case BulletPool.BulletType.PlayerMiddle:
                behavior = gameObject.AddComponent<PlayerMiddleBehavior>();
                bulletMaterial.material = bulletMaterials[0];
                break;
            //プレイヤーの近距離用の弾
            case BulletPool.BulletType.PlayerShort:
                behavior = gameObject.AddComponent<PlayerShortBehavior>();
                bulletMaterial.material = bulletMaterials[1];
                break;
            //プレイヤーの遠距離用の弾
            case BulletPool.BulletType.PlayerLong:
                behavior = gameObject.AddComponent<PlayerLongBehavior>();
                bulletMaterial.material = bulletMaterials[2];
                break;
        }
    }
}