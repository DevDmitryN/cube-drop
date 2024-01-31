using System;
using System.Threading.Tasks;
using Cinemachine;
using DG.Tweening;
using Infastructure;
using Levels;
using UnityEngine;

namespace _Project.Scirpts.Game.Camera
{
    public class FullLevelsCamera : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _positionPlayer;
        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private GameObject _splashSreen;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private PlayCubeController _player;
        
        private float _cameraZ = -12f;
        private float _speed = 1.5f;

        private void OnEnable()
        {
            GameSettings.OnLevelChanged += LevelLoaded;
        }
        
        private void OnDisable()
        {
            GameSettings.OnLevelChanged -= LevelLoaded;
        }

        private async void LevelLoaded(LevelInfo level)
        {
            _transform.DOKill();
            if (level.IsMainMenu)
                return;
            
            _mainCamera.SetActive(false);
            _splashSreen.SetActive(true);
            _transform.position = level.CameraPosition;
            await Task.Delay(50);
            _camera.enabled = true;
            _transform.DOMove(new Vector3(_positionPlayer.position.x, _positionPlayer.position.y, _cameraZ), _speed)
                .SetDelay(2f)
                .OnComplete(
                    () =>
                    {
                        _mainCamera.SetActive(true);
                        _splashSreen.SetActive(false);
                        _camera.enabled = false;
                        _player.IsCanStart = true;
                    });
        }
    }
}