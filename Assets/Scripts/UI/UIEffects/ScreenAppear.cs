using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenAppear : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeScreen;
    [SerializeField] private float appearDuration = 1.0f;
    public event Action OnDisappear;

    private Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }
    private void OnEnable() {
        StartCoroutine(AppearCoroutine());
    }
    private void SetActive(bool mode) {
        image.enabled = mode;
    }

    private IEnumerator AppearCoroutine() {
        float elapsedTime = 0f;
        fadeScreen.alpha = 1f;
        while (elapsedTime < appearDuration) {
            elapsedTime += Time.deltaTime;
            fadeScreen.alpha = 1.0f - Mathf.Clamp01(elapsedTime / appearDuration);
            yield return null;
        }
        fadeScreen.alpha = 0f;
        SetActive(false);
    }
    private IEnumerator DisappearCoroutine() {
        SetActive(true);
        float elapsedTime = 0f;
        fadeScreen.alpha = 0f;
        while (elapsedTime < appearDuration) {
            elapsedTime += Time.deltaTime;
            fadeScreen.alpha = Mathf.Clamp01(elapsedTime / appearDuration);
            yield return null;
        }
        fadeScreen.alpha = 1f;
        OnDisappear?.Invoke();
    }

    public void Disappear() {
        fadeScreen.gameObject.SetActive(true);
        StartCoroutine(DisappearCoroutine());
    }
}
