using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionGame
{
    /// <summary>
    /// A ScriptableObject that defines the player’s initial inventory and
    /// starting money.  Designers can create an asset from this class and
    /// tweak quantities per ingredient directly in the inspector.  Runtime
    /// systems can also reference this asset to initialise the player state.
    /// </summary>
    [CreateAssetMenu(menuName = "Potion Game/Player Inventory", fileName = "PlayerInventory")]
    public class PlayerInventoryDefinition : ScriptableObject
    {
        [Tooltip("Starting money available to the player.")]
        public int startingMoney = 0;

        [Tooltip("List of ingredient quantities the player starts with.")]
        public List<InventoryItem> startingIngredients = new List<InventoryItem>();
    }
}