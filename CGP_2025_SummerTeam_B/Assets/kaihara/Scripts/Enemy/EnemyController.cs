using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //魔法発射用クラス
    [SerializeField] private EnemyShooting enemyShooting;
    //体力処理
    [SerializeField] private EnemyHpController enemyHpController;
    //個別挙動用クラス
    [SerializeField] private EnemyMove enemyMove;
    //レイヤーのScriptableObject
    [SerializeField] private Layers layers;
    //見た目のやつ
    [SerializeField] private GameObject model;
    //プレイヤーのオブジェクト(BaseControllerから間接的に設定)
    private GameObject playerObject;
    //オブジェクトプールのスクリプト(同上)
    private MagicPool magicPool;
    //全体の処理をするクラス
    private GameMasterController gameMaster;
    //魔法の射程
    private float magicLength;

    //自分を敵のリストに登録
    public static List<EnemyController> enemys = new List<EnemyController>();

    void Awake()
    {
        enemys.Add(this);
        magicLength = enemyShooting.MagicLength;
    }
    void OnDestroy()
    {
        enemys.Remove(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //発射処理
        enemyShooting.Fire(playerObject, magicPool);
        //クールタイム減少
        enemyShooting.LifeTimeDecreaser(Time.fixedDeltaTime * gameMaster.PhysicsSpeed);
        //モデル動かす（プレイヤー方向へ滑らかに回転）
        if((playerObject.transform.position - model.transform.position).sqrMagnitude < Mathf.Pow(magicLength, 2))
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerObject.transform.position - model.transform.position, Vector3.up);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, 0.2f);
        }
        //個別の挙動
        if (enemyMove != null)
            enemyMove.Move(layers, playerObject, magicLength, gameMaster);
    }
    public void PlayerSetter(GameObject player)
    {
        playerObject = player;
        enemyHpController.PlayerAudioSetter(player.GetComponentInChildren<PlayerAudio>());
        if(enemyMove != null)
        enemyMove.SetPlayer(player);
    }
    public void MagicPoolSetter(MagicPool pool)
    {
        magicPool = pool;
    }
    public void GameMasterSetter(GameMasterController master)
    {
        gameMaster = master;
    }

}
