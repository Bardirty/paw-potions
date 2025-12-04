using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    public const string MAIN_MENU_SCENE = "MainMenuScene";
    private GameObject pauseMenu;
    private GameObject gameUI;

    private void Awake()
    {
        StaticUIManager.Instance?.ResumeButton.onClick.AddListener(OnResumeClicked);
        StaticUIManager.Instance?.LeaveButton.onClick.AddListener(OnLeaveClicked);
    }
    private void Start() {
        pauseMenu = StaticUIManager.Instance.PauseMenu;
        gameUI = StaticUIManager.Instance.Hud;
        pauseButton.onClick.AddListener(TogglePause);
        pauseMenu.SetActive(false);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }
    private void OnResumeClicked() => TogglePause();
    private void TogglePause() {
        gameUI.SetActive(pauseMenu.activeSelf);
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    private void OnLeaveClicked()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
}
