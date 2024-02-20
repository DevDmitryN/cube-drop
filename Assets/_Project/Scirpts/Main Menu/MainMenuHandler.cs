using Infastructure;
using UnityEngine;
using Zenject;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject _continueGameButton;
    private GameSettings _gameSettings;
    
    
    [Inject]
    private void Construct(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
    }

    private void OnEnable()
    {
        _continueGameButton.SetActive(_gameSettings.SaveLevel != 0);
    }

    public void StartGame()
    {
        _gameSettings.LastOpenLevel();
    }

    public void ContinueGame()
    {
        _gameSettings.LastOpenLevel();
    }
}
