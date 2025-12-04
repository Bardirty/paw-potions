using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersonManager : MonoBehaviour {
    public const string MAIN_MENU_SCENE_KEY = "MainMenuScene";
    [SerializeField] private List<PersonSO> persons;
    private PersonSO currentPerson;
    public PersonSO CurrentPerson => currentPerson;

    public event Action<PersonSO> OnPersonSelected;
    public event Action OnPersonsEnded;

    public static PersonManager Instance { get; private set; }

    private bool waitingForFinalDialogue = false;

    private int positiveMoney = 7;
    private int negativeMoney = 2;

    private void Awake() {
        if (SceneManager.GetActiveScene().name == MAIN_MENU_SCENE_KEY) {
            Destroy(gameObject);
            return;
        }
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        SetNewPerson();
        DialogueManager.Instance.OnDialogueFinished += OnDialogueFinished;
    }
    private void OnDestroy() {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueFinished -= OnDialogueFinished;
    }
    private void Pay(bool isGood) {
        Debug.Log("Payment confirmed");
        SoundManager.Instance?.PlayPayment();
        if(isGood)
            PawtionsGameManager.Instance?.AddMoney(positiveMoney);
        else PawtionsGameManager.Instance?.AddMoney(negativeMoney); 
        StaticUIManager.Instance?.UpdateMoney();
    }
    public void SetNewPerson() {
        if (persons.Count == 0) {
            OnPersonsEnded?.Invoke();
            return;
        }
        int index = UnityEngine.Random.Range(0, persons.Count);
        currentPerson = persons[index];
        persons.RemoveAt(index);

        OnPersonSelected?.Invoke(currentPerson);

        waitingForFinalDialogue = false;
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayMusic(currentPerson.personTheme);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayBell();
        DialogueManager.Instance.SetDialogue(currentPerson.interludeDialog);
    }
    public void CorrectPotion() {
        Pay(true);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayEnjoyedClient();
        StartFinalDialogue(currentPerson.positiveDialog);
    }
    public void IncorrectPotion() {
        Pay(false);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayNotEnjoyedClient();
        StartFinalDialogue(currentPerson.negativeDialog);
    }

    private void StartFinalDialogue(DialogSO dialogue) {
        waitingForFinalDialogue = true;
        DialogueManager.Instance.SetDialogue(dialogue);
    }

    private void OnDialogueFinished() {
        if (!waitingForFinalDialogue) return;

        waitingForFinalDialogue = false;
        SetNewPerson();
    }
}
