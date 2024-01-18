using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    [SerializeField] private PlayCubeController _playCube;
    [SerializeField] private float _unzoomSpeed = 20;
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
        _cubeTransform = _playCube.gameObject.transform;
        _cameraState = CameraState.Game;
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
                HandleGameState();
                break;
        }
    }


    bool a;

    private void HandleGameState()
    {
        switch ( _playCube.GetState() )
        {
            case PlayCubeController.PlayCubeState.NoAction:
                _zoomTransitionEnded = false;
                SetToCubeWithZoom();
                break;
            case PlayCubeController.PlayCubeState.Drag:
                UnzoomCameraWithTransition();
                break;
            case PlayCubeController.PlayCubeState.Drop:
                _zoomTransitionEnded = false;
                SetToCubePositionWithUnzoom();
                break;
            case PlayCubeController.PlayCubeState.RiseUp:
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
        if (_zoomTransitionEnded) 
        {
            return;
        }
        var targetZCoord = _cubeInitPosition.z - _zOffsetOnDrop;

        var zCoord = Mathf.Lerp(transform.position.z, targetZCoord, _unzoomSpeed * Time.deltaTime);

        if (Mathf.Round(zCoord * 100) == targetZCoord * 100) 
        {
            _zoomTransitionEnded = true;
            zCoord = targetZCoord;
        }

        // Debug.Log($"{zCoord} {_zoomTransitionEnded}");

        transform.position = new Vector3(
            _cubeInitPosition.x,
            _cubeInitPosition.y,
            zCoord
        );
    }

    private void ZoomCameraWithTransition()
    {
        if (_zoomTransitionEnded)
        {
            return;
        }
        var targetZCoord = _cubeInitPosition.z - _zOffset;

        var zCoord = Mathf.Lerp(transform.position.z, targetZCoord, _zoomSpeed * Time.deltaTime);

        if (Mathf.Round(zCoord * 100) == targetZCoord * 100)
        {
            Debug.Log("Zoom end");
            _zoomTransitionEnded = true;
            zCoord = targetZCoord;
        }

        // Debug.Log($"{zCoord} {_zoomTransitionEnded}");

        transform.position = new Vector3(
            _cubeInitPosition.x,
            _cubeInitPosition.y,
            zCoord
        );
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
    }
}
