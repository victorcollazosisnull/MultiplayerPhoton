using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject pausePanel;
    private PlayerInputsController inputs;
    private bool isPaused = false;

    private void Awake()
    {
        // Asignamos inputs aquí para tenerlo listo antes del OnEnable si es posible
        inputs = GetComponent<PlayerInputsController>();
    }

    private void Start()
    {
        // Verificación extra por si Awake no fue suficiente
        if (inputs == null) inputs = GetComponent<PlayerInputsController>();

        if (photonView.IsMine)
        {
            GameObject hud = GameObject.Find("MainHUD");
            if (hud != null)
            {
                Transform panelTransform = hud.transform.Find("PausePanel");
                if (panelTransform != null)
                {
                    pausePanel = panelTransform.gameObject;
                    pausePanel.SetActive(false);
                }
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        // Si inputs aún es nulo, lo buscamos rápido
        if (inputs == null) inputs = GetComponent<PlayerInputsController>();

        if (photonView.IsMine && inputs != null)
        {
            inputs.onPauseInput += TogglePause;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (photonView.IsMine && inputs != null)
        {
            inputs.onPauseInput -= TogglePause;
        }
    }

    public void TogglePause()
    {
        if (pausePanel == null)
        {
            GameObject hud = GameObject.Find("MainHUD");
            if (hud != null)
            {
                Transform pt = hud.transform.Find("PausePanel");
                if (pt != null) pausePanel = pt.gameObject;
            }
            if (pausePanel == null) return;
        }

        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }

    public void QuitGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LoginScene");
    }
}