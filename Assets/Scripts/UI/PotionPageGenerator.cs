using System;
using System.Linq;
using UnityEngine;

public class PotionPageGenerator : MonoBehaviour
{
    [Header("Prefabs & containers")]
    [SerializeField] private PotionLineVisualizer visualizerPattern;
    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private GameObject pagesContainer;

    [Header("Paging")]
    [SerializeField] private int potionPageLimit = 3;

    public GameObject[] Pages { get; private set; }
    public event Action<GameObject[]> OnPagesGenerated;

    private void Start() {
        GeneratePages();
    }

    private void GeneratePages() {
        if (visualizerPattern == null || pagePrefab == null || pagesContainer == null) {
            Debug.LogError("PotionPageGenerator: assign visualizerPattern, pagePrefab and pagesContainer in the inspector.");
            return;
        }
        var gm = PawtionsGameManager.Instance;
        if (gm == null || gm.Potions == null || gm.Potions.Length == 0) {
            Pages = Array.Empty<GameObject>();
            OnPagesGenerated?.Invoke(Pages);
            return;
        }
        int totalPotions = gm.Potions.Length;
        int pagesCount = Mathf.CeilToInt((float)totalPotions / potionPageLimit);
        Pages = new GameObject[pagesCount];

        int potionIndex = 0;

        for (int i = 0; i < pagesCount; i++) {
            GameObject page = Instantiate(pagePrefab, pagesContainer.transform);
            page.name = $"PotionPage_{i}";
            Pages[i] = page;
            page.SetActive(false);

            for (int j = 0; j < potionPageLimit; j++) {
                if (potionIndex >= totalPotions)
                    break;

                PotionLineVisualizer plv = Instantiate(visualizerPattern, page.transform);
                plv.SetPotion(gm.Potions[potionIndex]);
                potionIndex++;
            }
        }
        OnPagesGenerated?.Invoke(Pages);
    }
}
