using UnityEngine;
using Photon.Pun;

public class PlayerFactory : MonoBehaviour
{
    [SerializeField] private string playerPrefabName = "MyCharacter";

    public GameObject CreatePlayer(Vector3 position, Quaternion rotation)
    {
        int skinIndex = SandController.Instance.CurrentSkinIndex;

        object[] data = new object[] { skinIndex };

        GameObject player = PhotonNetwork.Instantiate(playerPrefabName, position, rotation, 0, data);

        return player;
    }
}