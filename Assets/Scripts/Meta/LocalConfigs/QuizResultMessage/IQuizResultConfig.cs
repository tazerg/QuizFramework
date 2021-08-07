namespace QuizFramework.LocalConfigs
{
    public interface IQuizResultConfig
    {
        string GetResultMessage(float resultRatio);
        bool IsGroupPassed(float resultRatio);
    }
}