using UnityEngine;
using UnityEngine.EventSystems;

public class UICursorInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    private void OnDisable() {
        CursorManager.Instance.SetBasicCursor();
    }
    public void OnPointerEnter(PointerEventData eventData) {
        CursorManager.Instance.SetSelectCursor();
    }

    public void OnPointerExit(PointerEventData eventData) {
        CursorManager.Instance.SetBasicCursor();
    }
    public void OnPointerDown(PointerEventData eventData) {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayGUIPress();
        CursorManager.Instance.SetHoldCursor();
    }

}
