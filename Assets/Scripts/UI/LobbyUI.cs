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

    [Header("Room List")]
    [SerializeField] private GameObject roomItemPrefab; 
    [SerializeField] private Transform contentParent;   
    private List<GameObject> allRoomItems = new List<GameObject>();
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    [Header("Selector")]
    [SerializeField] private PlayerCountSelector playerCountSelector;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void SetPlayerName()
    {
        PhotonNetwork.NickName = playerNameInput.text;
        print("Nombre seteado: " + PhotonNetwork.NickName);
    }

    public void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)playerCountSelector.GetValue();

        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "IsPlaying", false } };
        options.CustomRoomPropertiesForLobby = new string[] { "IsPlaying" };

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
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }

        foreach (GameObject item in allRoomItems)
        {
            Destroy(item);
        }
        allRoomItems.Clear();

        foreach (RoomInfo room in cachedRoomList.Values)
        {
            GameObject newRoom = Instantiate(roomItemPrefab, contentParent);
            RoomItem itemScript = newRoom.GetComponent<RoomItem>();

            itemScript.SetRoomInfo(room.Name, room.PlayerCount, room.MaxPlayers);

            allRoomItems.Add(newRoom);
        }
    }

    public override void OnJoinedRoom()
    {
        print("Entraste a: " + PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("ArenaScene");
        }
    }
}
