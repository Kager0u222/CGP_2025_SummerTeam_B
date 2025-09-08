
using UnityEditor.PackageManager;
using UnityEngine;

public abstract class BulletBehavior : MonoBehaviour
{

    //rigidbody
    private Rigidbody rb;

    //弾の速度
    public abstract float bulletSpeed { get; }
    //弾の射程
    public abstract float bulletLength { get; }
    //弾の威力
    public abstract float bulletDamage { get; }
    //弾のブレ
    public abstract float bulletShake { get; }
    //弾が敵のものであるか否か
    public abstract bool bulletIsEnemy { get; }
    //クールタイム
    public abstract float bulletCoolTime { get; }
    //弾が発射された時間を保存する
    private float firedTime;
    //オブジェクトプールとのやり取りをする奴
    private BulletController bulletController;
    public void Launch(BulletController bullet,Rigidbody rigidbody)
    {
        //rigidbody
        rb = rigidbody;
        //bulletcontrollerのインスタンス取得
        bulletController = bullet;
        //発射された時間を保存
        firedTime = Time.time;
        //ステータス設定
        Status();
        //乱数で向きを調整
        float anglermd = Random.Range(-bulletShake / 2, bulletShake / 2);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + anglermd, transform.rotation.eulerAngles.y + anglermd, transform.rotation.eulerAngles.z);
        //移動速度設定
        rb.linearVelocity = transform.forward*bulletSpeed;
        
    }

    void FixedUpdate()
    {
        Movement();
        if (Time.time - firedTime > bulletLength / bulletSpeed) bulletController.EndBullet();
    }
    public void Collision(Collider other,int groundLayer,int enemyLayer,int playerLayer,int gimmickLayer)
    {
        //地面に触れたら消滅
        if (other.gameObject.layer == groundLayer) bulletController.EndBullet();

        //自分の弾が敵もしくはギミックに触れたらもしくは敵の弾が自分に触れたら
        if (((other.gameObject.layer == enemyLayer || other.gameObject.layer == gimmickLayer) && !bulletIsEnemy) || (bulletIsEnemy && other.gameObject.layer == playerLayer))
        {
            //消滅
            bulletController.EndBullet();
            //接触後の特殊な判定
            Hit();
            if (other.gameObject.layer == gimmickLayer) return;
        }
        
    }
    
    public abstract void Status();

    public abstract void Movement();

    public abstract void Hit();
    
    
}



