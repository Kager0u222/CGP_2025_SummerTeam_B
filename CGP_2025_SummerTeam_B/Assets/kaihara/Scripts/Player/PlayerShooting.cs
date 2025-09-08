using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    //射撃のクールタイム
    private float fireCoolTime;
    //弾の射程
    private float bulletLength;
    //弾のprefab
    [SerializeField] private GameObject bulletPrefab;
    //弾の発射位置用オブジェクトの位置取得
    [SerializeField] private Transform bulletFirePosition;
    //弾のオブジェクトプール取得
    [SerializeField] private BulletPool bulletPool;


    //前回射撃時の時間保存用変数
    private float lastFiredTime;
    //魔法のタイプ
    private int magicType = 2;
    //弾のふるまいのクラス
    private BulletBehavior behavior;
    

    public void Shoot(int groundLayer, int enemyLayer, Transform cameraTransform, Transform cameraBaseTransform)
    {

        //前回射撃時から十分時間がたっていなければ終了
        if (Time.time - lastFiredTime < fireCoolTime) return;

        //レイヤーマスクの設定
        LayerMask layerMask = 1 << groundLayer;
        layerMask += 1 << enemyLayer;

        //レイの起点と向きの指定
        Ray ray = new Ray(cameraTransform.position, cameraBaseTransform.forward);
        //射出先情報の保存用変数
        RaycastHit hit;

        //弾射出先の情報の保存
        bool isObjectAtAim = Physics.Raycast(ray, out hit, bulletLength, layerMask);
        //射程内にオブジェクトなしなら射程ギリギリにオブジェクトがあるときと同じにする
        if (!isObjectAtAim) hit.point = cameraTransform.position + cameraTransform.forward * bulletLength;
        //弾召喚
        bulletPool.BorrowBullet(bulletFirePosition.position, Quaternion.LookRotation((hit.point - bulletFirePosition.position).normalized, Vector3.up),(BulletPool.BulletType)magicType);

        //発射した時間を保存
        lastFiredTime = Time.time;
    }
    public void ChangeBullet(float input)
    {
        //マウスホイール上で魔法の番号を増やす
        if (input > 0) magicType += 1;
        //逆
        else magicType -= 1;
        //ループ
        if (magicType >= 3) magicType = 0;
        if (magicType <= -1) magicType = 2;

        BulletPool.BulletType type;
        //番号をenumに変換
        type = (BulletPool.BulletType)magicType;
        //enumに応じてインスタンスをいったん作成
        switch (type)
        {
            case BulletPool.BulletType.PlayerMiddle:
                behavior = gameObject.AddComponent<PlayerMiddleBehavior>();
                break;
            case BulletPool.BulletType.PlayerShort:
                behavior = gameObject.AddComponent<PlayerShortBehavior>();
                break;
            case BulletPool.BulletType.PlayerLong:
                behavior = gameObject.AddComponent<PlayerLongBehavior>();
                break;
        }
        //必要な変数をプロパティで取得
        fireCoolTime = behavior.bulletCoolTime;
        bulletLength = behavior.bulletLength;
        //インスタンス削除
        Destroy(behavior);
    }
}
