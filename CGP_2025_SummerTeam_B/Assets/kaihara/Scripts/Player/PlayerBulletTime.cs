using UnityEngine;

public class PlayerBulletTime : MonoBehaviour
{
    //バレットタイムUIのクラス
    [SerializeField] private BulletTimeUIController bulletTimeUIController;
    //バレットタイム時の物理挙動の時間の経過速度
    [SerializeField, Range(0f, 3f)] private float bulletTimeSpeed;
    //バレットタイムの最大継続可能時間
    [SerializeField] private float bulletTimeLimitSeconds;
    //バレットタイムの使用可能時間の回復までの時間
    [SerializeField] private float bulletTimeRechargeSeconds;
    //ゲームの全体を処理するクラス
    private GameMasterController gameMasterController;
    //バレットタイムのチャージ
    private float bulletTimeCharge;
    //バレットタイム中か否か
    private bool isBulletTime = false;
    private void Start()
    {
        bulletTimeCharge = bulletTimeLimitSeconds;
    }
    private void FixedUpdate()
    {
        //バレットタイム中ならチャージを減らし見た目更新
        if (isBulletTime)
        {
            bulletTimeCharge -= Time.fixedDeltaTime;
            bulletTimeUIController.ChangeGauge(bulletTimeCharge, bulletTimeLimitSeconds, false);
            //チャージがゼロになったらバレットタイム中断
            if (bulletTimeCharge <= 0)
            {
                gameMasterController.PhysicsSpeedSetter(1);
                isBulletTime = false;
            }
        }
        //バレットタイム中でなくチャージが回復しきってなかったらチャージを増やし見た目更新
        if (!isBulletTime && bulletTimeCharge < bulletTimeLimitSeconds)
        {
            bulletTimeCharge += Time.fixedDeltaTime * bulletTimeLimitSeconds / bulletTimeRechargeSeconds;
            bulletTimeUIController.ChangeGauge(bulletTimeCharge, bulletTimeLimitSeconds, true);
        }
    }
    public void BulletTimeStart()
    {
        //チャージがマックスでなければ中断
        if (bulletTimeCharge < bulletTimeLimitSeconds) return;
        //時間減速
        gameMasterController.PhysicsSpeedSetter(bulletTimeSpeed);
        //バレットタイム中であることを保存
        isBulletTime = true;
    }
    public void BulletTimeCancel()
    {
        //すでにバレットタイム中でないなら中断
        if (!isBulletTime) return;
        //時間を戻す
        gameMasterController.PhysicsSpeedSetter(1);
        //バレットタイム中でないことを保存
        isBulletTime = false;
    }
    public void gameMasterSetter(GameMasterController master)
    {
        gameMasterController = master;
    }
}