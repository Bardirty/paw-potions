using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientLineVisualizer : MonoBehaviour
{
    private IngredientSO _ingredientSO;

    [SerializeField] private Image ingredientSprite;
    [SerializeField] private TextMeshProUGUI ingredientName;
    [SerializeField] private TextMeshProUGUI ingredientPrice;
    [SerializeField] private TextMeshProUGUI ingredientAmount;
    [SerializeField] private Button buyButton;
    private void Start() {
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(Buy);
    }

    private void OnEnable() {
        if (_ingredientSO != null) UpdateIngredient();
    }

    public void SetIngredient(IngredientSO ingredient) {
        _ingredientSO = ingredient;
        UpdateIngredient();
    }
    public void UpdateIngredient() {
        ingredientSprite.sprite = _ingredientSO.ingredientSprite;
        ingredientName.text = _ingredientSO.ingredientName;
        ingredientPrice.text = "$" + _ingredientSO.price.ToString();
        if(PawtionsGameManager.Instance != null)
            ingredientAmount.text = "x" + PawtionsGameManager.Instance
                .GetIngredientAmount(_ingredientSO).ToString();
    }

    private void Buy() {
        if (PawtionsGameManager.Instance != null) {
            PawtionsGameManager.Instance.Buy(_ingredientSO);
            UpdateIngredient();
        }
    }
}
