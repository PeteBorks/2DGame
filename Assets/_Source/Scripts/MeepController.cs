using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepController : MonoBehaviour
{

    [SerializeField]
    GameObject target;
    [SerializeField]
    PlayerController pawnController;
    [SerializeField]
    float distanceThreshold = 2.0f;
    public float moveSpeed = 9.0f;
    float crouchSpeed, normalSpeed;

    
    void Start()
    {
        crouchSpeed = moveSpeed / pawnController.crouchModifier;
        normalSpeed = moveSpeed;
    }

    void Update()
    {
    
    }

    void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, target.transform.position) > distanceThreshold)
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed*Time.deltaTime);
    }

    public void OnMasterCrouch()
    {
        if (moveSpeed == normalSpeed)
        {
            moveSpeed = crouchSpeed;
        }
        else
        {
            moveSpeed = normalSpeed;
        }
        
        
    }
}
