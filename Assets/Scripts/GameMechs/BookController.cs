using System;
using UnityEngine;
using UnityEngine.UI;

public class BookController : MonoBehaviour {
    [Header("Book")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [Header("Ingredients")]
    [SerializeField] private GameObject ingredientsContainer;
    private GameObject[] ingredientsPages;
    [SerializeField] private Button ingredientsPin;

    [Header("Potions")]
    [SerializeField] private GameObject potionsContainer;
    private GameObject[] potionsPages;
    [SerializeField] private Button potionsPin;

    private int pageIndex = 0;
    private GameObject activePage;

    private IngredientPageGenerator ingredientPageGenerator;
    private PotionPageGenerator potionPageGenerator;

    private void Awake() {
        ingredientsPin?.onClick.AddListener(ShowIngredients);
        potionsPin?.onClick.AddListener(ShowPotions);
        previousButton?.onClick.AddListener(PreviousPage);
        nextButton?.onClick.AddListener(NextPage);

        ingredientPageGenerator = FindObjectOfType<IngredientPageGenerator>();
        if (ingredientPageGenerator != null) {
            ingredientPageGenerator.OnPagesGenerated += OnIngredientPagesGenerated;
            if (ingredientPageGenerator.Pages != null && ingredientPageGenerator.Pages.Length > 0) {
                OnIngredientPagesGenerated(ingredientPageGenerator.Pages);
            }
        }
        else TryCollectIngredientPagesFromContainer();
        potionPageGenerator = FindObjectOfType<PotionPageGenerator>();
        if (potionPageGenerator != null) {
            potionPageGenerator.OnPagesGenerated += OnPotionPagesGenerated;
            if (potionPageGenerator.Pages != null && potionPageGenerator.Pages.Length > 0) {
                OnPotionPagesGenerated(potionPageGenerator.Pages);
            }
        }
        else TryCollectPotionsPagesFromContainer();
        gameObject.SetActive(false);
    }
    private void Start() {
        ShowIngredients();
    }
    private void OnDestroy() {
        if (ingredientPageGenerator != null)
            ingredientPageGenerator.OnPagesGenerated -= OnIngredientPagesGenerated;
        if (potionPageGenerator != null)
            potionPageGenerator.OnPagesGenerated -= OnPotionPagesGenerated;
    }

    private void OnIngredientPagesGenerated(GameObject[] pages) {
        ingredientsPages = pages ?? Array.Empty<GameObject>();
        foreach (var p in ingredientsPages)
            if (p != null) p.SetActive(false);

        pageIndex = 0;
        activePage = null;

        if (gameObject.activeSelf)
            ShowIngredients();
    }

    private void OnPotionPagesGenerated(GameObject[] pages) {
        potionsPages = pages ?? Array.Empty<GameObject>();
        foreach (var p in potionsPages)
            if (p != null) p.SetActive(false);

        pageIndex = 0;
        activePage = null;

        if (gameObject.activeSelf)
            ShowPotions();
    }

    private void TryCollectIngredientPagesFromContainer() {
        if (ingredientsContainer == null)
            return;
        int childCount = ingredientsContainer.transform.childCount;
        if (childCount == 0)
            return;
        ingredientsPages = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
            ingredientsPages[i] = ingredientsContainer.transform.GetChild(i).gameObject;
    }
    private void TryCollectPotionsPagesFromContainer() {
        if (potionsContainer == null)
            return;
        int childCount = potionsContainer.transform.childCount;
        if (childCount == 0)
            return;
        potionsPages = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
            potionsPages[i] = potionsContainer.transform.GetChild(i).gameObject;
    }
    private void ShowIngredients() => ShowContainer(ingredientsContainer, ingredientsPages);
    private void ShowPotions() => ShowContainer(potionsContainer, potionsPages);
    private void ShowContainer(GameObject container, GameObject[] pages) {
        if (container == null || pages == null || pages.Length == 0)
            return;
        ingredientsContainer?.SetActive(false);
        potionsContainer?.SetActive(false);
        container.SetActive(true);
        foreach (GameObject go in pages)
            if (go != null) go.SetActive(false);
        pageIndex = Mathf.Clamp(pageIndex, 0, pages.Length - 1);
        pageIndex = 0;
        activePage = pages[pageIndex];
        if (activePage != null) activePage.SetActive(true);
    }

    private void NextPage() {
        GameObject[] pages = GetActivePagesArray();
        if (pages == null || pages.Length == 0) return;
        pageIndex++;
        if (pageIndex >= pages.Length) pageIndex = 0;
        ChangePage(pages);
    }

    public void PreviousPage() {
        GameObject[] pages = GetActivePagesArray();
        if (pages == null || pages.Length == 0) return;
        pageIndex--;
        if (pageIndex < 0) pageIndex = pages.Length - 1;
        ChangePage(pages);
    }

    private GameObject[] GetActivePagesArray() {
        if (ingredientsContainer != null && ingredientsContainer.activeSelf) return ingredientsPages;
        if (potionsContainer != null && potionsContainer.activeSelf) return potionsPages;
        return null;
    }

    private void ChangePage(GameObject[] pages) {
        if (activePage != null) activePage.SetActive(false);
        pageIndex = Mathf.Clamp(pageIndex, 0, pages.Length - 1);
        activePage = pages[pageIndex];
        if (activePage != null) activePage.SetActive(true);
        if(SoundManager.Instance != null) SoundManager.Instance.PlayPageSwap();
    }
}
