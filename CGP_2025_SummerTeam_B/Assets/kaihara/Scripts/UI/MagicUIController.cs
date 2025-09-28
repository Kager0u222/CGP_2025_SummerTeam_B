using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MagicUIController : MonoBehaviour
{

    //魔法の表示のオブジェクト
    [SerializeField] private Image magicUI;
    //炎
    [SerializeField] private Image fireUI;
    //氷
    [SerializeField] private Image iceUI;
    //雷
    [SerializeField] private Image thunderUI;
    //リチャージ
    [SerializeField] private Image rechargeUI;
    //切り替え演出の移動幅
    [SerializeField] private float moveDistanceOnChange;
    //切り替え演出時の移動速度
    [SerializeField, Range(0, 1)] private float moveSpeedOnChange;
    //クールタイム終了時のカラー
    [SerializeField] private Color rechargedUIColor;
    //クールタイム中のカラー
    [SerializeField] private Color unrechargedUIColor;
    //プレイヤーの発射処理のクラス
    private PlayerShooting playerShooting;
    //上のセッター
    public void Shooting(PlayerShooting value)
    {
        playerShooting = value;
    }
    
    //現在の魔法の種類
    private int magicType = 0;
    //前回の魔法のImage
    private Image previousUI ;
    //次の魔法のImage
    private Image nextUI;
    //前の魔法をどっちに動かすか
    private int previousUIMoveDirection;
    //タイプ表示の中心
    private float typeUICenter;
    //リチャージUIの最大値
    private float rechargeUIMax;

    void Start()
    {
        //魔法タイプのUIの中央設定
        typeUICenter = fireUI.rectTransform.anchoredPosition.x;

        previousUI = thunderUI;
        nextUI = fireUI;

        //透明度リセット
        fireUI.color = new Color(1, 1, 1, 1);
        iceUI.color = new Color(1, 1, 1, 0);
        thunderUI.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //今のタイプのUIが中心になければ移動
        if (nextUI.rectTransform.anchoredPosition.x != typeUICenter)
        {
            //前のやつを移動+透明に
            previousUI.rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(previousUI.rectTransform.anchoredPosition.x, typeUICenter + moveDistanceOnChange * previousUIMoveDirection, moveSpeedOnChange), previousUI.rectTransform.anchoredPosition.y);
            previousUI.color = new Color(1, 1, 1, Mathf.Lerp(previousUI.color.a, 0, moveSpeedOnChange));
            //次のやつを移動+見えるように
            nextUI.rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(nextUI.rectTransform.anchoredPosition.x, typeUICenter, moveSpeedOnChange), nextUI.rectTransform.anchoredPosition.y);
            nextUI.color = new Color(1, 1, 1, Mathf.Lerp(nextUI.color.a, 1, moveSpeedOnChange));
        }
        //クールタイムが終わっていればゲージの最大値を変更+色変更
        if (playerShooting.CoolTime <= 0)
        {
            rechargeUIMax = playerShooting.MagicCoolTIme;
            rechargeUI.color = rechargedUIColor;
        }
        //そうでないときもそうでないときで色変更
        else rechargeUI.color = unrechargedUIColor;
        //クールタイムのゲージ処理
        rechargeUI.fillAmount = (rechargeUIMax - playerShooting.CoolTime) / rechargeUIMax;
    }
    //切り替え時演出
    public void ChangeMagic(int changeDirection)
    {
        //変更前の演出をカットしてUIを移動+透明・不透明化
        previousUI.rectTransform.anchoredPosition = new Vector2(typeUICenter + moveDistanceOnChange * previousUIMoveDirection, previousUI.rectTransform.anchoredPosition.y);
        previousUI.color = new Color(1, 1, 1, 0);
        nextUI.rectTransform.anchoredPosition = new Vector2(typeUICenter, nextUI.rectTransform.anchoredPosition.y);
        nextUI.color = new Color(1, 1, 1, 1);
        //変更前のタイプのImage保存
        if (magicType == 0) previousUI = fireUI;
        if (magicType == 1) previousUI = iceUI;
        if (magicType == 2) previousUI = thunderUI;
        //番号変更(正か不かは呼び出し元から参照)
        magicType -= 1 * changeDirection;
        //増えすぎ、減りすぎでループ
        if (magicType > 2) magicType = 0;
        if (magicType < 0) magicType = 2;
        //次のタイプのImage保存
        if (magicType == 0) nextUI = fireUI;
        if (magicType == 1) nextUI = iceUI;
        if (magicType == 2) nextUI = thunderUI;
        //UIの移動準備
        //前回のタイプのUIが移動する方向を決定
        previousUIMoveDirection = changeDirection;
        //次のUIを端に移動
        nextUI.rectTransform.anchoredPosition = new Vector2(typeUICenter - previousUIMoveDirection * moveDistanceOnChange, nextUI.rectTransform.anchoredPosition.y);
    }
}
