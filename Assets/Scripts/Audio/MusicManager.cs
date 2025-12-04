using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private MusicSO music;
    private AudioSource musicSource;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();
    }
    public void PlayMusic(AudioClip clip) {
        if (clip != null) {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
    public void PlayMainMenuMusic() => PlayMusic(music.mainMenuMusic);
    public void PlayMainMusic() => PlayMusic(music.mainMusic);

    public void StopMusic() => musicSource.Stop();
}
