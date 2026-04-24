using UnityEngine;
using Photon.Pun;

public class UIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject connectingPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject selectorCharacterPanel;

    private void Start()
    {
        connectingPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        connectingPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }
    public void ShowPanelSelector() 
    {
        selectorCharacterPanel.SetActive(true);
    }
    public void ExitPanelSelector() 
    {
        selectorCharacterPanel.SetActive(false);
    }
}