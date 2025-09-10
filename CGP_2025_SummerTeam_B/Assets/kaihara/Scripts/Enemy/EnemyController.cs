using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //魔法発射用クラス
    [SerializeField] private EnemyShooting enemyShooting;


    //自分を敵のリストに登録
    public static List<EnemyController> enemys = new List<EnemyController>();

    void Awake()
    {
        enemys.Add(this);
    }
    void OnDestroy()
    {
        enemys.Remove(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enemyShooting.Fire(Player, magicPool);
    }
    

    //プレイヤーのオブジェクト(BaseControllerから間接的に設定)
    public GameObject Player { get; set; }
    //オブジェクトプールのスクリプト(同上)
    public MagicPool magicPool { get; set; }
}
