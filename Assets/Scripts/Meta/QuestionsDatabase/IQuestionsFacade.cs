namespace QuizFramework.Database
{
    public interface IQuestionsFacade
    {
        bool HasNextGroup(ushort currentGroup);
    }
}