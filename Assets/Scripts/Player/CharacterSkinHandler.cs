using UnityEngine;
using Photon.Pun;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CharacterSkinHandler : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] skinPrefabs;
    private GameObject currentVisual;
    private int mySkinIndex = -1;

    private void Start()
    {
        if (photonView.IsMine)
        {
            mySkinIndex = PlayerPrefs.GetInt("CharacterIndex", 0);
            photonView.RPC("RPC_ApplySkin", RpcTarget.AllBuffered, mySkinIndex);
        }
    }

    [PunRPC]
    void RPC_ApplySkin(int index)
    {
        if (index < 0 || index >= skinPrefabs.Length) return;

        if (currentVisual != null) Destroy(currentVisual);

        currentVisual = Instantiate(skinPrefabs[index], transform.position, transform.rotation);
        currentVisual.transform.SetParent(this.transform);

        currentVisual.transform.localPosition = Vector3.zero;
        currentVisual.transform.localRotation = Quaternion.identity;

        if (currentVisual.TryGetComponent(out RotateObject rot)) rot.enabled = false;

        Debug.Log("Skin aplicada vía RPC: " + index);
    }
}