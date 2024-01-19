using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CameraFlow : MonoBehaviour
{
    [Inject] private PlayCubeController _playCube;
    private float _unzoomSpeed = 0.5f;
    [SerializeField] private float _zoomSpeed = 3;
    [SerializeField] private float _zOffset = 12;
    [SerializeField] private float _zOffsetOnDrop = 20;

    private Transform _cubeTransform;
    private Vector3 _cubeInitPosition;

    private CameraState _cameraState;

    private bool _zoomTransitionEnded = false;

    public void SetCubeInitPosition(Vector3 initPosition)
    {
        _cubeInitPosition = initPosition;
    }

    private void Awake()
    {
        _cubeTransform = _playCube.transform;
        _cameraState = CameraState.Game;

        PlayCubeController.OnStateChanged += HandleGameState;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        transform.position = new Vector3(
            _cubeTransform.position.x,
            _cubeTransform.position.y,
            _cubeTransform.position.z - _zOffset
            );

        _cubeInitPosition = _cubeTransform.position;
    }

    void Update()
    {
        switch( _cameraState )
        {
            case CameraState.Game:
                //HandleGameState();
                break;
            case CameraState.FollowCube:
                transform.position = new Vector3(
                    _cubeTransform.position.x,
                    _cubeTransform.position.y,
                    _cubeTransform.position.z - _zOffsetOnDrop
                );;
                break;
        }
    }

    private void OnDisable()
    {
        PlayCubeController.OnStateChanged -= HandleGameState;
    }

    

    private void HandleGameState(PlayCubeController.PlayCubeState cubeState)
    {
        switch (cubeState)
        {
            case PlayCubeController.PlayCubeState.NoAction:
                _zoomTransitionEnded = false;
                SetToCubeWithZoom();
                break;
            case PlayCubeController.PlayCubeState.Drag:
                UnzoomCameraWithTransition();
                break;
            case PlayCubeController.PlayCubeState.Drop:
                _cameraState = CameraState.FollowCube;
                //_zoomTransitionEnded = false;
                //SetToCubePositionWithUnzoom();
                break;
            case PlayCubeController.PlayCubeState.RiseUp:
                _cameraState = CameraState.Game;
                ZoomCameraWithTransition();
                break;
            default:
                _zoomTransitionEnded = false;
                SetToCubePositionWithUnzoom();
                break;
        }
    }

    private void SetToCubePositionWithUnzoom()
    {
        transform.position = new Vector3(
            _cubeTransform.position.x,
            _cubeTransform.position.y,
            _cubeTransform.position.z - _zOffsetOnDrop
        );
    }

    private void UnzoomCameraWithTransition()
    {
        // if (_zoomTransitionEnded) 
        // {
        //     return;
        // }
        // var targetZCoord = _cubeInitPosition.z - _zOffsetOnDrop;

        transform.DOMoveZ(_cubeInitPosition.z - _zOffsetOnDrop, _unzoomSpeed);

        // var zCoord = Mathf.Lerp(transform.position.z, targetZCoord, _unzoomSpeed * Time.deltaTime);
        //
        // if (Mathf.Round(zCoord * 100) == targetZCoord * 100) 
        // {
        //     _zoomTransitionEnded = true;
        //     zCoord = targetZCoord;
        // }
        //
        // // Debug.Log($"{zCoord} {_zoomTransitionEnded}");
        //
        // transform.position = new Vector3(
        //     _cubeInitPosition.x,
        //     _cubeInitPosition.y,
        //     zCoord
        // );
    }

    private void ZoomCameraWithTransition()
    {
        var cubeInitPosition = _playCube.GetInitPosition();
        
        transform.position = new Vector3(cubeInitPosition.x, cubeInitPosition.y, transform.position.z);
        
        transform.DOMoveZ(_cubeInitPosition.z - _zOffsetOnDrop, _zoomSpeed);
    }

    private void SetToCubeWithZoom()
    {
        transform.position = new Vector3(
           _cubeTransform.position.x,
           _cubeTransform.position.y,
           _cubeTransform.position.z - _zOffset
        );
    }

    public enum CameraState
    {
        Game,
        FollowCube,
    }
}
