using UnityEngine;

public class EnemyHpController : HpController
{   
    //敵の被弾時の音
    [SerializeField] private AudioClip damageSE;
    //敵の撃破時の音
    [SerializeField] private AudioClip deathSE;
    //敵の音流すやつ
    [SerializeField] private EnemyAudio enemyAudio;
    //プレイヤーの音鳴らすやつ
    private PlayerAudio playerAudio;
    //上のやつのセッター
    public void PlayerAudioSetter(PlayerAudio audio)
    {
        playerAudio = audio;
    }
    public override void OnDeath()
    {
        if (playerAudio != null)
        {
            playerAudio.PlaySE(deathSE);
            playerAudio = null;
        }
        Destroy(gameObject);
    }
    public override void OnDamage()
    {
        if(enemyAudio != null && Hp > 0)
            enemyAudio.PlaySE(damageSE);
    }
}