using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MagicUIController : MonoBehaviour
{
    //スプライト集
    [SerializeField] private Sprite[] sprites;
    //魔法の表示のオブジェクト
    [SerializeField] private Image magicUI;
    //現在の魔法の種類
    private int magicType = 2;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //切り替え時演出
    public void ChangeMagic(int changeDirection)
    {
        //番号変更(正か不かは呼び出し元から参照)
        magicType += 1 * changeDirection;
        //増えすぎ、減りすぎでループ
        if (magicType > 2) magicType = 0;
        if (magicType < 0) magicType = 2;
        //テクスチャ適応
        magicUI.sprite = sprites[magicType];
    }
}
