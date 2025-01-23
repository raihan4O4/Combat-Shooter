using Unity.Android.Gradle;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player movement")]
    public float playerSpeed = 2f;
    public float currentPlayerSpeed;
    public float playerSprint = 3f;
    public float currentPlayerSprint;

    [Header("Player camera")]
    public Transform playerCameraTransform;

    [Header("Player Animator and gravity")]
    public CharacterController characterController;
    const float gravity = -9.8f;
    public Animator playerAnimator;     

    [Header("Player Jump and Velocity")]
    public float jumpRange = 1f;
    float turnVelocity;
    public float turnTime;
    public float surfaceDistance = 0.4f;
    Vector3 velocity;
    bool isOnSurface;
    public Transform surfaceCheck;
    public LayerMask surfaceMask;

    [Header("Player Health")]
    public float playerHealth = 1000f;
    private float playerCurrentHealth;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCurrentHealth = playerHealth;
    }
    void Update()
    {
        isOnSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if (isOnSurface && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        PlayerMove();
        PlayerJump();
        PlayerSprint();
    }

    public void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && isOnSurface)
        {
            playerAnimator.SetBool("Walk", false);
            playerAnimator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            playerAnimator.ResetTrigger("Jump");
        }
    }

    public void PlayerMove()
    {
        float horizontalAxiz = Input.GetAxis("Horizontal");
        float verticalAxiz = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalAxiz, 0f, verticalAxiz).normalized;

        if (direction.magnitude >= 0.1f)
        {
            playerAnimator.SetBool("Walk", true);
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("Run", false);
            playerAnimator.SetTrigger("Jump");
            playerAnimator.SetBool("AimWalk", false);
            playerAnimator.SetBool("IdleAim", false);


            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            characterController.Move(playerSpeed * Time.deltaTime * moveDirection.normalized);
            currentPlayerSpeed = playerSpeed;
        }
        else
        {
            playerAnimator.SetBool("Idle", true);
            playerAnimator.SetTrigger("Jump");
            playerAnimator.SetBool("AimWalk", false);
            playerAnimator.SetBool("Run", false);
            playerAnimator.SetBool("Walk", false);
            currentPlayerSpeed = 0f;
        }
    }

    public void PlayerSprint()
    {
        if (Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && isOnSurface)
        {
            float horizontalAxiz = Input.GetAxis("Horizontal");
            float verticalAxiz = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontalAxiz, 0f, verticalAxiz).normalized;

            if (direction.magnitude >= 0.1f)
            {
                playerAnimator.SetBool("Run", true);
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("IdleAim", false);
                playerAnimator.SetBool("Walk", false);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCameraTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                characterController.Move(playerSprint * Time.deltaTime * moveDirection.normalized);
                currentPlayerSprint = playerSprint;
            }
            else
            {
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("Walk", false);
                currentPlayerSprint = 0f;
            }
        }
    }

    public void PlayerDamage(float damage)
    {
        playerCurrentHealth -= damage;

        if (playerCurrentHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject);
    }
}
