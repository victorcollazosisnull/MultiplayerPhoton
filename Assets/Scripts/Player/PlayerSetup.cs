using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPun
{
    [Header("Local Assets (Solo para mi)")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private AudioListener audioListener;

    [Header("Scripts a desactivar en Clones")]
    [SerializeField] private MonoBehaviour[] localScripts;

    void Awake()
    {
        if (photonView.IsMine)
        {
            if (playerCamera != null) playerCamera.SetActive(true);
            if (audioListener != null) audioListener.enabled = true;

            foreach (var script in localScripts)
            {
                script.enabled = true;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            if (playerCamera != null) playerCamera.SetActive(false);
            if (audioListener != null) audioListener.enabled = false;

            foreach (var script in localScripts)
            {
                script.enabled = false;
            }

            if (TryGetComponent(out CharacterController cc)) cc.enabled = false;
        }
    }
}