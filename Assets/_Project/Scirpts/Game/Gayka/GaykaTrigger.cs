using System;
using UnityEngine;
using Zenject;

public class GaykaTrigger1 : MonoBehaviour
{
    [SerializeField] private float _velocityMultiplier = 50;
    
    [Inject] private SoundManager _soundManager;
    
    private Transform _transform;

    private float _angleDirectionMin;

    private float _angleDirectionMax;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        var angle = transform.eulerAngles.z;

        while (angle > 180)
        {
            angle -= 360;
        }

        _angleDirectionMin = angle;
        _angleDirectionMax = angle + 180;
    }


    private Vector3 GetDirection(bool fromLeftToRight)
    {
        var directionAngle = Mathf.Deg2Rad * (_transform.eulerAngles.z + (fromLeftToRight ? 90 : -90));
        var directionX = Mathf.Cos(directionAngle);
        var directionY = Mathf.Sin(directionAngle);
        
        return new Vector3(directionX, directionY, 0);
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayCubeController>(out var box))
        {
            var fromLeftToRight = box.GetDirectionAngle() > _angleDirectionMin && box.GetDirectionAngle() < _angleDirectionMax;
            box.SetVelocity(GetDirection(fromLeftToRight) * _velocityMultiplier);
            _soundManager.SpeedUp();
        }
    }
}
