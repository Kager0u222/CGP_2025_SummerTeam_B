using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework.Constraints;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Pool;

public class MagicController : MonoBehaviour
{
    //レイヤーのScriptableObject
    [SerializeField] private Layers layers;
    //マテリアルの配列
    [SerializeField] private Material[] magicMaterials;
    //パーティクルのマテリアル
    [SerializeField] private Material[] particleMaterials;
    //パーティクルのカラー
    [SerializeField] private Gradient[] particleColor;
    //子のpartile system
    private ParticleSystem particle;
    //子のparticle systemのrenderer
    private ParticleSystemRenderer particleRenderer;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifetime;
    //子のtrail
    private ParticleSystem.TrailModule trail;
    
    //rigidbody
    private Rigidbody rb;
    //マテリアル用の変数
    private Renderer magicMaterial;

    //オブジェクトプールのクラス
    private MagicPool magicPool;
    //弾の挙動のインスタンスを保存する用の変数
    private MagicBehavior behavior;
    //ステータスのScriptableObjct
    [SerializeField] private MagicStatuses magicStatuses;

    //ゲーム全体の処理をするクラス
    private GameMasterController gameMasterController;
    //弾のリスト
    public static List<MagicController> magics = new List<MagicController>();
    //Startだとpoolの方で非アクティブ化するため動かないのでAwakeにしている
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        magicMaterial = GetComponent<Renderer>();
        magics.Add(this);
        particle = GetComponentInChildren<ParticleSystem>();
        particleRenderer = GetComponentInChildren<ParticleSystemRenderer>();
        colorOverLifetime = particle.colorOverLifetime;
        trail = particle.trails;
    }

    //弾発射時に呼び出し
    public void LaunchMagic(Vector3 launchPosition, Quaternion launchRotation, MagicTypeAsset.MagicType type, MagicPool pool)
    {
        //オブジェクトプールのインスタンスを保存
        magicPool = pool;
        //発射位置に移動
        transform.position = launchPosition;
        //回転
        transform.rotation = launchRotation;
        //受け取った弾の種類から弾の挙動を決定
        SetMagicType(type);
        //ステータスや見た目などの初期設定
        behavior.Launch(this, rb, layers, type, magicStatuses, gameMasterController, particle, particleRenderer);
    }

    //壁衝突などで呼び出し
    public void EndMagic()
    {
        //behaviorインスタンスの削除
        Destroy(behavior);
        behavior = null;
        //弾の返還
        magicPool.ReturnMagic(this);
    }
    
    //弾の挙動決定
    private void SetMagicType(MagicTypeAsset.MagicType type)
    {
        //弾にすでに弾の挙動のインスタンスがあったら消す
        if (behavior != null)
        {
            Destroy(behavior);
            behavior = null;
        }
        //弾の種類に応じて分岐
        switch (type)
        {
            //プレイヤーの通常の弾
            case MagicTypeAsset.MagicType.PlayerMiddle:
                behavior = gameObject.AddComponent<PlayerMiddleBehavior>();
                magicMaterial.material = magicMaterials[0];
                particleRenderer.material = particleMaterials[0];
                colorOverLifetime.color = particleColor[0];
                particleRenderer.renderMode = ParticleSystemRenderMode.Billboard;
                break;
            //プレイヤーの近距離用の弾
            case MagicTypeAsset.MagicType.PlayerShort:
                behavior = gameObject.AddComponent<PlayerShortBehavior>();
                magicMaterial.material = magicMaterials[1];
                particleRenderer.renderMode = ParticleSystemRenderMode.Billboard;
                break;
            //プレイヤーの遠距離用の弾
            case MagicTypeAsset.MagicType.PlayerLong:
                behavior = gameObject.AddComponent<PlayerLongBehavior>();
                magicMaterial.material = magicMaterials[0];
                particleRenderer.renderMode = ParticleSystemRenderMode.None;
                colorOverLifetime.color = particleColor[2];
                trail.enabled = true;
                trail.colorOverLifetime = particleColor[2];
                break;
            //敵の普通の弾
            case MagicTypeAsset.MagicType.EnemyNormal:
                behavior = gameObject.AddComponent<EnemyNormalBehavior>();
                magicMaterial.material = magicMaterials[3];
                particleRenderer.material = particleMaterials[2];
                colorOverLifetime.color = particleColor[1];
                particleRenderer.renderMode = ParticleSystemRenderMode.Billboard;
                break;
            //敵の短射程の弾
            case MagicTypeAsset.MagicType.EnemyShort:
                behavior = gameObject.AddComponent<EnemyShortBehavior>();
                magicMaterial.material = magicMaterials[3];
                particleRenderer.material = particleMaterials[2];
                colorOverLifetime.color = particleColor[1];
                particleRenderer.renderMode = ParticleSystemRenderMode.Billboard;
                break;
        }
    }
    public void GameMasterSetter(GameMasterController value)
    {
        gameMasterController = value;
    }
}