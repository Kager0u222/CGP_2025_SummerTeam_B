using UnityEngine;

public abstract class GimmickTrigger : MonoBehaviour
{
    //このギミックが解除されているか否か
    protected bool isUnlocked ;
    //上のやつのゲッター
    public bool IsUnlocked{get{ return isUnlocked; }}

}
