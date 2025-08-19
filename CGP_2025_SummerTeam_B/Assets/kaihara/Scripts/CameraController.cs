using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //カメラの位置の基準になるオブジェクトにつくスクリプト

    //playerオブジェクトのposition取得
    [SerializeField, Header("playerオブジェクトのposition取得用")]
    private Transform playerTransform;
    //cameraオブジェクト
    [SerializeField, Header("カメラオブジェクト取得用")]
    private GameObject cameraObject;
    //カメラの注視点のオブジェクト
    [SerializeField, Header("注視点オブジェクト取得用")]
    private GameObject cameraFocusObject;
    //カメラ感度
    [SerializeField, Header("カメラ感度")]
    private float cameraSensitivity;

    //物体の中心からみたカメラの中心(注視点?)の位置
    [SerializeField, Header("カメラの注視点のローカル座標")]
    private Vector3 cameraFocusLocalPosition;
    //カメラから注視点までの距離
    [SerializeField,Header("カメラ-注視点間の距離")]
    private float cameraToFocusDistance;
    //カメラの回転
    private Vector2 cameraRotation;
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
        int layerMask = 1 << 3;
        if (Physics.SphereCast(transform.position+transform.forward*0.3f, 1f, cameraObject.transform.position - transform.position - transform.forward*0.3f, out hit, Vector3.Distance(cameraObject.transform.position, transform.position)+0.3f,layerMask))
        {
            transform.position += (-cameraObject.transform.position + transform.position).normalized * Vector3.Distance(cameraObject.transform.position, hit.point);
        }
        
    }

    //視点移動のマウス入力取得
    public void OnCameraRotate(InputAction.CallbackContext context)
    {
        //マウスの移動量をカメラの回転量に加算
        cameraRotation.x = Mathf.Clamp(cameraRotation.x + context.ReadValue<Vector2>().y * cameraSensitivity, -90, 80);
        cameraRotation.y += context.ReadValue<Vector2>().x * cameraSensitivity;
        //上の値をカメラの角度に反映
        transform.rotation = Quaternion.Euler(-cameraRotation.x, cameraRotation.y, 0);
    }
}
