using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Project.Scirpts.Game.Platforms
{
    public class PerpendicularPlatform : Platform
    {
        
        [SerializeField] private float _velocityMultiplier = 20;

        [Inject] private SoundManager _soundManager;

        private Transform _transform;
        
        private Vector3 _direction = Vector3.left;

        private Material _material;

        private Color _defaultColor;

        private Animator _animator;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _material = GetComponent<Renderer>().material;
            _defaultColor = _material.color;
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            var directionAngle = Mathf.Deg2Rad * (_transform.eulerAngles.z + 90);

            var directionX = Mathf.Cos(directionAngle);
            var directionY = Mathf.Sin(directionAngle);

            _direction = new Vector3(directionX, directionY, 0);
        }
        
        private void OnDisable()
        {
            DOTween.Kill(_material);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayCubeController>(out var controller))
            {
                _animator.SetTrigger("TrPlay");
                _material.DOColor(Color.yellow,0.1f)
                    .OnComplete(() =>
                    {
                        _material.DOColor(_defaultColor, 0.2f);
                    });
                var velocity = _direction * _velocityMultiplier;
                
                controller.SetVelocity(velocity);
                _soundManager.Jump();
                
                Debug.DrawLine(_transform.position, velocity, Color.blue, 1);
            }
        }

       
    }
}