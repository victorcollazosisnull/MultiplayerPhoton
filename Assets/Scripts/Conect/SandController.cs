using UnityEngine;
using Photon.Pun;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SandController : MonoBehaviourPunCallbacks
{
    public static SandController Instance;
    public Transform[] spawnPoints;
    public PlayerFactory factory;
    [SerializeField] private GameObject panelMuerte;
    public int CurrentSkinIndex { get; private set; }

    private bool hasSpawned = false; 

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("[SandController] Start | InRoom="
            + PhotonNetwork.InRoom
            + " | hasSpawned="
            + hasSpawned);

        if (PhotonNetwork.IsConnectedAndReady
            && PhotonNetwork.InRoom
            && !hasSpawned)
        {
            SpawnPlayer();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("[SandController] OnJoinedRoom disparado! hasSpawned=" + hasSpawned);
        if (!hasSpawned) SpawnPlayer();
    }

    void SpawnPlayer()
    {
        hasSpawned = true;
        CurrentSkinIndex = PlayerPrefs.GetInt("CharacterIndex", 0);

        if (PhotonNetwork.LocalPlayer.TagObject is GameObject oldPlayer)
        {
            if (oldPlayer != null && oldPlayer.activeInHierarchy)
            {
                Debug.Log("Player ya existe, no spawnear.");
                return;
            }
            PhotonNetwork.LocalPlayer.TagObject = null;
        }

        if (factory == null) { Debug.LogError("No factory!"); return; }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        GameObject player = factory.CreatePlayer(spawnPoints[randomIndex].position, Quaternion.identity);
        PhotonNetwork.LocalPlayer.TagObject = player;
        Debug.Log("Player spawneado correctamente.");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        otherPlayer.TagObject = null;
    }

    public IEnumerator RespawnRoutine()
    {
        hasSpawned = false; 
        if (panelMuerte != null) panelMuerte.SetActive(true);
        yield return new WaitForSeconds(3f);
        if (panelMuerte != null) panelMuerte.SetActive(false);
        SpawnPlayer();
    }
}