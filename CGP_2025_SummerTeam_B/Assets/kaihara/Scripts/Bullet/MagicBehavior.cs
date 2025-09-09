
using UnityEditor.PackageManager;
using UnityEngine;

public abstract class MagicBehavior : MonoBehaviour
{

    //rigidbody
    private Rigidbody rb;
    //レイヤーのScriptableObject
    private Layers layers;

    //弾のステータス確認に使う変数
    private IMagicStatus currentStatus;
    //弾が発射された時間を保存する
    private float firedTime;
    //オブジェクトプールとのやり取りをする奴
    private MagicController magicController;
    //ステータスのScriptableObjct
    private MagicStatuses currentStatuses;
    public void Launch(MagicController magic, Rigidbody rigidbody, Layers layer, MagicTypeAsset.MagicType type,MagicStatuses statuses)
    {
        //rigidbody
        rb = rigidbody;
        //ステータスのScriptableObject
        currentStatuses = statuses;
        //レイヤーのSCriptableObject
        layers = layer;
        //magiccontrollerのインスタンス取得
        magicController = magic;
        //typeから取得すべきステータスのクラスを取得
        currentStatus = currentStatuses.GetStatus(type);
        //発射された時間を保存
        firedTime = Time.time;
        //乱数で向きを調整
        float angleXrmd = Random.Range(-currentStatus.MagicShake / 2, currentStatus.MagicShake / 2);
        float angleYrmd = Random.Range(-currentStatus.MagicShake / 2, currentStatus.MagicShake / 2);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + angleXrmd, transform.rotation.eulerAngles.y + angleYrmd, transform.rotation.eulerAngles.z);
        //移動速度設定
        rb.linearVelocity = transform.forward * currentStatus.MagicSpeed;

    }

    void FixedUpdate()
    {
        //移動時の特殊処理
        Movement();
        //射程分移動で消滅
        if (Time.time - firedTime > currentStatus.MagicLength / currentStatus.MagicSpeed) magicController.EndMagic();
    }
    public void Collision(Collider other)
    {
        //地面に触れたら消滅
        if (other.gameObject.layer == layers.GroundLayer) magicController.EndMagic();

        //自分の弾が敵もしくはギミックに触れたらもしくは敵の弾が自分に触れたら
        if (((other.gameObject.layer == layers.EnemyLayer || other.gameObject.layer == layers.GimmickLayer) && !currentStatus.MagicIsEnemy) || (currentStatus.MagicIsEnemy && other.gameObject.layer == layers.PlayerLayer))
        {
            //消滅
            magicController.EndMagic();
            //接触後の特殊な判定
            Hit();
            if (other.gameObject.layer == layers.GimmickLayer) return;
        }
        
    }


    public abstract void Movement();

    public abstract void Hit();
    
    
}



