using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    //弾のPrefab
    [SerializeField] private BulletController bulletPrefab;
    //弾の最大数
    [SerializeField] private int bulletMaxCount;
    //生成した弾の保存場所
    Queue<BulletController> BulletQueue = new Queue<BulletController>();
    //弾の初期位置
    private Vector3 bulletDefaultPosition = new Vector3(100, -100, 100);
    //このスクリプト
    [SerializeField]private BulletPool pool;

    //弾の種類を保存するenum
    public enum BulletType
    {
        PlayerMiddle,
        PlayerShort,
        PlayerLong
    }

    //必要な数の弾を生成しQueueに保存
    void Awake()
    {
        //bulletMaxControllerまで繰り返し
        for (int i = 0; i < bulletMaxCount; i++)
        {
            //生成
            BulletController bullet = Instantiate(bulletPrefab, bulletDefaultPosition, Quaternion.identity, transform);
            //弾非表示
            bullet.gameObject.SetActive(false);
            //Queueに弾を格納
            BulletQueue.Enqueue(bullet);
        }
    }

    //Queueから引き出す
    public void BorrowBullet(Vector3 launchPosition, Quaternion launchRotation, BulletType bulletType)
    {
        //在庫がなければnull
        if (BulletQueue.Count <= 0) return;
        BulletController bullet = BulletQueue.Dequeue();
        //弾表示
        bullet.gameObject.SetActive(true);
        //ステータスの受け渡しなど初期設定
        bullet.LaunchBullet(launchPosition, launchRotation, bulletType, pool);
    }

    public void ReturnBullet(BulletController bullet)
    {
        //弾非表示
        bullet.gameObject.SetActive(false);
        //Queueに弾を格納
        BulletQueue.Enqueue(bullet);
    }
    
}
