using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }
    [SerializeField] private SoundSO sounds;
    private AudioSource audioSource;
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    public void Play(AudioClip clip) {
        if(clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void PlayBoiling() => Play(sounds.boiling);
    public void PlayMixing() => Play(sounds.mixing);
    public void PlaySplash() => Play(sounds.splash);
    public void PlayGoodPotion() => Play(sounds.goodPotion);
    public void PlayBadPotion() => Play(sounds.badPotion);
    public void PlayFire() => Play(sounds.fire);
    public void PlayBell() => Play(sounds.bell);
    public void PlayGUIPress() => Play(sounds.GUIpress);
    public void PlayBookOpen() => Play(sounds.bookOpen);
    public void PlayBookClose() => Play(sounds.bookClose);
    public void PlayPageSwap() => Play(sounds.pageSwap);
    public void PlayDayStart() => Play(sounds.dayStart);
    public void PlayDayEnd() => Play(sounds.dayEnd);
    public void PlayEnjoyedClient() => Play(sounds.enjoyedClient);
    public void PlayNotEnjoyedClient() => Play(sounds.notEnjoyedClient);
    public void PlayPayment() => Play(sounds.payment);

}
