using UnityEngine;
using Photon.Pun;

public class WeaponAim : MonoBehaviourPun
{
    private Camera mainCam;
    [SerializeField] private float aimSpeed = 20f;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate() 
    {
        if (!photonView.IsMine) return;

        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            targetPoint = hit.point; 
        }
        else
        {
            targetPoint = ray.GetPoint(100f); 
        }

        Vector3 direction = targetPoint - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * aimSpeed);
    }
}