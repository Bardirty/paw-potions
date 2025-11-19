using System.Collections.Generic;
using UnityEngine;

namespace PotionGame
{
    [CreateAssetMenu(menuName = "Potion Game/Dialogue Node", fileName = "DialogueNode")]
    public class DialogueNode : ScriptableObject
    {
        [Tooltip("Speaker name shown in UI.")]
        public string speakerName;

        [Tooltip("The text displayed at this node.")]
        [TextArea]
        public string text;

        [Tooltip("List of actions executed when this node is reached.")]
        public List<DialogueAction> actions = new List<DialogueAction>();

        [Tooltip("List of choices presented to the player.")]
        public List<DialogueChoice> choices = new List<DialogueChoice>();

        [Tooltip("If not null, this node issues an order for a potion. The player must craft this recipe.")]
        public PotionRecipe requestedPotion;
    }
}
