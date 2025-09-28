
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerBulletTime : MonoBehaviour
{
    //バレットタイムUIのクラス
    [SerializeField] private BulletTimeUIController bulletTimeUIController;
    //ポストプロセスのvolume
    [SerializeField] private Volume volume;
    //バレットタイム時の物理挙動の時間の経過速度
    [SerializeField, Range(0f, 3f)] private float bulletTimeSpeed;
    //バレットタイムの最大継続可能時間
    [SerializeField] private float bulletTimeLimitSeconds;
    //バレットタイムの使用可能時間の回復までの時間
    [SerializeField] private float bulletTimeRechargeSeconds;
    //ポストプロセスの変更速度
    [SerializeField, Range(0,1)] private float postProcessChangeSpeed;
    //vignette.intensity
    [SerializeField, Range(0,1)] private float vignetteIntensity;
    //vignette.smoothness
    [SerializeField, Range(0,1)] private float vignetteSmoothness;
    //chromaticAberration.intensity
    [SerializeField, Range(0, 1)] private float chromaticAberrationIntensity;
    //colorAdjustments.postExposure
    [SerializeField] private float colorAdjustmentsPostExposure;
    //colorAdjustments.contrast
    [SerializeField, Range(0, 1)] private float colorAdjustmentsContrast;
    //colorAdjustments.colorFilter
    [SerializeField] private Color colorAdjustmentsColorFilter;
    //ゲームの全体を処理するクラス
    private GameMasterController gameMasterController;
    //vignette
    private Vignette vignette;
    //chromatic aberration
    private ChromaticAberration chromaticAberration;
    //color adjustment
    private ColorAdjustments colorAdjustments;
    //バレットタイムのチャージ
    private float bulletTimeCharge;
    //バレットタイム中か否か
    private bool isBulletTime = false;
    private void Start()
    {
        bulletTimeCharge = bulletTimeLimitSeconds;
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out colorAdjustments);
    }
    private void FixedUpdate()
    {
        UI();
        PostProcess();
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
    //UI処理
    public void UI()
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
    //ポストプロセス処理
    private void PostProcess()
    {
        //バレットタイム中なら
        if (isBulletTime)
        {
            //ほぼ変化が終わっていれば終了
            if (Mathf.Approximately(vignette.intensity.value, vignetteIntensity)) return;
            //vignette(画面の四隅暗くするやつ)
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, vignetteIntensity, postProcessChangeSpeed);
            vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, vignetteSmoothness, postProcessChangeSpeed);
            //chromaticAberration(色収差)
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, chromaticAberrationIntensity, postProcessChangeSpeed);
            //colorAdjustment(フィルターかけたりコントラスト調整したり)
            colorAdjustments.contrast.value = Mathf.Lerp(colorAdjustments.contrast.value, colorAdjustmentsContrast, postProcessChangeSpeed);
            colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, colorAdjustmentsPostExposure, postProcessChangeSpeed);
            colorAdjustments.colorFilter.value = Color.Lerp(colorAdjustments.colorFilter.value, colorAdjustmentsColorFilter, postProcessChangeSpeed);
        }
        //そうでなければ
        else
        {
            //ほぼ変化が終わっていれば終了
            if (Mathf.Approximately(vignette.intensity.value, 0)) return;
            //vignette(画面の四隅暗くするやつ)
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0, postProcessChangeSpeed);
            vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 0, postProcessChangeSpeed);
            //chromaticAberration(色収差)
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 0, postProcessChangeSpeed);
            //colorAdjustment(フィルターかけたりコントラスト調整したり)
            colorAdjustments.contrast.value = Mathf.Lerp(colorAdjustments.contrast.value, 0, postProcessChangeSpeed);
            colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, 0, postProcessChangeSpeed);
            colorAdjustments.colorFilter.value = Color.Lerp(colorAdjustments.colorFilter.value, new(1,1,1,1), postProcessChangeSpeed);
        }
    }
    public void gameMasterSetter(GameMasterController master)
    {
        gameMasterController = master;
    }
}