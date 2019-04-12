/**
 * PickableObject.cs
 * Created by: Pedro Borges
 * Created on: 10/04/19 (dd/mm/yy)
 */
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public Light l;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Stellar"))
        {
            collision.gameObject.GetComponent<CollectableManager>().AddCollectable(1);
            Destroy(l.gameObject);
            Destroy(gameObject);

        }
    }
}
