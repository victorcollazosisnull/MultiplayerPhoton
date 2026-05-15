using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCamTransform;

    void Start()
    {
        if (Camera.main != null)
            mainCamTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (mainCamTransform == null)
        {
            if (Camera.main != null) mainCamTransform = Camera.main.transform;
            return;
        }
        transform.LookAt(transform.position + mainCamTransform.forward);
    }
}