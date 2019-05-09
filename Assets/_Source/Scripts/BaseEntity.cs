/**
 * BaseEntity.cs
 * Created by: Pedro Borges
 * Created on: 30/03/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public float health = 100;
    float maxHealth;
    PlayerController player;
    EnemyPatrol enemy;

    void Awake()
    {
        maxHealth = health;
        player = gameObject.GetComponent<PlayerController>();
        enemy = gameObject.GetComponent<EnemyPatrol>();
    }

    public void TakeDamage(float damage)
    {
        
        if (health > damage)
        {
            health -= damage;
            if (enemy)
                enemy.DamageFeedback(true);
            if (player)
                player.Hit();
        }
        else
        {
            health = 0;
            if (enemy)
            {
                enemy.animator.SetBool("isDead", true);
                this.DelayedCall(2, () => DestroyThis());
                gameObject.GetComponent<Collider2D>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().simulated = false;
            }
            if (player)
            {
                player.inputEnabled = false;
                player.rb2D.velocity = Vector2.zero;
                if(player.animator.GetBool("isGrabbing"))
                {
                    player.wallJumpDelay = 50;
                    player.Detach(false);
                }
                StartCoroutine(WaitForLanding());
            }
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void ReenablePlayer()
    {
        player.animator.SetBool("isDead", false);
        player.animator.enabled = true;
        player.inputEnabled = true;
        player.wallJumpDelay = 1.5f;
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<Rigidbody2D>().simulated = true;
    }

    IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => player.isGrounded);
        player.animator.SetBool("isDead", true);
        player.inputEnabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        //player.animator.enabled = false;
        yield return new WaitForSeconds(2);
        FindObjectOfType<Main>().ResetToCheckpoint();
    }

}