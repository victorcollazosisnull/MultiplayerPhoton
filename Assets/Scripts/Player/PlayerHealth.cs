using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [SerializeField] private Image healthBarFill;
    [SerializeField] private float lerpSpeed = 5f;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (healthBarFill != null)
        {
            float targetFill = currentHealth / maxHealth;
            healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, targetFill, Time.deltaTime * lerpSpeed);
            healthBarFill.color = Color.Lerp(Color.red, Color.green, healthBarFill.fillAmount);
        }
    }

    [PunRPC]
    public void TakeDamage(float amount)
    {
        if (photonView.IsMine)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            photonView.RPC("RPC_UpdateHealth", RpcTarget.All, currentHealth);

            if (currentHealth <= 0) Die();
        }
    }

    [PunRPC]
    public void RPC_UpdateHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    void Die()
    {
        Debug.Log("He muerto: " + photonView.Owner.NickName);
    }
}