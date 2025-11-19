using System.Collections;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class VisualFader : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField] private bool fadeOnStart = false;
    [SerializeField] private float fadeSpeed = 1f;
    private IEnumerator _fadeCoroutine;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable() {
        if (fadeOnStart)
            FadeIn(1f);
    }

    private void OnDisable() {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
    }
    public void FadeIn(float duration)
    {
        StartCoroutine(_fadeCoroutine = FadeCoroutine(0f, 1f, duration));
    }
    public void FadeOut(float duration)
    {
        StartCoroutine(_fadeCoroutine = FadeCoroutine(1f, 0f, duration));
    }

    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        _canvasGroup.alpha = startAlpha;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime * fadeSpeed;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }
        _canvasGroup.alpha = endAlpha;
    }
}
