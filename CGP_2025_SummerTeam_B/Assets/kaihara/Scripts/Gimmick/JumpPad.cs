using UnityEngine;

public class JumpPad : MonoBehaviour
{
    //レイヤー
    [SerializeField] private Layers layers;
    //ジャンプ力
    [SerializeField] private float jumpPower ;

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーが触れたら
        if (other.gameObject.layer == layers.PlayerLayer)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            //速度リセット
            rb.linearVelocity = Vector3.zero;
            //ジャンプ力を加える
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}
