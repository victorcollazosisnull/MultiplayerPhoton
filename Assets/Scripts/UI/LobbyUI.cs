using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;

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

        bool inLobby = PhotonNetwork.InLobby;
        createRoomButton.interactable = inLobby;
        joinRoomButton.interactable = inLobby;

        if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public void SetPlayerName()
    {
        PhotonNetwork.NickName = playerNameInput.text;
        print("Nombre seteado: " + PhotonNetwork.NickName);
    }

    public void CreateRoom()
    {
        if (!PhotonNetwork.InLobby)
        {
            Debug.LogWarning("Aún no estás en el lobby, espera...");
            return;
        }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)playerCountSelector.GetValue();
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "IsPlaying", false } };
        options.CustomRoomPropertiesForLobby = new string[] { "IsPlaying" };
        PhotonNetwork.CreateRoom(roomNameInput.text, options);
    }

    public void JoinRoom()
    {
        if (!PhotonNetwork.InLobby)
        {
            Debug.LogWarning("Aún no estás en el lobby, espera...");
            return;
        }
        PhotonNetwork.JoinRoom(roomNameInput.text);
    }

    public void SelectCharacter(int index)
    {
        PlayerPrefs.SetInt("CharacterIndex", index);
        print("Personaje seleccionado: " + index);
    }

    public override void OnJoinedLobby()
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
    }

    public override void OnLeftRoom()
    {
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
    }

    public override void OnJoinedRoom()
    {
        print("Entraste a: " + PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("ArenaScene");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList) cachedRoomList.Remove(info.Name);
            else cachedRoomList[info.Name] = info;
        }
        foreach (GameObject item in allRoomItems) Destroy(item);
        allRoomItems.Clear();
        foreach (RoomInfo room in cachedRoomList.Values)
        {
            GameObject newRoom = Instantiate(roomItemPrefab, contentParent);
            newRoom.GetComponent<RoomItem>().SetRoomInfo(room.Name, room.PlayerCount, room.MaxPlayers);
            allRoomItems.Add(newRoom);
        }
    }
}