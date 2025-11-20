using UnityEngine;

public class DragndropableItem : MonoBehaviour
{
    private Vector2 startPosition;
    private void Start() {
        startPosition = transform.position;
    }
    private void OnMouseDrag() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }
    private void OnMouseUp() {
        transform.position = startPosition;
    }
}
