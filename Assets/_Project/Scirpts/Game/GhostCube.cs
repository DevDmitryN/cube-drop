using System;
using Infastructure;
using Levels;
using UnityEngine;
using Zenject;

namespace _Project.Scirpts.Game
{
    public class GhostCube : MonoBehaviour
    {
        private Transform _transform;
        
        private PlayCubeController _cube;

        private PlayCubeController.PlayCubeState _cubeState;
        

        [Inject]
        private void Constructor(PlayCubeController cube)
        {
            _cube = cube;
        }

        private void Awake()
        {
            _transform = transform;
            _cubeState = _cube.GetState();
            _transform.position = _cube.GetInitPosition();

            GameSettings.OnLevelChanged += OnLevelChanged;
            PlayCubeController.OnStateChanged += OnPlayCubeStateChanged;
        }

        private void OnDisable()
        {
            PlayCubeController.OnStateChanged -= OnPlayCubeStateChanged;
            GameSettings.OnLevelChanged -= OnLevelChanged;
        }

        private void Update()
        {
            switch (_cubeState)
            {
                case PlayCubeController.PlayCubeState.Drop:
                case PlayCubeController.PlayCubeState.RiseUp:
                    _transform.position = _cube.GetPosition();
                    break;
            }
        }

        private void OnPlayCubeStateChanged(PlayCubeController.PlayCubeState state)
        {
            _cubeState = state;

            switch (_cubeState)
            {
                case PlayCubeController.PlayCubeState.NoAction:
                    _transform.position = _cube.GetInitPosition();
                    break;
            }
        }

        private void OnLevelChanged(LevelInfo levelInfo)
        {
            _transform.position = levelInfo.StartPosition;
        }
    }
}