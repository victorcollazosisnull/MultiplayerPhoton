using Unity.Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerAimHandler : MonoBehaviourPun
{
    [SerializeField] private CinemachineCamera normalCam;
    [SerializeField] private CinemachineCamera aimCam;

    private PlayerInputsController inputs;

    private void Awake()
    {
        if (!photonView.IsMine) return;
        inputs = GetComponent<PlayerInputsController>();
    }

    private void OnEnable()
    {
        if (photonView.IsMine && inputs != null)
            inputs.onAimInput += ToggleAim;
    }

    private void OnDisable()
    {
        if (photonView.IsMine && inputs != null)
            inputs.onAimInput -= ToggleAim;
    }

    void ToggleAim(bool isAiming)
    {
        if (isAiming)
        {
            normalCam.Priority = 10;
            aimCam.Priority = 20;
        }
        else
        {
            normalCam.Priority = 20;
            aimCam.Priority = 10;
        }
    }
}