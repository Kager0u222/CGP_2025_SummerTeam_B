using UnityEngine;
using UnityEngine.UIElements;

public class Gate : MonoBehaviour
{
    //トリガーのクラス
    [SerializeField] private GimmickTrigger trigger;
    //消滅する速さ
    [SerializeField] private float disappearSpeed;
    //デフォルトのスケール
    private Vector3 defaultScale;

    void Start()
    {
        defaultScale = transform.localScale;
    }


    void Update()
    {
        //トリガーが解除されていれば徐々に小さく
        if (trigger.IsUnlocked) transform.localScale *= disappearSpeed;
        //トリガーが解除されていなければ大きさもとに戻す
        else transform.localScale = defaultScale;
        //完全に小さくなったら無効化
        if (transform.localScale == Vector3.zero) gameObject.SetActive(false);
    }
}
