using QuizFramework.InApps;
using QuizFramework.UI.Signals;

namespace QuizFramework.UI
{
    public class SupportButton : BaseQuizButton
    {
        private InAppInfo _info;

        public void Setup(InAppInfo info)
        {
            _info = info;

            Text.text = _info.Name;
        }
        
        protected override void OnAwake()
        {
        }

        protected override void OnButtonClicked()
        {
            SignalBus.Fire(new SupportProductSelected(_info.Id, _info.Name));
        }
    }
}