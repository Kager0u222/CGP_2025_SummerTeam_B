using UnityEditor.Callbacks;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //弾の速度
    [SerializeField]
    private float bulletSpeed;
    //弾の射程
    private float bulletLength;
    //地面のレイヤーの番号
    [SerializeField]
    private int groundLayer;
    //敵のレイヤーの番号
    [SerializeField]
    private int enemylayer;
    //プレイヤーのレイヤーの番号
    [SerializeField]
    private int playerlayer;
    //プレイヤーのタグ
    [SerializeField]
    private string playerTag;
    //敵のタグ
    [SerializeField]
    private string enemyTag;
    //rigidbody
    private Rigidbody rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        //消滅の予約
        Destroy(gameObject, bulletLength / bulletSpeed);
        //移動速度指定
        rb.linearVelocity = transform.forward * bulletSpeed;
    }
    void OnTriggerEnter(Collider other)
    {
        //地面に触れたら消滅
        if (other.gameObject.layer == groundLayer) Destroy(gameObject);
        

        //敵に触れたら自分と敵を消滅
        if (other.gameObject.layer == enemylayer && gameObject.tag == playerTag)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (gameObject.tag == enemyTag && other.gameObject.layer == playerlayer)
        {
            Destroy(gameObject);
            Debug.Log("HIT!!!");
        }
    }
    public void BulletLenghtSetter(float bulletlength)
    {
        bulletLength = bulletlength;
    }
}
