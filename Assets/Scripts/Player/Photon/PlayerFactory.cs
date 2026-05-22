using UnityEngine;
using Photon.Pun;

public class PlayerFactory : MonoBehaviour
{
    [SerializeField] private string playerPrefabName = "MyCharacter";

    public GameObject CreatePlayer(Vector3 position, Quaternion rotation)
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefabName, position, rotation);
        return player; 
    }
}