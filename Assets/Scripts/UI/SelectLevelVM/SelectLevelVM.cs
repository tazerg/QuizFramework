using System.Collections.Generic;
using System.Threading.Tasks;
using QuizFramework.Analytics;
using QuizFramework.Database;
using QuizFramework.Storage;
using QuizFramework.UI.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace QuizFramework.UI
{
    public class SelectLevelVM : BaseWindowVM<OpenSelectLevelSignal>
    {
        private const int RebuildLayoutMillisecondsDelay = 50;
        
        [Inject] private readonly IQuizAnalyticsStrategy _quizAnalyticsStrategy;
        [Inject] private readonly IQuestionDatabase _questionDatabase;
        [Inject] private readonly ILocalStorage _localStorage;
        [Inject] private readonly DiContainer _diContainer;

        [Header("Prefabs")]
        [SerializeField] private LevelButton _levelButtonPrefab;

        [Header("Inner components")] 
        [SerializeField] private GridLayoutGroup _levelButtonsParent;
        [SerializeField] private RectTransform _transformForRebuild;

        [Header("Inner ui elements")] 
        [SerializeField] private Button _backButton;

        private readonly IList<LevelButton> _spawnedLevelButtons = new List<LevelButton>();

        private bool _isLayoutRebuilded;
        
        protected override void OnInitialize()
        {
            _isLayoutRebuilded = false;
            SpawnLevelButtons();
            
            _backButton.onClick.AddListener(OnBackButtonClicked);
            
            SignalBus.Subscribe<LevelSelectedSignal>(OnLevelSelected);
        }

        protected override void OnDispose()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            
            SignalBus.Unsubscribe<LevelSelectedSignal>(OnLevelSelected);
        }

        protected override void CheckReferences()
        {
            if (_levelButtonPrefab == null)
                Debug.LogError("Level button prefab not set!");
            
            if (_levelButtonsParent == null)
                Debug.LogError("Level buttons parent not set!");
            
            if (_transformForRebuild == null)
                Debug.LogError("Transform for rebuild not set!");
            
            if (_backButton == null)
                Debug.LogError("Back button not set!");
        }

        protected override void OnHandleEvent(OpenSelectLevelSignal eventParams)
        {
            TryRebuildLayout();
            
            var maxPassedLevel = _localStorage.GetMaxPassedLevel();
            for (var i = 0; i < _spawnedLevelButtons.Count; i++)
            {
                var levelButton = _spawnedLevelButtons[i];
                var currentLevel = i + 1;
                var isAvailable = currentLevel <= maxPassedLevel + 1;
                var isPassed = currentLevel <= maxPassedLevel;
                levelButton.Setup(isAvailable, isPassed);
            }
        }

        private void SpawnLevelButtons()
        {
            var questionsGroupCount = _questionDatabase.GetQuestionGroupCount();
            
            for (var i = 0; i < questionsGroupCount; i++)
            {
                var newLevelButton = Instantiate(_levelButtonPrefab, _levelButtonsParent.transform);
                _diContainer.Inject(newLevelButton);
                newLevelButton.Initialize();
                var currentLevel = (ushort) (i + 1);
                newLevelButton.SetLevel(currentLevel);
                
                _spawnedLevelButtons.Add(newLevelButton);
            }
        }

        private async void TryRebuildLayout()
        {
            if (_isLayoutRebuilded)
            {
                return;
            }

            _levelButtonsParent.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_transformForRebuild);

            await Task.Delay(RebuildLayoutMillisecondsDelay);

            _levelButtonsParent.enabled = false;
            _isLayoutRebuilded = true;
        }

        private void OnLevelSelected(LevelSelectedSignal signal)
        {
            _quizAnalyticsStrategy.QuestionsGroupSelectedEvent(signal.SelectedLevel);
            Close();
        }

        private void OnBackButtonClicked()
        {
            SignalBus.Fire(new OpenMainMenuSignal());
            Close();
        }
    }
}