using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MagicPool : MonoBehaviour
{
    //弾のPrefab
    [SerializeField] private MagicController magicPrefab;
    //弾の最大数
    [SerializeField] private int magicMaxCount;
    //生成した弾の保存場所
    Queue<MagicController> MagicQueue = new Queue<MagicController>();
    //弾の初期位置
    private Vector3 magicDefaultPosition = new Vector3(100, -100, 100);
    //このスクリプト
    [SerializeField]private MagicPool pool;

    //必要な数の弾を生成しQueueに保存
    void Awake()
    {
        //magicMaxControllerまで繰り返し
        for (int i = 0; i < magicMaxCount; i++)
        {
            //生成
            MagicController magic = Instantiate(magicPrefab, magicDefaultPosition, Quaternion.identity, transform);
            //弾の子particle systemをオフ
            ParticleSystem particle = magic.GetComponentInChildren<ParticleSystem>();
            particle.Stop();
            //弾非表示
            magic.gameObject.SetActive(false);
            //Queueに弾を格納
            MagicQueue.Enqueue(magic);
        }
    }

    //Queueから引き出す
    public void BorrowMagic(Vector3 launchPosition, Quaternion launchRotation, MagicTypeAsset.MagicType magicType)
    {
        //在庫がなければnull
        if (MagicQueue.Count <= 0) return;
        MagicController magic = MagicQueue.Dequeue();
        //弾表示
        magic.gameObject.SetActive(true);
        //弾の子particle systemをオフ
        ParticleSystem particle = magic.GetComponentInChildren<ParticleSystem>();
        particle.Stop();
        //ステータスの受け渡しなど初期設定
        magic.LaunchMagic(launchPosition, launchRotation, magicType, pool);
    }

    public void ReturnMagic(MagicController magic)
    {
        //弾の子particle systemをオフ
        ParticleSystem particle = magic.GetComponentInChildren<ParticleSystem>();
        particle.Stop();
        //弾非表示
        magic.gameObject.SetActive(false);
        //Queueに弾を格納
        MagicQueue.Enqueue(magic);
    }
    
}
