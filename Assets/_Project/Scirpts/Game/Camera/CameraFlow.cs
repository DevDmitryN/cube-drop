using Cinemachine;
using UnityEngine;
using _Project.Scirpts.Game.Camera;

public class CameraFlow : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    //[SerializeField] private AnimationCurve _curve;
    
    private CinemachineTransposer _transporter;
    private State _cameraState;

    private const float ZOOM_SPEED = 5f;
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
                UnzoomCameraWithTransition();
                break;
            case State.RiseUp:
                ZoomCameraWithTransition();
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
        }
    }

    private void UnzoomCameraWithTransition()
    {
        if (_transporter.m_FollowOffset.z < Z_OFFSET_ON_DROP)
        {
            _cameraState = State.None;
            return;
        }

        _transporter.m_FollowOffset = new Vector3(0, 0, _transporter.m_FollowOffset.z - ZOOM_SPEED * Time.deltaTime);
        //* _curve.Evaluate(-1 * (_transporter.m_FollowOffset.z + -1 * Z_OFFSET) / (Z_OFFSET + -1 * Z_OFFSET_ON_DROP)));
    }

    private void ZoomCameraWithTransition()
    {
        if (_transporter.m_FollowOffset.z > Z_OFFSET)
        {
            _cameraState = State.None;
            return;
        }

        _transporter.m_FollowOffset = new Vector3(0, 0, _transporter.m_FollowOffset.z + ZOOM_SPEED * Time.deltaTime);
        // * _curve.Evaluate((_transporter.m_FollowOffset.z + -1 * Z_OFFSET_ON_DROP) / (Z_OFFSET + -1 * Z_OFFSET_ON_DROP)));
    }
}
