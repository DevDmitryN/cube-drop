using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Levels
{
    public class LevelsLoader 
    {
        private LevelInfo _currentLevel;
        private readonly List<LevelInfo> _levelInfos;

        public LevelsLoader(List<LevelInfo> levelInfos)
        {
            _levelInfos = levelInfos;
            _currentLevel = levelInfos[0];
        }

        public LevelInfo ChangeLevel(int level)
        {
            //Тут мы даем игроку дальше играть. По факту тут можно реализовать любую логику
            //Выкидывать в меню да и вообще что угодно
            if (_levelInfos.Count <= level)
            {
                level = _levelInfos.Count - 1;
                Debug.Log("Надо добавлять новые уровни, пора!");
            }
            else if (level < 0)
            {
                level = 0;
                Debug.Log("Упс, что то пошло не по плану(");
            }
            
            var levelInfo = _levelInfos[level];
            SceneManager.LoadScene(levelInfo.LevelIndex);
            _currentLevel = _levelInfos[level];
            return _currentLevel;
        }
        
        public LevelInfo RestartLevel()
        {
            if (_currentLevel == default)
            {
                Debug.Log("А как ты сюда зашел?!");
                return default;
            }
            
            SceneManager.LoadScene(_currentLevel.LevelIndex);
            return _currentLevel;
        }
    }
}