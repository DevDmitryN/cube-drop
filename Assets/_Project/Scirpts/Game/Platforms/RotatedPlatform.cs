using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scirpts.Game.Platforms
{
    public class RotatedPlatform : Platform
    {
        [SerializeField] private float _rotationSpeed = 1;

        [SerializeField] private bool _isClockwiseDirection = false;
        
        
        private void OnEnable()
        {
            var directionMultiplier = _isClockwiseDirection ? -1 : 1;
            transform.DORotate(new Vector3(0,0, 360 * directionMultiplier), _rotationSpeed, RotateMode.FastBeyond360)
                .SetRelative()
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Fixed)
                .SetLoops(-1, LoopType.Incremental);
        }

        private void OnDisable()
        {
            DOTween.Kill(transform);
        }

        protected override void OnTriggerEnter(Collider other)
        {
        
        }
    }
}
