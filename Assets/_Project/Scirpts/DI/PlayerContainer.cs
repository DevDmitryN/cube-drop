using UnityEngine;
using Zenject;

namespace _Project.Scirpts.DI
{
    public class PlayerContainer : MonoInstaller
    {
        [SerializeField] private PlayCubeController _playCubeController;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayCubeController>().FromInstance(_playCubeController).AsSingle();
        }
    }
}