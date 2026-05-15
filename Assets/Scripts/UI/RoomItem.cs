using UnityEngine;
using UnityEngine.UI;
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

        Button btn = GetComponentInChildren<Button>();

        if (currentPlayers >= maxPlayers)
        {
            btn.interactable = false; 
                                  
            btn.GetComponentInChildren<TMP_Text>().text = "FULL";
        }
        else
        {
            btn.interactable = true;
            btn.GetComponentInChildren<TMP_Text>().text = "JOIN";
        }
    }

    public void OnClickJoin()
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}