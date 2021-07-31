using System.Collections.Generic;

namespace QuizFramework.Database
{
    public interface IQuestionDatabase
    {
        ushort GetQuestionGroupCount();
        IEnumerable<Question> GetAllGroupQuestion(ushort group);
    }
}