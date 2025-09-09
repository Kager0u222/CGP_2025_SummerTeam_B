using UnityEngine;

public class EnemyHpController : HpController
{
    public override void OnDeath()
    {
        Destroy(gameObject);
    }
}