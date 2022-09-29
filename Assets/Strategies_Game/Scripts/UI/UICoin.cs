using TMPro;
using UnityEngine;

public class UICoin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textCoin;

    private void PrintTextCoin(int coin) => _textCoin.text = coin.ToString();

    private void OnEnable() => EventManager.OnPrintCoin += PrintTextCoin;

    private void OnDisable() => EventManager.OnPrintCoin -= PrintTextCoin; 
}
