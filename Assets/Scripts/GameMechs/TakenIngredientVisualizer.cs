using UnityEngine;

public class TakenIngredientVisualizer : MonoBehaviour {
    private DragndropableItem dragItem;
    private IngredientSO ingredient;

    [SerializeField] private bool isOverPot = false;
    [SerializeField] private PotController potRef = null;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    private void Start() {
        dragItem = GetComponent<DragndropableItem>();
    }
    public void SetSprite(IngredientSO ingr, Sprite sprite) {
        ingredient = ingr;
        _spriteRenderer.sprite = sprite;
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent(out PotController pot)) {
            isOverPot = true;
            potRef = pot;
        }
    }
    private void OnTriggerExit2D(Collider2D col) {
        if (col.TryGetComponent(out PotController pot)) {
            isOverPot = false;
            potRef = null;
        }
    }
    private void OnMouseUp() {
        if (isOverPot && potRef != null && ingredient != null) {
            potRef.PutItem(ingredient);
        }
        else if (PawtionsGameManager.Instance != null) {
            PawtionsGameManager.Instance.IncreaseIngredient(ingredient);
        }
        Destroy(gameObject);
    }

}
