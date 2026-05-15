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

    private float yaw;
    private float pitch;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inputs = GetComponentInParent<PlayerInputsController>();
        }
        else
        {
            this.enabled = false;
            return;
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

        Vector3 desiredPosition = target.position - transform.forward * distance + Vector3.up * 1.5f;
        transform.position = desiredPosition;
    }
}