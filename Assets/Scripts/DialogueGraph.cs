using System.Collections.Generic;
using UnityEngine;

namespace PotionGame
{
    [CreateAssetMenu(menuName = "Potion Game/Dialogue Graph", fileName = "DialogueGraph")]
    public class DialogueGraph : ScriptableObject
    {
        [Tooltip("Root node of this dialogue.")]
        public DialogueNode rootNode;

        [Tooltip("Optional list of all nodes belonging to this graph, for organisation.")]
        public List<DialogueNode> allNodes = new List<DialogueNode>();

        [Tooltip("Node that will be shown when the player brews the correct potion for this client.")]
        public DialogueNode potionThankYouNode;
    }
}
