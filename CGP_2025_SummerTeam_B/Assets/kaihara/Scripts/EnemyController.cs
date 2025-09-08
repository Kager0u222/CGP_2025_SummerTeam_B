using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //プレイヤーのレイヤー番号
    [SerializeField]
    private int playerLayer;
    //地面のレイヤー番号
    [SerializeField]
    private int groundLayer;
    //射撃のクールタイム
    [SerializeField]
    private float fireCoolTime;
    //プレイヤーオブジェクト
    private GameObject Player;
    //射撃の射程
    [SerializeField]
    private float bulletLength;
    //弾のprefab
    [SerializeField]
    private GameObject enemyBulletPrefab;
    //射撃時の時刻保存用
    private float lastFiredTime = 0;


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
    void Update()
    {
        Fire();
    }
    private void Fire()
    {
        //射撃タイミングのブレ
        float rmd = Random.Range(-0.1f, 0.1f);
        //前回射撃時から十分時間がたっていなければ終了
        if (Time.time - lastFiredTime < fireCoolTime*(1+rmd)) return;

        //レイヤーマスクの設定
        LayerMask layerMask = 1 << playerLayer;
        layerMask += 1 << groundLayer;

        //レイの起点と向きの指定
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Player.transform.position - transform.position + Vector3.up * 0.5f);
        //射出先情報の保存用変数
        RaycastHit hit;

        //弾射出先の情報の保存
        bool isObjectAtAim = Physics.Raycast(ray, out hit, bulletLength, layerMask);
        //射程内にオブジェクトなしもしくは地面と接触したら終了
        if (!isObjectAtAim) return;

        //弾召喚
        GameObject bullet = Instantiate(enemyBulletPrefab, transform.position + Vector3.up * 0.5f, Quaternion.LookRotation((hit.point - (transform.position + Vector3.up * 0.5f)).normalized, Vector3.up));
        
        //発射した時間を保存
        lastFiredTime = Time.time;
    }
    
    public void PlayerSetter(GameObject player)
    {
        Player = player;
    }
}
