using System.Xml.Serialization;
using UnityEngine;

public class OnGroundChcker : MonoBehaviour
{

    //接触判定仮置き
    private bool onGround;
    //レイヤーのScriptableObject
    [SerializeField]private Layers layers;


    //接触判定を取得
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layers.GroundLayer) onGround = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == layers.GroundLayer) onGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == layers.GroundLayer) onGround = false;
    }

    //接触したか否かを受け渡す
    public bool OnGroundGetter()
    {
        return onGround;
    }
}
