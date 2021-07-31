using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuizFramework.Database
{
    public class QuestionDatabase : IQuestionDatabase
    {
        [JsonProperty("questions")] private readonly IEnumerable<Question> _allQuestions;
        
        [JsonIgnore] private IDictionary<ushort, List<Question>> _questionGroups;
        
        public QuestionDatabase(IEnumerable<Question> questions)
        {
            _allQuestions = questions;
            FillQuestionGroupsDict();
        }

        public void FillQuestionGroupsDict()
        {
            _questionGroups = new Dictionary<ushort, List<Question>>();
            foreach (var question in _allQuestions)
            {
                var questionGroup = question.QuestionsGroup;
                if (!_questionGroups.TryGetValue(questionGroup, out var groupList))
                {
                    groupList = new List<Question>();
                    _questionGroups.Add(questionGroup, groupList);
                }
                
                groupList.Add(question);
            }
        }

        private ushort GetQuestionGroupCount()
        {
            return (ushort) _questionGroups.Count;
        }

        private IEnumerable<Question> GetAllGroupQuestion(ushort group)
        {
            if (!_questionGroups.TryGetValue(group, out var questions))
            {
                return new List<Question>();
            }

            return questions;
        }

        #region IQuestionDatabase

        ushort IQuestionDatabase.GetQuestionGroupCount()
        {
            return GetQuestionGroupCount();
        }

        IEnumerable<Question> IQuestionDatabase.GetAllGroupQuestion(ushort group)
        {
            return GetAllGroupQuestion(group);
        }

        #endregion
    }
}