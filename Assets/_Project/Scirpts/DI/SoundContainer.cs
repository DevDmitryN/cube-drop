using UnityEngine;
using Zenject;

namespace _Project.Scirpts.DI
{
    public class SoundContainer : MonoInstaller
    {
        [SerializeField] private SoundManager _soundManager;
        
        public override void InstallBindings()
        {
            Container.Bind<SoundManager>().FromInstance(_soundManager).AsSingle();
        }
    }
}