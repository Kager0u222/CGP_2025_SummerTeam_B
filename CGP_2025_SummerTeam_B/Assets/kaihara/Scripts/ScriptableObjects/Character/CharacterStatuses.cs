using UnityEngine;

[CreateAssetMenu(menuName = "Character/CharacterStatus")]
public class CharacterStatuses : ScriptableObject
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private EnemyStatus enemyStatus;
    public ICharacterStatuses GetCharactorStatuses(CharacterTypeAsset.CharacterType type) => type switch
    {
        CharacterTypeAsset.CharacterType.Player => playerStatus,
        CharacterTypeAsset.CharacterType.Enemy1 => enemyStatus,
        _ => null
    };
}
public interface ICharacterStatuses
    {
        float HpMax { get; }
    }
[System.Serializable]
public class PlayerStatus : ICharacterStatuses
{
    [SerializeField] private float hpmax;
    public float HpMax => hpmax;
}
[System.Serializable]
public class EnemyStatus : ICharacterStatuses
{
    [SerializeField] private float hpmax;
    public float HpMax => hpmax;
}