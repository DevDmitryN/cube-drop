using Infastructure;
using UnityEngine;
using Zenject;

namespace _Project.Scirpts.DI
{
    public class GameContainer : MonoInstaller
    {
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private GamePlayHandler _gamePlayHandler;
        
        public override void InstallBindings()
        {
            Container.Bind<GameSettings>().FromInstance(_gameSettings).AsSingle();
            Container.Bind<GamePlayHandler>().FromInstance(_gamePlayHandler).AsSingle();
        }
    }
}