using UnityEngine;
using System.Collections;

public enum SlideDirection {
    Up,
    Down,
    Left,
    Right,
}

public enum SlideType {
    In,
    Out,
}

public class SlideAnimator : MonoBehaviour {
    private RectTransform rect;
    private Vector2 originalPos;
    private Coroutine anim;

    private void Awake() {
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;
    }

    public void Slide(SlideDirection dir, SlideType type, float duration = 0.35f, float offset = 1f) {
        if (anim != null)
            StopCoroutine(anim);

        anim = StartCoroutine(SlideRoutine(dir, type, duration, offset));
    }

    private IEnumerator SlideRoutine(SlideDirection dir, SlideType type, float duration, float offset) {
        RectTransform canvas = rect.root as RectTransform;
        Vector2 canvasSize = canvas.rect.size;

        Vector2 offscreen = dir switch {
            SlideDirection.Up => originalPos + new Vector2(0, canvasSize.y * offset),
            SlideDirection.Down => originalPos + new Vector2(0, -canvasSize.y * offset),
            SlideDirection.Left => originalPos + new Vector2(-canvasSize.x * offset, 0),
            SlideDirection.Right => originalPos + new Vector2(canvasSize.x * offset, 0),
            _ => originalPos
        };

        Vector2 start = type == SlideType.In ? offscreen : originalPos;
        Vector2 target = type == SlideType.In ? originalPos : offscreen;

        rect.anchoredPosition = start;

        float t = 0;
        while (t < 1) {
            t += Time.unscaledDeltaTime / duration;
            float smooth = 1 - Mathf.Pow(1 - t, 3);

            rect.anchoredPosition = Vector2.Lerp(start, target, smooth);

            yield return null;
        }

        rect.anchoredPosition = target;
    }
}
