using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyCharacterMovement : MonoBehaviourPun
{
    private CharacterController controller;
    private PlayerInputsController inputs;
    private Vector2 moveInput;

    [Header("Move Values")]
    [SerializeField] private float speed = 5f;
    private Transform cameraTransform;

    [Header("Jump Values")]
    [Header("Physics")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -15f; 
    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputs = GetComponent<PlayerInputsController>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void OnEnable()
    {
        if (photonView.IsMine && inputs != null)
            inputs.onMoveInput += (val) => moveInput = val;
        inputs.onJumpInput += HandleJump;
    }

    private void OnDisable()
    {
        inputs.onJumpInput -= HandleJump;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        HandleMovement();
        ApplyGravity();
        HandleRotation(); 
    }

    private void HandleMovement()
    {
        if (cameraTransform == null) return;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime); 
    }

    private void HandleJump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void HandleRotation()
    {
        if (cameraTransform == null) return;

        Vector3 targetRotation = cameraTransform.forward;
        targetRotation.y = 0;

        if (targetRotation != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(targetRotation);
        }
    }
}