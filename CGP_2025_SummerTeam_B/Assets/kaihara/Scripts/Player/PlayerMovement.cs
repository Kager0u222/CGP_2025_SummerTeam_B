
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{

    //移動速度の変数
    [SerializeField] private float playerSpeed;
    //移動処理のaddforceの倍率(大きいほど移動がキー入力に追従しやすくなる)
    [SerializeField] private float moveAddforcePower;
    //空中での移動処理のaddforceの倍率(大きいほど移動がキー入力に追従しやすくなる)
    [SerializeField] private float moveAddforcePowerinAir;
    //ジャンプ力の変数
    [SerializeField] private float jumpPower;
    //重力の倍率
    [SerializeField] private float gravityPower;
    //壁ジャンプしたときの横方向へ飛ぶ強さ
    [SerializeField] private float wallJumpPower;
    //壁張り付き時の落下速度の最大値
    [SerializeField] private float maxFallSpeedOnWall;
    //向きの変化速度
    [SerializeField,Range(0,1)] private float rotationSpeed;
    //カメラ正面を向くときの向きの変化速度
    [SerializeField,Range(0,1)] private float rotationSpeedCameraForward;
    //移動方向
    Vector3 moveDirection;

    public void Move(Vector2 inputDirection, Transform cameraBaseTransform, bool onGround, bool onWall, Rigidbody rb,bool isFire,bool isWire,GameMasterController gameMaster)
    {
        if (isFire || isWire ) LockedRotate(inputDirection, cameraBaseTransform, gameMaster);
        else FreeRotate(inputDirection, cameraBaseTransform, gameMaster);
        //計算後のベクトルに従って力を加える
        //地面にいるとき
        if (onGround)
            rb.AddForce((moveDirection.x * playerSpeed - rb.linearVelocity.x) * moveAddforcePower, 0, (moveDirection.z * playerSpeed - rb.linearVelocity.z) * moveAddforcePower);
        //空中にいるとき
        else
        {
            rb.AddForce((moveDirection.x * playerSpeed - rb.linearVelocity.x / 4) * moveAddforcePowerinAir, gravityPower * Physics.gravity.y, (moveDirection.z * playerSpeed - rb.linearVelocity.z / 4) * moveAddforcePowerinAir);
            //落下中で壁を触っているときは落下速度に制限をかける
            if (onWall && rb.linearVelocity.y < 0)
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, maxFallSpeedOnWall, 100), rb.linearVelocity.z);
        }
    }
    //プレイヤーをキー入力した方向に向かせるタイプの移動
    private void FreeRotate(Vector2 inputDirection, Transform cameraBaseTransform,GameMasterController gameMaster)
    {

        //キー入力中は回転方向を計算し回転  そうでなければ向きそのまま
        if (inputDirection != Vector2.zero)
        {
            //カメラの正面と右方向のベクトルを出し、それのy成分を0にして正規化する(カメラの上下の傾きを無視するため)
            Vector3 cameraForward = cameraBaseTransform.forward;
            Vector3 cameraRight = cameraBaseTransform.right;
            cameraForward = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;
            cameraRight = new Vector3(cameraRight.x, 0f, cameraRight.z).normalized;
            //入力から向く方向を決定
            Vector3 direction = cameraForward * inputDirection.y + cameraRight * inputDirection.x;
            //回転(現在地から入力方向へ線形補完)
            transform.rotation = Quaternion.LookRotation(Vector3.Slerp(transform.forward, direction.normalized, rotationSpeed*gameMaster.PhysicsSpeed), Vector3.up);
        }
        //x,z軸中心の回転を0に
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        //移動方向(オブジェクトの正面方向)
        moveDirection = transform.forward * inputDirection.magnitude;
    }
    //プレイヤーの向きをカメラに合わせるタイプの移動
    private void LockedRotate(Vector2 inputDirection,Transform cameraBaseTransform,GameMasterController gameMaster)
    {
        //向いている向きをカメラに合わせる
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0, cameraBaseTransform.eulerAngles.y,0),rotationSpeedCameraForward*gameMaster.PhysicsSpeed);
        //入力を変換して視点依存の移動ベクトルに
        moveDirection = transform.forward * inputDirection.y + transform.right * inputDirection.x;
    }
    public void Jump(Vector3 wallNormalVector, Rigidbody rb)
    {
        //ワイヤーで上方向に加速しながら壁ジャンプするとすごい勢いで上にすっ飛んでくので縦方向の速度をリセット
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        //いい感じに力を加える
        rb.AddForce(wallNormalVector.x * wallJumpPower, jumpPower + 9.8f * gravityPower / 2, wallNormalVector.z * wallJumpPower, ForceMode.Impulse);
    }
}
