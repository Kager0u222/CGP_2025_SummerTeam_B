using UnityEngine;

[CreateAssetMenu(menuName = "Magic/MagicType")]
public class MagicTypeAsset : ScriptableObject
{
    public enum MagicType
    {
        PlayerMiddle,
        PlayerShort,
        PlayerLong,
        EnemyNormal,
        EnemyShort,
        EnemyRapid
    } 
}