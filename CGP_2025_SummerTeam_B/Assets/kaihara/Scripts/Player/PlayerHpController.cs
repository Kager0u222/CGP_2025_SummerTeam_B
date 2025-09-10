using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHpController : HpController
{
    [SerializeField] private HpBarController hpBarController;
    public override void OnDamage()
    {
        hpBarController.OnDamage();
    }
    public override void OnDeath()
    {
        SceneManager.LoadScene("PlayerMotionTestScene");
    }
}