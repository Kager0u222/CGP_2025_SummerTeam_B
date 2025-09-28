using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class PlayerWire : MonoBehaviour
{
    //ワイヤーの長さ
    [SerializeField] private float wireLength;
    //ワイヤーの長さのプロパティ
    public float WireLength { get{ return wireLength; } } 
    //ワイヤーの引っ張る強さ
    [SerializeField] private float wireTension;
    //ワイヤーの力の減衰
    [SerializeField] private float wireDamping;
    //ワイヤーの長さ(MAX)
    [SerializeField] private float wireMaxLength;
    //ワイヤーの長さ(Min)
    [SerializeField] private float wireMinLength;
    //ワイヤーの太さ
    [SerializeField] private float wireWidth;
    //ワイヤーの色
    [SerializeField] private Color wireColor;
    //アンカーの速度
    [SerializeField] private float anchorSpeed;
    //ワイヤーのマテリアル
    [SerializeField] private Material wireMaterial;
    //ワイヤーの発射位置用オブジェクトの位置取得
    [SerializeField] private Transform wireFireposition;
    //↑のゲッター
    public Transform WireFirePositionGetter()
    {
        return wireFireposition;
    }
    //ワイヤーのアンカー
    [SerializeField] private WireAnchor wireAnchor;
    //ワイヤーの追加で引っ張る力の強さ
    [SerializeField] private float subWireTension;

    //ワイヤーの追加で引っ張る力が最大になる距離
    [SerializeField] private float subWireTensionMaxDistance;
    //ギミックの場合に加える力
    [SerializeField] private float subWireTensionToGimmick;
    //レイヤーのScriptableObject
    [SerializeField] private Layers layers;
    //ワイヤー飛ばしたときの音
    [SerializeField] private AudioClip wireFireSE;
    //ワイヤーつないだときの音
    [SerializeField] private AudioClip wireConnectSE;
    //SE流すやつ
    [SerializeField] private PlayerAudio playerAudio;
    //接続中の物体のレイヤーの番号
    private int layerNumber;
    //ワイヤーがつながってる時の情報保存用
    private SpringJoint sj;
    //↑のゲッター
    public SpringJoint SpringJoint { get { return sj; } }
    //linerendererの情報保存用
    private LineRenderer lr;
    //↑のセッター
    public void LineRendererPositionSetter(Transform value)
    {
        lr.SetPosition(0, wireFireposition.position);
        lr.SetPosition(1, value.position);
    }
    //ワイヤーでつないだ物体のRigidbody
    private Rigidbody sjrb;
    //レイヤーマスク
    private LayerMask layerMask;

    private void Start()
    {
        //レイヤーマスクの設定
        layerMask = 1 << layers.GroundLayer;
        layerMask += 1 << layers.GimmickLayer;
    }

    //ワイヤー発射
    public void StartWire(Transform cameraTransform, Transform cameraBaseTransform)
    {
        //レイの起点と向きの指定
        Ray ray = new Ray(cameraTransform.position, cameraBaseTransform.forward);

        //射出先情報の保存用変数
        RaycastHit hit;

        //ワイヤー射出先の情報の保存と射出可能かの判断用bool値の保存
        bool canFireWire = Physics.Raycast(ray, out hit, wireLength, layerMask);

        //アンカー発射 向きはwireFirePositionから接触予定点に向けて
        if(canFireWire)
            wireAnchor.FireAnchor(anchorSpeed, hit.point - wireFireposition.position,wireLength);
        //ワイヤーがさせないときも発射するだけ発射する
        else 
            wireAnchor.FireAnchor(anchorSpeed, cameraTransform.position + cameraBaseTransform.forward * wireLength - wireFireposition.position,wireLength);
        //アンカーとLineRendererつなぐ
        var wire = gameObject.AddComponent<LineRenderer>();
        //ワイヤー発射音
        playerAudio.PlaySE(wireFireSE);

        //ワイヤーの設定
        //ワイヤーの両端を指定
        wire.SetPosition(0, wireFireposition.position);
        wire.SetPosition(1, wireAnchor.transform.position);
        //ワイヤーの太さ
        wire.startWidth = wireWidth;
        wire.endWidth = wireWidth;
        //ワイヤーの色
        wire.material = wireMaterial;
        wire.startColor = wireColor;
        wire.endColor = wireColor;
        //LineRendererを保存
        lr = gameObject.GetComponent<LineRenderer>();
    }
    //ワイヤー接続
    public void ConnectWire(Transform hitTransform, Rigidbody hitrb,Vector3 hitPoint)
    {
        //すでにワイヤーがつながってたら消す
        if (sj != null)
        {
            Destroy(sj);
            sj = null;
        }
        //ワイヤー接続音
        playerAudio.PlaySE(wireConnectSE);
        //playerObjectにspring jointをつけていろいろ設定
        var spring = gameObject.AddComponent<SpringJoint>();
        //接地点とplayerObjectを接続
        spring.connectedBody = hitrb;
        spring.connectedAnchor = hitTransform.InverseTransformPoint(hitPoint);

        spring.autoConfigureConnectedAnchor = false;

        //弾性力と減衰の設定
        spring.spring = wireTension;
        spring.damper = wireDamping;
        spring.maxDistance = wireMaxLength;
        spring.minDistance = wireMinLength;
        spring.enableCollision = true;
        //SpringJointを保存
        sj = gameObject.GetComponent<SpringJoint>();
        //ワイヤーでつないだ物体のRigidbody
        sjrb = sj.connectedBody.GetComponent<Rigidbody>();

        //isKinematic確認
        layerNumber = sj.connectedBody.gameObject.layer;

    }

    //ワイヤー切断
    public void EndWire()
    {
        Destroy(sj);
        Destroy(lr);
        //保存していたSpringJointを削除
        sj = null;
        //保存していたLineRendererを削除
        lr = null;
        //アンカーを無効化
        wireAnchor.IsAnchorSetter(false);
    }

    //ワイヤー展開中の挙動
    public void Wire(Rigidbody rb, int groundLayer, int gimmickLayer)
    {
        if (sj == null) return;
        //ワイヤーの表示の始点を更新する
        lr.SetPosition(0, wireFireposition.position);
        lr.SetPosition(1, sj.connectedBody.transform.TransformPoint(sj.connectedAnchor));

        //ワイヤーの補助パワーを追加で加える
        //力を加える方向を計算
        Vector3 powerDirection = (sj.connectedBody.transform.TransformPoint(sj.connectedAnchor) - transform.position).normalized;
        //どのくらい力を加えるか(距離に比例)
        float tension = Mathf.Clamp(Vector3.Distance(transform.position, sj.connectedBody.transform.TransformPoint(sj.connectedAnchor)) - wireMaxLength, 0, subWireTensionMaxDistance);
        //ワイヤーをつないでいるものが地面とかの場合自分に力加える
        if (layerNumber == groundLayer)
            rb.AddForce(powerDirection * tension * subWireTension);

        //ギミックとかの場合ギミックに力を加える
        // (軽いものが爆速で飛んでいくのを防ぐためにギミックの重さを加味して加える力を変えているが
        // //SpringJointの都合上あまりギミック用オブジェクトを重くできないので
        // プレイヤーの重さの半分より重いオブジェクトにかかる相対的な力はそれより小さいオブジェクトに比べ小さくなるようにしている)
        if (layerNumber == gimmickLayer)
            sjrb.AddForce(-powerDirection * tension * subWireTensionToGimmick * Mathf.Clamp(sjrb.mass, 0, rb.mass / 2));

    }
}
