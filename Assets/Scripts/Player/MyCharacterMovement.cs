using UnityEngine;
using Photon.Pun;

public class MyCharacterMovement : MonoBehaviourPun
{
    [SerializeField] private float speed = 5f;

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        transform.Translate(dir * speed * Time.deltaTime);
    }
}