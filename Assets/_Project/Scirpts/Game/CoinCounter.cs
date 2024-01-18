using System.Linq;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCounterText;

    private int _totalCoinAmount = 0;

    private int _currentCollectedCoinAmount = 0;

    private void Awake()
    {
        var coins = FindObjectsOfType<Coin>();

        _totalCoinAmount = coins.Count();

        UpdateCounterText();
    }

    private void OnEnable()
    {
        Coin.OnCoinCollected += OnCoinCollected;
    }


    private void OnDisable()
    {
        Coin.OnCoinCollected -= OnCoinCollected;
    }

    private void OnCoinCollected()
    {
        _currentCollectedCoinAmount++;
        UpdateCounterText();
    }

    private void UpdateCounterText()
    {
        coinCounterText.text = $"{_currentCollectedCoinAmount} / {_totalCoinAmount}";
    }
}
