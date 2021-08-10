using System.Threading.Tasks;
using QuizFramework.Analytics;
using QuizFramework.Database;
using QuizFramework.Extensions;
using QuizFramework.LocalConfig;
using QuizFramework.SignalBus;
using QuizFramework.Storage;
using QuizFramework.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace QuizFramework.Loading
{
    public class MetaLoader : MonoBehaviour
    {
        private const int FirstVersion = 1;

        [Inject] private ISignalBus _signalBus;
        [Inject] private IVersionChecker _versionChecker;
        [Inject] private IQuestionDownloaderConfig _questionDownloaderConfig;
        [Inject] private IQuestionDatabaseLoader _questionDatabaseLoader;
        [Inject] private ILocalStorage _localStorage;
        [Inject] private ILocalQuestionsProvider _localQuestionsProvider;
        [Inject] private IPlayerAnalyticsStrategy _playerAnalytics;
        
        private DiContainer _projectContainer;
        private DiContainer ProjectContainer
        {
            get
            {
                if (_projectContainer == null)
                    _projectContainer = ProjectContext.Instance.Container;

                return _projectContainer;
            }
        }

        private async void Start()
        {
            await LoadMeta();
            await SceneManager.LoadSceneAsync("Scenes/Main");
        }

        private async Task LoadMeta()
        {
            if (!_localStorage.HasLocalVersion())
            {
                await FirstLoadFlow();
                return;
            }
            
            await LoadFlow();
        }

        private async Task FirstLoadFlow()
        {
            var localStoredQuestions = _localQuestionsProvider.GetLocalQuestions();

            var questionDatabase = await _questionDatabaseLoader.LoadFromLocal(localStoredQuestions);
            ProjectContainer.BindInstance(questionDatabase).AsSingle();
            
            _localStorage.SaveVersion(FirstVersion);
            _localStorage.SaveQuestions(questionDatabase);
        }

        private async Task LoadFlow()
        {
            _signalBus.Fire(new LoadingProgressReportSignal(0f, "Проверка обновлений..."));

            var isCorrectVersion = await _versionChecker.IsCorrectVersion(_questionDownloaderConfig.QuestionsSheetId, _questionDownloaderConfig.VersionControlTabId);

            await LoadQuestions(isCorrectVersion);

            _signalBus.Fire(new LoadingProgressReportSignal(0.8f, "Загружаем игру..."));
        }

        private async Task LoadQuestions(bool isCorrectVersion)
        {
            if (isCorrectVersion)
            {
                _signalBus.Fire(new LoadingProgressReportSignal(0.4f, "Загружаем вопросы..."));
                
                var questionsJson = _localStorage.GetLocalQuestions();
                var localQuestionDatabase = await _questionDatabaseLoader.LoadFromLocal(questionsJson);
                
                ProjectContainer.BindInstance(localQuestionDatabase).AsSingle();
                return;
            }

            _signalBus.Fire(new LoadingProgressReportSignal(0.2f, "Скачиваем обновление..."));

            var remoteQuestionDatabase = await _questionDatabaseLoader.LoadFromRemote(_questionDownloaderConfig.QuestionsSheetId,
                _questionDownloaderConfig.QuestionsTabId, _questionDownloaderConfig.AnswersStartIndex, _questionDownloaderConfig.AnswersEndIndex);
            
            ProjectContainer.BindInstance(remoteQuestionDatabase).AsSingle();
            
            _signalBus.Fire(new LoadingProgressReportSignal(0.4f, "Загружаем вопросы..."));
            
            var oldDatabaseVersion = _localStorage.GetLocalVersion();
            _localStorage.SaveVersion(_versionChecker.RemoteVersion.Value);
            _localStorage.SaveQuestions(remoteQuestionDatabase);
            
            var newDatabaseVersion = _localStorage.GetLocalVersion();
            _playerAnalytics.QuizDatabaseVersionUpdatedEvent(oldDatabaseVersion, newDatabaseVersion);
        }
    }
}