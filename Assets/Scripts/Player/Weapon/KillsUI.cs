using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using ExitGames.Client.Photon;

public class KillsUI : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI killsText;

    private void Start()
    {
        if (!photonView.IsMine) return;

        GameObject hud = GameObject.Find("MainHUD");
        if (hud != null)
        {
            Transform killsTransform = hud.transform.Find("KillsText");
            if (killsTransform != null)
                killsText = killsTransform.GetComponent<TextMeshProUGUI>();
        }

        UpdateKillsUI();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!photonView.IsMine) return;
        if (targetPlayer != PhotonNetwork.LocalPlayer) return;

        if (changedProps.ContainsKey("Kills"))
            UpdateKillsUI();
    }

    private void UpdateKillsUI()
    {
        if (killsText == null) return;
        int kills = 0;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Kills", out object k))
            kills = (int)k;
        killsText.text = "Kills: " + kills;
    }
}