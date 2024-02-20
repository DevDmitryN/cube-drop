using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Coin : MonoBehaviour
{
    public static event Action OnCoinCollected;

    [SerializeField] private GameObject _particle;
    
    private ParticleSystem _psEffect;
    
    private readonly Vector3 TARGET_ROTATION_ANGLES = new Vector3(360, 360, 180);
    private const float COIN_ROTATION_SPEED = 8;
    
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
        //_angles = transform.eulerAngles;
         transform.DORotate(TARGET_ROTATION_ANGLES, COIN_ROTATION_SPEED, RotateMode.FastBeyond360)
             .SetRelative()
             .SetEase(Ease.Linear)
             .SetLoops(-1, LoopType.Incremental);
         _psEffect = _particle.GetComponent<ParticleSystem>();
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayCubeController>()) 
        {
            _psEffect.Play();
            _soundManager.Coin();
            OnCoinCollected?.Invoke();
            DOTween.Kill(transform); 
            gameObject.SetActive(false);
        }
    }

    #endregion
    
}
