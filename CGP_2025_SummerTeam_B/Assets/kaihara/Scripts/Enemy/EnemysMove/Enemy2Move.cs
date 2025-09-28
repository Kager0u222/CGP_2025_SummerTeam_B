using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy2Move : EnemyMove
{
    //移動速度
    [SerializeField] private float moveSpeed;
    //近づく距離
    [SerializeField] private float reachDistance;
    //近づき始める距離
    [SerializeField] private float movingDistance;
    //移動処理のaddforceの倍率
    [SerializeField] private float moveAddforcePower;
    //モデル
    [SerializeField] private GameObject model;

    public override void Move(Layers layers, GameObject player, float magicLength, GameMasterController master)
    {
        //プレイヤーとの距離の二乗を取得
        float sqrDistance = Vector3.SqrMagnitude(player.transform.position - transform.position);
        //プレイヤーの方向取得
        Vector3 direction = (player.transform.position - transform.position).normalized;

        //一定距離まで近づく
        if (sqrDistance > Mathf.Pow(reachDistance, 2) && sqrDistance < Mathf.Pow(movingDistance, 2))
        {
            rb.AddForce(new Vector3(direction.x * moveSpeed - rb.linearVelocity.x, direction.y * moveSpeed - rb.linearVelocity.y, direction.z * moveSpeed - rb.linearVelocity.z) * moveAddforcePower);
            Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - model.transform.position, Vector3.up);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, 0.2f);
        }

        //減速
        else
            rb.AddForce(new Vector3(-rb.linearVelocity.x, -rb.linearVelocity.y, -rb.linearVelocity.z) * moveAddforcePower);
    }
}