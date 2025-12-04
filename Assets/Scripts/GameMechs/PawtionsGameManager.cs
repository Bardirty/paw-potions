using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IngredientEntry {
    public IngredientSO ingredient;
    public int amount;

    public void IncrementAmount() => ++amount;
    public void DecrementAmount() => --amount;
}

public class PawtionsGameManager : MonoBehaviour
{
    [SerializeField] private List<IngredientEntry> ingredients;
    public List<IngredientEntry> Ingredients => ingredients;

    [SerializeField] private PotionSO[] potions;
    public PotionSO[] Potions => potions;
    public static PawtionsGameManager Instance { get; private set; }

    private PotionSO createdPotion;
    public PotionSO CreatedPotion => createdPotion;

    [SerializeField] private int money = 13;
    public int GetMoney() => money;
    public void AddMoney(int moneyCount) {
        Debug.Log("Money add");
        money += moneyCount;
    }

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public int GetIngredientAmount(IngredientSO ingredient) {
        foreach (IngredientEntry ie in Ingredients)
            if (ie.ingredient == ingredient) 
                return ie.amount;
        return 0;
    }
    public int GetIngredientIndex(IngredientSO ingredient) {
        if (Ingredients.Count == 0) {
            Debug.LogError("Ingredients dictionary is empty");
            return -1;
        }
        for (int i = 0; i < Ingredients.Count; i++)
            if (Ingredients[i].ingredient == ingredient)
                return i;
        Debug.LogError("Ingredient wasn't found in dictionary");
        return -1;
    }
    public void Buy(IngredientSO ingredient) {
        if (ingredient.price <= money) {
            money -= ingredient.price;
            IncreaseIngredient(ingredient);
            StaticUIManager.Instance?.UpdateMoney();
        }
    }
    public void DecreaseIngredient(IngredientSO ingredient) {
        int index = GetIngredientIndex(ingredient);
        IngredientEntry entry = Ingredients[index];
        if (entry.amount > 0)
            entry.amount--;
        Ingredients[index] = entry;
    }
    public void IncreaseIngredient(IngredientSO ingredient) {
        int index = GetIngredientIndex(ingredient);
        IngredientEntry entry = Ingredients[index];
        entry.amount++;
        Ingredients[index] = entry;
    }

    public void UnSetPotion() => createdPotion = null;
    public void SetCreatedPotion(PotionSO potionSO) => createdPotion = potionSO;

}
