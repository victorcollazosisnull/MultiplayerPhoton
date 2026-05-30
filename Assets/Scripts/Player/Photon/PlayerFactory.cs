using UnityEngine;
using Photon.Pun;

public class PlayerFactory : MonoBehaviour
{
    [SerializeField] private string playerPrefabName = "MyCharacter";

    public GameObject CreatePlayer(Vector3 position, Quaternion rotation)
    {
        int skinIndex = 0;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("SkinIndex", out object val))
            skinIndex = (int)val;
        else
            skinIndex = PlayerPrefs.GetInt("CharacterIndex", 0); 

        object[] data = new object[] { skinIndex };
        return PhotonNetwork.Instantiate(playerPrefabName, position, rotation, 0, data);
    }
}