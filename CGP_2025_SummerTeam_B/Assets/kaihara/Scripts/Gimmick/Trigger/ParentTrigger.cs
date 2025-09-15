using UnityEngine;

public class ParentTrigger : GimmickTrigger
{
    private void Update()
    {
        //子オブジェクトがなくなったら解除
        if (transform.childCount == 0) isUnlocked = true;
        else isUnlocked = false;
    }
}