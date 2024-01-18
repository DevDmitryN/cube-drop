using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Coin : MonoBehaviour
{
    public static event Action OnCoinCollected;
    
    [SerializeField] private float _cointRotationSpeed = 2;

    private float _maxRotationDeltaTime = 0.03f;
    private float _rotationDeltaTime;
    [SerializeField] private Vector3 _angles = new Vector3(360, 90, 180);

    private SoundManager _soundManager;

    [Inject]
    private void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }
    
    private void Awake()
    {
        _angles = transform.eulerAngles;
        transform.DORotate(_angles, _cointRotationSpeed, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(int.MaxValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayCubeController>()) 
        {
            _soundManager.Coin();
            OnCoinCollected?.Invoke();
            DOTween.Kill(transform);
            Destroy(gameObject);
        }
    }
}
