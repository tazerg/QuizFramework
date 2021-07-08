using QuizFramework.Advertisement;

namespace QuizFramework.SignalBus
{
    public struct ShowAdResultReceived
    {
        public AdShowResult AdShowResult;

        public ShowAdResultReceived(AdShowResult result)
        {
            AdShowResult = result;
        }
    }
}