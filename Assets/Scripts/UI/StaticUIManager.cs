using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StaticUIManager : MonoBehaviour {
    public const string MAIN_MENU_SCENE = "MainMenuScene";
    public const string CLIENT_SCENE = "ClientScene";
    public const string WORKSHOP_SCENE = "WorkshopScene";

    [SerializeField] private GameObject bookPanel;

    public static StaticUIManager Instance { get; private set; }

    [Header("HUDButtons")]
    [SerializeField] private Button clientButton;
    [SerializeField] private Button workshopButton;
    [SerializeField] private Button bookButton;

    [Header("Panels")]
    public GameObject PauseMenu;
    public GameObject Hud;

    [Header("PauseMenuButtons")]
    public Button ResumeButton;
    public Button LeaveButton;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        clientButton.onClick.AddListener(OnClientClicked);
        workshopButton.onClick.AddListener(OnWorkshopClicked);
        bookButton.onClick.AddListener(OnBookClicked);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == MAIN_MENU_SCENE) {
            Destroy(gameObject);
            return;
        }
    }

    private void OnClientClicked() => SceneManager.LoadScene(CLIENT_SCENE);
    private void OnWorkshopClicked() => SceneManager.LoadScene(WORKSHOP_SCENE);

    private void OnBookClicked() {
        bookPanel.SetActive(!bookPanel.activeSelf);
    }
}
