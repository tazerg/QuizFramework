namespace QuizFramework.UI.Signals
{
    public struct LevelSelectedSignal
    {
        public ushort SelectedLevel { get; }

        public LevelSelectedSignal(ushort selectedLevel)
        {
            SelectedLevel = selectedLevel;
        }
    }
}