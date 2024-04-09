using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerGunSelector GunSelector;

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float dashDistance = 5f;

    [SerializeField] private float gravity = 20f;


    private CharacterController characterController;

    private bool isDashing = false;
    private bool isReloading = false;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleRotationInput();
        HandleShootInput();
        HandleDashInput();

        // fara mai zboara :(
        ApplyGravity();
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

        int layerMask = ~LayerMask.GetMask("Wall Enclosure");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            var lookAtPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookAtPosition);
        }
    }

    private void HandleShootInput()
    {
        bool wantsToShoot = Input.GetButton("Fire1");
        bool isMoving = characterController.velocity.magnitude > 0.1f;

        GunSelector.ActiveGun.Tick(
            !isReloading
            && Application.isFocused && wantsToShoot
            && GunSelector.ActiveGun != null, isMoving
        );

        if (ShouldManualReload())
        {
            isReloading = true;
            GunSelector.ActiveGun.gunAmmoConfig.Reload();
            isReloading = false;
        }
        
    }

    private bool ShouldManualReload()
    {
        return !isReloading
               && Input.GetKeyDown(KeyCode.R)
               && GunSelector.ActiveGun.CanReload();
    }


    private void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing) StartCoroutine(Dash());
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            characterController.Move(Vector3.down * gravity * Time.deltaTime);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDir * 4.0f;
    }


    private IEnumerator Dash()
    {
        var startPosition = transform.position;

        var dashDirection = characterController.velocity.normalized;
        var dashTarget = startPosition + dashDirection * dashDistance;

        bool collided = false;
        RaycastHit hit;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, dashTarget, elapsedTime);

            if (Physics.Raycast(transform.position, dashDirection, out hit, 0.5f))
            {
                collided = true;
                break;
            }

            elapsedTime += Time.deltaTime * (moveSpeed * 5f) / dashDistance;

            yield return null;
        }

        if (!collided)
            transform.position = dashTarget;

        isDashing = false;
    }
}