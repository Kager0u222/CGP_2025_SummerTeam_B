
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class MagicBehavior : MonoBehaviour
{

    //rigidbody
    private Rigidbody rb;
    //レイヤーのScriptableObject
    private Layers layers;

    //弾のステータス確認に使う変数
    private IMagicStatus currentStatus;
    //弾の寿命
    private float lifeTime;
    //オブジェクトプールとのやり取りをする奴
    private MagicController magicController;
    //ステータスのScriptableObjct
    private MagicStatuses currentStatuses;
    //体力処理のクラス
    private HpController hpController;
    //ゲーム全体の処理をするクラス
    private GameMasterController gameMasterController;
    public void Launch(MagicController magic, Rigidbody rigidbody, Layers layer, MagicTypeAsset.MagicType type, MagicStatuses statuses, GameMasterController gameMaster)
    {
        //rigidbody
        rb = rigidbody;
        //ステータスのScriptableObject
        currentStatuses = statuses;
        //レイヤーのSCriptableObject
        layers = layer;
        //magiccontrollerのインスタンス取得
        magicController = magic;
        //ゲーム全体を処理するクラスを取得
        gameMasterController = gameMaster;
        //typeから取得すべきステータスのクラスを取得
        currentStatus = currentStatuses.GetStatus(type);
        //弾の寿命をリセット
        lifeTime = currentStatus.MagicLength / currentStatus.MagicSpeed;
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
        //衝突処理用にSpherecast
        RaycastHit hit;
        LayerMask layerMask = 1 << layers.EnemyLayer;
        layerMask += 1 << layers.PlayerLayer;
        layerMask += 1 << layers.GroundLayer;
        layerMask += 1 << layers.GimmickLayer;
        //衝突で衝突処理
        bool magicHit = Physics.SphereCast(transform.position - transform.forward * transform.localScale.z / 2, transform.localScale.x, transform.forward, out hit, currentStatus.MagicSpeed * Time.fixedDeltaTime + transform.localScale.z / 2, layerMask);
        if(magicHit)    Collision(hit.collider.gameObject);
        //寿命を減少(物理挙動の加減速も加味)
        lifeTime -= Time.fixedDeltaTime * gameMasterController.PhysicsSpeed;
        //射程分移動で消滅
        if (lifeTime < 0) magicController.EndMagic();
    }
    void OnTriggerStay(Collider other)
    {
        Collision(other.gameObject);
    }
    public void Collision(GameObject hit)
    {
        //地面に触れたら消滅
        if (hit.layer == layers.GroundLayer) magicController.EndMagic();

        //自分の弾が敵もしくはギミックに触れたらもしくは敵の弾が自分に触れたら
        if (((hit.layer == layers.EnemyLayer || hit.layer == layers.GimmickLayer) && !currentStatus.MagicIsEnemy) || (currentStatus.MagicIsEnemy && hit.layer == layers.PlayerLayer))
        {
            //消滅
            magicController.EndMagic();
            //接触後の特殊な判定
            Hit();
            hpController = hit.gameObject.GetComponent<HpController>();
            if (hpController != null) hpController.AddDamage(currentStatus.MagicDamage);
        }
    }


    public abstract void Movement();

    public abstract void Hit();
    
    
}



