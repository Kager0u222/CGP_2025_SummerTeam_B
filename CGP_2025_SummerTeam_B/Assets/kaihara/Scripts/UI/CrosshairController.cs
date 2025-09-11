using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    //レイヤーのScriptableObject
    [SerializeField] private Layers layers;
    //カメラの位置
    [SerializeField] private Transform cameraTransform;
    //カメラの移動の起点のオブジェクトの位置
    [SerializeField] private Transform cameraBaseTransform;
    //プレイヤーのワイヤーのクラス
    [SerializeField] private PlayerWire playerWire;
    //ワイヤーがさせるときのスプライト
    [SerializeField] private Sprite canWireCrosshair;
    //ワイヤーがさせないときのスプライト
    [SerializeField] private Sprite canntWireCrossHair;
    //クロスヘアのオブジェクト
    [SerializeField] private Image crosshairObject;
    void FixedUpdate()
    {
        //レイヤーマスクの設定
        LayerMask layerMask = 1 << layers.GroundLayer;
        layerMask += 1 << layers.GimmickLayer;

        //レイの起点と向きの指定
        Ray ray = new Ray(cameraTransform.position, cameraBaseTransform.forward);

        //射出先情報の保存用変数
        RaycastHit hit;

        //ワイヤー射出先の情報の保存と射出可能かの判断用bool値の保存
        bool canFireWire = Physics.Raycast(ray, out hit, playerWire.WireLength, layerMask);

        //射程内につなげるとこがなければつなげられないとき用のクロスヘア
        if (!canFireWire || !hit.rigidbody) crosshairObject.sprite = canntWireCrossHair;
        //つなげるとこがあればつなげるとき用のクロスヘア
        else crosshairObject.sprite = canWireCrosshair;
    }
}
