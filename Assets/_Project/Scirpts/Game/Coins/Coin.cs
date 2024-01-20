using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Coin : MonoBehaviour
{
    public static event Action OnCoinCollected;
    
    [SerializeField] private Vector3 _angles = new Vector3(360, 90, 180);
    [SerializeField] private float _cointRotationSpeed = 2;
    
    private float _rotationDeltaTime;
    
    private SoundManager _soundManager;

    [Inject]
    private void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    #region MONO

    private void OnEnable()
    {
        _angles = transform.eulerAngles;
        transform.DORotate(_angles, _cointRotationSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(int.MaxValue);
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayCubeController>()) 
        {
            _soundManager.Coin();
            OnCoinCollected?.Invoke();
            DOTween.Kill(transform); 
            gameObject.SetActive(false);
        }
    }

    #endregion
    
}
