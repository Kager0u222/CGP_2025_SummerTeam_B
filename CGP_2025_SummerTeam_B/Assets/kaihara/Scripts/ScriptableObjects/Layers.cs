using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Layers")]
public class Layers : ScriptableObject
{
    [SerializeField] private int groundLayer;
    [SerializeField] private int gimmickLayer;
    [SerializeField] private int enemyLayer;
    [SerializeField] private int playerLayer;
    public int GroundLayer { get{ return groundLayer; } } 
    public int GimmickLayer{ get{ return gimmickLayer; } }
    public int EnemyLayer{ get{ return enemyLayer; } }
    public int PlayerLayer{ get{ return playerLayer; } }
}