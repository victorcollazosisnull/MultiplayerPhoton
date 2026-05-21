using UnityEngine;
using Photon.Pun;

public class PlayerShooting : MonoBehaviourPun
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f; 
    [SerializeField] private ParticleSystem flashEffect;

    private PlayerInputsController inputs;

    private void Awake()
    {
        inputs = GetComponent<PlayerInputsController>();
    }

    private void OnEnable()
    {
        if (photonView.IsMine && inputs != null)
            inputs.onShootInput += RequestShoot;
    }

    private void OnDisable()
    {
        if (photonView.IsMine && inputs != null)
            inputs.onShootInput -= RequestShoot;
    }

    void RequestShoot()
    {
        photonView.RPC("RPC_Shoot", RpcTarget.All);
    }

    [PunRPC]
    void RPC_Shoot()
    {
        if (flashEffect != null) flashEffect.Play();

        if (photonView.IsMine)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                PhotonView targetPV = hit.transform.GetComponent<PhotonView>();
                if (targetPV != null && !targetPV.IsMine)
                {
                    targetPV.RPC("TakeDamage", RpcTarget.All, damage);
                }
            }
        }
    }
}