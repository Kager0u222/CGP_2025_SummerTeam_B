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
        PlayerController.OnGround = false;
        PlayerController.OnWall = false;
        if (collisionObjects == null) return;
        foreach (ContactPoint contactPoint in collisionObjects.contacts)
        {
            //床なら床のboolをtrue
            if (Vector3.Angle(Vector3.down, contactPoint.normal) >= 0 && Vector3.Angle(Vector3.down, contactPoint.normal) < 85)
                PlayerController.OnGround = true;
            //壁なら壁のboolをtrue
            if (Vector3.Angle(Vector3.down, contactPoint.normal) <= 95 && Vector3.Angle(Vector3.down, contactPoint.normal) >= 85)
            {
                PlayerController.OnWall = true;
                PlayerController.WallNormalVector = new Vector3(-contactPoint.normal.x, 0, -contactPoint.normal.z).normalized;
            }
        }
        if (!PlayerController.OnGround) PlayerController.OnGround = subOnGroundChecker.OnGroundGetter();
        if (PlayerController.OnGround) PlayerController.OnWall = false;
        if (!PlayerController.OnWall) PlayerController.WallNormalVector = Vector3.zero;
        
    }
}
