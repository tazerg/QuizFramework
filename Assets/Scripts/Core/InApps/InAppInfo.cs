using System;
using UnityEngine;

namespace QuizFramework.InApps
{
    [Serializable]
    public struct InAppInfo
    {
        [SerializeField] private InAppType _type;
        [SerializeField] private string _id;
        [SerializeField] private string _name;

        public InAppType Type => _type;
        public string Id => _id;
        public string Name => _name;
    }
}