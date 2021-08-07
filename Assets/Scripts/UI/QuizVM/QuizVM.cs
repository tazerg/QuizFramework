using System.Collections.Generic;
using System.Threading.Tasks;
using QuizFramework.Advertisement;
using QuizFramework.Database;
using QuizFramework.Quiz;
using QuizFramework.UI.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace QuizFramework.UI
{
    public class QuizVM : BaseWindowVM<LevelSelectedSignal>
    {
        private const int ShowAnswerResultDelay = 2000;
        
        [Inject] private readonly IQuizController _quizController;
        [Inject] private readonly IAdsService _adsService;

        [SerializeField] private Text _questionText;
        [SerializeField] private Text _groupProgressText;
        [SerializeField] private List<AnswerButton> _answerButtons;

        private Question _currentQuestion;
        private int _correctAnswersCount;
        private int _currentProgress;
        private int _groupQuestionsCount;
        
        protected override void OnInitialize()
        {
            SignalBus.Subscribe<AnswerSelectedSignal>(OnAnswerSelected);
        }

        protected override void OnDispose()
        {
            SignalBus.Unsubscribe<AnswerSelectedSignal>(OnAnswerSelected);
        }

        protected override void CheckReferences()
        {
            if (_questionText == null)
                Debug.LogError("Question text not set!");
            
            if (_groupProgressText == null)
                Debug.LogError("Group progress text not set!");
            
            if (_answerButtons == null || _answerButtons.Count == 0)
                Debug.LogError("Answer buttons not set!");
        }

        protected override void OnHandleEvent(LevelSelectedSignal eventParams)
        {
            var quizGroup = eventParams.SelectedLevel;
            if (_quizController.InitializeQuestionGroup(quizGroup))
            {
                _currentProgress = 0;
                _groupQuestionsCount = _quizController.QuestionsCount;
                TryShowNextQuestion();
                return;
            }
            
            Debug.LogError($"Can't initialize quiz controller for group {quizGroup}");
            Close();
            SignalBus.Fire(new OpenSelectLevelSignal());
        }

        private async void OnAnswerSelected(AnswerSelectedSignal signal)
        {
            DisableInteractableForAllAnswerButtons();
            
            var isAnswerCorrect = signal.AnswerIndex == _currentQuestion.IndexOfCorrectAnswer;
            signal.SelectAnswerCallback.Invoke(isAnswerCorrect);
            _correctAnswersCount += isAnswerCorrect ? 1 : 0;
            _currentProgress += 1;

            await Task.Delay(ShowAnswerResultDelay);
            
            TryShowNextQuestion();
        }

        private void DisableInteractableForAllAnswerButtons()
        {
            foreach (var answerButton in _answerButtons)
            {
                answerButton.SetInteractable(false);
            }
        }

        private async void TryShowNextQuestion()
        {
            if (!_quizController.HasNextQuestion)
            {
                SignalBus.Fire(new OpenLoadingSignal());
                
                await TryShowAd();
                
                SignalBus.Fire(new OpenQuizResultSignal(_currentQuestion.QuestionsGroup, _correctAnswersCount,
                    _groupQuestionsCount));
                SignalBus.Fire(new CloseLoadingSignal());
                
                Close();
                return;
            }

            var currentQuestion = _quizController.GetNextQuestion();
            if (currentQuestion == null)
            {
                Debug.LogError("Current question is null!");
                return;
            }
            
            _currentQuestion = currentQuestion.Value;
            ShowQuestion();
            ShowAnswers();
            UpdateProgress();
        }

        private async Task TryShowAd()
        {
            if (!_adsService.IsReady(AdPlacement.Rewarded))
            {
                return;
            }

            var adResult = await _adsService.ShowAd(AdPlacement.Rewarded);
            Debug.Log(adResult.ToString());
        }

        private void ShowQuestion()
        {
            _questionText.text = _currentQuestion.QuestionStr;
        }

        private void ShowAnswers()
        {
            var questionAnswers = _currentQuestion.Answers;
            for (var i = 0; i < _answerButtons.Count; i++)
            {
                var answerButton = _answerButtons[i];
                if (questionAnswers.Count <= i)
                {
                    answerButton.SetVisible(false);
                    continue;
                }

                answerButton.SetVisible(true);
                answerButton.SetInteractable(true);
                var answer = questionAnswers[i];
                answerButton.SetText(answer);
            }
        }

        private void UpdateProgress()
        {
            _groupProgressText.text = $"{_currentProgress}/{_groupQuestionsCount}";
        }
    }
}