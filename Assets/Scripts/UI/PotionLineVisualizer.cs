using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionLineVisualizer : MonoBehaviour {
    private PotionSO _potion;

    [SerializeField] private Image _spriteRenderer;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private GameObject _recipeContainer;
    public void SetPotion(PotionSO potion) {
        _potion = potion;
        UpdatePotion();
    }
    public void UpdatePotion() {
        _spriteRenderer.sprite = _potion.potionSprite;
        _title.text = _potion.potionName;
        _description.text = _potion.description;
        foreach (Transform child in _recipeContainer.transform)
            Destroy(child.gameObject);
        for (int i = 0; i < _potion.ingredients.Length; i++) {
            GameObject iconGO = new GameObject("IngredientIcon");
            iconGO.transform.SetParent(_recipeContainer.transform);
            Image img = iconGO.AddComponent<Image>();
            img.sprite = _potion.ingredients[i].ingredientSprite;
            iconGO.transform.localScale = Vector3.one;
        }
    }
}
