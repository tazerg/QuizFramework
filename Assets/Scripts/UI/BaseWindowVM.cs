using QuizFramework.SignalBus;
using UnityEngine;
using Zenject;

namespace QuizFramework.UI
{
    public abstract class BaseWindowVM<T> : MonoBehaviour, IWindow where T : struct
    {
        [Inject] protected ISignalBus SignalBus { get; private set; }

        protected void Close()
        {
            gameObject.SetActive(false);
        }

        protected abstract void OnInitialize();
        protected abstract void OnDispose();
        protected abstract void CheckReferences();
        protected abstract void OnHandleEvent(T eventParams);
        
        private void TryOpen()
        {
            if (gameObject.activeSelf)
            {
                return;
            }
            
            gameObject.SetActive(true);
        }

        private void Initialize()
        {
            CheckReferences();

            OnInitialize();
            
            SignalBus.Subscribe<T>(HandleEvent);
        }

        private void OnDestroy()
        {
            OnDispose();
            
            SignalBus.Unsubscribe<T>(HandleEvent);
        }

        private void HandleEvent(T eventParams)
        {
            TryOpen();
            OnHandleEvent(eventParams);
        }

        #region IWindow

        void IWindow.TryOpen()
        {
            TryOpen();
        }
        
        void IWindow.Close()
        {
            Close();
        }

        #endregion

        #region IInitializable
        
        void IInitializable.Initialize()
        {
            Initialize();
        }

        #endregion
    }
}