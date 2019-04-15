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

    void Awake()
    {
        maxHealth = health;
    }

    public void TakeDamage(float damage)
    {
        PlayerController player = gameObject.GetComponent<PlayerController>();
        EnemyPatrol enemy = gameObject.GetComponent<EnemyPatrol>();
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
                    player.rb2D.velocity = new Vector2(0, -10);
                }
                StartCoroutine(WaitForLanding(player));
            }
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    IEnumerator WaitForLanding(PlayerController p)
    {
        yield return new WaitUntil(() => p.isGrounded);
        p.animator.SetBool("isDead", true);
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
    }

}