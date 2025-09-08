using UnityEngine;

public class PlayerMiddleBehavior : BulletBehavior
{
    //火力
    public override float bulletDamage { get; } = 5;
    //弾速
    public override float bulletSpeed { get; } = 50;
    //射程
    public override float bulletLength { get; } = 100;
    //ブレ
    public override float bulletShake { get; } = 0.2f;
    //敵か否か
    public override bool bulletIsEnemy { get; } = false;
    //クールタイム
    public override float bulletCoolTime { get; } = 0.1f;
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