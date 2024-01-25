using UnityEngine;

namespace _Project.Scirpts.Game.Platforms
{
    public class DefaultPlatform : Platform
    {
        private float _multiply = 0f;
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayCubeController>(out var controller))
            {
                controller.AddMultiVelocity(_multiply);
            }
        }
    }
}