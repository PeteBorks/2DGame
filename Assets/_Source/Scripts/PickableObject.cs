/**
 * PickableObject.cs
 * Created by: Pedro Borges
 * Created on: 10/04/19 (dd/mm/yy)
 */
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public int value = 1;
    public Light l;
    public AudioClip sound;
    MeshRenderer meshR;

    private void Start()
    {
        meshR = GetComponent<MeshRenderer>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Stellar")) && meshR.enabled)
        {
            AudioManager.instance.PlaySound("pickup", transform.position, 0.4f);
            collision.gameObject.GetComponent<CollectableManager>().AddCollectable(value);
            GetComponentInChildren<ParticleSystem>().Play();
            meshR.enabled = false;
            Destroy(l.gameObject);
            Destroy(gameObject,2);

        }
    }
}
