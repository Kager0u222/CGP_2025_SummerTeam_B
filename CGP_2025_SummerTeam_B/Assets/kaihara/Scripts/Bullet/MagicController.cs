using System.Xml.Serialization;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Pool;

public class MagicController : MonoBehaviour
{
    //レイヤーのScriptableObject
    [SerializeField] private Layers layers;
    //マテリアルの配列
    [SerializeField] private Material[] magicMaterials;
    
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
    //Startだとpoolの方で非アクティブ化するため動かないのでAwakeにしている
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        magicMaterial = GetComponent<Renderer>();
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
        behavior.Launch(this,rb,layers,type,magicStatuses);

    }
    //壁衝突などで呼び出し
    public void EndMagic()
    {
        //behaviorインスタンスの削除
        Destroy(behavior);
        //弾の返還
        magicPool.ReturnMagic(this);
    }
    //弾の挙動決定
    private void SetMagicType(MagicTypeAsset.MagicType type)
    {
        //弾にすでに弾の挙動のインスタンスがあったら消す
        if (behavior != null) Destroy(behavior);
        //弾の種類に応じて分岐
        switch (type)
        {
            //プレイヤーの通常の弾
            case MagicTypeAsset.MagicType.PlayerMiddle:
                behavior = gameObject.AddComponent<PlayerMiddleBehavior>();
                magicMaterial.material = magicMaterials[0];
                break;
            //プレイヤーの近距離用の弾
            case MagicTypeAsset.MagicType.PlayerShort:
                behavior = gameObject.AddComponent<PlayerShortBehavior>();
                magicMaterial.material = magicMaterials[1];
                break;
            //プレイヤーの遠距離用の弾
            case MagicTypeAsset.MagicType.PlayerLong:
                behavior = gameObject.AddComponent<PlayerLongBehavior>();
                magicMaterial.material = magicMaterials[2];
                break;
            //敵の普通の弾
            case MagicTypeAsset.MagicType.EnemyNormal:
                behavior = gameObject.AddComponent<EnemyNormalBehavior>();
                magicMaterial.material = magicMaterials[3];
                break;
        }
    }
}