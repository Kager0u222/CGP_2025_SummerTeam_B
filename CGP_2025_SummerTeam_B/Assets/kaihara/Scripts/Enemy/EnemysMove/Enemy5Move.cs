using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy5Move : EnemyMove
{
    //回転速度
    [SerializeField,Range(0,1)] private float rotationSpeed;
    //トリガーのクラス
    [SerializeField] private GimmickTrigger trigger;
    //盾の挙動用のオブジェクト
    [SerializeField] private Transform shieldCore;

    public override void Move(Layers layers, GameObject player, float magicLength, GameMasterController master)
    {
        //盾の根本の回転
        if (shieldCore != null)
            shieldCore.rotation = Quaternion.LookRotation(Vector3.Slerp(shieldCore.forward, player.transform.position - shieldCore.position, rotationSpeed * master.PhysicsSpeed), Vector3.up);
        if (trigger.IsUnlocked) shieldCore.localScale -= new Vector3(0.01f, 0.01f, 0) * master.PhysicsSpeed;
        if (shieldCore.localScale.x <= 0) shieldCore.gameObject.SetActive(false);
    }
}