using System;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialoguePersonName;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private DialogSO dialogue;
    private int dialogueIndex;

    public event Action<bool> OnDialogueVisible;
    public event Action OnDialogueFinished;
    public event Action OnDialogueLineUpdated;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);        
        dialoguePanel.SetActive(false);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0))
            NextLine();
    }

    public DialogSO GetDialog() => dialogue;
    public DialogLineSO GetCurrentLine() {
        if (dialogue == null) return null;
        if (dialogue.dialogLines == null || dialogue.dialogLines.Length == 0) return null;
        if (dialogueIndex < 0 || dialogueIndex >= dialogue.dialogLines.Length) return null;
        return dialogue.dialogLines[dialogueIndex];
    }
    public void SetDialogue(DialogSO newDialogue) {
        if (newDialogue == null) return;
        if (newDialogue.dialogLines == null || newDialogue.dialogLines.Length == 0) {
            Debug.LogError("Trying to start dialogue with NO LINES");
            return;
        }
        dialogue = newDialogue;
        dialogueIndex = 0;

        dialoguePanel.SetActive(true);
        OnDialogueVisible?.Invoke(true);
        Debug.Log("Dialogue is Set");
        UpdateDialogue();
    }

    private void UpdateDialogue() {
        if (dialogue == null) return;
        dialoguePersonName.text = PersonManager.Instance.CurrentPerson.personName;
        dialogueText.text = dialogue.dialogLines[dialogueIndex].dialogLine;
        OnDialogueLineUpdated?.Invoke();
    }
    public void NextLine() {
        Debug.Log("trying go to next line");
        if (dialogue == null) return;
        dialogueIndex++;
        Debug.Log("Dialogue is not null");
        if (dialogueIndex >= dialogue.dialogLines.Length) {
            EndDialogue();
            return;
        }
        Debug.Log("Got to Update Dialogue");
        UpdateDialogue();
    }
    public void EndDialogue() {
        dialoguePanel.SetActive(false);
        dialogue = null;
        dialogueIndex = 0;
        OnDialogueVisible?.Invoke(false);
        OnDialogueFinished?.Invoke();
    }

}
