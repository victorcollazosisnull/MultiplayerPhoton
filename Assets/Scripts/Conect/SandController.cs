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
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
    }

    public override void OnJoinedRoom()
    {
        CurrentSkinIndex = PlayerPrefs.GetInt("CharacterIndex", 0);    

        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (PhotonNetwork.LocalPlayer.TagObject != null)
        {
            GameObject oldPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
            if (oldPlayer == null) 
            {
                PhotonNetwork.LocalPlayer.TagObject = null;
            }
            else
            {
                return; 
            }
        }

        if (factory == null)
        {
            Debug.LogError("¡No has asignado la Factory en el SandController!");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Vector3 pos = spawnPoints[randomIndex].position;

        GameObject player = factory.CreatePlayer(pos, Quaternion.identity);

        PhotonNetwork.LocalPlayer.TagObject = player;
        Debug.Log("Fábrica creó al personaje correctamente.");
    }

    public IEnumerator RespawnRoutine()
    {
        if (panelMuerte != null) panelMuerte.SetActive(true);

        yield return new WaitForSeconds(3f); 

        if (panelMuerte != null) panelMuerte.SetActive(false);

        SpawnPlayer();
    }
}