using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public const string MAIN_MENU_SCENE = "MainMenuScene";
    private GameObject pauseMenu => StaticUIManager.Instance.PauseMenu;
    private GameObject gameUI => StaticUIManager.Instance.Hud;

    private void Awake()
    {
        StaticUIManager.Instance?.ResumeButton.onClick.AddListener(OnResumeClicked);
        StaticUIManager.Instance?.LeaveButton.onClick.AddListener(OnLeaveClicked);
    }
    private void Start() {
        pauseMenu.SetActive(false);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }
    private void OnResumeClicked() => TogglePause();
    private void TogglePause() {
        gameUI.SetActive(!pauseMenu.activeSelf);
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    private void OnLeaveClicked()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
}
