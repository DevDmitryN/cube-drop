using Cinemachine;
using UnityEngine;
using _Project.Scirpts.Game.Camera;
using Levels;

public class CameraFlow : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    //[SerializeField] private AnimationCurve _curve;
    
    private CinemachineTransposer _transporter;
    private State _cameraState;

    private const float ZOOM_SPEED = 10f;
    private const float UNZOOM_SPEED = 20f;
    private const float Z_OFFSET = -12f;
    private const float Z_OFFSET_ON_DROP = -20f;

    #region MONO

    private void OnEnable()
    {
        PlayCubeController.OnStateChanged += HandleGameState;
        _transporter = _camera.GetCinemachineComponent<CinemachineTransposer>();
    }
    
    private void OnDisable()
    {
        PlayCubeController.OnStateChanged -= HandleGameState;
    }
    
    private void Update()
    {
        switch( _cameraState)
        {
            case State.Drag:
                UnzoomingCameraWithTransition();
                break;
            case State.RiseUp:
                ZoomingCameraWithTransition();
                break;
        }
    }

    #endregion
    
    private void HandleGameState(PlayCubeController.PlayCubeState cubeState)
    {
        switch (cubeState)
        {
            case PlayCubeController.PlayCubeState.Drag:
                _cameraState = State.Drag;
                break;
            case PlayCubeController.PlayCubeState.RiseUp:
                _cameraState = State.RiseUp;
                break;
            case PlayCubeController.PlayCubeState.NoAction:
                ZoomCamera();
                break;
        }
    }

    private void UnzoomingCameraWithTransition()
    {
        if (_transporter.m_FollowOffset.z < Z_OFFSET_ON_DROP)
        {
            _cameraState = State.None;
            return;
        }

        _transporter.m_FollowOffset = new Vector3(0, 0, _transporter.m_FollowOffset.z - UNZOOM_SPEED * Time.deltaTime);
    }

    private void ZoomingCameraWithTransition()
    {
        if (_transporter.m_FollowOffset.z > Z_OFFSET)
        {
            _cameraState = State.None;
            return;
        }

        _transporter.m_FollowOffset = new Vector3(0, 0, _transporter.m_FollowOffset.z + ZOOM_SPEED * Time.deltaTime);
    }

    private void ZoomCamera()
    {
        _transporter.m_FollowOffset = new Vector3(0, 0, Z_OFFSET);
    }
}
