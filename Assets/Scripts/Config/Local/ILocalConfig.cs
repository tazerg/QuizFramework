namespace QuizFramework.LocalConfig
{
    public interface ILocalConfig
    {
        string QuestionsSheetId { get; }
        string QuestionsTabId { get; }
        string VersionControlTabId { get; }        
        byte AnswersStartIndex { get; }
        byte AnswersEndIndex { get; }
    }
}