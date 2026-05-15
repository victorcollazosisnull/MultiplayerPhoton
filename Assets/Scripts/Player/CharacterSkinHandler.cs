using UnityEngine;
using Photon.Pun;

public class CharacterSkinHandler : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private GameObject[] skinPrefabs; 
    private int currentSkinIndex = -1;
    private GameObject currentVisual;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            currentSkinIndex = PlayerPrefs.GetInt("CharacterIndex", 0);
            UpdateSkin(currentSkinIndex);
        }
    }

    void UpdateSkin(int index)
    {
        if (currentVisual != null) Destroy(currentVisual);

        currentVisual = Instantiate(skinPrefabs[index], transform.position, transform.rotation);
        currentVisual.transform.SetParent(this.transform);

        if (currentVisual.TryGetComponent(out RotateObject rot)) rot.enabled = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentSkinIndex);
        }
        else
        {
            int receivedIndex = (int)stream.ReceiveNext();
            if (currentSkinIndex != receivedIndex)
            {
                currentSkinIndex = receivedIndex;
                UpdateSkin(currentSkinIndex);
            }
        }
    }
}