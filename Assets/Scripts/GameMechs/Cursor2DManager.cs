using UnityEngine;

public class Cursor2DManager : MonoBehaviour {
    private void OnMouseEnter() {
        CursorManager.Instance.SetSelectCursor();
    }
    private void OnMouseExit() {
        CursorManager.Instance.SetBasicCursor();
    }
    private void OnMouseDown() {
        CursorManager.Instance.SetHoldCursor();
    }
    private void OnDestroy() { 
        CursorManager.Instance.SetBasicCursor();
    }
}
