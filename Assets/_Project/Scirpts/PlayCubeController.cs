using DG.Tweening;
using Infastructure;
using UnityEngine;
using Zenject;

public class PlayCubeController : MonoBehaviour
{
    #region serialize fields

    [Inject] SoundManager _soundManager;

    [SerializeField] Camera _camera;

    [SerializeField] CameraFlow _cameraFlow;

    [SerializeField] GameObject _finishPopup;

    [SerializeField] GameObject _deadPopup;

    [SerializeField] private LifeCounter _lifeCounter;

    [SerializeField] float _velocityDropCoefficient = 3;

    [SerializeField] float _angularVelocityDropCoefficient = 5;

    [SerializeField] float _initHeight = 2;

    [SerializeField] float _riseUpSpeed = 1;


    [SerializeField] private TrajectionLine _trajectionLine;

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

    private float _maxScaleDeltaTime = 0.03f;
    private float _scaleDeltaTime;

    private float _returnToInitPositionTime = 0.03f;
    private float _returnToInitPositionDeltaTime;

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

    #region size fields

    private Vector3 _initSize;

    private Vector3 _currentSize
    {
        get { return _transform.localScale; }
        set { _transform.localScale = value; }
    }

    private float _sizeIncrementStep = 0.05f;
    private Vector3 _maxSize;

    #endregion

    #region position fields

    private Vector3 _initPosition;

    private Vector3 _currentPosition
    {
        get { return _transform.position; }
        set { _transform.position = value; }
    }

    private float _maxDistanceFromInitPosition = 2f;

    private float _distanceToDrag = 0.01f;

    float _riseUpStep = 1f;
    float _positionYBeforeRiseUp;

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

    private void Awake()
    {
        Application.targetFrameRate = 90;

        _transform = transform;

        _rigidbody = GetComponent<Rigidbody>();

        FreezePosition(true);

        _angles = gameObject.transform.eulerAngles;

        _initSize = gameObject.transform.localScale;
        _maxSize = IncrementVector(_currentSize, .3f);

        GameSettings.OnSetPosition += InitPosition;
    }

    private void InitPosition(Vector3 position)
    {
        _currentPosition = position;
        _transform.position = position;
        _initPosition = position;
    }

    // Update is called once per frame
    void FixedUpdate()
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


        // ???????????? hover
        HandleMouseOver();
    }

    void OnMouseOver()
    {
        if (!_mouseActionState.IsMouseOver && _state is PlayCubeState.NoAction or PlayCubeState.Drag)
        {
            _mouseActionState.IsHoverHandled = false;
            _mouseActionState.IsMouseOver = true;
        }
    }

    void OnMouseExit()
    {
        if (_mouseActionState.IsMouseOver)
        {
            _mouseActionState.IsMouseOver = false;
            _mouseActionState.IsHoverHandled = false;
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

            return;
        }

        RotateArround(_defaultAngleStep);
    }

    private bool TryHandleDrag()
    {
        // ??????? ??? ? ?????
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

        // ????? ????????? ??? ? ????? - ???????? ??????, ???????? ???????? ? ??????? ????????.
        // ????????? ??????????????? Drop
        if (Input.GetMouseButtonUp(0) && _mouseActionState.IsBtnHolded)
        {
            var newPosition = GetMousePosition();

            var distanceFromInit = Vector3.Distance(newPosition, _initPosition);

            distanceFromInit = Mathf.Min(distanceFromInit, _maxDistanceFromInitPosition);

            var isClickInCubeZone = distanceFromInit < _maxDistanceFromInitPosition;

            if (!isClickInCubeZone)
            {
                distanceFromInit = _maxDistanceFromInitPosition;

                // ???? ???? ????????? ?? ???????? ?????? ?????????? ???????
                var angleOnBorder = Mathf.Atan2(newPosition.y - _initPosition.y, newPosition.x - _initPosition.x);
                var newPositionX = Mathf.Cos(angleOnBorder) * _maxDistanceFromInitPosition + _initPosition.x;
                var newPositionY = Mathf.Sin(angleOnBorder) * _maxDistanceFromInitPosition + _initPosition.y;


                newPosition = new Vector3(newPositionX, newPositionY, 0);
            }

            FreezePosition(false);

            _rigidbody.velocity = (newPosition - _initPosition) * -1 * distanceFromInit * _velocityDropCoefficient;

            _rigidbody.angularVelocity = new Vector3(distanceFromInit * _angularVelocityDropCoefficient,
                distanceFromInit * _angularVelocityDropCoefficient, distanceFromInit * _angularVelocityDropCoefficient);

            _state = PlayCubeState.Drop;

            _mouseActionState.IsBtnHolded = false;

            _mouseActionState.IsHoverHandled = false;

            _trajectionLine.Deactivate();

            return false;
        }

        // ???????????? ??????? ??? ?? ???????
        // ??????? ??????
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

                // ???? ???? ????????? ?? ???????? ?????? ?????????? ???????
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
            _angles = Vector3.zero;
            FreezePosition(true);
            SetNewInitPosition();

            if (IsDead())
            {
                _deadPopup.gameObject.SetActive(true);
                _state = PlayCubeState.Dead;
            }
            else
            {
                _state = PlayCubeState.RiseUp;
            }
        }
    }

    public void FinishGame()
    {
        _rigidbody.angularVelocity = Vector3.zero;
        FreezePosition(true);
        _finishPopup.gameObject.SetActive(true);
        _state = PlayCubeState.Finish;
    }

    private bool IsDead()
    {
        _lifeCounter.DecrementLife();

        return _lifeCounter.IsLifeEnded();
    }

    private void SetNewInitPosition()
    {
        _initPosition = new Vector3(transform.position.x, transform.position.y + _initHeight, 0);
        _cameraFlow.SetCubeInitPosition(_initPosition);
        _positionYBeforeRiseUp = _currentPosition.y;
    }

    float RiseUpFunc(float x)
    {
        return x * 0.01f;
    }

    private void HandleRiseUp()
    {
        _transform.DOMove(Vector3.down, 1f).OnComplete(SetNewInitPosition);
        _returnToInitPositionDeltaTime += Time.fixedDeltaTime;

        if (_returnToInitPositionDeltaTime >= _returnToInitPositionTime)
        {
            _returnToInitPositionDeltaTime = 0;

            var positionY = _positionYBeforeRiseUp + RiseUpFunc(_riseUpStep);

            _currentPosition = new Vector3(_currentPosition.x, positionY, 0);

            _riseUpStep += 5;


            if (_currentPosition.y >= _initPosition.y)
            {
                _currentPosition = _initPosition;
                _state = PlayCubeState.NoAction;
                _riseUpStep = 1;
            }
        }


        //_currentPosition = Vector3.Lerp(_currentPosition, _initPosition, Time.fixedDeltaTime * _riseUpSpeed);


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
        _rotationDeltaTime += Time.fixedDeltaTime;

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

    /// <summary>
    /// ???????????? ????????? ??????? ?????? ??? ????????? ???? ? ??? ??????? ?????? ????.
    /// 
    /// ???? IsHoverHandled = true, ?? ?? ????????????
    /// </summary>
    private void HandleMouseOver()
    {
        if (_mouseActionState.IsHoverHandled)
        {
            return;
        }

        _scaleDeltaTime += Time.fixedDeltaTime;

        if (_scaleDeltaTime > _maxScaleDeltaTime)
        {
            _scaleDeltaTime = 0;

            if (_mouseActionState.IsMouseOver || (!_mouseActionState.IsHoverHandled && _mouseActionState.IsBtnHolded))
            {
                _currentSize = IncrementVector(_currentSize, _sizeIncrementStep);

                _mouseActionState.IsHoverHandled = _currentSize.x >= _maxSize.x;

                if (_mouseActionState.IsHoverHandled)
                {
                    _currentSize = _maxSize;
                }
            }
            else
            {
                _currentSize = IncrementVector(_currentSize, -_sizeIncrementStep);

                _mouseActionState.IsHoverHandled = _currentSize.x <= _initSize.x;

                if (_mouseActionState.IsHoverHandled)
                {
                    _currentSize = _initSize;
                }
            }
        }
    }

    #endregion

    private Vector3 IncrementVector(Vector3 v, float step)
    {
        return new Vector3(v.x + step, v.y + step, v.z + step);
    }

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