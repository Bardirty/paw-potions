using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempSwitcher : MonoBehaviour
{
    [SerializeField] private string nameOfScene = "";
    [SerializeField] private float appearDelay = 2f;
    private void Start() {
        SoundManager.Instance?.PlayDayStart();
        MusicManager.Instance?.StopMusic();
        StartCoroutine(ShowScene());
    }

    private IEnumerator ShowScene() {
        yield return new WaitForSeconds(appearDelay);
        SceneManager.LoadSceneAsync(nameOfScene);
    }
}
