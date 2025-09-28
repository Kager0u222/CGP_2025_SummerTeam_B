using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    //弾の発射位置用オブジェクトの位置取得
    [SerializeField] private Transform magicFirePosition;
    //弾のオブジェクトプール取得
    [SerializeField] private MagicPool magicPool;
    //弾のステータスのSprictableObject
    [SerializeField] private MagicStatuses magicStatuses;
    //魔法表示UIのクラス
    [SerializeField] private MagicUIController magicUIController;
    //レイヤー
    [SerializeField] private Layers layers;
    //弾の発射時の効果音
    [SerializeField] private AudioClip shootSE;
    //魔法切り替え時の効果音
    [SerializeField] private AudioClip changeSE;
    //プレイヤーの音流すやつ
    [SerializeField] private PlayerAudio playerAudio;
    //弾のステータス確認に使う変数
    private IMagicStatus magicStatus;
    //魔法切り替えSEのクールタイム
    private const float SECoolTime = 0.1f;
    //前回魔法切り替えSEを鳴らしたとき保存
    private float lastSETime = 0;
    //残りクールタイム変数
    private float coolTime;
    //↑のゲッター
    public float CoolTime {get{ return coolTime; }}
    //魔法のタイプ
    private int magicType = 0;
    //魔法のクールタイムのゲッター
    public float MagicCoolTIme {get { return magicStatus.MagicCoolTime; }}
    //レイヤーマスク
    private LayerMask layerMask;

    void Awake()
    {
        //魔法のタイプを初期化
        magicStatus = magicStatuses.GetStatus(MagicTypeAsset.MagicType.PlayerMiddle);
        //このクラスをUIのクラスに渡す
        magicUIController.Shooting(this);
        //レイヤーマスクの設定
        layerMask = 1 << layers.GroundLayer;
        layerMask += 1 << layers.EnemyLayer;
        layerMask += 1 << layers.BarrierLayer;
    }

    public void Shoot(Transform cameraTransform, Transform cameraBaseTransform)
    {

        //前回射撃時から十分時間がたっていなければ終了
        if (coolTime > 0) return;

        //レイの起点と向きの指定
        Ray ray = new Ray(cameraTransform.position, cameraBaseTransform.forward);
        //射出先情報の保存用変数
        RaycastHit hit;

        //弾射出先の情報の保存
        bool isObjectAtAim = Physics.Raycast(ray, out hit, magicStatus.MagicLength, layerMask);
        //射程内にオブジェクトなしなら射程ギリギリにオブジェクトがあるときと同じにする
        if (!isObjectAtAim) hit.point = cameraTransform.position + cameraTransform.forward * magicStatus.MagicLength;
        //弾召喚
        for (int i = 0; i < magicStatus.MagicLaunchCount; i++)
            magicPool.BorrowMagic(magicFirePosition.position, Quaternion.LookRotation((hit.point - magicFirePosition.position).normalized, Vector3.up), (MagicTypeAsset.MagicType)magicType);

        //発射した時間を保存
        coolTime = magicStatus.MagicCoolTime;
        //射撃音
        playerAudio.PlaySE(shootSE);
    }
    public void ChangeMagic(float input)
    {
        //マウスホイールで魔法の番号を変更
        magicType -= Mathf.FloorToInt(input / Mathf.Abs(input));
        //ループ
        if (magicType >= 3) magicType = 0;
        if (magicType <= -1) magicType = 2;
        //enum用意
        MagicTypeAsset.MagicType type;
        //番号をenumに変換
        type = (MagicTypeAsset.MagicType)magicType;
        //enumに応じてステータスを取得
        magicStatus = magicStatuses.GetStatus(type);
        //UI切り替え
        magicUIController.ChangeMagic(Mathf.FloorToInt(input / Mathf.Abs(input)));
        //SEのクールタイムが十分でなければ終了
        if (Time.time - lastSETime < SECoolTime) return;
        //魔法切り替え音
        playerAudio.PlaySE(changeSE);
        //魔法切り替えした時間を保存
        lastSETime = Time.time;
    }
    public void LifeTimeDecreaser(float value)
    {
        coolTime -= value;
    }
}
