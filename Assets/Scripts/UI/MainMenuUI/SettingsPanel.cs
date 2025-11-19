using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour {

    [Header("Params")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle volume;
    [SerializeField] private Button backToMenu;

    [SerializeField] private GameObject mainMenuPanel;
    private void Awake() {
        volume.onValueChanged.AddListener(OnVolumeToggleChanged);
        backToMenu.onClick.AddListener(OnBackToMenuClicked);
        LoadPrefs();

    }
    private void Start() {
        gameObject.SetActive(false);
    }

    private void OnVolumeToggleChanged(bool isOn) {
        audioMixer.SetFloat("MasterVolume", isOn ? 0f : -80f);
    }
    private void OnBackToMenuClicked() {
        SavePrefs();
        mainMenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void LoadPrefs() {
        volume.isOn = PlayerPrefs.GetInt("VolumeOn", 1) == 1;
        OnVolumeToggleChanged(volume.isOn);
    }
    private void SavePrefs() {
        PlayerPrefs.SetInt("VolumeOn", volume.isOn ? 1 : 0);
    }
}
