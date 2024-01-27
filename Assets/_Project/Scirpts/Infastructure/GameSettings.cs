using System;
using System.Collections.Generic;
using Levels;
using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;

namespace Infastructure
{
    public class GameSettings : MonoBehaviour
    {
        public static event Action<LevelInfo> OnLevelChanged;
        
        [SerializeField] private List<LevelInfo> _levels; 
        [SerializeField] private int _targetFrameRate;
        
        private LevelsLoader _levelsLoader;
        private int _mainMenuLevelIndex = 5;
        private int _saveLevel = 0;
        private const string LAST_LEVEL = "LastLevel";

        public int SaveLevel => _saveLevel;
        
        #region MONO

        private void Awake()
        {
            // _saveLevel = PlayerPrefs.HasKey(LAST_LEVEL) ? PlayerPrefs.GetInt(LAST_LEVEL) : 0;
            _saveLevel = 0;
            Application.targetFrameRate = _targetFrameRate;

            _levelsLoader = new LevelsLoader(_levels);
        }

        // for develop levels
        // comment on build
        private void OnEnable()
        {
            var index = SceneManager.GetActiveScene().buildIndex ;
            var level = _levelsLoader.ChangeLevel(index);
            OnLevelChanged?.Invoke(level);
        }

        #endregion
        
        public void LastOpenLevel()
        {
            var level = _levelsLoader.ChangeLevel(_saveLevel);
            OnLevelChanged?.Invoke(level);
        }
        
        public void RestartLevel()
        {
            var level = _levelsLoader.RestartLevel();
            OnLevelChanged?.Invoke(level);
        }
        
        public void NextLevel()
        {
            var level = _levelsLoader.ChangeLevel(++_saveLevel);
            PlayerPrefs.SetInt(LAST_LEVEL, level.LevelIndex);
            PlayerPrefs.Save();
            OnLevelChanged?.Invoke(level);
        }
        
        public void OpenMainMenu()
        {
            SceneManager.LoadScene(_mainMenuLevelIndex);
            OnLevelChanged?.Invoke(_levels[_mainMenuLevelIndex]);
        }
    }
}