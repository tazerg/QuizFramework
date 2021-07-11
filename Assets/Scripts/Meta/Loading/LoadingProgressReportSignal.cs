namespace QuizFramework.Loading
{
    public struct LoadingProgressReportSignal
    {
        public float Progress { get; }
        public string Status { get; }

        public LoadingProgressReportSignal(float progress, string status)
        {
            Progress = progress;
            Status = status;
        }
    }
}