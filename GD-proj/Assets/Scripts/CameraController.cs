using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float moveSpeed = 4f;

    void Start()
    {
        
    }

    void Update()
    {
        MoveCamera();
    }


    void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * Time.deltaTime);
    }
}
