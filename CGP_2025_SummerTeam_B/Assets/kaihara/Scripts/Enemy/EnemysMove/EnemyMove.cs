using System.Xml.Serialization;
using UnityEngine;

public abstract class EnemyMove : MonoBehaviour
{
    protected Rigidbody rb;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    public abstract void Move(Layers layers, GameObject player, float magicLength);
}