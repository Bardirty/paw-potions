using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedPotionVisualizer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private DragndropableItem dragItem;
    private PotionSO potion;
    private bool isOverPerson = false;
    private PersonController personRef = null;
    private void Start() {
        dragItem = GetComponent<DragndropableItem>();
        if (PawtionsGameManager.Instance != null && PawtionsGameManager.Instance.CreatedPotion != null) {
            potion = PawtionsGameManager.Instance.CreatedPotion;
            _spriteRenderer.sprite = potion.potionSprite;
            return;
        }
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out PersonController person)) {
            isOverPerson = true;
            personRef = person;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.TryGetComponent(out PersonController person)) {
            isOverPerson = false;
            personRef = null;
        }
    }
    private void OnMouseUp() {
        if (isOverPerson && personRef != null && potion != null) {
            personRef.CheckPotion(potion);
            if(PawtionsGameManager.Instance != null)
                PawtionsGameManager.Instance.UnSetPotion();
            Destroy(gameObject);
        }
    }
}
