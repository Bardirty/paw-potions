using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PotionGame
{
    /// <summary>
    /// Handles progression through a dialogue tree at runtime.  The
    /// DialogueManager keeps track of the current node, processes node and
    /// choice actions and raises events to notify UI systems of updates.
    /// Variable names and public functions defined here are intended to be
    /// connected to UI controls by the designer.  For example, the UI can
    /// display currentNode.text and call SelectChoice(index) when the player
    /// clicks a response option.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        [Tooltip("Reference to the InventoryManager so actions can modify player resources.")]
        public InventoryManager inventoryManager;


        [Tooltip("Callback invoked whenever the current dialogue node changes.")]
        public event Action<DialogueNode> OnNodeChanged;

        /// <summary>
        /// The dialogue graph currently being executed.
        /// </summary>
        public DialogueGraph currentGraph;

        /// <summary>
        /// The node currently being displayed to the player.
        /// </summary>
        public DialogueNode currentNode;

        /// <summary>
        /// If true, conversation is waiting for the player to craft a requested potion.
        /// </summary>
        public bool awaitingPotion;

        /// <summary>
        /// The potion that the current node requests the player to craft.  When set
        /// to null there is no outstanding order.
        /// </summary>
        public PotionRecipe pendingPotionRequest;

        /// <summary>
        /// Starts a new conversation using the provided dialogue graph.  Resets
        /// state and executes the root node’s actions.
        /// </summary>
        public void StartConversation(DialogueGraph graph)
        {
            currentGraph = graph;
            awaitingPotion = false;
            pendingPotionRequest = null;

            if (currentGraph != null)
                currentNode = currentGraph.rootNode;
            else
                currentNode = null;

            if (currentNode != null)
                ExecuteNode(currentNode);
            else
                OnNodeChanged?.Invoke(null);
        }

        /// <summary>
        /// Called by UI when the player selects a choice.  Executes any
        /// actions on the choice, then transitions to the next node.  If
        /// the next node is null the conversation ends.
        /// </summary>
        /// <param name="choiceIndex">Index of the selected choice in currentNode.choices.</param>
        public void SelectChoice(int choiceIndex)
        {
            if (currentNode == null) return;
            if (choiceIndex < 0 || choiceIndex >= currentNode.choices.Count)
                return;
            var choice = currentNode.choices[choiceIndex];
            // Execute choice actions
            ExecuteActions(choice.actions);
            // Move to next node
            if (choice.nextNode != null)
            {
                currentNode = choice.nextNode;
                ExecuteNode(currentNode);
            }
            else
            {
                // End conversation
                EndConversation();
            }
        }

        /// <summary>
        /// Should be called by CauldronManager when the player finishes a
        /// potion.  If awaitingPotion is true and the produced recipe
        /// matches the requested potion, the conversation continues to the
        /// next node; otherwise the player should try again.
        /// </summary>
        public void OnPotionCrafted(PotionRecipe recipe)
        {
            // jeśli nie czekamy na potion – nic nas to nie obchodzi
            if (!awaitingPotion || pendingPotionRequest == null)
                return;

            // sprawdź, czy uwarzony potion to dokładnie ten, który został zamówiony
            if (recipe == pendingPotionRequest)
            {
                awaitingPotion = false;
                pendingPotionRequest = null;

                if (currentGraph != null && currentGraph.potionThankYouNode != null)
                {
                    // 🔥 tu DOKŁADNIE dzieje się to, o co prosisz:
                    currentNode = currentGraph.potionThankYouNode;
                    ExecuteNode(currentNode);   // to odpali OnNodeChanged → UI pokaże podziękowanie
                }
                else
                {
                    Debug.LogWarning("No potionThankYouNode set on current dialogue graph.");
                }
            }
            else
            {
                // tu możesz kiedyś dodać reakcję typu "zły eliksir" z osobnym node'em
                Debug.Log("Crafted potion does not match requested recipe.");
            }
        }


        /// <summary>
        /// Ends the current conversation.  Designers can hook into this to
        /// hide dialogue UI or trigger other game state changes.
        /// </summary>
        public void EndConversation()
        {
            // Clear pending request
            pendingPotionRequest = null;
            awaitingPotion = false;
            currentNode = null;
            OnNodeChanged?.Invoke(null);
        }

        /// <summary>
        /// Executes all actions associated with a dialogue node and then
        /// notifies listeners that the node has changed.  If the node
        /// contains a potion request, sets awaitingPotion and holds the
        /// conversation until OnPotionCrafted resumes it.
        /// </summary>
        private void ExecuteNode(DialogueNode node)
        {
            if (node == null)
                return;

            currentNode = node;

            // wykonaj akcje node'a (dodawanie kasy, odblokowanie składników itd.)
            if (node.actions != null && inventoryManager != null)
            {
                ExecuteActions(node.actions);
            }

            // 🔥 jeśli node zawiera zamówienie na potion – zapamiętujemy, że czekamy
            if (node.requestedPotion != null)
            {
                awaitingPotion = true;
                pendingPotionRequest = node.requestedPotion;
            }
            else
            {
                // w innym wypadku nie czekamy na potion
                awaitingPotion = false;
                pendingPotionRequest = null;
            }

            // Wyślij aktualny node do UI
            OnNodeChanged?.Invoke(node);
        }


        /// <summary>
        /// Executes a list of actions, modifying the player’s inventory or
        /// currency as required.  Additional custom behaviour can be
        /// implemented in a switch statement.
        /// </summary>
        private void ExecuteActions(List<DialogueAction> actions)
        {
            if (actions == null || actions.Count == 0)
                return;
            foreach (var action in actions)
            {
                switch (action.actionType)
                {
                    case DialogueActionType.AddIngredient:
                        if (action.ingredient != null)
                            inventoryManager.AddIngredient(action.ingredient, action.amount);
                        break;
                    case DialogueActionType.AddMoney:
                        inventoryManager.AddMoney(action.amount);
                        break;
                    case DialogueActionType.StartQuest:
                        // Start a quest or trigger any custom event.  The questName
                        // field can be used to identify which quest to start.
                        Debug.Log($"Quest started: {action.questName}");
                        break;
                    case DialogueActionType.EndConversation:
                        EndConversation();
                        break;
                    case DialogueActionType.UnlockIngredient:
                        if (action.ingredient != null)
                        {
                            inventoryManager.UnlockIngredient(action.ingredient);
                        }
                        break;
                }
            }
        }
    }
}