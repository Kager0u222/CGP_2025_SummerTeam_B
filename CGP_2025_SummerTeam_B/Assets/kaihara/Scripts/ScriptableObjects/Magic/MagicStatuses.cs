using UnityEngine;

[CreateAssetMenu(menuName = "Magic/MagicStatuses")]
public class MagicStatuses : ScriptableObject
{
    //各魔法別のデータを保存するクラス
    [SerializeField] public PlayerMiddleStatuses playerMiddle;
    [SerializeField] public PlayerMiddleStatuses playerShort;
    [SerializeField] public PlayerMiddleStatuses playerLong;
    [SerializeField] public EnemyNormalStatuses enemyNormal;
    [SerializeField] public EnemyShortStatuses enemyShort;
    //ImagicStatus型でデータ保存クラスを返す(ポリモーフィズム)
    public IMagicStatus GetStatus(MagicTypeAsset.MagicType type) => type switch
    {
        MagicTypeAsset.MagicType.EnemyNormal => enemyNormal,
        MagicTypeAsset.MagicType.PlayerMiddle => playerMiddle,
        MagicTypeAsset.MagicType.PlayerShort => playerShort,
        MagicTypeAsset.MagicType.PlayerLong => playerLong,
        MagicTypeAsset.MagicType.EnemyShort => enemyShort,
        _ => null
    };
}
//パラメータ返す用のインターフェース
public interface IMagicStatus
{
    float MagicDamage { get; }
    float MagicLength { get; }
    float MagicSpeed { get; }
    float MagicShake { get; }
    float MagicCoolTime { get; }
    int MagicLaunchCount { get; }
    bool MagicIsEnemy { get; }

}
[System.Serializable]
public class PlayerMiddleStatuses : IMagicStatus
{
    //火力、射程、弾速、拡散の度合い、クールタイムの変数
    [SerializeField] float magicDamage, magicLength, magicSpeed, magicShake, magicCoolTime;
    //同時発射数の変数
    [SerializeField] int magicLaunchCount;
    //敵であるか否かの変数
    [SerializeField] bool magicIsEnemy;
    //各種値の取得用プロパティ
    public float MagicDamage => magicDamage;
    public float MagicLength => magicLength;
    public float MagicSpeed => magicSpeed;
    public float MagicShake => magicShake;
    public float MagicCoolTime => magicCoolTime;
    public int MagicLaunchCount => magicLaunchCount;
    public bool MagicIsEnemy => magicIsEnemy;
}
[System.Serializable]
public class PlayerShortStatuses : IMagicStatus
{
    //火力、射程、弾速、拡散の度合い、クールタイムの変数
    [SerializeField] float magicDamage, magicLength, magicSpeed, magicShake, magicCoolTime;
    //同時発射数の変数
    [SerializeField] int magicLaunchCount;
    //敵であるか否かの変数
    [SerializeField] bool magicIsEnemy;
    //各種値の取得用プロパティ
    public float MagicDamage => magicDamage;
    public float MagicLength => magicLength;
    public float MagicSpeed => magicSpeed;
    public float MagicShake => magicShake;
    public float MagicCoolTime => magicCoolTime;
    public int MagicLaunchCount => magicLaunchCount;
    public bool MagicIsEnemy => magicIsEnemy;
}
[System.Serializable]
public class PlayerLongStatuses : IMagicStatus
{
    //火力、射程、弾速、拡散の度合い、クールタイムの変数
    [SerializeField] float magicDamage, magicLength, magicSpeed, magicShake, magicCoolTime;
    //同時発射数の変数
    [SerializeField] int magicLaunchCount;
    //敵であるか否かの変数
    [SerializeField] bool magicIsEnemy;
    //各種値の取得用プロパティ
    public float MagicDamage => magicDamage;
    public float MagicLength => magicLength;
    public float MagicSpeed => magicSpeed;
    public float MagicShake => magicShake;
    public float MagicCoolTime => magicCoolTime;
    public int MagicLaunchCount => magicLaunchCount;
    public bool MagicIsEnemy => magicIsEnemy;
}
[System.Serializable]
public class EnemyNormalStatuses : IMagicStatus
{
    //火力、射程、弾速、拡散の度合い、クールタイムの変数
    [SerializeField] float magicDamage, magicLength, magicSpeed, magicShake, magicCoolTime;
    //同時発射数の変数
    [SerializeField] int magicLaunchCount;
    //敵であるか否かの変数
    [SerializeField] bool magicIsEnemy;
    //各種値の取得用プロパティ
    public float MagicDamage => magicDamage;
    public float MagicLength => magicLength;
    public float MagicSpeed => magicSpeed;
    public float MagicShake => magicShake;
    public float MagicCoolTime => magicCoolTime;
    public int MagicLaunchCount => magicLaunchCount;
    public bool MagicIsEnemy => magicIsEnemy;
}
[System.Serializable]
public class EnemyShortStatuses : IMagicStatus
{
    //火力、射程、弾速、拡散の度合い、クールタイムの変数
    [SerializeField] float magicDamage, magicLength, magicSpeed, magicShake, magicCoolTime;
    //同時発射数の変数
    [SerializeField] int magicLaunchCount;
    //敵であるか否かの変数
    [SerializeField] bool magicIsEnemy;
    //各種値の取得用プロパティ
    public float MagicDamage => magicDamage;
    public float MagicLength => magicLength;
    public float MagicSpeed => magicSpeed;
    public float MagicShake => magicShake;
    public float MagicCoolTime => magicCoolTime;
    public int MagicLaunchCount => magicLaunchCount;
    public bool MagicIsEnemy => magicIsEnemy;
}