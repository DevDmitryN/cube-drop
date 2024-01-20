using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Infastructure;
using JetBrains.Annotations;
using Levels;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayCubeController : MonoBehaviour
{
    public static event Action<PlayCubeState> OnStateChanged;
    #region serialize fields

    [Inject] SoundManager _soundManager;

    [SerializeField] Camera _camera;

    [SerializeField] private TrajectionLine _trajectionLine;

    float _velocityDropCoefficient = 10;

    float _angularVelocityDropCoefficient = 5;

    float _initHeight = 2;

    float _riseUpTime = 1;

    #endregion

    #region internal props fields

    private Transform _transform;

    private Rigidbody _rigidbody;

    #endregion

    #region states fields

    private PlayCubeState _state = PlayCubeState.NoAction;

    private MouseActionState _mouseActionState = new MouseActionState()
        {IsHoverHandled = true, IsMouseOver = false, IsBtnHolded = false};

    #endregion

    #region delta time fields

    private float _maxRotationDeltaTime = 0.03f;
    private float _rotationDeltaTime;

    #endregion

    #region angles fields

    private Vector3 _angles;
    private float _defaultAngleStep = 5f;

    private Vector3 _currentAngles
    {
        get => _transform.eulerAngles;
        set => _transform.eulerAngles = value;
    }

    #endregion angles

    #region position fields

    private Vector3 _initPosition;

    private Vector3 _currentPosition
    {
        get { return _transform.position; }
        set { _transform.position = value; }
    }

    private float _maxDistanceFromInitPosition = 2f;
    
    
    #endregion

    #region Getters

    public PlayCubeState GetState()
    {
        return _state;
    }

    public Vector3 GetInitPosition()
    {
        return _initPosition;
    }

    #endregion

    #region Unity methods

    private GamePlayHandler _gamePlayHandler;

    [Inject]
    private void Constructor(GamePlayHandler gamePlayHandler)
    {
        _gamePlayHandler = gamePlayHandler;
    }
    
    private void OnEnable()
    {
        _transform = transform;

        _rigidbody = GetComponent<Rigidbody>();

        FreezePosition(true);

        _angles = gameObject.transform.eulerAngles;
        
        GameSettings.OnLevelChanged += LevelChaged;
    }

    private void OnDisable()
    {
        GameSettings.OnLevelChanged -= LevelChaged;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case PlayCubeState.NoAction:
                HandleNoActionState();
                break;
            case PlayCubeState.Drag:
                TryHandleDrag();
                break;
            case PlayCubeState.Drop:
                HandleDrop();
                break;
            case PlayCubeState.RiseUp:
                HandleRiseUp();
                break;
            case PlayCubeState.Finish:
                HandleFinishState();
                break;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (_state == PlayCubeState.Drop)
        {
            _soundManager.Strike();
        }
    }

    #endregion

    #region Handle states methods

    private void HandleNoActionState()
    {
        if (TryHandleDrag())
        {
            _state = PlayCubeState.Drag;
            OnStateChanged?.Invoke(PlayCubeState.Drag);
            return;
        }

        RotateArround(_defaultAngleStep);
    }

    private bool TryHandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var newPosition = GetMousePosition();
            var isClickInCubeZone = Vector3.Distance(newPosition, _initPosition) < _maxDistanceFromInitPosition;

            if (!isClickInCubeZone)
            {
                return false;
            }

            _mouseActionState.IsBtnHolded = isClickInCubeZone;
            _mouseActionState.IsHoverHandled = false;

            _trajectionLine.Activate(_initPosition);
        }
        
        if (Input.GetMouseButtonUp(0) && _mouseActionState.IsBtnHolded)
        {
            var newPosition = GetMousePosition();

            var distanceFromInit = Vector3.Distance(newPosition, _initPosition);

            distanceFromInit = Mathf.Min(distanceFromInit, _maxDistanceFromInitPosition);

            var isClickInCubeZone = distanceFromInit < _maxDistanceFromInitPosition;

            if (!isClickInCubeZone)
            {
                distanceFromInit = _maxDistanceFromInitPosition;
                
                var angleOnBorder = Mathf.Atan2(newPosition.y - _initPosition.y, newPosition.x - _initPosition.x);
                var newPositionX = Mathf.Cos(angleOnBorder) * _maxDistanceFromInitPosition + _initPosition.x;
                var newPositionY = Mathf.Sin(angleOnBorder) * _maxDistanceFromInitPosition + _initPosition.y;


                newPosition = new Vector3(newPositionX, newPositionY, 0);
            }

            FreezePosition(false);

            _rigidbody.velocity = (newPosition - _initPosition) * -1 * distanceFromInit * _velocityDropCoefficient;

            _rigidbody.angularVelocity = new Vector3(distanceFromInit * _angularVelocityDropCoefficient,
                distanceFromInit * _angularVelocityDropCoefficient, distanceFromInit * _angularVelocityDropCoefficient);

           

            _mouseActionState.IsBtnHolded = false;

            _trajectionLine.Deactivate();
            
            _state = PlayCubeState.Drop;
            
            OnStateChanged?.Invoke(PlayCubeState.Drop);
            return false;
        }
        
        if (_mouseActionState.IsBtnHolded)
        {
            var newPosition = GetMousePosition();

            var distanceFromInit = Vector3.Distance(newPosition, _initPosition);

            var isClickInCubeZone = distanceFromInit < _maxDistanceFromInitPosition;


            if (isClickInCubeZone)
            {
                _currentPosition = newPosition;
            }
            else
            {
                distanceFromInit = _maxDistanceFromInitPosition;
                
                var angleOnBorder = Mathf.Atan2(newPosition.y - _initPosition.y, newPosition.x - _initPosition.x);

                var newPositionX = Mathf.Cos(angleOnBorder) * _maxDistanceFromInitPosition + _initPosition.x;
                var newPositionY = Mathf.Sin(angleOnBorder) * _maxDistanceFromInitPosition + _initPosition.y;

                _currentPosition = new Vector3(newPositionX, newPositionY, 0);
            }

            _trajectionLine.UpdateDestination(_currentPosition, distanceFromInit);

            RotateArround(distanceFromInit * _angularVelocityDropCoefficient);

            return true;
        }

        return false;
    }

    private void HandleDrop()
    {
        if (IsCubeStopped())
        {
            FreezePosition(true);
            
            if (IsDead())
            {
                _state = PlayCubeState.Dead;
                OnStateChanged?.Invoke(_state);
            }
            else
            {
                SetNewInitPosition();
                _angles = Vector3.zero;
                _transform.DOMove(_initPosition, _riseUpTime)
                    .OnComplete(() =>
                    {
                        _state = PlayCubeState.NoAction;
                        OnStateChanged?.Invoke(_state);
                    });
                _state = PlayCubeState.RiseUp;
                OnStateChanged?.Invoke(_state);
            }
            
        }
    }

    public void FinishGame()
    {
        _rigidbody.angularVelocity = Vector3.zero;
        FreezePosition(true);
        _state = PlayCubeState.Finish;
        OnStateChanged?.Invoke(_state);
    }

    private bool IsDead()
    {
        return _gamePlayHandler.DecrementLife();
    }

    private void LevelChaged(LevelInfo level)
    {
        if (level != null)
        {
            SetNewInitPosition(level.StartPosition);
            _state = PlayCubeState.NoAction;
        }
    }
    
    private void SetNewInitPosition(Vector3? initPosition = null)
    {
        if (initPosition.HasValue)
        {
            _currentPosition = initPosition.Value;
            _initPosition = initPosition.Value;
        }
        else
        {
            _initPosition = new Vector3(transform.position.x, transform.position.y + _initHeight, 0);
        }
        _rigidbody.velocity = Vector3.zero;
        FreezePosition(true);

    }

    private void HandleRiseUp()
    {
        RotateArround(_defaultAngleStep);
    }

    private void HandleFinishState()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
    }

    #endregion

    #region work with internal properties methods

    private bool IsCubeStopped()
    {
        return _rigidbody.velocity.magnitude == 0 && _rigidbody.angularVelocity.magnitude == 0;
    }

    private void FreezePosition(bool freeze)
    {
        _rigidbody.constraints = freeze ? RigidbodyConstraints.FreezePosition : RigidbodyConstraints.FreezePositionZ;
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

    #endregion

    private Vector3 GetMousePosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = _camera.nearClipPlane + 8; //_camera.transform.position.z * -1;
        mousePosition = _camera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }

    public enum PlayCubeState
    {
        NoAction,
        Drop,
        Drag,
        RiseUp,
        Finish,
        Dead
    }
}

public class MouseActionState
{
    public bool IsMouseOver;
    public bool IsHoverHandled;
    public bool IsBtnHolded;
}