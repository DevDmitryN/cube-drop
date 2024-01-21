using System;
using Infastructure;
using Levels;
using TMPro;
using UnityEngine;
using Zenject;

public class GamePlayHandler : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private TextMeshProUGUI _lifeCounterText;
    private readonly int _defaultLifeAmount = 3;
    private int _currentLifeAmount;
    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI _coinCounterText;
    private int _totalCoinAmount;
    [Header("GamePlay Bar")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameplayCanvas;
    [Header("Ended game Bar")]
    [SerializeField] private GameObject _levelsCanvas;
    [SerializeField] private GameObject _finishPanel;
    [SerializeField] private GameObject _deadPanel;
    
    private GameSettings _gameSettings;
    
    [Inject]
    private void Construct(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
    }

    private void OnEnable()
    {
        GameSettings.OnLevelChanged += LevelChanged;
        PlayCubeController.OnStateChanged += StateCubeChanged;
    }

    private void OnDisable()
    {
        GameSettings.OnLevelChanged -= LevelChanged;
        PlayCubeController.OnStateChanged += StateCubeChanged;
    }

    public void RestartLevel()
    {
        _gameSettings.RestartLevel();
    }

    public void NextLevel()
    {
        _gameSettings.NextLevel();
    }

    public void OpenPausePanel() 
    {
        _pausePanel.SetActive(true);
    }
    
    public void ClosePausePanel() 
    {
        _pausePanel.SetActive(false);
    }

    public void OpenMainMenu()
    {
        _gameSettings.OpenMainMenu();
    }

    private void LevelChanged(LevelInfo level)
    {
        _finishPanel.SetActive(false);
        _deadPanel.SetActive(false);
        if (level == default)
        {
            _gameplayCanvas.SetActive(false);
            _levelsCanvas.SetActive(false);
        
        }
        else
        {
            _gameplayCanvas.SetActive(true);
            _levelsCanvas.SetActive(true);
            _totalCoinAmount = level.CoinCount;
            _currentLifeAmount = _defaultLifeAmount;
            _lifeCounterText.text = _currentLifeAmount.ToString();
        }
    }

    public bool DecrementLife()
    {
        _currentLifeAmount--;
        _lifeCounterText.text = _currentLifeAmount.ToString();
        return _currentLifeAmount == 0;
    }

    public void UpdateCounterText(int currentCoin, int? totalCoins = null)
    {
        _coinCounterText.text = $"{currentCoin} / {totalCoins ?? _totalCoinAmount}";
    }

    private void StateCubeChanged(PlayCubeController.PlayCubeState state)
    {
        if (state == PlayCubeController.PlayCubeState.Finish)
        {
            _finishPanel.SetActive(true);
        }
        else if (state == PlayCubeController.PlayCubeState.Dead)
        {
            _deadPanel.SetActive(true);
        }
    }
}
