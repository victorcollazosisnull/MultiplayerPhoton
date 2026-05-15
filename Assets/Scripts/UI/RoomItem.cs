using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomItem : MonoBehaviour
{
    public TMP_Text roomNameText;
    public string roomName;

    public void SetRoomInfo(string _name, int currentPlayers, int maxPlayers)
    {
        roomName = _name;
        roomNameText.text = _name + " (" + currentPlayers + "/" + maxPlayers + ")";
    }

    public void OnClickJoin()
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}