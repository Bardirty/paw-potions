using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PotionGame
{
    /// <summary>
    /// Manages the contents of the cauldron.  Handles adding and removing
    /// ingredients, evaluating the current combination against the recipe
    /// database and updating the visual representation accordingly.  UI code
    /// should call AddIngredientToCauldron() when the player drags a sprite
    /// over the vessel and EmptyCauldron() to clear it.  When a recipe is
    /// matched, the OnPotionCreated event is invoked with the matched
    /// recipe so that other systems (e.g. DialogueManager) can react.
    /// </summary>
    public class CauldronManager : MonoBehaviour
    {
        [Tooltip("List of available potion recipes.  Each recipe is a ScriptableObject asset.")]
        public List<PotionRecipe> recipes = new List<PotionRecipe>();

        [Tooltip("Renderer showing the current cauldron content sprite.")]
        public SpriteRenderer cauldronRenderer;

        [Tooltip("Default sprite shown when mix does not match any recipe.")]
        public Sprite defaultCauldronSprite;

        private List<IngredientDefinition> currentIngredients = new List<IngredientDefinition>();

        // 🔹 Ostatnio zmiksowany napar – może być null
        private PotionRecipe currentMixedRecipe;

        public event Action<PotionRecipe> OnPotionCreated;

        public List<IngredientDefinition> GetCurrentIngredients()
        {
            return currentIngredients;
        }

        /// <summary>
        /// The most recent matched recipe.  Null when no recipe has been matched.
        /// </summary>
        public PotionRecipe currentPotion;


        /// <summary>
        /// Adds an ingredient to the cauldron and checks whether a known
        /// recipe matches.  UI code should call this when the player drops
        /// an ingredient sprite into the cauldron area.
        /// </summary>
        public void AddIngredientToCauldron(IngredientDefinition ingredient)
        {
            if (ingredient == null) return;

            currentIngredients.Add(ingredient);

            // Tylko dodajemy składnik, NIC nie sprawdzamy automatycznie
            // Wizualki kotła możesz ogarniać tutaj.
        }

        public void MixCurrentIngredients()
        {
            // sprawdź, czy lista przepisów w ogóle istnieje
            if (recipes == null || recipes.Count == 0)
            {
                Debug.LogError("CauldronManager has no recipes assigned.");
                currentMixedRecipe = null;
                UpdateCauldronSprite(null);
                return;
            }

            PotionRecipe matched = FindMatchingRecipe();
            currentMixedRecipe = matched;

            UpdateCauldronSprite(matched);

            if (matched != null)
            {
                Debug.Log($"Mix result: {matched.potionName}");
            }
            else
            {
                Debug.Log("Mix result: no matching recipe.");
            }
        }

        private void UpdateCauldronSprite(PotionRecipe recipe)
        {
            if (cauldronRenderer == null)
                return;

            if (recipe != null && recipe.resultSprite != null)
                cauldronRenderer.sprite = recipe.resultSprite;
            else
                cauldronRenderer.sprite = defaultCauldronSprite;
        }




        /// <summary>
        /// Empties all ingredients from the cauldron and resets the visual
        /// representation to the default mixture sprite.  UI code should
        /// provide a button or keybinding to call this method.
        /// </summary>
        public void EmptyCauldron()
        {
            currentIngredients.Clear();
            currentPotion = null;
            UpdateCauldronVisual();
        }

        /// <summary>
        /// Evaluates the current list of ingredients against the recipe
        /// database.  If a recipe matches, sets currentPotion and updates
        /// the visuals; otherwise shows the default mixture.  When a
        /// recipe is matched, the OnPotionCreated event is fired.
        /// </summary>
        private void EvaluateCurrentMixture()
        {
            if (recipes == null || recipes.Count == 0)
            {
                Debug.LogWarning("CauldronManager has no recipes assigned.");
                return;
            }
            if (currentIngredients.Count == 0)
            {
                currentPotion = null;
                UpdateCauldronVisual();
                return;
            }
            var matchedRecipe = FindRecipe(new List<IngredientDefinition>(currentIngredients));
            if (matchedRecipe != null)
            {
                currentPotion = matchedRecipe;
                UpdateCauldronVisual();
                OnPotionCreated?.Invoke(matchedRecipe);
            }
            else
            {
                currentPotion = null;
                UpdateCauldronVisual();
            }
        }

        /// <summary>
        /// Searches the list of recipes for a match to the given ingredients.  Ingredients
        /// are compared without regard to order.  Returns the matching recipe or null.
        /// </summary>
        private PotionRecipe FindRecipe(List<IngredientDefinition> ingredients)
        {
            // Sort the input list to normalise order
            ingredients.Sort((a, b) => a.GetInstanceID().CompareTo(b.GetInstanceID()));
            foreach (var recipe in recipes)
            {
                if (recipe == null) continue;
                if (recipe.ingredients.Count != ingredients.Count)
                    continue;
                // Copy and sort recipe ingredients
                var sortedRecipeIngredients = new List<IngredientDefinition>(recipe.ingredients);
                sortedRecipeIngredients.Sort((a, b) => a.GetInstanceID().CompareTo(b.GetInstanceID()));
                bool match = true;
                for (int i = 0; i < ingredients.Count; i++)
                {
                    if (sortedRecipeIngredients[i] != ingredients[i])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return recipe;
                }
            }
            return null;
        }

        /// <summary>
        /// Updates the cauldronRenderer sprite based on the currentPotion
        /// field.  If a recipe is matched the recipe’s resultSprite is used;
        /// otherwise the default sprite is shown.
        /// </summary>
        private void UpdateCauldronVisual()
        {
            if (cauldronRenderer == null)
                return;
            if (currentPotion != null && currentPotion.resultSprite != null)
            {
                cauldronRenderer.sprite = currentPotion.resultSprite;
            }
            else
            {
                cauldronRenderer.sprite = defaultCauldronSprite;
            }
        }

        private void MatchRecipe()
        {
            PotionRecipe matched = FindMatchingRecipe(); // twoja logika porównywania składników

            if (matched != null)
            {
                OnPotionCreated?.Invoke(matched);
            }
        }

        public void GivePotionToClient()
        {
            if (currentMixedRecipe == null)
            {
                Debug.Log("No mixed potion to give to client.");
                return;
            }

            Debug.Log($"Giving mixed potion to client: {currentMixedRecipe.potionName}");
            OnPotionCreated?.Invoke(currentMixedRecipe);

            // opcjonalnie: po wydaniu naparu opróżnij kocioł i zresetuj sprite
            currentIngredients.Clear();
            currentMixedRecipe = null;
            UpdateCauldronSprite(null);
        }



        /// <summary>
        /// Sprawdza, czy aktualne składniki w kotle pasują do którejś z receptur.
        /// Jeśli tak – zwraca dopasowaną receptę.
        /// Jeśli nie – zwraca null.
        /// </summary>
        private PotionRecipe FindMatchingRecipe()
        {
            foreach (var recipe in recipes)
            {
                if (recipe == null || recipe.ingredients == null)
                    continue;

                // Jeżeli liczba składników się nie zgadza — nie ma sensu sprawdzać dalej
                if (recipe.ingredients.Count != currentIngredients.Count)
                    continue;

                // Tworzymy kopię, żeby móc "odhaczać" znalezione składniki
                List<IngredientDefinition> unmatched = new List<IngredientDefinition>(recipe.ingredients);
                bool allMatched = true;

                foreach (var ing in currentIngredients)
                {
                    if (!unmatched.Contains(ing))
                    {
                        allMatched = false;
                        break;
                    }
                    else
                    {
                        unmatched.Remove(ing);
                    }
                }

                if (allMatched)
                    return recipe;
            }

            return null;
        }

        private void TryMatchRecipe()
        {
            PotionRecipe matched = FindMatchingRecipe();

            if (matched != null)
            {
                Debug.Log($"Matched recipe: {matched.potionName}");
                OnPotionCreated?.Invoke(matched);
            }
        }

    }
}