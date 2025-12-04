using UnityEngine;

public class IngredientContainer : MonoBehaviour {
    [Header("Ingredient Base")]
    [SerializeField] private IngredientSO ingredient;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite fullContainer;
    [SerializeField] private Sprite emptyContainer;


    [SerializeField] private Sprite takenFormSprite;
    [SerializeField] private TakenIngredientVisualizer ingredientPrefab;

    private void Start() {
        UpdateState();
    }

    private void UpdateState() {
        if (PawtionsGameManager.Instance == null)
            return;
        if(PawtionsGameManager.Instance.GetIngredientAmount(ingredient) <= 0)
            spriteRenderer.sprite = emptyContainer;
        else
            spriteRenderer.sprite = fullContainer;
    }
    private void OnMouseDown() {
        if (PawtionsGameManager.Instance == null || StaticUIManager.Instance.IsGameNotInteractable()) return;

        int amount = PawtionsGameManager.Instance.GetIngredientAmount(ingredient);
        if (amount <= 0)
            return;

        PawtionsGameManager.Instance.DecreaseIngredient(ingredient);
        UpdateState();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -2;

        TakenIngredientVisualizer item = Instantiate(ingredientPrefab, mousePos, Quaternion.identity);
        item.SetSprite(ingredient, takenFormSprite);

        item.GetComponent<DragndropableItem>().BeginDrag();

    }
    private void OnMouseUp() {
        Invoke(nameof(UpdateState), 0.05f);
    }

}
