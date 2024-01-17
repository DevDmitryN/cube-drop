using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float _cointRotationSpeed = 2;

    [SerializeField] SoundManager _soundManager;

    [SerializeField] CoinCounter _coinCounter;

    private float _maxRotationDeltaTime = 0.03f;
    private float _rotationDeltaTime;
    private Vector3 _angles;

    // Start is called before the first frame update
    void Awake()
    {
        _angles = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        _rotationDeltaTime += Time.deltaTime;

        if (_rotationDeltaTime > _maxRotationDeltaTime)
        {
            _rotationDeltaTime = 0;
           

            var angleX = _angles.x > 360 ? _angles.x - 360 : _angles.x;
            var angleY = _angles.y > 360 ? _angles.y - 360 : _angles.y;
            var angleZ = _angles.z > 360 ? _angles.z - 360 : _angles.z;

            _angles = new Vector3(angleX + _cointRotationSpeed, angleY + _cointRotationSpeed, angleZ + 1f);

            transform.eulerAngles = _angles;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var cubeController = other.GetComponent<PlayCubeController>();

        if (cubeController != null) 
        {
            _soundManager.Coin();
            _coinCounter.OnCoinCollected();
            Destroy(gameObject);
        }
    }
}
