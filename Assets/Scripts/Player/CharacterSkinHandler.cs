using UnityEngine;
using Photon.Pun;

public class CharacterSkinHandler : MonoBehaviourPun
{
    [SerializeField] private GameObject[] skinPrefabs;

    private GameObject currentVisual;

    private void Start()
    {
        ApplySkin();
    }

    void ApplySkin()
    {
        object[] data = photonView.InstantiationData;

        if (data == null || data.Length == 0)
        {
            Debug.LogError("No hay InstantiationData");
            return;
        }

        int index = (int)data[0];

        if (index < 0 || index >= skinPrefabs.Length)
            return;

        if (currentVisual != null)
            Destroy(currentVisual);

        currentVisual = Instantiate(skinPrefabs[index], transform);

        currentVisual.transform.localPosition = Vector3.zero;
        currentVisual.transform.localRotation = Quaternion.identity;

        if (currentVisual.TryGetComponent(out RotateObject rot))
            rot.enabled = false;

        Debug.Log("Skin aplicada: " + index);
    }
}