using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO")]
public class SoundSO : ScriptableObject {
    [Header("Pot")]
    public AudioClip boiling;
    public AudioClip mixing;
    public AudioClip splash;
    public AudioClip goodPotion;
    public AudioClip badPotion;
    public AudioClip fire;

    public AudioClip bell;
    [Header("UI")]
    public AudioClip GUIpress;
    public AudioClip bookOpen;
    public AudioClip bookClose;
    public AudioClip pageSwap;

    [Header("Gameplay")]
    public AudioClip dayStart;
    public AudioClip dayEnd;
    public AudioClip enjoyedClient;
    public AudioClip notEnjoyedClient;
    public AudioClip payment;

}
