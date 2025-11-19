using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PotionGame
{
    public class UIManager : MonoBehaviour
    {
        [Header("Core Managers")]
        public InventoryManager inventoryManager;
        public CauldronManager cauldronManager;
        public DialogueManager dialogueManager;
        public GameManager gameManager;

        [Header("Dialogue UI")]
        public TMP_Text speakerNameText;
        public TMP_Text dialogueBodyText;

        [Tooltip("Parent object for dialogue choice buttons (e.g. Vertical Layout Group).")]
        public Transform choicesContainer;

        [Tooltip("Prefab for a single choice button. Must have a Button + TMP_Text child.")]
        public Button choiceButtonPrefab;

        [Header("Inventory UI")]
        public TMP_Text moneyText;

        [Tooltip("Parent object where ingredient buttons will be created.")]
        public Transform ingredientsContainer;

        [Tooltip("Prefab for ingredient button. Must have Button + TMP_Text child.")]
        public Button ingredientButtonPrefab;

        [Tooltip("List of all ingredients that can appear as buttons.")]
        public List<IngredientDefinition> availableIngredients = new List<IngredientDefinition>();

        [Header("Cauldron UI")]
        [Tooltip("Optional: Text to show current ingredients in cauldron (for debugging / clarity).")]
        public TMP_Text cauldronContentsText;

        [Header("System Buttons (hook these from Inspector)")]
        public Button clearCauldronButton;
        public Button nextClientButton;
        public Button giveToClientButton;
        public Button mixButton;

        [Header("Shop UI")]
        public GameObject shopPanel;
        public Transform shopItemsContainer;
        public Button shopItemButtonPrefab;
        public TMP_Text shopInfoText;

        private void Awake()
        {
            // Subskrypcje eventów z managerów
            if (inventoryManager != null)
                inventoryManager.OnInventoryChanged += () =>
                {
                    RefreshInventoryUI();
                    RebuildIngredientButtons();
                };


            if (dialogueManager != null)
                dialogueManager.OnNodeChanged += RefreshDialogueUI;

            if (cauldronManager != null)
                cauldronManager.OnPotionCreated += OnPotionCreated;

            // Podpięcie przycisków, jeśli zostały przypisane w Inspectorze
            if (clearCauldronButton != null)
                clearCauldronButton.onClick.AddListener(OnClearCauldronButtonPressed);

            if (nextClientButton != null)
                nextClientButton.onClick.AddListener(OnNextClientButtonPressed);

            if (giveToClientButton != null)
                giveToClientButton.onClick.AddListener(OnGiveToClientButtonPressed);

            if (mixButton != null)
                mixButton.onClick.AddListener(OnMixButtonPressed); 
        }

        private void Start()
        {
            // Zbuduj UI przy starcie
            RebuildIngredientButtons();
            RefreshInventoryUI();

            if (shopPanel != null)
                shopPanel.SetActive(false);
        }

        public void OnGiveToClientButtonPressed()
        {
            if (cauldronManager == null)
                return;

            cauldronManager.GivePotionToClient();
        }

        public void OnMixButtonPressed()
        {
            if (cauldronManager == null)
                return;

            cauldronManager.MixCurrentIngredients();
            RefreshCauldronContentsUI(); // opcjonalnie, jeśli chcesz też tekst aktualizować
        }


        #region INVENTORY UI

        public void RefreshInventoryUI()
        {
            if (inventoryManager == null)
                return;

            if (moneyText != null)
                moneyText.text = $"$ {inventoryManager.money}";
        }

        public void RebuildIngredientButtons()
        {
            if (ingredientsContainer == null || ingredientButtonPrefab == null || inventoryManager == null)
                return;

            // wyczyść stare przyciski
            for (int i = ingredientsContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(ingredientsContainer.GetChild(i).gameObject);
            }

            // 🔹 BIERZEMY TYLKO ODBLOKOWANE SKŁADNIKI
            var unlockedIngredients = inventoryManager.GetUnlockedIngredients();
            if (unlockedIngredients == null || unlockedIngredients.Count == 0)
            {
                // opcjonalnie możesz wstawić tu jakiś „brak składników” tekst
                return;
            }

            foreach (var ingredient in unlockedIngredients)
            {
                if (ingredient == null) continue;

                Button btn = Instantiate(ingredientButtonPrefab, ingredientsContainer);
                TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
                if (label != null)
                {
                    int qty = inventoryManager.GetQuantity(ingredient);
                    label.text = $"{ingredient.ingredientName} x{qty}";
                }

                IngredientDefinition capturedIngredient = ingredient;

                btn.onClick.AddListener(() =>
                {
                    OnIngredientButtonPressed(capturedIngredient);
                });
            }
        }


        public void OnIngredientButtonPressed(IngredientDefinition ingredient)
        {
            if (ingredient == null || cauldronManager == null || inventoryManager == null)
                return;

            int quantity = inventoryManager.GetQuantity(ingredient);
            if (quantity <= 0)
            {
                Debug.Log($"Brak składnika: {ingredient.ingredientName}");
                return;
            }

            bool consumed = inventoryManager.ConsumeIngredient(ingredient, 1);
            if (!consumed)
                return;

            cauldronManager.AddIngredientToCauldron(ingredient);

            RefreshInventoryUI();
            RefreshCauldronContentsUI();
        }

        #endregion

        #region CAULDRON UI + BUTTONS

        public void OnClearCauldronButtonPressed()
        {
            if (cauldronManager == null)
                return;

            cauldronManager.EmptyCauldron();
            RefreshCauldronContentsUI();
        }

        public void OnCraftButtonPressed()
        {
            Debug.Log("Craft button pressed (opcjonalna logika).");
        }

        public void RefreshCauldronContentsUI()
        {
            if (cauldronContentsText == null || cauldronManager == null)
                return;

            var ingredients = cauldronManager.GetCurrentIngredients();
            if (ingredients == null || ingredients.Count == 0)
            {
                cauldronContentsText.text = "Kocioł: pusty";
                return;
            }

            List<string> names = new List<string>();
            foreach (var ing in ingredients)
            {
                if (ing != null)
                    names.Add(ing.ingredientName);
            }

            cauldronContentsText.text = "Kocioł: " + string.Join(", ", names);
        }

        private void OnPotionCreated(PotionRecipe recipe)
        {
            if (recipe != null)
            {
                Debug.Log($"Uwarzono miksturę: {recipe.potionName}");
                RefreshCauldronContentsUI();
            }

            if (dialogueManager != null)
            {
                dialogueManager.OnPotionCrafted(recipe);
            }
        }

        #endregion

        #region SHOP UI

        /// <summary>
        /// Pokazuje panel sklepiku i generuje listę przycisków dla odblokowanych składników.
        /// </summary>
        public void OpenShop()
        {
            if (shopPanel != null)
                shopPanel.SetActive(true);

            RebuildShopItems();
        }

        /// <summary>
        /// Chowa panel sklepiku.
        /// </summary>
        public void CloseShop()
        {
            if (shopPanel != null)
                shopPanel.SetActive(false);
        }

        /// <summary>
        /// Tworzy przyciski w sklepiku na podstawie odblokowanych składników.
        /// </summary>
        public void RebuildShopItems()
        {
            if (shopItemsContainer == null || shopItemButtonPrefab == null || inventoryManager == null)
                return;

            // wyczyść stare
            for (int i = shopItemsContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(shopItemsContainer.GetChild(i).gameObject);
            }

            var unlocked = inventoryManager.GetUnlockedIngredients();
            if (unlocked == null || unlocked.Count == 0)
            {
                if (shopInfoText != null)
                    shopInfoText.text = "Brak odblokowanych składników do kupienia.";
                return;
            }

            if (shopInfoText != null)
                shopInfoText.text = "";

            foreach (var ing in unlocked)
            {
                if (ing == null) continue;

                Button btn = Instantiate(shopItemButtonPrefab, shopItemsContainer);
                TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
                if (label != null)
                    label.text = $"{ing.ingredientName} - ${ing.cost}";

                IngredientDefinition captured = ing;

                btn.onClick.AddListener(() =>
                {
                    OnShopItemBuyPressed(captured);
                });
            }
        }

        /// <summary>
        /// Wywoływane po kliknięciu przycisku w sklepie dla konkretnego składnika.
        /// </summary>
        public void OnShopItemBuyPressed(IngredientDefinition ingredient)
        {
            if (inventoryManager == null || ingredient == null)
                return;

            bool success = inventoryManager.BuyIngredient(ingredient);
            if (shopInfoText != null)
            {
                if (success)
                    shopInfoText.text = $"Kupiono: {ingredient.ingredientName}";
                else
                    shopInfoText.text = $"Nie udało się kupić {ingredient.ingredientName}.";
            }

            RefreshInventoryUI();
            RebuildIngredientButtons(); // jeśli chcesz, żeby przyciski składników od razu pokazywały nowe stany
        }

        #endregion


        #region DIALOGUE UI

        public void RefreshDialogueUI(DialogueNode node)
        {
            if (node == null)
            {
                if (speakerNameText != null) speakerNameText.text = "";
                if (dialogueBodyText != null) dialogueBodyText.text = "";
                ClearChoices();
                return;
            }

            if (speakerNameText != null)
                speakerNameText.text = node.speakerName;

            if (dialogueBodyText != null)
                dialogueBodyText.text = node.text;

            RebuildChoices(node);
        }

        private void ClearChoices()
        {
            if (choicesContainer == null)
                return;

            for (int i = choicesContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(choicesContainer.GetChild(i).gameObject);
            }
        }

        private void RebuildChoices(DialogueNode node)
        {
            if (choicesContainer == null || choiceButtonPrefab == null)
                return;

            ClearChoices();

            if (node.choices == null || node.choices.Count == 0)
                return;

            for (int i = 0; i < node.choices.Count; i++)
            {
                int index = i;

                var choice = node.choices[i];
                Button btn = Instantiate(choiceButtonPrefab, choicesContainer);
                TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
                if (label != null)
                    label.text = choice.choiceText;

                btn.onClick.AddListener(() =>
                {
                    OnChoiceButtonPressed(index);
                });
            }
        }

        public void OnChoiceButtonPressed(int index)
        {
            if (dialogueManager == null)
                return;

            dialogueManager.SelectChoice(index);
        }

        public void OnNextClientButtonPressed()
        {
            if (gameManager == null)
                return;

            gameManager.BeginNextClient();
        }

        #endregion
    }
}
