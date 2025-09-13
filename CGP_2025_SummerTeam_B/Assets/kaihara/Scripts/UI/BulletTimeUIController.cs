using UnityEngine.UI;
using UnityEngine;

public class BulletTimeUIController : MonoBehaviour
{
    //ゲージのImage
    [SerializeField] private Image bulletTimeUI;
    //ゲージ用のスプライト
    [SerializeField] private Sprite[] sprites;

    public void ChangeGauge(float bulletTimeCharge, float bulletTimeMax, bool isRecharging)
    {
        //ゲージを変動
        bulletTimeUI.fillAmount = bulletTimeCharge / bulletTimeMax;
        //リチャージ中色変える
        if (isRecharging) bulletTimeUI.sprite = sprites[1];
        //チャージがマックスなら色変える
        if(bulletTimeCharge >= bulletTimeMax) bulletTimeUI.sprite = sprites[0];

    }
}
