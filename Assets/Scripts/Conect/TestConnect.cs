using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
public class TestConnect : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        print("Connecting to server...");

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.ConnectToRegion("sa");
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to server.");
        PhotonNetwork.JoinLobby();
        print(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server for reason: " + cause.ToString());
    }
    public override void OnJoinedLobby()
    {
        print("Joined Lobby");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print ("Salas disponibles: " +  roomList.Count);    
        for (int i = 0; i < roomList.Count; i++)
        {
            print ("Sala: " + roomList[i].Name + " | Jugadores: " + roomList[i].PlayerCount);
        }
    }
    public void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.CreateRoom("Sala_" + Random.Range(0, 1000), options);
    }
    public override void OnCreatedRoom()
    {
        print("Sala creada correctamente");
    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinedRoom()
    {
        print("Te uniste a la sala: " + PhotonNetwork.CurrentRoom.Name);
    }
}