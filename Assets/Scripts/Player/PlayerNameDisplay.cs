using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameDisplay : MonoBehaviourPun
{
    [SerializeField] private TMP_Text nameText;

    void Start()
    {
        if (photonView.Owner != null)
        {
            nameText.text = photonView.Owner.NickName;
        }
        if (photonView.IsMine)
        {
            nameText.color = Color.yellow;
        }
    }

    //void Update()
    //{
    //    if (Camera.main != null)
    //    {
    //        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
    //                         Camera.main.transform.rotation * Vector3.up);
    //    }
    //}
}