// This file defines the core data structures and runtime systems for a simple
// potion‑maker visual novel game.  The goal of the design is to decouple
// game logic from presentation so that designers can author content from
// Unity’s inspector or a custom editor window while artists build UI.

using System;
using System.Collections.Generic;
using UnityEngine;

// Place all game code inside a dedicated namespace to avoid name clashes.
namespace PotionGame
{
    #region Ingredient and Inventory Definitions
    /// <summary>
    /// Describes a single ingredient type.  Designers create instances of this
    /// class as assets (via the CreateAssetMenu attribute) to define the name,
    /// icon and other metadata for each ingredient that can appear in the
    /// game.  Because this class derives from ScriptableObject it is
    /// serializable and can be referenced by other assets without having to
    /// duplicate data.  ScriptableObjects act as data containers; the Unity
    /// manual notes that they are commonly used to store shared data across
    /// multiple objects and reduce memory usage by avoiding duplicate values【790020272590712†L86-L112】.
    /// </summary>
    [CreateAssetMenu(menuName = "Potion Game/Ingredient Definition", fileName = "NewIngredient")]
    public class IngredientDefinition : ScriptableObject
    {
        [Tooltip("Human readable name of the ingredient.")]
        public string ingredientName;

        [Tooltip("Sprite used to represent this ingredient in the UI.")]
        public Sprite icon;

        [Tooltip("Purchase cost of this ingredient in in‑game currency.")]
        public int cost;

        [Tooltip("Default quantity of this ingredient given to the player at game start.")]
        public int defaultQuantity = 0;
    }

    /// <summary>
    /// Stores a specific ingredient and an amount.  This class is serializable
    /// so that a list of starting inventory can be defined in the inspector.
    /// </summary>
    [Serializable]
    public class InventoryItem
    {
        public IngredientDefinition ingredient;
        public int quantity;
    }


    #endregion



    #region Dialogue System
    /// <summary>
    /// Enumerates the kinds of actions that can be triggered from a dialogue
    /// node or choice.  Actions allow designers to hook gameplay events into
    /// the narrative.  These are deliberately generic; new cases can be
    /// added as needed.  In the sample runtime, the DialogueManager
    /// interprets these actions to modify inventory or currency.
    /// </summary>
    public enum DialogueActionType
    {
        None,
        AddIngredient,
        AddMoney,
        UnlockIngredient,
        StartQuest,
        EndConversation
    }

    /// <summary>
    /// Defines a single action to perform when a dialogue node is reached or a
    /// choice is selected.  Depending on the action type, either the
    /// ingredient, amount or money fields are used.  Multiple actions can be
    /// assigned to a node or choice to build complex behaviour.
    /// </summary>
    [Serializable]
    public class DialogueAction
    {
        public DialogueActionType actionType = DialogueActionType.None;

        [Tooltip("Ingredient involved in this action (if applicable).")]
        public IngredientDefinition ingredient;

        [Tooltip("Number of ingredients to add or amount of money to give.")]
        public int amount;

        [Tooltip("If true, unlocks the ingredient so it can appear in the shop.")]
        public bool unlock = false;

        [Tooltip("Name of quest or narrative marker.")]
        public string questName;
    }

    /// <summary>
    /// A choice presented to the player within a dialogue node.  Each choice
    /// specifies the text to display, optional actions to perform when
    /// selected, and the next dialogue node to jump to.  The next node is
    /// referenced by a field rather than an index so that designers can
    /// easily link nodes by dragging references in the inspector.
    /// </summary>
    [Serializable]
    public class DialogueChoice
    {
        [Tooltip("Text displayed to the player for this choice.")]
        public string choiceText;

        [Tooltip("Next node to visit when this choice is selected.  Leave optional to end the conversation.")]
        public DialogueNode nextNode;

        [Tooltip("Actions executed when this choice is selected.")]
        public List<DialogueAction> actions = new List<DialogueAction>();
    }
 #endregion
 



 

    #region Editor Extensions (moved to a separate file)
    // Editor code has been moved to PotionGameEditor.cs to keep runtime and
    // editor functionality separate.  See that file for custom inspectors.
    #endregion
}