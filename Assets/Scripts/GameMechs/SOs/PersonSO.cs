using UnityEngine;

[CreateAssetMenu(fileName = "New Person")]
public class PersonSO : ScriptableObject {
    public string personName;
    public Sprite[] personSprites;
    public DialogSO interludeDialog;
    public DialogSO positiveDialog;
    public DialogSO negativeDialog;
    public PotionSO correctPotion;
    public AudioClip personTheme;
}
