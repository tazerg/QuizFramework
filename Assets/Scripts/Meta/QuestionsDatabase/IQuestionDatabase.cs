using System.Collections.Generic;

namespace QuizFramework.Database
{
    public interface IQuestionDatabase
    {
        IEnumerable<Question> GetAllGroupQuestion(ushort group);
    }
}