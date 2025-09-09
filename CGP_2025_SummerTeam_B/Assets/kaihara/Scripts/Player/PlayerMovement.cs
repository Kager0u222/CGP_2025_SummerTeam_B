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

    public void Move(Vector2 inputDirection,Transform cameraBaseTransform,bool onGround,bool onWall,Rigidbody rb)
    {
        //向いている向きをカメラに合わせる
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraBaseTransform.eulerAngles.y, transform.eulerAngles.z);
        //入力を変換して視点依存の移動ベクトルに
        Vector3 moveDirection = transform.forward * inputDirection.y + transform.right * inputDirection.x;
        //計算後のベクトルに従って力を加える
        //地面にいるとき
        if (onGround)
            rb.AddForce((moveDirection.x * playerSpeed - rb.linearVelocity.x) * moveAddforcePower, 0, (moveDirection.z * playerSpeed - rb.linearVelocity.z) * moveAddforcePower);
        //空中にいるとき
        else
        {
            
            rb.AddForce((moveDirection.x * playerSpeed - rb.linearVelocity.x / 4) * moveAddforcePowerinAir, 0, (moveDirection.z * playerSpeed - rb.linearVelocity.z / 4) * moveAddforcePowerinAir);
            //落下中で壁を触っているときは落下速度に制限をかける
            if (onWall && rb.linearVelocity.y < 0)
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y - 9.8f * gravityPower * Time.fixedDeltaTime, maxFallSpeedOnWall, 100), rb.linearVelocity.z);
            //
            else
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y - 9.8f * gravityPower * Time.fixedDeltaTime, rb.linearVelocity.z);
        }
    }
    public void Jump(Vector3 wallNormalVector,Rigidbody rb)
    {
        //ワイヤーで上方向に加速しながら壁ジャンプするとすごい勢いで上にすっ飛んでくので縦方向の速度をリセット
        rb.linearVelocity = new Vector3(rb.linearVelocity.x,0,rb.linearVelocity.z);
        //いい感じに力を加える
        rb.AddForce(wallNormalVector.x*wallJumpPower,jumpPower + 9.8f * gravityPower / 2,wallNormalVector.z*wallJumpPower, ForceMode.Impulse);
    }
}
