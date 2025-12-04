using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class PinSlideAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private SlideAnimator anim;

    [SerializeField] private float offset = 0.3f;
    [SerializeField] private float duration = 0.35f;

    private void Awake() {
        anim = GetComponent<SlideAnimator>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        anim.Slide(SlideDirection.Left, SlideType.Out, duration, offset);
    }

    public void OnPointerExit(PointerEventData eventData) {
        anim.Slide(SlideDirection.Left, SlideType.In, duration, offset);
    }
}