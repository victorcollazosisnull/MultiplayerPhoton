using UnityEngine;
using Photon.Pun;

public class SandController : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;
    public PlayerFactory factory;

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (PhotonNetwork.LocalPlayer.TagObject != null) return;

        if (factory == null)
        {
            Debug.LogError("¡No has asignado la Factory en el SandController!");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Vector3 pos = spawnPoints[randomIndex].position;

        GameObject player = factory.CreatePlayer(pos, Quaternion.identity);

        PhotonNetwork.LocalPlayer.TagObject = player;
        Debug.Log("Fábrica creó al personaje correctamente.");
    }
}