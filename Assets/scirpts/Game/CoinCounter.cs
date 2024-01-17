using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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

    public void OnCoinCollected()
    {
        _currentCollectedCoinAmount++;
        UpdateCounterText();
    }

    private void UpdateCounterText()
    {
        coinCounterText.text = $"{_currentCollectedCoinAmount} / {_totalCoinAmount}";
    }
}
