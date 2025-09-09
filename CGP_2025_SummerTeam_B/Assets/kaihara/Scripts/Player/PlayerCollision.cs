using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    //プレイヤーのメインのクラス
    [SerializeField] private PlayerController PlayerController;
    //地面判定の補助用オブジェクトのクラス
    [SerializeField] private OnGroundChcker subOnGroundChecker;
    //接地判定の処理
    public void Collision(Collision collisionObjects)
    {

        //接地判定・壁判定をリセット
        PlayerController.OnGround = false;
        PlayerController.OnWall = false;
        
        if (collisionObjects == null) return;
        //各接地判定ごとに処理
        foreach (ContactPoint contactPoint in collisionObjects.contacts)
        {
            //床なら床のboolをtrue
            if (Vector3.Angle(Vector3.down, contactPoint.normal) >= 0 && Vector3.Angle(Vector3.down, contactPoint.normal) < 85)
                PlayerController.OnGround = true;
            //壁なら壁のboolをtrueにし、壁の法線ベクトルを取得
            if (Vector3.Angle(Vector3.down, contactPoint.normal) <= 95 && Vector3.Angle(Vector3.down, contactPoint.normal) >= 85)
            {
                PlayerController.OnWall = true;
                PlayerController.WallNormalVector = new Vector3(-contactPoint.normal.x, 0, -contactPoint.normal.z).normalized;
            }
        }
        //接地判定が取れていない場合補助の方も確認
        if (!PlayerController.OnGround) PlayerController.OnGround = subOnGroundChecker.OnGroundGetter();
        //地面にいるときに壁ジャンプしてほしくないのでその時はonWall外す
        if (PlayerController.OnGround) PlayerController.OnWall = false;
        //壁に触ってない判定なら壁ジャンプしてほしくないので壁の法線ベクトルをリセットする
        if (!PlayerController.OnWall) PlayerController.WallNormalVector = Vector3.zero;
        
    }
}
