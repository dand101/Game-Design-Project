using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float dashDistance = 5f;


    private bool isDashing = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleRotationInput();
        HandleShootInput();
        HandleDashInput();
    }

    void HandleMovementInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0, moveY);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    void HandleRotationInput()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookAtPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookAtPosition);
        }
    }

    void HandleShootInput()
    {
        if (Input.GetButton("Fire1"))
        {
            PlayerGun.instance.Shoot();
        }
    }


    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 dashDirection = new Vector3(moveX, 0, moveY).normalized;

        Vector3 startPosition = transform.position;
        Vector3 dashTarget = startPosition + dashDirection * dashDistance;

        while (Vector3.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, moveSpeed * Time.deltaTime * 5f);
            yield return null;
        }

        isDashing = false;
    }
}
