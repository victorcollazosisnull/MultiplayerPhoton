using UnityEngine;
using Photon.Pun;
using TMPro; 

public class ArenaManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text hintText; 
    private bool canStart = false;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        UpdateHintText();
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        canStart = PhotonNetwork.CurrentRoom.PlayerCount >= 2;

        if (canStart)
        {
            if (Input.GetKeyDown(KeyCode.F)) 
            {
                StartMatch();
            }
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdateHintText();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdateHintText();
    }

    void UpdateHintText()
    {
        if (hintText == null) return;

        if (!PhotonNetwork.IsMasterClient)
        {
            hintText.text = "Esperando que el Host inicie...";
            return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            hintText.text = "Esperando más jugadores... (" + PhotonNetwork.CurrentRoom.PlayerCount + "/2)";
        }
        else
        {
            hintText.text = "¡LISTO! Presiona [F] para iniciar la batalla";
        }
    }

    public void StartMatch()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("Game");
    }
}