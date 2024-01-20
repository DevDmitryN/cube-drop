using Infastructure;
using Levels;
using UnityEngine;
using Zenject;

public class CoinCounter : MonoBehaviour
{
    private int _currentCollectedCoinAmount = 0;
    
    private GamePlayHandler _gamePlayHandler;

    [Inject]
    private void Constructor(GamePlayHandler gamePlayHandler)
    {
        _gamePlayHandler = gamePlayHandler;
    }
    
    private void OnEnable()
    {
        Coin.OnCoinCollected += OnCoinCollected;
        GameSettings.OnLevelChanged += LevelChanged;
    }
    
    private void OnDisable()
    {
        Coin.OnCoinCollected -= OnCoinCollected;
        GameSettings.OnLevelChanged -= LevelChanged;
    }

    private void OnCoinCollected()
    {
        _gamePlayHandler.UpdateCounterText(++_currentCollectedCoinAmount);
    }

    private void LevelChanged(LevelInfo level)
    {
        _currentCollectedCoinAmount = 0;
        _gamePlayHandler.UpdateCounterText(_currentCollectedCoinAmount, level.CoinCount);
    }
}
