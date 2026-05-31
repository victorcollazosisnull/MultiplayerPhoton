using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class UIManager : MonoBehaviourPunCallbacks
{
    public enum PanelType
    {
        Connecting, Lobby, RoomSettings, Profile, RoomsList, CharacterSelector
    }

    [System.Serializable]
    public struct PanelEntry
    {
        public PanelType type;
        public GameObject panelObject;
    }

    [SerializeField] private List<PanelEntry> allPanels;

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
            OpenPanel(PanelType.Lobby);
        else
            OpenPanel(PanelType.Connecting);
    }

    // SOLO abrir panel, sin llamar JoinLobby
    public override void OnJoinedLobby()
    {
        OpenPanel(PanelType.Lobby);
    }

    // QUITADO: OnConnectedToMaster — PhotonRoomManager lo maneja

    public void OpenPanel(PanelType typeToOpen)
    {
        foreach (var entry in allPanels)
            entry.panelObject.SetActive(entry.type == typeToOpen);
    }

    public void ReturnToMain() => OpenPanel(PanelType.Lobby);
    public void ShowLobby() => OpenPanel(PanelType.Lobby);
    public void ShowProfile() => OpenPanel(PanelType.Profile);
    public void ShowRoomSettings() => OpenPanel(PanelType.RoomSettings);
    public void ShowRoomsList() => OpenPanel(PanelType.RoomsList);
    public void ShowCharacterSelector() => OpenPanel(PanelType.CharacterSelector);
}