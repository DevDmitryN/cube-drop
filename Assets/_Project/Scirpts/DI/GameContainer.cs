using Infastructure;
using UnityEngine;
using Zenject;

namespace _Project.Scirpts.DI
{
    public class GameContainer : MonoInstaller
    {
        [SerializeField] private GameSettings _gameSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<GameSettings>().FromInstance(_gameSettings).AsSingle();
        }
    }
}