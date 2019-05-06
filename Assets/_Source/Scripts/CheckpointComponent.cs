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
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() && !on)
        {
            SaveGame();
            FindObjectOfType<Main>().currentCheckpoint = this;
        }
    }

    void SaveGame()
    {
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
}