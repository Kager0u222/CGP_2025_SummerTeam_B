using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    //レイヤー
    [SerializeField] private Layers layers;
    //マテリアル
    [SerializeField] private Material[] materials;
    //チェックポイント通った時のSE
    [SerializeField] private AudioClip checkPointSE;
    //見た目用オブジェクトのMeshRenderer
    [SerializeField] private MeshRenderer magicCircleRenderer;


    void Update()
    {
        //マテリアル変更
        if (GameMasterController.currentCheckPoint == transform.position)
            magicCircleRenderer.material = materials[0];
        else
            magicCircleRenderer.material = materials[1];
    }
    private void OnTriggerStay(Collider other)
    {
        //プレイヤーと接触かつチェックポイント未更新なら
        if (other.gameObject.layer == layers.PlayerLayer && GameMasterController.currentCheckPoint != transform.position)
        {
            //チェックポイント更新
            GameMasterController.currentCheckPoint = transform.position;
            //SE鳴らす
            other.GetComponentInChildren<PlayerAudio>().PlaySE(checkPointSE);
        }
    }
}
