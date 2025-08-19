using System.Xml.Serialization;
using UnityEngine;

public class OnGroundChcker : MonoBehaviour
{
    //プレイヤーのオブジェクト
    private GameObject playerObject;
    //接触判定仮置き
    private bool onGround;
    //地面のレイヤー
    [SerializeField]
    private int groundLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //プレイヤーのオブジェクトを保存
        playerObject = transform.parent.gameObject;
        
    }


    //接触判定を取得
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == groundLayer) onGround = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == groundLayer) onGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == groundLayer) onGround = false;
    }

    //接触したか否かを受け渡す
    public bool OnGroundGetter()
    {
        return onGround;
    }
}
