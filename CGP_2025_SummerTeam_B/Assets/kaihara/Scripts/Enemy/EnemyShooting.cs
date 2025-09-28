using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    //レイヤーのScriptableObject
    [SerializeField] Layers layers;
    //弾の種類
    [SerializeField] private MagicTypeAsset.MagicType magicType;
    //弾のステータスのSprictableObject
    [SerializeField] private MagicStatuses magicStatuses;

    //弾のステータス確認に使う変数
    private IMagicStatus magicStatus;
    //射撃時の時刻保存用
    private float lifeTime = 0;
    private void Awake()
    {
        magicStatus = magicStatuses.GetStatus(magicType);
    }
    public void Fire(GameObject Player, MagicPool magicPool)
    {
        //前回射撃時から十分時間がたっていなければ終了
        if (lifeTime > 0) return;

        //レイヤーマスクの設定
        LayerMask layerMask = 1 << layers.PlayerLayer;
        layerMask += 1 << layers.GroundLayer;
        layerMask += 1 << layers.BarrierLayer;

        //レイの起点と向きの指定
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Player.transform.position - transform.position + Vector3.up * 0.5f);
        //射出先情報の保存用変数
        RaycastHit hit;

        //弾射出先の情報の保存
        bool isObjectAtAim = Physics.Raycast(ray, out hit, magicStatus.MagicLength, layerMask);
        //射程内にオブジェクトなしもしくは地面と接触したら終了
        if (!isObjectAtAim || hit.collider.gameObject.layer == layers.GroundLayer) return;

        //弾召喚
        for (int i = 0; i < magicStatus.MagicLaunchCount; i++)
            magicPool.BorrowMagic(transform.position + Vector3.up * 0.5f, Quaternion.LookRotation((hit.point - (transform.position + Vector3.up * 0.5f)).normalized, Vector3.up), magicType);

        //発射した時間を保存
        lifeTime = magicStatus.MagicCoolTime;
    }
    public void LifeTimeDecreaser(float value)
    {
        lifeTime -= value;
    }
    public float MagicLength {get { return magicStatus.MagicLength; }}
}