using UnityEngine;
using Photon.Pun;

public class SandController : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;

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

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Vector3 pos = spawnPoints[randomIndex].position;

        GameObject player = PhotonNetwork.Instantiate("MyCharacter", pos, Quaternion.identity);

        PhotonNetwork.LocalPlayer.TagObject = player;

        Debug.Log("Personaje instanciado para: " + PhotonNetwork.NickName);
    }
}