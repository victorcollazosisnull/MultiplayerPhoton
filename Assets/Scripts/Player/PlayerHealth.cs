using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("UI 3D")]
    [SerializeField] private Image floatingHealthBar;
    [SerializeField] private TextMeshProUGUI floatingNameText;

    [Header("UI Mia")]
    [SerializeField] private Image screenHealthBar;
    [SerializeField] private TextMeshProUGUI screenNameText;
    [SerializeField] private Image damageOverlay;
    [SerializeField] private float lerpSpeed = 5f;

    private int lastAttackerActorNumber = -1;
    private void Awake() { currentHealth = maxHealth; }

    private void Start()
    {
        if (floatingNameText != null)
            floatingNameText.text = photonView.Owner.NickName;

        if (photonView.IsMine)
        {
            GameObject hud = GameObject.Find("MainHUD");
            if (hud != null)
            {
                screenNameText = hud.transform.Find("PlayerName_UI")?.GetComponent<TextMeshProUGUI>();
                if (screenNameText != null) screenNameText.text = photonView.Owner.NickName;

                screenHealthBar = hud.transform.Find("PlayerHealthBar_UI")?.GetComponent<Image>();

                Transform overlayTransform = hud.transform.Find("DamageOverlay");
                if (overlayTransform != null)
                {
                    damageOverlay = overlayTransform.GetComponent<Image>();
                    damageOverlay.color = new Color(1, 0, 0, 0); 
                }
            }
        }
    }

    private void Update()
    {
        float targetFill = currentHealth / maxHealth;

        if (currentHealth <= 0) targetFill = 0;

        if (photonView.IsMine)
        {
            if (screenHealthBar != null)
            {
                screenHealthBar.fillAmount = (currentHealth <= 0) ? 0 : Mathf.Lerp(screenHealthBar.fillAmount, targetFill, Time.deltaTime * lerpSpeed);
                screenHealthBar.color = Color.Lerp(Color.red, Color.green, screenHealthBar.fillAmount);
            }
        }
        else
        {
            if (floatingHealthBar != null)
            {
                floatingHealthBar.fillAmount = (currentHealth <= 0) ? 0 : Mathf.Lerp(floatingHealthBar.fillAmount, targetFill, Time.deltaTime * lerpSpeed);
                floatingHealthBar.color = Color.Lerp(Color.red, Color.green, floatingHealthBar.fillAmount);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(float amount, int attackerActorNumber)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "ArenaScene") return;
        if (photonView.IsMine)
        {
            lastAttackerActorNumber = attackerActorNumber;
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (damageOverlay != null)
            {
                StopAllCoroutines();
                StartCoroutine(FlashDamageOverlay());
            }

            photonView.RPC("RPC_UpdateHealth", RpcTarget.All, currentHealth);
            if (currentHealth <= 0) Die();
        }
    }

    IEnumerator FlashDamageOverlay()
    {
        float alpha = 0.5f; 
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            damageOverlay.color = new Color(1, 0, 0, alpha);
            yield return null;
        }
    }

    [PunRPC]
    public void RPC_UpdateHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    void Die()
    {
        if (photonView.IsMine)
        {
            if (SandController.Instance != null)
                SandController.Instance.StartCoroutine(SandController.Instance.RespawnRoutine());
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public float GetCurrentHealth() => currentHealth;
}