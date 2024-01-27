using System;
using UnityEngine;

namespace _Project.Scirpts.Game.Platforms
{
    public class PerpendicularPlatform : Platform
    {

        [SerializeField] private float _velocityMultiplier = 20;

        private Transform _transform;
        
        private Vector3 _direction = Vector3.left;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void OnEnable()
        {
            var directionAngle = Mathf.Deg2Rad * (_transform.eulerAngles.z + 90);

            var directionX = Mathf.Cos(directionAngle);
            var directionY = Mathf.Sin(directionAngle);

            _direction = new Vector3(directionX, directionY, 0);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayCubeController>(out var controller))
            {
                controller.SetVelocity(_direction * _velocityMultiplier);
            }
        }

       
    }
}