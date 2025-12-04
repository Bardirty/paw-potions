using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragndropableItem : MonoBehaviour {
    private Camera cam;
    private Vector3 offset;
    private bool isDragging = false;
    private void Awake() {
        cam = Camera.main;
    }
    public void BeginDrag() {
        isDragging = true;
        Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mouse;
    }
    private void OnMouseDown() {
        BeginDrag();
    }
    private void OnMouseDrag() {
        if (!isDragging) return;

        Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouse.x + offset.x,
                                         mouse.y + offset.y,
                                         transform.position.z);
    }
    private void OnMouseUp() {
        isDragging = false;
    }
}
