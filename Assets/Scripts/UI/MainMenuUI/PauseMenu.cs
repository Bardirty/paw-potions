using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button leaveButton;

    public const string MAIN_MENU_SCENE = "MainMenuScene";
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        resumeButton.onClick.AddListener(OnResumeClicked);
        leaveButton.onClick.AddListener(OnLeaveClicked);
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
    private void TogglePause() => pauseMenu.SetActive(!pauseMenu.activeSelf);

    private void OnLeaveClicked()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
}
