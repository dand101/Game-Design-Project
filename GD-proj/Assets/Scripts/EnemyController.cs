using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    private float moveSpeed = 2f;

    private int health = 10;

    [SerializeField] private GameObject player;

    private CharacterController characterController;

    [SerializeField] private Camera cam;

    [SerializeField] private NavMeshAgent agent;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        health -= 1;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        HandleMovementInput();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        agent.SetDestination(hit.point);
        //    }
        //}
    }

    void HandleMovementInput()
    {
        agent.SetDestination(player.transform.position);

        //Vector3 moveDirection = (player.transform.position - transform.position).normalized;
        //characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
