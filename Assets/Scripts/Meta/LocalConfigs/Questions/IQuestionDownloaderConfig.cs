namespace QuizFramework.LocalConfig
{
    public interface IQuestionDownloaderConfig
    {
        string QuestionsSheetId { get; }
        string QuestionsTabId { get; }
        string VersionControlTabId { get; }        
        byte AnswersStartIndex { get; }
        byte AnswersEndIndex { get; }
    }
}