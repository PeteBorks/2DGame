using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    Transform targetTransform;
    [SerializeField]
    Vector3 offset;

    Vector3 targetPosition;
    
    void Start()
    {
        targetPosition = targetTransform.position;
        offset = transform.position - targetPosition;
    }

    void Update()
    {
        targetPosition = targetTransform.position;
    }

    void LateUpdate()
    {
        transform.position = targetPosition + offset;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}
