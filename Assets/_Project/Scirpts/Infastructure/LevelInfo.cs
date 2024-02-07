using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Levels
{
    [CreateAssetMenu(fileName = "Levels", menuName = "CreateLevel", order = 0)]
    public class LevelInfo : ScriptableObject
    {
        public int CoinCount;
        public Vector3 StartPosition;
        public int LevelIndex;
        public bool IsMainMenu;
        public Vector3 CameraPosition;
        public bool IsEducational;
        public Vector3 EducationFinishPoint;
        public Vector3 EducationCoinPoint;
    }
}