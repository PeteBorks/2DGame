/**
 * LifeStation.cs
 * Created by: Pedro Borges
 * Created on: 02/05/19 (dd/mm/yy)
 */

using UnityEngine;

public class LifeStation : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    ParticleSystem particle1;
    [SerializeField]
    ParticleSystem particle2;
    SpriteRenderer sprite;
    MeshRenderer mesh;
    Light l;
    GameObject forcefield;

    void Start()
    {
        animator = GetComponent<Animator>();
        mesh = GetComponentInChildren<MeshRenderer>();
        forcefield = GetComponentInChildren<ParticleSystemForceField>().gameObject;
        
        sprite = GetComponentInChildren<SpriteRenderer>();
        l = GetComponentInChildren<Light>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (mesh.enabled && player && player.health < 100)
        {
            if (player.health > 50)
                player.health = 100;
            else
                player.health += 50;
            player.GetComponent<VignetteFeedback>().Life();
            particle1.Stop();
            ParticleSystem.MainModule main = particle1.main;
            main.simulationSpeed = 3.5f;
            ParticleSystem.MainModule main2 = particle2.main;
            main2.loop = false;
            animator.SetTrigger("off");
            mesh.gameObject.SetActive(false);
            forcefield.SetActive(false);
        }
    }

}