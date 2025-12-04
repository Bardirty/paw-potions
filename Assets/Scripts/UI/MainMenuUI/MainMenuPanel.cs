using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    public const string PLAY_SCENE_NAME = "DayStartScene";

    [Header("Button")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        creditsButton.onClick.AddListener(OnCreditsButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }
    private void Start() {
        if(MusicManager.Instance != null)
            MusicManager.Instance.PlayMainMenuMusic();
    }

    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(PLAY_SCENE_NAME);
    }

    private void OnSettingsButtonClicked()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    private void OnCreditsButtonClicked()
    {
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
