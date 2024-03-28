using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector GunSelector;

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float dashDistance = 5f;

    private CharacterController characterController;

    private bool isDashing = false;

    // Start is called before the first frame update
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovementInput();
        HandleRotationInput();
        HandleShootInput();
        HandleDashInput();
    }

    private void HandleMovementInput()
    {
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        var moveDirection = new Vector3(moveX, 0, moveY);
        characterController.Move(moveDirection * (moveSpeed * Time.deltaTime));
    }

    private void HandleRotationInput()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            var lookAtPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookAtPosition);
        }
    }

    private void HandleShootInput()
    {
        if (Input.GetButton("Fire1"))
        {
            GunSelector.ActiveGun.Shoot();
        }
    }


    private void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        // TODO: use physics
        isDashing = true;

        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");
        var dashDirection = new Vector3(moveX, 0, moveY).normalized;

        var startPosition = transform.position;
        var dashTarget = startPosition + dashDirection * dashDistance;

        while (Vector3.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, moveSpeed * Time.deltaTime * 5f);
            yield return null;
        }

        isDashing = false;
    }
}
