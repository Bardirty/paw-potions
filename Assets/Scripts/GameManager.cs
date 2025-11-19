using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionGame
{
    /// <summary>
    /// Coordinates the overall game flow.  The GameManager maintains the
    /// inventory, dialogue and cauldron systems and exposes public
    /// functions that the UI can call.  Designers can configure the list
    /// of clients (dialogue graphs) via the inspector.  At runtime, the
    /// GameManager selects a client at random, starts its dialogue and
    /// handles transitions when the player completes orders.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Tooltip("List of available client dialogues.  A random dialogue is selected when a new client arrives.")]
        public List<DialogueGraph> clients = new List<DialogueGraph>();

        [Tooltip("Reference to the CauldronManager responsible for potion mixing.")]
        public CauldronManager cauldronManager;

        [Tooltip("Reference to the DialogueManager responsible for conversations.")]
        public DialogueManager dialogueManager;

        [Tooltip("Reference to the InventoryManager handling resources.")]
        public InventoryManager inventoryManager;

        /// <summary>
        /// Current active client graph.  Used to verify correct order completion.
        /// </summary>
        private DialogueGraph activeClient;

        private void Start()
        {
            // Subscribe to events
            if (cauldronManager != null)
            {
                cauldronManager.OnPotionCreated += OnPotionCreated;
            }
            // Start with a random client
            BeginNextClient();
        }

        /// <summary>
        /// Called when a potion is crafted in the cauldron.  Checks whether the
        /// potion matches the current pending request in the dialogue.  If
        /// so, forwards the event to the dialogue manager to progress the
        /// conversation.
        /// </summary>
        private void OnPotionCreated(PotionRecipe recipe)
        {
            dialogueManager.OnPotionCrafted(recipe);
        }

        /// <summary>
        /// Selects a new client at random from the list of available clients
        /// and begins its dialogue.  Called when the current conversation
        /// ends or at the start of the game.
        /// </summary>
        public void BeginNextClient()
        {
            if (clients == null || clients.Count == 0 || dialogueManager == null)
            {
                Debug.LogWarning("Brak klientów lub DialogueManager niepodpięty.");
                return;
            }

            // Zbuduj listę klientów, których możemy obsłużyć
            List<DialogueGraph> availableClients = new List<DialogueGraph>();
            foreach (var graph in clients)
            {
                if (IsClientAvailable(graph))
                    availableClients.Add(graph);
            }

            if (availableClients.Count == 0)
            {
                Debug.LogWarning("Brak klientów, których zlecenie da się zrealizować z obecnie odblokowanych składników.");
                // tu możesz np. wyświetlić info w UI, że trzeba najpierw kupić/odblokować nowe składniki
                return;
            }

            // losujemy jednego z dostępnych klientów
            int index = UnityEngine.Random.Range(0, availableClients.Count);
            DialogueGraph nextClient = availableClients[index];

            dialogueManager.StartConversation(nextClient);
        }


        /// <summary>
        /// Sprawdza, czy dla danego klienta (DialogueGraph) wszystkie zamówione potiony
        /// składają się wyłącznie z odblokowanych składników.
        /// </summary>
        private bool IsClientAvailable(DialogueGraph graph)
        {
            Debug.Log("IsClientAvailable");
            if (graph == null)
                return false;

            // jeśli nie mamy inventoryManagera albo lista allNodes jest pusta – na wszelki wypadek uznajemy, że klient jest dostępny
            if (inventoryManager == null || graph.allNodes == null || graph.allNodes.Count == 0)
                return true;
            Debug.Log("inventoryManager");

            // dla każdego node'a w grafie sprawdzamy, czy ma requestedPotion
            foreach (var node in graph.allNodes)
            {
                Debug.Log("foreach");
                if (node == null || node.requestedPotion == null)
                    continue;

                var recipe = node.requestedPotion;
                if (recipe.ingredients == null)
                    continue;

                // jeśli jakiś składnik z receptury NIE jest odblokowany – klient odpada
                foreach (var ing in recipe.ingredients)
                {
                    if (ing == null)
                    {
                        Debug.Log(ing);
                        continue;
                    }
                        

                    if (!inventoryManager.IsIngredientUnlocked(ing))
                    {
                        // tego klienta nie obsłużymy w aktualnym stanie gry
                        return false;
                    }
                }
            }

            // jeśli żadna receptura klienta nie wymaga zablokowanego składnika – jest OK
            return true;
        }

    }
}