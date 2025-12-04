using UnityEngine;

public class PersonSpriteManager : MonoBehaviour {
    private PersonSO currentPerson;
    [SerializeField] private SpriteRenderer personSprite;
    private void Start() {
        PersonManager.Instance.OnPersonSelected += SetPerson;
        if (PersonManager.Instance.CurrentPerson != null)
            SetPerson(PersonManager.Instance.CurrentPerson);
        DialogueManager.Instance.OnDialogueLineUpdated += UpdatePersonSprite;
        UpdatePersonSprite();
    }

    private void OnDestroy() {
        if (PersonManager.Instance != null)
            PersonManager.Instance.OnPersonSelected -= SetPerson;
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueLineUpdated -= UpdatePersonSprite;
    }

    private void SetPerson(PersonSO person) {
        currentPerson = person;
        Debug.Log("PersonSet - entry");
    }
    private void UpdatePersonSprite() {
        if (currentPerson == null) return;
        if (DialogueManager.Instance == null) return;
        Debug.Log("Dialogue Manager is available and not null");
        var dlg = DialogueManager.Instance.GetDialog();
        if (dlg == null || dlg.dialogLines == null || dlg.dialogLines.Length == 0) {
            if (currentPerson != null)
                personSprite.sprite = currentPerson.personSprites[0];
            return;
        }

        var line = DialogueManager.Instance.GetCurrentLine();

        if (line == null) {
            personSprite.sprite = currentPerson.personSprites[0];
            return;
        }
        int index = line.characterSpriteIndex;
        if (index < 0 || index >= currentPerson.personSprites.Length) {
            Debug.LogWarning($"Sprite index {index} out of range for {currentPerson.personName}");
            return;
        }
        personSprite.sprite = currentPerson.personSprites[index];
    }
}
