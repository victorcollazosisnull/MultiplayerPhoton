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
                    PlayerHealth victimHealth = targetPV.GetComponent<PlayerHealth>();

                    if (victimHealth != null)
                    {
                        if (victimHealth.GetCurrentHealth() <= damage)
                        {
                            RegistrarKill(PhotonNetwork.LocalPlayer.NickName, targetPV.Owner.NickName);
                        }

                        targetPV.RPC("TakeDamage", RpcTarget.All, damage, PhotonNetwork.LocalPlayer.ActorNumber);
                    }
                }
            }
        }
    }
    void RegistrarKill(string atacante, string victima)
    {
        Debug.Log("[RegistrarKill] " + atacante + " mató a " + victima);
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Kills"))
        {
            int currentKills = (int)PhotonNetwork.LocalPlayer.CustomProperties["Kills"];
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("Kills", currentKills + 1);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable { { "Kills", 1 } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        photonView.RPC("RPC_KillMessage", RpcTarget.All, atacante, victima);
    }

    [PunRPC]
    void RPC_KillMessage(string atacante, string victima)
    {
        Debug.Log("<color=red>" + atacante + " eliminó a " + victima + "</color>");
    }
}