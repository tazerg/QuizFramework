using System;
using UnityEngine;

namespace QuizFramework.LocalConfigs
{
    [Serializable]
    public struct QuizResultMessageInfo
    {
        [SerializeField] private float _minRatio;
        [SerializeField] private float _maxRatio;
        [SerializeField] private string _message;

        public float MinRatio => _minRatio;
        public float MaxRatio => _maxRatio;
        public string Message => _message;
    }
}