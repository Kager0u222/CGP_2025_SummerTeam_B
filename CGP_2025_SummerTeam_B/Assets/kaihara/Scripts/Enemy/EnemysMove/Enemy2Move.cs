using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy2Move : EnemyMove
{
    //移動速度
    [SerializeField] private float moveSpeed;
    //近づく距離
    [SerializeField] private float reachDistance;
    //移動処理のaddforceの倍率
    [SerializeField] private float moveAddforcePower;

    public override void Move(Layers layers, GameObject player, float magicLength,GameMasterController master)
    {
        //プレイヤーとの距離の二乗を取得
        float sqrDistance = Vector3.SqrMagnitude(player.transform.position - transform.position);
        //プレイヤーの方向取得
        Vector3 direction = (player.transform.position - transform.position).normalized;

        //一定距離まで近づく
        if (sqrDistance > Mathf.Pow(reachDistance, 2))
            rb.AddForce(new Vector3(direction.x * moveSpeed - rb.linearVelocity.x, direction.y * moveSpeed - rb.linearVelocity.y, direction.z * moveSpeed - rb.linearVelocity.z) * moveAddforcePower);
    }
}