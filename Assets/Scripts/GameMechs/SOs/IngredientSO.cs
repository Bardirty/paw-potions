using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient")]
public class IngredientSO : ScriptableObject
{
    public string ingredientName;
    public Sprite ingredientSprite;
    public int price = 0;
}
