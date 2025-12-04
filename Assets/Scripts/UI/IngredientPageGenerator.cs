using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class IngredientPageGenerator : MonoBehaviour {
    [Header("Prefabs & containers")]
    [SerializeField] private IngredientLineVisualizer visualizerPattern;
    [SerializeField] private GameObject pagePrefab;                     
    [SerializeField] private GameObject pagesContainer;                 

    [Header("Paging")]
    [SerializeField] private int ingredientPageLimit = 4;

    public GameObject[] Pages { get; private set; }

    public event Action<GameObject[]> OnPagesGenerated;

    private void Start() {
        GeneratePages();
    }

    private void GeneratePages() {
        if (visualizerPattern == null || pagePrefab == null || pagesContainer == null) {
            Debug.LogError("IngredientPageGenerator: assign visualizerPattern, pagePrefab and pagesContainer in the inspector.");
            return;
        }

        var gm = PawtionsGameManager.Instance;
        if (gm == null || gm.Ingredients == null || gm.Ingredients.Count == 0) {
            Pages = Array.Empty<GameObject>();
            OnPagesGenerated?.Invoke(Pages);
            return;
        }

        int totalIngredients = gm.Ingredients.Count;
        int pagesCount = Mathf.CeilToInt((float)totalIngredients / ingredientPageLimit);
        Pages = new GameObject[pagesCount];

        int ingredientIndex = 0;

        for (int i = 0; i < pagesCount; i++) {
            GameObject page = Instantiate(pagePrefab, pagesContainer.transform);
            page.name = $"IngredientPage_{i}";
            Pages[i] = page;

            page.SetActive(false);

            for (int j = 0; j < ingredientPageLimit; j++) {
                if (ingredientIndex >= totalIngredients)
                    break;

                IngredientLineVisualizer ilv = Instantiate(visualizerPattern, page.transform);
                ilv.SetIngredient(gm.Ingredients[ingredientIndex].ingredient);
                ingredientIndex++;
            }
        }
        OnPagesGenerated?.Invoke(Pages);
    }
}
