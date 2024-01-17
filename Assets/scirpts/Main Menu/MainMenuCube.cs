using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuCube : MonoBehaviour
{
    private Transform _transform;

    private float _maxRotationDeltaTime = 0.03f;
    private float _rotationDeltaTime;
    private Vector3 _angles;
    private float _defaultAngleStep = 5f;
    private Vector3 _currentAngles
    {
        get { return _transform.eulerAngles; }
        set { _transform.eulerAngles = value; }
    }

    
    void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateArround(_defaultAngleStep);
    }

    private void RotateArround(float angleStep)
    {
        _rotationDeltaTime += Time.deltaTime;

        if (_rotationDeltaTime > _maxRotationDeltaTime)
        {
            _rotationDeltaTime = 0;

            var angleX = _angles.x > 360 ? _angles.x - 360 : _angles.x;
            var angleY = _angles.y > 360 ? _angles.y - 360 : _angles.y;
            var angleZ = _angles.z > 360 ? _angles.z - 360 : _angles.z;

            _angles = new Vector3(angleX + angleStep, angleY + angleStep, angleZ + 1f);
            _currentAngles = _angles;
        }
    }
}
