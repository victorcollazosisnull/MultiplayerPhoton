using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private TMP_Text roomListText;

    [Header("Selector")]
    [SerializeField] private PlayerCountSelector playerCountSelector;
    public void SetPlayerName()
    {
        PhotonNetwork.NickName = playerNameInput.text;
        print("Nombre seteado: " + PhotonNetwork.NickName);
    }

    public void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)playerCountSelector.GetValue();

        PhotonNetwork.CreateRoom(roomNameInput.text, options);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNameInput.text);
    }
    public void SelectCharacter(int index)
    {
        PlayerPrefs.SetInt("CharacterIndex", index);
        print("Personaje seleccionado: " + index);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomListText.text = "";

        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo room = roomList[i];

            if (room.RemovedFromList) continue;

            roomListText.text += room.Name + " (" + room.PlayerCount + "/" + room.MaxPlayers + ")\n";
        }
    }

    public override void OnJoinedRoom()
    {
        print("Entraste a: " + PhotonNetwork.CurrentRoom.Name);
    }
}
