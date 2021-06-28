using System.Threading.Tasks;
using QuizFramework.Database;
using QuizFramework.Extensions;
using QuizFramework.LocalConfig;
using QuizFramework.Storage;
using QuizFramework.UI;
using QuizFramework.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace QuizFramework.Loading
{
    public class MetaLoader : MonoBehaviour
    {
        private LoadingVM _loadingVm;
        private DiContainer _projectContainer;
        
        private IVersionChecker _versionChecker;
        private ILocalConfig _localConfig;
        private IQuestionDatabaseLoader _questionDatabaseLoader;
        private ILocalStorage _localStorage;

        private DiContainer ProjectContainer
        {
            get
            {
                if (_projectContainer == null)
                    _projectContainer = ProjectContext.Instance.Container;

                return _projectContainer;
            }
        }
        
        [Inject]
        public void Inject(LoadingVM loadingVm, IVersionChecker versionChecker, ILocalConfig localConfig, 
            IQuestionDatabaseLoader questionDatabaseLoader, ILocalStorage localStorage)
        {
            _loadingVm = loadingVm;
            
            _versionChecker = versionChecker;
            _localConfig = localConfig;
            _questionDatabaseLoader = questionDatabaseLoader;
            _localStorage = localStorage;
        }

        private async void Start()
        {
            await LoadFlow();
        }

        private async Task LoadFlow()
        {
            _loadingVm.Progressbar.Report(0f);
            _loadingVm.SetProgressStatus("Проверка обновлений...");

            var isCorrectVersion = await _versionChecker.IsCorrectVersion(_localConfig.QuestionsSheetId, _localConfig.VersionControlTabId);

            await LoadQuestions(isCorrectVersion);

            _loadingVm.Progressbar.Report(0.8f);
            _loadingVm.SetProgressStatus("Загружаем игру...");
            
            await SceneManager.LoadSceneAsync("Scenes/Main");
        }

        private async Task LoadQuestions(bool isCorrectVersion)
        {
            if (isCorrectVersion)
            {
                _loadingVm.Progressbar.Report(0.4f);
                _loadingVm.SetProgressStatus("Загружаем вопросы...");
                
                var questionsJson = _localStorage.GetLocalQuestions();
                var localQuestionDatabase = _questionDatabaseLoader.LoadFromLocal(questionsJson);
                
                ProjectContainer.BindInstance(localQuestionDatabase).AsSingle();
                return;
            }

            _loadingVm.Progressbar.Report(0.2f);
            _loadingVm.SetProgressStatus("Скачиваем обновление...");
            
            var remoteQuestionDatabase = await _questionDatabaseLoader.LoadFromRemote(_localConfig.QuestionsSheetId,
                _localConfig.QuestionsTabId, _localConfig.AnswersStartIndex, _localConfig.AnswersEndIndex);
            
            ProjectContainer.BindInstance(remoteQuestionDatabase).AsSingle();
            
            _loadingVm.Progressbar.Report(0.4f);
            _loadingVm.SetProgressStatus("Загружаем вопросы...");
            
            _localStorage.SaveVersionToLocal(_versionChecker.RemoteVersion.Value);
            _localStorage.SaveQuestionsToLocal(remoteQuestionDatabase);
        }
    }
}