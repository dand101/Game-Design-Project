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

    private float playerDistanceThreshold = 10f;

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

        if ((player.transform.position - transform.position).magnitude < playerDistanceThreshold)
        {
            agent.isStopped = true;
            Debug.Log("Hit");

            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.transform.position, out hit)) {
                Vector3 reflectVec = Vector3.Reflect(hit.point - transform.position, hit.normal);
                Debug.DrawLine(transform.position, hit.point, Color.red);
                Debug.DrawRay(hit.point, reflectVec, Color.green);
            }
        }

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
