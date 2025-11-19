using System.Collections.Generic;
using UnityEngine;

namespace PotionGame
{
    [CreateAssetMenu(menuName = "Potion Game/Potion Recipe", fileName = "NewPotionRecipe")]
    public class PotionRecipe : ScriptableObject
    {
        [Tooltip("Unique name for this potion recipe.")]
        public string potionName;

        [Tooltip("List of ingredients required to craft this potion. Order does not matter.")]
        public List<IngredientDefinition> ingredients = new List<IngredientDefinition>();

        [Tooltip("Sprite representing the visual effect in the cauldron when this recipe is active.")]
        public Sprite resultSprite;

        [Tooltip("Optional description or notes for designers.")]
        [TextArea]
        public string description;
    }
}
