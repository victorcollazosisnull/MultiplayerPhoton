using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{
    public static GameNetworkManager Instance;

    public enum NetworkState
    {
        Disconnected,
        Connecting,
        ConnectedToMaster,
        InLobby,
        JoiningRoom,
        InRoom,
        LeavingRoom
    }

    public NetworkState CurrentState;

    [Header("Settings")]
    public string gameVersion = "1.0";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Connect();
    }

    // ======================================
    // CONNECTION
    // ======================================

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
            return;

        CurrentState = NetworkState.Connecting;

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;

        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("Conectando a Photon...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al Master");

        CurrentState = NetworkState.ConnectedToMaster;

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Entraste al Lobby");

        CurrentState = NetworkState.InLobby;
    }

    // ======================================
    // ROOMS
    // ======================================

    public void CreateRoom(string roomName, byte maxPlayers)
    {
        if (CurrentState != NetworkState.InLobby)
        {
            Debug.LogWarning("No estás en lobby");
            return;
        }

        CurrentState = NetworkState.JoiningRoom;

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayers,

            CustomRoomProperties =
                new ExitGames.Client.Photon.Hashtable
                {
                { "IsPlaying", false }
                },

            CustomRoomPropertiesForLobby =
                new string[] { "IsPlaying" }
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions);

        Debug.Log("Creando room: " + roomName);
    }

    public void JoinRoom(string roomName)
    {
        if (CurrentState != NetworkState.InLobby)
        {
            Debug.LogWarning("No estás en lobby");
            return;
        }

        CurrentState = NetworkState.JoiningRoom;

        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Entraste al Room");

        CurrentState = NetworkState.InRoom;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("ArenaScene");
        }
    }

    public void LeaveRoom()
    {
        Debug.Log(
            "[LeaveRoom] InRoom="
            + PhotonNetwork.InRoom
            + " State="
            + PhotonNetwork.NetworkClientState
        );

        if (!PhotonNetwork.InRoom)
        {
            Debug.LogWarning(
                "No estás dentro de un room"
            );
            return;
        }

        CurrentState = NetworkState.LeavingRoom;

        Debug.Log("Saliendo del room...");

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Saliste del Room");

        PhotonNetwork.LocalPlayer.TagObject = null;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CurrentState = NetworkState.ConnectedToMaster;

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        SceneManager.LoadScene("Login");
    }

    // ======================================
    // FAILS
    // ======================================

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error creando room: " + message);

        CurrentState = NetworkState.InLobby;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error uniéndose: " + message);

        CurrentState = NetworkState.InLobby;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Desconectado: " + cause);

        CurrentState = NetworkState.Disconnected;
    }
}