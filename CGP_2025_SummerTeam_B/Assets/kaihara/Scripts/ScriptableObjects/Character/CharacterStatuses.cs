using UnityEngine;

[CreateAssetMenu(menuName = "Character/CharacterStatus")]
public class CharacterStatuses : ScriptableObject
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Enemy1Status enemy1Status;
    [SerializeField] private Enemy2Status enemy2Status;
    public ICharacterStatuses GetCharactorStatuses(CharacterTypeAsset.CharacterType type) => type switch
    {
        CharacterTypeAsset.CharacterType.Player => playerStatus,
        CharacterTypeAsset.CharacterType.Enemy1 => enemy1Status,
        CharacterTypeAsset.CharacterType.Enemy2 => enemy2Status,
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
public class Enemy1Status : ICharacterStatuses
{
    [SerializeField] private float hpmax;
    public float HpMax => hpmax;
}
[System.Serializable]
public class Enemy2Status : ICharacterStatuses
{
    [SerializeField] private float hpmax;
    public float HpMax => hpmax;
}