using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Layers")]
public class Layers : ScriptableObject
{
    //各種レイヤーの番号指定
    [SerializeField] private int groundLayer;
    [SerializeField] private int gimmickLayer;
    [SerializeField] private int enemyLayer;
    [SerializeField] private int playerLayer;
    [SerializeField] private int barrierLayer;
    //レイヤー番号取得用プロパティ
    public int GroundLayer { get { return groundLayer; } }
    public int GimmickLayer { get { return gimmickLayer; } }
    public int EnemyLayer { get { return enemyLayer; } }
    public int PlayerLayer { get { return playerLayer; } }
    public int BarrierLayer { get { return barrierLayer; } }
}