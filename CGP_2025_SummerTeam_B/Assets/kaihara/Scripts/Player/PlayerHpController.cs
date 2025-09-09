using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHpController : HpController
{
    public override void OnDeath()
    {
        SceneManager.LoadScene("PlayerMotionTestScene");
    }
}