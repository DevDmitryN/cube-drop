using Infastructure;
using UnityEngine;
using Zenject;

public class MainMenuHandler : MonoBehaviour
{
    private GameSettings _gameSettings;
    
    [Inject]
    private void Construct(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
    }
    
    public void OnStartClick()
    {
        _gameSettings.RestartLevel();
    }
}
