using UnityEngine;

[CreateAssetMenu(menuName = "Character/Charactertype")]
public class CharacterTypeAsset : ScriptableObject
{
    public enum CharacterType
    {
        Player,
        Enemy1,
        Enemy2,
        Enemy3,
        Enemy4,
        Enemy5 
    }
}