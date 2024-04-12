using System.Collections;
using System.Collections.Generic;
using _Project.Scirpts.Game.Platforms;
using UnityEngine;

public class GaykaZone : MonoBehaviour
{
    [SerializeField] private float _velocityMultiplier = 20;
    
    private Vector3 _direction = Vector3.left;
    
    private Transform _transform;

    private Vector3 _directionRight = Vector3.left;

    private Vector3 _directionLeft = Vector3.left;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
    
    private void OnEnable()
    {
        var directionAngle = Mathf.Deg2Rad * (_transform.eulerAngles.z - 90);
        
        var directionX = Mathf.Cos(directionAngle);
        var directionY = Mathf.Sin(directionAngle);
        
        _directionLeft = new Vector3(directionX, directionY, 0);
        
        directionAngle = Mathf.Deg2Rad * (_transform.eulerAngles.z + 90);
        directionX = Mathf.Cos(directionAngle);
        directionY = Mathf.Sin(directionAngle);
        
        _directionRight = new Vector3(directionX, directionY, 0);
        
        Debug.DrawRay(_transform.position, _directionLeft, Color.yellow, 1000);
        Debug.DrawRay(_transform.position, _directionRight, Color.green, 1000);
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        // Debug.Log("trigger");
        if (other.TryGetComponent<PlayCubeController>(out var box))
        { 
            Debug.DrawLine(_transform.position, _direction*100, Color.red, 10);
            var leftToRight = box.transform.position.x < _transform.position.x;
            Debug.Log($"left to right {leftToRight}");
            box.SetVelocity((leftToRight ? _directionRight : _directionLeft) * _velocityMultiplier);
            //_soundManager.Jump();
        }
    }

    private Vector3 GetDirection(bool leftToRight)
    {
        var multiplier = leftToRight ? 1 : -1;
        var directionAngle = Mathf.Deg2Rad * (_transform.eulerAngles.z - (90 * multiplier));
        var directionX = Mathf.Cos(directionAngle);
        var directionY = Mathf.Sin(directionAngle);
        Debug.DrawRay(_transform.position, new Vector3(directionX, directionY, 0), Color.yellow, 10);
        return new Vector3(directionX, directionY, 0);
    }
}
