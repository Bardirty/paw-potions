using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StaticUIManager : MonoBehaviour {
    [Header("Scenes where UI is allowed")]

    public const string MAIN_MENU_SCENE = "MainMenuScene";
    public const string CLIENT_SCENE = "ClientScene";
    public const string WORKSHOP_SCENE = "WorkshopScene";
    
    public string[] allowedScenes = { CLIENT_SCENE, WORKSHOP_SCENE };


    [SerializeField] private GameObject bookPanel;

    public static StaticUIManager Instance { get; private set; }

    [Header("HUDButtons")]
    public Button clientButton;
    public Button workshopButton;
    public Button bookButton;

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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!allowedScenes.Contains(scene.name)) {
            SceneManager.sceneLoaded -= OnSceneLoaded; // <<< фикс
            Destroy(gameObject);
            return;
        }
        ReconnectUI();
    }


    private void ReconnectUI() {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null) return;

        if (clientButton != null) {
            clientButton.onClick.RemoveAllListeners();
            clientButton.onClick.AddListener(OnClientClicked);
        }

        if (workshopButton != null) {
            workshopButton.onClick.RemoveAllListeners();
            workshopButton.onClick.AddListener(OnWorkshopClicked);
        }

        if (bookButton != null) {
            bookButton.onClick.RemoveAllListeners();
            bookButton.onClick.AddListener(OnBookClicked);
        }
    }

    private void OnClientClicked() => SceneManager.LoadScene(CLIENT_SCENE);
    private void OnWorkshopClicked() => SceneManager.LoadScene(WORKSHOP_SCENE);

    private void OnBookClicked() {
        bookPanel.SetActive(!bookPanel.activeSelf);
    }
}
