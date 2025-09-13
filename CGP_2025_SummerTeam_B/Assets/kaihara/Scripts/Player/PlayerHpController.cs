
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHpController : HpController
{
    [SerializeField] private HpBarController hpBarController;
    //回復可能回数
    [SerializeField] private int canHealLimit;
    //回復量
    [SerializeField] private float healAmount;
    //回復回数とかの表示のUIのクラス
    [SerializeField] private HealUIController healUIController;
    //残り回復可能回数 
    private int canHealCount;
    private void Start()
    {
        canHealCount = canHealLimit;
        //表示の変更
        healUIController.ChangeGauge(canHealCount, canHealLimit);
    }
    public override void OnDamage()
    {
        hpBarController.OnDamage();
    }
    public override void OnDeath()
    {
        SceneManager.LoadScene("PlayerMotionTestScene");
    }
    public void OnHeal(InputAction.CallbackContext context)
    {
        if (context.performed && canHealCount > 0)
        {
            AddDamage(-healAmount);
            canHealCount -= 1;
            //表示の変更
            Debug.Log(canHealCount + " " + canHealLimit);
            healUIController.ChangeGauge(canHealCount, canHealLimit);
        }
    }
}