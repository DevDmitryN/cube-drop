using System;
using System.Threading.Tasks;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Infastructure;
using JetBrains.Annotations;
using Lean.Localization;
using Levels;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _educationText;
        
        private float _cameraZ = -12f;
        private float _speed = 1.5f;

        private LevelInfo _level;
        
        private EducationalStep _educationalStep = EducationalStep.Finish;

        private bool _isMovementEnded = false;

        private void Update()
        {
            if (_level == null || !_level.IsEducational || _educationalStep == EducationalStep.StartGame)
                return;
            
            if (_isMovementEnded)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    switch (_educationalStep)
                    {
                        case EducationalStep.Finish:
                            ShowEducationCoin();
                            _educationalStep = EducationalStep.Coin;
                            break;
                        case EducationalStep.Coin:
                            Debug.Log("coin instruction");
                            ShowEducationCube();
                            _educationalStep = EducationalStep.Cube;
                            EnableGame();
                            break;
                        case EducationalStep.Cube:
                            _educationText.gameObject.SetActive(false);
                            _educationalStep = EducationalStep.StartGame;
                            break;
                    }
                }
            }
        }

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

            _level = level;
            
            if (level.IsEducational)
            {
                await ShowEducationFinishPoint();
            }
            else
            {
                await ShowLevel(level);
            }
            
        }

        private async Task ShowLevel(LevelInfo level)
        {
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

        private async Task ShowEducationFinishPoint()
        {
            _mainCamera.SetActive(false);
            _splashSreen.SetActive(true);
            _transform.position = _level.CameraPosition;
            
            _educationText.gameObject.SetActive(true);
            _educationText.text = LeanLocalization.GetTranslationText("Привет");
            
            await Task.Delay(50);
            
            _camera.enabled = true;
            
            _isMovementEnded = false;
            _educationalStep = EducationalStep.Finish;
            
            _transform
                .DOMove(new Vector3(_level.EducationFinishPoint.x, _level.EducationFinishPoint.y, _cameraZ), _speed)
                .SetDelay(2f)
                .OnComplete(() =>
                {
                    _educationText.gameObject.SetActive(true);
                    _educationText.text =  LeanLocalization.GetTranslationText("Это финиш. Для прохождения уровней тебе нужно попасть сюда.");
                    _isMovementEnded = true;
                });
        }

        private void ShowEducationCoin()
        {
            _educationText.gameObject.SetActive(false);
            
            _isMovementEnded = false;
            
            _transform
                .DOMove(new Vector3(_level.EducationCoinPoint.x, _level.EducationCoinPoint.y, _cameraZ), _speed)
                .OnComplete(() =>
                {
                    _educationText.gameObject.SetActive(true);
                    _educationText.text = LeanLocalization.GetTranslationText("Это монетки. ") +
                                                                              "\n "+LeanLocalization.GetTranslationText("Ты можешь их собрирать, в будущем они могут тебе пригодиться.") +
                                                                              "\n "+LeanLocalization.GetTranslationText("Количество монеток показывается выше.");
                    _isMovementEnded = true;
                });
        }

        private void ShowEducationCube()
        {
            _educationText.gameObject.SetActive(false);
            _isMovementEnded = false;
            _transform.DOMove(new Vector3(_positionPlayer.position.x, _positionPlayer.position.y, _cameraZ), _speed)
                .OnComplete(() =>
                {
                    _educationText.gameObject.SetActive(true);
                    _educationText.text = LeanLocalization.GetTranslationText("Это твой кубик. ") +
                                                                              "\n "+LeanLocalization.GetTranslationText("Потяни за него и попади в финишь. ") +
                                                                              "\n "+LeanLocalization.GetTranslationText("Чем сильнее ты его тянешь, тем быстее он полетит.") +
                                                                              "\n "+LeanLocalization.GetTranslationText("Количество попыток отображается выше рядом с сердечками.");
                    _isMovementEnded = true;
                });
        }

        private void EnableGame()
        {
            _mainCamera.SetActive(true);
            _splashSreen.SetActive(false);
            _camera.enabled = false;
            _player.IsCanStart = true;
        }
    }
}

public enum EducationalStep
{
    Finish,
    Coin,
    Cube,
    StartGame
}