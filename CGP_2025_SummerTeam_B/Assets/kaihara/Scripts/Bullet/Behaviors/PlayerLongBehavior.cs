using UnityEngine;

public class PlayerLongBehavior : BulletBehavior
{
    //火力
    public override float bulletDamage { get; } = 30;
    //弾速
    public override float bulletSpeed { get; } = 200;
    //射程
    public override float bulletLength { get; } = 100;
    //ブレ
    public override float bulletShake { get; } = 0;
    //敵か否か
    public override bool bulletIsEnemy { get; } = false;
    //クールタイム
    public override float bulletCoolTime { get; } = 1f;
    public override void Status()
    {
        
    }
    public override void Movement()
    {

    }

    public override void Hit()
    {

    }

}