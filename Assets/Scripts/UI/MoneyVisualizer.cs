using TMPro;
using UnityEngine;

public class MoneyVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    private void Awake() {
        UpdateMoney();
    }

    public void UpdateMoney() {
        if(PawtionsGameManager.Instance != null) {
            _moneyText.text = PawtionsGameManager.Instance.GetMoney().ToString();
        }
    }
}
