using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CameraFlow : MonoBehaviour
{
    [Inject] private PlayCubeController _playCube;
    [SerializeField] private CinemachineVirtualCamera _camera;
    private float _unzoomSpeed = 0.3f;
    private float _zoomSpeed = 0.7f;
    private float _zOffset = 12;
    private float _zOffsetOnDrop = 20;

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
        // _cubeTransform = _playCube.transform;
        // _cameraState = CameraState.Game;

        PlayCubeController.OnStateChanged += HandleGameState;
    }

    // Start is called before the first frame update
    void Start()
    {
        //
        // transform.position = new Vector3(
        //     _cubeTransform.position.x,
        //     _cubeTransform.position.y,
        //     _cubeTransform.position.z - _zOffset
        //     );

        //_cubeInitPosition = _cubeTransform.position;
    }

    void Update()
    {
        switch( _cameraState )
        {
            case CameraState.Game:
                //HandleGameState();
                break;
            case CameraState.FollowCube:
                // transform.position = new Vector3(
                //     _cubeTransform.position.x,
                //     _cubeTransform.position.y,
                //     _cubeTransform.position.z - _zOffsetOnDrop
                // );;
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
                // _zoomTransitionEnded = false;
                // SetToCubeWithZoom();
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
                // _zoomTransitionEnded = false;
                // SetToCubePositionWithUnzoom();
                break;
        }
    }

    private void SetToCubePositionWithUnzoom()
    {
        // transform.position = new Vector3(
        //     _cubeTransform.position.x,
        //     _cubeTransform.position.y,
        //     _cubeTransform.position.z - _zOffsetOnDrop
        // );
    }

    private void UnzoomCameraWithTransition()
    {
        var cubeInitPosition = _playCube.GetInitPosition();
        
        var targetPosition= new Vector3(cubeInitPosition.x, cubeInitPosition.y, -20);
        _camera.m_Follow.DOMove(targetPosition, _zoomSpeed).SetEase(Ease.Flash);
       // transform.DOMove(targetPosition, _unzoomSpeed).SetEase(Ease.Flash);
    }

    private void ZoomCameraWithTransition()
    {
        var cubeInitPosition = _playCube.GetInitPosition();
        
        var targetPosition= new Vector3(cubeInitPosition.x, cubeInitPosition.y, -12);
        _camera.m_Follow.DOMove(targetPosition, _zoomSpeed).SetEase(Ease.Flash);
    }

    private void SetToCubeWithZoom()
    {
        // transform.position = new Vector3(
        //    _cubeTransform.position.x,
        //    _cubeTransform.position.y,
        //    _cubeTransform.position.z - _zOffset
        // );
    }

    public enum CameraState
    {
        Game,
        FollowCube,
    }
}
