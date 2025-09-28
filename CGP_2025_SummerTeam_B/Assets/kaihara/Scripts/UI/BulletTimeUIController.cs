using UnityEngine.UI;
using UnityEngine;

public class BulletTimeUIController : MonoBehaviour
{
    //ゲージのImage
    [SerializeField] private Image bulletTimeUI;
    //ゲージ用のカラー
    [SerializeField] private Color[] color;

    public void ChangeGauge(float bulletTimeCharge, float bulletTimeMax, bool isRecharging)
    {
        //ゲージを変動
        bulletTimeUI.fillAmount = bulletTimeCharge / bulletTimeMax;
        //リチャージ中色変える
        if (isRecharging) bulletTimeUI.color = color[1];
        //チャージがマックスなら色変える
        if(bulletTimeCharge >= bulletTimeMax) bulletTimeUI.color = color[0];

    }
}
