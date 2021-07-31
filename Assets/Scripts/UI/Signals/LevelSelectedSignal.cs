namespace QuizFramework.UI.Signals
{
    public struct LevelSelectedSignal
    {
        private ushort SelectedLevel { get; }

        public LevelSelectedSignal(ushort selectedLevel)
        {
            SelectedLevel = selectedLevel;
        }
    }
}