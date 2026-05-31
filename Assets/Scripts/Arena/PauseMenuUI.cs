using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("[PauseMenuUI] QuitGame");

        GameNetworkManager.Instance.LeaveRoom();
    }
}