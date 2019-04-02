/**
 * BaseEntity.cs
 * Created by: Pedro Borges
 * Created on: 30/03/19 (dd/mm/yy)
 */

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
        if(health > damage)
        {
            health -= damage;
            if (gameObject.GetComponent<EnemyPatrol>())
                gameObject.GetComponent<EnemyPatrol>().DamageFeedback(true);
            if (gameObject.GetComponent<PlayerController>())
                gameObject.GetComponent<PlayerController>().Hit(true);
        }
        else
        {
            health = 0;
            if (gameObject.GetComponent<EnemyPatrol>())
                gameObject.GetComponent<EnemyPatrol>().DamageFeedback(true);
            if (gameObject.GetComponent<PlayerController>())
            {
                gameObject.GetComponent<PlayerController>().animator.SetBool("isDead", true);
                gameObject.GetComponent<PlayerController>().inputEnabled = false;
            }
                
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            
        }
        
    }

    void Update()
    {


    }
}