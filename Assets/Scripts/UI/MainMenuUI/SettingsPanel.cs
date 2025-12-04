using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour {
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [Header("UI")]
    [SerializeField] private Button backToMenu;
    [SerializeField] private GameObject mainMenuPanel;

    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string SOUND_KEY = "SoundVolume";

    private void Awake() {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);

        backToMenu.onClick.AddListener(OnBackToMenuClicked);

        LoadPrefs();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    private void SetMasterVolume(float value)
        => audioMixer.SetFloat("MasterVolume", SliderToDb(value));

    private void SetMusicVolume(float value)
        => audioMixer.SetFloat("MusicVolume", SliderToDb(value));

    private void SetSoundVolume(float value)
        => audioMixer.SetFloat("SoundVolume", SliderToDb(value));
    private float SliderToDb(float sliderValue) {
        return Mathf.Lerp(-80f, 0f, sliderValue);
    }

    private void LoadPrefs() {
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        soundSlider.value = PlayerPrefs.GetFloat(SOUND_KEY, 1f);

        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSoundVolume(soundSlider.value);
    }

    private void SavePrefs() {
        PlayerPrefs.SetFloat(MASTER_KEY, masterSlider.value);
        PlayerPrefs.SetFloat(MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(SOUND_KEY, soundSlider.value);
    }
    private void OnBackToMenuClicked() {
        SavePrefs();

        mainMenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
