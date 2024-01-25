using UnityEngine;

namespace _Project.Scirpts.Game.Platforms
{
    public class SpeedPlatform : Platform
    {
        private float _multiply = 2f;
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayCubeController>(out var controller))
            {
                controller.AddMultiVelocity(_multiply);
            }
        }
    }
}