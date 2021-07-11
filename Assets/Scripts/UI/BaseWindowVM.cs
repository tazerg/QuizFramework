using QuizFramework.SignalBus;
using UnityEngine;
using Zenject;

namespace QuizFramework.UI
{
    public abstract class BaseWindowVM<T> : MonoBehaviour where T : struct
    {
        [Inject] protected ISignalBus SignalBus { get; private set; }

        protected void Close()
        {
            gameObject.SetActive(false);
        }

        protected abstract void OnHandleEvent(T eventParams);
        protected abstract void CheckReferences();
        protected abstract void Initialize();
        protected abstract void Dispose();
        
        private void TryOpen()
        {
            if (gameObject.activeSelf)
            {
                return;
            }
            
            gameObject.SetActive(true);
        }

        private void Awake()
        {
            CheckReferences();

            Initialize();
            
            SignalBus.Subscribe<T>(HandleEvent);
        }

        private void OnDestroy()
        {
            Dispose();
            
            SignalBus.Unsubscribe<T>(HandleEvent);
        }

        private void HandleEvent(T eventParams)
        {
            TryOpen();
            OnHandleEvent(eventParams);
        }
    }
}