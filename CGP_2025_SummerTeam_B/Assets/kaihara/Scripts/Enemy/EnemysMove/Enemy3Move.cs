using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy3Move : EnemyMove
{
    //回転速度
    [SerializeField,Range(0,1)] private float rotationSpeed;
    //盾の挙動用のオブジェクト
    [SerializeField] private Transform shieldCore;

    public override void Move(Layers layers, GameObject player, float magicLength,GameMasterController master)
    {
        //盾の根本の回転
        shieldCore.rotation = Quaternion.LookRotation(Vector3.Slerp(shieldCore.forward, player.transform.position - shieldCore.position, rotationSpeed * master.PhysicsSpeed), Vector3.up);
    }
}