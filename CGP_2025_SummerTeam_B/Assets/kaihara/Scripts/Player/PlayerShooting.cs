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
    //弾のステータス確認に使う変数
    private IMagicStatus magicStatus;

    //前回射撃時の時間保存用変数
    private float lifeTime;
    //魔法のタイプ
    private int magicType = 2;

    void Awake()
    {
        //魔法のタイプを初期化
        ChangeMagic(1);
    }

    public void Shoot(int groundLayer, int enemyLayer, Transform cameraTransform, Transform cameraBaseTransform)
    {

        //前回射撃時から十分時間がたっていなければ終了
        if (lifeTime > 0) return;

        //レイヤーマスクの設定
        LayerMask layerMask = 1 << groundLayer;
        layerMask += 1 << enemyLayer;

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
        lifeTime = magicStatus.MagicCoolTime;
    }
    public void ChangeMagic(float input)
    {
        //マウスホイールで魔法の番号を変更
        magicType += Mathf.FloorToInt(input/Mathf.Abs(input));
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
        magicUIController.ChangeMagic(Mathf.FloorToInt(input/Mathf.Abs(input)));
    }
    public void LifeTimeDecreaser(float value)
    {
        lifeTime -= value;
    }
}
