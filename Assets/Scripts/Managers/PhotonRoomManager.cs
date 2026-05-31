//// PhotonRoomManager.cs
//using UnityEngine;
//using Photon.Pun;

//public class PhotonRoomManager : MonoBehaviourPunCallbacks
//{
//    public static PhotonRoomManager Instance;
//    public static bool IsReadyForLobby = false; // flag simple
//    private bool pendingLoginLoad = false;

//    private void Awake()
//    {
//        if (Instance != null) { Destroy(gameObject); return; }
//        Instance = this;
//        DontDestroyOnLoad(gameObject);
//    }

//    public override void OnLeftRoom()
//    {
//        Debug.Log("[PhotonRoomManager] OnLeftRoom - State=" + PhotonNetwork.NetworkClientState);
//        IsReadyForLobby = false;
//        PhotonNetwork.AutomaticallySyncScene = false;
//        // Cargar Login aquí, luego JoinLobby desde LobbyUI.Start()
//        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
//    }

//    public override void OnJoinedLobby()
//    {
//        Debug.Log("[PhotonRoomManager] OnJoinedLobby!");
//        IsReadyForLobby = true;
//        PhotonNetwork.AutomaticallySyncScene = true;
//        LobbyUI lobbyUI = Object.FindFirstObjectByType<LobbyUI>();
//        if (lobbyUI != null) lobbyUI.EnableButtons();
//    }
//    public override void OnConnectedToMaster()
//    {
//        Debug.Log("[PhotonRoomManager] OnConnectedToMaster");
//        PhotonNetwork.JoinLobby();
//    }
//}