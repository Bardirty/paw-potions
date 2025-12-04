using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : MonoBehaviour
{
    private PersonSO person;
    private void Start() {
        if (PersonManager.Instance != null) {
            PersonManager.Instance.OnPersonSelected += SetPerson;
            SetPerson(PersonManager.Instance.CurrentPerson);
        }
    }

    private void OnDestroy() {
        if (PersonManager.Instance != null)
            PersonManager.Instance.OnPersonSelected -= SetPerson;
    }

    private void SetPerson(PersonSO p) {
        person = p;
    }

    public void CheckPotion(PotionSO potion) {
        PersonManager pm = PersonManager.Instance;
        if(person != null && pm != null) {
            if(potion == person.correctPotion) pm.CorrectPotion();
            else pm.IncorrectPotion();
        }
    }
}
