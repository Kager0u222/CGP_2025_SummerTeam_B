
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealUIController : MonoBehaviour
{
    //回復回数の表示用text
    [SerializeField] private TextMeshProUGUI healCounter;
    //ゲージのImage
    [SerializeField] private Image healGauge;
    //ゲージの変化速度
    [SerializeField, Range(0, 1)] private float gaugeChangeSpeed;
    //残り回復回数
    private int healCount;
    //最大回復回数
    private int healLimit;

    public void ChangeGauge(int count, int limit)
    {
        healCount = count;
        healLimit = limit;
        //回数を数字で表示
        healCounter.text = "×" + healCount.ToString();

    }
    public void Update()
    {
        //ゲージでも表示
        healGauge.fillAmount = Mathf.Lerp(healGauge.fillAmount, (float)healCount / healLimit, gaugeChangeSpeed);
    }
}
