using UnityEngine;

public class PlayerShortBehavior : BulletBehavior
{
    //火力
    public override float bulletDamage { get; } = 1;
    //弾速
    public override float bulletSpeed { get; } = 50;
    //射程
    public override float bulletLength { get; } = 50;
    //ブレ
    public override float bulletShake { get; } = 10;
    //敵か否か
    public override bool bulletIsEnemy { get; } = false;
    //クールタイム
    public override float bulletCoolTime { get; } = 0.5f;
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