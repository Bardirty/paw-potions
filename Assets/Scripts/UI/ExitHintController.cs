using UnityEngine;

public class ExitHintController : MonoBehaviour
{
    [Header("Hold time to exit")]
    [SerializeField] private float holdTime = 1f;

    private float escTimer = 0f;
    private bool isHolding = false;

    private void Start() {
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayMainMusic();
    }

    void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            if (!isHolding) {
                isHolding = true;
                escTimer = 0f;
            }

            escTimer += Time.deltaTime;

            if (escTimer >= holdTime) {
                QuitGame();
            }
        }
        else {
            isHolding = false;
            escTimer = 0f;
        }
    }

    private void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
