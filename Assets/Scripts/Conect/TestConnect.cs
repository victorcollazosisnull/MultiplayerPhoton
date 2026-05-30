using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class TestConnect : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;

        if (!PhotonNetwork.IsConnected)
        {
            print("Connecting to server...");
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            print("Ya conectado, saltando ConnectUsingSettings.");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected: " + cause.ToString());
    }
}