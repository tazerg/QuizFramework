using QuizFramework.Analytics;
using QuizFramework.Database;
using QuizFramework.LocalConfigs;
using QuizFramework.Storage;
using QuizFramework.UI.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace QuizFramework.UI
{
    public class QuizResultVM : BaseWindowVM<OpenQuizResultSignal>
    {
        private const string ResultMessagePattern = "Твой результат: {0} из {1}. {2}";
        private const int ShowRateUsNeededLevel = 5;

        [Inject] private readonly IQuizAnalyticsStrategy _quizAnalytics;
        [Inject] private readonly IQuizResultConfig _quizResultConfig;
        [Inject] private readonly IQuestionsFacade _questionsFacade;
        [Inject] private readonly ILocalStorage _localStorage;
        
        [SerializeField] private Text _resultText;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _backToMenuButton;

        private ushort _passedGroup;
        private bool _hasNextGroup;
        
        protected override void OnInitialize()
        {
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _retryButton.onClick.AddListener(OnRetryButtonClicked);
            _backToMenuButton.onClick.AddListener(OnBackToMenuButtonCLicked);
        }

        protected override void OnDispose()
        {
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
            _retryButton.onClick.RemoveListener(OnRetryButtonClicked);
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonCLicked);
        }

        protected override void CheckReferences()
        {
            if (_resultText == null)
                Debug.LogError("Result text not set!");
            
            if (_continueButton == null)
                Debug.LogError("Continue button not set!");
            
            if (_retryButton == null)
                Debug.LogError("Retry button not set!");
            
            if (_backToMenuButton == null)
                Debug.LogError("Back to menu button");
        }

        protected override void OnHandleEvent(OpenQuizResultSignal eventParams)
        {
            _passedGroup = eventParams.CurrentGroup;
            _hasNextGroup = _questionsFacade.HasNextGroup(_passedGroup);
            
            var correctAnswersCount = eventParams.CorrectAnswersCount;
            var allQuestionsCount = eventParams.AllQuestionsCount;
            var resultRatio = (float) correctAnswersCount / allQuestionsCount;
            ShowResultMessage(correctAnswersCount, allQuestionsCount, resultRatio);

            var isGroupPassed = _quizResultConfig.IsGroupPassed(resultRatio);
            var maxPassedGroup = _localStorage.GetMaxPassedLevel();
            var hasOldestPassedGroup = maxPassedGroup >= _passedGroup;
            _continueButton.gameObject.SetActive(_hasNextGroup && (isGroupPassed || hasOldestPassedGroup));

            _quizAnalytics.QuestionsGroupPassedEvent(_passedGroup, maxPassedGroup, correctAnswersCount, allQuestionsCount, isGroupPassed);
            TrySavePassedGroup(isGroupPassed, hasOldestPassedGroup);
            TryShowRateUsPopup();
        }

        private void ShowResultMessage(int correctAnswersCount, int allQuestionsCount, float resultRatio)
        {
            var resultMessage = _quizResultConfig.GetResultMessage(resultRatio);
            _resultText.text = string.Format(ResultMessagePattern, correctAnswersCount, allQuestionsCount, resultMessage);
        }

        private void TrySavePassedGroup(bool isGroupPassed, bool hasOldestPassedGroup)
        {
            if (!isGroupPassed)
            {
                return;
            }

            if (hasOldestPassedGroup)
            {
                return;
            }
            
            _localStorage.SaveMaxPassedLevel(_passedGroup);
        }

        private void TryShowRateUsPopup()
        {
            if (_passedGroup != ShowRateUsNeededLevel)
            {
                return;
            }

            var maxPassedLevel = _localStorage.GetMaxPassedLevel();
            if (maxPassedLevel != ShowRateUsNeededLevel)
            {
                return;
            }
            
            SignalBus.Fire(new ShowRateUsPopupSignal());
        }

        private void OnContinueButtonClicked()
        {
            OpenQuizVM((ushort) (_passedGroup + 1));
        }

        private void OnRetryButtonClicked()
        {
            OpenQuizVM(_passedGroup);
        }
        
        private void OnBackToMenuButtonCLicked()
        {
            SignalBus.Fire(new OpenMainMenuSignal());
            Close();
        }

        private void OpenQuizVM(ushort selectedLevel)
        {
            SignalBus.Fire(new LevelSelectedSignal(selectedLevel));
            Close();
        }
    }
}