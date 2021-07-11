using Zenject;

namespace QuizFramework.UI
{
    public interface IWindow : IInitializable
    {
        void TryOpen();
        void Close();
    }
}