using UnityEngine;

public abstract class HpController : MonoBehaviour
{
    //ステータスのScriptableObject
    [SerializeField] private CharacterStatuses characterStatuses;
    //キャラのタイプ
    [SerializeField] private CharacterTypeAsset.CharacterType type;
    //キャラのステータスとってくる用変数
    private ICharacterStatuses status;
    //hpの現在値
    private float hp;
    //なんか値が欲しくなった時用のプロパティ
    public float Hp => hp;
    public float HpMax => status.HpMax;
    void Awake()
    {
        //ステータスを取ってくる場所を指定
        status = characterStatuses.GetCharactorStatuses(type);
        //体力を最大値に設定
        hp = status.HpMax;
    }
    //ダメージを加える処理
    public void AddDamage(float damage)
    {
        hp -= damage;
        OnDamage();
        if (hp <= 0) OnDeath();
    }
    //体力ゼロの時の処理
    public abstract void OnDeath();
    //被ダメージ時の処理
    public abstract void OnDamage();

}