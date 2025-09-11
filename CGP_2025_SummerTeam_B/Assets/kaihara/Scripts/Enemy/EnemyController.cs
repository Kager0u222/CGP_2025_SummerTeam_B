using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //魔法発射用クラス
    [SerializeField] private EnemyShooting enemyShooting;
    //プレイヤーのオブジェクト(BaseControllerから間接的に設定)
    private GameObject playerObject;
    //オブジェクトプールのスクリプト(同上)
    private MagicPool magicPool;

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
        enemyShooting.Fire(playerObject, magicPool);
    }
    public void PlayerSetter(GameObject player)
    {
        playerObject = player;
    }
    public void MagicPoolSetter(MagicPool pool)
    {
        magicPool = pool;
    }
}
