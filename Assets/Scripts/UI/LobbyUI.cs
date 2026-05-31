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

    [Header("Room List")]
    [SerializeField] private GameObject roomItemPrefab;
    [SerializeField] private Transform contentParent;

    private List<GameObject> allRoomItems = new();
    private Dictionary<string, RoomInfo> cachedRoomList = new();

    [Header("Selector")]
    [SerializeField] private PlayerCountSelector playerCountSelector;

    void Start()
    {
        Debug.Log(
            $"[LobbyUI] Start | State={GameNetworkManager.Instance.CurrentState}"
        );

        RefreshButtons();
    }

    // ==========================
    // PLAYER NAME
    // ==========================

    public void SetPlayerName()
    {
        PhotonNetwork.NickName =
            playerNameInput.text.Trim();

        Debug.Log(
            "Nombre seteado: "
            + PhotonNetwork.NickName
        );
    }

    // ==========================
    // CREATE ROOM
    // ==========================

    public void CreateRoom()
    {
        string roomName =
            roomNameInput.text.Trim();

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning(
                "Escribe un nombre de sala"
            );

            return;
        }

        GameNetworkManager.Instance.CreateRoom(
            roomName,
            (byte)playerCountSelector.GetValue()
        );
    }

    // ==========================
    // CHARACTER
    // ==========================

    public void SelectCharacter(int index)
    {
        PlayerPrefs.SetInt(
            "CharacterIndex",
            index
        );

        Debug.Log(
            "Personaje seleccionado: "
            + index
        );
    }

    // ==========================
    // UI STATE
    // ==========================

    void Update()
    {
        RefreshButtons();
    }

    void RefreshButtons()
    {
        bool canInteract =
            GameNetworkManager.Instance.CurrentState
            == GameNetworkManager.NetworkState.InLobby;

        createRoomButton.interactable =
            canInteract;
    }

    // ==========================
    // ROOM LIST
    // ==========================

    public override void OnRoomListUpdate(
        List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] =
                    info;
            }
        }

        foreach (GameObject item in allRoomItems)
        {
            Destroy(item);
        }

        allRoomItems.Clear();

        foreach (RoomInfo room
                 in cachedRoomList.Values)
        {
            GameObject newRoom =
                Instantiate(
                    roomItemPrefab,
                    contentParent
                );

            newRoom.GetComponent<RoomItem>()
                .SetRoomInfo(
                    room.Name,
                    room.PlayerCount,
                    room.MaxPlayers
                );

            allRoomItems.Add(newRoom);
        }
    }
}