using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //カメラの位置の基準になるオブジェクトにつくスクリプト

    //playerオブジェクトのposition取得
    [SerializeField] private Transform playerTransform;
    //cameraオブジェクト
    [SerializeField] private GameObject cameraObject;
    //カメラの注視点のオブジェクト
    [SerializeField]private GameObject cameraFocusObject;
    //カメラ感度
    [SerializeField]private float cameraSensitivity;

    //物体の中心からみたカメラの中心(注視点?)の位置
    [SerializeField] private Vector3 cameraFocusLocalPosition;
    //カメラから注視点までの距離
    [SerializeField] private float cameraToFocusDistance;
    //レイヤーのScriptableObject
    [SerializeField] private Layers layers;

    void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }


    void Update()
    {
        
        //プレイヤーの位置に移動
        transform.position = playerTransform.position;
        //注視点の移動
        cameraFocusObject.transform.localPosition = cameraFocusLocalPosition;
        //カメラと注視点の間の距離
        cameraObject.transform.localPosition = new Vector3(0f,0f,cameraToFocusDistance);
        //カメラのめり込みの確認
        RaycastHit hit;
        LayerMask layerMask = 1 << layers.GroundLayer;
        layerMask += 1 << layers.EnemyLayer;
        layerMask += 1 << layers.GimmickLayer;
        layerMask += 1 << layers.BarrierLayer;
        if (Physics.SphereCast(transform.position, 0.3f, cameraObject.transform.position - transform.position, out hit, Vector3.Distance(cameraObject.transform.position, transform.position) + 0.3f, layerMask))
        {
            transform.position += (-cameraObject.transform.position + transform.position).normalized * Vector3.Distance(cameraObject.transform.position, hit.point);
        }
        
    }


}
