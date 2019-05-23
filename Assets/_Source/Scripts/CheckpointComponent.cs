/**
 * CheckpointComponent.cs
 * Created by: Pedro Borges
 * Created on: 03/05/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

public class CheckpointComponent : MonoBehaviour
{
    Animator animator;
    bool on;
    public Transform loadLocation;
    public float playerHealth;
    public int collectables;
    public Color ambientColor;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() && !on)
        {
            SaveGame(collision.GetComponent<PlayerController>());
            FindObjectOfType<Main>().currentCheckpoint = this;
            AudioManager.instance.PlaySound("checkpoint", transform.position, 0.5f);
        }
    }

    void SaveGame(PlayerController p)
    {
        playerHealth = p.health;
        collectables = p.GetComponent<CollectableManager>().collectables;
        ambientColor = RenderSettings.ambientLight;
        animator.enabled = true;
        on = true;
        StartCoroutine(RandomGlich());
    }

    IEnumerator RandomGlich()
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 5));
        animator.SetTrigger("loop");
        StartCoroutine(RandomGlich());
    }

    public void StartSound()
    {
        GetComponentInChildren<AudioSource>().enabled = true;
    }


}