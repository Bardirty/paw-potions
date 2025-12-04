using System;
using UnityEngine;

public class PotAnimator : MonoBehaviour
{
    private Animator _animator;

    private const string MIX_TRIGGER = "Mix";
    private const string ADD_TRIGGER = "Add";
    private const string GOOD_TRIGGER = "Good";
    private const string BAD_TRIGGER = "Bad";

    public event Action OnMixAnimationEnd;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void PlayMixAnimation() {
        _animator.SetTrigger(MIX_TRIGGER);
    }
    public void PlayAddAnimation()
    {
        _animator.SetTrigger(ADD_TRIGGER);
    }
    public void PlayGoodAnimation()
    {
        _animator.SetTrigger(GOOD_TRIGGER);
    }
    public void PlayBadAnimation()
    {
        _animator.SetTrigger(BAD_TRIGGER);
    }

    public void ToggleOnAddAnimaionAdd() {
        OnMixAnimationEnd?.Invoke();
    }
}
