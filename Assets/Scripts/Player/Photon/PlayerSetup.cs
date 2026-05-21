using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    [Header("Cinemachine (Solo para mi)")]
    [SerializeField] private GameObject virtualCamerasParent;

    [Header("Scripts a desactivar en Clones")]
    [SerializeField] private MonoBehaviour[] localScripts;

    void Start()
    {
        if (photonView.IsMine)
        {
            if (virtualCamerasParent != null) virtualCamerasParent.SetActive(true);

            foreach (var script in localScripts)
            {
                script.enabled = true;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            if (virtualCamerasParent != null) virtualCamerasParent.SetActive(false);

            foreach (var script in localScripts)
            {
                script.enabled = false;
            }

            if (TryGetComponent(out CharacterController cc)) cc.enabled = false;
        }
    }
}