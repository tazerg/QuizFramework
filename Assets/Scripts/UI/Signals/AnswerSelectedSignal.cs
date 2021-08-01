using System;

namespace QuizFramework.UI.Signals
{
    public struct AnswerSelectedSignal
    {
        public byte AnswerIndex { get; }
        public Action<bool> SelectAnswerCallback { get; }

        public AnswerSelectedSignal(byte answerIndex, Action<bool> selectAnswerCallback)
        {
            AnswerIndex = answerIndex;
            SelectAnswerCallback = selectAnswerCallback;
        }
    }
}