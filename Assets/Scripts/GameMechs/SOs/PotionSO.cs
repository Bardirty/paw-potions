using UnityEngine;

[CreateAssetMenu(fileName = "New Potion")]
public class PotionSO : ScriptableObject
{
    public string potionName;
    public Sprite potionSprite;
    public string description;
    public IngredientSO[] ingredients;
}
