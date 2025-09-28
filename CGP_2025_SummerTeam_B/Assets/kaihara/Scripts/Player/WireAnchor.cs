using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class WireAnchor : MonoBehaviour
{
    //プレイヤーのワイヤー処理クラス
    [SerializeField]private PlayerWire playerWire;
    //rigidbody
    private Rigidbody rb;

    //レイヤーのScriptableObject
    [SerializeField] private Layers layers;
    //ゲーム全体の処理をするクラス
    [SerializeField] private GameMasterController gameMasterController;

    //速度
    private float anchorSpeed;
    //アンカー発射中か否か
    private bool isAnchor = false;
    
    //↑のセッター
    public void IsAnchorSetter(bool value)
    {
        isAnchor = value;
    }

    //アンカーの寿命
    private float lifeTime;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //アンカー発射中ならLineRenderer処理+衝突処理の補助
        if (isAnchor)
        {
            //LineRendererの接触点を更新;
            playerWire.LineRendererPositionSetter(transform);
            //すでにSpringJointがついていたらSpringJointのConnectedBodyのところに移動
            if (playerWire.SpringJoint) transform.position = playerWire.SpringJoint.connectedBody.transform.TransformPoint(playerWire.SpringJoint.connectedAnchor);

            //そうでなければ着弾確認処理
            else
            {
                //衝突処理用にSpherecast
                RaycastHit hit;
                LayerMask layerMask = 1 << layers.GroundLayer;
                layerMask += 1 << layers.GimmickLayer;
                //衝突で衝突処理
                bool isHit = Physics.SphereCast(transform.position - transform.forward * transform.localScale.z / 2, transform.localScale.x, transform.forward, out hit, anchorSpeed * Time.fixedDeltaTime + transform.localScale.z / 2, layerMask);
                if (isHit&&hit.rigidbody)
                {
                    //ワイヤー接続
                    playerWire.ConnectWire(hit.transform, hit.rigidbody, hit.point);
                    //速度リセット
                    rb.linearVelocity = Vector3.zero;
                    //着弾点に移動
                    transform.position = hit.point;
                }
                //このときのみ射程処理
                //寿命減少
                lifeTime -= Time.fixedDeltaTime * gameMasterController.PhysicsSpeed;
                //寿命が切れたら強制終了
                if (lifeTime <= 0) playerWire.EndWire();
            }

        }
        //そうでないならプレイヤーの位置に移動
        else
        {
            transform.position = playerWire.WireFirePositionGetter().position;
            rb.linearVelocity = Vector3.zero;
        }

        
    }
    public void FireAnchor(float speed, Vector3 anchorRotaion, float wireLength)
    {
        //向き変更
        transform.rotation = Quaternion.LookRotation(anchorRotaion, Vector3.up);
        //速度設定
        rb.linearVelocity = transform.forward * speed;
        //cast用に速度保存
        anchorSpeed = speed;
        //アンカー発射中処理起動
        isAnchor = true;
        //寿命設定
        lifeTime = wireLength / anchorSpeed;
    }


}
