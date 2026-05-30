using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject pausePanel;
    private PlayerInputsController inputs;
    private bool isPaused = false;

    [Header("Scripts a pausar")]
    [SerializeField] private MonoBehaviour[] scriptsToPause;

    private void Awake()
    {
        inputs = GetComponent<PlayerInputsController>();
        Debug.Log($"[PauseMenu] Awake | inputs={inputs} | isMine={photonView.IsMine}");
    }

    private void Start()
    {
        if (inputs == null) inputs = GetComponent<PlayerInputsController>();
        Debug.Log($"[PauseMenu] Start | isMine={photonView.IsMine} | inputs={inputs}");

        if (photonView.IsMine)
        {
            GameObject hud = GameObject.Find("MainHUD");
            Debug.Log($"[PauseMenu] MainHUD encontrado: {hud != null}");

            if (hud != null)
            {
                Transform panelTransform = hud.transform.Find("PausePanel");
                Debug.Log($"[PauseMenu] PausePanel encontrado: {panelTransform != null}");

                if (panelTransform != null)
                {
                    pausePanel = panelTransform.gameObject;
                    pausePanel.SetActive(false);
                    Debug.Log($"[PauseMenu] pausePanel asignado: {pausePanel.name}");
                }
            }

            if (inputs != null)
            {
                inputs.onPauseInput -= TogglePause;
                inputs.onPauseInput += TogglePause;
                Debug.Log("[PauseMenu] Suscripción a onPauseInput OK");
            }
            else
            {
                Debug.LogError("[PauseMenu] inputs ES NULL en Start, no se pudo suscribir!");
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (inputs == null) inputs = GetComponent<PlayerInputsController>();
        if (photonView.IsMine && inputs != null)
        {
            inputs.onPauseInput -= TogglePause;
            inputs.onPauseInput += TogglePause;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (photonView.IsMine && inputs != null)
            inputs.onPauseInput -= TogglePause;
    }

    public void TogglePause()
    {
        if (pausePanel == null)
        {
            Debug.LogWarning("[PauseMenu] pausePanel es null en TogglePause, buscando...");
            GameObject hud = GameObject.Find("MainHUD");
            if (hud != null)
            {
                Transform pt = hud.transform.Find("PausePanel");
                if (pt != null) pausePanel = pt.gameObject;
            }
            if (pausePanel == null)
            {
                Debug.LogError("[PauseMenu] No se encontró PausePanel! Abortando.");
                return;
            }
        }

        isPaused = !isPaused;

        if (inputs != null) inputs.IsBlocked = isPaused;

        if (pausePanel != null) pausePanel.SetActive(isPaused);
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }

    public void QuitGame()
    {
        if (SandController.Instance != null)
            Destroy(SandController.Instance.gameObject);

        if (PhotonNetwork.LocalPlayer.TagObject is GameObject playerObj && playerObj != null)
            PhotonNetwork.Destroy(playerObj);

        PhotonNetwork.LocalPlayer.TagObject = null;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("[PauseMenu] OnLeftRoom disparado, cargando Login...");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PhotonNetwork.LocalPlayer.TagObject = null;
        SceneManager.LoadScene("Login");
    }
}