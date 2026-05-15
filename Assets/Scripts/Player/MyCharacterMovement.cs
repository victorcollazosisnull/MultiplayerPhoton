using UnityEngine;
using Photon.Pun;

public class MyCharacterMovement : MonoBehaviourPun
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller; 
    private PlayerInputsController inputs;
    private Vector2 moveInput;

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); 
        inputs = GetComponent<PlayerInputsController>();
    }

    private void OnEnable()
    {
        if (photonView.IsMine && inputs != null)
            inputs.onMoveInput += (val) => moveInput = val;
    }

    private void Update() 
    {
        if (!photonView.IsMine) return;

        HandleMovement();
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

            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }
}