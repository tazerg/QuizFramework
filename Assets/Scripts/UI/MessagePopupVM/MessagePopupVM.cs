using QuizFramework.UI.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace QuizFramework.UI
{
    public class MessagePopupVM : BaseWindowVM<ShowMessagePopupSignal>
    {
        [SerializeField] private Text _messageText;
        [SerializeField] private Button _closeButton;
        
        protected override void OnInitialize()
        {
            _closeButton.onClick.AddListener(Close);
        }

        protected override void OnDispose()
        {
            _closeButton.onClick.RemoveListener(Close);
        }

        protected override void CheckReferences()
        {
            if (_messageText == null) 
                Debug.LogError("Message text not set!");
            
            if (_closeButton == null)
                Debug.LogError("Close button not set!");
        }

        protected override void OnHandleEvent(ShowMessagePopupSignal eventParams)
        {
            _messageText.text = eventParams.Message;
        }
    }
}