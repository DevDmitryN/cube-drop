using System.Collections;
using System.Collections.Generic;
using _Project.Scirpts.Game.Platforms;
using UnityEngine;
using UnityEngine.Serialization;

public class GaykaTrigger : MonoBehaviour
{
    [SerializeField] private float _velocityMultiplier = 50;

    /// <summary>
    /// false - с права на лево 
    /// true - с лева на право
    /// </summary>
    [SerializeField] private bool _directionModifier;

    [SerializeField] private GaykaTrigger _dependedTrigger;
    
    private Vector3 _direction = Vector3.left;
    
    private Transform _transform;
    
    private bool _triggerEnable { get; set; } = true;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
    
    private void OnEnable()
    {
        var directionAngle = Mathf.Deg2Rad * (_transform.eulerAngles.z + (_directionModifier ? 90 : -90));
        
        var directionX = Mathf.Cos(directionAngle);
        var directionY = Mathf.Sin(directionAngle);
        
        _direction = new Vector3(directionX, directionY, 0);
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (!_triggerEnable)
        {
            return;
        }

        if (other.TryGetComponent<PlayCubeController>(out var box))
        { 
            _dependedTrigger.DisableTrigger();
            box.SetVelocity(_direction * _velocityMultiplier);
            StartCoroutine(EnableDependedTrigger());
        }
    }

    private void DisableTrigger()
    {
        _triggerEnable = false;
    }
    
    private void EnableTrigger()
    {
        _triggerEnable = true;
    }

    private IEnumerator EnableDependedTrigger()
    {
        Debug.Log("Disable trigger");
        yield return new WaitForSeconds(0.01f);
        _dependedTrigger.EnableTrigger();
        Debug.Log("Trigger enable");
    }
}
