using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class DamageBlock : MonoBehaviour
{
    [SerializeField] private float _velocityModifier = 5;
    
    [Inject] private SoundManager _soundManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayCubeController>(out var cube))
        {
            cube.TakeDamage(1);
            
            var directionAngle = cube.GetDirectionAngle() + 180;;
            var directionX = Mathf.Cos(directionAngle);
            var directionY = Mathf.Sin(directionAngle);
        
            var direction = new Vector3(directionX, directionY, 0);
            
            cube.SetVelocity(direction * _velocityModifier);
            
            Debug.DrawLine(GetComponent<Transform>().position, direction, Color.red, 2);
        }
    }
}
