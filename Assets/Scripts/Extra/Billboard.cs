using UnityEngine;
using Photon.Pun;

public class Billboard : MonoBehaviourPun
{
    [SerializeField] private Transform camTransform;

    void LateUpdate()
    {
        if (camTransform == null)
        {
            if (Camera.main != null)
                camTransform = Camera.main.transform;
            else
                return;
        }

        transform.rotation = camTransform.rotation;
    }
}