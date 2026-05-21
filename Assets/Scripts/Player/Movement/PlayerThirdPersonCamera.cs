using UnityEngine;
using Photon.Pun;

public class PlayerThirdPersonCamera : MonoBehaviourPun
{
    [Header("References")]
    private PlayerInputsController inputs;
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private Vector2 sensitivity = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector2 pitchClamp = new Vector2(-40f, 80f);
    [SerializeField] private float distance = 2.5f;
    [SerializeField] private Vector3 targetOffset = new Vector3(0.5f, 1.5f, 0); 

    [Header("Collision")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius = 0.2f;
    [SerializeField] private float minDistance = 0.5f;

    private float yaw;
    private float pitch;
    private float currentDistance;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inputs = GetComponentInParent<PlayerInputsController>();
            currentDistance = distance;
        }
        else
        {
            this.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (photonView.IsMine && inputs != null)
            inputs.onLookInput += OnLookInput;
    }

    private void OnDisable()
    {
        if (photonView.IsMine && inputs != null)
            inputs.onLookInput -= OnLookInput;
    }

    private void OnLookInput(Vector2 lookDelta)
    {
        yaw += lookDelta.x * sensitivity.x;
        pitch -= lookDelta.y * sensitivity.y;
        pitch = Mathf.Clamp(pitch, pitchClamp.x, pitchClamp.y);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 headPosition = target.position + (transform.right * targetOffset.x) + (transform.up * targetOffset.y);

        Vector3 direction = -transform.forward;
        RaycastHit hit;

        if (Physics.SphereCast(headPosition, collisionRadius, direction, out hit, distance, collisionMask))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, distance);
        }
        else
        {
            currentDistance = Mathf.Lerp(currentDistance, distance, Time.deltaTime * 10f);
        }

        transform.position = headPosition + (direction * currentDistance);
    }
}