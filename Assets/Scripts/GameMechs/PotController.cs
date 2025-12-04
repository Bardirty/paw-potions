using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour
{
    [SerializeField] private PotAnimator potAnimator;
    [SerializeField] private List<IngredientSO> usedIngredients;


    private void Start()
    {
        usedIngredients = new List<IngredientSO>();
        potAnimator.OnMixAnimationEnd += CheckPot;
    }

    public void PutItem(IngredientSO ingredient)
    {
        usedIngredients ??= new List<IngredientSO>();
        usedIngredients.Add(ingredient);
        potAnimator.PlayAddAnimation();
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySplash();
    }

    public void MixItems() {
        potAnimator.PlayMixAnimation();
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayMixing();
    }

    public void CheckPot() {
        if (usedIngredients == null || usedIngredients.Count == 0) {
            BadRecipe();
            return;
        }
        if(PawtionsGameManager.Instance == null)
            return;
        foreach (var potion in PawtionsGameManager.Instance.Potions) {
            if (IsRecipeMatch(potion)) {
                GoodRecipe();
                if (PawtionsGameManager.Instance != null) {
                    PawtionsGameManager.Instance.SetCreatedPotion(potion);
                }
                usedIngredients.Clear();
                return;
            }
        }
        BadRecipe();
        usedIngredients.Clear();
    }
    private void OnMouseDown() {
        if(StaticUIManager.Instance.IsGameNotInteractable())
            return;
        MixItems();
    }
    private bool IsRecipeMatch(PotionSO potion) {
        if (potion.ingredients.Length != usedIngredients.Count)
            return false;

        List<IngredientSO> buffer = new(usedIngredients);

        foreach (var ing in potion.ingredients) {
            if (!buffer.Remove(ing))
                return false;
        }

        return true;
    }

    public void GoodRecipe() {
        potAnimator.PlayGoodAnimation();
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayGoodPotion();
    }
    public void BadRecipe() {
        potAnimator.PlayBadAnimation();
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayBadPotion();
    }

}
