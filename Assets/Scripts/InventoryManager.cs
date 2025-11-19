using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PotionGame
{
    /// <summary>
    /// Manages the player’s inventory and currency at runtime.  The
    /// InventoryManager holds a dictionary of ingredient counts and the
    /// current amount of money.  It provides methods for spending money,
    /// adding or removing ingredients and checking availability.  UI code
    /// should call these methods when the player buys ingredients from the
    /// shop or consumes them in the cauldron.
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        [Tooltip("Definition asset that specifies starting quantities and money.")]
        public PlayerInventoryDefinition startingInventoryDefinition;

        /// <summary>
        /// Dictionary mapping ingredient definitions to quantities.
        /// </summary>
        public Dictionary<IngredientDefinition, int> ingredientCounts = new Dictionary<IngredientDefinition, int>();

        /// <summary>
        /// Current money the player has.
        /// </summary>
        public int money;

        // lista odblokowanych składników
        [Tooltip("Ingredients that are initially unlocked and can appear in the shop / UI.")]
        public List<IngredientDefinition> initiallyUnlockedIngredients = new List<IngredientDefinition>();

        public HashSet<IngredientDefinition> unlockedIngredients = new HashSet<IngredientDefinition>();


        /// <summary>
        /// Event fired when the inventory changes.  UI systems can listen
        /// to update displays.
        /// </summary>
        public event Action OnInventoryChanged;

        private void Awake()
        {
            InitialiseInventory();
        }

        /// <summary>
        /// Initialises the inventory using values from the startingInventoryDefinition.
        /// </summary>
        public void InitialiseInventory()
        {
            ingredientCounts.Clear();
            if (startingInventoryDefinition != null)
            {
                money = startingInventoryDefinition.startingMoney;
                foreach (var item in startingInventoryDefinition.startingIngredients)
                {
                    if (item.ingredient == null) continue;
                    ingredientCounts[item.ingredient] = item.quantity;
                    Debug.Log(" unlockedIngredients += " + item.ingredient);
                    unlockedIngredients.Add(item.ingredient); // jak mamy w startowym stanie, to też traktujemy jako odblokowany
          
                }
            }

            // dodatkowe początkowe odblokowania
            foreach (var ing in initiallyUnlockedIngredients)
            {
                if (ing != null)
                    unlockedIngredients.Add(ing);
            }
            OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// Returns the quantity of a specific ingredient currently available.
        /// </summary>
        public int GetQuantity(IngredientDefinition ingredient)
        {
            if (ingredientCounts.TryGetValue(ingredient, out int qty))
            {
                return qty;
            }
            return 0;
        }

        /// <summary>
        /// Attempts to add the specified ingredient and quantity to the inventory.
        /// </summary>
        public void AddIngredient(IngredientDefinition ingredient, int amount)
        {
            if (ingredient == null || amount <= 0) return;
            if (ingredientCounts.ContainsKey(ingredient))
                ingredientCounts[ingredient] += amount;
            else
                ingredientCounts[ingredient] = amount;
            OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// Attempts to remove a quantity of an ingredient from the inventory.  Returns
        /// true if successful.
        /// </summary>
        public bool ConsumeIngredient(IngredientDefinition ingredient, int amount)
        {
            if (ingredient == null || amount <= 0) return false;
            if (ingredientCounts.TryGetValue(ingredient, out int qty) && qty >= amount)
            {
                ingredientCounts[ingredient] = qty - amount;
                OnInventoryChanged?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to spend the given amount of money.  Returns true if the
        /// player had enough funds.
        /// </summary>
        public bool SpendMoney(int amount)
        {
            if (amount <= 0) return true;
            if (money >= amount)
            {
                money -= amount;
                OnInventoryChanged?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds money to the player’s balance.
        /// </summary>
        public void AddMoney(int amount)
        {
            if (amount <= 0) return;
            money += amount;
            OnInventoryChanged?.Invoke();
        }

        //ODBLOKOWYWANIE SKŁADNIKÓW (pod dialogi i sklep)

        public void UnlockIngredient(IngredientDefinition ingredient)
        {
            if (ingredient == null) return;

            if (unlockedIngredients.Add(ingredient))
            {
                Debug.Log($"Unlocked ingredient: {ingredient.ingredientName}");
                OnInventoryChanged?.Invoke();
            }
        }

        public bool IsIngredientUnlocked(IngredientDefinition ingredient)
        {
            return ingredient != null && unlockedIngredients.Contains(ingredient);
        }

        public List<IngredientDefinition> GetUnlockedIngredients()
        {
            return new List<IngredientDefinition>(unlockedIngredients);
        }

        //KUPNO SKŁADNIKA W SKLEPIKU

        public bool BuyIngredient(IngredientDefinition ingredient)
        {
            if (ingredient == null) return false;

        if (!IsIngredientUnlocked(ingredient))
        {
            Debug.Log($"Ingredient {ingredient.ingredientName} is not unlocked.");
            return false;
        }

        int cost = ingredient.cost; // zakładam, że w IngredientDefinition masz pole cost

        if (!SpendMoney(cost))
        {
            Debug.Log($"Not enough money to buy {ingredient.ingredientName} (cost: {cost}).");
            return false;
        }

        AddIngredient(ingredient, 1);
        return true;
    }
    }
}