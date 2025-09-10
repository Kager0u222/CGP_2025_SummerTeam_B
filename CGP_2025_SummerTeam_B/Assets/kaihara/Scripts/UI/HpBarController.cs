using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    //プレイヤーのHpのクラス
    [SerializeField] private PlayerHpController playerHpController;
    //Hpの緑部分(現在値)
    [SerializeField] private Image hpGauge;
    //Hpの赤部分(減少量を見せるところ)
    [SerializeField] private Image hpDamage;
    //Hpの数値部分
    [SerializeField] private TextMeshProUGUI hpData;
    //ダメージを食らってからどれくらいで赤ゲージを減少させ始めるか
    [SerializeField] private float redGaugeBiginDecreasingTime;
    //赤ゲージの減少速度
    [SerializeField] private float redGaugeDecreace;
    //Hpの数字の増減速度
    [SerializeField] private float hpDataChangeSpeed;
    //ダメージを食らった時刻
    private float damagedTime = 0;
    //Textに適応する用のHp保存変数
    private float hp;

    void Start()
    {
        //体力満タンの状態で表示
        hpGauge.fillAmount = 1;
        hpDamage.fillAmount = 1;
        hpData.text = playerHpController.Hp + "/" + playerHpController.HpMax;
        hp = playerHpController.Hp;
    }


    void Update()
    {
        //前回被弾してから十分に時間がたちかつ赤ゲージが緑ゲージより長いとき赤ゲージを減少させる
        if (Time.time - damagedTime > redGaugeBiginDecreasingTime && hpDamage.fillAmount > hpGauge.fillAmount)
            hpDamage.fillAmount -= redGaugeDecreace;
        //赤ゲージが減少しすぎたら緑ゲージと同じにする
        if (hpGauge.fillAmount > hpDamage.fillAmount) hpDamage.fillAmount = hpGauge.fillAmount;
        //Hpの表記と実際のHpに乖離があるとき
        if (hp != playerHpController.Hp)
        {
            //表記Hpを実際のHpに近づける
            hp += hpDataChangeSpeed * (playerHpController.Hp - hp) / Mathf.Abs(playerHpController.Hp - hp);
            //小数点以下切り捨てで表記に反映
            hpData.text = Mathf.Floor(hp) + "/" + playerHpController.HpMax;
            if (Mathf.Abs(playerHpController.Hp - hp) < hpDataChangeSpeed) hp = playerHpController.Hp;
        }
    }
    //体力バー更新用
    public void OnDamage()
    {
        damagedTime = Time.time;
        hpGauge.fillAmount = playerHpController.Hp / playerHpController.HpMax;
    }
}
